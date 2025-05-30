//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class InsBlueBookCrud {
		///<summary>Gets one InsBlueBook object from the database using the primary key.  Returns null if not found.</summary>
		public static InsBlueBook SelectOne(long insBlueBookNum) {
			string command="SELECT * FROM insbluebook "
				+"WHERE InsBlueBookNum = "+POut.Long(insBlueBookNum);
			List<InsBlueBook> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one InsBlueBook object from the database using a query.</summary>
		public static InsBlueBook SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<InsBlueBook> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of InsBlueBook objects from the database using a query.</summary>
		public static List<InsBlueBook> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<InsBlueBook> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<InsBlueBook> TableToList(DataTable table) {
			List<InsBlueBook> retVal=new List<InsBlueBook>();
			InsBlueBook insBlueBook;
			foreach(DataRow row in table.Rows) {
				insBlueBook=new InsBlueBook();
				insBlueBook.InsBlueBookNum = PIn.Long  (row["InsBlueBookNum"].ToString());
				insBlueBook.ProcCodeNum    = PIn.Long  (row["ProcCodeNum"].ToString());
				insBlueBook.CarrierNum     = PIn.Long  (row["CarrierNum"].ToString());
				insBlueBook.PlanNum        = PIn.Long  (row["PlanNum"].ToString());
				insBlueBook.GroupNum       = PIn.String(row["GroupNum"].ToString());
				insBlueBook.InsPayAmt      = PIn.Double(row["InsPayAmt"].ToString());
				insBlueBook.AllowedOverride= PIn.Double(row["AllowedOverride"].ToString());
				insBlueBook.DateTEntry     = PIn.DateT (row["DateTEntry"].ToString());
				insBlueBook.ProcNum        = PIn.Long  (row["ProcNum"].ToString());
				insBlueBook.ProcDate       = PIn.Date  (row["ProcDate"].ToString());
				insBlueBook.ClaimType      = PIn.String(row["ClaimType"].ToString());
				insBlueBook.ClaimNum       = PIn.Long  (row["ClaimNum"].ToString());
				retVal.Add(insBlueBook);
			}
			return retVal;
		}

		///<summary>Converts a list of InsBlueBook into a DataTable.</summary>
		public static DataTable ListToTable(List<InsBlueBook> listInsBlueBooks,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="InsBlueBook";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("InsBlueBookNum");
			table.Columns.Add("ProcCodeNum");
			table.Columns.Add("CarrierNum");
			table.Columns.Add("PlanNum");
			table.Columns.Add("GroupNum");
			table.Columns.Add("InsPayAmt");
			table.Columns.Add("AllowedOverride");
			table.Columns.Add("DateTEntry");
			table.Columns.Add("ProcNum");
			table.Columns.Add("ProcDate");
			table.Columns.Add("ClaimType");
			table.Columns.Add("ClaimNum");
			foreach(InsBlueBook insBlueBook in listInsBlueBooks) {
				table.Rows.Add(new object[] {
					POut.Long  (insBlueBook.InsBlueBookNum),
					POut.Long  (insBlueBook.ProcCodeNum),
					POut.Long  (insBlueBook.CarrierNum),
					POut.Long  (insBlueBook.PlanNum),
					            insBlueBook.GroupNum,
					POut.Double(insBlueBook.InsPayAmt),
					POut.Double(insBlueBook.AllowedOverride),
					POut.DateT (insBlueBook.DateTEntry,false),
					POut.Long  (insBlueBook.ProcNum),
					POut.DateT (insBlueBook.ProcDate,false),
					            insBlueBook.ClaimType,
					POut.Long  (insBlueBook.ClaimNum),
				});
			}
			return table;
		}

		///<summary>Inserts one InsBlueBook into the database.  Returns the new priKey.</summary>
		public static long Insert(InsBlueBook insBlueBook) {
			return Insert(insBlueBook,false);
		}

		///<summary>Inserts one InsBlueBook into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(InsBlueBook insBlueBook,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				insBlueBook.InsBlueBookNum=ReplicationServers.GetKey("insbluebook","InsBlueBookNum");
			}
			string command="INSERT INTO insbluebook (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="InsBlueBookNum,";
			}
			command+="ProcCodeNum,CarrierNum,PlanNum,GroupNum,InsPayAmt,AllowedOverride,DateTEntry,ProcNum,ProcDate,ClaimType,ClaimNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(insBlueBook.InsBlueBookNum)+",";
			}
			command+=
				     POut.Long  (insBlueBook.ProcCodeNum)+","
				+    POut.Long  (insBlueBook.CarrierNum)+","
				+    POut.Long  (insBlueBook.PlanNum)+","
				+"'"+POut.String(insBlueBook.GroupNum)+"',"
				+		 POut.Double(insBlueBook.InsPayAmt)+","
				+		 POut.Double(insBlueBook.AllowedOverride)+","
				+    DbHelper.Now()+","
				+    POut.Long  (insBlueBook.ProcNum)+","
				+    POut.Date  (insBlueBook.ProcDate)+","
				+"'"+POut.String(insBlueBook.ClaimType)+"',"
				+    POut.Long  (insBlueBook.ClaimNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				insBlueBook.InsBlueBookNum=Db.NonQ(command,true,"InsBlueBookNum","insBlueBook");
			}
			return insBlueBook.InsBlueBookNum;
		}

		///<summary>Inserts one InsBlueBook into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(InsBlueBook insBlueBook) {
			return InsertNoCache(insBlueBook,false);
		}

		///<summary>Inserts one InsBlueBook into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(InsBlueBook insBlueBook,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO insbluebook (";
			if(!useExistingPK && isRandomKeys) {
				insBlueBook.InsBlueBookNum=ReplicationServers.GetKeyNoCache("insbluebook","InsBlueBookNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="InsBlueBookNum,";
			}
			command+="ProcCodeNum,CarrierNum,PlanNum,GroupNum,InsPayAmt,AllowedOverride,DateTEntry,ProcNum,ProcDate,ClaimType,ClaimNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(insBlueBook.InsBlueBookNum)+",";
			}
			command+=
				     POut.Long  (insBlueBook.ProcCodeNum)+","
				+    POut.Long  (insBlueBook.CarrierNum)+","
				+    POut.Long  (insBlueBook.PlanNum)+","
				+"'"+POut.String(insBlueBook.GroupNum)+"',"
				+	   POut.Double(insBlueBook.InsPayAmt)+","
				+	   POut.Double(insBlueBook.AllowedOverride)+","
				+    DbHelper.Now()+","
				+    POut.Long  (insBlueBook.ProcNum)+","
				+    POut.Date  (insBlueBook.ProcDate)+","
				+"'"+POut.String(insBlueBook.ClaimType)+"',"
				+    POut.Long  (insBlueBook.ClaimNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				insBlueBook.InsBlueBookNum=Db.NonQ(command,true,"InsBlueBookNum","insBlueBook");
			}
			return insBlueBook.InsBlueBookNum;
		}

		///<summary>Updates one InsBlueBook in the database.</summary>
		public static void Update(InsBlueBook insBlueBook) {
			string command="UPDATE insbluebook SET "
				+"ProcCodeNum    =  "+POut.Long  (insBlueBook.ProcCodeNum)+", "
				+"CarrierNum     =  "+POut.Long  (insBlueBook.CarrierNum)+", "
				+"PlanNum        =  "+POut.Long  (insBlueBook.PlanNum)+", "
				+"GroupNum       = '"+POut.String(insBlueBook.GroupNum)+"', "
				+"InsPayAmt      =  "+POut.Double(insBlueBook.InsPayAmt)+", "
				+"AllowedOverride=  "+POut.Double(insBlueBook.AllowedOverride)+", "
				//DateTEntry not allowed to change
				+"ProcNum        =  "+POut.Long  (insBlueBook.ProcNum)+", "
				+"ProcDate       =  "+POut.Date  (insBlueBook.ProcDate)+", "
				+"ClaimType      = '"+POut.String(insBlueBook.ClaimType)+"', "
				+"ClaimNum       =  "+POut.Long  (insBlueBook.ClaimNum)+" "
				+"WHERE InsBlueBookNum = "+POut.Long(insBlueBook.InsBlueBookNum);
			Db.NonQ(command);
		}

		///<summary>Updates one InsBlueBook in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(InsBlueBook insBlueBook,InsBlueBook oldInsBlueBook) {
			string command="";
			if(insBlueBook.ProcCodeNum != oldInsBlueBook.ProcCodeNum) {
				if(command!="") { command+=",";}
				command+="ProcCodeNum = "+POut.Long(insBlueBook.ProcCodeNum)+"";
			}
			if(insBlueBook.CarrierNum != oldInsBlueBook.CarrierNum) {
				if(command!="") { command+=",";}
				command+="CarrierNum = "+POut.Long(insBlueBook.CarrierNum)+"";
			}
			if(insBlueBook.PlanNum != oldInsBlueBook.PlanNum) {
				if(command!="") { command+=",";}
				command+="PlanNum = "+POut.Long(insBlueBook.PlanNum)+"";
			}
			if(insBlueBook.GroupNum != oldInsBlueBook.GroupNum) {
				if(command!="") { command+=",";}
				command+="GroupNum = '"+POut.String(insBlueBook.GroupNum)+"'";
			}
			if(insBlueBook.InsPayAmt != oldInsBlueBook.InsPayAmt) {
				if(command!="") { command+=",";}
				command+="InsPayAmt = "+POut.Double(insBlueBook.InsPayAmt)+"";
			}
			if(insBlueBook.AllowedOverride != oldInsBlueBook.AllowedOverride) {
				if(command!="") { command+=",";}
				command+="AllowedOverride = "+POut.Double(insBlueBook.AllowedOverride)+"";
			}
			//DateTEntry not allowed to change
			if(insBlueBook.ProcNum != oldInsBlueBook.ProcNum) {
				if(command!="") { command+=",";}
				command+="ProcNum = "+POut.Long(insBlueBook.ProcNum)+"";
			}
			if(insBlueBook.ProcDate.Date != oldInsBlueBook.ProcDate.Date) {
				if(command!="") { command+=",";}
				command+="ProcDate = "+POut.Date(insBlueBook.ProcDate)+"";
			}
			if(insBlueBook.ClaimType != oldInsBlueBook.ClaimType) {
				if(command!="") { command+=",";}
				command+="ClaimType = '"+POut.String(insBlueBook.ClaimType)+"'";
			}
			if(insBlueBook.ClaimNum != oldInsBlueBook.ClaimNum) {
				if(command!="") { command+=",";}
				command+="ClaimNum = "+POut.Long(insBlueBook.ClaimNum)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE insbluebook SET "+command
				+" WHERE InsBlueBookNum = "+POut.Long(insBlueBook.InsBlueBookNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(InsBlueBook,InsBlueBook) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(InsBlueBook insBlueBook,InsBlueBook oldInsBlueBook) {
			if(insBlueBook.ProcCodeNum != oldInsBlueBook.ProcCodeNum) {
				return true;
			}
			if(insBlueBook.CarrierNum != oldInsBlueBook.CarrierNum) {
				return true;
			}
			if(insBlueBook.PlanNum != oldInsBlueBook.PlanNum) {
				return true;
			}
			if(insBlueBook.GroupNum != oldInsBlueBook.GroupNum) {
				return true;
			}
			if(insBlueBook.InsPayAmt != oldInsBlueBook.InsPayAmt) {
				return true;
			}
			if(insBlueBook.AllowedOverride != oldInsBlueBook.AllowedOverride) {
				return true;
			}
			//DateTEntry not allowed to change
			if(insBlueBook.ProcNum != oldInsBlueBook.ProcNum) {
				return true;
			}
			if(insBlueBook.ProcDate.Date != oldInsBlueBook.ProcDate.Date) {
				return true;
			}
			if(insBlueBook.ClaimType != oldInsBlueBook.ClaimType) {
				return true;
			}
			if(insBlueBook.ClaimNum != oldInsBlueBook.ClaimNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one InsBlueBook from the database.</summary>
		public static void Delete(long insBlueBookNum) {
			string command="DELETE FROM insbluebook "
				+"WHERE InsBlueBookNum = "+POut.Long(insBlueBookNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many InsBlueBooks from the database.</summary>
		public static void DeleteMany(List<long> listInsBlueBookNums) {
			if(listInsBlueBookNums==null || listInsBlueBookNums.Count==0) {
				return;
			}
			string command="DELETE FROM insbluebook "
				+"WHERE InsBlueBookNum IN("+string.Join(",",listInsBlueBookNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<InsBlueBook> listNew,List<InsBlueBook> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<InsBlueBook> listIns    =new List<InsBlueBook>();
			List<InsBlueBook> listUpdNew =new List<InsBlueBook>();
			List<InsBlueBook> listUpdDB  =new List<InsBlueBook>();
			List<InsBlueBook> listDel    =new List<InsBlueBook>();
			listNew.Sort((InsBlueBook x,InsBlueBook y) => { return x.InsBlueBookNum.CompareTo(y.InsBlueBookNum); });
			listDB.Sort((InsBlueBook x,InsBlueBook y) => { return x.InsBlueBookNum.CompareTo(y.InsBlueBookNum); });
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			InsBlueBook fieldNew;
			InsBlueBook fieldDB;
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
				else if(fieldNew.InsBlueBookNum<fieldDB.InsBlueBookNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.InsBlueBookNum>fieldDB.InsBlueBookNum) {//dbPK less than newPK, dbItem is 'next'
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
			DeleteMany(listDel.Select(x => x.InsBlueBookNum).ToList());
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}