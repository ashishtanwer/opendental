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
	public class WebSchedCarrierRuleCrud {
		///<summary>Gets one WebSchedCarrierRule object from the database using the primary key.  Returns null if not found.</summary>
		public static WebSchedCarrierRule SelectOne(long webSchedCarrierRuleNum) {
			string command="SELECT * FROM webschedcarrierrule "
				+"WHERE WebSchedCarrierRuleNum = "+POut.Long(webSchedCarrierRuleNum);
			List<WebSchedCarrierRule> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one WebSchedCarrierRule object from the database using a query.</summary>
		public static WebSchedCarrierRule SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<WebSchedCarrierRule> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of WebSchedCarrierRule objects from the database using a query.</summary>
		public static List<WebSchedCarrierRule> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<WebSchedCarrierRule> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<WebSchedCarrierRule> TableToList(DataTable table) {
			List<WebSchedCarrierRule> retVal=new List<WebSchedCarrierRule>();
			WebSchedCarrierRule webSchedCarrierRule;
			foreach(DataRow row in table.Rows) {
				webSchedCarrierRule=new WebSchedCarrierRule();
				webSchedCarrierRule.WebSchedCarrierRuleNum= PIn.Long  (row["WebSchedCarrierRuleNum"].ToString());
				webSchedCarrierRule.ClinicNum             = PIn.Long  (row["ClinicNum"].ToString());
				webSchedCarrierRule.CarrierName           = PIn.String(row["CarrierName"].ToString());
				webSchedCarrierRule.DisplayName           = PIn.String(row["DisplayName"].ToString());
				webSchedCarrierRule.Message               = PIn.String(row["Message"].ToString());
				webSchedCarrierRule.Rule                  = (OpenDentBusiness.RuleType)PIn.Int(row["Rule"].ToString());
				retVal.Add(webSchedCarrierRule);
			}
			return retVal;
		}

		///<summary>Converts a list of WebSchedCarrierRule into a DataTable.</summary>
		public static DataTable ListToTable(List<WebSchedCarrierRule> listWebSchedCarrierRules,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="WebSchedCarrierRule";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("WebSchedCarrierRuleNum");
			table.Columns.Add("ClinicNum");
			table.Columns.Add("CarrierName");
			table.Columns.Add("DisplayName");
			table.Columns.Add("Message");
			table.Columns.Add("Rule");
			foreach(WebSchedCarrierRule webSchedCarrierRule in listWebSchedCarrierRules) {
				table.Rows.Add(new object[] {
					POut.Long  (webSchedCarrierRule.WebSchedCarrierRuleNum),
					POut.Long  (webSchedCarrierRule.ClinicNum),
					            webSchedCarrierRule.CarrierName,
					            webSchedCarrierRule.DisplayName,
					            webSchedCarrierRule.Message,
					POut.Int   ((int)webSchedCarrierRule.Rule),
				});
			}
			return table;
		}

		///<summary>Inserts one WebSchedCarrierRule into the database.  Returns the new priKey.</summary>
		public static long Insert(WebSchedCarrierRule webSchedCarrierRule) {
			return Insert(webSchedCarrierRule,false);
		}

		///<summary>Inserts one WebSchedCarrierRule into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(WebSchedCarrierRule webSchedCarrierRule,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				webSchedCarrierRule.WebSchedCarrierRuleNum=ReplicationServers.GetKey("webschedcarrierrule","WebSchedCarrierRuleNum");
			}
			string command="INSERT INTO webschedcarrierrule (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="WebSchedCarrierRuleNum,";
			}
			command+="ClinicNum,CarrierName,DisplayName,Message,Rule) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(webSchedCarrierRule.WebSchedCarrierRuleNum)+",";
			}
			command+=
				     POut.Long  (webSchedCarrierRule.ClinicNum)+","
				+"'"+POut.String(webSchedCarrierRule.CarrierName)+"',"
				+"'"+POut.String(webSchedCarrierRule.DisplayName)+"',"
				+    DbHelper.ParamChar+"paramMessage,"
				+    POut.Int   ((int)webSchedCarrierRule.Rule)+")";
			if(webSchedCarrierRule.Message==null) {
				webSchedCarrierRule.Message="";
			}
			OdSqlParameter paramMessage=new OdSqlParameter("paramMessage",OdDbType.Text,POut.StringParam(webSchedCarrierRule.Message));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramMessage);
			}
			else {
				webSchedCarrierRule.WebSchedCarrierRuleNum=Db.NonQ(command,true,"WebSchedCarrierRuleNum","webSchedCarrierRule",paramMessage);
			}
			return webSchedCarrierRule.WebSchedCarrierRuleNum;
		}

		///<summary>Inserts many WebSchedCarrierRules into the database.</summary>
		public static void InsertMany(List<WebSchedCarrierRule> listWebSchedCarrierRules) {
			InsertMany(listWebSchedCarrierRules,false);
		}

		///<summary>Inserts many WebSchedCarrierRules into the database.  Provides option to use the existing priKey.</summary>
		public static void InsertMany(List<WebSchedCarrierRule> listWebSchedCarrierRules,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				foreach(WebSchedCarrierRule webSchedCarrierRule in listWebSchedCarrierRules) {
					Insert(webSchedCarrierRule);
				}
			}
			else {
				StringBuilder sbCommands=null;
				int index=0;
				int countRows=0;
				while(index < listWebSchedCarrierRules.Count) {
					WebSchedCarrierRule webSchedCarrierRule=listWebSchedCarrierRules[index];
					StringBuilder sbRow=new StringBuilder("(");
					bool hasComma=false;
					if(sbCommands==null) {
						sbCommands=new StringBuilder();
						sbCommands.Append("INSERT INTO webschedcarrierrule (");
						if(useExistingPK) {
							sbCommands.Append("WebSchedCarrierRuleNum,");
						}
						sbCommands.Append("ClinicNum,CarrierName,DisplayName,Message,Rule) VALUES ");
						countRows=0;
					}
					else {
						hasComma=true;
					}
					if(useExistingPK) {
						sbRow.Append(POut.Long(webSchedCarrierRule.WebSchedCarrierRuleNum)); sbRow.Append(",");
					}
					sbRow.Append(POut.Long(webSchedCarrierRule.ClinicNum)); sbRow.Append(",");
					sbRow.Append("'"+POut.String(webSchedCarrierRule.CarrierName)+"'"); sbRow.Append(",");
					sbRow.Append("'"+POut.String(webSchedCarrierRule.DisplayName)+"'"); sbRow.Append(",");
					sbRow.Append("'"+POut.String(webSchedCarrierRule.Message)+"'"); sbRow.Append(",");
					sbRow.Append(POut.Int((int)webSchedCarrierRule.Rule)); sbRow.Append(")");
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
						if(index==listWebSchedCarrierRules.Count-1) {
							Db.NonQ(sbCommands.ToString());
						}
						index++;
					}
				}
			}
		}

		///<summary>Inserts one WebSchedCarrierRule into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(WebSchedCarrierRule webSchedCarrierRule) {
			return InsertNoCache(webSchedCarrierRule,false);
		}

		///<summary>Inserts one WebSchedCarrierRule into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(WebSchedCarrierRule webSchedCarrierRule,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO webschedcarrierrule (";
			if(!useExistingPK && isRandomKeys) {
				webSchedCarrierRule.WebSchedCarrierRuleNum=ReplicationServers.GetKeyNoCache("webschedcarrierrule","WebSchedCarrierRuleNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="WebSchedCarrierRuleNum,";
			}
			command+="ClinicNum,CarrierName,DisplayName,Message,Rule) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(webSchedCarrierRule.WebSchedCarrierRuleNum)+",";
			}
			command+=
				     POut.Long  (webSchedCarrierRule.ClinicNum)+","
				+"'"+POut.String(webSchedCarrierRule.CarrierName)+"',"
				+"'"+POut.String(webSchedCarrierRule.DisplayName)+"',"
				+    DbHelper.ParamChar+"paramMessage,"
				+    POut.Int   ((int)webSchedCarrierRule.Rule)+")";
			if(webSchedCarrierRule.Message==null) {
				webSchedCarrierRule.Message="";
			}
			OdSqlParameter paramMessage=new OdSqlParameter("paramMessage",OdDbType.Text,POut.StringParam(webSchedCarrierRule.Message));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramMessage);
			}
			else {
				webSchedCarrierRule.WebSchedCarrierRuleNum=Db.NonQ(command,true,"WebSchedCarrierRuleNum","webSchedCarrierRule",paramMessage);
			}
			return webSchedCarrierRule.WebSchedCarrierRuleNum;
		}

		///<summary>Updates one WebSchedCarrierRule in the database.</summary>
		public static void Update(WebSchedCarrierRule webSchedCarrierRule) {
			string command="UPDATE webschedcarrierrule SET "
				+"ClinicNum             =  "+POut.Long  (webSchedCarrierRule.ClinicNum)+", "
				+"CarrierName           = '"+POut.String(webSchedCarrierRule.CarrierName)+"', "
				+"DisplayName           = '"+POut.String(webSchedCarrierRule.DisplayName)+"', "
				+"Message               =  "+DbHelper.ParamChar+"paramMessage, "
				+"Rule                  =  "+POut.Int   ((int)webSchedCarrierRule.Rule)+" "
				+"WHERE WebSchedCarrierRuleNum = "+POut.Long(webSchedCarrierRule.WebSchedCarrierRuleNum);
			if(webSchedCarrierRule.Message==null) {
				webSchedCarrierRule.Message="";
			}
			OdSqlParameter paramMessage=new OdSqlParameter("paramMessage",OdDbType.Text,POut.StringParam(webSchedCarrierRule.Message));
			Db.NonQ(command,paramMessage);
		}

		///<summary>Updates one WebSchedCarrierRule in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(WebSchedCarrierRule webSchedCarrierRule,WebSchedCarrierRule oldWebSchedCarrierRule) {
			string command="";
			if(webSchedCarrierRule.ClinicNum != oldWebSchedCarrierRule.ClinicNum) {
				if(command!="") { command+=",";}
				command+="ClinicNum = "+POut.Long(webSchedCarrierRule.ClinicNum)+"";
			}
			if(webSchedCarrierRule.CarrierName != oldWebSchedCarrierRule.CarrierName) {
				if(command!="") { command+=",";}
				command+="CarrierName = '"+POut.String(webSchedCarrierRule.CarrierName)+"'";
			}
			if(webSchedCarrierRule.DisplayName != oldWebSchedCarrierRule.DisplayName) {
				if(command!="") { command+=",";}
				command+="DisplayName = '"+POut.String(webSchedCarrierRule.DisplayName)+"'";
			}
			if(webSchedCarrierRule.Message != oldWebSchedCarrierRule.Message) {
				if(command!="") { command+=",";}
				command+="Message = "+DbHelper.ParamChar+"paramMessage";
			}
			if(webSchedCarrierRule.Rule != oldWebSchedCarrierRule.Rule) {
				if(command!="") { command+=",";}
				command+="Rule = "+POut.Int   ((int)webSchedCarrierRule.Rule)+"";
			}
			if(command=="") {
				return false;
			}
			if(webSchedCarrierRule.Message==null) {
				webSchedCarrierRule.Message="";
			}
			OdSqlParameter paramMessage=new OdSqlParameter("paramMessage",OdDbType.Text,POut.StringParam(webSchedCarrierRule.Message));
			command="UPDATE webschedcarrierrule SET "+command
				+" WHERE WebSchedCarrierRuleNum = "+POut.Long(webSchedCarrierRule.WebSchedCarrierRuleNum);
			Db.NonQ(command,paramMessage);
			return true;
		}

		///<summary>Returns true if Update(WebSchedCarrierRule,WebSchedCarrierRule) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(WebSchedCarrierRule webSchedCarrierRule,WebSchedCarrierRule oldWebSchedCarrierRule) {
			if(webSchedCarrierRule.ClinicNum != oldWebSchedCarrierRule.ClinicNum) {
				return true;
			}
			if(webSchedCarrierRule.CarrierName != oldWebSchedCarrierRule.CarrierName) {
				return true;
			}
			if(webSchedCarrierRule.DisplayName != oldWebSchedCarrierRule.DisplayName) {
				return true;
			}
			if(webSchedCarrierRule.Message != oldWebSchedCarrierRule.Message) {
				return true;
			}
			if(webSchedCarrierRule.Rule != oldWebSchedCarrierRule.Rule) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one WebSchedCarrierRule from the database.</summary>
		public static void Delete(long webSchedCarrierRuleNum) {
			string command="DELETE FROM webschedcarrierrule "
				+"WHERE WebSchedCarrierRuleNum = "+POut.Long(webSchedCarrierRuleNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many WebSchedCarrierRules from the database.</summary>
		public static void DeleteMany(List<long> listWebSchedCarrierRuleNums) {
			if(listWebSchedCarrierRuleNums==null || listWebSchedCarrierRuleNums.Count==0) {
				return;
			}
			string command="DELETE FROM webschedcarrierrule "
				+"WHERE WebSchedCarrierRuleNum IN("+string.Join(",",listWebSchedCarrierRuleNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}