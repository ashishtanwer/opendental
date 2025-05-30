//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class APIKeyCrud {
		///<summary>Gets one APIKey object from the database using the primary key.  Returns null if not found.</summary>
		public static APIKey SelectOne(long aPIKeyNum) {
			string command="SELECT * FROM apikey "
				+"WHERE APIKeyNum = "+POut.Long(aPIKeyNum);
			List<APIKey> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one APIKey object from the database using a query.</summary>
		public static APIKey SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<APIKey> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of APIKey objects from the database using a query.</summary>
		public static List<APIKey> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<APIKey> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<APIKey> TableToList(DataTable table) {
			List<APIKey> retVal=new List<APIKey>();
			APIKey aPIKey;
			foreach(DataRow row in table.Rows) {
				aPIKey=new APIKey();
				aPIKey.APIKeyNum = PIn.Long  (row["APIKeyNum"].ToString());
				aPIKey.CustApiKey= PIn.String(row["CustApiKey"].ToString());
				aPIKey.DevName   = PIn.String(row["DevName"].ToString());
				retVal.Add(aPIKey);
			}
			return retVal;
		}

		///<summary>Converts a list of APIKey into a DataTable.</summary>
		public static DataTable ListToTable(List<APIKey> listAPIKeys,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="APIKey";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("APIKeyNum");
			table.Columns.Add("CustApiKey");
			table.Columns.Add("DevName");
			foreach(APIKey aPIKey in listAPIKeys) {
				table.Rows.Add(new object[] {
					POut.Long  (aPIKey.APIKeyNum),
					            aPIKey.CustApiKey,
					            aPIKey.DevName,
				});
			}
			return table;
		}

		///<summary>Inserts one APIKey into the database.  Returns the new priKey.</summary>
		public static long Insert(APIKey aPIKey) {
			return Insert(aPIKey,false);
		}

		///<summary>Inserts one APIKey into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(APIKey aPIKey,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				aPIKey.APIKeyNum=ReplicationServers.GetKey("apikey","APIKeyNum");
			}
			string command="INSERT INTO apikey (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="APIKeyNum,";
			}
			command+="CustApiKey,DevName) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(aPIKey.APIKeyNum)+",";
			}
			command+=
				 "'"+POut.String(aPIKey.CustApiKey)+"',"
				+"'"+POut.String(aPIKey.DevName)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				aPIKey.APIKeyNum=Db.NonQ(command,true,"APIKeyNum","aPIKey");
			}
			return aPIKey.APIKeyNum;
		}

		///<summary>Inserts one APIKey into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(APIKey aPIKey) {
			return InsertNoCache(aPIKey,false);
		}

		///<summary>Inserts one APIKey into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(APIKey aPIKey,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO apikey (";
			if(!useExistingPK && isRandomKeys) {
				aPIKey.APIKeyNum=ReplicationServers.GetKeyNoCache("apikey","APIKeyNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="APIKeyNum,";
			}
			command+="CustApiKey,DevName) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(aPIKey.APIKeyNum)+",";
			}
			command+=
				 "'"+POut.String(aPIKey.CustApiKey)+"',"
				+"'"+POut.String(aPIKey.DevName)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				aPIKey.APIKeyNum=Db.NonQ(command,true,"APIKeyNum","aPIKey");
			}
			return aPIKey.APIKeyNum;
		}

		///<summary>Updates one APIKey in the database.</summary>
		public static void Update(APIKey aPIKey) {
			string command="UPDATE apikey SET "
				+"CustApiKey= '"+POut.String(aPIKey.CustApiKey)+"', "
				+"DevName   = '"+POut.String(aPIKey.DevName)+"' "
				+"WHERE APIKeyNum = "+POut.Long(aPIKey.APIKeyNum);
			Db.NonQ(command);
		}

		///<summary>Updates one APIKey in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(APIKey aPIKey,APIKey oldAPIKey) {
			string command="";
			if(aPIKey.CustApiKey != oldAPIKey.CustApiKey) {
				if(command!="") { command+=",";}
				command+="CustApiKey = '"+POut.String(aPIKey.CustApiKey)+"'";
			}
			if(aPIKey.DevName != oldAPIKey.DevName) {
				if(command!="") { command+=",";}
				command+="DevName = '"+POut.String(aPIKey.DevName)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE apikey SET "+command
				+" WHERE APIKeyNum = "+POut.Long(aPIKey.APIKeyNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(APIKey,APIKey) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(APIKey aPIKey,APIKey oldAPIKey) {
			if(aPIKey.CustApiKey != oldAPIKey.CustApiKey) {
				return true;
			}
			if(aPIKey.DevName != oldAPIKey.DevName) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one APIKey from the database.</summary>
		public static void Delete(long aPIKeyNum) {
			string command="DELETE FROM apikey "
				+"WHERE APIKeyNum = "+POut.Long(aPIKeyNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many APIKeys from the database.</summary>
		public static void DeleteMany(List<long> listAPIKeyNums) {
			if(listAPIKeyNums==null || listAPIKeyNums.Count==0) {
				return;
			}
			string command="DELETE FROM apikey "
				+"WHERE APIKeyNum IN("+string.Join(",",listAPIKeyNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}