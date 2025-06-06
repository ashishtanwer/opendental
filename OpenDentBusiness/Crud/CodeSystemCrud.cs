//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class CodeSystemCrud {
		///<summary>Gets one CodeSystem object from the database using the primary key.  Returns null if not found.</summary>
		public static CodeSystem SelectOne(long codeSystemNum) {
			string command="SELECT * FROM codesystem "
				+"WHERE CodeSystemNum = "+POut.Long(codeSystemNum);
			List<CodeSystem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one CodeSystem object from the database using a query.</summary>
		public static CodeSystem SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<CodeSystem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of CodeSystem objects from the database using a query.</summary>
		public static List<CodeSystem> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<CodeSystem> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<CodeSystem> TableToList(DataTable table) {
			List<CodeSystem> retVal=new List<CodeSystem>();
			CodeSystem codeSystem;
			foreach(DataRow row in table.Rows) {
				codeSystem=new CodeSystem();
				codeSystem.CodeSystemNum = PIn.Long  (row["CodeSystemNum"].ToString());
				codeSystem.CodeSystemName= PIn.String(row["CodeSystemName"].ToString());
				codeSystem.VersionCur    = PIn.String(row["VersionCur"].ToString());
				codeSystem.VersionAvail  = PIn.String(row["VersionAvail"].ToString());
				codeSystem.HL7OID        = PIn.String(row["HL7OID"].ToString());
				codeSystem.Note          = PIn.String(row["Note"].ToString());
				retVal.Add(codeSystem);
			}
			return retVal;
		}

		///<summary>Converts a list of CodeSystem into a DataTable.</summary>
		public static DataTable ListToTable(List<CodeSystem> listCodeSystems,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="CodeSystem";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("CodeSystemNum");
			table.Columns.Add("CodeSystemName");
			table.Columns.Add("VersionCur");
			table.Columns.Add("VersionAvail");
			table.Columns.Add("HL7OID");
			table.Columns.Add("Note");
			foreach(CodeSystem codeSystem in listCodeSystems) {
				table.Rows.Add(new object[] {
					POut.Long  (codeSystem.CodeSystemNum),
					            codeSystem.CodeSystemName,
					            codeSystem.VersionCur,
					            codeSystem.VersionAvail,
					            codeSystem.HL7OID,
					            codeSystem.Note,
				});
			}
			return table;
		}

		///<summary>Inserts one CodeSystem into the database.  Returns the new priKey.</summary>
		public static long Insert(CodeSystem codeSystem) {
			return Insert(codeSystem,false);
		}

		///<summary>Inserts one CodeSystem into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(CodeSystem codeSystem,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				codeSystem.CodeSystemNum=ReplicationServers.GetKey("codesystem","CodeSystemNum");
			}
			string command="INSERT INTO codesystem (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="CodeSystemNum,";
			}
			command+="CodeSystemName,VersionCur,VersionAvail,HL7OID,Note) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(codeSystem.CodeSystemNum)+",";
			}
			command+=
				 "'"+POut.String(codeSystem.CodeSystemName)+"',"
				+"'"+POut.String(codeSystem.VersionCur)+"',"
				+"'"+POut.String(codeSystem.VersionAvail)+"',"
				+"'"+POut.String(codeSystem.HL7OID)+"',"
				+"'"+POut.String(codeSystem.Note)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				codeSystem.CodeSystemNum=Db.NonQ(command,true,"CodeSystemNum","codeSystem");
			}
			return codeSystem.CodeSystemNum;
		}

		///<summary>Inserts one CodeSystem into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(CodeSystem codeSystem) {
			return InsertNoCache(codeSystem,false);
		}

		///<summary>Inserts one CodeSystem into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(CodeSystem codeSystem,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO codesystem (";
			if(!useExistingPK && isRandomKeys) {
				codeSystem.CodeSystemNum=ReplicationServers.GetKeyNoCache("codesystem","CodeSystemNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="CodeSystemNum,";
			}
			command+="CodeSystemName,VersionCur,VersionAvail,HL7OID,Note) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(codeSystem.CodeSystemNum)+",";
			}
			command+=
				 "'"+POut.String(codeSystem.CodeSystemName)+"',"
				+"'"+POut.String(codeSystem.VersionCur)+"',"
				+"'"+POut.String(codeSystem.VersionAvail)+"',"
				+"'"+POut.String(codeSystem.HL7OID)+"',"
				+"'"+POut.String(codeSystem.Note)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				codeSystem.CodeSystemNum=Db.NonQ(command,true,"CodeSystemNum","codeSystem");
			}
			return codeSystem.CodeSystemNum;
		}

		///<summary>Updates one CodeSystem in the database.</summary>
		public static void Update(CodeSystem codeSystem) {
			string command="UPDATE codesystem SET "
				+"CodeSystemName= '"+POut.String(codeSystem.CodeSystemName)+"', "
				+"VersionCur    = '"+POut.String(codeSystem.VersionCur)+"', "
				+"VersionAvail  = '"+POut.String(codeSystem.VersionAvail)+"', "
				+"HL7OID        = '"+POut.String(codeSystem.HL7OID)+"', "
				+"Note          = '"+POut.String(codeSystem.Note)+"' "
				+"WHERE CodeSystemNum = "+POut.Long(codeSystem.CodeSystemNum);
			Db.NonQ(command);
		}

		///<summary>Updates one CodeSystem in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(CodeSystem codeSystem,CodeSystem oldCodeSystem) {
			string command="";
			if(codeSystem.CodeSystemName != oldCodeSystem.CodeSystemName) {
				if(command!="") { command+=",";}
				command+="CodeSystemName = '"+POut.String(codeSystem.CodeSystemName)+"'";
			}
			if(codeSystem.VersionCur != oldCodeSystem.VersionCur) {
				if(command!="") { command+=",";}
				command+="VersionCur = '"+POut.String(codeSystem.VersionCur)+"'";
			}
			if(codeSystem.VersionAvail != oldCodeSystem.VersionAvail) {
				if(command!="") { command+=",";}
				command+="VersionAvail = '"+POut.String(codeSystem.VersionAvail)+"'";
			}
			if(codeSystem.HL7OID != oldCodeSystem.HL7OID) {
				if(command!="") { command+=",";}
				command+="HL7OID = '"+POut.String(codeSystem.HL7OID)+"'";
			}
			if(codeSystem.Note != oldCodeSystem.Note) {
				if(command!="") { command+=",";}
				command+="Note = '"+POut.String(codeSystem.Note)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE codesystem SET "+command
				+" WHERE CodeSystemNum = "+POut.Long(codeSystem.CodeSystemNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(CodeSystem,CodeSystem) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(CodeSystem codeSystem,CodeSystem oldCodeSystem) {
			if(codeSystem.CodeSystemName != oldCodeSystem.CodeSystemName) {
				return true;
			}
			if(codeSystem.VersionCur != oldCodeSystem.VersionCur) {
				return true;
			}
			if(codeSystem.VersionAvail != oldCodeSystem.VersionAvail) {
				return true;
			}
			if(codeSystem.HL7OID != oldCodeSystem.HL7OID) {
				return true;
			}
			if(codeSystem.Note != oldCodeSystem.Note) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one CodeSystem from the database.</summary>
		public static void Delete(long codeSystemNum) {
			string command="DELETE FROM codesystem "
				+"WHERE CodeSystemNum = "+POut.Long(codeSystemNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many CodeSystems from the database.</summary>
		public static void DeleteMany(List<long> listCodeSystemNums) {
			if(listCodeSystemNums==null || listCodeSystemNums.Count==0) {
				return;
			}
			string command="DELETE FROM codesystem "
				+"WHERE CodeSystemNum IN("+string.Join(",",listCodeSystemNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}