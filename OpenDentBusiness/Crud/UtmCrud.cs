//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class UtmCrud {
		///<summary>Gets one Utm object from the database using the primary key.  Returns null if not found.</summary>
		public static Utm SelectOne(long utmNum) {
			string command="SELECT * FROM utm "
				+"WHERE UtmNum = "+POut.Long(utmNum);
			List<Utm> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Utm object from the database using a query.</summary>
		public static Utm SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Utm> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Utm objects from the database using a query.</summary>
		public static List<Utm> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Utm> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Utm> TableToList(DataTable table) {
			List<Utm> retVal=new List<Utm>();
			Utm utm;
			foreach(DataRow row in table.Rows) {
				utm=new Utm();
				utm.UtmNum      = PIn.Long  (row["UtmNum"].ToString());
				utm.CampaignName= PIn.String(row["CampaignName"].ToString());
				utm.MediumInfo  = PIn.String(row["MediumInfo"].ToString());
				utm.SourceInfo  = PIn.String(row["SourceInfo"].ToString());
				retVal.Add(utm);
			}
			return retVal;
		}

		///<summary>Converts a list of Utm into a DataTable.</summary>
		public static DataTable ListToTable(List<Utm> listUtms,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="Utm";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("UtmNum");
			table.Columns.Add("CampaignName");
			table.Columns.Add("MediumInfo");
			table.Columns.Add("SourceInfo");
			foreach(Utm utm in listUtms) {
				table.Rows.Add(new object[] {
					POut.Long  (utm.UtmNum),
					            utm.CampaignName,
					            utm.MediumInfo,
					            utm.SourceInfo,
				});
			}
			return table;
		}

		///<summary>Inserts one Utm into the database.  Returns the new priKey.</summary>
		public static long Insert(Utm utm) {
			return Insert(utm,false);
		}

		///<summary>Inserts one Utm into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Utm utm,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				utm.UtmNum=ReplicationServers.GetKey("utm","UtmNum");
			}
			string command="INSERT INTO utm (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="UtmNum,";
			}
			command+="CampaignName,MediumInfo,SourceInfo) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(utm.UtmNum)+",";
			}
			command+=
				 "'"+POut.String(utm.CampaignName)+"',"
				+"'"+POut.String(utm.MediumInfo)+"',"
				+"'"+POut.String(utm.SourceInfo)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				utm.UtmNum=Db.NonQ(command,true,"UtmNum","utm");
			}
			return utm.UtmNum;
		}

		///<summary>Inserts one Utm into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Utm utm) {
			return InsertNoCache(utm,false);
		}

		///<summary>Inserts one Utm into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(Utm utm,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO utm (";
			if(!useExistingPK && isRandomKeys) {
				utm.UtmNum=ReplicationServers.GetKeyNoCache("utm","UtmNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="UtmNum,";
			}
			command+="CampaignName,MediumInfo,SourceInfo) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(utm.UtmNum)+",";
			}
			command+=
				 "'"+POut.String(utm.CampaignName)+"',"
				+"'"+POut.String(utm.MediumInfo)+"',"
				+"'"+POut.String(utm.SourceInfo)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				utm.UtmNum=Db.NonQ(command,true,"UtmNum","utm");
			}
			return utm.UtmNum;
		}

		///<summary>Updates one Utm in the database.</summary>
		public static void Update(Utm utm) {
			string command="UPDATE utm SET "
				+"CampaignName= '"+POut.String(utm.CampaignName)+"', "
				+"MediumInfo  = '"+POut.String(utm.MediumInfo)+"', "
				+"SourceInfo  = '"+POut.String(utm.SourceInfo)+"' "
				+"WHERE UtmNum = "+POut.Long(utm.UtmNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Utm in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(Utm utm,Utm oldUtm) {
			string command="";
			if(utm.CampaignName != oldUtm.CampaignName) {
				if(command!="") { command+=",";}
				command+="CampaignName = '"+POut.String(utm.CampaignName)+"'";
			}
			if(utm.MediumInfo != oldUtm.MediumInfo) {
				if(command!="") { command+=",";}
				command+="MediumInfo = '"+POut.String(utm.MediumInfo)+"'";
			}
			if(utm.SourceInfo != oldUtm.SourceInfo) {
				if(command!="") { command+=",";}
				command+="SourceInfo = '"+POut.String(utm.SourceInfo)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE utm SET "+command
				+" WHERE UtmNum = "+POut.Long(utm.UtmNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(Utm,Utm) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(Utm utm,Utm oldUtm) {
			if(utm.CampaignName != oldUtm.CampaignName) {
				return true;
			}
			if(utm.MediumInfo != oldUtm.MediumInfo) {
				return true;
			}
			if(utm.SourceInfo != oldUtm.SourceInfo) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one Utm from the database.</summary>
		public static void Delete(long utmNum) {
			string command="DELETE FROM utm "
				+"WHERE UtmNum = "+POut.Long(utmNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many Utms from the database.</summary>
		public static void DeleteMany(List<long> listUtmNums) {
			if(listUtmNums==null || listUtmNums.Count==0) {
				return;
			}
			string command="DELETE FROM utm "
				+"WHERE UtmNum IN("+string.Join(",",listUtmNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}