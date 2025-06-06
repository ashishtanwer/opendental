//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using EhrLaboratories;

namespace OpenDentBusiness.Crud{
	public class EhrLabImageCrud {
		///<summary>Gets one EhrLabImage object from the database using the primary key.  Returns null if not found.</summary>
		public static EhrLabImage SelectOne(long ehrLabImageNum) {
			string command="SELECT * FROM ehrlabimage "
				+"WHERE EhrLabImageNum = "+POut.Long(ehrLabImageNum);
			List<EhrLabImage> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EhrLabImage object from the database using a query.</summary>
		public static EhrLabImage SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrLabImage> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EhrLabImage objects from the database using a query.</summary>
		public static List<EhrLabImage> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EhrLabImage> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EhrLabImage> TableToList(DataTable table) {
			List<EhrLabImage> retVal=new List<EhrLabImage>();
			EhrLabImage ehrLabImage;
			foreach(DataRow row in table.Rows) {
				ehrLabImage=new EhrLabImage();
				ehrLabImage.EhrLabImageNum= PIn.Long  (row["EhrLabImageNum"].ToString());
				ehrLabImage.EhrLabNum     = PIn.Long  (row["EhrLabNum"].ToString());
				ehrLabImage.DocNum        = PIn.Long  (row["DocNum"].ToString());
				retVal.Add(ehrLabImage);
			}
			return retVal;
		}

		///<summary>Converts a list of EhrLabImage into a DataTable.</summary>
		public static DataTable ListToTable(List<EhrLabImage> listEhrLabImages,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EhrLabImage";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EhrLabImageNum");
			table.Columns.Add("EhrLabNum");
			table.Columns.Add("DocNum");
			foreach(EhrLabImage ehrLabImage in listEhrLabImages) {
				table.Rows.Add(new object[] {
					POut.Long  (ehrLabImage.EhrLabImageNum),
					POut.Long  (ehrLabImage.EhrLabNum),
					POut.Long  (ehrLabImage.DocNum),
				});
			}
			return table;
		}

		///<summary>Inserts one EhrLabImage into the database.  Returns the new priKey.</summary>
		public static long Insert(EhrLabImage ehrLabImage) {
			return Insert(ehrLabImage,false);
		}

		///<summary>Inserts one EhrLabImage into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EhrLabImage ehrLabImage,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				ehrLabImage.EhrLabImageNum=ReplicationServers.GetKey("ehrlabimage","EhrLabImageNum");
			}
			string command="INSERT INTO ehrlabimage (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EhrLabImageNum,";
			}
			command+="EhrLabNum,DocNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(ehrLabImage.EhrLabImageNum)+",";
			}
			command+=
				     POut.Long  (ehrLabImage.EhrLabNum)+","
				+    POut.Long  (ehrLabImage.DocNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrLabImage.EhrLabImageNum=Db.NonQ(command,true,"EhrLabImageNum","ehrLabImage");
			}
			return ehrLabImage.EhrLabImageNum;
		}

		///<summary>Inserts one EhrLabImage into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrLabImage ehrLabImage) {
			return InsertNoCache(ehrLabImage,false);
		}

		///<summary>Inserts one EhrLabImage into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EhrLabImage ehrLabImage,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO ehrlabimage (";
			if(!useExistingPK && isRandomKeys) {
				ehrLabImage.EhrLabImageNum=ReplicationServers.GetKeyNoCache("ehrlabimage","EhrLabImageNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EhrLabImageNum,";
			}
			command+="EhrLabNum,DocNum) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(ehrLabImage.EhrLabImageNum)+",";
			}
			command+=
				     POut.Long  (ehrLabImage.EhrLabNum)+","
				+    POut.Long  (ehrLabImage.DocNum)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				ehrLabImage.EhrLabImageNum=Db.NonQ(command,true,"EhrLabImageNum","ehrLabImage");
			}
			return ehrLabImage.EhrLabImageNum;
		}

		///<summary>Updates one EhrLabImage in the database.</summary>
		public static void Update(EhrLabImage ehrLabImage) {
			string command="UPDATE ehrlabimage SET "
				+"EhrLabNum     =  "+POut.Long  (ehrLabImage.EhrLabNum)+", "
				+"DocNum        =  "+POut.Long  (ehrLabImage.DocNum)+" "
				+"WHERE EhrLabImageNum = "+POut.Long(ehrLabImage.EhrLabImageNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EhrLabImage in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EhrLabImage ehrLabImage,EhrLabImage oldEhrLabImage) {
			string command="";
			if(ehrLabImage.EhrLabNum != oldEhrLabImage.EhrLabNum) {
				if(command!="") { command+=",";}
				command+="EhrLabNum = "+POut.Long(ehrLabImage.EhrLabNum)+"";
			}
			if(ehrLabImage.DocNum != oldEhrLabImage.DocNum) {
				if(command!="") { command+=",";}
				command+="DocNum = "+POut.Long(ehrLabImage.DocNum)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE ehrlabimage SET "+command
				+" WHERE EhrLabImageNum = "+POut.Long(ehrLabImage.EhrLabImageNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(EhrLabImage,EhrLabImage) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EhrLabImage ehrLabImage,EhrLabImage oldEhrLabImage) {
			if(ehrLabImage.EhrLabNum != oldEhrLabImage.EhrLabNum) {
				return true;
			}
			if(ehrLabImage.DocNum != oldEhrLabImage.DocNum) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EhrLabImage from the database.</summary>
		public static void Delete(long ehrLabImageNum) {
			string command="DELETE FROM ehrlabimage "
				+"WHERE EhrLabImageNum = "+POut.Long(ehrLabImageNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many EhrLabImages from the database.</summary>
		public static void DeleteMany(List<long> listEhrLabImageNums) {
			if(listEhrLabImageNums==null || listEhrLabImageNums.Count==0) {
				return;
			}
			string command="DELETE FROM ehrlabimage "
				+"WHERE EhrLabImageNum IN("+string.Join(",",listEhrLabImageNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}