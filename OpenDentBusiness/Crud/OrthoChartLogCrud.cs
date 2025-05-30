//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class OrthoChartLogCrud {
		///<summary>Gets one OrthoChartLog object from the database using the primary key.  Returns null if not found.</summary>
		public static OrthoChartLog SelectOne(long orthoChartLogNum) {
			string command="SELECT * FROM orthochartlog "
				+"WHERE OrthoChartLogNum = "+POut.Long(orthoChartLogNum);
			List<OrthoChartLog> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one OrthoChartLog object from the database using a query.</summary>
		public static OrthoChartLog SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<OrthoChartLog> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of OrthoChartLog objects from the database using a query.</summary>
		public static List<OrthoChartLog> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<OrthoChartLog> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<OrthoChartLog> TableToList(DataTable table) {
			List<OrthoChartLog> retVal=new List<OrthoChartLog>();
			OrthoChartLog orthoChartLog;
			foreach(DataRow row in table.Rows) {
				orthoChartLog=new OrthoChartLog();
				orthoChartLog.OrthoChartLogNum= PIn.Long  (row["OrthoChartLogNum"].ToString());
				orthoChartLog.PatNum          = PIn.Long  (row["PatNum"].ToString());
				orthoChartLog.ComputerName    = PIn.String(row["ComputerName"].ToString());
				orthoChartLog.DateTimeLog     = PIn.DateT (row["DateTimeLog"].ToString());
				orthoChartLog.DateTimeService = PIn.DateT (row["DateTimeService"].ToString());
				orthoChartLog.UserNum         = PIn.Long  (row["UserNum"].ToString());
				orthoChartLog.ProvNum         = PIn.Long  (row["ProvNum"].ToString());
				orthoChartLog.OrthoChartRowNum= PIn.Long  (row["OrthoChartRowNum"].ToString());
				orthoChartLog.LogData         = PIn.String(row["LogData"].ToString());
				retVal.Add(orthoChartLog);
			}
			return retVal;
		}

		///<summary>Converts a list of OrthoChartLog into a DataTable.</summary>
		public static DataTable ListToTable(List<OrthoChartLog> listOrthoChartLogs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="OrthoChartLog";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("OrthoChartLogNum");
			table.Columns.Add("PatNum");
			table.Columns.Add("ComputerName");
			table.Columns.Add("DateTimeLog");
			table.Columns.Add("DateTimeService");
			table.Columns.Add("UserNum");
			table.Columns.Add("ProvNum");
			table.Columns.Add("OrthoChartRowNum");
			table.Columns.Add("LogData");
			foreach(OrthoChartLog orthoChartLog in listOrthoChartLogs) {
				table.Rows.Add(new object[] {
					POut.Long  (orthoChartLog.OrthoChartLogNum),
					POut.Long  (orthoChartLog.PatNum),
					            orthoChartLog.ComputerName,
					POut.DateT (orthoChartLog.DateTimeLog,false),
					POut.DateT (orthoChartLog.DateTimeService,false),
					POut.Long  (orthoChartLog.UserNum),
					POut.Long  (orthoChartLog.ProvNum),
					POut.Long  (orthoChartLog.OrthoChartRowNum),
					            orthoChartLog.LogData,
				});
			}
			return table;
		}

		///<summary>Inserts one OrthoChartLog into the database.  Returns the new priKey.</summary>
		public static long Insert(OrthoChartLog orthoChartLog) {
			return Insert(orthoChartLog,false);
		}

		///<summary>Inserts one OrthoChartLog into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(OrthoChartLog orthoChartLog,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				orthoChartLog.OrthoChartLogNum=ReplicationServers.GetKey("orthochartlog","OrthoChartLogNum");
			}
			string command="INSERT INTO orthochartlog (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="OrthoChartLogNum,";
			}
			command+="PatNum,ComputerName,DateTimeLog,DateTimeService,UserNum,ProvNum,OrthoChartRowNum,LogData) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(orthoChartLog.OrthoChartLogNum)+",";
			}
			command+=
				     POut.Long  (orthoChartLog.PatNum)+","
				+"'"+POut.String(orthoChartLog.ComputerName)+"',"
				+    POut.DateT (orthoChartLog.DateTimeLog)+","
				+    POut.DateT (orthoChartLog.DateTimeService)+","
				+    POut.Long  (orthoChartLog.UserNum)+","
				+    POut.Long  (orthoChartLog.ProvNum)+","
				+    POut.Long  (orthoChartLog.OrthoChartRowNum)+","
				+    DbHelper.ParamChar+"paramLogData)";
			if(orthoChartLog.LogData==null) {
				orthoChartLog.LogData="";
			}
			OdSqlParameter paramLogData=new OdSqlParameter("paramLogData",OdDbType.Text,POut.StringParam(orthoChartLog.LogData));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramLogData);
			}
			else {
				orthoChartLog.OrthoChartLogNum=Db.NonQ(command,true,"OrthoChartLogNum","orthoChartLog",paramLogData);
			}
			return orthoChartLog.OrthoChartLogNum;
		}

		///<summary>Inserts one OrthoChartLog into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(OrthoChartLog orthoChartLog) {
			return InsertNoCache(orthoChartLog,false);
		}

		///<summary>Inserts one OrthoChartLog into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(OrthoChartLog orthoChartLog,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO orthochartlog (";
			if(!useExistingPK && isRandomKeys) {
				orthoChartLog.OrthoChartLogNum=ReplicationServers.GetKeyNoCache("orthochartlog","OrthoChartLogNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="OrthoChartLogNum,";
			}
			command+="PatNum,ComputerName,DateTimeLog,DateTimeService,UserNum,ProvNum,OrthoChartRowNum,LogData) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(orthoChartLog.OrthoChartLogNum)+",";
			}
			command+=
				     POut.Long  (orthoChartLog.PatNum)+","
				+"'"+POut.String(orthoChartLog.ComputerName)+"',"
				+    POut.DateT (orthoChartLog.DateTimeLog)+","
				+    POut.DateT (orthoChartLog.DateTimeService)+","
				+    POut.Long  (orthoChartLog.UserNum)+","
				+    POut.Long  (orthoChartLog.ProvNum)+","
				+    POut.Long  (orthoChartLog.OrthoChartRowNum)+","
				+    DbHelper.ParamChar+"paramLogData)";
			if(orthoChartLog.LogData==null) {
				orthoChartLog.LogData="";
			}
			OdSqlParameter paramLogData=new OdSqlParameter("paramLogData",OdDbType.Text,POut.StringParam(orthoChartLog.LogData));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramLogData);
			}
			else {
				orthoChartLog.OrthoChartLogNum=Db.NonQ(command,true,"OrthoChartLogNum","orthoChartLog",paramLogData);
			}
			return orthoChartLog.OrthoChartLogNum;
		}

		///<summary>Updates one OrthoChartLog in the database.</summary>
		public static void Update(OrthoChartLog orthoChartLog) {
			string command="UPDATE orthochartlog SET "
				+"PatNum          =  "+POut.Long  (orthoChartLog.PatNum)+", "
				+"ComputerName    = '"+POut.String(orthoChartLog.ComputerName)+"', "
				+"DateTimeLog     =  "+POut.DateT (orthoChartLog.DateTimeLog)+", "
				+"DateTimeService =  "+POut.DateT (orthoChartLog.DateTimeService)+", "
				+"UserNum         =  "+POut.Long  (orthoChartLog.UserNum)+", "
				+"ProvNum         =  "+POut.Long  (orthoChartLog.ProvNum)+", "
				+"OrthoChartRowNum=  "+POut.Long  (orthoChartLog.OrthoChartRowNum)+", "
				+"LogData         =  "+DbHelper.ParamChar+"paramLogData "
				+"WHERE OrthoChartLogNum = "+POut.Long(orthoChartLog.OrthoChartLogNum);
			if(orthoChartLog.LogData==null) {
				orthoChartLog.LogData="";
			}
			OdSqlParameter paramLogData=new OdSqlParameter("paramLogData",OdDbType.Text,POut.StringParam(orthoChartLog.LogData));
			Db.NonQ(command,paramLogData);
		}

		///<summary>Updates one OrthoChartLog in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(OrthoChartLog orthoChartLog,OrthoChartLog oldOrthoChartLog) {
			string command="";
			if(orthoChartLog.PatNum != oldOrthoChartLog.PatNum) {
				if(command!="") { command+=",";}
				command+="PatNum = "+POut.Long(orthoChartLog.PatNum)+"";
			}
			if(orthoChartLog.ComputerName != oldOrthoChartLog.ComputerName) {
				if(command!="") { command+=",";}
				command+="ComputerName = '"+POut.String(orthoChartLog.ComputerName)+"'";
			}
			if(orthoChartLog.DateTimeLog != oldOrthoChartLog.DateTimeLog) {
				if(command!="") { command+=",";}
				command+="DateTimeLog = "+POut.DateT(orthoChartLog.DateTimeLog)+"";
			}
			if(orthoChartLog.DateTimeService != oldOrthoChartLog.DateTimeService) {
				if(command!="") { command+=",";}
				command+="DateTimeService = "+POut.DateT(orthoChartLog.DateTimeService)+"";
			}
			if(orthoChartLog.UserNum != oldOrthoChartLog.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(orthoChartLog.UserNum)+"";
			}
			if(orthoChartLog.ProvNum != oldOrthoChartLog.ProvNum) {
				if(command!="") { command+=",";}
				command+="ProvNum = "+POut.Long(orthoChartLog.ProvNum)+"";
			}
			if(orthoChartLog.OrthoChartRowNum != oldOrthoChartLog.OrthoChartRowNum) {
				if(command!="") { command+=",";}
				command+="OrthoChartRowNum = "+POut.Long(orthoChartLog.OrthoChartRowNum)+"";
			}
			if(orthoChartLog.LogData != oldOrthoChartLog.LogData) {
				if(command!="") { command+=",";}
				command+="LogData = "+DbHelper.ParamChar+"paramLogData";
			}
			if(command=="") {
				return false;
			}
			if(orthoChartLog.LogData==null) {
				orthoChartLog.LogData="";
			}
			OdSqlParameter paramLogData=new OdSqlParameter("paramLogData",OdDbType.Text,POut.StringParam(orthoChartLog.LogData));
			command="UPDATE orthochartlog SET "+command
				+" WHERE OrthoChartLogNum = "+POut.Long(orthoChartLog.OrthoChartLogNum);
			Db.NonQ(command,paramLogData);
			return true;
		}

		///<summary>Returns true if Update(OrthoChartLog,OrthoChartLog) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(OrthoChartLog orthoChartLog,OrthoChartLog oldOrthoChartLog) {
			if(orthoChartLog.PatNum != oldOrthoChartLog.PatNum) {
				return true;
			}
			if(orthoChartLog.ComputerName != oldOrthoChartLog.ComputerName) {
				return true;
			}
			if(orthoChartLog.DateTimeLog != oldOrthoChartLog.DateTimeLog) {
				return true;
			}
			if(orthoChartLog.DateTimeService != oldOrthoChartLog.DateTimeService) {
				return true;
			}
			if(orthoChartLog.UserNum != oldOrthoChartLog.UserNum) {
				return true;
			}
			if(orthoChartLog.ProvNum != oldOrthoChartLog.ProvNum) {
				return true;
			}
			if(orthoChartLog.OrthoChartRowNum != oldOrthoChartLog.OrthoChartRowNum) {
				return true;
			}
			if(orthoChartLog.LogData != oldOrthoChartLog.LogData) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one OrthoChartLog from the database.</summary>
		public static void Delete(long orthoChartLogNum) {
			string command="DELETE FROM orthochartlog "
				+"WHERE OrthoChartLogNum = "+POut.Long(orthoChartLogNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many OrthoChartLogs from the database.</summary>
		public static void DeleteMany(List<long> listOrthoChartLogNums) {
			if(listOrthoChartLogNums==null || listOrthoChartLogNums.Count==0) {
				return;
			}
			string command="DELETE FROM orthochartlog "
				+"WHERE OrthoChartLogNum IN("+string.Join(",",listOrthoChartLogNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}