//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OpenDentBusiness.Crud{
	public class BugSubmissionHashCrud {
		///<summary>Gets one BugSubmissionHash object from the database using the primary key.  Returns null if not found.</summary>
		public static BugSubmissionHash SelectOne(long bugSubmissionHashNum) {
			string command="SELECT * FROM bugsubmissionhash "
				+"WHERE BugSubmissionHashNum = "+POut.Long(bugSubmissionHashNum);
			List<BugSubmissionHash> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one BugSubmissionHash object from the database using a query.</summary>
		public static BugSubmissionHash SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<BugSubmissionHash> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of BugSubmissionHash objects from the database using a query.</summary>
		public static List<BugSubmissionHash> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<BugSubmissionHash> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<BugSubmissionHash> TableToList(DataTable table) {
			List<BugSubmissionHash> retVal=new List<BugSubmissionHash>();
			BugSubmissionHash bugSubmissionHash;
			foreach(DataRow row in table.Rows) {
				bugSubmissionHash=new BugSubmissionHash();
				bugSubmissionHash.BugSubmissionHashNum= PIn.Long  (row["BugSubmissionHashNum"].ToString());
				bugSubmissionHash.FullHash            = PIn.String(row["FullHash"].ToString());
				bugSubmissionHash.PartialHash         = PIn.String(row["PartialHash"].ToString());
				bugSubmissionHash.BugId               = PIn.Long  (row["BugId"].ToString());
				bugSubmissionHash.DateTimeModify      = PIn.DateT (row["DateTimeModify"].ToString());
				bugSubmissionHash.DateTimeEntry       = PIn.DateT (row["DateTimeEntry"].ToString());
				retVal.Add(bugSubmissionHash);
			}
			return retVal;
		}

		///<summary>Converts a list of BugSubmissionHash into a DataTable.</summary>
		public static DataTable ListToTable(List<BugSubmissionHash> listBugSubmissionHashs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="BugSubmissionHash";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("BugSubmissionHashNum");
			table.Columns.Add("FullHash");
			table.Columns.Add("PartialHash");
			table.Columns.Add("BugId");
			table.Columns.Add("DateTimeModify");
			table.Columns.Add("DateTimeEntry");
			foreach(BugSubmissionHash bugSubmissionHash in listBugSubmissionHashs) {
				table.Rows.Add(new object[] {
					POut.Long  (bugSubmissionHash.BugSubmissionHashNum),
					            bugSubmissionHash.FullHash,
					            bugSubmissionHash.PartialHash,
					POut.Long  (bugSubmissionHash.BugId),
					POut.DateT (bugSubmissionHash.DateTimeModify,false),
					POut.DateT (bugSubmissionHash.DateTimeEntry,false),
				});
			}
			return table;
		}

		///<summary>Inserts one BugSubmissionHash into the database.  Returns the new priKey.</summary>
		public static long Insert(BugSubmissionHash bugSubmissionHash) {
			return Insert(bugSubmissionHash,false);
		}

		///<summary>Inserts one BugSubmissionHash into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(BugSubmissionHash bugSubmissionHash,bool useExistingPK) {
			string command="INSERT INTO bugsubmissionhash (";
			if(useExistingPK) {
				command+="BugSubmissionHashNum,";
			}
			command+="FullHash,PartialHash,BugId,DateTimeEntry) VALUES(";
			if(useExistingPK) {
				command+=POut.Long(bugSubmissionHash.BugSubmissionHashNum)+",";
			}
			command+=
				 "'"+POut.String(bugSubmissionHash.FullHash)+"',"
				+"'"+POut.String(bugSubmissionHash.PartialHash)+"',"
				+    POut.Long  (bugSubmissionHash.BugId)+","
				//DateTimeModify can only be set by MySQL
				+    DbHelper.Now()+")";
			if(useExistingPK) {
				Db.NonQ(command);
			}
			else {
				bugSubmissionHash.BugSubmissionHashNum=Db.NonQ(command,true,"BugSubmissionHashNum","bugSubmissionHash");
			}
			return bugSubmissionHash.BugSubmissionHashNum;
		}

		///<summary>Inserts many BugSubmissionHashs into the database.</summary>
		public static void InsertMany(List<BugSubmissionHash> listBugSubmissionHashs) {
			InsertMany(listBugSubmissionHashs,false);
		}

		///<summary>Inserts many BugSubmissionHashs into the database.  Provides option to use the existing priKey.</summary>
		public static void InsertMany(List<BugSubmissionHash> listBugSubmissionHashs,bool useExistingPK) {
				StringBuilder sbCommands=null;
				int index=0;
				int countRows=0;
				while(index < listBugSubmissionHashs.Count) {
					BugSubmissionHash bugSubmissionHash=listBugSubmissionHashs[index];
					StringBuilder sbRow=new StringBuilder("(");
					bool hasComma=false;
					if(sbCommands==null) {
						sbCommands=new StringBuilder();
						sbCommands.Append("INSERT INTO bugsubmissionhash (");
						if(useExistingPK) {
							sbCommands.Append("BugSubmissionHashNum,");
						}
						sbCommands.Append("FullHash,PartialHash,BugId,DateTimeEntry) VALUES ");
						countRows=0;
					}
					else {
						hasComma=true;
					}
					if(useExistingPK) {
						sbRow.Append(POut.Long(bugSubmissionHash.BugSubmissionHashNum)); sbRow.Append(",");
					}
					sbRow.Append("'"+POut.String(bugSubmissionHash.FullHash)+"'"); sbRow.Append(",");
					sbRow.Append("'"+POut.String(bugSubmissionHash.PartialHash)+"'"); sbRow.Append(",");
					sbRow.Append(POut.Long(bugSubmissionHash.BugId)); sbRow.Append(",");
					//DateTimeModify can only be set by MySQL
					sbRow.Append(DbHelper.Now()); sbRow.Append(")");
					if(sbCommands.Length+sbRow.Length+1 > TableBase.MaxAllowedPacketCount && countRows > 0) {
						Db.NonQ(sbCommands.ToString());
						sbCommands=null;
					}
					else {
						if(hasComma) {
							sbCommands.Append(",");
						}
						sbCommands.Append(sbRow.ToString());
						countRows++;
						if(index==listBugSubmissionHashs.Count-1) {
							Db.NonQ(sbCommands.ToString());
						}
						index++;
					}
				}
		}

		///<summary>Inserts one BugSubmissionHash into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(BugSubmissionHash bugSubmissionHash) {
			return InsertNoCache(bugSubmissionHash,false);
		}

		///<summary>Inserts one BugSubmissionHash into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(BugSubmissionHash bugSubmissionHash,bool useExistingPK) {
			string command="INSERT INTO bugsubmissionhash (";
			if(useExistingPK) {
				command+="BugSubmissionHashNum,";
			}
			command+="FullHash,PartialHash,BugId,DateTimeEntry) VALUES(";
			if(useExistingPK) {
				command+=POut.Long(bugSubmissionHash.BugSubmissionHashNum)+",";
			}
			command+=
				 "'"+POut.String(bugSubmissionHash.FullHash)+"',"
				+"'"+POut.String(bugSubmissionHash.PartialHash)+"',"
				+    POut.Long  (bugSubmissionHash.BugId)+","
				//DateTimeModify can only be set by MySQL
				+    DbHelper.Now()+")";
			if(useExistingPK) {
				Db.NonQ(command);
			}
			else {
				bugSubmissionHash.BugSubmissionHashNum=Db.NonQ(command,true,"BugSubmissionHashNum","bugSubmissionHash");
			}
			return bugSubmissionHash.BugSubmissionHashNum;
		}

		///<summary>Updates one BugSubmissionHash in the database.</summary>
		public static void Update(BugSubmissionHash bugSubmissionHash) {
			string command="UPDATE bugsubmissionhash SET "
				+"FullHash            = '"+POut.String(bugSubmissionHash.FullHash)+"', "
				+"PartialHash         = '"+POut.String(bugSubmissionHash.PartialHash)+"', "
				+"BugId               =  "+POut.Long  (bugSubmissionHash.BugId)+" "
				//DateTimeModify can only be set by MySQL
				//DateTimeEntry not allowed to change
				+"WHERE BugSubmissionHashNum = "+POut.Long(bugSubmissionHash.BugSubmissionHashNum);
			Db.NonQ(command);
		}

		///<summary>Updates one BugSubmissionHash in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(BugSubmissionHash bugSubmissionHash,BugSubmissionHash oldBugSubmissionHash) {
			string command="";
			if(bugSubmissionHash.FullHash != oldBugSubmissionHash.FullHash) {
				if(command!="") { command+=",";}
				command+="FullHash = '"+POut.String(bugSubmissionHash.FullHash)+"'";
			}
			if(bugSubmissionHash.PartialHash != oldBugSubmissionHash.PartialHash) {
				if(command!="") { command+=",";}
				command+="PartialHash = '"+POut.String(bugSubmissionHash.PartialHash)+"'";
			}
			if(bugSubmissionHash.BugId != oldBugSubmissionHash.BugId) {
				if(command!="") { command+=",";}
				command+="BugId = "+POut.Long(bugSubmissionHash.BugId)+"";
			}
			//DateTimeModify can only be set by MySQL
			//DateTimeEntry not allowed to change
			if(command=="") {
				return false;
			}
			command="UPDATE bugsubmissionhash SET "+command
				+" WHERE BugSubmissionHashNum = "+POut.Long(bugSubmissionHash.BugSubmissionHashNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(BugSubmissionHash,BugSubmissionHash) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(BugSubmissionHash bugSubmissionHash,BugSubmissionHash oldBugSubmissionHash) {
			if(bugSubmissionHash.FullHash != oldBugSubmissionHash.FullHash) {
				return true;
			}
			if(bugSubmissionHash.PartialHash != oldBugSubmissionHash.PartialHash) {
				return true;
			}
			if(bugSubmissionHash.BugId != oldBugSubmissionHash.BugId) {
				return true;
			}
			//DateTimeModify can only be set by MySQL
			//DateTimeEntry not allowed to change
			return false;
		}

		///<summary>Deletes one BugSubmissionHash from the database.</summary>
		public static void Delete(long bugSubmissionHashNum) {
			string command="DELETE FROM bugsubmissionhash "
				+"WHERE BugSubmissionHashNum = "+POut.Long(bugSubmissionHashNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many BugSubmissionHashs from the database.</summary>
		public static void DeleteMany(List<long> listBugSubmissionHashNums) {
			if(listBugSubmissionHashNums==null || listBugSubmissionHashNums.Count==0) {
				return;
			}
			string command="DELETE FROM bugsubmissionhash "
				+"WHERE BugSubmissionHashNum IN("+string.Join(",",listBugSubmissionHashNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}