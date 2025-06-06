using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SchoolCourses {
		#region CachePattern

		private class SchoolCourseCache : CacheListAbs<SchoolCourse> {
			protected override List<SchoolCourse> GetCacheFromDb() {
				string command=
					"SELECT * FROM schoolcourse "
					+"ORDER BY CourseID";
				return Crud.SchoolCourseCrud.SelectMany(command);
			}
			protected override List<SchoolCourse> TableToList(DataTable table) {
				return Crud.SchoolCourseCrud.TableToList(table);
			}
			protected override SchoolCourse Copy(SchoolCourse schoolCourse) {
				return schoolCourse.Copy();
			}
			protected override DataTable ListToTable(List<SchoolCourse> listSchoolCourses) {
				return Crud.SchoolCourseCrud.ListToTable(listSchoolCourses,"SchoolCourse");
			}
			protected override void FillCacheIfNeeded() {
				SchoolCourses.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static SchoolCourseCache _schoolCourseCache=new SchoolCourseCache();

		public static List<SchoolCourse> GetDeepCopy(bool isShort=false) {
			return _schoolCourseCache.GetDeepCopy(isShort);
		}

		public static SchoolCourse GetFirstOrDefault(Func<SchoolCourse,bool> funcMatch,bool isShort=false) {
			return _schoolCourseCache.GetFirstOrDefault(funcMatch,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_schoolCourseCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_schoolCourseCache.FillCacheFromTable(table);
				return table;
			}
			return _schoolCourseCache.GetTableFromCache(doRefreshCache);
		}

		public static void ClearCache() {
			_schoolCourseCache.ClearCache();
		}
		#endregion

		///<summary></summary>
		public static void Update(SchoolCourse schoolCourse){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),schoolCourse);
				return;
			}
			Crud.SchoolCourseCrud.Update(schoolCourse);
		}

		///<summary></summary>
		public static long Insert(SchoolCourse schoolCourse) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				schoolCourse.SchoolCourseNum=Meth.GetLong(MethodBase.GetCurrentMethod(),schoolCourse);
				return schoolCourse.SchoolCourseNum;
			}
			return Crud.SchoolCourseCrud.Insert(schoolCourse);
		}

		///<summary></summary>
		public static void InsertOrUpdate(SchoolCourse schoolCourse, bool isNew){
			Meth.NoCheckMiddleTierRole();
			//if(IsRepeating && DateTask.Year>1880){
			//	throw new Exception(Lans.g(this,"Task cannot be tagged repeating and also have a date."));
			//}
			if(isNew){
				Insert(schoolCourse);
			}
			else{
				Update(schoolCourse);
			}
		}

		///<summary></summary>
		public static void Delete(long courseNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),courseNum);
				return;
			}
			//check for attached reqneededs---------------------------------------------------------------------
			string command="SELECT COUNT(*) FROM reqneeded WHERE SchoolCourseNum = '"
				+POut.Long(courseNum)+"'";
			DataTable table=Db.GetTable(command);
			if(PIn.String(table.Rows[0][0].ToString())!="0") {
				throw new Exception(Lans.g("SchoolCourses","Course already in use by 'requirements needed' table."));
			}
			//check for attached reqstudents--------------------------------------------------------------------------
			command="SELECT COUNT(*) FROM reqstudent WHERE SchoolCourseNum = '"
				+POut.Long(courseNum)+"'";
			table=Db.GetTable(command);
			if(PIn.String(table.Rows[0][0].ToString())!="0") {
				throw new Exception(Lans.g("SchoolCourses","Course already in use by 'student requirements' table."));
			}
			//delete---------------------------------------------------------------------------------------------
			command= "DELETE from schoolcourse WHERE SchoolCourseNum = '"
				+POut.Long(courseNum)+"'";
 			Db.NonQ(command);
		}

		///<summary>Description is CourseID Descript.</summary>
		public static string GetDescript(long schoolCourseNum) {
			Meth.NoCheckMiddleTierRole();
			SchoolCourse schoolCourse=GetFirstOrDefault(x => x.SchoolCourseNum==schoolCourseNum);
			if (schoolCourse==null) {
				return "";
			}
			return GetDescript(schoolCourse);
		}

		public static string GetDescript(SchoolCourse schoolCourse){
			Meth.NoCheckMiddleTierRole();
			return schoolCourse.CourseID+" "+schoolCourse.Descript;
		}

		public static string GetCourseID(long schoolCourseNum) {
			Meth.NoCheckMiddleTierRole();
			SchoolCourse schoolCourse=GetFirstOrDefault(x => x.SchoolCourseNum==schoolCourseNum);
			if (schoolCourse==null) {
				return "";
			}
			return schoolCourse.CourseID;
		}
	}

}