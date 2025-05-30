//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class EClipboardImageCaptureDefCrud {
		///<summary>Gets one EClipboardImageCaptureDef object from the database using the primary key.  Returns null if not found.</summary>
		public static EClipboardImageCaptureDef SelectOne(long eClipboardImageCaptureDefNum) {
			string command="SELECT * FROM eclipboardimagecapturedef "
				+"WHERE EClipboardImageCaptureDefNum = "+POut.Long(eClipboardImageCaptureDefNum);
			List<EClipboardImageCaptureDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EClipboardImageCaptureDef object from the database using a query.</summary>
		public static EClipboardImageCaptureDef SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EClipboardImageCaptureDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EClipboardImageCaptureDef objects from the database using a query.</summary>
		public static List<EClipboardImageCaptureDef> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EClipboardImageCaptureDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EClipboardImageCaptureDef> TableToList(DataTable table) {
			List<EClipboardImageCaptureDef> retVal=new List<EClipboardImageCaptureDef>();
			EClipboardImageCaptureDef eClipboardImageCaptureDef;
			foreach(DataRow row in table.Rows) {
				eClipboardImageCaptureDef=new EClipboardImageCaptureDef();
				eClipboardImageCaptureDef.EClipboardImageCaptureDefNum= PIn.Long  (row["EClipboardImageCaptureDefNum"].ToString());
				eClipboardImageCaptureDef.DefNum                      = PIn.Long  (row["DefNum"].ToString());
				eClipboardImageCaptureDef.IsSelfPortrait              = PIn.Bool  (row["IsSelfPortrait"].ToString());
				eClipboardImageCaptureDef.FrequencyDays               = PIn.Int   (row["FrequencyDays"].ToString());
				eClipboardImageCaptureDef.ClinicNum                   = PIn.Long  (row["ClinicNum"].ToString());
				eClipboardImageCaptureDef.OcrCaptureType              = (OpenDentBusiness.EnumOcrCaptureType)PIn.Int(row["OcrCaptureType"].ToString());
				eClipboardImageCaptureDef.Frequency                   = (OpenDentBusiness.EnumEClipFreq)PIn.Int(row["Frequency"].ToString());
				eClipboardImageCaptureDef.ResubmitInterval            = TimeSpan.FromTicks(PIn.Long(row["ResubmitInterval"].ToString()));
				retVal.Add(eClipboardImageCaptureDef);
			}
			return retVal;
		}

		///<summary>Converts a list of EClipboardImageCaptureDef into a DataTable.</summary>
		public static DataTable ListToTable(List<EClipboardImageCaptureDef> listEClipboardImageCaptureDefs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EClipboardImageCaptureDef";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EClipboardImageCaptureDefNum");
			table.Columns.Add("DefNum");
			table.Columns.Add("IsSelfPortrait");
			table.Columns.Add("FrequencyDays");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("OcrCaptureType");
			table.Columns.Add("Frequency");
			table.Columns.Add("ResubmitInterval");
			foreach(EClipboardImageCaptureDef eClipboardImageCaptureDef in listEClipboardImageCaptureDefs) {
				table.Rows.Add(new object[] {
					POut.Long  (eClipboardImageCaptureDef.EClipboardImageCaptureDefNum),
					POut.Long  (eClipboardImageCaptureDef.DefNum),
					POut.Bool  (eClipboardImageCaptureDef.IsSelfPortrait),
					POut.Int   (eClipboardImageCaptureDef.FrequencyDays),
					POut.Long  (eClipboardImageCaptureDef.ClinicNum),
					POut.Int   ((int)eClipboardImageCaptureDef.OcrCaptureType),
					POut.Int   ((int)eClipboardImageCaptureDef.Frequency),
					POut.Long (eClipboardImageCaptureDef.ResubmitInterval.Ticks),
				});
			}
			return table;
		}

		///<summary>Inserts one EClipboardImageCaptureDef into the database.  Returns the new priKey.</summary>
		public static long Insert(EClipboardImageCaptureDef eClipboardImageCaptureDef) {
			return Insert(eClipboardImageCaptureDef,false);
		}

		///<summary>Inserts one EClipboardImageCaptureDef into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EClipboardImageCaptureDef eClipboardImageCaptureDef,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				eClipboardImageCaptureDef.EClipboardImageCaptureDefNum=ReplicationServers.GetKey("eclipboardimagecapturedef","EClipboardImageCaptureDefNum");
			}
			string command="INSERT INTO eclipboardimagecapturedef (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EClipboardImageCaptureDefNum,";
			}
			command+="DefNum,IsSelfPortrait,FrequencyDays,ClinicNum,OcrCaptureType,Frequency,ResubmitInterval) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(eClipboardImageCaptureDef.EClipboardImageCaptureDefNum)+",";
			}
			command+=
				     POut.Long  (eClipboardImageCaptureDef.DefNum)+","
				+    POut.Bool  (eClipboardImageCaptureDef.IsSelfPortrait)+","
				+    POut.Int   (eClipboardImageCaptureDef.FrequencyDays)+","
				+    POut.Long  (eClipboardImageCaptureDef.ClinicNum)+","
				+    POut.Int   ((int)eClipboardImageCaptureDef.OcrCaptureType)+","
				+    POut.Int   ((int)eClipboardImageCaptureDef.Frequency)+","
				+"'"+POut.Long  (eClipboardImageCaptureDef.ResubmitInterval.Ticks)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				eClipboardImageCaptureDef.EClipboardImageCaptureDefNum=Db.NonQ(command,true,"EClipboardImageCaptureDefNum","eClipboardImageCaptureDef");
			}
			return eClipboardImageCaptureDef.EClipboardImageCaptureDefNum;
		}

		///<summary>Inserts one EClipboardImageCaptureDef into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EClipboardImageCaptureDef eClipboardImageCaptureDef) {
			return InsertNoCache(eClipboardImageCaptureDef,false);
		}

		///<summary>Inserts one EClipboardImageCaptureDef into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EClipboardImageCaptureDef eClipboardImageCaptureDef,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO eclipboardimagecapturedef (";
			if(!useExistingPK && isRandomKeys) {
				eClipboardImageCaptureDef.EClipboardImageCaptureDefNum=ReplicationServers.GetKeyNoCache("eclipboardimagecapturedef","EClipboardImageCaptureDefNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EClipboardImageCaptureDefNum,";
			}
			command+="DefNum,IsSelfPortrait,FrequencyDays,ClinicNum,OcrCaptureType,Frequency,ResubmitInterval) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(eClipboardImageCaptureDef.EClipboardImageCaptureDefNum)+",";
			}
			command+=
				     POut.Long  (eClipboardImageCaptureDef.DefNum)+","
				+    POut.Bool  (eClipboardImageCaptureDef.IsSelfPortrait)+","
				+    POut.Int   (eClipboardImageCaptureDef.FrequencyDays)+","
				+    POut.Long  (eClipboardImageCaptureDef.ClinicNum)+","
				+    POut.Int   ((int)eClipboardImageCaptureDef.OcrCaptureType)+","
				+    POut.Int   ((int)eClipboardImageCaptureDef.Frequency)+","
				+"'"+POut.Long(eClipboardImageCaptureDef.ResubmitInterval.Ticks)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				eClipboardImageCaptureDef.EClipboardImageCaptureDefNum=Db.NonQ(command,true,"EClipboardImageCaptureDefNum","eClipboardImageCaptureDef");
			}
			return eClipboardImageCaptureDef.EClipboardImageCaptureDefNum;
		}

		///<summary>Updates one EClipboardImageCaptureDef in the database.</summary>
		public static void Update(EClipboardImageCaptureDef eClipboardImageCaptureDef) {
			string command="UPDATE eclipboardimagecapturedef SET "
				+"DefNum                      =  "+POut.Long  (eClipboardImageCaptureDef.DefNum)+", "
				+"IsSelfPortrait              =  "+POut.Bool  (eClipboardImageCaptureDef.IsSelfPortrait)+", "
				+"FrequencyDays               =  "+POut.Int   (eClipboardImageCaptureDef.FrequencyDays)+", "
				+"ClinicNum                   =  "+POut.Long  (eClipboardImageCaptureDef.ClinicNum)+", "
				+"OcrCaptureType              =  "+POut.Int   ((int)eClipboardImageCaptureDef.OcrCaptureType)+", "
				+"Frequency                   =  "+POut.Int   ((int)eClipboardImageCaptureDef.Frequency)+", "
				+"ResubmitInterval            =  "+POut.Long  (eClipboardImageCaptureDef.ResubmitInterval.Ticks)+" "
				+"WHERE EClipboardImageCaptureDefNum = "+POut.Long(eClipboardImageCaptureDef.EClipboardImageCaptureDefNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EClipboardImageCaptureDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EClipboardImageCaptureDef eClipboardImageCaptureDef,EClipboardImageCaptureDef oldEClipboardImageCaptureDef) {
			string command="";
			if(eClipboardImageCaptureDef.DefNum != oldEClipboardImageCaptureDef.DefNum) {
				if(command!="") { command+=",";}
				command+="DefNum = "+POut.Long(eClipboardImageCaptureDef.DefNum)+"";
			}
			if(eClipboardImageCaptureDef.IsSelfPortrait != oldEClipboardImageCaptureDef.IsSelfPortrait) {
				if(command!="") { command+=",";}
				command+="IsSelfPortrait = "+POut.Bool(eClipboardImageCaptureDef.IsSelfPortrait)+"";
			}
			if(eClipboardImageCaptureDef.FrequencyDays != oldEClipboardImageCaptureDef.FrequencyDays) {
				if(command!="") { command+=",";}
				command+="FrequencyDays = "+POut.Int(eClipboardImageCaptureDef.FrequencyDays)+"";
			}
			if(eClipboardImageCaptureDef.ClinicNum != oldEClipboardImageCaptureDef.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(eClipboardImageCaptureDef.ClinicNum)+"";
			}
			if(eClipboardImageCaptureDef.OcrCaptureType != oldEClipboardImageCaptureDef.OcrCaptureType) {
				if(command!="") { command+=",";}
				command+="OcrCaptureType = "+POut.Int   ((int)eClipboardImageCaptureDef.OcrCaptureType)+"";
			}
			if(eClipboardImageCaptureDef.Frequency != oldEClipboardImageCaptureDef.Frequency) {
				if(command!="") { command+=",";}
				command+="Frequency = "+POut.Int   ((int)eClipboardImageCaptureDef.Frequency)+"";
			}
			if(eClipboardImageCaptureDef.ResubmitInterval != oldEClipboardImageCaptureDef.ResubmitInterval) {
				if(command!="") { command+=",";}
				command+="ResubmitInterval = '"+POut.Long  (eClipboardImageCaptureDef.ResubmitInterval.Ticks)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE eclipboardimagecapturedef SET "+command
				+" WHERE EClipboardImageCaptureDefNum = "+POut.Long(eClipboardImageCaptureDef.EClipboardImageCaptureDefNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(EClipboardImageCaptureDef,EClipboardImageCaptureDef) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EClipboardImageCaptureDef eClipboardImageCaptureDef,EClipboardImageCaptureDef oldEClipboardImageCaptureDef) {
			if(eClipboardImageCaptureDef.DefNum != oldEClipboardImageCaptureDef.DefNum) {
				return true;
			}
			if(eClipboardImageCaptureDef.IsSelfPortrait != oldEClipboardImageCaptureDef.IsSelfPortrait) {
				return true;
			}
			if(eClipboardImageCaptureDef.FrequencyDays != oldEClipboardImageCaptureDef.FrequencyDays) {
				return true;
			}
			if(eClipboardImageCaptureDef.ClinicNum != oldEClipboardImageCaptureDef.ClinicNum) {
				return true;
			}
			if(eClipboardImageCaptureDef.OcrCaptureType != oldEClipboardImageCaptureDef.OcrCaptureType) {
				return true;
			}
			if(eClipboardImageCaptureDef.Frequency != oldEClipboardImageCaptureDef.Frequency) {
				return true;
			}
			if(eClipboardImageCaptureDef.ResubmitInterval != oldEClipboardImageCaptureDef.ResubmitInterval) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EClipboardImageCaptureDef from the database.</summary>
		public static void Delete(long eClipboardImageCaptureDefNum) {
			string command="DELETE FROM eclipboardimagecapturedef "
				+"WHERE EClipboardImageCaptureDefNum = "+POut.Long(eClipboardImageCaptureDefNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many EClipboardImageCaptureDefs from the database.</summary>
		public static void DeleteMany(List<long> listEClipboardImageCaptureDefNums) {
			if(listEClipboardImageCaptureDefNums==null || listEClipboardImageCaptureDefNums.Count==0) {
				return;
			}
			string command="DELETE FROM eclipboardimagecapturedef "
				+"WHERE EClipboardImageCaptureDefNum IN("+string.Join(",",listEClipboardImageCaptureDefNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<EClipboardImageCaptureDef> listNew,List<EClipboardImageCaptureDef> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<EClipboardImageCaptureDef> listIns    =new List<EClipboardImageCaptureDef>();
			List<EClipboardImageCaptureDef> listUpdNew =new List<EClipboardImageCaptureDef>();
			List<EClipboardImageCaptureDef> listUpdDB  =new List<EClipboardImageCaptureDef>();
			List<EClipboardImageCaptureDef> listDel    =new List<EClipboardImageCaptureDef>();
			listNew.Sort((EClipboardImageCaptureDef x,EClipboardImageCaptureDef y) => { return x.EClipboardImageCaptureDefNum.CompareTo(y.EClipboardImageCaptureDefNum); });
			listDB.Sort((EClipboardImageCaptureDef x,EClipboardImageCaptureDef y) => { return x.EClipboardImageCaptureDefNum.CompareTo(y.EClipboardImageCaptureDefNum); });
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			EClipboardImageCaptureDef fieldNew;
			EClipboardImageCaptureDef fieldDB;
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
				else if(fieldNew.EClipboardImageCaptureDefNum<fieldDB.EClipboardImageCaptureDefNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.EClipboardImageCaptureDefNum>fieldDB.EClipboardImageCaptureDefNum) {//dbPK less than newPK, dbItem is 'next'
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
			DeleteMany(listDel.Select(x => x.EClipboardImageCaptureDefNum).ToList());
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}