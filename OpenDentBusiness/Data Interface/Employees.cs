using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Employees{
		#region Update
		///<summary>Will throw exception if the employee has no name.</summary>
		public static void Update(Employee employee) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),employee);
				return;
			}
			if(employee.LName=="" && employee.FName=="") {
				throw new ApplicationException(Lans.g("FormEmployeeEdit","Must include either first name or last name"));
			}
			Crud.EmployeeCrud.Update(employee);
		}

		///<summary>Will throw exception if the employee has no name.</summary>
		public static void UpdateChanged(Employee employee,Employee employeeOld, bool doInvalidate=false) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),employee,employeeOld, doInvalidate);
				return;
			}
			if(employee.LName=="" && employee.FName=="") {
				throw new ApplicationException(Lans.g("FormEmployeeEdit","Must include either first name or last name"));
			}
			if(Crud.EmployeeCrud.Update(employee,employeeOld) && doInvalidate) {
				Signalods.SetInvalid(InvalidType.Employees);
			}
		}

		///<summary>Updates the employee's ClockStatus if necessary based on their clock events. This method handles future clock events as having
		///already occurred. Ex: If I clock out for home at 6:00 but edit my time card to say 7:00, at 6:30 my status will say Home.</summary>
		public static void UpdateClockStatus(long employeeNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),employeeNum);
				return;
			}
			//Get the last clockevent for the employee. Will include clockevent with "in" before NOW, and "out" anytime before 23:59:59 of TODAY.
			string command = @"SELECT * FROM clockevent 
				WHERE TimeDisplayed2<="+DbHelper.DateAddSecond(DbHelper.DateAddDay(DbHelper.Curdate(),"1"),"-1")+" AND TimeDisplayed1<="+DbHelper.Now()+@"
				AND EmployeeNum="+POut.Long(employeeNum)+@"
				ORDER BY IF(YEAR(TimeDisplayed2) < 1880,TimeDisplayed1,TimeDisplayed2) DESC";
			command=DbHelper.LimitOrderBy(command,1);
			ClockEvent clockEvent=Crud.ClockEventCrud.SelectOne(command);
			Employee employee=GetEmpFromDB(employeeNum);
			Employee employeeOld=employee.Copy();
			if(clockEvent!=null && clockEvent.TimeDisplayed2>DateTime.Now) {//Future time manual clock out.
				employee.ClockStatus=Lans.g("ContrStaff","Manual Entry");
			}
			else if(clockEvent==null //Employee has never clocked in
				|| (clockEvent.TimeDisplayed2.Year > 1880 && clockEvent.ClockStatus==TimeClockStatus.Home))//Clocked out for home
			{
				employee.ClockStatus=Lans.g("enumTimeClockStatus",TimeClockStatus.Home.ToString());
			}
			else if(clockEvent.TimeDisplayed2.Year > 1880 && clockEvent.ClockStatus==TimeClockStatus.Lunch) {//Clocked out for lunch
				employee.ClockStatus=Lans.g("enumTimeClockStatus",TimeClockStatus.Lunch.ToString());
			}
			else if(clockEvent.TimeDisplayed1.Year > 1880 && clockEvent.TimeDisplayed2.Year < 1880 && clockEvent.ClockStatus==TimeClockStatus.Break) {
				employee.ClockStatus=Lans.g("enumTimeClockStatus",TimeClockStatus.Break.ToString());
			}
			else if(clockEvent.TimeDisplayed2.Year > 1880 && clockEvent.ClockStatus==TimeClockStatus.Break) {//Clocked back in from break
				employee.ClockStatus=Lans.g("ContrStaff","Working");
			}
			else {//The employee has not clocked out yet.
				employee.ClockStatus=Lans.g("ContrStaff","Working");
			}
			UpdateChanged(employee,employeeOld,true);
			RefreshCache(); //Need to refresh manually since the employee could potentially edit their clock events faster then the cache refresh in UpdateChaged().
		}
		#endregion

		#region CachePattern
		private class EmployeeCache : CacheListAbs<Employee> {
			protected override List<Employee> GetCacheFromDb() {
				string command="SELECT * FROM employee ORDER BY IsHidden,FName,LName";
				return Crud.EmployeeCrud.SelectMany(command);
			}
			protected override List<Employee> TableToList(DataTable table) {
				return Crud.EmployeeCrud.TableToList(table);
			}
			protected override Employee Copy(Employee employee) {
				return employee.Copy();
			}
			protected override DataTable ListToTable(List<Employee> listEmployees) {
				return Crud.EmployeeCrud.ListToTable(listEmployees,"Employee");
			}
			protected override void FillCacheIfNeeded() {
				Employees.GetTableFromCache(false);
			}
			protected override bool IsInListShort(Employee employee) {
				return !employee.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EmployeeCache _employeeCache=new EmployeeCache();

		public static Employee GetFirstOrDefault(Func<Employee,bool> match,bool isShort=false) {
			return _employeeCache.GetFirstOrDefault(match,isShort);
		}

		public static List<Employee> GetDeepCopy(bool isShort=false) {
			return _employeeCache.GetDeepCopy(isShort);
		}

		public static List<Employee> GetWhere(Predicate<Employee> match,bool isShort = false) {
			return _employeeCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_employeeCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_employeeCache.FillCacheFromTable(table);
				return table;
			}
			return _employeeCache.GetTableFromCache(doRefreshCache);
		}

		public static void ClearCache() {
			_employeeCache.ClearCache();
		}
		#endregion

		///<summary>Instead of using the cache, which sorts by FName, LName.</summary>
		public static List<Employee> GetForTimeCard() {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<Employee>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM employee WHERE IsHidden=0 ORDER BY LName,Fname";
			return Crud.EmployeeCrud.SelectMany(command);
		}

		/*public static Employee[] GetListByExtension(){
			if(ListShort==null){
				return new Employee[0];
			}
			Employee[] arrayCopy=new Employee[ListShort.Length];
			ListShort.CopyTo(arrayCopy,0);
			int[] arrayKeys=new int[ListShort.Length];
			for(int i=0;i<ListShort.Length;i++){
				arrayKeys[i]=ListShort[i].PhoneExt;
			}
			Array.Sort(arrayKeys,arrayCopy);
			//List<Employee> retVal=new List<Employee>(ListShort);
			//retVal.Sort(
			return arrayCopy;
		}*/

		///<summary></summary>
		public static long Insert(Employee employee){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				employee.EmployeeNum=Meth.GetLong(MethodBase.GetCurrentMethod(),employee);
				return employee.EmployeeNum;
			}
			if(employee.LName=="" && employee.FName=="") {
				throw new ApplicationException(Lans.g("FormEmployeeEdit","Must include either first name or last name"));
			}
			return Crud.EmployeeCrud.Insert(employee);
		}

		///<summary>Surround with try-catch</summary>
		public static void Delete(long employeeNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),employeeNum);
				return;
			}
			//appointment.Assistant will not block deletion
			//schedule.EmployeeNum will not block deletion
			string command="SELECT COUNT(*) FROM clockevent WHERE EmployeeNum="+POut.Long(employeeNum);
			if(Db.GetCount(command)!="0"){
				throw new ApplicationException(Lans.g("FormEmployeeSelect",
					"Not allowed to delete employee because of attached clock events."));
			}
			command="SELECT COUNT(*) FROM timeadjust WHERE EmployeeNum="+POut.Long(employeeNum);
			if(Db.GetCount(command)!="0") {
				throw new ApplicationException(Lans.g("FormEmployeeSelect",
					"Not allowed to delete employee because of attached time adjustments."));
			}
			command="SELECT COUNT(*) FROM userod WHERE EmployeeNum="+POut.Long(employeeNum);
			if(Db.GetCount(command)!="0") {
				throw new ApplicationException(Lans.g("FormEmployeeSelect",
					"Not allowed to delete employee because of attached user."));
			}
			command="UPDATE appointment SET Assistant=0 WHERE Assistant="+POut.Long(employeeNum);
			Db.NonQ(command);
			command="SELECT ScheduleNum FROM schedule WHERE EmployeeNum="+POut.Long(employeeNum);
			DataTable table=Db.GetTable(command);
			List<string> listScheduleNums=new List<string>();//Used for deleting scheduleops below
			for(int i=0;i<table.Rows.Count;i++) {
				listScheduleNums.Add(table.Rows[i]["ScheduleNum"].ToString());
			}
			if(listScheduleNums.Count>0) {
				command="DELETE FROM scheduleop WHERE ScheduleNum IN("+POut.String(String.Join(",",listScheduleNums))+")";
				Db.NonQ(command);
			}
			//command="DELETE FROM scheduleop WHERE ScheduleNum IN(SELECT ScheduleNum FROM schedule WHERE EmployeeNum="+POut.Long(employeeNum)+")";
			//Db.NonQ(command);
			command="DELETE FROM schedule WHERE EmployeeNum="+POut.Long(employeeNum);
			Db.NonQ(command);
			command= "DELETE FROM employee WHERE EmployeeNum ="+POut.Long(employeeNum);
			Db.NonQ(command);
			command="DELETE FROM timecardrule WHERE EmployeeNum="+POut.Long(employeeNum);
			Db.NonQ(command);
		}

		/*
		///<summary>Returns LName,FName MiddleI for the provided employee.</summary>
		public static string GetNameLF(Employee emp){
			return(emp.LName+", "+emp.FName+" "+emp.MiddleI);
		}

		///<summary>Loops through List to find matching employee, and returns LName,FName MiddleI.</summary>
		public static string GetNameLF(int employeeNum){
			for(int i=0;i<ListLong.Length;i++){
				if(ListLong[i].EmployeeNum==employeeNum){
					return GetNameLF(ListLong[i]);
				}
			}
			return "";
		}*/

		///<summary>Returns FName MiddleI LName for the provided employee.</summary>
		public static string GetNameFL(Employee employee) {
			Meth.NoCheckMiddleTierRole();
			return (employee.FName+" "+employee.MiddleI+" "+employee.LName);
		}

		///<summary>Returns FNameL, where L is the first letter of the last name.</summary>
		public static string GetNameCondensed(Employee employee) {
			Meth.NoCheckMiddleTierRole();
			string retVal=employee.FName;
			if(employee.LName.Length>0){
				retVal+=employee.LName.Substring(0,1);
			}
			return retVal;
		}

		///<summary>Loops through List to find matching employee, and returns FName MiddleI LName.</summary>
		public static string GetNameFL(long employeeNum) {
			Meth.NoCheckMiddleTierRole();
			Employee employee=GetFirstOrDefault(x => x.EmployeeNum==employeeNum);
			//if(isCondensed){
			//	return (employee==null ? "" : GetNameCondensed(employee));
			//}
			if(employee==null){
				return "";
			}
			return GetNameFL(employee);
		}

		///<summary>Loops through List to find matching employee, and returns first 2 letters of first name.  Will later be improved with abbr field.</summary>
		public static string GetAbbr(long employeeNum) {
			Meth.NoCheckMiddleTierRole();
			string retVal="";
			Employee employee=GetFirstOrDefault(x => x.EmployeeNum==employeeNum);
			if(employee==null) {
				return retVal;
			}
			retVal=employee.FName;
			if(retVal.Length > 2) {
				retVal=retVal.Substring(0,2);
			}
			return retVal;
		}
		///<summary>Get a single employee from the database. Will return null if not found.</summary>
		public static Employee GetEmpFromDB(long employeeNum) {
      if(employeeNum==0) {
				return null;
      }
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<Employee>(MethodBase.GetCurrentMethod(),employeeNum);
			} 
			return Crud.EmployeeCrud.SelectOne(employeeNum); //might return null
    }

		///<summary>From cache</summary>
		public static Employee GetEmp(long employeeNum) {
			Meth.NoCheckMiddleTierRole();
			return GetFirstOrDefault(x => x.EmployeeNum==employeeNum);
		}

		///<summary>Find formatted name in list.  Takes in a name that was previously formatted by Employees.GetNameFL and finds a match in the list.  If no match is found then returns null.</summary>
		public static Employee GetEmp(string nameFL,List<Employee> listEmployees) {
			Meth.NoCheckMiddleTierRole();
			for(int i=0;i<listEmployees.Count;i++) {
				if(GetNameFL(listEmployees[i])==nameFL) {
					return listEmployees[i];
				}
			}
			return null;
		}
		
		///<summary>From cache</summary>
		public static List<Employee> GetEmps(List<long> listEmployeeNums) {
			Meth.NoCheckMiddleTierRole();
			return GetWhere(x => listEmployeeNums.Contains(x.EmployeeNum));
		}

		///<summary>Gets all employees associated to users that have a clinic set to the clinic passed in.  Passing in 0 will get a list of employees not assigned to any clinic.  Gets employees from the cache which is sorted by FName, LastName.</summary>
		public static List<Employee> GetEmpsForClinic(long clinicNum) {
			Meth.NoCheckMiddleTierRole();
			return GetEmpsForClinic(clinicNum,false);
		}

		///<summary>Gets all the employees for a specific clinicNum, according to their associated user.  Pass in a clinicNum of 0 to get the list of unassigned or "all" employees (depending on isAll flag).  In addition to setting clinicNum to 0, set isAll true to get a list of all employees or false to get a list of employees that are not associated to any clinics.  Always gets the list of employees from the cache which is sorted by FName, LastName.</summary>
		public static List<Employee> GetEmpsForClinic(long clinicNum,bool isAll,bool getUnassigned=false) {
			Meth.NoCheckMiddleTierRole();
			List<Employee> listEmployeesShort=Employees.GetDeepCopy(true);
			if(!PrefC.HasClinicsEnabled || (clinicNum==0 && isAll)) {//Simply return all employees.
				return listEmployeesShort;
			}
			List<Employee> listEmployeesWithClinic=new List<Employee>();
			List<Employee> listEmployeesUnassigned=new List<Employee>();
			Dictionary<long,List<UserClinic>> dictUserClinics=new Dictionary<long, List<UserClinic>>();
			for(int e=0;e<listEmployeesShort.Count;e++) {
				List<Userod> listUsers=Userods.GetUsersByEmployeeNum(listEmployeesShort[e].EmployeeNum);
				if(listUsers.Count==0) {
					listEmployeesUnassigned.Add(listEmployeesShort[e]);
					continue;
				}
				for(int u=0;u<listUsers.Count;u++) {//At this point we know there is at least one Userod associated to this employee.
					if(listUsers[u].ClinicNum==0) {//User's default clinic is HQ
						listEmployeesUnassigned.Add(listEmployeesShort[e]);
						continue;
					}
					if(!dictUserClinics.ContainsKey(listUsers[u].UserNum)) {//User is restricted to a clinic(s).  Compare to clinicNum
						dictUserClinics[listUsers[u].UserNum]=UserClinics.GetForUser(listUsers[u].UserNum);//run only once per user
					}
					if(dictUserClinics[listUsers[u].UserNum].Count==0) {//unrestricted user, employee should show in all lists
						listEmployeesUnassigned.Add(listEmployeesShort[e]);
						listEmployeesWithClinic.Add(listEmployeesShort[e]);
					}
					else if(dictUserClinics[listUsers[u].UserNum].Exists(x => x.ClinicNum==clinicNum)) {//user restricted to this clinic
						listEmployeesWithClinic.Add(listEmployeesShort[e]);
					}
				}
			}
			if(getUnassigned) {
				return listEmployeesUnassigned.Union(listEmployeesWithClinic).OrderBy(x=> Employees.GetNameFL(x)).ToList();
			}
			//Returning the isAll employee list was handled above (all non-hidden emps, ListShort).
			if(clinicNum==0) {//Return list of unassigned employees.  This is used for the 'Headquarters' clinic filter.
				return listEmployeesUnassigned.GroupBy(x => x.EmployeeNum).Select(x => x.First()).ToList();//select distinct emps
			}
			//Return list of employees restricted to the specified clinic.
			return listEmployeesWithClinic.GroupBy(x => x.EmployeeNum).Select(x => x.First()).ToList();//select distinct emps
		}

		///<summary>Gets employees from database. Returns an empty list if none found.</summary>
		public static List<Employee> GetEmployeesForApi(int limit,int offset,long reportsTo) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<Employee>>(MethodBase.GetCurrentMethod(),limit,offset,reportsTo);
			}
			string command="SELECT * FROM employee ";
			if(reportsTo>-1) {
				command+="WHERE ReportsTo="+POut.Long(reportsTo)+" ";
			}
			command+="ORDER BY employeenum "//Ensure order for limit and offset
				+"LIMIT "+POut.Int(offset)+", "+POut.Int(limit);
			return Crud.EmployeeCrud.SelectMany(command);
		}

		/// <summary> Returns -1 if employeeNum is not found.  0 if not hidden and 1 if hidden.</summary>		
		public static int IsHidden(long employeeNum) {
			Meth.NoCheckMiddleTierRole();
			Employee employee=GetFirstOrDefault(x => x.EmployeeNum==employeeNum);
			if(employee==null){
				return -1;
			}
			if(employee.IsHidden) {
				return 1;
			}
			return 0;
		}

		///<summary>Loops through List to find the given extension and returns the employeeNum if found.  Otherwise, returns -1;</summary>
		public static long GetEmpNumAtExtension(int phoneExt) {
			Meth.NoCheckMiddleTierRole();
			Employee employee=GetFirstOrDefault(x => x.PhoneExt==phoneExt);
			if(employee==null) {
				return -1; 
			}
			return employee.EmployeeNum;
		}

		public static int SortByLastName(Employee employee1,Employee employee2) {
			return employee1.LName.CompareTo(employee2.LName);
		}

		public static int SortByFirstName(Employee employee1,Employee employee2) {
			return employee1.FName.CompareTo(employee2.FName);
		}

		/// <summary>sorting class used to sort Employee in various ways</summary>
		public class EmployeeComparer:IComparer<Employee> {
		
			private SortBy SortOn=SortBy.lastName;
			
			public EmployeeComparer(SortBy sortBy) {
				SortOn=sortBy;
			}
			
			public int Compare(Employee employee1,Employee employee2) {
				int ret=0;
				switch(SortOn) {
					case SortBy.empNum:
						ret=employee1.EmployeeNum.CompareTo(employee2.EmployeeNum); 
						break;
					case SortBy.ext:
						ret=employee1.PhoneExt.CompareTo(employee2.PhoneExt); 
						break;
					case SortBy.firstName:
						ret=employee1.FName.CompareTo(employee2.FName); 
						break;
					case SortBy.LFName:
						ret=employee1.LName.CompareTo(employee2.LName);
						if(ret==0) {
							ret=employee1.FName.CompareTo(employee2.FName);
						}
						break;
					case SortBy.lastName:
					default:
						ret=employee1.LName.CompareTo(employee2.LName); 
						break;
				}
				if(ret==0) {//last name is tie breaker
					return employee1.LName.CompareTo(employee2.LName);
				}
				//we got here so our sort was successful
				return ret;
			}

			public enum SortBy {
				///<summary>0 - By Extension.</summary>
				ext,
				///<summary>1 - By EmployeeNum.</summary>
				empNum,
				///<summary>2 - By FName.</summary>
				firstName,
				///<summary>3 - By LName.</summary>
				lastName,
				///<summary>4 - By LName, then FName.</summary>
				LFName
			};
		}
	}
}
