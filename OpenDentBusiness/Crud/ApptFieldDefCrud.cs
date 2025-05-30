//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class ApptFieldDefCrud {
		///<summary>Gets one ApptFieldDef object from the database using the primary key.  Returns null if not found.</summary>
		public static ApptFieldDef SelectOne(long apptFieldDefNum) {
			string command="SELECT * FROM apptfielddef "
				+"WHERE ApptFieldDefNum = "+POut.Long(apptFieldDefNum);
			List<ApptFieldDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ApptFieldDef object from the database using a query.</summary>
		public static ApptFieldDef SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ApptFieldDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ApptFieldDef objects from the database using a query.</summary>
		public static List<ApptFieldDef> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ApptFieldDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<ApptFieldDef> TableToList(DataTable table) {
			List<ApptFieldDef> retVal=new List<ApptFieldDef>();
			ApptFieldDef apptFieldDef;
			foreach(DataRow row in table.Rows) {
				apptFieldDef=new ApptFieldDef();
				apptFieldDef.ApptFieldDefNum= PIn.Long  (row["ApptFieldDefNum"].ToString());
				apptFieldDef.FieldName      = PIn.String(row["FieldName"].ToString());
				apptFieldDef.FieldType      = (OpenDentBusiness.ApptFieldType)PIn.Int(row["FieldType"].ToString());
				apptFieldDef.PickList       = PIn.String(row["PickList"].ToString());
				apptFieldDef.ItemOrder      = PIn.Int   (row["ItemOrder"].ToString());
				retVal.Add(apptFieldDef);
			}
			return retVal;
		}

		///<summary>Converts a list of ApptFieldDef into a DataTable.</summary>
		public static DataTable ListToTable(List<ApptFieldDef> listApptFieldDefs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="ApptFieldDef";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("ApptFieldDefNum");
			table.Columns.Add("FieldName");
			table.Columns.Add("FieldType");
			table.Columns.Add("PickList");
			table.Columns.Add("ItemOrder");
			foreach(ApptFieldDef apptFieldDef in listApptFieldDefs) {
				table.Rows.Add(new object[] {
					POut.Long  (apptFieldDef.ApptFieldDefNum),
					            apptFieldDef.FieldName,
					POut.Int   ((int)apptFieldDef.FieldType),
					            apptFieldDef.PickList,
					POut.Int   (apptFieldDef.ItemOrder),
				});
			}
			return table;
		}

		///<summary>Inserts one ApptFieldDef into the database.  Returns the new priKey.</summary>
		public static long Insert(ApptFieldDef apptFieldDef) {
			return Insert(apptFieldDef,false);
		}

		///<summary>Inserts one ApptFieldDef into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(ApptFieldDef apptFieldDef,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				apptFieldDef.ApptFieldDefNum=ReplicationServers.GetKey("apptfielddef","ApptFieldDefNum");
			}
			string command="INSERT INTO apptfielddef (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ApptFieldDefNum,";
			}
			command+="FieldName,FieldType,PickList,ItemOrder) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(apptFieldDef.ApptFieldDefNum)+",";
			}
			command+=
				 "'"+POut.String(apptFieldDef.FieldName)+"',"
				+    POut.Int   ((int)apptFieldDef.FieldType)+","
				+    DbHelper.ParamChar+"paramPickList,"
				+    POut.Int   (apptFieldDef.ItemOrder)+")";
			if(apptFieldDef.PickList==null) {
				apptFieldDef.PickList="";
			}
			OdSqlParameter paramPickList=new OdSqlParameter("paramPickList",OdDbType.Text,POut.StringParam(apptFieldDef.PickList));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramPickList);
			}
			else {
				apptFieldDef.ApptFieldDefNum=Db.NonQ(command,true,"ApptFieldDefNum","apptFieldDef",paramPickList);
			}
			return apptFieldDef.ApptFieldDefNum;
		}

		///<summary>Inserts one ApptFieldDef into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ApptFieldDef apptFieldDef) {
			return InsertNoCache(apptFieldDef,false);
		}

		///<summary>Inserts one ApptFieldDef into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(ApptFieldDef apptFieldDef,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO apptfielddef (";
			if(!useExistingPK && isRandomKeys) {
				apptFieldDef.ApptFieldDefNum=ReplicationServers.GetKeyNoCache("apptfielddef","ApptFieldDefNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="ApptFieldDefNum,";
			}
			command+="FieldName,FieldType,PickList,ItemOrder) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(apptFieldDef.ApptFieldDefNum)+",";
			}
			command+=
				 "'"+POut.String(apptFieldDef.FieldName)+"',"
				+    POut.Int   ((int)apptFieldDef.FieldType)+","
				+    DbHelper.ParamChar+"paramPickList,"
				+    POut.Int   (apptFieldDef.ItemOrder)+")";
			if(apptFieldDef.PickList==null) {
				apptFieldDef.PickList="";
			}
			OdSqlParameter paramPickList=new OdSqlParameter("paramPickList",OdDbType.Text,POut.StringParam(apptFieldDef.PickList));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramPickList);
			}
			else {
				apptFieldDef.ApptFieldDefNum=Db.NonQ(command,true,"ApptFieldDefNum","apptFieldDef",paramPickList);
			}
			return apptFieldDef.ApptFieldDefNum;
		}

		///<summary>Updates one ApptFieldDef in the database.</summary>
		public static void Update(ApptFieldDef apptFieldDef) {
			string command="UPDATE apptfielddef SET "
				+"FieldName      = '"+POut.String(apptFieldDef.FieldName)+"', "
				+"FieldType      =  "+POut.Int   ((int)apptFieldDef.FieldType)+", "
				+"PickList       =  "+DbHelper.ParamChar+"paramPickList, "
				+"ItemOrder      =  "+POut.Int   (apptFieldDef.ItemOrder)+" "
				+"WHERE ApptFieldDefNum = "+POut.Long(apptFieldDef.ApptFieldDefNum);
			if(apptFieldDef.PickList==null) {
				apptFieldDef.PickList="";
			}
			OdSqlParameter paramPickList=new OdSqlParameter("paramPickList",OdDbType.Text,POut.StringParam(apptFieldDef.PickList));
			Db.NonQ(command,paramPickList);
		}

		///<summary>Updates one ApptFieldDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(ApptFieldDef apptFieldDef,ApptFieldDef oldApptFieldDef) {
			string command="";
			if(apptFieldDef.FieldName != oldApptFieldDef.FieldName) {
				if(command!="") { command+=",";}
				command+="FieldName = '"+POut.String(apptFieldDef.FieldName)+"'";
			}
			if(apptFieldDef.FieldType != oldApptFieldDef.FieldType) {
				if(command!="") { command+=",";}
				command+="FieldType = "+POut.Int   ((int)apptFieldDef.FieldType)+"";
			}
			if(apptFieldDef.PickList != oldApptFieldDef.PickList) {
				if(command!="") { command+=",";}
				command+="PickList = "+DbHelper.ParamChar+"paramPickList";
			}
			if(apptFieldDef.ItemOrder != oldApptFieldDef.ItemOrder) {
				if(command!="") { command+=",";}
				command+="ItemOrder = "+POut.Int(apptFieldDef.ItemOrder)+"";
			}
			if(command=="") {
				return false;
			}
			if(apptFieldDef.PickList==null) {
				apptFieldDef.PickList="";
			}
			OdSqlParameter paramPickList=new OdSqlParameter("paramPickList",OdDbType.Text,POut.StringParam(apptFieldDef.PickList));
			command="UPDATE apptfielddef SET "+command
				+" WHERE ApptFieldDefNum = "+POut.Long(apptFieldDef.ApptFieldDefNum);
			Db.NonQ(command,paramPickList);
			return true;
		}

		///<summary>Returns true if Update(ApptFieldDef,ApptFieldDef) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(ApptFieldDef apptFieldDef,ApptFieldDef oldApptFieldDef) {
			if(apptFieldDef.FieldName != oldApptFieldDef.FieldName) {
				return true;
			}
			if(apptFieldDef.FieldType != oldApptFieldDef.FieldType) {
				return true;
			}
			if(apptFieldDef.PickList != oldApptFieldDef.PickList) {
				return true;
			}
			if(apptFieldDef.ItemOrder != oldApptFieldDef.ItemOrder) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one ApptFieldDef from the database.</summary>
		public static void Delete(long apptFieldDefNum) {
			string command="DELETE FROM apptfielddef "
				+"WHERE ApptFieldDefNum = "+POut.Long(apptFieldDefNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many ApptFieldDefs from the database.</summary>
		public static void DeleteMany(List<long> listApptFieldDefNums) {
			if(listApptFieldDefNums==null || listApptFieldDefNums.Count==0) {
				return;
			}
			string command="DELETE FROM apptfielddef "
				+"WHERE ApptFieldDefNum IN("+string.Join(",",listApptFieldDefNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<ApptFieldDef> listNew,List<ApptFieldDef> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<ApptFieldDef> listIns    =new List<ApptFieldDef>();
			List<ApptFieldDef> listUpdNew =new List<ApptFieldDef>();
			List<ApptFieldDef> listUpdDB  =new List<ApptFieldDef>();
			List<ApptFieldDef> listDel    =new List<ApptFieldDef>();
			listNew.Sort((ApptFieldDef x,ApptFieldDef y) => { return x.ApptFieldDefNum.CompareTo(y.ApptFieldDefNum); });
			listDB.Sort((ApptFieldDef x,ApptFieldDef y) => { return x.ApptFieldDefNum.CompareTo(y.ApptFieldDefNum); });
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			ApptFieldDef fieldNew;
			ApptFieldDef fieldDB;
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
				else if(fieldNew.ApptFieldDefNum<fieldDB.ApptFieldDefNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.ApptFieldDefNum>fieldDB.ApptFieldDefNum) {//dbPK less than newPK, dbItem is 'next'
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
			DeleteMany(listDel.Select(x => x.ApptFieldDefNum).ToList());
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}