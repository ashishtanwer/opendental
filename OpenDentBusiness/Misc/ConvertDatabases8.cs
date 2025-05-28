using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Windows.Controls.Primitives;
using static OpenDentBusiness.LargeTableHelper;
using ADODB;

namespace OpenDentBusiness {
	public partial class ConvertDatabases {
		#region Helper Methods
		///<summary>Converts frequency limitation preferences into CodeGroups. Current frequency limitation benefits will be converted into these new code groups.</summary>
		private static void ConvertFrequencyLimitationPreferences() {
			//It is important to have ItemOrder mimic the hard coded values of the old 'benefit.GetFrequencyGroupNum()' method so that the sorting of benefits (comparer) doesn't change by default.
			List<FrequencyLimitationHelper> listFrequencyLimitationHelpers=new List<FrequencyLimitationHelper>();
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenBWCodes","BW",0,codeGroupFixed:1,isQuantityRequired:false,isCovCatAllowed:true));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenExamCodes","Exam",1,codeGroupFixed:3,isQuantityRequired:false,isCovCatAllowed:true));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenPanoCodes","Pano/FMX",2,codeGroupFixed:2,isQuantityRequired:false,isCovCatAllowed:true));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenCancerScreeningCodes","Cancer Screening",3));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenProphyCodes","Prophy",4,codeGroupFixed:5));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenFlourideCodes","Fluoride",5,codeGroupFixed:8));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenSealantCodes","Sealant",6,codeGroupFixed:9));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenCrownCodes","Crown",7));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenSRPCodes","SRP",8,codeGroupFixed:6));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenFullDebridementCodes","Full Debridement",9,codeGroupFixed:7));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenPerioMaintCodes","Perio Maint",10,codeGroupFixed:4));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenDenturesCodes","Dentures",11));
			listFrequencyLimitationHelpers.Add(new FrequencyLimitationHelper("InsBenImplantCodes","Implants",12));
			for(int i=0;i<listFrequencyLimitationHelpers.Count;i++) {
				listFrequencyLimitationHelpers[i].CreateCodeGroup();
				listFrequencyLimitationHelpers[i].ConvertBenefits();
			}
		}

		private class FrequencyLimitationHelper {
			///<summary>This value corresponds to CodeGroup.EnumCodeGroupFixed</summary>
			private int _codeGroupFixed;
			private long _codeGroupNum;
			private string _groupName;
			private bool _isCovCatAllowed;
			private bool _isQuantityRequired;
			private int _itemOrder;
			///<summary>The obsolete PrefName.</summary>
			private string _prefNameOld;
			private string _procCodes;

			#region Enum Values
			private const int _benefitTypeLimitations=5;
			private const int _coverageLevelNone=0;
			private const int _ebenefitCatRoutinePreventive=12;
			private const int _quantityQualifierNumberOfServices=1;
			private const int _quantityQualifierYears=4;
			private const int _quantityQualifierMonths=5;
			private const int _timePeriodNone=0;
			private const int _timePeriodServiceYear=1;
			private const int _timePeriodCalendarYear=2;
			private const int _timePeriodNumberInLast12Months=5;
			#endregion

			public FrequencyLimitationHelper(string prefNameOld,string groupName,int itemOrder,int codeGroupFixed=0,bool isQuantityRequired=true,bool isCovCatAllowed=false) {
				_codeGroupFixed=codeGroupFixed;
				_groupName=groupName;
				_isCovCatAllowed=isCovCatAllowed;
				_isQuantityRequired=isQuantityRequired;
				_itemOrder=itemOrder;
				_prefNameOld=prefNameOld;
			}

			internal void CreateCodeGroup() {
				//Get the ValueString from the preference table which will be used as the ProcCodes value on the codegroup table (both are comma separated).
				_procCodes=Db.GetScalar($"SELECT ValueString FROM preference WHERE PrefName='{_prefNameOld}'");
				//If any prefs are empty, fill them with codes. This covers the "default fallback" hard coded codes that are seen in many places.
				if(string.IsNullOrWhiteSpace(_procCodes) && !CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
					_procCodes=GetDefaultProcCodesForPrefName(_prefNameOld);
				}
				//Insert a new code group into the database and keep track of the InsertID (PK) that the database returns.
				string command=$"INSERT INTO codegroup (GroupName,ProcCodes,ItemOrder,CodeGroupFixed,IsHidden) VALUES ('{_groupName}','{POut.String(_procCodes)}',{_itemOrder},{_codeGroupFixed},0)";
				_codeGroupNum=Db.NonQ(command,true);
			}

			internal void ConvertBenefits() {
				//The following code mimics the exact behavior of ProcedureCodes.GetCodeNumsForPref() when this convert script section was written.
				//This is done so that there is absolutely no change in behavior in the program after converting the preferences into the new codegroup table.
				//E.g. ProcedureCodes.GetCodeNumsForPref() was case-sensitive so the following query will utilize BINARY in order to preserve that behavior.
				//Extract the ProcCode values from the comma separated list in the old preference ValueString column. Trim any white space to mimic ProcedureCodes.GetCodeNumsForPref().
				List<string> listProcCodes=_procCodes.Split(",",StringSplitOptions.RemoveEmptyEntries).Select(x => POut.String(x).Trim()).ToList();
				if(listProcCodes.IsNullOrEmpty()) {
					return;//Nothing to convert.
				}
				//Find all of the CodeNums that exactly match the ProcCodes that were just extracted (case-sensitive).
				List<long> listCodeNums=Db.GetListLong($"SELECT CodeNum FROM procedurecode WHERE BINARY ProcCode IN ('{string.Join("','",listProcCodes)}')");
				//Grab all benefits that do not have a CodeGroupNum set and match all of the criteria for the current FrequencyLimitation being converted.
				//The following code mimics all of the old "Benefits.Is...Frequency()" methods that take in a benefit and check to see if the benefit passed in is an X frequency or not (e.g. IsBitewingFrequency).
				string command=@$"SELECT BenefitNum
					FROM benefit
					WHERE BenefitType = {_benefitTypeLimitations}
					AND MonetaryAmt = -1
					AND Percent = -1
					AND ((QuantityQualifier IN ({_quantityQualifierMonths},{_quantityQualifierYears}) AND TimePeriod={_timePeriodNone})
						OR (QuantityQualifier = {_quantityQualifierNumberOfServices} AND TimePeriod IN ({_timePeriodServiceYear},{_timePeriodCalendarYear},{_timePeriodNumberInLast12Months})))
					AND CoverageLevel = {_coverageLevelNone}";
				//Exam frequency limitation benefits not required to utilize code nums and can also use the RoutinePreventive CovCat.
				//The 'Simplified View' within the Edit Benefits window used to automatically convert the exam frequency limitation benefit over to using RoutinePreventive CovCat.
				//This was simply how the program has always stored this benefit (e.g. as far back as v15.3 when frequency limitations didn't impact insurance estimates).
				//That behavior continued even when frequency limitations started impacting estimates (around v16.4).
				//However, the RoutinePreventive CovCat was purely used as an identifier and was not utilized for the code span associated with it.
				//This convert script uses the RoutinePreventive CovCat as an identifier for exam frequency limitation benefits but ignores the procedure codes associated with the CovCat.
				if(_prefNameOld=="InsBenExamCodes") {
					//Check to see if the RoutinePreventive covcat exists; this logic mimics CovCats.GetForEbenCat()
					DataTable table=Db.GetTable($"SELECT CovCatNum FROM covcat WHERE EbenefitCat={_ebenefitCatRoutinePreventive} AND IsHidden=0 ORDER BY CovOrder LIMIT 1");
					//If there is no RoutinePreventive covcat AND there are no valid CodeNums found from the preference then there is nothing to convert.
					if(table.Rows.Count==0 && listCodeNums.IsNullOrEmpty()) {
						return;//Nothing to convert.
					}
					//Grab benefits that are associated with the RoutinePreventive CovCat OR any of the CodeNums explicitly specified within the preference.
					command+=" AND (";
					if(table.Rows.Count > 0) {
						command+=$"(CodeNum=0 AND CovCatNum={table.Rows[0]["CovCatNum"].ToString()})";
					}
					if(!listCodeNums.IsNullOrEmpty()) {
						if(table.Rows.Count > 0) {
							command+=" OR ";//Allow matches based off of the RoutinePreventive covcat OR the CodeNums specified within the preference.
						}
						command+=$"(CodeNum IN ({string.Join(",",listCodeNums)}))";
					}
					command+=")";
				}
				else {//InsBenExamCodes is the only preference allowed to ignore ProcCodes and utilize CovCats instead. All others are required to match explicit CodeNums.
					if(listCodeNums.IsNullOrEmpty()) {
						return;//Nothing to convert.
					}
					//However, there could be old benefits with both a CovCat and a CodeNum set and only InsBenBWCodes and InsBenPanoCodes had logic that would allow such a scenario.
					if(!_isCovCatAllowed) {
						command+=" AND CovCatNum=0";
					}
					command+=$" AND CodeNum IN ({string.Join(",",listCodeNums)})";
				}
				if(_isQuantityRequired) {
					command+=" AND Quantity!=0";
				}
				List<long> listBenefitNums=Db.GetListLong(command);
				if(listBenefitNums.IsNullOrEmpty()) {
					return;//Nothing to convert.
				}
				//Convert all of the benefits that were just found over to using code groups.
				//Do this by clearing out CodeNum and CovCatNum; then set CodeGroupNum accordingly.
				Db.NonQ($"UPDATE benefit SET CodeNum=0,CovCatNum=0,CodeGroupNum={_codeGroupNum} WHERE BenefitNum IN ({string.Join(",",listBenefitNums)})");
			}

			private string GetDefaultProcCodesForPrefName(string prefName) {
				switch(prefName) {
					//Default codes below this point were found in To16_4_19()
					case "InsBenBWCodes":
						return "D0272,D0274";
					case "InsBenExamCodes":
						return "D0120,D0150";
					case "InsBenPanoCodes":
						return "D0210,D0330";
					//Default codes below this point were found in To18_2_1()
					case "InsBenCancerScreeningCodes":
						return "D0431";
					case "InsBenProphyCodes":
						return "D1110,D1120";
					case "InsBenFlourideCodes":
						return "D1206,D1208";
					case "InsBenSealantCodes":
						return "D1351";
					case "InsBenCrownCodes":
						return "D2740,D2750,D2751,D2752,D2780,D2781,D2782,D2783,D2790,D2791,D2792,D2794";
					case "InsBenSRPCodes":
						return "D4341,D4342";
					case "InsBenFullDebridementCodes":
						return "D4355";
					case "InsBenPerioMaintCodes":
						return "D4910";
					case "InsBenDenturesCodes":
						return "D5110,D5120,D5130,D5140,D5211,D5212,D5213,D5214,D5221,D5222,D5223,D5224,D5225,D5226";
					case "InsBenImplantCodes":
						return "D6010";
					default:
						return "";
				}
			}
		}

		internal static void FixADA2024IsPreAuthLocation() {
			//Change the alignment of the Preauthorization checkbox on the 2024 claim form.
			string command="SELECT ClaimFormNum FROM claimform WHERE Description='ADA 2024'";
			long claimFormNum2024 = Db.GetLong(command);
			if(claimFormNum2024>0) {
				//Only change if the XPos, YPos, and Width are the default values that we used when we created the claim form.
				command="SELECT ClaimFormItemNum " +
					"FROM claimformitem " +
					$"WHERE ClaimFormNum={POut.Long(claimFormNum2024)} AND FieldName='IsPreAuth' " +
					$"AND XPos=184 AND (YPos=74 OR YPos=72) AND Width=0";//Default values
				long claimFormItemNum = Db.GetLong(command);
				if(claimFormItemNum>0) {
					command=$"UPDATE claimformitem SET XPos=224, YPos=56 WHERE ClaimFormItemNum={POut.Long(claimFormItemNum)}";
					Db.NonQ(command);
				}
			}
		}

		///<summary>This helper method CANNOT be edited for backwards compatibility reasons.</summary>
		internal static void AddAlertTypesToOdAllTypesCategory(int alertTypeNum) {
			string command="SELECT AlertCategoryNum FROM alertcategory WHERE IsHQCategory=1 AND InternalName='OdAllTypes'";
			long alertCategoryNum=PIn.Long(Db.GetScalar(command));
			command="SELECT AlertCategoryLinkNum FROM alertcategorylink WHERE AlertCategoryNum="+alertCategoryNum
				+" AND AlertType="+alertTypeNum;
			long alertCategoryLinkNum=PIn.Long(Db.GetScalar(command));
			if(alertCategoryLinkNum==0) {//No table entry was found so create one
				command="INSERT INTO alertcategorylink(AlertCategoryNum,AlertType) VALUES("+alertCategoryNum+","+alertTypeNum+")";
				Db.NonQ(command);
			}
		}

		///<summary>Safe to run many times</summary>
		private static void ObsolesceCDTCodesFor2025() {
			#region I56520
			if(CultureInfo.CurrentCulture.Name.EndsWith("US")) {//United States
				//Move deprecated codes to the Obsolete procedure code category.
				//Make sure the procedure code category exists before moving the procedure codes.
				string procCatDescript="Obsolete";
				long defNum=0;
				string command="SELECT DefNum FROM definition WHERE Category=11 AND ItemName='"+POut.String(procCatDescript)+"'";//11 is DefCat.ProcCodeCats
				DataTable dtDef=Db.GetTable(command);
				if(dtDef.Rows.Count==0) { //The procedure code category does not exist, add it
					command="SELECT COUNT(*) FROM definition WHERE Category=11";//11 is DefCat.ProcCodeCats
					int countCats=PIn.Int(Db.GetCount(command));
					command="INSERT INTO definition (Category,ItemName,ItemOrder) "
							+"VALUES (11"+",'"+POut.String(procCatDescript)+"',"+POut.Int(countCats)+")";//11 is DefCat.ProcCodeCats
					defNum=Db.NonQ(command,true);
				}
				else { //The procedure code category already exists, get the existing defnum
					defNum=PIn.Long(dtDef.Rows[0]["DefNum"].ToString());
				}
				string[] cdtCodesDeleted=new string[] {
					"D2941",
					"D6095"
				};
				//Change the procedure codes' category to Obsolete.
				command="UPDATE procedurecode SET ProcCat="+POut.Long(defNum)
				+" WHERE ProcCode IN('"+string.Join("','",cdtCodesDeleted.Select(x => POut.String(x)))+"') ";
				Db.NonQ(command);
			}//end United States CDT codes update
			#endregion I56520
		}
		#endregion

		private static void To23_2_1() {
			string strUpgrading="Upgrading database to version: 23.2.1";
			string command;
			DataTable table;
			//-----------------------------------------------------------------------------------------------------
			//41732 - Insert PracticeBooster bridge----------------------------------------------------------------
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note" 
				 +") VALUES(" 
				 +"'PracticeBooster', " 
				 +"'PracticeBooster from practicebooster.com', " 
				 +"'0', " 
				 +"'"+POut.String(@"https://practicebooster.com/login.asp")+"', "
				 +"'', "//leave blank if none 
				 +"'')"; 
			long programNum=Db.NonQ(command,true); 
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) " 
				 +"VALUES (" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'7', "//ToolBarsAvail.MainToolbar
				 +"'PracticeBooster')"; 
			Db.NonQ(command);
			//end PracticeBooster bridge
			//B44132 Adding missing alert links for the 'All' option
			List<int> listAlertTypes=new List<int> {17,18,19,23,25,31,33,34,35,36,37,38};
			for (int i = 0; i < listAlertTypes.Count; i++) {
				command=$"SELECT AlertCategoryLinkNum FROM alertcategorylink WHERE AlertCategoryNum=1 AND AlertType={listAlertTypes[i]}";
				long alertCategoryLinkNum=PIn.Long(Db.GetScalar(command));
				if (alertCategoryLinkNum < 1 ) { 
					command=$"INSERT INTO alertcategorylink(AlertCategoryNum,AlertType) VALUES(1, {listAlertTypes[i]});"; 
					Db.NonQ(command);
				}
			}//end B44132
			//F44562 adding permissions for audit trail viewing
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			long groupNum;
			for(int i=0;i<table.Rows.Count;i++) {
				groupNum=PIn.Long(table.Rows[i]["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) VALUES("+POut.Long(groupNum)+",247)";//Permission is ViewAppointmentAuditTrail
				Db.NonQ(command);
			}//End F44562
			command="INSERT INTO preference(PrefName,ValueString) VALUES('RedirectShortURLsFromHQ','')";
			Db.NonQ(command);
			//Insert JazzClassicCapture bridge-----------------------------------------------------------------
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note" 
				 +") VALUES(" 
				 +"'JazzClassicCapture', " 
				 +"'Jazz Classic Capture', " 
				 +"'0', " 
				 +"'"+POut.String(@"C:\Program Files\Jazz Imaging LLC\Jazz Classic\Classic.exe")+"', " 
				 +"'"+POut.String(@"/C")+"', " 
				 +"'')"; 
			programNum=Db.NonQ(command,true); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'XML output file path', " 
				 +"'C:\\\\ProgramData\\\\Jazz Imaging LLC\\\\Classic\\\\OpenDentalPatientInfo.xml')"; 
			Db.NonQ(command); 
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) " 
				 +"VALUES (" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'2', "//ToolBarsAvail.ChartModule 
				 +"'Capture')"; 
			Db.NonQ(command); 
			//end JazzClassicCapture bridge 
			//Insert JazzClassicExamView bridge----------------------------------------------------------------------
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note" 
				 +") VALUES(" 
				 +"'JazzClassicExamView', " 
				 +"'Jazz Classic Exam View', " 
				 +"'0', " 
				 +"'"+POut.String(@"C:\Program Files\Jazz Imaging LLC\Jazz Classic\Classic.exe")+"', " 
				 +"'"+POut.String(@"/E")+"', " 
				 +"'')"; 
			programNum=Db.NonQ(command,true); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'XML output file path', " 
				 +"'C:\\\\ProgramData\\\\Jazz Imaging LLC\\\\Classic\\\\OpenDentalPatientInfo.xml')"; 
			Db.NonQ(command); 
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) " 
				 +"VALUES (" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'2', "//ToolBarsAvail.ChartModule 
				 +"'View Exam')"; 
			Db.NonQ(command); 
			//end JazzClassicExamView bridge 
			//Insert JazzClassicPatientUpdate bridge-----------------------------------------------------------------
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note" 
				 +") VALUES(" 
				 +"'JazzClassicPatientUpdate', " 
				 +"'Jazz Classic Patient Update', " 
				 +"'0', " 
				 +"'"+POut.String(@"C:\Program Files\Jazz Imaging LLC\Jazz Classic\Classic.exe")+"', " 
				 +"'"+POut.String(@"/P")+"', " 
				 +"'')"; 
			programNum=Db.NonQ(command,true); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'XML output file path', " 
				 +"'C:\\\\ProgramData\\\\Jazz Imaging LLC\\\\Classic\\\\OpenDentalPatientInfo.xml')"; 
			Db.NonQ(command); 
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) " 
				 +"VALUES (" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'7', "//ToolBarsAvail.MainToolbar 
				 +"'Classic Patient Update')"; 
			Db.NonQ(command); 
			//end JazzClassicPatientUpdate bridge 
			//Set the default image for all three Jazz Classic bridges.
			command="UPDATE program "
				+"SET ButtonImage='iVBORw0KGgoAAAANSUhEUgAAABYAAAAWCAIAAABL1vtsAAABgmlDQ1BJQ0MgUHJvZmlsZQAAKM+VkU0oRFEcxX8" +
				"ziDSyMEmS3gIrSkiWGiJFaWbUDBbee2OGmvdmem9kY6lslYWPja+FjTVbC1ullI+SpZUVsZGe/32jZlKj3LrdX+" +
				"fec7r3XAgeZE3Lre4Fyy440fGIlkjOarXPhGikmXa6ddPNT8XG4lQcH7cE1HrTo7L432hILbomBDThYTPvFIQXh" +
				"AdXC3nFO8Jhc0lPCZ8KdztyQeF7pRtFflGc8TmoMsNOPDoiHBbWMmVslLG55FjCA8IdKcuW/GCiyCnFa4qt7Ir5" +
				"c0/1wtCiPRNTusw2xplgimk0DFZYJkuBHlltUVyish+p4G/1/dPiMsS1jCmOUXJY6L4f9Qe/u3XT/X3FpFAEap4" +
				"8760Tarfga9PzPg897+sIqh7hwi75cwcw9C76Zknr2IfGdTi7LGnGNpxvQMtDXnd0X6qSGUyn4fVEvikJTddQP1" +
				"fs7Wef4zuIS1eTV7C7B10ZyZ6v8O668t7+POP3R+QbwptyxxwNMQ4AAAAJcEhZcwAACu8AAArvAX12ikgAAAAVd" +
				"EVYdFNvZnR3YXJlAEdJTVAgMi4xMC4zMoNlmMwAAAAHdElNRQfmDA8OIALgTsfsAAAEYElEQVQ4TyVU728URRje" +
				"P88Y/eQHYrF80UhC/GAEiWI0aGMAbSRYCESxggQjtkEQQylahQZoLaW99q73Y3fv9vfvmZ2dmd29u7ayPtdLJnP" +
				"v7Mz7vM/zvDOnRFyWeT9NRBmndLcyRbUnoqgohNhn3IkErRJCWBXnSTLgjPSFTBnjglWiICSuWCYU0ecxFYz/t1" +
				"dEKfdtrhd9kpJq4O/L/cArish7Odyt0oxWaUDkkOeRzIdREgMiz/exVAgNfvh54cTJq1sbzvffLIoi5cVA7liDK" +
				"6ddKggxL12e/+Kz8144nLky/9GZ2UbHmPn2IRPDufnHJz48t/Z8U1l6+OLMzPWW5m5rzaNvXQQ5ybE/G51612ps" +
				"qLX2oYmp+pbuGPSV1z5eedLyqZyYmF5YWjz1yeWuQSzHVG7+tHRnYYWluZvSI5M3gqTM7Z578o3B/Wvi0tRevPv" +
				"74vrhty88X1Nrz54ce//2/aWVycmZuTv3Zn/8uygr2/GUjhYcPfbp2akbDxZ3Xn394tnz17ce3Btcncqe/mGfPt" +
				"z689/pr64dee/M0/Xm59PXJt/5bunR1puHznVUcvyDC8ePn19e3lBgSRDzHbWXZa7meE1DfUnMynJKERRuOaS85" +
				"4eGTguatDx/4BCRMzRMyEEYB47DaBYojA1FOQyk3OVxIUueVQOa6IMqKPciWezysMdKKVIpizyjEaNc0CShhawy" +
				"GaboUpYoPcM1LT9leUK440aen2DEScZFnjJOKEPnZV7SNHO9AMrDUGiaZ1mR51HD9DTdUro9p6Ma2/X2Zm0Hn2w" +
				"n3NpuYYnAsoMxKGZsqZrZandNM9R1F3OnYzWg33AVnMCP68U4VG90mi1d79qIu93AMCLfz4KAI8DSNGNd9zc2Gw" +
				"BCFrgDy3XJCAI5YE5TiToghRiIKKKqNk5gIO52PQSGEehdyzDdMKKghsP4ruw01Y7aMy0vTiAbjmRBSFwv6hlOt" +
				"2d7fowYaZbtR3HquCHOpEzSVEApamuao6iagaPYw1cMQAiJt9TPON4TFxL9K+AllnnRh8eE8owXmEcUeo4fEIWL" +
				"0nZG9FANXDDLfAAg5KAXyAEEZsRAxIycMEpBAa5BERgpgERNLDTdxNxqw04L/IELOASQPZYwLoD2oZHwEiYiMS+" +
				"GCoAxUBMq4CK24WKv58OtRgNXdj9JCrQZflEi6tuqqvqOzyIiI8LzYjfjJVjgZeYHyiUKgiF6DAjwXF2t1etGra" +
				"a+2KjDXaATkodx5PoeLh3+FphIRhd8DHGgdmQ17ijU4lKBPBrWbtu1WhvkoaLdNoNAaipptwLbYvV6t9k0UUyBS" +
				"WOUAzlwrjygk6JT7U4PaaBz4LRP2GBtXZu98ejLr29u7tBfflt+8M9208iUccPGEBhwaIzCZAmdeJEsHe16IbV9" +
				"cev24/W5v4KV5tqvC62F5dVbd8MXjZGdYyEAGqOMl4AAFnhlBx+Z6D961pi7u0LbdWf9mdDqGN7Wau50/gcl8+v" +
				"71X00NgAAAABJRU5ErkJggg==' "
				+"WHERE ProgName IN ('JazzClassicCapture','JazzClassicExamView','JazzClassicPatientUpdate')";
			Db.NonQ(command);
			//Insert Shining3D bridge----------------------------------------------------------------- 
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note" 
				 +") VALUES(" 
				 +"'Shining3D', " 
				 +"'Shining3D from https://www.shining3d.com', " 
				 +"'0', " 
				 +"'"+POut.String(@"D:\DentalLauncher\IntraoralScan\Bin\DentalLauncher.exe")+"', " 
				 +"'"+POut.String(@"--source ""OpenDental"" --pName ""[NameFL]"" --pAge ""[BirthDate_yyyyMMdd]"" --pGender ""[Gender]""")+"', "
				 +"'')"; 
			programNum=Db.NonQ(command,true); 
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) " 
				 +"VALUES (" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'7', "//ToolBarsAvail.MainToolbar 
				 +"'IntraoralScan')"; 
			Db.NonQ(command);
			//end Shining3D bridge 
			//Frequency Limitations--------------------------------------------------------------------------------
			command="DROP TABLE IF EXISTS codegroup";
			Db.NonQ(command);
			command=@"CREATE TABLE codegroup (
				CodeGroupNum bigint NOT NULL auto_increment PRIMARY KEY,
				GroupName varchar(50) NOT NULL,
				ProcCodes text NOT NULL,
				ItemOrder int NOT NULL,
				CodeGroupFixed tinyint NOT NULL,
				IsHidden tinyint NOT NULL
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,strUpgrading+" - Adding benefit.CodeGroupNum");
			//Add a new column to the benefit table named "CodeGroupNum" as an FK to codegroup.CodeGroupNum.
			command="ALTER TABLE benefit ADD CodeGroupNum bigint NOT NULL, ADD INDEX (CodeGroupNum)";
			Db.NonQ(command);
			ODEvent.Fire(ODEventType.ConvertDatabases,strUpgrading+" - Frequency Limitations");
			ConvertFrequencyLimitationPreferences();
			ODEvent.Fire(ODEventType.ConvertDatabases,strUpgrading);
			//I45000 SkySQL - Read Only Server
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReadOnlyServerSslCa','')";
			Db.NonQ(command);
			//end I45000
			//I44628 - Permission to select archived patients
			//Add permission to everyone------------------------------------------------------
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				groupNum=PIn.Long(table.Rows[i]["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
					+"VALUES("+POut.Long(groupNum)+",249)"; //249 - ArchivedPatientSelect
				Db.NonQ(command);
			}
			//End I44628
		}//End of 23_2_1() method

		private static void To23_2_3() {
			string command=@"SELECT PayPlanNum FROM payplan WHERE PlanNum!=0";//PlanNum set for Insurance Plans only per schema
			List<long> listPayPlanNums=Db.GetListLong(command);
			if(listPayPlanNums.Count>0) {
				//Set the guarantors in the listPayPlanNums to 0
				command="UPDATE payplan SET Guarantor=0 WHERE PayPlanNum IN("+String.Join(",",listPayPlanNums)+")";
				Db.NonQ(command);
			}
		}//End of 23_2_1() method

		private static void To23_2_5(){
			string command="DROP TABLE IF EXISTS orthochartlog";
			Db.NonQ(command);
			command=@"CREATE TABLE orthochartlog (
				OrthoChartLogNum bigint NOT NULL auto_increment PRIMARY KEY,
				PatNum bigint NOT NULL,
				ComputerName varchar(255) NOT NULL,
				DateTimeLog datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				DateTimeService datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				UserNum bigint NOT NULL,
				ProvNum bigint NOT NULL,
				OrthoChartRowNum bigint NOT NULL,
				LogData MEDIUMTEXT NOT NULL
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('OrthoChartLoggingOn','0')";
			Db.NonQ(command);
		}//End of 23_2_5() method

		private static void To23_2_9(){
			//E46260
			string command="INSERT INTO preference(PrefName,ValueString) VALUES('EraChkPaymentType','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EraAchPaymentType','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EraFwtPaymentType','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EraDefaultPaymentType','0')";
			Db.NonQ(command);
			//End of E46260
		}//End of To23_2_9() method

		private static void To23_2_19(){
			//I44056 adding a new table called payplantemplate
			string command="DROP TABLE IF EXISTS payplantemplate";
			Db.NonQ(command);
			command=@"CREATE TABLE payplantemplate (
				PayPlanTemplateNum bigint NOT NULL auto_increment PRIMARY KEY,
				PayPlanTemplateName varchar(255) NOT NULL,
				ClinicNum bigint NOT NULL,
				APR double NOT NULL,
				InterestDelay int NOT NULL,
				PayAmt double NOT NULL,
				NumberOfPayments int NOT NULL,
				ChargeFrequency tinyint NOT NULL,
				DownPayment double NOT NULL,
				DynamicPayPlanTPOption tinyint NOT NULL,
				Note varchar(255) NOT NULL,
				IsHidden tinyint NOT NULL,
				INDEX(ClinicNum)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//End I44056	
		}//End of To23_2_19() method

		private static void To23_2_21(){
			string command="SELECT ProgramNum FROM program WHERE ProgName='Scanora'";
			long programNum=Db.GetLong(command);
			command="UPDATE programproperty "+
				"SET PropertyValue='"+POut.String(@"C:\ProgramData\Scanora\Scanora.ini")+"' "+
				"WHERE ProgramNum="+POut.Long(programNum)+" "+
				"AND PropertyDesc='"+POut.String("Import.ini path")+"' "+
				"AND PropertyValue='"+POut.String(@"C:\Scanora\Scanora.ini")+"'";
			Db.NonQ(command);
		}//End of To23_2_21 method

		private static void To23_2_22(){
			string command="DELETE FROM clinicpref WHERE prefname='EClipboardClinicalValidationFrequency'";//This has been made into a global (non-clinic) pref, per Nathan/Sean/Sam.
			Db.NonQ(command);
			//I48122 ODXam - streamline App Store approval
			command="INSERT INTO preference(PrefName,ValueString) VALUES('DevAutoApproveDevice','0')";//False by default
			Db.NonQ(command);
			//End I48122
		}//End of To23_2_22 method

		private static void To23_2_23() {
			string command;
			if(!LargeTableHelper.IndexExists("taskancestor","TaskListNum,TaskNum")) {
				command="ALTER TABLE taskancestor ADD INDEX TaskListCov (TaskListNum,TaskNum)";
				List<string> listIndexNames=LargeTableHelper.GetIndexNames("taskancestor","TaskListNum");
				if(listIndexNames.Count>0) {
					command+=","+string.Join(",",listIndexNames.Select(x => $"DROP INDEX {POut.String(x)}"));
				}
				Db.NonQ(command);
			}
		}//End of To23_2_23 method

		private static void To23_2_24() {
			string command=@"UPDATE programproperty SET PropertyValue='C:\\ProgramData\\Jazz Imaging LLC\\Classic\\OpenDentalPatientInfo.xml' WHERE ProgramNum IN(SELECT programnum FROM program WHERE ProgName LIKE 'JazzClassic%')";
			Db.NonQ(command);
		}//End of To23_2_4 method

		private static void To23_2_26() {
			string command="INSERT INTO claimform(Description,IsHidden,FontName,FontSize,UniqueID,PrintImages,OffsetX,OffsetY,Width,Height) VALUES ('ADA 2024',0,'Arial',9,'OD11',1,0,0,850,1100)";
			long claimFormNum2024=Db.NonQ(command,true);
			if(claimFormNum2024>0) {
				command=$@"INSERT  INTO claimformitem(ClaimFormNum,ImageFileName,FieldName,FormatString,XPos,YPos,Width,Height) VALUES 
					({POut.Long(claimFormNum2024)},'ADA2024_J430.gif','','',0,2,850,1100),
					({POut.Long(claimFormNum2024)},'','DateLastSRP','',738,759,90,14),
					({POut.Long(claimFormNum2024)},'','PatientIsGenderUnknown','',615,419,0,0),
					({POut.Long(claimFormNum2024)},'','PatientIsFemale','',595,419,0,0),
					({POut.Long(claimFormNum2024)},'','PatientIsMale','',575,419,0,0),
					({POut.Long(claimFormNum2024)},'','BillingDentistPh123','',82,1047,30,14),
					({POut.Long(claimFormNum2024)},'','TreatingDentistSigDate','MM/dd/yyyy',707,926,90,14),
					({POut.Long(claimFormNum2024)},'','OtherInsSubscrIsGenderUnknown','',216,310,0,0),
					({POut.Long(claimFormNum2024)},'','TreatingDentistProviderID','',684,1046,145,14),
					({POut.Long(claimFormNum2024)},'','Miss19','',295,685,0,0),
					({POut.Long(claimFormNum2024)},'','BillingDentistPh456','',124,1047,30,14),
					({POut.Long(claimFormNum2024)},'','BillingDentistPh78910','',162,1047,50,14),
					({POut.Long(claimFormNum2024)},'','PayToDentistST','',231,985,40,14),
					({POut.Long(claimFormNum2024)},'','TreatingDentistSignature','',429,926,245,14),
					({POut.Long(claimFormNum2024)},'','PayToDentistCity','',34,985,185,14),
					({POut.Long(claimFormNum2024)},'','PayToDentistZip','',284,985,100,14),
					({POut.Long(claimFormNum2024)},'','P1Surface','',321,484,58,14),
					({POut.Long(claimFormNum2024)},'','PatientAddress2','',445,369,350,14),
					({POut.Long(claimFormNum2024)},'','Miss1','',35,668,0,0),
					({POut.Long(claimFormNum2024)},'','Miss8','',175,668,0,0),
					({POut.Long(claimFormNum2024)},'','Miss2','',55,668,0,0),
					({POut.Long(claimFormNum2024)},'','P10Description','',521,635,239,14),
					({POut.Long(claimFormNum2024)},'','Miss3','',74,668,0,0),
					({POut.Long(claimFormNum2024)},'','P10Fee','',829,635,68,14),
					({POut.Long(claimFormNum2024)},'','Miss10','',215,668,0,0),
					({POut.Long(claimFormNum2024)},'','Miss4','',95,668,0,0),
					({POut.Long(claimFormNum2024)},'','P10Code','',381,635,58,14),
					({POut.Long(claimFormNum2024)},'','P10System','',172,635,27,14),
					({POut.Long(claimFormNum2024)},'','P10Date','MM/dd/yyyy',35,635,90,14),
					({POut.Long(claimFormNum2024)},'','P9Fee','',829,618,68,14),
					({POut.Long(claimFormNum2024)},'','P10ToothNumber','',202,635,117,14),
					({POut.Long(claimFormNum2024)},'','P10Surface','',321,635,58,14),
					({POut.Long(claimFormNum2024)},'','P9Surface','',321,618,58,14),
					({POut.Long(claimFormNum2024)},'','P10Area','',141,635,29,14),
					({POut.Long(claimFormNum2024)},'','P9Description','',521,618,239,14),
					({POut.Long(claimFormNum2024)},'','P9Area','',141,618,29,14),
					({POut.Long(claimFormNum2024)},'','P8Date','MM/dd/yyyy',35,602,90,14),
					({POut.Long(claimFormNum2024)},'','P9Code','',381,618,58,14),
					({POut.Long(claimFormNum2024)},'','P8Surface','',321,602,58,14),
					({POut.Long(claimFormNum2024)},'','P9ToothNumber','',202,618,117,14),
					({POut.Long(claimFormNum2024)},'','P8Description','',521,602,239,14),
					({POut.Long(claimFormNum2024)},'','P9System','',172,618,27,14),
					({POut.Long(claimFormNum2024)},'','P8Fee','',829,602,68,14),
					({POut.Long(claimFormNum2024)},'','P8Code','',381,602,58,14),
					({POut.Long(claimFormNum2024)},'','P9Date','MM/dd/yyyy',35,618,90,14),
					({POut.Long(claimFormNum2024)},'','P8Area','',141,602,29,14),
					({POut.Long(claimFormNum2024)},'','P7Code','',381,585,58,14),
					({POut.Long(claimFormNum2024)},'','P7Surface','',321,585,58,14),
					({POut.Long(claimFormNum2024)},'','P8ToothNumber','',202,602,117,14),
					({POut.Long(claimFormNum2024)},'','P7Fee','',829,585,68,14),
					({POut.Long(claimFormNum2024)},'','P7ToothNumber','',202,585,117,14),
					({POut.Long(claimFormNum2024)},'','P6Fee','',829,568,68,14),
					({POut.Long(claimFormNum2024)},'','P8System','',172,602,27,14),
					({POut.Long(claimFormNum2024)},'','P7Description','',521,585,239,14),
					({POut.Long(claimFormNum2024)},'','P7System','',172,585,27,14),
					({POut.Long(claimFormNum2024)},'','P6Code','',381,568,58,14),
					({POut.Long(claimFormNum2024)},'','P7Date','MM/dd/yyyy',35,585,90,14),
					({POut.Long(claimFormNum2024)},'','P4Surface','',321,534,58,14),
					({POut.Long(claimFormNum2024)},'','P6Date','MM/dd/yyyy',35,568,90,14),
					({POut.Long(claimFormNum2024)},'','P6Surface','',321,568,58,14),
					({POut.Long(claimFormNum2024)},'','P6System','',172,568,27,14),
					({POut.Long(claimFormNum2024)},'','P6ToothNumber','',202,568,117,14),
					({POut.Long(claimFormNum2024)},'','P6Area','',141,568,29,14),
					({POut.Long(claimFormNum2024)},'','P6Description','',521,568,239,14),
					({POut.Long(claimFormNum2024)},'','P7Area','',141,585,29,14),
					({POut.Long(claimFormNum2024)},'','P5Description','',521,551,239,14),
					({POut.Long(claimFormNum2024)},'','P4Fee','',829,534,68,14),
					({POut.Long(claimFormNum2024)},'','P4Description','',521,534,239,14),
					({POut.Long(claimFormNum2024)},'','P5Surface','',321,551,58,14),
					({POut.Long(claimFormNum2024)},'','P5Fee','',829,551,68,14),
					({POut.Long(claimFormNum2024)},'','P5ToothNumber','',202,551,117,14),
					({POut.Long(claimFormNum2024)},'','P5Code','',381,551,58,14),
					({POut.Long(claimFormNum2024)},'','P5Date','MM/dd/yyyy',35,551,90,14),
					({POut.Long(claimFormNum2024)},'','P4Date','MM/dd/yyyy',35,534,90,14),
					({POut.Long(claimFormNum2024)},'','P5System','',172,551,27,14),
					({POut.Long(claimFormNum2024)},'','P4System','',172,534,27,14),
					({POut.Long(claimFormNum2024)},'','P4Area','',141,534,29,14),
					({POut.Long(claimFormNum2024)},'','P4Code','',381,534,58,14),
					({POut.Long(claimFormNum2024)},'','P4ToothNumber','',202,534,117,14),
					({POut.Long(claimFormNum2024)},'','P5Area','',141,551,29,14),
					({POut.Long(claimFormNum2024)},'','P3Fee','',829,517,68,14),
					({POut.Long(claimFormNum2024)},'','P3Date','MM/dd/yyyy',35,517,90,14),
					({POut.Long(claimFormNum2024)},'','P3Code','',381,517,58,14),
					({POut.Long(claimFormNum2024)},'','P3Area','',141,517,29,14),
					({POut.Long(claimFormNum2024)},'','P3Surface','',321,517,58,14),
					({POut.Long(claimFormNum2024)},'','P3ToothNumber','',202,517,117,14),
					({POut.Long(claimFormNum2024)},'','P2ToothNumber','',202,500,117,14),
					({POut.Long(claimFormNum2024)},'','P3Description','',521,517,239,14),
					({POut.Long(claimFormNum2024)},'','P3System','',172,517,27,14),
					({POut.Long(claimFormNum2024)},'','P2Description','',521,500,239,14),
					({POut.Long(claimFormNum2024)},'','P1ToothNumber','',202,484,117,14),
					({POut.Long(claimFormNum2024)},'','P1Fee','',829,484,68,14),
					({POut.Long(claimFormNum2024)},'','P2Area','',141,500,29,14),
					({POut.Long(claimFormNum2024)},'','PatientCity','',445,385,185,14),
					({POut.Long(claimFormNum2024)},'','P2Code','',381,500,58,14),
					({POut.Long(claimFormNum2024)},'','P1Description','',521,484,239,14),
					({POut.Long(claimFormNum2024)},'','P1Code','',381,484,58,14),
					({POut.Long(claimFormNum2024)},'','P1Area','',141,484,29,14),
					({POut.Long(claimFormNum2024)},'','P2Date','MM/dd/yyyy',35,500,90,14),
					({POut.Long(claimFormNum2024)},'','PatientST','',642,385,40,14),
					({POut.Long(claimFormNum2024)},'','P1System','',172,484,27,14),
					({POut.Long(claimFormNum2024)},'','P2Fee','',829,500,68,14),
					({POut.Long(claimFormNum2024)},'','P2System','',172,500,27,14),
					({POut.Long(claimFormNum2024)},'','PatientDOB','MM/dd/yyyy',445,418,90,14),
					({POut.Long(claimFormNum2024)},'','P2Surface','',321,500,58,14),
					({POut.Long(claimFormNum2024)},'','PatientZip','',695,385,100,14),
					({POut.Long(claimFormNum2024)},'','PatientAddress','',445,353,350,14),
					({POut.Long(claimFormNum2024)},'','PatientLastFirst','',445,337,350,14),
					({POut.Long(claimFormNum2024)},'','EmployerName','',579,251,250,14),
					({POut.Long(claimFormNum2024)},'','RelatIsSelf','',445,303,0,0),
					({POut.Long(claimFormNum2024)},'','RelatIsChild','',555,303,0,0),
					({POut.Long(claimFormNum2024)},'','GroupNum','',445,251,114,14),
					({POut.Long(claimFormNum2024)},'','RelatIsSpouse','',495,303,0,0),
					({POut.Long(claimFormNum2024)},'','RelatIsOther','',645,303,0,0),
					({POut.Long(claimFormNum2024)},'','SubscrDOB','MM/dd/yyyy',445,218,90,14),
					({POut.Long(claimFormNum2024)},'','SubscrAddress2','',445,168,350,14),
					({POut.Long(claimFormNum2024)},'','OtherInsRelatIsOther','',354,340,0,14),
					({POut.Long(claimFormNum2024)},'','SubscrZip','',695,184,100,14),
					({POut.Long(claimFormNum2024)},'','SubscrIsMale','',574,217,0,0),
					({POut.Long(claimFormNum2024)},'','OtherInsSubscrIsFemale','',196,310,0,0),
					({POut.Long(claimFormNum2024)},'','OtherInsSubscrIsMale','',175,310,0,0),
					({POut.Long(claimFormNum2024)},'','SubscrIsFemale','',596,217,0,0),
					({POut.Long(claimFormNum2024)},'','SubscrIsGenderUnknown','',619,217,0,0),
					({POut.Long(claimFormNum2024)},'','SubscrCity','',445,184,185,14),
					({POut.Long(claimFormNum2024)},'','PreAuthString','',31,102,390,14),
					({POut.Long(claimFormNum2024)},'','Miss18','',315,685,0,0),
					({POut.Long(claimFormNum2024)},'','BillingDentistSSNorTIN','',295,1024,114,14),
					({POut.Long(claimFormNum2024)},'','IsPreAuth','',184,72,0,0),
					({POut.Long(claimFormNum2024)},'','OtherInsExistsDent','',74,250,0,14),
					({POut.Long(claimFormNum2024)},'','PriInsCarrierName','',32,146,350,14),
					({POut.Long(claimFormNum2024)},'','IsStandardClaim','',34,73,0,0),
					({POut.Long(claimFormNum2024)},'','PriInsCity','',32,194,185,14),
					({POut.Long(claimFormNum2024)},'','PriInsAddress2','',32,178,350,14),
					({POut.Long(claimFormNum2024)},'','PriInsAddress','',32,162,350,14),
					({POut.Long(claimFormNum2024)},'','OtherInsGroupNum','',34,338,130,14),
					({POut.Long(claimFormNum2024)},'','OtherInsSubscrID','',255,311,169,14),
					({POut.Long(claimFormNum2024)},'','PriInsZip','',282,194,100,14),
					({POut.Long(claimFormNum2024)},'','IsEnclosuresAttached','',770,742,0,0),
					({POut.Long(claimFormNum2024)},'','OtherInsRelatIsSelf','',174,340,0,14),
					({POut.Long(claimFormNum2024)},'','OtherInsExistsMed','',154,250,0,14),
					({POut.Long(claimFormNum2024)},'','OtherInsCarrierName','',34,370,350,14),
					({POut.Long(claimFormNum2024)},'','OtherInsSubscrDOB','MM/dd/yyyy',34,309,90,14),
					({POut.Long(claimFormNum2024)},'','OtherInsRelatIsSpouse','',224,340,0,14),
					({POut.Long(claimFormNum2024)},'','OtherInsRelatIsChild','',284,340,0,14),
					({POut.Long(claimFormNum2024)},'','OtherInsSubscrLastFirst','',34,280,390,14),
					({POut.Long(claimFormNum2024)},'','SubscrLastFirst','',445,136,350,14),
					({POut.Long(claimFormNum2024)},'','OtherInsST','',231,402,40,14),
					({POut.Long(claimFormNum2024)},'','OtherInsAddress','',34,386,350,14),
					({POut.Long(claimFormNum2024)},'','SubscrAddress','',445,152,350,14),
					({POut.Long(claimFormNum2024)},'','SubscrST','',643,184,40,14),
					({POut.Long(claimFormNum2024)},'','PriInsST','',230,194,40,14),
					({POut.Long(claimFormNum2024)},'','OtherInsCity','',34,402,185,14),
					({POut.Long(claimFormNum2024)},'','OtherInsZip','',284,402,100,14),
					({POut.Long(claimFormNum2024)},'','Miss30','',74,685,0,0),
					({POut.Long(claimFormNum2024)},'','Miss24','',194,685,0,0),
					({POut.Long(claimFormNum2024)},'','Miss28','',114,685,0,0),
					({POut.Long(claimFormNum2024)},'','IsOrtho','',533,791,0,0),
					({POut.Long(claimFormNum2024)},'','Miss23','',215,685,0,0),
					({POut.Long(claimFormNum2024)},'','PatientRelease','',39,800,240,14),
					({POut.Long(claimFormNum2024)},'','PatientAssignmentDate','MM/dd/yyyy',306,862,90,14),
					({POut.Long(claimFormNum2024)},'','ICDindicatorAB','',515,652,24,14),
					({POut.Long(claimFormNum2024)},'','DateOrthoPlaced','MM/dd/yyyy',675,790,90,14),
					({POut.Long(claimFormNum2024)},'','Miss31','',55,685,0,0),
					({POut.Long(claimFormNum2024)},'','Remarks','',86,702,743,25),
					({POut.Long(claimFormNum2024)},'','IsOccupational','',443,853,0,0),
					({POut.Long(claimFormNum2024)},'','IsReplacementProsth','',564,823,0,0),
					({POut.Long(claimFormNum2024)},'','DatePriorProsthPlaced','MM/dd/yyyy',674,823,90,14),
					({POut.Long(claimFormNum2024)},'','IsNotOrtho','',443,791,0,0),
					({POut.Long(claimFormNum2024)},'','IsNotReplacementProsth','',534,823,0,0),
					({POut.Long(claimFormNum2024)},'','IsAutoAccident','',593,854,0,0),
					({POut.Long(claimFormNum2024)},'','MonthsOrthoTotal','',434,823,30,14),
					({POut.Long(claimFormNum2024)},'','AccidentDate','MM/dd/yyyy',583,873,90,14),
					({POut.Long(claimFormNum2024)},'','TreatingDentistCity','',437,1024,185,14),
					({POut.Long(claimFormNum2024)},'','PatientReleaseDate','MM/dd/yyyy',306,800,90,14),
					({POut.Long(claimFormNum2024)},'','IsOtherAccident','',693,853,0,0),
					({POut.Long(claimFormNum2024)},'','AccidentST','',794,873,35,14),
					({POut.Long(claimFormNum2024)},'','PayToDentistAddress','',34,953,350,14),
					({POut.Long(claimFormNum2024)},'','BillingDentist','',34,937,350,14),
					({POut.Long(claimFormNum2024)},'','TreatingDentistST','',634,1024,40,14),
					({POut.Long(claimFormNum2024)},'','Miss16','',334,668,0,0),
					({POut.Long(claimFormNum2024)},'','TreatingDentistAddress','',437,1008,350,14),
					({POut.Long(claimFormNum2024)},'','Miss14','',295,668,0,0),
					({POut.Long(claimFormNum2024)},'','Miss11','',235,668,0,0),
					({POut.Long(claimFormNum2024)},'','BillingDentistLicenseNum','',166,1024,109,14),
					({POut.Long(claimFormNum2024)},'','TreatingDentistPh78910','',558,1046,40,14),
					({POut.Long(claimFormNum2024)},'','TreatingDentistPh456','',519,1046,30,14),
					({POut.Long(claimFormNum2024)},'','TreatingDentistPh123','',480,1046,30,14),
					({POut.Long(claimFormNum2024)},'','PayToDentistAddress2','',34,969,350,14),
					({POut.Long(claimFormNum2024)},'','TreatingDentistLicense','',701,970,128,14),
					({POut.Long(claimFormNum2024)},'','Miss6','',135,668,0,0),
					({POut.Long(claimFormNum2024)},'','Miss5','',114,668,0,0),
					({POut.Long(claimFormNum2024)},'','TreatingDentistZip','',686,1024,100,14),
					({POut.Long(claimFormNum2024)},'','Miss7','',156,668,0,0),
					({POut.Long(claimFormNum2024)},'','Miss15','',315,668,0,0),
					({POut.Long(claimFormNum2024)},'','TotalFee','',829,684,68,14),
					({POut.Long(claimFormNum2024)},'','BillingDentistNPI','',35,1024,111,14),
					({POut.Long(claimFormNum2024)},'','TreatingProviderSpecialty','',701,988,128,14),
					({POut.Long(claimFormNum2024)},'','Miss9','',194,668,0,0),
					({POut.Long(claimFormNum2024)},'','PlaceNumericCode','',511,745,27,14),
					({POut.Long(claimFormNum2024)},'','Miss12','',254,668,0,0),
					({POut.Long(claimFormNum2024)},'','Miss13','',275,668,0,0),
					({POut.Long(claimFormNum2024)},'','DiagnosisA','',502,667,81,14),
					({POut.Long(claimFormNum2024)},'','DiagnosisC','',610,667,81,14),
					({POut.Long(claimFormNum2024)},'','TreatingDentistNPI','',450,972,168,14),
					({POut.Long(claimFormNum2024)},'','Miss17','',334,685,0,0),
					({POut.Long(claimFormNum2024)},'','P3DiagnosisPoint','',441,517,48,14),
					({POut.Long(claimFormNum2024)},'','P4DiagnosisPoint','',441,534,48,14),
					({POut.Long(claimFormNum2024)},'','P2DiagnosisPoint','',441,500,48,14),
					({POut.Long(claimFormNum2024)},'','DiagnosisB','',502,684,81,14),
					({POut.Long(claimFormNum2024)},'','P7DiagnosisPoint','',441,585,48,14),
					({POut.Long(claimFormNum2024)},'','P6DiagnosisPoint','',441,568,48,14),
					({POut.Long(claimFormNum2024)},'','P8DiagnosisPoint','',441,602,48,14),
					({POut.Long(claimFormNum2024)},'','P5DiagnosisPoint','',441,551,48,14),
					({POut.Long(claimFormNum2024)},'','P1DiagnosisPoint','',441,484,48,14),
					({POut.Long(claimFormNum2024)},'','P4UnitQty','',491,534,28,14),
					({POut.Long(claimFormNum2024)},'','DiagnosisD','',610,684,81,14),
					({POut.Long(claimFormNum2024)},'','P2UnitQty','',491,500,28,14),
					({POut.Long(claimFormNum2024)},'','P9DiagnosisPoint','',441,618,48,14),
					({POut.Long(claimFormNum2024)},'','P7UnitQty','',491,585,28,14),
					({POut.Long(claimFormNum2024)},'','P10DiagnosisPoint','',441,635,48,14),
					({POut.Long(claimFormNum2024)},'','P1UnitQty','',491,484,28,14),
					({POut.Long(claimFormNum2024)},'','P10UnitQty','',491,635,28,14),
					({POut.Long(claimFormNum2024)},'','P5UnitQty','',491,551,28,14),
					({POut.Long(claimFormNum2024)},'','BillingDentistProviderID','',299,1047,114,14),
					({POut.Long(claimFormNum2024)},'','P9UnitQty','',491,618,28,14),
					({POut.Long(claimFormNum2024)},'','P3UnitQty','',491,517,28,14),
					({POut.Long(claimFormNum2024)},'','P8UnitQty','',491,602,28,14),
					({POut.Long(claimFormNum2024)},'','PatientPatNum','',662,418,167,14),
					({POut.Long(claimFormNum2024)},'','Miss22','',235,685,0,0),
					({POut.Long(claimFormNum2024)},'','Miss27','',135,685,0,0),
					({POut.Long(claimFormNum2024)},'','P6UnitQty','',491,568,28,14),
					({POut.Long(claimFormNum2024)},'','Miss29','',95,685,0,0),
					({POut.Long(claimFormNum2024)},'','Miss26','',156,685,0,0),
					({POut.Long(claimFormNum2024)},'','Miss21','',254,685,0,0),
					({POut.Long(claimFormNum2024)},'','PatientAssignment','',39,862,240,14),
					({POut.Long(claimFormNum2024)},'','Miss32','',35,685,0,0),
					({POut.Long(claimFormNum2024)},'','Miss20','',275,685,0,0),
					({POut.Long(claimFormNum2024)},'','P1Date','MM/dd/yyyy',35,484,90,14),
					({POut.Long(claimFormNum2024)},'','Miss25','',175,685,0,0),
					({POut.Long(claimFormNum2024)},'','PriInsElectID','',77,218,167,14),
					({POut.Long(claimFormNum2024)},'','SubscrID','',662,218,167,14),
					({POut.Long(claimFormNum2024)},'','OtherInsElectID','',106,421,167,14);";
				Db.NonQ(command);
				//Switch all of the insplan using the 2019 to the new 2024 claim form.
				command="SELECT ClaimFormNum FROM claimform WHERE Description='ADA 2019'";
				long claimFormNum2019=Db.GetLong(command);
				if(claimFormNum2019>0) {
					command=$"UPDATE insplan SET ClaimFormNum={POut.Long(claimFormNum2024)} WHERE ClaimFormNum>0 AND ClaimFormNum={POut.Long(claimFormNum2019)}";
					Db.NonQ(command);
					//Set the new 2024 claimform as the Default claimform
					command=$"UPDATE preference SET ValueString='{POut.Long(claimFormNum2024)}' WHERE PrefName='DefaultClaimForm' AND ValueString='{POut.Long(claimFormNum2019)}'";
					Db.NonQ(command);
				}
			}
			command="SELECT COUNT(*) FROM preference WHERE PrefName='ProgramLinksDisabledForWeb'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('ProgramLinksDisabledForWeb','Apixia,AudaxCeph,CADI,DBSWin,DemandForce,DentalEye,"
				+"DentalTekSmartOfficePhone,DentX,Dolphin,EvaSoft,iCat,IAP,Guru,HouseCalls,MediaDent,Owandy,PandaPerioAdvanced,Patterson,PT,PTupdate,Schick,Trojan,TigerView,"
				+"TrophyEnhanced,UAppoint,Vipersoft,VixWinOld,Xcharge')";
				Db.NonQ(command);
			}
		}//End of To23_2_26 method

		private static void To23_2_27() {
			//Switch all of the insplan using the 2024 back to 2019 claim form.
			string command="SELECT ClaimFormNum FROM claimform WHERE Description='ADA 2019'";
			long claimFormNum2019=Db.GetLong(command);
			command="SELECT ClaimFormNum FROM claimform WHERE Description='ADA 2024'";
			long claimFormNum2024=Db.GetLong(command);
			if(claimFormNum2019>0 && claimFormNum2024>0) {
				command=$"UPDATE insplan SET ClaimFormNum={POut.Long(claimFormNum2019)} WHERE ClaimFormNum>0 AND ClaimFormNum={POut.Long(claimFormNum2024)}";
				Db.NonQ(command);
				//Set the default claimform back to the 2019 claimform
				command=$"UPDATE preference SET ValueString='{POut.Long(claimFormNum2019)}' WHERE PrefName='DefaultClaimForm' AND ValueString='{POut.Long(claimFormNum2024)}'";
				Db.NonQ(command);
			}
		}

		private static void To23_2_28() {
			string command="SELECT ProgramNum FROM program WHERE ProgName='CareCredit'";
			long programNum=Db.GetLong(command);
			if(programNum>0) {
				string propertyDesc="CareCreditPartnerCode";
				command=$"SELECT * FROM programproperty where ProgramNum={POut.Long(programNum)} AND PropertyDesc='{POut.String(propertyDesc)}'";
				DataTable table=Db.GetTable(command);
				if(table.Rows.Count==0) {
					command=$"INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) VALUES(" +
						$"{POut.Long(programNum)}, " +
						$"'{POut.String(propertyDesc)}', " +
						$"'{POut.Long(1065)}')"; //1065 is the hard coded value we're replacing in the CareCredit bridges solution
					Db.NonQ(command);
				}
			}
		}

		private static void To23_2_30() {
			//Change the alignment of the TreatingProviderSpecialty and TreatingDentistProviderID claimformitem of the 2024 claim form.
			string command="SELECT ClaimFormNum FROM claimform WHERE Description='ADA 2024'";
			long claimFormNum2024=Db.GetLong(command);
			if(claimFormNum2024>0) {
				//Only change if the XPos, YPos, and Width are the default values that we used when we created the claim form.
				command="SELECT ClaimFormItemNum " +
					"FROM claimformitem " +
					$"WHERE ClaimFormNum={POut.Long(claimFormNum2024)} AND FieldName='TreatingProviderSpecialty' " +
					$"AND XPos=701 AND YPos=988 AND Width=128";//Default values
				long claimFormItemNum=Db.GetLong(command);
				if(claimFormItemNum>0) {
					command=$"UPDATE claimformitem SET XPos=738, YPos=991, Width=92 WHERE ClaimFormItemNum={POut.Long(claimFormItemNum)}";
					Db.NonQ(command);
				}
				command="SELECT ClaimFormItemNum " +
					"FROM claimformitem " +
					$"WHERE ClaimFormNum={POut.Long(claimFormNum2024)} AND FieldName='TreatingDentistProviderID' " +
					$"AND XPos=684 AND YPos=1046 AND Width=145";//Default values
				claimFormItemNum=Db.GetLong(command);
				if(claimFormItemNum>0) {
					command=$"UPDATE claimformitem SET XPos=689, YPos=1046, Width=140 WHERE ClaimFormItemNum={POut.Long(claimFormItemNum)}";
					Db.NonQ(command);
				}
			}
		}

		private static void To23_2_54() {
			//Start E50054
			string command="UPDATE claimproc SET NoBillIns=0 WHERE IsOverPay!=0";
			Db.NonQ(command);
			//End E50054
		}

		private static void To23_2_67() {
			FixADA2024IsPreAuthLocation();
		}//End of 23_2_67() method

		private static void To23_3_1() {
			string command;
			//F44596 - Frequency Limitations by Treatment Area
			command="ALTER TABLE benefit ADD TreatArea tinyint NOT NULL";
			Db.NonQ(command);
			//B46378 - Definitions window hide Adjustment Type created wrong message
			command="UPDATE preference SET ValueString='0' WHERE PrefName='RefundAdjustmentType'";//clearing this preference since it was never implemented
			//Users were able to set a value for this preference for a brief time before it was removed from the UI.That locks any value the was previosly set by the user.
			//This job will allow users to hide the preference associated with the RefundAdjustmentType.
			Db.NonQ(command);
			//End of B46378
			//I45584 SkySQL - Reporting Server
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ReportingServerSslCa','')";
			Db.NonQ(command);
			//end I45584
			//I46592 - EService Listener - Enable by default
			command="UPDATE preference SET ValueString='1' WHERE PrefName='EServiceListenerEnabled'";
			Db.NonQ(command);
			//end I46592
			DataTable table;
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EclaimsSubscIDUsesPatID','0')";
			Db.NonQ(command);
			//Insert One2 bridge----------------------------------------------------------------- 
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
				 +") VALUES("
				 +"'One2', "
				 +"'One2 from https://osstem.com/', "
				 +"'0', "
				 +"'"+POut.String(@"C:\osstem\onevision\one2\oov.exe")+"', "
				 +"'', "//leave blank if none 
				 +"'')";
			long programNum = Db.NonQ(command,true);
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
				+") VALUES("
				+"'"+POut.Long(programNum)+"', "
				+"'"+POut.String("Enter 0 to use PatientNum, or 1 to use ChartNum")+"', "
				+"'0')";
			Db.NonQ(command); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
				 +") VALUES(" 
				 +"'"+POut.Long(programNum)+"', " 
				 +"'XML output file path', " 
				 +"'"+POut.String(@"C:\osstem\patient.xml")+"')";
			Db.NonQ(command); 
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
				 +"VALUES ("
				 +"'"+POut.Long(programNum)+"', "
				 +"'2', "//ToolBarsAvail.ChartModule 
				 +"'One2')";
			Db.NonQ(command);
			//end One2 bridge 
			//F42018
			command="INSERT INTO alertcategory(IsHQCategory,InternalName,Description) VALUES('1','ClearedNoteSignatures','Cleared Note Signatures')";
			long alertCatNum=Db.NonQ(command,true);
			command=$"INSERT INTO alertcategorylink(AlertCategoryNum,AlertType) VALUES("+POut.Long(alertCatNum)+",39)";
			Db.NonQ(command);
			//End F42018
			if(!LargeTableHelper.IndexExists("taskancestor","TaskListNum,TaskNum")) {
				command="ALTER TABLE taskancestor ADD INDEX TaskListCov (TaskListNum,TaskNum)";
				List<string> listIndexNames=LargeTableHelper.GetIndexNames("taskancestor","TaskListNum");
				if(listIndexNames.Count>0) {
					command+=","+string.Join(",",listIndexNames.Select(x => $"DROP INDEX {POut.String(x)}"));
				}
				Db.NonQ(command);
			}
			command="ALTER TABLE imagedraw ADD ImageAnnotVendor tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE imagedraw ADD Details text NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ClaimFinalizeWarning','0')";
			Db.NonQ(command);
			command="ALTER TABLE mobileappdevice ADD IsODTouchEnabled tinyint NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES ('ODTouchDeviceLimit','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES ('IsODTouchEnabled','0')";
			Db.NonQ(command);
			command="ALTER TABLE mobileappdevice CHANGE IsAllowed IsEclipboardEnabled tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE mobileappdevice CHANGE LastLogin EclipboardLastLogin datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			command="ALTER TABLE mobileappdevice CHANGE LastAttempt EclipboardLastAttempt datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			command="ALTER TABLE mobileappdevice ADD ODTouchLastLogin datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			command="ALTER TABLE mobileappdevice ADD ODTouchLastAttempt datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			command="ALTER TABLE procedurecode ADD DiagnosticCodes varchar(255) NOT NULL";//F47318
			Db.NonQ(command);
			//I48496
			command="UPDATE displayreport SET Description='Payment Plans Overcharged' WHERE DisplayReportNum=58";
			Db.NonQ(command);
			//End I48496
			//I47350
			command="INSERT INTO preference(PrefName,ValueString) VALUES ('EraAutoPostWriteOff','1')";
			Db.NonQ(command);
			//End I47350
		}//End of 23_3_1() method

		private static void To23_3_2() {
			string command="SELECT ProgramNum FROM program WHERE ProgName='CareCredit'";
			long programNum=Db.GetLong(command);
			if(programNum>0) {
				string propertyDesc="CareCreditPartnerCode";
				command=$"SELECT * FROM programproperty where ProgramNum={POut.Long(programNum)} AND PropertyDesc='{POut.String(propertyDesc)}'";
				DataTable table=Db.GetTable(command);
				if(table.Rows.Count==0) {
					command=$"INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) VALUES(" +
						$"{POut.Long(programNum)}, " +
						$"'{POut.String(propertyDesc)}', " +
						$"'{POut.Long(1065)}')"; //1065 is the hard coded value we're replacing in the CareCredit bridges solution
					Db.NonQ(command);
				}
			}
			//Insert SOTACloud bridge----------------------------------------------------------------- 
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
				 +") VALUES("
				 +"'SOTACloud', "
				 +"'SOTA Cloud from sotacloud.com', "
				 +"'0', " 
				 +"'', "//This is a API based bridge, so there is no local .exe.
				 +"'', "//No cmd line args.
				 +"'')";
			programNum=Db.NonQ(command,true);
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
				+") VALUES("
				+"'"+POut.Long(programNum)+"', "
				+"'"+POut.String("Enter 0 to use PatientNum, or 1 to use ChartNum")+"', "
				+"'0')";
			Db.NonQ(command); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
				 +") VALUES("
				 +"'"+POut.Long(programNum)+"', "
				 +"'"+POut.String("Practice Instance Name")+"', "
				 +"'')";
			Db.NonQ(command);
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
				 +"VALUES ("
				 +"'"+POut.Long(programNum)+"', "
				 +"'2', "//ToolBarsAvail.ChartModule
				 +"'SOTA Cloud')";
			Db.NonQ(command);
			//end SOTACloud bridge 
		}

		private static void To23_3_4() {
			//Change the alignment of the TreatingProviderSpecialty and TreatingDentistProviderID claimformitem of the 2024 claim form.
			string command="SELECT ClaimFormNum FROM claimform WHERE Description='ADA 2024'";
			long claimFormNum2024=Db.GetLong(command);
			if(claimFormNum2024>0) {
				//Only change if the XPos, YPos, and Width are the default values that we used when we created the claim form.
				command="SELECT ClaimFormItemNum " +
					"FROM claimformitem " +
					$"WHERE ClaimFormNum={POut.Long(claimFormNum2024)} AND FieldName='TreatingProviderSpecialty' " +
					$"AND XPos=701 AND YPos=988 AND Width=128";//Default values
				long claimFormItemNum=Db.GetLong(command);
				if(claimFormItemNum>0) {
					command=$"UPDATE claimformitem SET XPos=738, YPos=991, Width=92 WHERE ClaimFormItemNum={POut.Long(claimFormItemNum)}";
					Db.NonQ(command);
				}
				command="SELECT ClaimFormItemNum " +
					"FROM claimformitem " +
					$"WHERE ClaimFormNum={POut.Long(claimFormNum2024)} AND FieldName='TreatingDentistProviderID' " +
					$"AND XPos=684 AND YPos=1046 AND Width=145";//Default values
				claimFormItemNum=Db.GetLong(command);
				if(claimFormItemNum>0) {
					command=$"UPDATE claimformitem SET XPos=689, YPos=1046, Width=140 WHERE ClaimFormItemNum={POut.Long(claimFormItemNum)}";
					Db.NonQ(command);
				}
			}
		}

		private static void To23_3_6() {
			string command="SELECT COUNT(*) FROM preference WHERE PrefName='WebSchedManualSendTriggered'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('WebSchedManualSendTriggered','')";
				Db.NonQ(command);
			}
		}

		private static void To23_3_22() {
			//Start E50054
			string command="UPDATE claimproc SET NoBillIns=0 WHERE IsOverPay!=0";
			Db.NonQ(command);
			//End E50054
		}

		private static void To23_3_28() {
			string command="ALTER TABLE payment ADD INDEX (ProcessStatus)";
			Db.NonQ(command);
			long databaseMode=Db.GetLong("SELECT ValueString FROM preference WHERE PrefName='DatabaseMode'");
			command="INSERT INTO preference(PrefName,ValueString) VALUES('DatabaseGlobalVariablesDontSet','"+POut.Long(databaseMode)+"')";
			Db.NonQ(command);
		}

		private static void To23_3_30() {
			string command="INSERT INTO preference(PrefName,ValueString) VALUES('WebServiceServerNameCanBeBlank','0')";
			Db.NonQ(command);
		}

		private static void To23_3_34() {
			string command="SELECT COUNT(*) FROM preference WHERE PrefName='CloudIsAppStream'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('CloudIsAppStream','0')";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM preference WHERE PrefName='CloudFileWatcherDirectory'";
			if(Db.GetInt(command)==0) {
				command=@"INSERT INTO preference(PrefName,ValueString) VALUES('CloudFileWatcherDirectory','"+POut.String(@"C:\ODCloudClientTransfer\Standard")+"')";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM preference WHERE PrefName='CloudFileWatcherDirectoryAPI'";
			if(Db.GetInt(command)==0) {
				command=@"INSERT INTO preference(PrefName,ValueString) VALUES('CloudFileWatcherDirectoryAPI','"+POut.String(@"C:\ODCloudClientTransfer\API")+"')";
				Db.NonQ(command);
			}
		}

		private static void To23_3_38() {
			string command;
			DataTable table;
			command="ALTER TABLE claim ADD SecurityHash varchar(255) NOT NULL";
			Db.NonQ(command);
			LargeTableHelper.AlterTable("claimproc","ClaimProcNum",new LargeTableHelper.ColNameAndDef("SecurityHash","varchar(255) NOT NULL"));
			//Misc.SecurityHash.UpdateHashing();
		}//End of 23_3_38() method

		private static void To23_3_40() {
			FixADA2024IsPreAuthLocation();
		}//End of 23_3_40() method

		private static void To23_3_42() {
			string command;
			//Start E52592
			command="INSERT INTO preference(PrefName,ValueString) VALUES('PayPlanRequireLockForAPR','1')";
			Db.NonQ(command);
			//End E52592
		}//End of 23_3_42() method

		private static void To23_3_50() {
			FixADA2024IsPreAuthLocation();//B53212
		}//End of 23_3_50() method

		private static void To23_3_57() {
			//Start B53978
			AddAlertTypesToOdAllTypesCategory(alertTypeNum:39);//AlertType.SignatureCleared
			//End B53978
		}//End of 23_3_57() method

		private static void To23_3_59() {
			//Start B53968
			string command="UPDATE preference SET ValueString='time.nist.gov' WHERE ValueString='nist-time-server.eoni.com' AND PrefName='NistTimeServerUrl'";
			Db.NonQ(command);
			//End B53968
		}//End of 23_3_59()

		private static void To24_1_1() {
			string command;
			DataTable table;
			//I46602 - insurance card scanner
			LargeTableHelper.AlterTable("document","DocNum",new LargeTableHelper.ColNameAndDef("OcrResponseData","text NOT NULL"));
			LargeTableHelper.AlterTable("document","DocNum",new LargeTableHelper.ColNameAndDef("ImageCaptureType","tinyint NOT NULL DEFAULT 0"));
			command="ALTER TABLE eclipboardimagecapture ADD OcrCaptureType tinyint NOT NULL DEFAULT '0'";
			Db.NonQ(command);
			command="ALTER TABLE eclipboardimagecapturedef ADD OcrCaptureType tinyint NOT NULL DEFAULT '0'";
			Db.NonQ(command);
			//end I46602
			//Start E49896
			command="INSERT INTO preference(PrefName,ValueString) VALUES('BillingEmailIncludeAutograph','0')";//Default to false for backward compatibility.
			Db.NonQ(command);
			//End E49896
			command="ALTER TABLE clearinghouse ADD LocationID VARCHAR(255) NOT NULL DEFAULT ''";
			DataCore.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('SaveEDSAttachments','1')";
			Db.NonQ(command);
			//Start E34842
			//CareCreditPreApprovalAmt
			command="SELECT ProgramNum FROM program WHERE ProgName='CareCredit'";
			long programNum=Db.GetLong(command);
			command="SELECT MAX(ItemOrder)+1 FROM patfielddef";
			int maxOrder=Db.GetInt(command);
			command=$"INSERT INTO patfielddef (FieldName,FieldType,ItemOrder) VALUES ('{POut.String("CareCredit Pre-Approval Amount")}',7,{POut.Int(maxOrder)})";
			long patDefNum=Db.NonQ(command,true);
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) "
				 +"VALUES("
				 +POut.Long(programNum)+", "
				 +"'CareCreditPatFieldPreApprovalAmt', "
				 +"'"+POut.Long(patDefNum)+"')";
			Db.NonQ(command);
			//CareCreditAvailableCredit
			command="SELECT MAX(ItemOrder)+1 FROM patfielddef";
			maxOrder=Db.GetInt(command);
			command=$"INSERT INTO patfielddef (FieldName,FieldType,ItemOrder) VALUES ('{POut.String("CareCredit Available Credit")}',8,{POut.Int(maxOrder)})";
			patDefNum=Db.NonQ(command,true);
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) "
				 +"VALUES("
				 +POut.Long(programNum)+", "
				 +"'CareCreditPatFieldAvailableCredit', "
				 +"'"+POut.Long(patDefNum)+"')";
			Db.NonQ(command);
			//End E34842
			#region S49090 - Benefit age limitations for fluoride and sealants, changed to use CodeGroups.
			command="ALTER TABLE codegroup ADD ShowInAgeLimit tinyint NOT NULL";
			Db.NonQ(command);
			// Codes are D1351 and D1206 for US, and 13401 and 12111 for Canada.
			// Get the CodeGroupNums for Fluoride and Sealant.
			string procCodeFlo="D1206";
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canada
				procCodeFlo="12111";
			}
			string procCodeSeal="D1351";
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				procCodeSeal="13401";
			}
			command="SELECT CodeGroupNum FROM codegroup WHERE ProcCodes LIKE '%"+procCodeFlo+"%'";
			long codeGroupNumFlo=Db.GetLong(command);
			if(codeGroupNumFlo==0) {
				// If one doesn't exist, create one and take its CodeGroupNum.
				command="SELECT COUNT(*) FROM codegroup";
				int count=Db.GetInt(command);//0-based. Example existing 0-4, this gets 5.
				command="INSERT INTO codegroup (GroupName, ProcCodes,ItemOrder,CodeGroupFixed) "
					+"VALUES ('Fluoride','" + procCodeFlo + "'," + POut.Int(count) + ",8)";// 8 = EnumCodeGroupFixed.Fluoride
				Db.NonQ(command);
				command="SELECT CodeGroupNum FROM codegroup WHERE ProcCodes LIKE '%" + procCodeFlo + "%'";
				codeGroupNumFlo=Db.GetLong(command);
			}
			command="SELECT CodeGroupNum FROM codegroup WHERE ProcCodes LIKE '%" + procCodeSeal + "%'";
			long codeGroupNumSeal=Db.GetLong(command);
			if(codeGroupNumSeal==0) {
				// If one doesn't exist, create one and take its CodeGroupNum.
				command="SELECT COUNT(*) FROM codegroup";
				int count=Db.GetInt(command);
				command="INSERT INTO codegroup (GroupName, ProcCodes,ItemOrder,CodeGroupFixed) "
					+ "VALUES ('Sealant','" + procCodeSeal + "'," + POut.Int(count) + ",9)"; // 9 = EnumCodeGroupFixed.Sealant
				Db.NonQ(command);
				command="SELECT CodeGroupNum FROM codegroup WHERE ProcCodes LIKE '%" + procCodeSeal + "%'";
				codeGroupNumSeal=Db.GetLong(command);
			}
			// Update the Fluoride and Sealant CodeGroups' ShowInAgeLimit property to be True.
			command="UPDATE codegroup SET ShowInAgeLimit=1 "
				+ "WHERE CodeGroupNum IN (" + POut.Long(codeGroupNumFlo) + "," + POut.Long(codeGroupNumSeal) + ")";
			Db.NonQ(command);
			// Get the codenum for Fluoride
			command="SELECT CodeNum FROM procedurecode WHERE ProcCode='" + procCodeFlo + "'";
			long codeNumFlo=Db.GetLong(command);
			// Get the codenum for Sealant
			command="SELECT CodeNum FROM procedurecode WHERE ProcCode='" + procCodeSeal + "'";
			long codeNumSeal=Db.GetLong(command);
			// Update the Fluoride Benefits.
			command="UPDATE benefit"
				+ " SET CodeNum=0, CodeGroupNum=" + POut.Long(codeGroupNumFlo)
				+ " WHERE CodeNum=" + POut.Long(codeNumFlo) 
				+ " AND BenefitType=" + POut.Enum(InsBenefitType.Limitations) 
				+ " AND MonetaryAmt=-1"
				+ " AND PatPlanNum=0"
				+ " AND Percent=-1"
				+ " AND QuantityQualifier=" + POut.Enum(BenefitQuantity.AgeLimit)
				+ " AND CoverageLevel=" + POut.Enum(BenefitCoverageLevel.None);
			Db.NonQ(command);
			// Update the Sealant Benefits.
			command="UPDATE benefit"
				+ " SET CodeNum=0, CodeGroupNum=" + POut.Long(codeGroupNumSeal)
				+ " WHERE CodeNum=" + POut.Long(codeNumSeal)
				+ " AND BenefitType=" + POut.Enum(InsBenefitType.Limitations)
				+ " AND MonetaryAmt=-1"
				+ " AND PatPlanNum=0"
				+ " AND Percent=-1"
				+ " AND QuantityQualifier=" + POut.Enum(BenefitQuantity.AgeLimit)
				+ " AND CoverageLevel=" + POut.Enum(BenefitCoverageLevel.None);
			Db.NonQ(command);
			#endregion S49090 - Benefit age limitations for fluoride and sealants, changed to use CodeGroups.
			//I47180 - ODCloudClient for AWS AppStream 2.0
			command="SELECT COUNT(*) FROM preference WHERE PrefName='CloudIsAppStream'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('CloudIsAppStream','0')";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM preference WHERE PrefName='CloudFileWatcherDirectory'";
			if(Db.GetInt(command)==0) {
				command=@"INSERT INTO preference(PrefName,ValueString) VALUES('CloudFileWatcherDirectory','"+POut.String(@"C:\ODCloudClientTransfer\Standard")+"')";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM preference WHERE PrefName='CloudFileWatcherDirectoryAPI'";
			if(Db.GetInt(command)==0) {
				command=@"INSERT INTO preference(PrefName,ValueString) VALUES('CloudFileWatcherDirectoryAPI','"+POut.String(@"C:\ODCloudClientTransfer\API")+"')";
				Db.NonQ(command);
			}
			command="INSERT INTO preference(PrefName,ValueString) VALUES('FamilyBalancerChangedSinceNumDays','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('FamilyBalancerChangedSinceUseLastDayRun','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('FamilyBalancerDateLastRun',"+POut.DateT(DateTime.MinValue)+")";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('FamilyBalancerDeleteAllTransfers','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('FamilyBalancerEnabled','0')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('FamilyBalancerTimeRun',"+POut.DateT(DateTime.MinValue)+")";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('FamilyBalancerUseFIFO','1')";
			Db.NonQ(command);
			//Fixing the convert script for B46378
			long preferenceNextPk=Db.GetLong("SELECT MAX(PrefNum) FROM preference")+1;
			//First add a primary key value back.
			command=$"UPDATE preference SET PrefNum={POut.Long(preferenceNextPk)} WHERE PrefName='RefundAdjustmentType' AND PrefNum=0";
			Db.NonQ(command);
			//Next clear out the ValueString like B46378 intended if they failed to get the correct convert script.
			command=$"UPDATE preference SET ValueString='0' WHERE PrefName='RefundAdjustmentType'";
			Db.NonQ(command);
			command="ALTER TABLE sheet ADD WebFormSheetID bigint NOT NULL, ADD INDEX (WebFormSheetID)";
			Db.NonQ(command);
			#region h43092 - Patient Field Enhancements
			command="DROP TABLE IF EXISTS patfieldpickitem";
			Db.NonQ(command);
			command=@"CREATE TABLE patfieldpickitem (
				PatFieldPickItemNum bigint NOT NULL auto_increment PRIMARY KEY,
				PatFieldDefNum bigint NOT NULL,
				Name varchar(255) NOT NULL,
				Abbreviation varchar(255) NOT NULL,
				IsHidden tinyint NOT NULL,
				ItemOrder int NOT NULL,
				INDEX(PatFieldDefNum)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="SELECT PickList, PatFieldDefNum FROM patfielddef WHERE FieldType=1 ";//1=PickList
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				string pickListOld=table.Rows[i]["PickList"].ToString();
				long patFieldDefNum=(long)table.Rows[i]["PatFieldDefNum"];
				List<string> listPickListItems=pickListOld.Split("\r\n",StringSplitOptions.RemoveEmptyEntries).ToList();
				for(int j=0;j<listPickListItems.Count;j++) {
					command="INSERT INTO patfieldpickitem (PatFieldDefNum,Name,ItemOrder) VALUES("
						+    POut.Long  (patFieldDefNum)+","
						+"'"+POut.String(listPickListItems[j])+"',"
						+    POut.Int   (j)+")";
					Db.NonQ(command);
				}
			}
			command="UPDATE patfielddef SET PickList=''";
			Db.NonQ(command);
			#endregion h43092 - Patient Field Enhancements
			//F51274 NADG provided the default values. Descriptions for each code in file attached to the job.
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EraNoAutoProcessCarcCodes','97,16,252,242,50,251,B7,226,250')";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS mobilenotification";
			Db.NonQ(command);
			command=@"CREATE TABLE mobilenotification (
				MobileNotificationNum bigint NOT NULL auto_increment PRIMARY KEY,
				NotificationType tinyint NOT NULL,
				DeviceId varchar(255) NOT NULL,
				PrimaryKeys text NOT NULL,
				Tags text NOT NULL,
				DateTimeEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				DateTimeExpires datetime NOT NULL DEFAULT '0001-01-01 00:00:00'
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//F48814 - Automatically recalculate secondary claims when the primary claim is received
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ClaimPrimaryReceivedRecalcSecondary','1')";
			Db.NonQ(command);
			//F48814 - End
			//E47666 - CareCredit - Changes Needed to Support Consumer Self-Service
			command="UPDATE programproperty SET PropertyValue='2' WHERE PropertyDesc='CareCreditQSBatchDays' AND PropertyValue='1'";
			Db.NonQ(command);
			//end E47666
			//Starting MsgToPay convert
			command="INSERT INTO preference (PrefName,ValueString) VALUES('PaymentPortalMsgToPayTextMessageTemplate','Your balance with [ClinicName] is $[StatementBalance]. To make your payment, visit [MsgToPayURL].\r\n\r\nTo view your statement, go to [StatementURL].\"')";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('PaymentPortalMsgToPaySubjectTemplate','New Statement From [ClinicName]')";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES('PaymentPortalMsgToPayEmailMessageTemplate','{\"Template\":\"Your balance with [ClinicName] is $[StatementBalance]. To make your payment, visit [MsgToPayURL].\r\n\r\nTo view your statement, go to [StatementURL].\",\"EmailType\":1}')";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS msgtopaysent";
			Db.NonQ(command);
			command=@"CREATE TABLE msgtopaysent (
				MsgToPaySentNum bigint NOT NULL auto_increment PRIMARY KEY,
				PatNum bigint NOT NULL,
				ClinicNum bigint NOT NULL,
				SendStatus tinyint NOT NULL,
				Source tinyint NOT NULL,
				MessageType tinyint NOT NULL,
				MessageFk bigint NOT NULL,
				Subject text NOT NULL,
				Message text NOT NULL,
				EmailType tinyint NOT NULL,
				DateTimeEntry datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				DateTimeSent datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				ResponseDescript text NOT NULL,
				ApptReminderRuleNum bigint NOT NULL,
				ShortGUID varchar(255) NOT NULL,
				DateTimeSendFailed datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				INDEX(PatNum),
				INDEX(DateTimeSent),
				INDEX(ClinicNum),
				INDEX(MessageFk)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//End of MsgToPay convert
			command="INSERT INTO preference (PrefName,ValueString) VALUES('OCRClinicsSignedUp','')";
			Db.NonQ(command);
		}//End of 24_1_1() method

		private static void To24_1_4() {
			string command;
			DataTable table;
			command="INSERT INTO preference(PrefName,ValueString) VALUES('MsgToPaySendThreadFrequency','5')";
			Db.NonQ(command);
		}//End of 24_1_4() method

		private static void To24_1_5() {
			string command;
			DataTable table;
			if(!LargeTableHelper.ColumnExists(LargeTableHelper.GetCurrentDatabase(),"claim","SecurityHash")) {
				command="ALTER TABLE claim ADD SecurityHash varchar(255) NOT NULL";
				Db.NonQ(command);
			}
			LargeTableHelper.AlterTable("claimproc","ClaimProcNum",new LargeTableHelper.ColNameAndDef("SecurityHash","varchar(255) NOT NULL"));
			//Misc.SecurityHash.UpdateHashing();
		}//End of 24_1_5() method

		private static void To24_1_8() {
			FixADA2024IsPreAuthLocation();
		}//End of 24_1_8() method

		private static void To24_1_11() {
			string command="SELECT ValueString FROM preference WHERE PrefName='PaymentPortalMsgToPayTextMessageTemplate'";
			string valueString=Db.GetScalar(command);
			if(valueString=="Your balance with [ClinicName] is $[StatementBalance]. To make your payment, visit [MsgToPayURL].\r\n\r\nTo view your statement, go to [StatementURL].\"") {
				command="UPDATE preference SET ValueString='Your balance with [ClinicName] is $[StatementBalance]. To make your payment, visit [MsgToPayURL].\r\n\r\nTo view your statement, go to [StatementURL].' WHERE PrefName='PaymentPortalMsgToPayTextMessageTemplate'";
				Db.NonQ(command);
			}
		}//End of 24_1_11() method

		private static void To24_1_12() {
			string command;
			//Start E52592
			command="SELECT COUNT(*) FROM preference WHERE PrefName='PayPlanRequireLockForAPR'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('PayPlanRequireLockForAPR','1')";
				Db.NonQ(command);
			}
			//End E52592
		}//End of 24_1_12() method

		private static void To24_1_18() {
			string command;
			LargeTableHelper.AlterTable("document","DocNum",new LargeTableHelper.ColNameAndDef("PrintHeading","tinyint NOT NULL"));
			command="UPDATE document SET PrintHeading=1 WHERE ImgType=1";//1=radiograph
			Db.NonQ(command);
		}

		private static void To24_1_21() {
			FixADA2024IsPreAuthLocation();
		}//End of 24_1_21() method

		private static void To24_1_23() {
			string command;
			command="ALTER TABLE claim ADD Narrative text NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('OmhNy','0')";
			Db.NonQ(command);
		}//End of 24_1_23() method

		private static void To24_1_24() {
			string command;
			command="ALTER TABLE mobilenotification ADD AppTarget tinyint NOT NULL";
			Db.NonQ(command);
		}//End of 24_1_24() method

		private static void To24_1_27() {
			string command="ALTER TABLE claimattach ADD ImageReferenceId int NOT NULL";
			Db.NonQ(command);
		}//End of 24_1_27()

		private static void To24_1_29() {
			//Start B53978
			AddAlertTypesToOdAllTypesCategory(alertTypeNum:39);//AlertType.SignatureCleared
			//End B53978
		}//End of 24_1_29()

		private static void To24_1_31() {
			//Start B53968
			string command="UPDATE preference SET ValueString='time.nist.gov' WHERE ValueString='nist-time-server.eoni.com' AND PrefName='NistTimeServerUrl'";
			Db.NonQ(command);
			//End B53968
		}//End of 24_1_31()

		private static void To24_1_33() {
			//Start B54068
			string command="UPDATE patient SET SecurityHash=''";
			Db.NonQ(command);
			//End B54068
		}//End of 24_1_33()

		private static void To24_1_36() {
			//Start S54250
			string command="ALTER TABLE userod ADD BadgeId int NOT NULL";
			Db.NonQ(command);
			//End S54250
		}//End of 24_1_36()

		private static void To24_1_37() {
			//Start S54250
			string command="INSERT INTO preference(PrefName,ValueString) VALUES('SecurityBadgesRequirePassword','1')";
			Db.NonQ(command);
			command="ALTER TABLE userod MODIFY BadgeId VARCHAR(255) NOT NULL";
			Db.NonQ(command);
			//End S54250
		}//End of 24_1_37()

		private static void To24_1_60() {
			//Start B55522
			string command="SELECT ProgramNum FROM program WHERE ProgName='XDR'";
			long programNum=Db.GetLong(command);
			command="SELECT PropertyDesc,ComputerName,ClinicNum,IsMasked,IsHighSecurity,MIN(ProgramPropertyNum) ProgPropMin,COUNT(*) CountDup "
				+"FROM programproperty "
				+"WHERE ProgramNum="+POut.Long(programNum)+" "
				+"GROUP BY PropertyDesc,ComputerName,ClinicNum,IsMasked,IsHighSecurity "
				+"HAVING COUNT(*)>1";
			DataTable table=Db.GetTable(command);
			for(int i = 0;i<table.Rows.Count;i++) {
				DataRow row=table.Rows[i];
				command="DELETE FROM programproperty "
					+"WHERE ProgramNum="+POut.Long(programNum)+" "
					+"AND PropertyDesc='"+POut.String(PIn.String(row["PropertyDesc"].ToString()))+"' "
					+"AND ComputerName='"+POut.String(PIn.String(row["ComputerName"].ToString()))+"' "
					+"AND ClinicNum="+POut.Long(PIn.Long(row["ClinicNum"].ToString()))+" "
					+"AND IsMasked="+POut.Bool(PIn.Bool(row["IsMasked"].ToString()))+" "
					+"AND IsHighSecurity="+POut.Bool(PIn.Bool(row["IsHighSecurity"].ToString()))+" "
					+"AND ProgramPropertyNum!="+POut.Long(PIn.Long(row["ProgPropMin"].ToString()));
				Db.NonQ(command);
			}//End B55522
		}//End of 24_1_60()

		private static void To24_1_76() {
			string command="UPDATE preference SET ValueString=CONCAT(ValueString,',NewCrop') WHERE PrefName='ProgramLinksDisabledForWeb'";
			Db.NonQ(command);
			//This query does not need to be added to 24.2 because all cloud users were on 24.1 or earlier on the date that this was released.
		}//End of 24_1_76()

		private static void To24_1_80() {
			//Start 46289
			string command="SELECT ProgramNum FROM program WHERE ProgName='eRx'";
			long programNum=Db.GetLong(command);
			command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiVersion'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) VALUES("+POut.Long(programNum)+",'DoseSpotApiVersion','1')";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiMigrationRequested'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) VALUES("+POut.Long(programNum)+",'DoseSpotApiMigrationRequested','0')";
				Db.NonQ(command);
			}
			//End 46298
		}//End of 24_1_80

		private static void To24_1_85() {
			//Start B57668
			string command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiVersion'";
			if(Db.GetInt(command)>0) {
				command="UPDATE programproperty SET PropertyValue='1' WHERE PropertyDesc='DoseSpotApiVersion'";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiMigrationRequested'";
			if(Db.GetInt(command)>0) {
				command="UPDATE programproperty SET PropertyValue='0' WHERE PropertyDesc='DoseSpotApiMigrationRequested'";
				Db.NonQ(command);
			}
			//End B57668
		}//End of 24_1_85

		private static void To24_2_1() {
			string command;
			DataTable table;
			command="ALTER TABLE apptview ADD OnlyScheduledProvDays tinyint NOT NULL";
			Db.NonQ(command);
			command="UPDATE clearinghouse SET Description='DentalXChange ClaimConnect' WHERE Description='ClaimConnect'";
			Db.NonQ(command);
			//Start F45614
			command="DROP TABLE IF EXISTS languagepat";
			Db.NonQ(command);
			command=@"CREATE TABLE languagepat (
				LanguagePatNum bigint NOT NULL auto_increment PRIMARY KEY,
				PrefName varchar(255) NOT NULL,
				Language varchar(255) NOT NULL,
				Translation text NOT NULL,
				INDEX(PrefName)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//End F45614
			//Start F40926
			command="ALTER TABLE repeatcharge ADD Frequency tinyint(4) NOT NULL DEFAULT '0'";
			Db.NonQ(command);
			//End F40926
			//Start E52308
			//Add permission to everyone------------------------------------------------------
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			long groupNum;
			for(int i=0;i<table.Rows.Count;i++) {
				 groupNum=PIn.Long(table.Rows[i]["UserGroupNum"].ToString());
				 command="INSERT INTO grouppermission (UserGroupNum,PermType) "
						+"VALUES("+POut.Long(groupNum)+",252)";//252 is EnumPermType.ClaimProcFeeBilledToInsEdit
				 Db.NonQ(command);
			}
			//original instructions for this alert were wrong after a UI refactor. Update the instructions if the alert still exists
			//AlertRead rows associated with this AlertItem were not updated because this is an obscure and minor issue
			command="SELECT * FROM alertitem WHERE Type=5 AND Description LIKE '%The Web Sched New Pat feature now asks patients questions to verify patient information.%'";
			table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				long alertItemNum=PIn.Long(table.Rows[0]["AlertItemNum"].ToString());
				command=@"UPDATE alertitem SET Description=
					'The Web Sched features now include options to validate patient contact information before allowing scheduling. 
					\nTo customize this behavior, go to eServices -> Web Sched -> Advanced and select between Text and Email Authentication options or offer both.' 
					WHERE AlertItemNum="+POut.Long(alertItemNum);
				Db.NonQ(command);
			}
			//End E52308
			command="ALTER TABLE program CHANGE Path Path TEXT NOT NULL DEFAULT '', " +
				"CHANGE CommandLine CommandLine TEXT NOT NULL DEFAULT ''";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS eform";
			Db.NonQ(command);
			command=@"CREATE TABLE eform (
				EFormNum bigint NOT NULL auto_increment PRIMARY KEY,
				FormType tinyint NOT NULL,
				PatNum bigint NOT NULL,
				DateTimeShown datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				Description varchar(255) NOT NULL,
				DateTEdited datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				INDEX(PatNum)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS eformdef";
			Db.NonQ(command);
			command=@"CREATE TABLE eformdef (
				EFormDefNum bigint NOT NULL auto_increment PRIMARY KEY,
				FormType tinyint NOT NULL,
				Description varchar(255) NOT NULL,
				DateTCreated datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
				IsInternalHidden tinyint NOT NULL
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS eformfield";
			Db.NonQ(command);
			command=@"CREATE TABLE eformfield (
				EFormFieldNum bigint NOT NULL auto_increment PRIMARY KEY,
				EFormNum bigint NOT NULL,
				PatNum bigint NOT NULL,
				FieldType tinyint NOT NULL,
				DbLink varchar(255) NOT NULL,
				ValueLabel text NOT NULL,
				ValueString text NOT NULL,
				ItemOrder int NOT NULL,
				PickListVis varchar(255) NOT NULL,
				PickListDb varchar(255) NOT NULL,
				IsHorizStacking tinyint NOT NULL,
				IsTextWrap tinyint NOT NULL,
				Width int NOT NULL,
				FontScale int NOT NULL,
				IsRequired tinyint NOT NULL,
				ConditionalParent varchar(255) NOT NULL,
				ConditionalValue varchar(255) NOT NULL,
				INDEX(EFormNum),
				INDEX(PatNum)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS eformfielddef";
			Db.NonQ(command);
			command=@"CREATE TABLE eformfielddef (
				EFormFieldDefNum bigint NOT NULL auto_increment PRIMARY KEY,
				EFormDefNum bigint NOT NULL,
				FieldType tinyint NOT NULL,
				DbLink varchar(255) NOT NULL,
				ValueLabel text NOT NULL,
				ItemOrder int NOT NULL,
				PickListVis varchar(255) NOT NULL,
				PickListDb varchar(255) NOT NULL,
				IsHorizStacking tinyint NOT NULL,
				IsTextWrap tinyint NOT NULL,
				Width int NOT NULL,
				FontScale int NOT NULL,
				IsRequired tinyint NOT NULL,
				ConditionalParent varchar(255) NOT NULL,
				ConditionalValue varchar(255) NOT NULL,
				INDEX(EFormDefNum)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//Start I53268
			LargeTableHelper.AlterTable("appointment","AptNum",new ColNameAndDef("ItemOrderPlanned","int NOT NULL"));
			LargeTableHelper.AlterTable("histappointment","HistApptNum",new ColNameAndDef("ItemOrderPlanned","int NOT NULL"));
			command=@"UPDATE appointment
					INNER JOIN plannedappt ON appointment.AptNum = plannedappt.AptNum
					SET appointment.ItemOrderPlanned = plannedappt.ItemOrder";
			Db.NonQ(command);
			command="DROP TABLE IF EXISTS plannedappt";
			Db.NonQ(command);
			//End I53268
			command="SELECT ProgramNum FROM program WHERE ProgName='Midway'";
			long programNumMidWay=Db.GetLong(command);
			if(programNumMidWay>0) {
				command="DELETE FROM programproperty WHERE ProgramNum="+programNumMidWay;
				Db.NonQ(command);
				command="DELETE FROM toolbutitem WHERE ProgramNum="+programNumMidWay;
				Db.NonQ(command);
				command="DELETE FROM program WHERE ProgramNum="+programNumMidWay;
				Db.NonQ(command);
			}
			command="SELECT ProgramNum FROM program WHERE ProgName='EwooEZDent'";
			long programNumEwoo=Db.GetLong(command);
			if(programNumEwoo>0) {
				command="UPDATE program SET ProgName='EzDenti', ProgDesc='EzDenti' WHERE ProgramNum="+POut.Long(programNumEwoo);
				Db.NonQ(command);
				command="UPDATE toolbutitem SET ButtonText='EzDenti' WHERE ProgramNum="+POut.Long(programNumEwoo);
				Db.NonQ(command);
			}
			//Insert Ez3D-i bridge-------------------------------------------------------------------------
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
				+") VALUES("
				+"'Ez3Di', "
				+"'Ez3Di', "
				+"'0', "
				+"'"+POut.String(@"C:\EasyDent4\Edp4\EasyDent4.exe")+"', "
				+"'', "
				+"'')";
			long programNum=Db.NonQ(command,getInsertID:true);
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
				+") VALUES("
				+"'"+POut.Long(programNum)+"', "
				+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
				+"'0')";
			Db.NonQ(command);
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
				+"VALUES ("
				+"'"+POut.Long(programNum)+"', "
				+"'2', "
				+"'Ez3Di')";
			Db.NonQ(command);
			//end Ez3D-i bridge
			//Start F50716
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ClaimMedReceivedPromptForPrimaryClaim','1')";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ClaimMedReceivedForcePrimaryStatus','0')";
			Db.NonQ(command);
			//End F50716
			command="ALTER TABLE eroutingaction ADD ForeignKeyType tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eroutingaction ADD ForeignKey bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eroutingactiondef ADD ForeignKeyType tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eroutingactiondef ADD ForeignKey bigint NOT NULL";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('AddressVerifyWithUSPS','1')";
			Db.NonQ(command);
			//Start I3821
			//Add AllergyMerge permission to everyone------------------------------------------------------
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				groupNum=PIn.Long(table.Rows[i]["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
					+"VALUES("+POut.Long(groupNum)+",253)";//253 is EnumPermType.AllergyMerge
				Db.NonQ(command);
			}
			//End I3821
			command="UPDATE preference SET ValueString=0 WHERE ValueString=1 AND PrefName='RxSendNewToQueue'";
			Db.NonQ(command);
			//Insert BolaAI bridge----------------------------------------------------------------- 
			command = "INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
				 + ") VALUES("
				 + "'BolaAI', "
				 + "'Bola AI from https://bola.ai', "
				 + "'0', "//Disabled 
				 + "'"+POut.String(@"%USERPROFILE%\AppData\Local\bolavoiceassistant\Bola Voice Assistant.exe")+"', "
				 + "'', "//No cmd line args.
				 + "'Shows in the Perio Chart window when enabled.')";
			Db.NonQ(command);
			command ="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue,ClinicNum"
				+") VALUES("
				+"(SELECT ProgramNum FROM program WHERE progName='BolaAI'), "
				+"'Disable Advertising', "
				+"'0', "
				+"'0')";
			Db.NonQ(command);
			//end BolaAI bridge
			command="SELECT ProgramNum FROM program WHERE ProgName='Appriss'";
			programNum=Db.GetLong(command);
			if(programNum > 0) {
				command=$"UPDATE program SET ProgName='Bamboo', ProgDesc='Bamboo' WHERE ProgramNum={POut.Long(programNum)}";
				Db.NonQ(command);
				command=$"UPDATE programproperty SET PropertyDesc=REPLACE(PropertyDesc,'Appriss','Bamboo') WHERE ProgramNum={POut.Long(programNum)}";
				Db.NonQ(command);
				command=$"UPDATE toolbutitem SET ButtonText=REPLACE(ButtonText,'Appriss','Bamboo') WHERE ProgramNum={POut.Long(programNum)}";
				Db.NonQ(command);
			}
			command="SELECT ProgramNum, Enabled FROM program WHERE ProgName='TrophyEnhanced'";
			DataTable dataTable=Db.GetTable(command);
			if(dataTable.Rows.Count>0) {
				programNum=PIn.Long(dataTable.Rows[0]["ProgramNum"].ToString());
				bool isEnabled=PIn.Bool(dataTable.Rows[0]["Enabled"].ToString());
				int programPropertyValue=1;
				if(isEnabled) {//If the program is already enabled, don't override the old way numbering
					programPropertyValue=0;
				}
				if(programNum > 0) {
					command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
						 +") VALUES(" 
						 +"'"+POut.Long(programNum)+"', " 
						 +"'Enter 0 to use the 5th and 6th digits, or 1 to use the last 2 digits of the Patient Number for folder creation in Numbered Mode', "
						 +"'"+programPropertyValue.ToString()+"')";
					Db.NonQ(command);
				}
			}
			command="SELECT ProgramNum FROM program WHERE ProgName='XVWeb'";
			programNum=Db.GetLong(command);
			if(programNum > 0) {
				command="DELETE FROM programproperty WHERE ProgramNum="+POut.Long(programNum)+" "
					+"AND PropertyDesc IN ('Image Category','Save Images (yes or no)','Image Quality')";
				Db.NonQ(command);
			}
			//Start F52172
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ImagingOrderDescending','0')";
			Db.NonQ(command);
			//End F52172
			if(!IndexExists("histappointment","HistDateTStamp")) {
				command="ALTER TABLE histappointment ADD INDEX (HistDateTStamp)";
				Db.NonQ(command);
			}
			//Start S47726
			command="ALTER TABLE imagedraw ADD PearlLayer tinyint NOT NULL";
			Db.NonQ(command);
			//End S47726
			command="ALTER TABLE supplyorderitem ADD DateReceived date NOT NULL DEFAULT '0001-01-01'";
			Db.NonQ(command);
		}//End of 24_2_1() method

		private static void To24_2_2() {
			string command="SELECT COUNT(*) FROM preference WHERE PrefName='OCRClinicsSignedUp'";
			long count=Db.GetLong(command);
			if(count==0) {//missing
				command="INSERT INTO preference (PrefName,ValueString) VALUES('OCRClinicsSignedUp','')";
				Db.NonQ(command);
			}
			//Start S54250
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ChildDaycare','0')";
			Db.NonQ(command);
			//End S54250
		}//End of 24_2_2() method

		private static void To24_2_3() {
			//Start 54624
			string command="SELECT DISTINCT UserGroupNum FROM grouppermission WHERE PermType=24";//SecurityAdmin - Pulling all security admins
			DataTable table=Db.GetTable(command);
			long groupNum;
			for(int i=0;i<table.Rows.Count;i++) {
				groupNum=PIn.Long(table.Rows[i]["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
					+"VALUES("+POut.Long(groupNum)+",255)";//BadgeIdEdit - Setting permission for security admins
				Db.NonQ(command);
			}
			//End 54624
		}//End of 24_2_3() method

		private static void To24_2_4() {
			//Start 50988
			string command="ALTER TABLE printer ADD FileExtension varchar(255) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE printer ADD IsVirtualPrinter tinyint NOT NULL";
			Db.NonQ(command);
			//End 50988
		}//End of 24_2_4() method

		private static void To24_2_6() {
			//Start B54856
			string command="SELECT ProgramNum FROM program WHERE ProgName='BolaAI'";
			long programNum=Db.GetLong(command);
			command="UPDATE program "+
				"SET Path='"+POut.String(@"%USERPROFILE%\AppData\Local\bolavoiceassistant\Bola Voice Assistant.exe")+"' "+
				"WHERE ProgramNum="+POut.Long(programNum)+" "+
				"AND Path='"+POut.String(@"%USERPROFILE%AppDataLocalolavoiceassistantBola Voice Assistant.exe")+"' ";//This is the incorrect value that I copied over from the database.
			Db.NonQ(command);
			//End B54856
		}//End of 24_2_6()

		private static void To24_2_12() {
			string command="UPDATE procedurecode SET TreatArea=0 " +//0 = TreatmentArea.None
				"WHERE TreatArea=3 AND ProcCode IN ('D0120','D0140','D0145','D0150','D0270','D0272','D0273','D0274')";//3 = TreatmentArea.Mouth
			Db.NonQ(command);
		}//End of 24_2_12()

		private static void To24_2_18() {
			string command="ALTER TABLE eformfield ADD LabelAlign tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfielddef ADD LabelAlign tinyint NOT NULL";
			Db.NonQ(command);
		}//End of 24_2_18()

		private static void To24_2_20() {
			//Start B55522
			string command="SELECT ProgramNum FROM program WHERE ProgName='XDR'";
			long programNum=Db.GetLong(command);
			command="SELECT PropertyDesc,ComputerName,ClinicNum,IsMasked,IsHighSecurity,MIN(ProgramPropertyNum) ProgPropMin,COUNT(*) CountDup "
				+"FROM programproperty "
				+"WHERE ProgramNum="+POut.Long(programNum)+" "
				+"GROUP BY PropertyDesc,ComputerName,ClinicNum,IsMasked,IsHighSecurity "
				+"HAVING COUNT(*)>1";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				DataRow row=table.Rows[i];
				command="DELETE FROM programproperty "
					+"WHERE ProgramNum="+POut.Long(programNum)+" "
					+"AND PropertyDesc='"+POut.String(PIn.String(row["PropertyDesc"].ToString()))+"' "
					+"AND ComputerName='"+POut.String(PIn.String(row["ComputerName"].ToString()))+"' "
					+"AND ClinicNum="+POut.Long(PIn.Long(row["ClinicNum"].ToString()))+" "
					+"AND IsMasked="+POut.Bool(PIn.Bool(row["IsMasked"].ToString()))+" "
					+"AND IsHighSecurity="+POut.Bool(PIn.Bool(row["IsHighSecurity"].ToString()))+" "
					+"AND ProgramPropertyNum!="+POut.Long(PIn.Long(row["ProgPropMin"].ToString()));
				Db.NonQ(command);
			}//End B55522
		}//End of 24_2_20()

		private static void To24_2_21() {
			//Start E54726
			//For the new pref 'EmailStatementsSecure': if we find that a Preference row exists for the older pref, 'EmailDefaultSendPlatform', then insert the new pref with a default value matching that of the older pref. Otherwise, hard code the new pref's value to 'Secure' (which is the default in OD).
			string command="SELECT ValueString FROM preference WHERE PrefName='EmailDefaultSendPlatform'";
			string valueString=Db.GetScalar(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EmailStatementsSecure','"+(string.IsNullOrWhiteSpace(valueString) ? "Secure" : POut.String(valueString))+"')";
			Db.NonQ(command);
			//Similar to above, if we find that a ClinicPref row exists for the older clinicpref, 'EmailDefaultSendPlatform', for any existing clinics, then insert the new clinicpref with a default value matching that of the older clinicpref for each clinic. Otherwise, do nothing.
			command="SELECT * FROM clinicpref WHERE PrefName='EmailDefaultSendPlatform'";
			DataTable table=Db.GetTable(command);
			long clinicNum;
			for(int i=0;i<table.Rows.Count;i++){
				clinicNum=PIn.Long(table.Rows[i]["ClinicNum"].ToString());
				command="INSERT INTO clinicpref(PrefName,ClinicNum,ValueString) VALUES('EmailStatementsSecure',"+POut.Long(clinicNum)+",'"+POut.String(table.Rows[i]["ValueString"].ToString())+"')";
				Db.NonQ(command);
			}
			//End E54726
		}//End of 24_2_21()
		
		private static void To24_2_22() {
			//Start I52398
			string command="ALTER TABLE eroutingactiondef ADD LabelOverride VARCHAR(255) NOT NULL DEFAULT ''";
			Db.NonQ(command);
			command="ALTER TABLE eroutingaction ADD LabelOverride VARCHAR(255) NOT NULL DEFAULT ''";
			Db.NonQ(command);
			//End I52398
		}//End of 24_2_22

		private static void To24_2_23() {
			//Start S53342
			string command="INSERT INTO preference(PrefName,ValueString) VALUES('EformsSpaceBelowEachField','10')";
			Db.NonQ(command);
			command="ALTER TABLE eformfield ADD SpaceBelow INT NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfield ADD ReportableName VARCHAR(255) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfield ADD IsLocked tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfielddef ADD SpaceBelow INT NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfielddef ADD ReportableName VARCHAR(255) NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfielddef ADD IsLocked tinyint NOT NULL";
			Db.NonQ(command);
			command="UPDATE eformfield SET SpaceBelow=-1";
			Db.NonQ(command);
			command="UPDATE eformfielddef SET SpaceBelow=-1";
			Db.NonQ(command);
			//End S53342
		}//End of 24_2_23

		private static void To24_2_26() {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("US")) {
				//Bug fix for B55924
				string command="UPDATE preference SET ValueString=0 WHERE ValueString=1 AND PrefName='AddressVerifyWithUSPS'";
				Db.NonQ(command);
			}
		}

		private static void To24_2_27() {
			//Start S53342
			string command="ALTER TABLE languagepat ADD EFormFieldDefNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE languagepat ADD INDEX (EFormFieldDefNum)";
			Db.NonQ(command);
			//End S53342
		}//End of 24_2_27

		private static void To24_2_28() {
			//Start S53342
			string command="ALTER TABLE eform ADD MaxWidth int NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformdef ADD MaxWidth int NOT NULL";
			Db.NonQ(command);
			command="UPDATE eformdef SET MaxWidth =450";
			Db.NonQ(command);
			command="UPDATE eform SET MaxWidth =450";
			Db.NonQ(command);
			//End S53342
			//Start I53050
			command="ALTER TABLE eformfield ADD Border tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfield ADD IsWidthPercentage tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfield ADD MinWidth int NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfielddef ADD Border tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfielddef ADD IsWidthPercentage tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfielddef ADD MinWidth int NOT NULL";
			Db.NonQ(command);
			//End I53050
			//Start I53372
			command="INSERT INTO preference (PrefName,ValueString) VALUES('TextOptOutSendNotification','0')";
			Db.NonQ(command);
			//End I53372
		}//End of 24_2_28

		private static void To24_2_39() {
			//Start 46289
			string command="SELECT ProgramNum FROM program WHERE ProgName='eRx'";
			long programNum=Db.GetLong(command);
			command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiVersion'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) VALUES("+POut.Long(programNum)+",'DoseSpotApiVersion','1')";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiMigrationRequested'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) VALUES("+POut.Long(programNum)+",'DoseSpotApiMigrationRequested','0')";
				Db.NonQ(command);
			}
			//End 46298
		}//End of 24_2_39

		private static void To24_2_46() {
			//Start B57668
			string command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiVersion'";
			if(Db.GetInt(command)>0) {
				command="UPDATE programproperty SET PropertyValue='1' WHERE PropertyDesc='DoseSpotApiVersion'";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiMigrationRequested'";
			if(Db.GetInt(command)>0) {
				command="UPDATE programproperty SET PropertyValue='0' WHERE PropertyDesc='DoseSpotApiMigrationRequested'";
				Db.NonQ(command);
			}
			//End B57668
		}//End of 24_2_46

		private static void To24_2_48() {
			string command="INSERT INTO preference (PrefName,ValueString) VALUES ('CareCreditBatchProcessDateTime',"+POut.DateT(DateTime.MinValue,true)+")";
			Db.NonQ(command);
			command="INSERT INTO preference (PrefName,ValueString) VALUES ('CareCreditBatchPullbackDateTime',"+POut.DateT(DateTime.MinValue,true)+")";
			Db.NonQ(command);
		}//End of 24_2_48

		private static void To24_2_47() {
			ObsolesceCDTCodesFor2025();
		}

		private static void To24_2_50() {
			string command;
			//Start I57518
			try {
				if(!IndexExists("appointment","DateTStamp")) {
					command="ALTER TABLE appointment ADD INDEX (DateTStamp)";
					Db.NonQ(command);
				}
			}
			catch(Exception ex) {
				ex.DoNothing(); //Only an index. (Exception ex) required to catch thrown exception
			}
			//End I57518
		}//End of 24_2_50

		private static void To24_2_51() {
			string command;
			//Start I56324
			command="SELECT COUNT(*) FROM preference WHERE PrefName='PayPlanItemDateShowProc'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('PayPlanItemDateShowProc','0')";
				Db.NonQ(command);
			}
			//End I56324
		}//End of 24_2_51

		private static void To24_3_1() {
			string command;
			DataTable table;
			//Start S47726 PearlAI PearlRequest table
			command="DROP TABLE IF EXISTS pearlrequest";
			Db.NonQ(command);
			command=@"CREATE TABLE pearlrequest (
				PearlRequestNum bigint NOT NULL auto_increment PRIMARY KEY,
				RequestId varchar(255) NOT NULL,
				DocNum bigint NOT NULL,
				RequestStatus tinyint NOT NULL,
				DateTSent date NOT NULL DEFAULT '0001-01-01',
				DateTChecked date NOT NULL DEFAULT '0001-01-01',
				INDEX(RequestStatus)
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//End S47726 PearlAI PearlRequest table
			// Start S47726 PearlAI bridge
			command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
							+") VALUES("
							+"'Pearl', "
							+"'Pearl AI', "
							+"'0', "
							+"'', "
							+"'', "
							+"'For \"Show Pearl annotations by default\", enter true or false. For \"Image categories for automatic upload\", enter a list of Image category names separated by commas.')"; 
			long programNum=Db.NonQ(command,true); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
							+") VALUES(" 
							+"'"+POut.Long(programNum)+"', " 
							+"'Text font size', "
							+"'18')"; 
			Db.NonQ(command); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
							+") VALUES(" 
							+"'"+POut.Long(programNum)+"', " 
							+"'Client ID', "
							+"'')"; 
			Db.NonQ(command);  
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
							+") VALUES(" 
							+"'"+POut.Long(programNum)+"', " 
							+"'Client Secret', "
							+"'')"; 
			Db.NonQ(command);  
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
							+") VALUES(" 
							+"'"+POut.Long(programNum)+"', " 
							+"'Organization ID', "
							+"'')"; 
			Db.NonQ(command);
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
							+") VALUES(" 
							+"'"+POut.Long(programNum)+"', " 
							+"'Office ID', "
							+"'')"; 
			Db.NonQ(command); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue" 
							+") VALUES(" 
							+"'"+POut.Long(programNum)+"', " 
							+"'Show Pearl annotations by default', "
							+"'true')"; 
			Db.NonQ(command); 
			command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
							+") VALUES("
							+"'"+POut.Long(programNum)+"', "
							+"'Image categories for automatic upload', "
							+"'BWs,FMXs,Panos')";
			Db.NonQ(command); 
			command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) " 
							+"VALUES (" 
							+"'"+POut.Long(programNum)+"', " 
							+"'3', "//ToolBarsAvail.ImagesModule 
							+"'Pearl')"; 
			Db.NonQ(command);			
			//End S47726 PearlAI bridge
			command="ALTER TABLE patient MODIFY ChartNumber VARCHAR(100) NOT NULL DEFAULT ''";
			Db.NonQ(command);
			command="ALTER TABLE grouppermission MODIFY PermType smallint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE securitylog MODIFY PermType smallint NOT NULL";
			Db.NonQ(command);
			//Start I55480
			command="INSERT INTO preference(PrefName,ValueString) VALUES('PatientSelectWindowShowGetAll','1')";
			Db.NonQ(command);
			//End I55480
			//Start F54684
			command="INSERT INTO preference (PrefName,ValueString) VALUES('ApptModuleProviderPrompt','0')";//Default to ApptModuleProviderPrompt.PromptDefaultYes
			Db.NonQ(command);
			//End F54684
			//Start F54682
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			long groupNum;
			for(int i=0;i<table.Rows.Count;i++) {
				groupNum=PIn.Long(table.Rows[i]["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
			//Start B57168
			//		+"VALUES("+POut.Long(groupNum)+",256)";//daycare permission only used at HQ
					+"VALUES("+POut.Long(groupNum)+",257)";//Perio Chart Copy
			//End B57168
				Db.NonQ(command);
			}
			//End F54862
			//Start F55160
			command="INSERT INTO preference(PrefName,ValueString) VALUES('ClaimEditRequireNoMissingData','0')";
			Db.NonQ(command);
			//End F55160
			//Start I48590
			command="INSERT INTO preference(PrefName,ValueString) VALUES('UserQueryDefaultRaw','0')";
			Db.NonQ(command);
			command="ALTER TABLE userquery ADD DefaultFormatRaw tinyint NOT NULL";//default is 0, meaning existing userqueries will be human readable, same as before
			Db.NonQ(command);
			//End I48590
			//Start S56756
			command="DROP TABLE IF EXISTS eformimportrule";
			Db.NonQ(command);
			command=@"CREATE TABLE eformimportrule (
				EFormImportRuleNum bigint NOT NULL auto_increment PRIMARY KEY,
				FieldName varchar(255) NOT NULL,
				Situation tinyint NOT NULL,
				Action tinyint NOT NULL
				) DEFAULT CHARSET=utf8";
			Db.NonQ(command);
			//End S56756
			//Begin I51748
			command="ALTER TABLE msgtopaysent ADD ApptNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE msgtopaysent ADD INDEX (ApptNum)";
			Db.NonQ(command);
			command="ALTER TABLE msgtopaysent ADD ApptDateTime datetime NOT NULL DEFAULT '0001-01-01 00:00:00'";
			Db.NonQ(command);
			command="ALTER TABLE msgtopaysent ADD TSPrior bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE msgtopaysent ADD INDEX (TSPrior)";
			Db.NonQ(command);
			command="ALTER TABLE msgtopaysent ADD StatementNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE msgtopaysent ADD INDEX (StatementNum)";
			Db.NonQ(command);
			//Index is used for an S-class method MsgToPaySents.GetAllUnsent()
			command="ALTER TABLE msgtopaysent ADD INDEX(Source,DateTimeSent,SendStatus)";
			Db.NonQ(command);
			//End I51748
			//Start S53050
			command="ALTER TABLE eclipboardsheetdef ADD EFormDefNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eclipboardsheetdef ADD INDEX (EFormDefNum)";
			Db.NonQ(command);
			command="ALTER TABLE eform ADD EFormDefNum bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eform ADD INDEX (EFormDefNum)";
			Db.NonQ(command);
			command="ALTER TABLE eform ADD Status tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eform ADD RevID int NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformdef ADD RevID int NOT NULL";
			Db.NonQ(command);	
			//End S53050
			//Start F55162 Prevent attaching multiple tasks to an appointment
			command="INSERT INTO preference(PrefName,ValueString) VALUES('TasksForApptAllowMultiple','1')";
			Db.NonQ(command);
			//End F55162
			//Start I54172
			command="INSERT INTO preference(PrefName,ValueString) VALUES ('BackupIndexesDisabled','1')";
			Db.NonQ(command);
			//End I54172
		}//End of 24_3_1() method

		private static void To24_3_3() {
			string command;
			//Start S53050
			command="ALTER TABLE eform ADD ShowLabelsBold tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformdef ADD ShowLabelsBold tinyint NOT NULL";
			Db.NonQ(command);
			//End S53050
			//Start I56704
			command="INSERT INTO preference(PrefName,ValueString) VALUES('InsOutOfNetworkBlankLikeZero','0')";
			Db.NonQ(command);
			//End I56704
			//Start S53050
			command="UPDATE preference SET ValueString='20' WHERE PrefName='EformsSpaceBelowEachField'";
			Db.NonQ(command);
			command="ALTER TABLE eformfield ADD WidthLabel int NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfielddef ADD WidthLabel int NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfield ADD SpaceToRight int NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformfielddef ADD SpaceToRight int NOT NULL";
			Db.NonQ(command);
			command="UPDATE eformfield SET SpaceToRight = -1";
			Db.NonQ(command);
			command="UPDATE eformfielddef SET SpaceToRight = -1";
			Db.NonQ(command);
			command="INSERT INTO preference(PrefName,ValueString) VALUES('EformsSpaceToRightEachField','10')";
			Db.NonQ(command);
			command="ALTER TABLE eform ADD SpaceBelowEachField int NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformdef ADD SpaceBelowEachField int NOT NULL";
			Db.NonQ(command);
			command="UPDATE eform SET SpaceBelowEachField = -1";
			Db.NonQ(command);
			command="UPDATE eformdef SET SpaceBelowEachField = -1";
			Db.NonQ(command);
			command="ALTER TABLE eform ADD SpaceToRightEachField int NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformdef ADD SpaceToRightEachField int NOT NULL";
			Db.NonQ(command);
			command="UPDATE eform SET SpaceToRightEachField = -1";
			Db.NonQ(command);
			command="UPDATE eformdef SET SpaceToRightEachField = -1";
			Db.NonQ(command);
			//End S53050
		}//End of 24_3_3() method

		private static void To24_3_4() {
			//Start 46289
			string command="SELECT ProgramNum FROM program WHERE ProgName='eRx'";
			long programNum=Db.GetLong(command);
			command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiVersion'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) VALUES("+POut.Long(programNum)+",'DoseSpotApiVersion','1')";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiMigrationRequested'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) VALUES("+POut.Long(programNum)+",'DoseSpotApiMigrationRequested','0')";
				Db.NonQ(command);
			}
			//End 46298
		}

		private static void To24_3_7() {
			//Start S53050
			string command="ALTER TABLE eform ADD SaveImageCategory bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eformdef ADD SaveImageCategory bigint NOT NULL";
			Db.NonQ(command);
			//End S53050
		}

		private static void To24_3_10() {
			//Start S53050
			string command="UPDATE eformfield SET PickListVis=REPLACE(PickListVis, ',', '|')";
			Db.NonQ(command);
			command="UPDATE eformfield SET PickListDb=REPLACE(PickListDb, ',', '|')";
			Db.NonQ(command);
			command="UPDATE eformfielddef SET PickListVis=REPLACE(PickListVis, ',', '|')";
			Db.NonQ(command);
			command="UPDATE eformfielddef SET PickListDb=REPLACE(PickListDb, ',', '|')";
			Db.NonQ(command);
			//End S53050
			//Start I56390
			command="ALTER TABLE eclipboardimagecapturedef ADD Frequency tinyint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eclipboardimagecapturedef ADD ResubmitInterval bigint NOT NULL";
			Db.NonQ(command);
			command="ALTER TABLE eclipboardimagecapturedef ADD INDEX (ResubmitInterval)";
			Db.NonQ(command);
			command="ALTER TABLE eclipboardsheetdef ADD Frequency tinyint NOT NULL";
			Db.NonQ(command);
			//End I56390
		}

		private static void To24_3_12() {
			//Start B57668
			string command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiVersion'";
			if(Db.GetInt(command)>0) {
				command="UPDATE programproperty SET PropertyValue='1' WHERE PropertyDesc='DoseSpotApiVersion'";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='DoseSpotApiMigrationRequested'";
			if(Db.GetInt(command)>0) {
				command="UPDATE programproperty SET PropertyValue='0' WHERE PropertyDesc='DoseSpotApiMigrationRequested'";
				Db.NonQ(command);
			}
			//End B57668
		}//End of 24_3_12

		private static void To24_3_14() {
			ObsolesceCDTCodesFor2025();
			//Start S53342 eForms
			string command="SELECT DISTINCT UserGroupNum FROM grouppermission WHERE PermType=42";//SheetEdit
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				long userGroupNum=PIn.Long(table.Rows[i]["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
					+"VALUES("+POut.Long(userGroupNum)+",259)";//EFormEdit
				Db.NonQ(command);
			}
			command="SELECT DISTINCT UserGroupNum FROM grouppermission WHERE PermType=136";//SheetDelete
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				long userGroupNum=PIn.Long(table.Rows[i]["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
					+"VALUES("+POut.Long(userGroupNum)+",260)";//EFormDelete
				Db.NonQ(command);
			}
			//End S53342 eForms
		}

		private static void To24_3_15() {
			string command="SELECT COUNT(*) FROM preference WHERE PrefName='CareCreditBatchProcessDateTime'";
			long countRows=Db.GetLong(command);
			if(countRows==0) {
				command="INSERT INTO preference (PrefName,ValueString) VALUES ('CareCreditBatchProcessDateTime',"+POut.DateT(DateTime.MinValue,true)+")";
				Db.NonQ(command);
			}
			command="SELECT COUNT(*) FROM preference WHERE PrefName='CareCreditBatchPullbackDateTime'";
			countRows=Db.GetLong(command);
			if(countRows==0) {
				command="INSERT INTO preference (PrefName,ValueString) VALUES ('CareCreditBatchPullbackDateTime',"+POut.DateT(DateTime.MinValue,true)+")";
				Db.NonQ(command);
			}
		}//End of 24_3_15

		private static void To24_3_17() {
			string command;
			//Start I57518
			try {
				if(!IndexExists("appointment","DateTStamp")) {
					command="ALTER TABLE appointment ADD INDEX (DateTStamp)";
					Db.NonQ(command);
				}
			}
			catch(Exception ex) {
				ex.DoNothing(); //Only an index. (Exception ex) required to catch thrown exception
			}
			//End I57518
		}//End of 24_3_17

		private static void To24_3_18() {
			string command;
			//Start I56324
			command="SELECT COUNT(*) FROM preference WHERE PrefName='PayPlanItemDateShowProc'";
			if(Db.GetInt(command)==0) {
				command="INSERT INTO preference(PrefName,ValueString) VALUES('PayPlanItemDateShowProc','0')";
				Db.NonQ(command);
			}
			//End I56324
		}//End of 24_3_18

		private static void To24_3_20() {
			//Start I56390
			string command="UPDATE eclipboardsheetdef SET Frequency=2 WHERE ResubmitInterval>0";//2=Timespan
			Db.NonQ(command);
			command="UPDATE eclipboardsheetdef SET PrefillStatus=0 WHERE PrefillStatus=2";//0=New, 2=Once(deprecated)
			Db.NonQ(command);
			command="UPDATE eclipboardimagecapturedef SET Frequency=1 WHERE FrequencyDays=0";//1=EachTime - Don't need to do this for eclipboardsheetdef since 0 days previously indicated Once
			Db.NonQ(command);
			command="UPDATE eclipboardimagecapturedef SET Frequency=2 WHERE FrequencyDays>0";//2=Timespan
			Db.NonQ(command);
			//Without the following line, the db conversion might fail and result in db corruption if 
			command="UPDATE eclipboardimagecapturedef SET Frequency=0, ResubmitInterval=0, FrequencyDays=0 WHERE FrequencyDays>36500";//Frequency 0=Once
			Db.NonQ(command);
			command="UPDATE eclipboardimagecapturedef SET ResubmitInterval=864000000000*FrequencyDays WHERE FrequencyDays>0";
			Db.NonQ(command);
			command="UPDATE eclipboardimagecapturedef SET FrequencyDays=0";
			Db.NonQ(command);
			//End I56390
		}//End of 24_3_20

		private static void To24_3_24(){
			//Start I56394
			string command;
			DataTable table;
			command="SELECT DISTINCT UserGroupNum FROM grouppermission WHERE PermType=8";//Setup
			table=Db.GetTable(command);
			long groupNum;
			for(int i=0;i<table.Rows.Count;i++) {
				groupNum=PIn.Long(table.Rows[i]["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
					+"VALUES("+POut.Long(groupNum)+",262)";//ChartViewsEdit
				Db.NonQ(command);
			}
			//End I56394
			//Start I58122
			//Add SuperFamilyDisband to everyone------------------------------------------------------
			command="SELECT DISTINCT UserGroupNum FROM grouppermission";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				groupNum=PIn.Long(table.Rows[i]["UserGroupNum"].ToString());
				command="INSERT INTO grouppermission (UserGroupNum,PermType) "
					+"VALUES("+POut.Long(groupNum)+",263)";//SuperFamilyDisband
				Db.NonQ(command);
			}
			//End I58122
		}//End of 24_3_24

		private static void To24_3_27(){
			//Start B58322
			string command="SELECT COUNT(*) FROM program WHERE ProgName='Pearl'";
			long countRows=Db.GetLong(command);
			if(countRows>1) {
				command="SELECT MIN(ProgramNum) FROM program WHERE ProgName='Pearl'";
				long programNum=Db.GetLong(command);
				command="DELETE FROM toolbutitem WHERE ProgramNum="+POut.Long(programNum);
				Db.NonQ(command);
				command="DELETE FROM programproperty WHERE ProgramNum="+POut.Long(programNum);
				Db.NonQ(command);
				command="DELETE FROM program WHERE ProgramNum="+POut.Long(programNum);
				Db.NonQ(command);
			}
			//End B58322
		}//End of 

		private static void To24_3_30() {
			//Start B58344
			string command="INSERT INTO alertcategory(IsHQCategory,InternalName,Description) VALUES('1','Programs','Program Link Alerts')";
			long alertCatNum=Db.NonQ(command,true);
			command="INSERT INTO alertcategorylink(AlertCategoryNum,AlertType) VALUES("+POut.Long(alertCatNum)+",40)";//Pearl
			Db.NonQ(command);
			//End B58344
		}//End of 24_3_30

		private static void To24_3_31() {
			//Start E58652
			string command="SELECT ProgramNum FROM program WHERE ProgName='Pearl'";
			long programNum=Db.GetLong(command);
			command="SELECT COUNT(*) FROM programproperty WHERE PropertyDesc='Disable Advertising' AND ProgramNum="+POut.Long(programNum);
			if(Db.GetInt(command)==0) {
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue) VALUES("+POut.Long(programNum)+",'Disable Advertising','0')";
				Db.NonQ(command);
			}
			command="UPDATE program "+
				"SET ButtonImage='iVBORw0KGgoAAAANSUhEUgAAABYAAAAWCAYAAADEtGw7AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAA"
				+"ZGSURBVEhLrdR7TFvXAcfxHxg/wA8gGDB+XAzGr9jYvAwYsA2mmEfAgF8YGzDGBps8IDQjaaRMbE3bqUvaLdMyiUaN2iVL0620ndKGJG1KStu0jdYURVPTJU0zoS6pUq3Lkj"
				+"RdwHDmosph/++P77n/3PPROfccXRBCQLM5AF43QHfCMjoBuWkSgt4x1Dr2gKV3IVtZgqpaGwRDUxBf+Bc0HxN6zlyMWzqzkoRdBJkHViB+LcbJfDnGoi49SPrBXINZdjcEFT"
				+"6I1T6oO0cdwrq+o9l1gWNSy+gOfk1/rcjQJTa3+tPzBncj4+S3oN4nTzFPx65bzqzYBAcJ2C+scnJPxf6QdWFpt/DaMj8BC/x+mEKj2NgU/BVD7SY0tevrZLVjkan3rLLLev"
				+"/N1PvvKazDB3PadkF6/Jt87jlyC28sE/n80uHCU6tg/ZHwOCdWrjHnV49kXSHiBFxoc0PbHvWm6fxkQ63vgKy2H1rjCOo8O+Q8g/8qqGYis28Pa6fmkfX6g0f575IF6uzys+"
				+"lvLV2WzS+LWacIi3dy5VPO/Op03jUiSsC2wM4CYd3gDFvn/7K6f4xlC+2gu4eeTDXat2Vzynov5+qdL6T5nwD/ne/AmyPnhGeXXrd8uFzNfpvcFL5xNyw5fAXMV+8vcD4g04"
				+"L18PNHTmiM7p1/hqzjnqIpelpkCf99Q93QIqfc9yVd3XU/X9ftyd05g4JPiJJ9OvYN50zsn5J3lt9NPkOI8JWbLzbtfykl/U/f/oX1IXlO8MU6eNueQ3J16/hxFLaT1BLvhR"
				+"xj3yGFtf8X2TUDn8fhO2nyTWre5GugLpInmLNLq/qzsS31nxBb3gfklbSTyzdazt8389+M/TX5PXI8Z5FQCZhXGuDzygd+m17ee7ugeYtSZQnA2bcZus6xGcjsRNzgbTAcfA"
				+"mGK2RKOBf7TdkCQcmJ69DNLuYXnidHqt4nLRvfI49nz383yv/o68wELK6oBVXTvYmp7SEyW3Sr1DQIlqrryWRFN6Fp3KSozn1AYdqL6mfvomaJIP3s36DxbM1St0YiFQffVP"
				+"fcJHDNEZhfvQXqxXNr5trAiPqR43TRecWueZEptKBojk7Til1E0xbdLzYHn8uo6l6Ulu0VIHUvJPGJ4qHNKKi2h1JzGklJdI/Hdf0yHLPLaDp2FwXHPnoIo2MLGJYBFNSHg0"
				+"kqB4GqixSaAvvaHeOQWcIhqF1EVB/8vUjvowlVJfCGfwKBOXKUrXbeCv7smaz+I9Pwz9/GprklyGcX1sG6HaAbd0HcENnGKnavypuivQbrGPTmERFL6/ksRef5D03jIoVNoa"
				+"f7QkF09A+BWztyKNPQ84/KzskM4cZxiBxHkd07A5Zzeh3M7UJythMcvfd5TqXvc7V1LMVinwJljcwyip3fN/btLM9vCO+DykkMLUGvstULjrH3sYwKz708y0Qx5HsA6WYkU1"
				+"Ew88fXwSgEja6L3w7vy2Jr9FJ1+ygktYHBHz6JoiMSbrJHwJe5maxSz6dcw8ANVb2Pzte3B/g1I3cMnq2msj43yvzeROtggMagI6PM8ztuef9SbrX3Lcg77hc0DJ8sqgyDRT"
				+"WipduH1tD2NlaZfyXf1NejaQ55uaX9d7S9E0ZxdATUcCTRQ9hsRLK5HrxK3+MpKhdhFvdcRfy5sTnyVJd9EHWmRmzIYMBotQpzaga/Um2a2K1s27qdVdJ5lzI/JksS/Bp0yf"
				+"5ED+GpKTCnfg6teyLAUDlWZY+MeLnlAx8rm4b3hSPDsLfZ1naVmikWs3Weq9r2iZ/mN45Mx1f/Ra7GmQao4/9yfaIETGsMgPNIEEWWITVd4/xeag3/Mt0wcF5pG9k/HN+avb"
				+"X5R1hCxc/hUmHTlsOZBv9FaeP4oTx9FGBQAEeVKAEXdUxAGq/INUnLM4Xejt+OG0xd7215c+iZ1qEgShutazBbqJBwy/yfZVT1f8XUeR4oWwY9WTobUigNGFJ9ogS8QTgMDn"
				+"8ISu04Krt3D0PuIMlaOymwbnOz1RZAIY2fQx04lWZGdlXwOBQekm70XxRX+nKT8gRIoqj/KQHzOANIZfRBKhlFRa2HUdjocld3P9ohpJ5OSuLHV6sWAc020PNlkCgqhRWdkQ"
				+"FV3aSCy48AAjEgyosXf+fHEvD/P4L/ArNV0k7TEWDDAAAAAElFTkSuQmCC' "
				+"WHERE ProgramNum="+POut.Long(programNum);
			Db.NonQ(command);
			//End E58652
			//Start B58086
			command="UPDATE paysplit SET ProcNum=0, AdjNum=0 WHERE PayPlanNum > 0 AND PayPlanDebitType=2";//2=interest
			Db.NonQ(command);
			//End B58086
		}//End of 24_3_31

		private static void To24_3_35() {
			string command;
			//Start B58938
			command="ALTER TABLE appointment MODIFY ProcDescript TEXT";
			Db.NonQ(command);
			//End B58938
		}//End of 24_3_34() method

		private static void To24_3_37() {
			string command;
			//Start B59212
			try {
				if(!IndexExists("claim","DateService,SecDateEntry")) {
					command="ALTER TABLE claim ADD INDEX DateService(DateService,SecDateEntry)";
					Db.NonQ(command);
				}
			}
			catch(Exception ex) {
				ex.DoNothing(); //Only an index. (Exception ex) required to catch thrown exception
			}
			//End B59212
		}//End of 24_3_37() method

		private static void To24_3_45() {
			//Start B59848
			string command="UPDATE procedurecode SET TreatArea=2 "//2 = TreatmentArea.Tooth
				+"WHERE ProcCode IN ('D0220','D0230')";
			Db.NonQ(command);
			//End B59848
		}//End of 24_3_45() method

		private static void To24_3_49() {
			//Start B60028
			string command="UPDATE procedurecode SET PaintType=14 "//14 = PaintType.Veneer
				+"WHERE ProcCode IN ('D2960','D2961','D2962')";
			Db.NonQ(command);
			//End B60028
		}//End of 24_3_49() method

		private static void To24_3_53() {
			//Start B60246
			string command="UPDATE preference SET ValueString='5' WHERE PrefName='AppointmentWithoutProcsDefaultLength' AND ValueString IN ('0','1','2','3','4')";
			Db.NonQ(command);
			//End B60246
		}//End of 24_3_53() method

		private static void To24_3_63() {
			string command;
			//Start B60961
			command="SELECT ProgramNum FROM program WHERE ProgName='CADI'";
			long displayProgramNum=Db.GetLong(command);
			command ="UPDATE program SET Path = 'C:\\\\CADI\\\\CADI.exe'" +//Updating path format
				$@"WHERE ProgramNum={POut.Long(displayProgramNum)} AND Path='C:CADICADI.exe'";//If path format is still incorrect
			Db.NonQ(command);
			//End B60961
		}//End 24_3_63()
	}
}