using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
  ///<summary></summary>
	public class Counties{
		///<summary>Gets county names similar to the one provided.</summary>
		public static List<County> Refresh(string name){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<County>>(MethodBase.GetCurrentMethod(),name);
			}
			string command=
				"SELECT * from county "
				+"WHERE CountyName LIKE '"+POut.String(name)+"%' "
				+"ORDER BY CountyName";
			List<County> listCounties=Crud.CountyCrud.SelectMany(command);
			for(int i=0;i<listCounties.Count;i++) {
				listCounties[i].CountyNameOld=listCounties[i].CountyName;
			}
			return listCounties;
		}

		///<summary>Gets all counties.</summary>
		public static List<County> GetAll(){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<County>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * from county ORDER BY CountyName";
			List<County> listCounties=Crud.CountyCrud.SelectMany(command);
			for(int i=0;i<listCounties.Count;i++) {
				listCounties[i].CountyNameOld=listCounties[i].CountyName;
			}
			return listCounties;
		}

		///<summary>Gets an array of strings containing all the counties in alphabetical order.  Used for the screening interface which must be simpler than the usual interface.</summary>
		public static List<string> GetListNames(){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod());
			}
			string command =
				"SELECT CountyName from county "
				+"ORDER BY CountyName";
			DataTable table=Db.GetTable(command);
			List<string> listStringNames=new List<string>();
			for(int i=0;i<table.Rows.Count;i++){
				listStringNames.Add(PIn.String(table.Rows[i]["CountyName"].ToString()));
			}
			return listStringNames;
		}

		///<summary>Need to make sure Countyname not already in db.</summary>
		public static long Insert(County county){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				county.CountyNum=Meth.GetLong(MethodBase.GetCurrentMethod(),county);
				return county.CountyNum;
			}
			return Crud.CountyCrud.Insert(county);
		}

		///<summary>Updates the Countyname and code in the County table, and also updates all patients that were using the oldCounty name.</summary>
		public static void Update(County county){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),county);
				return;
			}
			//Can't use CRUD here because we're updating by the OldCountyName
			string command = "UPDATE county SET "
				+"CountyName ='"  +POut.String(county.CountyName)+"'"
				+",CountyCode ='" +POut.String(county.CountyCode)+"'"
				+" WHERE CountyName = '"+POut.String(county.CountyNameOld)+"'";
			Db.NonQ(command);
			//then, update all patients using that County
			command = "UPDATE patient SET "
				+"County ='"  +POut.String(county.CountyName)+"'"
				+" WHERE County = '"+POut.String(county.CountyNameOld)+"'";
			Db.NonQ(command);
		}

		///<summary>Must run UsedBy before running this.</summary>
		public static void Delete(County county){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),county);
				return;
			}
			string command= "DELETE from county WHERE CountyName = '"+POut.String(county.CountyName)+"'";
			Db.NonQ(command);
		}

		///<summary>Use before DeleteCur to determine if this County name is in use. Returns a formatted string that can be used to quickly display the names of all patients using the Countyname.</summary>
		public static string UsedBy(string countyName){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),countyName);
			}
			string command=
				"SELECT LName,FName FROM patient "
				+"WHERE County = '"+POut.String(countyName)+"'";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return "";
			}
			string retVal="";
			for(int i=0;i<table.Rows.Count;i++){
				retVal+=PIn.String(table.Rows[i][0].ToString())+", "
					+PIn.String(table.Rows[i][1].ToString());
				if(i<table.Rows.Count-1){//if not the last row
					retVal+="\r";
				}
			}
			return retVal;
		}

		///<summary>Use before Insert to determine if this County name already exists. Also used when closing patient edit window to validate that the Countyname exists.</summary>
		public static bool DoesExist(string countyName){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),countyName);
			}
			string command=
				"SELECT * FROM county "
				+"WHERE CountyName = '"+POut.String(countyName)+"' ";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return false;
			}
			return true;
		}
	}

	

}













