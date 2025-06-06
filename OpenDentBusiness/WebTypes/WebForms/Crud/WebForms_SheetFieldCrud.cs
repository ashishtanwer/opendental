//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using OpenDentBusiness;
using OpenDentBusiness.WebTypes.WebForms;

namespace OpenDentBusiness.WebTypes.WebForms.Crud{
	public class WebForms_SheetFieldCrud {
		///<summary>Gets one WebForms_SheetField object from the database using the primary key.  Returns null if not found.</summary>
		public static WebForms_SheetField SelectOne(long sheetFieldID) {
			string command="SELECT * FROM webforms_sheetfield "
				+"WHERE SheetFieldID = "+POut.Long(sheetFieldID);
			List<WebForms_SheetField> list=TableToList(DataCore.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one WebForms_SheetField object from the database using a query.</summary>
		public static WebForms_SheetField SelectOne(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<WebForms_SheetField> list=TableToList(DataCore.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of WebForms_SheetField objects from the database using a query.</summary>
		public static List<WebForms_SheetField> SelectMany(string command) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<WebForms_SheetField> list=TableToList(DataCore.GetTable(command));
			return list;
		}

		///<summary>Converts a list of WebForms_SheetField into a DataTable.</summary>
		public static DataTable ListToTable(List<WebForms_SheetField> listWebForms_SheetFields,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="WebForms_SheetField";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("SheetFieldID");
			table.Columns.Add("SheetID");
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
			table.Columns.Add("ItemColor");
			table.Columns.Add("TabOrderMobile");
			table.Columns.Add("UiLabelMobile");
			table.Columns.Add("UiLabelMobileRadioButton");
			table.Columns.Add("SheetFieldDefNum");
			table.Columns.Add("CanElectronicallySign");
			table.Columns.Add("IsSigProvRestricted");
			foreach(WebForms_SheetField webForms_SheetField in listWebForms_SheetFields) {
				table.Rows.Add(new object[] {
					POut.Long  (webForms_SheetField.SheetFieldID),
					POut.Long  (webForms_SheetField.SheetID),
					POut.Int   ((int)webForms_SheetField.FieldType),
					            webForms_SheetField.FieldName,
					            webForms_SheetField.FieldValue,
					POut.Float (webForms_SheetField.FontSize),
					            webForms_SheetField.FontName,
					POut.Bool  (webForms_SheetField.FontIsBold),
					POut.Int   (webForms_SheetField.XPos),
					POut.Int   (webForms_SheetField.YPos),
					POut.Int   (webForms_SheetField.Width),
					POut.Int   (webForms_SheetField.Height),
					POut.Int   ((int)webForms_SheetField.GrowthBehavior),
					            webForms_SheetField.RadioButtonValue,
					            webForms_SheetField.RadioButtonGroup,
					POut.Bool  (webForms_SheetField.IsRequired),
					POut.Int   (webForms_SheetField.TabOrder),
					            webForms_SheetField.ReportableName,
					POut.Int   ((int)webForms_SheetField.TextAlign),
					POut.Int   (webForms_SheetField.ItemColor.ToArgb()),
					POut.Int   (webForms_SheetField.TabOrderMobile),
					            webForms_SheetField.UiLabelMobile,
					            webForms_SheetField.UiLabelMobileRadioButton,
					POut.Long  (webForms_SheetField.SheetFieldDefNum),
					POut.Bool  (webForms_SheetField.CanElectronicallySign),
					POut.Bool  (webForms_SheetField.IsSigProvRestricted),
				});
			}
			return table;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<WebForms_SheetField> TableToList(DataTable table) {
			List<WebForms_SheetField> retVal=new List<WebForms_SheetField>();
			WebForms_SheetField webForms_SheetField;
			for(int i=0;i<table.Rows.Count;i++) {
				webForms_SheetField=new WebForms_SheetField();
				webForms_SheetField.SheetFieldID            = PIn.Long  (table.Rows[i]["SheetFieldID"].ToString());
				webForms_SheetField.SheetID                 = PIn.Long  (table.Rows[i]["SheetID"].ToString());
				webForms_SheetField.FieldType               = (OpenDentBusiness.SheetFieldType)PIn.Int(table.Rows[i]["FieldType"].ToString());
				webForms_SheetField.FieldName               = PIn.String(table.Rows[i]["FieldName"].ToString());
				webForms_SheetField.FieldValue              = PIn.String(table.Rows[i]["FieldValue"].ToString());
				webForms_SheetField.FontSize                = PIn.Float (table.Rows[i]["FontSize"].ToString());
				webForms_SheetField.FontName                = PIn.String(table.Rows[i]["FontName"].ToString());
				webForms_SheetField.FontIsBold              = PIn.Bool  (table.Rows[i]["FontIsBold"].ToString());
				webForms_SheetField.XPos                    = PIn.Int   (table.Rows[i]["XPos"].ToString());
				webForms_SheetField.YPos                    = PIn.Int   (table.Rows[i]["YPos"].ToString());
				webForms_SheetField.Width                   = PIn.Int   (table.Rows[i]["Width"].ToString());
				webForms_SheetField.Height                  = PIn.Int   (table.Rows[i]["Height"].ToString());
				webForms_SheetField.GrowthBehavior          = (OpenDentBusiness.GrowthBehaviorEnum)PIn.Int(table.Rows[i]["GrowthBehavior"].ToString());
				webForms_SheetField.RadioButtonValue        = PIn.String(table.Rows[i]["RadioButtonValue"].ToString());
				webForms_SheetField.RadioButtonGroup        = PIn.String(table.Rows[i]["RadioButtonGroup"].ToString());
				webForms_SheetField.IsRequired              = PIn.Bool  (table.Rows[i]["IsRequired"].ToString());
				webForms_SheetField.TabOrder                = PIn.Int   (table.Rows[i]["TabOrder"].ToString());
				webForms_SheetField.ReportableName          = PIn.String(table.Rows[i]["ReportableName"].ToString());
				webForms_SheetField.TextAlign               = (System.Windows.Forms.HorizontalAlignment)PIn.Int(table.Rows[i]["TextAlign"].ToString());
				webForms_SheetField.ItemColor               = Color.FromArgb(PIn.Int(table.Rows[i]["ItemColor"].ToString()));
				webForms_SheetField.TabOrderMobile          = PIn.Int   (table.Rows[i]["TabOrderMobile"].ToString());
				webForms_SheetField.UiLabelMobile           = PIn.String(table.Rows[i]["UiLabelMobile"].ToString());
				webForms_SheetField.UiLabelMobileRadioButton= PIn.String(table.Rows[i]["UiLabelMobileRadioButton"].ToString());
				webForms_SheetField.SheetFieldDefNum        = PIn.Long  (table.Rows[i]["SheetFieldDefNum"].ToString());
				webForms_SheetField.CanElectronicallySign   = PIn.Bool  (table.Rows[i]["CanElectronicallySign"].ToString());
				webForms_SheetField.IsSigProvRestricted     = PIn.Bool  (table.Rows[i]["IsSigProvRestricted"].ToString());
				retVal.Add(webForms_SheetField);
			}
			return retVal;
		}

		///<summary>Inserts one WebForms_SheetField into the database.  Returns the new priKey.</summary>
		public static long Insert(WebForms_SheetField webForms_SheetField) {
			return Insert(webForms_SheetField,false);
		}

		///<summary>Inserts one WebForms_SheetField into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(WebForms_SheetField webForms_SheetField,bool useExistingPK) {
			string command="INSERT INTO webforms_sheetfield (";
			if(useExistingPK) {
				command+="SheetFieldID,";
			}
			command+="SheetID,FieldType,FieldName,FieldValue,FontSize,FontName,FontIsBold,XPos,YPos,Width,Height,GrowthBehavior,RadioButtonValue,RadioButtonGroup,IsRequired,TabOrder,ReportableName,TextAlign,ItemColor,TabOrderMobile,UiLabelMobile,UiLabelMobileRadioButton,SheetFieldDefNum,CanElectronicallySign,IsSigProvRestricted) VALUES(";
			if(useExistingPK) {
				command+=POut.Long(webForms_SheetField.SheetFieldID)+",";
			}
			command+=
				     POut.Long  (webForms_SheetField.SheetID)+","
				+    POut.Int   ((int)webForms_SheetField.FieldType)+","
				+"'"+POut.String(webForms_SheetField.FieldName)+"',"
				+    DbHelper.ParamChar+"paramFieldValue,"
				+    POut.Float (webForms_SheetField.FontSize)+","
				+"'"+POut.String(webForms_SheetField.FontName)+"',"
				+    POut.Bool  (webForms_SheetField.FontIsBold)+","
				+    POut.Int   (webForms_SheetField.XPos)+","
				+    POut.Int   (webForms_SheetField.YPos)+","
				+    POut.Int   (webForms_SheetField.Width)+","
				+    POut.Int   (webForms_SheetField.Height)+","
				+    POut.Int   ((int)webForms_SheetField.GrowthBehavior)+","
				+"'"+POut.String(webForms_SheetField.RadioButtonValue)+"',"
				+"'"+POut.String(webForms_SheetField.RadioButtonGroup)+"',"
				+    POut.Bool  (webForms_SheetField.IsRequired)+","
				+    POut.Int   (webForms_SheetField.TabOrder)+","
				+"'"+POut.String(webForms_SheetField.ReportableName)+"',"
				+    POut.Int   ((int)webForms_SheetField.TextAlign)+","
				+    POut.Int   (webForms_SheetField.ItemColor.ToArgb())+","
				+    POut.Int   (webForms_SheetField.TabOrderMobile)+","
				+"'"+POut.String(webForms_SheetField.UiLabelMobile)+"',"
				+"'"+POut.String(webForms_SheetField.UiLabelMobileRadioButton)+"',"
				+    POut.Long  (webForms_SheetField.SheetFieldDefNum)+","
				+    POut.Bool  (webForms_SheetField.CanElectronicallySign)+","
				+    POut.Bool  (webForms_SheetField.IsSigProvRestricted)+")";
			if(webForms_SheetField.FieldValue==null) {
				webForms_SheetField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,webForms_SheetField.FieldValue);
			if(useExistingPK) {
				DataCore.NonQ(command,paramFieldValue);
			}
			else {
				webForms_SheetField.SheetFieldID=DataCore.NonQ(command,true,paramFieldValue);
			}
			return webForms_SheetField.SheetFieldID;
		}

		///<summary>Inserts many WebForms_SheetFields into the database.</summary>
		public static void InsertMany(List<WebForms_SheetField> listWebForms_SheetFields) {
			InsertMany(listWebForms_SheetFields,false);
		}

		///<summary>Inserts many WebForms_SheetFields into the database.  Provides option to use the existing priKey.</summary>
		public static void InsertMany(List<WebForms_SheetField> listWebForms_SheetFields,bool useExistingPK) {
			StringBuilder sbCommands=null;
			int index=0;
			int countRows=0;
			while(index < listWebForms_SheetFields.Count) {
				WebForms_SheetField webForms_SheetField=listWebForms_SheetFields[index];
				StringBuilder sbRow=new StringBuilder("(");
				bool hasComma=false;
				if(sbCommands==null) {
					sbCommands=new StringBuilder();
					sbCommands.Append("INSERT INTO webforms_sheetfield (");
					if(useExistingPK) {
						sbCommands.Append("SheetFieldID,");
					}
					sbCommands.Append("SheetID,FieldType,FieldName,FieldValue,FontSize,FontName,FontIsBold,XPos,YPos,Width,Height,GrowthBehavior,RadioButtonValue,RadioButtonGroup,IsRequired,TabOrder,ReportableName,TextAlign,ItemColor,TabOrderMobile,UiLabelMobile,UiLabelMobileRadioButton,SheetFieldDefNum,CanElectronicallySign,IsSigProvRestricted) VALUES ");
					countRows=0;
				}
				else {
					hasComma=true;
				}
				if(useExistingPK) {
					sbRow.Append(POut.Long(webForms_SheetField.SheetFieldID)); sbRow.Append(",");
				}
				sbRow.Append(POut.Long(webForms_SheetField.SheetID)); sbRow.Append(",");
				sbRow.Append(POut.Int((int)webForms_SheetField.FieldType)); sbRow.Append(",");
				sbRow.Append("'"+POut.String(webForms_SheetField.FieldName)+"'"); sbRow.Append(",");
				sbRow.Append("'"+POut.String(webForms_SheetField.FieldValue)+"'"); sbRow.Append(",");
				sbRow.Append(POut.Float(webForms_SheetField.FontSize)); sbRow.Append(",");
				sbRow.Append("'"+POut.String(webForms_SheetField.FontName)+"'"); sbRow.Append(",");
				sbRow.Append(POut.Bool(webForms_SheetField.FontIsBold)); sbRow.Append(",");
				sbRow.Append(POut.Int(webForms_SheetField.XPos)); sbRow.Append(",");
				sbRow.Append(POut.Int(webForms_SheetField.YPos)); sbRow.Append(",");
				sbRow.Append(POut.Int(webForms_SheetField.Width)); sbRow.Append(",");
				sbRow.Append(POut.Int(webForms_SheetField.Height)); sbRow.Append(",");
				sbRow.Append(POut.Int((int)webForms_SheetField.GrowthBehavior)); sbRow.Append(",");
				sbRow.Append("'"+POut.String(webForms_SheetField.RadioButtonValue)+"'"); sbRow.Append(",");
				sbRow.Append("'"+POut.String(webForms_SheetField.RadioButtonGroup)+"'"); sbRow.Append(",");
				sbRow.Append(POut.Bool(webForms_SheetField.IsRequired)); sbRow.Append(",");
				sbRow.Append(POut.Int(webForms_SheetField.TabOrder)); sbRow.Append(",");
				sbRow.Append("'"+POut.String(webForms_SheetField.ReportableName)+"'"); sbRow.Append(",");
				sbRow.Append(POut.Int((int)webForms_SheetField.TextAlign)); sbRow.Append(",");
				sbRow.Append(POut.Int(webForms_SheetField.ItemColor.ToArgb())); sbRow.Append(",");
				sbRow.Append(POut.Int(webForms_SheetField.TabOrderMobile)); sbRow.Append(",");
				sbRow.Append("'"+POut.String(webForms_SheetField.UiLabelMobile)+"'"); sbRow.Append(",");
				sbRow.Append("'"+POut.String(webForms_SheetField.UiLabelMobileRadioButton)+"'"); sbRow.Append(",");
				sbRow.Append(POut.Long(webForms_SheetField.SheetFieldDefNum)); sbRow.Append(",");
				sbRow.Append(POut.Bool(webForms_SheetField.CanElectronicallySign)); sbRow.Append(",");
				sbRow.Append(POut.Bool(webForms_SheetField.IsSigProvRestricted)); sbRow.Append(")");
				if(sbCommands.Length+sbRow.Length+1 > TableBase.MaxAllowedPacketCount && countRows > 0) {
					DataCore.NonQ(sbCommands.ToString());
					sbCommands=null;
				}
				else {
					if(hasComma) {
						sbCommands.Append(",");
					}
					sbCommands.Append(sbRow.ToString());
					countRows++;
					if(index==listWebForms_SheetFields.Count-1) {
						DataCore.NonQ(sbCommands.ToString());
					}
					index++;
				}
			}
		}

		///<summary>Updates one WebForms_SheetField in the database.</summary>
		public static void Update(WebForms_SheetField webForms_SheetField) {
			string command="UPDATE webforms_sheetfield SET "
				+"SheetID                 =  "+POut.Long  (webForms_SheetField.SheetID)+", "
				+"FieldType               =  "+POut.Int   ((int)webForms_SheetField.FieldType)+", "
				+"FieldName               = '"+POut.String(webForms_SheetField.FieldName)+"', "
				+"FieldValue              =  "+DbHelper.ParamChar+"paramFieldValue, "
				+"FontSize                =  "+POut.Float (webForms_SheetField.FontSize)+", "
				+"FontName                = '"+POut.String(webForms_SheetField.FontName)+"', "
				+"FontIsBold              =  "+POut.Bool  (webForms_SheetField.FontIsBold)+", "
				+"XPos                    =  "+POut.Int   (webForms_SheetField.XPos)+", "
				+"YPos                    =  "+POut.Int   (webForms_SheetField.YPos)+", "
				+"Width                   =  "+POut.Int   (webForms_SheetField.Width)+", "
				+"Height                  =  "+POut.Int   (webForms_SheetField.Height)+", "
				+"GrowthBehavior          =  "+POut.Int   ((int)webForms_SheetField.GrowthBehavior)+", "
				+"RadioButtonValue        = '"+POut.String(webForms_SheetField.RadioButtonValue)+"', "
				+"RadioButtonGroup        = '"+POut.String(webForms_SheetField.RadioButtonGroup)+"', "
				+"IsRequired              =  "+POut.Bool  (webForms_SheetField.IsRequired)+", "
				+"TabOrder                =  "+POut.Int   (webForms_SheetField.TabOrder)+", "
				+"ReportableName          = '"+POut.String(webForms_SheetField.ReportableName)+"', "
				+"TextAlign               =  "+POut.Int   ((int)webForms_SheetField.TextAlign)+", "
				+"ItemColor               =  "+POut.Int   (webForms_SheetField.ItemColor.ToArgb())+", "
				+"TabOrderMobile          =  "+POut.Int   (webForms_SheetField.TabOrderMobile)+", "
				+"UiLabelMobile           = '"+POut.String(webForms_SheetField.UiLabelMobile)+"', "
				+"UiLabelMobileRadioButton= '"+POut.String(webForms_SheetField.UiLabelMobileRadioButton)+"', "
				+"SheetFieldDefNum        =  "+POut.Long  (webForms_SheetField.SheetFieldDefNum)+", "
				+"CanElectronicallySign   =  "+POut.Bool  (webForms_SheetField.CanElectronicallySign)+", "
				+"IsSigProvRestricted     =  "+POut.Bool  (webForms_SheetField.IsSigProvRestricted)+" "
				+"WHERE SheetFieldID = "+POut.Long(webForms_SheetField.SheetFieldID);
			if(webForms_SheetField.FieldValue==null) {
				webForms_SheetField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,webForms_SheetField.FieldValue);
			DataCore.NonQ(command,paramFieldValue);
		}

		///<summary>Updates one WebForms_SheetField in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(WebForms_SheetField webForms_SheetField,WebForms_SheetField oldWebForms_SheetField) {
			string command="";
			if(webForms_SheetField.SheetID != oldWebForms_SheetField.SheetID) {
				if(command!="") { command+=",";}
				command+="SheetID = "+POut.Long(webForms_SheetField.SheetID)+"";
			}
			if(webForms_SheetField.FieldType != oldWebForms_SheetField.FieldType) {
				if(command!="") { command+=",";}
				command+="FieldType = "+POut.Int   ((int)webForms_SheetField.FieldType)+"";
			}
			if(webForms_SheetField.FieldName != oldWebForms_SheetField.FieldName) {
				if(command!="") { command+=",";}
				command+="FieldName = '"+POut.String(webForms_SheetField.FieldName)+"'";
			}
			if(webForms_SheetField.FieldValue != oldWebForms_SheetField.FieldValue) {
				if(command!="") { command+=",";}
				command+="FieldValue = "+DbHelper.ParamChar+"paramFieldValue";
			}
			if(webForms_SheetField.FontSize != oldWebForms_SheetField.FontSize) {
				if(command!="") { command+=",";}
				command+="FontSize = "+POut.Float(webForms_SheetField.FontSize)+"";
			}
			if(webForms_SheetField.FontName != oldWebForms_SheetField.FontName) {
				if(command!="") { command+=",";}
				command+="FontName = '"+POut.String(webForms_SheetField.FontName)+"'";
			}
			if(webForms_SheetField.FontIsBold != oldWebForms_SheetField.FontIsBold) {
				if(command!="") { command+=",";}
				command+="FontIsBold = "+POut.Bool(webForms_SheetField.FontIsBold)+"";
			}
			if(webForms_SheetField.XPos != oldWebForms_SheetField.XPos) {
				if(command!="") { command+=",";}
				command+="XPos = "+POut.Int(webForms_SheetField.XPos)+"";
			}
			if(webForms_SheetField.YPos != oldWebForms_SheetField.YPos) {
				if(command!="") { command+=",";}
				command+="YPos = "+POut.Int(webForms_SheetField.YPos)+"";
			}
			if(webForms_SheetField.Width != oldWebForms_SheetField.Width) {
				if(command!="") { command+=",";}
				command+="Width = "+POut.Int(webForms_SheetField.Width)+"";
			}
			if(webForms_SheetField.Height != oldWebForms_SheetField.Height) {
				if(command!="") { command+=",";}
				command+="Height = "+POut.Int(webForms_SheetField.Height)+"";
			}
			if(webForms_SheetField.GrowthBehavior != oldWebForms_SheetField.GrowthBehavior) {
				if(command!="") { command+=",";}
				command+="GrowthBehavior = "+POut.Int   ((int)webForms_SheetField.GrowthBehavior)+"";
			}
			if(webForms_SheetField.RadioButtonValue != oldWebForms_SheetField.RadioButtonValue) {
				if(command!="") { command+=",";}
				command+="RadioButtonValue = '"+POut.String(webForms_SheetField.RadioButtonValue)+"'";
			}
			if(webForms_SheetField.RadioButtonGroup != oldWebForms_SheetField.RadioButtonGroup) {
				if(command!="") { command+=",";}
				command+="RadioButtonGroup = '"+POut.String(webForms_SheetField.RadioButtonGroup)+"'";
			}
			if(webForms_SheetField.IsRequired != oldWebForms_SheetField.IsRequired) {
				if(command!="") { command+=",";}
				command+="IsRequired = "+POut.Bool(webForms_SheetField.IsRequired)+"";
			}
			if(webForms_SheetField.TabOrder != oldWebForms_SheetField.TabOrder) {
				if(command!="") { command+=",";}
				command+="TabOrder = "+POut.Int(webForms_SheetField.TabOrder)+"";
			}
			if(webForms_SheetField.ReportableName != oldWebForms_SheetField.ReportableName) {
				if(command!="") { command+=",";}
				command+="ReportableName = '"+POut.String(webForms_SheetField.ReportableName)+"'";
			}
			if(webForms_SheetField.TextAlign != oldWebForms_SheetField.TextAlign) {
				if(command!="") { command+=",";}
				command+="TextAlign = "+POut.Int   ((int)webForms_SheetField.TextAlign)+"";
			}
			if(webForms_SheetField.ItemColor != oldWebForms_SheetField.ItemColor) {
				if(command!="") { command+=",";}
				command+="ItemColor = "+POut.Int(webForms_SheetField.ItemColor.ToArgb())+"";
			}
			if(webForms_SheetField.TabOrderMobile != oldWebForms_SheetField.TabOrderMobile) {
				if(command!="") { command+=",";}
				command+="TabOrderMobile = "+POut.Int(webForms_SheetField.TabOrderMobile)+"";
			}
			if(webForms_SheetField.UiLabelMobile != oldWebForms_SheetField.UiLabelMobile) {
				if(command!="") { command+=",";}
				command+="UiLabelMobile = '"+POut.String(webForms_SheetField.UiLabelMobile)+"'";
			}
			if(webForms_SheetField.UiLabelMobileRadioButton != oldWebForms_SheetField.UiLabelMobileRadioButton) {
				if(command!="") { command+=",";}
				command+="UiLabelMobileRadioButton = '"+POut.String(webForms_SheetField.UiLabelMobileRadioButton)+"'";
			}
			if(webForms_SheetField.SheetFieldDefNum != oldWebForms_SheetField.SheetFieldDefNum) {
				if(command!="") { command+=",";}
				command+="SheetFieldDefNum = "+POut.Long(webForms_SheetField.SheetFieldDefNum)+"";
			}
			if(webForms_SheetField.CanElectronicallySign != oldWebForms_SheetField.CanElectronicallySign) {
				if(command!="") { command+=",";}
				command+="CanElectronicallySign = "+POut.Bool(webForms_SheetField.CanElectronicallySign)+"";
			}
			if(webForms_SheetField.IsSigProvRestricted != oldWebForms_SheetField.IsSigProvRestricted) {
				if(command!="") { command+=",";}
				command+="IsSigProvRestricted = "+POut.Bool(webForms_SheetField.IsSigProvRestricted)+"";
			}
			if(command=="") {
				return false;
			}
			if(webForms_SheetField.FieldValue==null) {
				webForms_SheetField.FieldValue="";
			}
			OdSqlParameter paramFieldValue=new OdSqlParameter("paramFieldValue",OdDbType.Text,webForms_SheetField.FieldValue);
			command="UPDATE webforms_sheetfield SET "+command
				+" WHERE SheetFieldID = "+POut.Long(webForms_SheetField.SheetFieldID);
			DataCore.NonQ(command,paramFieldValue);
			return true;
		}

		///<summary>Deletes one WebForms_SheetField from the database.</summary>
		public static void Delete(long sheetFieldID) {
			string command="DELETE FROM webforms_sheetfield "
				+"WHERE SheetFieldID = "+POut.Long(sheetFieldID);
			DataCore.NonQ(command);
		}

	}
}