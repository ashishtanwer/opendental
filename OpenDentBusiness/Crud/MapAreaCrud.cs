//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class MapAreaCrud {
		///<summary>Gets one MapArea object from the database using the primary key.  Returns null if not found.</summary>
		public static MapArea SelectOne(long mapAreaNum) {
			string command="SELECT * FROM maparea "
				+"WHERE MapAreaNum = "+POut.Long(mapAreaNum);
			List<MapArea> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one MapArea object from the database using a query.</summary>
		public static MapArea SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MapArea> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of MapArea objects from the database using a query.</summary>
		public static List<MapArea> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MapArea> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<MapArea> TableToList(DataTable table) {
			List<MapArea> retVal=new List<MapArea>();
			MapArea mapArea;
			foreach(DataRow row in table.Rows) {
				mapArea=new MapArea();
				mapArea.MapAreaNum         = PIn.Long  (row["MapAreaNum"].ToString());
				mapArea.Extension          = PIn.Int   (row["Extension"].ToString());
				mapArea.XPos               = PIn.Double(row["XPos"].ToString());
				mapArea.YPos               = PIn.Double(row["YPos"].ToString());
				mapArea.Width              = PIn.Double(row["Width"].ToString());
				mapArea.Height             = PIn.Double(row["Height"].ToString());
				mapArea.Description        = PIn.String(row["Description"].ToString());
				mapArea.ItemType           = (OpenDentBusiness.MapItemType)PIn.Int(row["ItemType"].ToString());
				mapArea.MapAreaContainerNum= PIn.Long  (row["MapAreaContainerNum"].ToString());
				mapArea.FontSize           = PIn.Float (row["FontSize"].ToString());
				retVal.Add(mapArea);
			}
			return retVal;
		}

		///<summary>Converts a list of MapArea into a DataTable.</summary>
		public static DataTable ListToTable(List<MapArea> listMapAreas,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="MapArea";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("MapAreaNum");
			table.Columns.Add("Extension");
			table.Columns.Add("XPos");
			table.Columns.Add("YPos");
			table.Columns.Add("Width");
			table.Columns.Add("Height");
			table.Columns.Add("Description");
			table.Columns.Add("ItemType");
			table.Columns.Add("MapAreaContainerNum");
			table.Columns.Add("FontSize");
			foreach(MapArea mapArea in listMapAreas) {
				table.Rows.Add(new object[] {
					POut.Long  (mapArea.MapAreaNum),
					POut.Int   (mapArea.Extension),
					POut.Double(mapArea.XPos),
					POut.Double(mapArea.YPos),
					POut.Double(mapArea.Width),
					POut.Double(mapArea.Height),
					            mapArea.Description,
					POut.Int   ((int)mapArea.ItemType),
					POut.Long  (mapArea.MapAreaContainerNum),
					POut.Float (mapArea.FontSize),
				});
			}
			return table;
		}

		///<summary>Inserts one MapArea into the database.  Returns the new priKey.</summary>
		public static long Insert(MapArea mapArea) {
			return Insert(mapArea,false);
		}

		///<summary>Inserts one MapArea into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(MapArea mapArea,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				mapArea.MapAreaNum=ReplicationServers.GetKey("maparea","MapAreaNum");
			}
			string command="INSERT INTO maparea (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="MapAreaNum,";
			}
			command+="Extension,XPos,YPos,Width,Height,Description,ItemType,MapAreaContainerNum,FontSize) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(mapArea.MapAreaNum)+",";
			}
			command+=
				     POut.Int   (mapArea.Extension)+","
				+		 POut.Double(mapArea.XPos)+","
				+		 POut.Double(mapArea.YPos)+","
				+		 POut.Double(mapArea.Width)+","
				+		 POut.Double(mapArea.Height)+","
				+"'"+POut.String(mapArea.Description)+"',"
				+    POut.Int   ((int)mapArea.ItemType)+","
				+    POut.Long  (mapArea.MapAreaContainerNum)+","
				+    POut.Float (mapArea.FontSize)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				mapArea.MapAreaNum=Db.NonQ(command,true,"MapAreaNum","mapArea");
			}
			return mapArea.MapAreaNum;
		}

		///<summary>Inserts one MapArea into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(MapArea mapArea) {
			return InsertNoCache(mapArea,false);
		}

		///<summary>Inserts one MapArea into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(MapArea mapArea,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO maparea (";
			if(!useExistingPK && isRandomKeys) {
				mapArea.MapAreaNum=ReplicationServers.GetKeyNoCache("maparea","MapAreaNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="MapAreaNum,";
			}
			command+="Extension,XPos,YPos,Width,Height,Description,ItemType,MapAreaContainerNum,FontSize) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(mapArea.MapAreaNum)+",";
			}
			command+=
				     POut.Int   (mapArea.Extension)+","
				+	   POut.Double(mapArea.XPos)+","
				+	   POut.Double(mapArea.YPos)+","
				+	   POut.Double(mapArea.Width)+","
				+	   POut.Double(mapArea.Height)+","
				+"'"+POut.String(mapArea.Description)+"',"
				+    POut.Int   ((int)mapArea.ItemType)+","
				+    POut.Long  (mapArea.MapAreaContainerNum)+","
				+    POut.Float (mapArea.FontSize)+")";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				mapArea.MapAreaNum=Db.NonQ(command,true,"MapAreaNum","mapArea");
			}
			return mapArea.MapAreaNum;
		}

		///<summary>Updates one MapArea in the database.</summary>
		public static void Update(MapArea mapArea) {
			string command="UPDATE maparea SET "
				+"Extension          =  "+POut.Int   (mapArea.Extension)+", "
				+"XPos               =  "+POut.Double(mapArea.XPos)+", "
				+"YPos               =  "+POut.Double(mapArea.YPos)+", "
				+"Width              =  "+POut.Double(mapArea.Width)+", "
				+"Height             =  "+POut.Double(mapArea.Height)+", "
				+"Description        = '"+POut.String(mapArea.Description)+"', "
				+"ItemType           =  "+POut.Int   ((int)mapArea.ItemType)+", "
				+"MapAreaContainerNum=  "+POut.Long  (mapArea.MapAreaContainerNum)+", "
				+"FontSize           =  "+POut.Float (mapArea.FontSize)+" "
				+"WHERE MapAreaNum = "+POut.Long(mapArea.MapAreaNum);
			Db.NonQ(command);
		}

		///<summary>Updates one MapArea in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(MapArea mapArea,MapArea oldMapArea) {
			string command="";
			if(mapArea.Extension != oldMapArea.Extension) {
				if(command!="") { command+=",";}
				command+="Extension = "+POut.Int(mapArea.Extension)+"";
			}
			if(mapArea.XPos != oldMapArea.XPos) {
				if(command!="") { command+=",";}
				command+="XPos = "+POut.Double(mapArea.XPos)+"";
			}
			if(mapArea.YPos != oldMapArea.YPos) {
				if(command!="") { command+=",";}
				command+="YPos = "+POut.Double(mapArea.YPos)+"";
			}
			if(mapArea.Width != oldMapArea.Width) {
				if(command!="") { command+=",";}
				command+="Width = "+POut.Double(mapArea.Width)+"";
			}
			if(mapArea.Height != oldMapArea.Height) {
				if(command!="") { command+=",";}
				command+="Height = "+POut.Double(mapArea.Height)+"";
			}
			if(mapArea.Description != oldMapArea.Description) {
				if(command!="") { command+=",";}
				command+="Description = '"+POut.String(mapArea.Description)+"'";
			}
			if(mapArea.ItemType != oldMapArea.ItemType) {
				if(command!="") { command+=",";}
				command+="ItemType = "+POut.Int   ((int)mapArea.ItemType)+"";
			}
			if(mapArea.MapAreaContainerNum != oldMapArea.MapAreaContainerNum) {
				if(command!="") { command+=",";}
				command+="MapAreaContainerNum = "+POut.Long(mapArea.MapAreaContainerNum)+"";
			}
			if(mapArea.FontSize != oldMapArea.FontSize) {
				if(command!="") { command+=",";}
				command+="FontSize = "+POut.Float(mapArea.FontSize)+"";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE maparea SET "+command
				+" WHERE MapAreaNum = "+POut.Long(mapArea.MapAreaNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(MapArea,MapArea) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(MapArea mapArea,MapArea oldMapArea) {
			if(mapArea.Extension != oldMapArea.Extension) {
				return true;
			}
			if(mapArea.XPos != oldMapArea.XPos) {
				return true;
			}
			if(mapArea.YPos != oldMapArea.YPos) {
				return true;
			}
			if(mapArea.Width != oldMapArea.Width) {
				return true;
			}
			if(mapArea.Height != oldMapArea.Height) {
				return true;
			}
			if(mapArea.Description != oldMapArea.Description) {
				return true;
			}
			if(mapArea.ItemType != oldMapArea.ItemType) {
				return true;
			}
			if(mapArea.MapAreaContainerNum != oldMapArea.MapAreaContainerNum) {
				return true;
			}
			if(mapArea.FontSize != oldMapArea.FontSize) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one MapArea from the database.</summary>
		public static void Delete(long mapAreaNum) {
			string command="DELETE FROM maparea "
				+"WHERE MapAreaNum = "+POut.Long(mapAreaNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many MapAreas from the database.</summary>
		public static void DeleteMany(List<long> listMapAreaNums) {
			if(listMapAreaNums==null || listMapAreaNums.Count==0) {
				return;
			}
			string command="DELETE FROM maparea "
				+"WHERE MapAreaNum IN("+string.Join(",",listMapAreaNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

	}
}