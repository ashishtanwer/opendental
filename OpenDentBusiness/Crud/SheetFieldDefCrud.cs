//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness.Crud{
	public class SheetFieldDefCrud {
		///<summary>Gets one SheetFieldDef object from the database using the primary key.  Returns null if not found.</summary>
		public static SheetFieldDef SelectOne(long sheetFieldDefNum) {
			string command="SELECT * FROM sheetfielddef "
				+"WHERE SheetFieldDefNum = "+POut.Long(sheetFieldDefNum);
			List<SheetFieldDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SheetFieldDef object from the database using a query.</summary>
		public static SheetFieldDef SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SheetFieldDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SheetFieldDef objects from the database using a query.</summary>
		public static List<SheetFieldDef> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SheetFieldDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<SheetFieldDef> TableToList(DataTable table) {
			List<SheetFieldDef> retVal=new List<SheetFieldDef>();
			SheetFieldDef sheetFieldDef;
			foreach(DataRow row in table.Rows) {
				sheetFieldDef=new SheetFieldDef();
				sheetFieldDef.SheetFieldDefNum        = PIn.Long  (row["SheetFieldDefNum"].ToString());
				sheetFieldDef.SheetDefNum             = PIn.Long  (row["SheetDefNum"].ToString());
				sheetFieldDef.FieldType               = (OpenDentBusiness.SheetFieldType)PIn.Int(row["FieldType"].ToString());
				sheetFieldDef.FieldName               = PIn.String(row["FieldName"].ToString());
				sheetFieldDef.FieldValue              = PIn.String(row["FieldValue"].ToString());
				sheetFieldDef.FontSize                = PIn.Float (row["FontSize"].ToString());
				sheetFieldDef.FontName                = PIn.String(row["FontName"].ToString());
				sheetFieldDef.FontIsBold              = PIn.Bool  (row["FontIsBold"].ToString());
				sheetFieldDef.XPos                    = PIn.Int   (row["XPos"].ToString());
				sheetFieldDef.YPos                    = PIn.Int   (row["YPos"].ToString());
				sheetFieldDef.Width                   = PIn.Int   (row["Width"].ToString());
				sheetFieldDef.Height                  = PIn.Int   (row["Height"].ToString());
				sheetFieldDef.GrowthBehavior          = (OpenDentBusiness.GrowthBehaviorEnum)PIn.Int(row["GrowthBehavior"].ToString());
				sheetFieldDef.RadioButtonValue        = PIn.String(row["RadioButtonValue"].ToString());
				sheetFieldDef.RadioButtonGroup        = PIn.String(row["RadioButtonGroup"].ToString());
				sheetFieldDef.IsRequired              = PIn.Bool  (row["IsRequired"].ToString());
				sheetFieldDef.TabOrder                = PIn.Int   (row["TabOrder"].ToString());
				sheetFieldDef.ReportableName          = PIn.String(row["ReportableName"].ToString());
				sheetFieldDef.TextAlign               = (System.Windows.Forms.HorizontalAlignment)PIn.Int(row["TextAlign"].ToString());
				sheetFieldDef.IsPaymentOption         = PIn.Bool  (row["IsPaymentOption"].ToString());
				sheetFieldDef.IsLocked                = PIn.Bool  (row["IsLocked"].ToString());
				sheetFieldDef.ItemColor               = Color.FromArgb(PIn.Int(row["ItemColor"].ToString()));
				sheetFieldDef.TabOrderMobile          = PIn.Int   (row["TabOrderMobile"].ToString());
				sheetFieldDef.UiLabelMobile           = PIn.String(row["UiLabelMobile"].ToString());
				sheetFieldDef.UiLabelMobileRadioButton= PIn.String(row["UiLabelMobileRadioButton"].ToString());
				sheetFieldDef.LayoutMode              = (OpenDentBusiness.SheetFieldLayoutMode)PIn.Int(row["LayoutMode"].ToString());
				sheetFieldDef.Language                = PIn.String(row["Language"].ToString());
				sheetFieldDef.CanElectronicallySign   = PIn.Bool  (row["CanElectronicallySign"].ToString());
				sheetFieldDef.IsSigProvRestricted     = PIn.Bool  (row["IsSigProvRestricted"].ToString());
				retVal.Add(sheetFieldDef);
			}
			return retVal;
		}

		///<summary>Converts a list of SheetFieldDef into a DataTable.</summary>
		public static DataTable ListToTable(List<SheetFieldDef> listSheetFieldDefs,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="SheetFieldDef";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("SheetFieldDefNum");
			table.Columns.Add("SheetDefNum");
			table.Columns.Add("FieldType");
			table.Columns.Add("FieldName");
			table.Columns.Add("FieldValue");
			table.Columns.Add("FontSize");
			table.Columns.Add("FontName");
			table.Columns.Add("FontIsBold");
			table.Columns.Add("XPos");
			table.Columns.Add("YPos");
			table.Columns.Add("Width");
			table.Columns.Add("Height");
			table.Columns.Add("GrowthBehavior");
			table.Columns.Add("RadioButtonValue");
			table.Columns.Add("RadioButtonGroup");
			table.Columns.Add("IsRequired");
			table.Columns.Add("TabOrder");
			table.Columns.Add("ReportableName");
			table.Columns.Add("TextAlign");
			table.Columns.Add("IsPaymentOption");
			table.Columns.Add("IsLocked");
			table.Columns.Add("ItemColor");
			table.Columns.Add("TabOrderMobile");
			table.Columns.Add("UiLabelMobile");
			table.Columns.Add("UiLabelMobileRadioButton");
			table.Columns.Add("LayoutMode");
			table.Columns.Add("Language");
			table.Columns.Add("CanElectronicallySign");
			table.Columns.Add("IsSigProvRestricted");
			foreach(SheetFieldDef sheetFieldDef in listSheetFieldDefs) {
				table.Rows.Add(new object[] {
					POut.Long  (sheetFieldDef.SheetFieldDefNum),
					POut.Long  (sheetFieldDef.SheetDefNum),
					POut.Int   ((int)sheetFieldDef.FieldType),
					            sheetFieldDef.FieldName,
					            sheetFieldDef.FieldValue,
					POut.Float (sheetFieldDef.FontSize),
					            sheetFieldDef.FontName,
					POut.Bool  (sheetFieldDef.FontIsBold),
					POut.Int   (sheetFieldDef.XPos),
					POut.Int   (sheetFieldDef.YPos),
					POut.Int   (sheetFieldDef.Width),
					POut.Int   (sheetFieldDef.Height),
					POut.Int   ((int)sheetFieldDef.GrowthBehavior),
					            sheetFieldDef.RadioButtonValue,
					            sheetFieldDef.RadioButtonGroup,
					POut.Bool  (sheetFieldDef.IsRequired),
					POut.Int   (sheetFieldDef.TabOrder),
					            sheetFieldDef.ReportableName,
					POut.Int   ((int)sheetFieldDef.TextAlign),
					POut.Bool  (sheetFieldDef.IsPaymentOption),
					POut.Bool  (sheetFieldDef.IsLocked),
					POut.Int   (sheetFieldDef.ItemColor.ToArgb()),
					POut.Int   (sheetFieldDef.TabOrderMobile),
					            sheetFieldDef.UiLabelMobile,
					            sheetFieldDef.UiLabelMobileRadioButton,
					POut.Int   ((int)sheetFieldDef.LayoutMode),
					            sheetFieldDef.Language,
					POut.Bool  (sheetFieldDef.CanElectronicallySign),
					POut.Bool  (sheetFieldDef.IsSigProvRestricted),
				});
			}
			return table;
		}

		///<summary>Inserts one SheetFieldDef into the database.  Returns the new priKey.</summary>
		public static long Insert(SheetFieldDef sheetFieldDef) {
			return Insert(sheetFieldDef,false);
		}

		///<summary>Inserts one SheetFieldDef into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(SheetFieldDef sheetFieldDef,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				sheetFieldDef.SheetFieldDefNum=ReplicationServers.GetKey("sheetfielddef","SheetFieldDefNum");
			}
			string command="INSERT INTO sheetfielddef (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SheetFieldDefNum,";
			}
			command+="SheetDefNum,FieldType,FieldName,FieldValue,FontSize,FontName,FontIsBold,XPos,YPos,Width,Height,GrowthBehavior,RadioButtonValue,RadioButtonGroup,IsRequired,TabOrder,ReportableName,TextAlign,IsPaymentOption,IsLocked,ItemColor,TabOrderMobile,UiLabelMobile,UiLabelMobileRadioButton,LayoutMode,Language,CanElectronicallySign,IsSigProvRestricted) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(sheetFieldDef.SheetFieldDefNum)+",";
			}
			command+=
				     POut.Long  (sheetFieldDef.SheetDefNum)+","
				+    POut.Int   ((int)sheetFieldDef.FieldType)+","
				+"'"+POut.String(sheetFieldDef.FieldName)+"',"
				+    DbHelper.ParamChar+"paramFieldValue,"
				+    POut.Float (sheetFieldDef.FontSize)+","
				+"'"+POut.String(sheetFieldDef.FontName)+"',"
				+    POut.Bool  (sheetFieldDef.FontIsBold)+","
				+    POut.Int   (sheetFieldDef.XPos)+","
				+    POut.Int   (sheetFieldDef.YPos)+","
				+    POut.Int   (sheetFieldDef.Width)+","
				+    POut.Int   (sheetFieldDef.Height)+","
				+    POut.Int   ((int)sheetFieldDef.GrowthBehavior)+","
				+"'"+POut.String(sheetFieldDef.RadioButtonValue)+"',"
				+"'"+POut.String(sheetFieldDef.RadioButtonGroup)+"',"
				+    POut.Bool  (sheetFieldDef.IsRequired)+","
				+    POut.Int   (sheetFieldDef.TabOrder)+","
				+"'"+POut.String(sheetFieldDef.ReportableName)+"',"
				+    POut.Int   ((int)sheetFieldDef.TextAlign)+","
				+    POut.Bool  (sheetFieldDef.IsPaymentOption)+","
				+    POut.Bool  (sheetFieldDef.IsLocked)+","
				+    POut.Int   (sheetFieldDef.ItemColor.ToArgb())+","
				+    POut.Int   (sheetFieldDef.TabOrderMobile)+","
				+    DbHelper.ParamChar+"paramUiLabelMobile,"
				+    DbHelper.ParamChar+"paramUiLabelMobileRadioButton,"
				+    POut.Int   ((int)sheetFieldDef.LayoutMode)+","
				+"'"+POut.String(sheetFieldDef.Language)+"',"
				+    POut.Bool  (sheetFieldDef.CanElectronicallySign)+","
				+    POut.Bool  (sheetFieldDef.IsSigProvRestricted)+")";
			if(sheetFieldDef.FieldValue==null) {
				sheetFieldDef.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(sheetFieldDef.FieldValue));
			if(sheetFieldDef.UiLabelMobile==null) {
				sheetFieldDef.UiLabelMobile="";
			}
			OdSqlParameter paramUiLabelMobile=new OdSqlParameter("paramUiLabelMobile",OdDbType.Text,POut.StringParam(sheetFieldDef.UiLabelMobile));
			if(sheetFieldDef.UiLabelMobileRadioButton==null) {
				sheetFieldDef.UiLabelMobileRadioButton="";
			}
			OdSqlParameter paramUiLabelMobileRadioButton=new OdSqlParameter("paramUiLabelMobileRadioButton",OdDbType.Text,POut.StringParam(sheetFieldDef.UiLabelMobileRadioButton));
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramFieldValue,paramUiLabelMobile,paramUiLabelMobileRadioButton);
			}
			else {
				sheetFieldDef.SheetFieldDefNum=Db.NonQ(command,true,"SheetFieldDefNum","sheetFieldDef",paramFieldValue,paramUiLabelMobile,paramUiLabelMobileRadioButton);
			}
			return sheetFieldDef.SheetFieldDefNum;
		}

		///<summary>Inserts one SheetFieldDef into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SheetFieldDef sheetFieldDef) {
			return InsertNoCache(sheetFieldDef,false);
		}

		///<summary>Inserts one SheetFieldDef into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(SheetFieldDef sheetFieldDef,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO sheetfielddef (";
			if(!useExistingPK && isRandomKeys) {
				sheetFieldDef.SheetFieldDefNum=ReplicationServers.GetKeyNoCache("sheetfielddef","SheetFieldDefNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="SheetFieldDefNum,";
			}
			command+="SheetDefNum,FieldType,FieldName,FieldValue,FontSize,FontName,FontIsBold,XPos,YPos,Width,Height,GrowthBehavior,RadioButtonValue,RadioButtonGroup,IsRequired,TabOrder,ReportableName,TextAlign,IsPaymentOption,IsLocked,ItemColor,TabOrderMobile,UiLabelMobile,UiLabelMobileRadioButton,LayoutMode,Language,CanElectronicallySign,IsSigProvRestricted) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(sheetFieldDef.SheetFieldDefNum)+",";
			}
			command+=
				     POut.Long  (sheetFieldDef.SheetDefNum)+","
				+    POut.Int   ((int)sheetFieldDef.FieldType)+","
				+"'"+POut.String(sheetFieldDef.FieldName)+"',"
				+    DbHelper.ParamChar+"paramFieldValue,"
				+    POut.Float (sheetFieldDef.FontSize)+","
				+"'"+POut.String(sheetFieldDef.FontName)+"',"
				+    POut.Bool  (sheetFieldDef.FontIsBold)+","
				+    POut.Int   (sheetFieldDef.XPos)+","
				+    POut.Int   (sheetFieldDef.YPos)+","
				+    POut.Int   (sheetFieldDef.Width)+","
				+    POut.Int   (sheetFieldDef.Height)+","
				+    POut.Int   ((int)sheetFieldDef.GrowthBehavior)+","
				+"'"+POut.String(sheetFieldDef.RadioButtonValue)+"',"
				+"'"+POut.String(sheetFieldDef.RadioButtonGroup)+"',"
				+    POut.Bool  (sheetFieldDef.IsRequired)+","
				+    POut.Int   (sheetFieldDef.TabOrder)+","
				+"'"+POut.String(sheetFieldDef.ReportableName)+"',"
				+    POut.Int   ((int)sheetFieldDef.TextAlign)+","
				+    POut.Bool  (sheetFieldDef.IsPaymentOption)+","
				+    POut.Bool  (sheetFieldDef.IsLocked)+","
				+    POut.Int   (sheetFieldDef.ItemColor.ToArgb())+","
				+    POut.Int   (sheetFieldDef.TabOrderMobile)+","
				+    DbHelper.ParamChar+"paramUiLabelMobile,"
				+    DbHelper.ParamChar+"paramUiLabelMobileRadioButton,"
				+    POut.Int   ((int)sheetFieldDef.LayoutMode)+","
				+"'"+POut.String(sheetFieldDef.Language)+"',"
				+    POut.Bool  (sheetFieldDef.CanElectronicallySign)+","
				+    POut.Bool  (sheetFieldDef.IsSigProvRestricted)+")";
			if(sheetFieldDef.FieldValue==null) {
				sheetFieldDef.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(sheetFieldDef.FieldValue));
			if(sheetFieldDef.UiLabelMobile==null) {
				sheetFieldDef.UiLabelMobile="";
			}
			OdSqlParameter paramUiLabelMobile=new OdSqlParameter("paramUiLabelMobile",OdDbType.Text,POut.StringParam(sheetFieldDef.UiLabelMobile));
			if(sheetFieldDef.UiLabelMobileRadioButton==null) {
				sheetFieldDef.UiLabelMobileRadioButton="";
			}
			OdSqlParameter paramUiLabelMobileRadioButton=new OdSqlParameter("paramUiLabelMobileRadioButton",OdDbType.Text,POut.StringParam(sheetFieldDef.UiLabelMobileRadioButton));
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command,paramFieldValue,paramUiLabelMobile,paramUiLabelMobileRadioButton);
			}
			else {
				sheetFieldDef.SheetFieldDefNum=Db.NonQ(command,true,"SheetFieldDefNum","sheetFieldDef",paramFieldValue,paramUiLabelMobile,paramUiLabelMobileRadioButton);
			}
			return sheetFieldDef.SheetFieldDefNum;
		}

		///<summary>Updates one SheetFieldDef in the database.</summary>
		public static void Update(SheetFieldDef sheetFieldDef) {
			string command="UPDATE sheetfielddef SET "
				+"SheetDefNum             =  "+POut.Long  (sheetFieldDef.SheetDefNum)+", "
				+"FieldType               =  "+POut.Int   ((int)sheetFieldDef.FieldType)+", "
				+"FieldName               = '"+POut.String(sheetFieldDef.FieldName)+"', "
				+"FieldValue              =  "+DbHelper.ParamChar+"paramFieldValue, "
				+"FontSize                =  "+POut.Float (sheetFieldDef.FontSize)+", "
				+"FontName                = '"+POut.String(sheetFieldDef.FontName)+"', "
				+"FontIsBold              =  "+POut.Bool  (sheetFieldDef.FontIsBold)+", "
				+"XPos                    =  "+POut.Int   (sheetFieldDef.XPos)+", "
				+"YPos                    =  "+POut.Int   (sheetFieldDef.YPos)+", "
				+"Width                   =  "+POut.Int   (sheetFieldDef.Width)+", "
				+"Height                  =  "+POut.Int   (sheetFieldDef.Height)+", "
				+"GrowthBehavior          =  "+POut.Int   ((int)sheetFieldDef.GrowthBehavior)+", "
				+"RadioButtonValue        = '"+POut.String(sheetFieldDef.RadioButtonValue)+"', "
				+"RadioButtonGroup        = '"+POut.String(sheetFieldDef.RadioButtonGroup)+"', "
				+"IsRequired              =  "+POut.Bool  (sheetFieldDef.IsRequired)+", "
				+"TabOrder                =  "+POut.Int   (sheetFieldDef.TabOrder)+", "
				+"ReportableName          = '"+POut.String(sheetFieldDef.ReportableName)+"', "
				+"TextAlign               =  "+POut.Int   ((int)sheetFieldDef.TextAlign)+", "
				+"IsPaymentOption         =  "+POut.Bool  (sheetFieldDef.IsPaymentOption)+", "
				+"IsLocked                =  "+POut.Bool  (sheetFieldDef.IsLocked)+", "
				+"ItemColor               =  "+POut.Int   (sheetFieldDef.ItemColor.ToArgb())+", "
				+"TabOrderMobile          =  "+POut.Int   (sheetFieldDef.TabOrderMobile)+", "
				+"UiLabelMobile           =  "+DbHelper.ParamChar+"paramUiLabelMobile, "
				+"UiLabelMobileRadioButton=  "+DbHelper.ParamChar+"paramUiLabelMobileRadioButton, "
				+"LayoutMode              =  "+POut.Int   ((int)sheetFieldDef.LayoutMode)+", "
				+"Language                = '"+POut.String(sheetFieldDef.Language)+"', "
				+"CanElectronicallySign   =  "+POut.Bool  (sheetFieldDef.CanElectronicallySign)+", "
				+"IsSigProvRestricted     =  "+POut.Bool  (sheetFieldDef.IsSigProvRestricted)+" "
				+"WHERE SheetFieldDefNum = "+POut.Long(sheetFieldDef.SheetFieldDefNum);
			if(sheetFieldDef.FieldValue==null) {
				sheetFieldDef.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(sheetFieldDef.FieldValue));
			if(sheetFieldDef.UiLabelMobile==null) {
				sheetFieldDef.UiLabelMobile="";
			}
			OdSqlParameter paramUiLabelMobile=new OdSqlParameter("paramUiLabelMobile",OdDbType.Text,POut.StringParam(sheetFieldDef.UiLabelMobile));
			if(sheetFieldDef.UiLabelMobileRadioButton==null) {
				sheetFieldDef.UiLabelMobileRadioButton="";
			}
			OdSqlParameter paramUiLabelMobileRadioButton=new OdSqlParameter("paramUiLabelMobileRadioButton",OdDbType.Text,POut.StringParam(sheetFieldDef.UiLabelMobileRadioButton));
			Db.NonQ(command,paramFieldValue,paramUiLabelMobile,paramUiLabelMobileRadioButton);
		}

		///<summary>Updates one SheetFieldDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(SheetFieldDef sheetFieldDef,SheetFieldDef oldSheetFieldDef) {
			string command="";
			if(sheetFieldDef.SheetDefNum != oldSheetFieldDef.SheetDefNum) {
				if(command!="") { command+=",";}
				command+="SheetDefNum = "+POut.Long(sheetFieldDef.SheetDefNum)+"";
			}
			if(sheetFieldDef.FieldType != oldSheetFieldDef.FieldType) {
				if(command!="") { command+=",";}
				command+="FieldType = "+POut.Int   ((int)sheetFieldDef.FieldType)+"";
			}
			if(sheetFieldDef.FieldName != oldSheetFieldDef.FieldName) {
				if(command!="") { command+=",";}
				command+="FieldName = '"+POut.String(sheetFieldDef.FieldName)+"'";
			}
			if(sheetFieldDef.FieldValue != oldSheetFieldDef.FieldValue) {
				if(command!="") { command+=",";}
				command+="FieldValue = "+DbHelper.ParamChar+"paramFieldValue";
			}
			if(sheetFieldDef.FontSize != oldSheetFieldDef.FontSize) {
				if(command!="") { command+=",";}
				command+="FontSize = "+POut.Float(sheetFieldDef.FontSize)+"";
			}
			if(sheetFieldDef.FontName != oldSheetFieldDef.FontName) {
				if(command!="") { command+=",";}
				command+="FontName = '"+POut.String(sheetFieldDef.FontName)+"'";
			}
			if(sheetFieldDef.FontIsBold != oldSheetFieldDef.FontIsBold) {
				if(command!="") { command+=",";}
				command+="FontIsBold = "+POut.Bool(sheetFieldDef.FontIsBold)+"";
			}
			if(sheetFieldDef.XPos != oldSheetFieldDef.XPos) {
				if(command!="") { command+=",";}
				command+="XPos = "+POut.Int(sheetFieldDef.XPos)+"";
			}
			if(sheetFieldDef.YPos != oldSheetFieldDef.YPos) {
				if(command!="") { command+=",";}
				command+="YPos = "+POut.Int(sheetFieldDef.YPos)+"";
			}
			if(sheetFieldDef.Width != oldSheetFieldDef.Width) {
				if(command!="") { command+=",";}
				command+="Width = "+POut.Int(sheetFieldDef.Width)+"";
			}
			if(sheetFieldDef.Height != oldSheetFieldDef.Height) {
				if(command!="") { command+=",";}
				command+="Height = "+POut.Int(sheetFieldDef.Height)+"";
			}
			if(sheetFieldDef.GrowthBehavior != oldSheetFieldDef.GrowthBehavior) {
				if(command!="") { command+=",";}
				command+="GrowthBehavior = "+POut.Int   ((int)sheetFieldDef.GrowthBehavior)+"";
			}
			if(sheetFieldDef.RadioButtonValue != oldSheetFieldDef.RadioButtonValue) {
				if(command!="") { command+=",";}
				command+="RadioButtonValue = '"+POut.String(sheetFieldDef.RadioButtonValue)+"'";
			}
			if(sheetFieldDef.RadioButtonGroup != oldSheetFieldDef.RadioButtonGroup) {
				if(command!="") { command+=",";}
				command+="RadioButtonGroup = '"+POut.String(sheetFieldDef.RadioButtonGroup)+"'";
			}
			if(sheetFieldDef.IsRequired != oldSheetFieldDef.IsRequired) {
				if(command!="") { command+=",";}
				command+="IsRequired = "+POut.Bool(sheetFieldDef.IsRequired)+"";
			}
			if(sheetFieldDef.TabOrder != oldSheetFieldDef.TabOrder) {
				if(command!="") { command+=",";}
				command+="TabOrder = "+POut.Int(sheetFieldDef.TabOrder)+"";
			}
			if(sheetFieldDef.ReportableName != oldSheetFieldDef.ReportableName) {
				if(command!="") { command+=",";}
				command+="ReportableName = '"+POut.String(sheetFieldDef.ReportableName)+"'";
			}
			if(sheetFieldDef.TextAlign != oldSheetFieldDef.TextAlign) {
				if(command!="") { command+=",";}
				command+="TextAlign = "+POut.Int   ((int)sheetFieldDef.TextAlign)+"";
			}
			if(sheetFieldDef.IsPaymentOption != oldSheetFieldDef.IsPaymentOption) {
				if(command!="") { command+=",";}
				command+="IsPaymentOption = "+POut.Bool(sheetFieldDef.IsPaymentOption)+"";
			}
			if(sheetFieldDef.IsLocked != oldSheetFieldDef.IsLocked) {
				if(command!="") { command+=",";}
				command+="IsLocked = "+POut.Bool(sheetFieldDef.IsLocked)+"";
			}
			if(sheetFieldDef.ItemColor != oldSheetFieldDef.ItemColor) {
				if(command!="") { command+=",";}
				command+="ItemColor = "+POut.Int(sheetFieldDef.ItemColor.ToArgb())+"";
			}
			if(sheetFieldDef.TabOrderMobile != oldSheetFieldDef.TabOrderMobile) {
				if(command!="") { command+=",";}
				command+="TabOrderMobile = "+POut.Int(sheetFieldDef.TabOrderMobile)+"";
			}
			if(sheetFieldDef.UiLabelMobile != oldSheetFieldDef.UiLabelMobile) {
				if(command!="") { command+=",";}
				command+="UiLabelMobile = "+DbHelper.ParamChar+"paramUiLabelMobile";
			}
			if(sheetFieldDef.UiLabelMobileRadioButton != oldSheetFieldDef.UiLabelMobileRadioButton) {
				if(command!="") { command+=",";}
				command+="UiLabelMobileRadioButton = "+DbHelper.ParamChar+"paramUiLabelMobileRadioButton";
			}
			if(sheetFieldDef.LayoutMode != oldSheetFieldDef.LayoutMode) {
				if(command!="") { command+=",";}
				command+="LayoutMode = "+POut.Int   ((int)sheetFieldDef.LayoutMode)+"";
			}
			if(sheetFieldDef.Language != oldSheetFieldDef.Language) {
				if(command!="") { command+=",";}
				command+="Language = '"+POut.String(sheetFieldDef.Language)+"'";
			}
			if(sheetFieldDef.CanElectronicallySign != oldSheetFieldDef.CanElectronicallySign) {
				if(command!="") { command+=",";}
				command+="CanElectronicallySign = "+POut.Bool(sheetFieldDef.CanElectronicallySign)+"";
			}
			if(sheetFieldDef.IsSigProvRestricted != oldSheetFieldDef.IsSigProvRestricted) {
				if(command!="") { command+=",";}
				command+="IsSigProvRestricted = "+POut.Bool(sheetFieldDef.IsSigProvRestricted)+"";
			}
			if(command=="") {
				return false;
			}
			if(sheetFieldDef.FieldValue==null) {
				sheetFieldDef.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,POut.StringParam(sheetFieldDef.FieldValue));
			if(sheetFieldDef.UiLabelMobile==null) {
				sheetFieldDef.UiLabelMobile="";
			}
			OdSqlParameter paramUiLabelMobile=new OdSqlParameter("paramUiLabelMobile",OdDbType.Text,POut.StringParam(sheetFieldDef.UiLabelMobile));
			if(sheetFieldDef.UiLabelMobileRadioButton==null) {
				sheetFieldDef.UiLabelMobileRadioButton="";
			}
			OdSqlParameter paramUiLabelMobileRadioButton=new OdSqlParameter("paramUiLabelMobileRadioButton",OdDbType.Text,POut.StringParam(sheetFieldDef.UiLabelMobileRadioButton));
			command="UPDATE sheetfielddef SET "+command
				+" WHERE SheetFieldDefNum = "+POut.Long(sheetFieldDef.SheetFieldDefNum);
			Db.NonQ(command,paramFieldValue,paramUiLabelMobile,paramUiLabelMobileRadioButton);
			return true;
		}

		///<summary>Returns true if Update(SheetFieldDef,SheetFieldDef) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(SheetFieldDef sheetFieldDef,SheetFieldDef oldSheetFieldDef) {
			if(sheetFieldDef.SheetDefNum != oldSheetFieldDef.SheetDefNum) {
				return true;
			}
			if(sheetFieldDef.FieldType != oldSheetFieldDef.FieldType) {
				return true;
			}
			if(sheetFieldDef.FieldName != oldSheetFieldDef.FieldName) {
				return true;
			}
			if(sheetFieldDef.FieldValue != oldSheetFieldDef.FieldValue) {
				return true;
			}
			if(sheetFieldDef.FontSize != oldSheetFieldDef.FontSize) {
				return true;
			}
			if(sheetFieldDef.FontName != oldSheetFieldDef.FontName) {
				return true;
			}
			if(sheetFieldDef.FontIsBold != oldSheetFieldDef.FontIsBold) {
				return true;
			}
			if(sheetFieldDef.XPos != oldSheetFieldDef.XPos) {
				return true;
			}
			if(sheetFieldDef.YPos != oldSheetFieldDef.YPos) {
				return true;
			}
			if(sheetFieldDef.Width != oldSheetFieldDef.Width) {
				return true;
			}
			if(sheetFieldDef.Height != oldSheetFieldDef.Height) {
				return true;
			}
			if(sheetFieldDef.GrowthBehavior != oldSheetFieldDef.GrowthBehavior) {
				return true;
			}
			if(sheetFieldDef.RadioButtonValue != oldSheetFieldDef.RadioButtonValue) {
				return true;
			}
			if(sheetFieldDef.RadioButtonGroup != oldSheetFieldDef.RadioButtonGroup) {
				return true;
			}
			if(sheetFieldDef.IsRequired != oldSheetFieldDef.IsRequired) {
				return true;
			}
			if(sheetFieldDef.TabOrder != oldSheetFieldDef.TabOrder) {
				return true;
			}
			if(sheetFieldDef.ReportableName != oldSheetFieldDef.ReportableName) {
				return true;
			}
			if(sheetFieldDef.TextAlign != oldSheetFieldDef.TextAlign) {
				return true;
			}
			if(sheetFieldDef.IsPaymentOption != oldSheetFieldDef.IsPaymentOption) {
				return true;
			}
			if(sheetFieldDef.IsLocked != oldSheetFieldDef.IsLocked) {
				return true;
			}
			if(sheetFieldDef.ItemColor != oldSheetFieldDef.ItemColor) {
				return true;
			}
			if(sheetFieldDef.TabOrderMobile != oldSheetFieldDef.TabOrderMobile) {
				return true;
			}
			if(sheetFieldDef.UiLabelMobile != oldSheetFieldDef.UiLabelMobile) {
				return true;
			}
			if(sheetFieldDef.UiLabelMobileRadioButton != oldSheetFieldDef.UiLabelMobileRadioButton) {
				return true;
			}
			if(sheetFieldDef.LayoutMode != oldSheetFieldDef.LayoutMode) {
				return true;
			}
			if(sheetFieldDef.Language != oldSheetFieldDef.Language) {
				return true;
			}
			if(sheetFieldDef.CanElectronicallySign != oldSheetFieldDef.CanElectronicallySign) {
				return true;
			}
			if(sheetFieldDef.IsSigProvRestricted != oldSheetFieldDef.IsSigProvRestricted) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one SheetFieldDef from the database.</summary>
		public static void Delete(long sheetFieldDefNum) {
			string command="DELETE FROM sheetfielddef "
				+"WHERE SheetFieldDefNum = "+POut.Long(sheetFieldDefNum);
			Db.NonQ(command);
		}

		///<summary>Deletes many SheetFieldDefs from the database.</summary>
		public static void DeleteMany(List<long> listSheetFieldDefNums) {
			if(listSheetFieldDefNums==null || listSheetFieldDefNums.Count==0) {
				return;
			}
			string command="DELETE FROM sheetfielddef "
				+"WHERE SheetFieldDefNum IN("+string.Join(",",listSheetFieldDefNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<SheetFieldDef> listNew,List<SheetFieldDef> listDB) {
			//Adding items to lists changes the order of operation. All inserts are completed first, then updates, then deletes.
			List<SheetFieldDef> listIns    =new List<SheetFieldDef>();
			List<SheetFieldDef> listUpdNew =new List<SheetFieldDef>();
			List<SheetFieldDef> listUpdDB  =new List<SheetFieldDef>();
			List<SheetFieldDef> listDel    =new List<SheetFieldDef>();
			listNew.Sort((SheetFieldDef x,SheetFieldDef y) => { return x.SheetFieldDefNum.CompareTo(y.SheetFieldDefNum); });
			listDB.Sort((SheetFieldDef x,SheetFieldDef y) => { return x.SheetFieldDefNum.CompareTo(y.SheetFieldDefNum); });
			int idxNew=0;
			int idxDB=0;
			int rowsUpdatedCount=0;
			SheetFieldDef fieldNew;
			SheetFieldDef fieldDB;
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
				else if(fieldNew.SheetFieldDefNum<fieldDB.SheetFieldDefNum) {//newPK less than dbPK, newItem is 'next'
					listIns.Add(fieldNew);
					idxNew++;
					continue;
				}
				else if(fieldNew.SheetFieldDefNum>fieldDB.SheetFieldDefNum) {//dbPK less than newPK, dbItem is 'next'
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
			DeleteMany(listDel.Select(x => x.SheetFieldDefNum).ToList());
			if(rowsUpdatedCount>0 || listIns.Count>0 || listDel.Count>0) {
				return true;
			}
			return false;
		}

	}
}