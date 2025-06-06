//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class SigMessageCrud {
		///<summary>Gets one SigMessage object from the database using the primary key.  Returns null if not found.</summary>
		public static SigMessage SelectOne(long sigMessageNum) {
			string command="SELECT * FROM sigmessage "
				+"WHERE SigMessageNum = "+POut.Long(sigMessageNum);
			List<SigMessage> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SigMessage object from the database using a query.</summary>
		public static SigMessage SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SigMessage> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SigMessage objects from the database using a query.</summary>
		public static List<SigMessage> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SigMessage> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<SigMessage> TableToList(DataTable table) {
			List<SigMessage> retVal=new List<SigMessage>();
			SigMessage sigMessage;
			foreach(DataRow row in table.Rows) {
				sigMessage=new SigMessage();
				sigMessage.SigMessageNum        = PIn.Long  (row["SigMessageNum"].ToString());
				sigMessage.ButtonText           = PIn.String(row["ButtonText"].ToString());
				sigMessage.ButtonIndex          = PIn.Int   (row["ButtonIndex"].ToString());
				sigMessage.SynchIcon            = PIn.Byte  (row["SynchIcon"].ToString());
				sigMessage.FromUser             = PIn.String(row["FromUser"].ToString());
				sigMessage.ToUser               = PIn.String(row["ToUser"].ToString());
				sigMessage.MessageDateTime      = PIn.DateT (row["MessageDateTime"].ToString());
				sigMessage.AckDateTime          = PIn.DateT (row["AckDateTime"].ToString());
				sigMessage.SigText              = PIn.String(row["SigText"].ToString());
				sigMessage.SigElementDefNumUser = PIn.Long  (row["SigElementDefNumUser"].ToString());
				sigMessage.SigElementDefNumExtra= PIn.Long  (row["SigElementDefNumExtra"].ToString());
				sigMessage.SigElementDefNumMsg  = PIn.Long  (row["SigElementDefNumMsg"].ToString());
				retVal.Add(sigMessage);
			}
			return retVal;
		}

		///<summary>Converts a list of SigMessage into a DataTable.</summary>
		public static DataTable ListToTable(List<SigMessage> listSigMessages,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="SigMessage";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("SigMessageNum");
			table.Columns.Add("ButtonText");
			table.Columns.Add("ButtonIndex");
			table.Columns.Add("SynchIcon");
			table.Columns.Add("FromUser");
			table.Columns.Add("ToUser");
			table.Columns.Add("MessageDateTime");
			table.Columns.Add("AckDateTime");
			table.Columns.Add("SigText");
			table.Columns.Add("SigElementDefNumUser");
			table.Columns.Add("SigElementDefNumExtra");
			table.Columns.Add("SigElementDefNumMsg");
			foreach(SigMessage sigMessage in listSigMessages) {
				table.Rows.Add(new object[] {
					POut.Long  (sigMessage.SigMessageNum),
					            sigMessage.ButtonText,
					POut.Int   (sigMessage.ButtonIndex),
					POut.Byte  (sigMessage.SynchIcon),
					            sigMessage.FromUser,
					            sigMessage.ToUser,
					POut.DateT (sigMessage.MessageDateTime,false),
					POut.DateT (sigMessage.AckDateTime,false),
					            sigMessage.SigText,
					POut.Long  (sigMessage.SigElementDefNumUser),
					POut.Long  (sigMessage.SigElementDefNumExtra),
					POut.Long  (sigMessage.SigElementDefNumMsg),
				});
			}
			return table;
		}

		///<summary>Inserts one SigMessage into the database.  Returns the new priKey.</summary>
		public static long Insert(SigMessage sigMessage) {
			return Insert(sigMessage,false);
		}

		///<summary>Inserts one SigMessage into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(SigMessage sigMessage,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				sigMessage.SigMessageNum=ReplicationServers.GetKey("sigmessage","SigMessageNum");
			}
			string command="INSERT INTO sigmessage (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SigMessageNum,";
			}
			command+="ButtonText,ButtonIndex,SynchIcon,FromUser,ToUser,MessageDateTime,AckDateTime,SigText,SigElementDefNumUser,SigElementDefNumExtra,SigElementDefNumMsg) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(sigMessage.SigMessageNum)+",";
			}
			command+=
				 "'"+POut.String(sigMessage.ButtonText)+"',"
				+    POut.Int   (sigMessage.ButtonIndex)+","
				+    POut.Byte  (sigMessage.SynchIcon)+","
				+"'"+POut.String(sigMessage.FromUser)+"',"
				+"'"+POut.String(sigMessage.ToUser)+"',"
				+    DbHelper.Now()+","
				+    POut.DateT (sigMessage.AckDateTime)+","
				+"'"+POut.String(sigMessage.SigText)+"',"
				+    POut.Long  (sigMessage.SigElementDefNumUser)+","
				+    POut.Long  (sigMessage.SigElementDefNumExtra)+","
				+    POut.Long  (sigMessage.SigElementDefNumMsg)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				sigMessage.SigMessageNum=Db.NonQ(command,true,"SigMessageNum","sigMessage");
			}
			return sigMessage.SigMessageNum;
		}

		///<summary>Inserts one SigMessage into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SigMessage sigMessage) {
			return InsertNoCache(sigMessage,false);
		}

		///<summary>Inserts one SigMessage into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SigMessage sigMessage,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO sigmessage (";
			if(!useExistingPK && isRandomKeys) {
				sigMessage.SigMessageNum=ReplicationServers.GetKeyNoCache("sigmessage","SigMessageNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="SigMessageNum,";
			}
			command+="ButtonText,ButtonIndex,SynchIcon,FromUser,ToUser,MessageDateTime,AckDateTime,SigText,SigElementDefNumUser,SigElementDefNumExtra,SigElementDefNumMsg) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(sigMessage.SigMessageNum)+",";
			}
			command+=
				 "'"+POut.String(sigMessage.ButtonText)+"',"
				+    POut.Int   (sigMessage.ButtonIndex)+","
				+    POut.Byte  (sigMessage.SynchIcon)+","
				+"'"+POut.String(sigMessage.FromUser)+"',"
				+"'"+POut.String(sigMessage.ToUser)+"',"
				+    DbHelper.Now()+","
				+    POut.DateT (sigMessage.AckDateTime)+","
				+"'"+POut.String(sigMessage.SigText)+"',"
				+    POut.Long  (sigMessage.SigElementDefNumUser)+","
				+    POut.Long  (sigMessage.SigElementDefNumExtra)+","
				+    POut.Long  (sigMessage.SigElementDefNumMsg)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				sigMessage.SigMessageNum=Db.NonQ(command,true,"SigMessageNum","sigMessage");
			}
			return sigMessage.SigMessageNum;
		}

		///<summary>Updates one SigMessage in the database.</summary>
		public static void Update(SigMessage sigMessage) {
			string command="UPDATE sigmessage SET "
				+"ButtonText           = '"+POut.String(sigMessage.ButtonText)+"', "
				+"ButtonIndex          =  "+POut.Int   (sigMessage.ButtonIndex)+", "
				+"SynchIcon            =  "+POut.Byte  (sigMessage.SynchIcon)+", "
				+"FromUser             = '"+POut.String(sigMessage.FromUser)+"', "
				+"ToUser               = '"+POut.String(sigMessage.ToUser)+"', "
				//MessageDateTime not allowed to change
				+"AckDateTime          =  "+POut.DateT (sigMessage.AckDateTime)+", "
				+"SigText              = '"+POut.String(sigMessage.SigText)+"', "
				+"SigElementDefNumUser =  "+POut.Long  (sigMessage.SigElementDefNumUser)+", "
				+"SigElementDefNumExtra=  "+POut.Long  (sigMessage.SigElementDefNumExtra)+", "
				+"SigElementDefNumMsg  =  "+POut.Long  (sigMessage.SigElementDefNumMsg)+" "
				+"WHERE SigMessageNum = "+POut.Long(sigMessage.SigMessageNum);
			Db.NonQ(command);
		}

		///<summary>Updates one SigMessage in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(SigMessage sigMessage,SigMessage oldSigMessage) {
			string command="";
			if(sigMessage.ButtonText != oldSigMessage.ButtonText) {
				if(command!="") { command+=",";}
				command+="ButtonText = '"+POut.String(sigMessage.ButtonText)+"'";
			}
			if(sigMessage.ButtonIndex != oldSigMessage.ButtonIndex) {
				if(command!="") { command+=",";}
				command+="ButtonIndex = "+POut.Int(sigMessage.ButtonIndex)+"";
			}
			if(sigMessage.SynchIcon != oldSigMessage.SynchIcon) {
				if(command!="") { command+=",";}
				command+="SynchIcon = "+POut.Byte(sigMessage.SynchIcon)+"";
			}
			if(sigMessage.FromUser != oldSigMessage.FromUser) {
				if(command!="") { command+=",";}
				command+="FromUser = '"+POut.String(sigMessage.FromUser)+"'";
			}
			if(sigMessage.ToUser != oldSigMessage.ToUser) {
				if(command!="") { command+=",";}
				command+="ToUser = '"+POut.String(sigMessage.ToUser)+"'";
			}
			//MessageDateTime not allowed to change
			if(sigMessage.AckDateTime != oldSigMessage.AckDateTime) {
				if(command!="") { command+=",";}
				command+="AckDateTime = "+POut.DateT(sigMessage.AckDateTime)+"";
			}
			if(sigMessage.SigText != oldSigMessage.SigText) {
				if(command!="") { command+=",";}
				command+="SigText = '"+POut.String(sigMessage.SigText)+"'";
			}
			if(sigMessage.SigElementDefNumUser != oldSigMessage.SigElementDefNumUser) {
				if(command!="") { command+=",";}
				command+="SigElementDefNumUser = "+POut.Long(sigMessage.SigElementDefNumUser)+"";
			}
			if(sigMessage.SigElementDefNumExtra != oldSigMessage.SigElementDefNumExtra) {
				if(command!="") { command+=",";}
				command+="SigElementDefNumExtra = "+POut.Long(sigMessage.SigElementDefNumExtra)+"";
			}
			if(sigMessage.SigElementDefNumMsg != oldSigMessage.SigElementDefNumMsg) {
				if(command!="") { command+=",";}
				command+="SigElementDefNumMsg = "+POut.Long(sigMessage.SigElementDefNumMsg)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE sigmessage SET "+command
				+" WHERE SigMessageNum = "+POut.Long(sigMessage.SigMessageNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(SigMessage,SigMessage) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(SigMessage sigMessage,SigMessage oldSigMessage) {
			if(sigMessage.ButtonText != oldSigMessage.ButtonText) {
				return true;
			}
			if(sigMessage.ButtonIndex != oldSigMessage.ButtonIndex) {
				return true;
			}
			if(sigMessage.SynchIcon != oldSigMessage.SynchIcon) {
				return true;
			}
			if(sigMessage.FromUser != oldSigMessage.FromUser) {
				return true;
			}
			if(sigMessage.ToUser != oldSigMessage.ToUser) {
				return true;
			}
			//MessageDateTime not allowed to change
			if(sigMessage.AckDateTime != oldSigMessage.AckDateTime) {
				return true;
			}
			if(sigMessage.SigText != oldSigMessage.SigText) {
				return true;
			}
			if(sigMessage.SigElementDefNumUser != oldSigMessage.SigElementDefNumUser) {
				return true;
			}
			if(sigMessage.SigElementDefNumExtra != oldSigMessage.SigElementDefNumExtra) {
				return true;
			}
			if(sigMessage.SigElementDefNumMsg != oldSigMessage.SigElementDefNumMsg) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one SigMessage from the database.</summary>
		public static void Delete(long sigMessageNum) {
			string command="DELETE FROM sigmessage "
				+"WHERE SigMessageNum = "+POut.Long(sigMessageNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many SigMessages from the database.</summary>
		public static void DeleteMany(List<long> listSigMessageNums) {
			if(listSigMessageNums==null || listSigMessageNums.Count==0) {
				return;
			}
			string command="DELETE FROM sigmessage "
				+"WHERE SigMessageNum IN("+string.Join(",",listSigMessageNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}