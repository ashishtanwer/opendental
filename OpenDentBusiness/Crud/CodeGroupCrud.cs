//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class CodeGroupCrud {
		///<summary>Gets one CodeGroup object from the database using the primary key.  Returns null if not found.</summary>
		public static CodeGroup SelectOne(long codeGroupNum) {
			string command="SELECT * FROM codegroup "
				+"WHERE CodeGroupNum = "+POut.Long(codeGroupNum);
			List<CodeGroup> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one CodeGroup object from the database using a query.</summary>
		public static CodeGroup SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<CodeGroup> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of CodeGroup objects from the database using a query.</summary>
		public static List<CodeGroup> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<CodeGroup> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<CodeGroup> TableToList(DataTable table) {
			List<CodeGroup> retVal=new List<CodeGroup>();
			CodeGroup codeGroup;
			foreach(DataRow row in table.Rows) {
				codeGroup=new CodeGroup();
				codeGroup.CodeGroupNum  = PIn.Long  (row["CodeGroupNum"].ToString());
				codeGroup.GroupName     = PIn.String(row["GroupName"].ToString());
				codeGroup.ProcCodes     = PIn.String(row["ProcCodes"].ToString());
				codeGroup.ItemOrder     = PIn.Int   (row["ItemOrder"].ToString());
				codeGroup.CodeGroupFixed= (OpenDentBusiness.EnumCodeGroupFixed)PIn.Int(row["CodeGroupFixed"].ToString());
				codeGroup.IsHidden      = PIn.Bool  (row["IsHidden"].ToString());
				codeGroup.ShowInAgeLimit= PIn.Bool  (row["ShowInAgeLimit"].ToString());
				retVal.Add(codeGroup);
			}
			return retVal;
		}

		///<summary>Converts a list of CodeGroup into a DataTable.</summary>
		public static DataTable ListToTable(List<CodeGroup> listCodeGroups,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="CodeGroup";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("CodeGroupNum");
			table.Columns.Add("GroupName");
			table.Columns.Add("ProcCodes");
			table.Columns.Add("ItemOrder");
			table.Columns.Add("CodeGroupFixed");
			table.Columns.Add("IsHidden");
			table.Columns.Add("ShowInAgeLimit");
			foreach(CodeGroup codeGroup in listCodeGroups) {
				table.Rows.Add(new object[] {
					POut.Long  (codeGroup.CodeGroupNum),
					            codeGroup.GroupName,
					            codeGroup.ProcCodes,
					POut.Int   (codeGroup.ItemOrder),
					POut.Int   ((int)codeGroup.CodeGroupFixed),
					POut.Bool  (codeGroup.IsHidden),
					POut.Bool  (codeGroup.ShowInAgeLimit),
				});
			}
			return table;
		}

		///<summary>Inserts one CodeGroup into the database.  Returns the new priKey.</summary>
		public static long Insert(CodeGroup codeGroup) {
			return Insert(codeGroup,false);
		}

		///<summary>Inserts one CodeGroup into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(CodeGroup codeGroup,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				codeGroup.CodeGroupNum=ReplicationServers.GetKey("codegroup","CodeGroupNum");
			}
			string command="INSERT INTO codegroup (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="CodeGroupNum,";
			}
			command+="GroupName,ProcCodes,ItemOrder,CodeGroupFixed,IsHidden,ShowInAgeLimit) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(codeGroup.CodeGroupNum)+",";
			}
			command+=
				 "'"+POut.String(codeGroup.GroupName)+"',"
				+    DbHelper.ParamChar+"paramProcCodes,"
				+    POut.Int   (codeGroup.ItemOrder)+","
				+    POut.Int   ((int)codeGroup.CodeGroupFixed)+","
				+    POut.Bool  (codeGroup.IsHidden)+","
				+    POut.Bool  (codeGroup.ShowInAgeLimit)+")";
			if(codeGroup.ProcCodes==null) {
				codeGroup.ProcCodes="";
			}
			OdSqlParameter paramProcCodes=new OdSqlParameter("paramProcCodes",OdDbType.Text,POut.StringParam(codeGroup.ProcCodes));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramProcCodes);
			}
			else {
				codeGroup.CodeGroupNum=Db.NonQ(command,true,"CodeGroupNum","codeGroup",paramProcCodes);
			}
			return codeGroup.CodeGroupNum;
		}

		///<summary>Inserts one CodeGroup into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(CodeGroup codeGroup) {
			return InsertNoCache(codeGroup,false);
		}

		///<summary>Inserts one CodeGroup into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(CodeGroup codeGroup,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO codegroup (";
			if(!useExistingPK && isRandomKeys) {
				codeGroup.CodeGroupNum=ReplicationServers.GetKeyNoCache("codegroup","CodeGroupNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="CodeGroupNum,";
			}
			command+="GroupName,ProcCodes,ItemOrder,CodeGroupFixed,IsHidden,ShowInAgeLimit) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(codeGroup.CodeGroupNum)+",";
			}
			command+=
				 "'"+POut.String(codeGroup.GroupName)+"',"
				+    DbHelper.ParamChar+"paramProcCodes,"
				+    POut.Int   (codeGroup.ItemOrder)+","
				+    POut.Int   ((int)codeGroup.CodeGroupFixed)+","
				+    POut.Bool  (codeGroup.IsHidden)+","
				+    POut.Bool  (codeGroup.ShowInAgeLimit)+")";
			if(codeGroup.ProcCodes==null) {
				codeGroup.ProcCodes="";
			}
			OdSqlParameter paramProcCodes=new OdSqlParameter("paramProcCodes",OdDbType.Text,POut.StringParam(codeGroup.ProcCodes));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramProcCodes);
			}
			else {
				codeGroup.CodeGroupNum=Db.NonQ(command,true,"CodeGroupNum","codeGroup",paramProcCodes);
			}
			return codeGroup.CodeGroupNum;
		}

		///<summary>Updates one CodeGroup in the database.</summary>
		public static void Update(CodeGroup codeGroup) {
			string command="UPDATE codegroup SET "
				+"GroupName     = '"+POut.String(codeGroup.GroupName)+"', "
				+"ProcCodes     =  "+DbHelper.ParamChar+"paramProcCodes, "
				+"ItemOrder     =  "+POut.Int   (codeGroup.ItemOrder)+", "
				+"CodeGroupFixed=  "+POut.Int   ((int)codeGroup.CodeGroupFixed)+", "
				+"IsHidden      =  "+POut.Bool  (codeGroup.IsHidden)+", "
				+"ShowInAgeLimit=  "+POut.Bool  (codeGroup.ShowInAgeLimit)+" "
				+"WHERE CodeGroupNum = "+POut.Long(codeGroup.CodeGroupNum);
			if(codeGroup.ProcCodes==null) {
				codeGroup.ProcCodes="";
			}
			OdSqlParameter paramProcCodes=new OdSqlParameter("paramProcCodes",OdDbType.Text,POut.StringParam(codeGroup.ProcCodes));
			Db.NonQ(command,paramProcCodes);
		}

		///<summary>Updates one CodeGroup in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(CodeGroup codeGroup,CodeGroup oldCodeGroup) {
			string command="";
			if(codeGroup.GroupName != oldCodeGroup.GroupName) {
				if(command!="") { command+=",";}
				command+="GroupName = '"+POut.String(codeGroup.GroupName)+"'";
			}
			if(codeGroup.ProcCodes != oldCodeGroup.ProcCodes) {
				if(command!="") { command+=",";}
				command+="ProcCodes = "+DbHelper.ParamChar+"paramProcCodes";
			}
			if(codeGroup.ItemOrder != oldCodeGroup.ItemOrder) {
				if(command!="") { command+=",";}
				command+="ItemOrder = "+POut.Int(codeGroup.ItemOrder)+"";
			}
			if(codeGroup.CodeGroupFixed != oldCodeGroup.CodeGroupFixed) {
				if(command!="") { command+=",";}
				command+="CodeGroupFixed = "+POut.Int   ((int)codeGroup.CodeGroupFixed)+"";
			}
			if(codeGroup.IsHidden != oldCodeGroup.IsHidden) {
				if(command!="") { command+=",";}
				command+="IsHidden = "+POut.Bool(codeGroup.IsHidden)+"";
			}
			if(codeGroup.ShowInAgeLimit != oldCodeGroup.ShowInAgeLimit) {
				if(command!="") { command+=",";}
				command+="ShowInAgeLimit = "+POut.Bool(codeGroup.ShowInAgeLimit)+"";
			}
			if(command=="") {
				return false;
			}
			if(codeGroup.ProcCodes==null) {
				codeGroup.ProcCodes="";
			}
			OdSqlParameter paramProcCodes=new OdSqlParameter("paramProcCodes",OdDbType.Text,POut.StringParam(codeGroup.ProcCodes));
			command="UPDATE codegroup SET "+command
				+" WHERE CodeGroupNum = "+POut.Long(codeGroup.CodeGroupNum);
			Db.NonQ(command,paramProcCodes);
			return true;
		}

		///<summary>Returns true if Update(CodeGroup,CodeGroup) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(CodeGroup codeGroup,CodeGroup oldCodeGroup) {
			if(codeGroup.GroupName != oldCodeGroup.GroupName) {
				return true;
			}
			if(codeGroup.ProcCodes != oldCodeGroup.ProcCodes) {
				return true;
			}
			if(codeGroup.ItemOrder != oldCodeGroup.ItemOrder) {
				return true;
			}
			if(codeGroup.CodeGroupFixed != oldCodeGroup.CodeGroupFixed) {
				return true;
			}
			if(codeGroup.IsHidden != oldCodeGroup.IsHidden) {
				return true;
			}
			if(codeGroup.ShowInAgeLimit != oldCodeGroup.ShowInAgeLimit) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one CodeGroup from the database.</summary>
		public static void Delete(long codeGroupNum) {
			string command="DELETE FROM codegroup "
				+"WHERE CodeGroupNum = "+POut.Long(codeGroupNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many CodeGroups from the database.</summary>
		public static void DeleteMany(List<long> listCodeGroupNums) {
			if(listCodeGroupNums==null || listCodeGroupNums.Count==0) {
				return;
			}
			string command="DELETE FROM codegroup "
				+"WHERE CodeGroupNum IN("+string.Join(",",listCodeGroupNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<CodeGroup> listNew,List<CodeGroup> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<CodeGroup> listIns    =new List<CodeGroup>();
			List<CodeGroup> listUpdNew =new List<CodeGroup>();
			List<CodeGroup> listUpdDB  =new List<CodeGroup>();
			List<CodeGroup> listDel    =new List<CodeGroup>();
			listNew.Sort((CodeGroup x,CodeGroup y) => { return x.CodeGroupNum.CompareTo(y.CodeGroupNum); });
			listDB.Sort((CodeGroup x,CodeGroup y) => { return x.CodeGroupNum.CompareTo(y.CodeGroupNum); });
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			CodeGroup fieldNew;
			CodeGroup fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listDB.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];
				}
				fieldDB=null;
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				else if(fieldNew.CodeGroupNum<fieldDB.CodeGroupNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.CodeGroupNum>fieldDB.CodeGroupNum) {//dbPK less than newPK, dbItem is 'next'
					listDel.Add(fieldDB);
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				listUpdNew.Add(fieldNew);
				listUpdDB.Add(fieldDB);
				idxNew++;
				idxDB++;
			}
			//Commit changes to DB
			for(int i=0;i<listIns.Count;i++) {
				Insert(listIns[i]);
			}
			for(int i=0;i<listUpdNew.Count;i++) {
				if(Update(listUpdNew[i],listUpdDB[i])) {
					rowsUpdatedCount++;
				}
			}
			DeleteMany(listDel.Select(x => x.CodeGroupNum).ToList());
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}