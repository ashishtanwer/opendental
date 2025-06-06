//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class AutoCodeItemCrud {
		///<summary>Gets one AutoCodeItem object from the database using the primary key.  Returns null if not found.</summary>
		public static AutoCodeItem SelectOne(long autoCodeItemNum) {
			string command="SELECT * FROM autocodeitem "
				+"WHERE AutoCodeItemNum = "+POut.Long(autoCodeItemNum);
			List<AutoCodeItem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one AutoCodeItem object from the database using a query.</summary>
		public static AutoCodeItem SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<AutoCodeItem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of AutoCodeItem objects from the database using a query.</summary>
		public static List<AutoCodeItem> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<AutoCodeItem> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<AutoCodeItem> TableToList(DataTable table) {
			List<AutoCodeItem> retVal=new List<AutoCodeItem>();
			AutoCodeItem autoCodeItem;
			foreach(DataRow row in table.Rows) {
				autoCodeItem=new AutoCodeItem();
				autoCodeItem.AutoCodeItemNum= PIn.Long  (row["AutoCodeItemNum"].ToString());
				autoCodeItem.AutoCodeNum    = PIn.Long  (row["AutoCodeNum"].ToString());
				autoCodeItem.OldCode        = PIn.String(row["OldCode"].ToString());
				autoCodeItem.CodeNum        = PIn.Long  (row["CodeNum"].ToString());
				retVal.Add(autoCodeItem);
			}
			return retVal;
		}

		///<summary>Converts a list of AutoCodeItem into a DataTable.</summary>
		public static DataTable ListToTable(List<AutoCodeItem> listAutoCodeItems,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="AutoCodeItem";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("AutoCodeItemNum");
			table.Columns.Add("AutoCodeNum");
			table.Columns.Add("OldCode");
			table.Columns.Add("CodeNum");
			foreach(AutoCodeItem autoCodeItem in listAutoCodeItems) {
				table.Rows.Add(new object[] {
					POut.Long  (autoCodeItem.AutoCodeItemNum),
					POut.Long  (autoCodeItem.AutoCodeNum),
					            autoCodeItem.OldCode,
					POut.Long  (autoCodeItem.CodeNum),
				});
			}
			return table;
		}

		///<summary>Inserts one AutoCodeItem into the database.  Returns the new priKey.</summary>
		public static long Insert(AutoCodeItem autoCodeItem) {
			return Insert(autoCodeItem,false);
		}

		///<summary>Inserts one AutoCodeItem into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(AutoCodeItem autoCodeItem,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				autoCodeItem.AutoCodeItemNum=ReplicationServers.GetKey("autocodeitem","AutoCodeItemNum");
			}
			string command="INSERT INTO autocodeitem (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="AutoCodeItemNum,";
			}
			command+="AutoCodeNum,OldCode,CodeNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(autoCodeItem.AutoCodeItemNum)+",";
			}
			command+=
				     POut.Long  (autoCodeItem.AutoCodeNum)+","
				+"'"+POut.String(autoCodeItem.OldCode)+"',"
				+    POut.Long  (autoCodeItem.CodeNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				autoCodeItem.AutoCodeItemNum=Db.NonQ(command,true,"AutoCodeItemNum","autoCodeItem");
			}
			return autoCodeItem.AutoCodeItemNum;
		}

		///<summary>Inserts one AutoCodeItem into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(AutoCodeItem autoCodeItem) {
			return InsertNoCache(autoCodeItem,false);
		}

		///<summary>Inserts one AutoCodeItem into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(AutoCodeItem autoCodeItem,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO autocodeitem (";
			if(!useExistingPK && isRandomKeys) {
				autoCodeItem.AutoCodeItemNum=ReplicationServers.GetKeyNoCache("autocodeitem","AutoCodeItemNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="AutoCodeItemNum,";
			}
			command+="AutoCodeNum,OldCode,CodeNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(autoCodeItem.AutoCodeItemNum)+",";
			}
			command+=
				     POut.Long  (autoCodeItem.AutoCodeNum)+","
				+"'"+POut.String(autoCodeItem.OldCode)+"',"
				+    POut.Long  (autoCodeItem.CodeNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				autoCodeItem.AutoCodeItemNum=Db.NonQ(command,true,"AutoCodeItemNum","autoCodeItem");
			}
			return autoCodeItem.AutoCodeItemNum;
		}

		///<summary>Updates one AutoCodeItem in the database.</summary>
		public static void Update(AutoCodeItem autoCodeItem) {
			string command="UPDATE autocodeitem SET "
				+"AutoCodeNum    =  "+POut.Long  (autoCodeItem.AutoCodeNum)+", "
				+"OldCode        = '"+POut.String(autoCodeItem.OldCode)+"', "
				+"CodeNum        =  "+POut.Long  (autoCodeItem.CodeNum)+" "
				+"WHERE AutoCodeItemNum = "+POut.Long(autoCodeItem.AutoCodeItemNum);
			Db.NonQ(command);
		}

		///<summary>Updates one AutoCodeItem in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(AutoCodeItem autoCodeItem,AutoCodeItem oldAutoCodeItem) {
			string command="";
			if(autoCodeItem.AutoCodeNum != oldAutoCodeItem.AutoCodeNum) {
				if(command!="") { command+=",";}
				command+="AutoCodeNum = "+POut.Long(autoCodeItem.AutoCodeNum)+"";
			}
			if(autoCodeItem.OldCode != oldAutoCodeItem.OldCode) {
				if(command!="") { command+=",";}
				command+="OldCode = '"+POut.String(autoCodeItem.OldCode)+"'";
			}
			if(autoCodeItem.CodeNum != oldAutoCodeItem.CodeNum) {
				if(command!="") { command+=",";}
				command+="CodeNum = "+POut.Long(autoCodeItem.CodeNum)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE autocodeitem SET "+command
				+" WHERE AutoCodeItemNum = "+POut.Long(autoCodeItem.AutoCodeItemNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(AutoCodeItem,AutoCodeItem) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(AutoCodeItem autoCodeItem,AutoCodeItem oldAutoCodeItem) {
			if(autoCodeItem.AutoCodeNum != oldAutoCodeItem.AutoCodeNum) {
				return true;
			}
			if(autoCodeItem.OldCode != oldAutoCodeItem.OldCode) {
				return true;
			}
			if(autoCodeItem.CodeNum != oldAutoCodeItem.CodeNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one AutoCodeItem from the database.</summary>
		public static void Delete(long autoCodeItemNum) {
			string command="DELETE FROM autocodeitem "
				+"WHERE AutoCodeItemNum = "+POut.Long(autoCodeItemNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many AutoCodeItems from the database.</summary>
		public static void DeleteMany(List<long> listAutoCodeItemNums) {
			if(listAutoCodeItemNums==null || listAutoCodeItemNums.Count==0) {
				return;
			}
			string command="DELETE FROM autocodeitem "
				+"WHERE AutoCodeItemNum IN("+string.Join(",",listAutoCodeItemNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}