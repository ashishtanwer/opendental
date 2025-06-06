//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class EmailAutographCrud {
		///<summary>Gets one EmailAutograph object from the database using the primary key.  Returns null if not found.</summary>
		public static EmailAutograph SelectOne(long emailAutographNum) {
			string command="SELECT * FROM emailautograph "
				+"WHERE EmailAutographNum = "+POut.Long(emailAutographNum);
			List<EmailAutograph> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EmailAutograph object from the database using a query.</summary>
		public static EmailAutograph SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EmailAutograph> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EmailAutograph objects from the database using a query.</summary>
		public static List<EmailAutograph> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EmailAutograph> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EmailAutograph> TableToList(DataTable table) {
			List<EmailAutograph> retVal=new List<EmailAutograph>();
			EmailAutograph emailAutograph;
			foreach(DataRow row in table.Rows) {
				emailAutograph=new EmailAutograph();
				emailAutograph.EmailAutographNum= PIn.Long  (row["EmailAutographNum"].ToString());
				emailAutograph.Description      = PIn.String(row["Description"].ToString());
				emailAutograph.EmailAddress     = PIn.String(row["EmailAddress"].ToString());
				emailAutograph.AutographText    = PIn.String(row["AutographText"].ToString());
				retVal.Add(emailAutograph);
			}
			return retVal;
		}

		///<summary>Converts a list of EmailAutograph into a DataTable.</summary>
		public static DataTable ListToTable(List<EmailAutograph> listEmailAutographs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EmailAutograph";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EmailAutographNum");
			table.Columns.Add("Description");
			table.Columns.Add("EmailAddress");
			table.Columns.Add("AutographText");
			foreach(EmailAutograph emailAutograph in listEmailAutographs) {
				table.Rows.Add(new object[] {
					POut.Long  (emailAutograph.EmailAutographNum),
					            emailAutograph.Description,
					            emailAutograph.EmailAddress,
					            emailAutograph.AutographText,
				});
			}
			return table;
		}

		///<summary>Inserts one EmailAutograph into the database.  Returns the new priKey.</summary>
		public static long Insert(EmailAutograph emailAutograph) {
			return Insert(emailAutograph,false);
		}

		///<summary>Inserts one EmailAutograph into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EmailAutograph emailAutograph,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				emailAutograph.EmailAutographNum=ReplicationServers.GetKey("emailautograph","EmailAutographNum");
			}
			string command="INSERT INTO emailautograph (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EmailAutographNum,";
			}
			command+="Description,EmailAddress,AutographText) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(emailAutograph.EmailAutographNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramDescription,"
				+"'"+POut.String(emailAutograph.EmailAddress)+"',"
				+    DbHelper.ParamChar+"paramAutographText)";
			if(emailAutograph.Description==null) {
				emailAutograph.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(emailAutograph.Description));
			if(emailAutograph.AutographText==null) {
				emailAutograph.AutographText="";
			}
			OdSqlParameter paramAutographText=new OdSqlParameter("paramAutographText",OdDbType.Text,POut.StringParam(emailAutograph.AutographText));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramDescription,paramAutographText);
			}
			else {
				emailAutograph.EmailAutographNum=Db.NonQ(command,true,"EmailAutographNum","emailAutograph",paramDescription,paramAutographText);
			}
			return emailAutograph.EmailAutographNum;
		}

		///<summary>Inserts one EmailAutograph into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EmailAutograph emailAutograph) {
			return InsertNoCache(emailAutograph,false);
		}

		///<summary>Inserts one EmailAutograph into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EmailAutograph emailAutograph,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO emailautograph (";
			if(!useExistingPK && isRandomKeys) {
				emailAutograph.EmailAutographNum=ReplicationServers.GetKeyNoCache("emailautograph","EmailAutographNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EmailAutographNum,";
			}
			command+="Description,EmailAddress,AutographText) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(emailAutograph.EmailAutographNum)+",";
			}
			command+=
				     DbHelper.ParamChar+"paramDescription,"
				+"'"+POut.String(emailAutograph.EmailAddress)+"',"
				+    DbHelper.ParamChar+"paramAutographText)";
			if(emailAutograph.Description==null) {
				emailAutograph.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(emailAutograph.Description));
			if(emailAutograph.AutographText==null) {
				emailAutograph.AutographText="";
			}
			OdSqlParameter paramAutographText=new OdSqlParameter("paramAutographText",OdDbType.Text,POut.StringParam(emailAutograph.AutographText));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramDescription,paramAutographText);
			}
			else {
				emailAutograph.EmailAutographNum=Db.NonQ(command,true,"EmailAutographNum","emailAutograph",paramDescription,paramAutographText);
			}
			return emailAutograph.EmailAutographNum;
		}

		///<summary>Updates one EmailAutograph in the database.</summary>
		public static void Update(EmailAutograph emailAutograph) {
			string command="UPDATE emailautograph SET "
				+"Description      =  "+DbHelper.ParamChar+"paramDescription, "
				+"EmailAddress     = '"+POut.String(emailAutograph.EmailAddress)+"', "
				+"AutographText    =  "+DbHelper.ParamChar+"paramAutographText "
				+"WHERE EmailAutographNum = "+POut.Long(emailAutograph.EmailAutographNum);
			if(emailAutograph.Description==null) {
				emailAutograph.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(emailAutograph.Description));
			if(emailAutograph.AutographText==null) {
				emailAutograph.AutographText="";
			}
			OdSqlParameter paramAutographText=new OdSqlParameter("paramAutographText",OdDbType.Text,POut.StringParam(emailAutograph.AutographText));
			Db.NonQ(command,paramDescription,paramAutographText);
		}

		///<summary>Updates one EmailAutograph in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EmailAutograph emailAutograph,EmailAutograph oldEmailAutograph) {
			string command="";
			if(emailAutograph.Description != oldEmailAutograph.Description) {
				if(command!="") { command+=",";}
				command+="Description = "+DbHelper.ParamChar+"paramDescription";
			}
			if(emailAutograph.EmailAddress != oldEmailAutograph.EmailAddress) {
				if(command!="") { command+=",";}
				command+="EmailAddress = '"+POut.String(emailAutograph.EmailAddress)+"'";
			}
			if(emailAutograph.AutographText != oldEmailAutograph.AutographText) {
				if(command!="") { command+=",";}
				command+="AutographText = "+DbHelper.ParamChar+"paramAutographText";
			}
			if(command=="") {
				return false;
			}
			if(emailAutograph.Description==null) {
				emailAutograph.Description="";
			}
			OdSqlParameter paramDescription=new OdSqlParameter("paramDescription",OdDbType.Text,POut.StringParam(emailAutograph.Description));
			if(emailAutograph.AutographText==null) {
				emailAutograph.AutographText="";
			}
			OdSqlParameter paramAutographText=new OdSqlParameter("paramAutographText",OdDbType.Text,POut.StringParam(emailAutograph.AutographText));
			command="UPDATE emailautograph SET "+command
				+" WHERE EmailAutographNum = "+POut.Long(emailAutograph.EmailAutographNum);
			Db.NonQ(command,paramDescription,paramAutographText);
			return true;
		}

		///<summary>Returns true if Update(EmailAutograph,EmailAutograph) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EmailAutograph emailAutograph,EmailAutograph oldEmailAutograph) {
			if(emailAutograph.Description != oldEmailAutograph.Description) {
				return true;
			}
			if(emailAutograph.EmailAddress != oldEmailAutograph.EmailAddress) {
				return true;
			}
			if(emailAutograph.AutographText != oldEmailAutograph.AutographText) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EmailAutograph from the database.</summary>
		public static void Delete(long emailAutographNum) {
			string command="DELETE FROM emailautograph "
				+"WHERE EmailAutographNum = "+POut.Long(emailAutographNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many EmailAutographs from the database.</summary>
		public static void DeleteMany(List<long> listEmailAutographNums) {
			if(listEmailAutographNums==null || listEmailAutographNums.Count==0) {
				return;
			}
			string command="DELETE FROM emailautograph "
				+"WHERE EmailAutographNum IN("+string.Join(",",listEmailAutographNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}