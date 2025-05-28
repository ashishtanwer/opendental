using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.ReportingComplex;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public partial class FormRpPaySheet : FormODBase{
		private List<Clinic> _listClinics;
		private List<Provider> _listProviders;
		private List<Def> _listInsDefs;
		private List<Def> _listPayDefs;
		private List<Def> _listClaimPayGroupDefs;

		///<summary></summary>
		public FormRpPaySheet(){
			InitializeComponent();
			InitializeLayoutManager();
 			Lan.F(this);
		}

		private void FormPaymentSheet_Load(object sender, System.EventArgs e) {
			_listProviders=Providers.GetListReports();
			_listProviders.Insert(0,Providers.GetUnearnedProv());
			date1.SelectionStart=DateTime.Today;
			date2.SelectionStart=DateTime.Today;
			if(!Security.IsAuthorized(EnumPermType.ReportDailyAllProviders,true)) {
				//They either have permission or have a provider at this point.  If they don't have permission they must have a provider.
				_listProviders=_listProviders.FindAll(x => x.ProvNum==Security.CurUser.ProvNum);
				Provider prov=_listProviders.FirstOrDefault();
				if(prov!=null) {
					_listProviders.AddRange(Providers.GetWhere(x => x.FName==prov.FName && x.LName==prov.LName && x.ProvNum!=prov.ProvNum));
				}
				checkAllProv.Checked=false;
				checkAllProv.Enabled=false;
			}
			listProv.Items.AddList(_listProviders,x => x.GetLongDesc());
			//If the user is not allowed to run the report for all providers, default the selection to the first in the list box.
			if(checkAllProv.Enabled==false && listProv.Items.Count > 0) {
				listProv.SetSelected(0);
			}
			if(!Security.IsAuthorized(EnumPermType.ReportDailyAllProviders,true) && listProv.Items.Count>0) {
				listProv.SetAll(true);
			}
			if(!PrefC.HasClinicsEnabled) {
				listClin.Visible=false;
				labelClin.Visible=false;
				checkAllClin.Visible=false;
			}
			else {
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				if(!Security.CurUser.ClinicIsRestricted) {
					listClin.Items.Add(Lan.g(this,"Unassigned"));
					listClin.SetSelected(0);
				}
				for(int i=0;i<_listClinics.Count;i++) {
					listClin.Items.Add(_listClinics[i].Abbr);
					if(Clinics.ClinicNum==0) {
						listClin.SetSelected(listClin.Items.Count-1);
						checkAllClin.Checked=true;
					}
					if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
						listClin.SelectedIndices.Clear();
						listClin.SetSelected(listClin.Items.Count-1);
					}
				}
			}
			checkReportDisplayUnearnedTP.Checked=PrefC.GetBool(PrefName.ReportsDoShowHiddenTPPrepayments);
			_listPayDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			_listPayDefs.Add(new Def {
				ItemName="Income Transfer",
				DefNum=0,//Income Transfer's PayType is 0.
			});
			_listInsDefs=Defs.GetDefsForCategory(DefCat.InsurancePaymentType,isShort:true);
			_listClaimPayGroupDefs=Defs.GetDefsForCategory(DefCat.ClaimPaymentGroups,true);
			listPatientTypes.Items.AddList(_listPayDefs,x => x.ItemName);
			listPatientTypes.Visible=false;
			listInsuranceTypes.Items.AddList(_listInsDefs,x => x.ItemName);
			listInsuranceTypes.Visible=false;
			listClaimPayGroups.Items.AddList(_listClaimPayGroupDefs,x => x.ItemName);
			listClaimPayGroups.Visible=false;
			Plugins.HookAddCode(this,"FormPaymentSheet_Load_end");
		}

		private void checkAllProv_Click(object sender,EventArgs e) {
			if(checkAllProv.Checked) {
				listProv.ClearSelected();
			}
		}

		private void listProv_Click(object sender,EventArgs e) {
			if(listProv.SelectedIndices.Count>0) {
				checkAllProv.Checked=false;
			}
		}

		private void checkAllClin_Click(object sender,EventArgs e) {
			if(checkAllClin.Checked) {
				listClin.SetAll(true);
			}
			else {
				listClin.ClearSelected();
			}
		}

		private void listClin_Click(object sender,EventArgs e) {
			if(listClin.SelectedIndices.Count>0) {
				checkAllClin.Checked=false;
			}
		}

		private void checkAllClaimPayGroups_Click(object sender,EventArgs e) {
			if(checkAllClaimPayGroups.Checked) {
				listClaimPayGroups.Visible=false;
			}
			else {
				listClaimPayGroups.Visible=true;
			}
		}

		private void checkAllTypes_Click(object sender,EventArgs e) {
			if(checkPatientTypes.Checked){
				listPatientTypes.Visible=false;
			}
			else{
				listPatientTypes.Visible=true;
			}
		}

		private void checkIns_Click(object sender,EventArgs e) {
			if(checkInsuranceTypes.Checked) {
				listInsuranceTypes.Visible=false;
			}
			else {
				listInsuranceTypes.Visible=true;
			}
		}

		///<summary>Gets all payments from the DataTable that was passed in and checks if its payment source is 
		///considered an online payment. If it is then it will add that DataRow to the returning DataTable and 
		///remove the DataRow from the table that was passed in.</summary>
		private DataTable ModifyRowsForOnlinePayments(DataTable table) {
			DataTable dataTableOnlinePayments=table.Clone();
			List<long> listPayNums=new List<long>();
			for(int i=0;i<table.Rows.Count;i++) {
				listPayNums.Add(PIn.Long(table.Rows[i]["PayNum"].ToString()));
			}
			List<Payment> listPayments=Payments.GetPayments(listPayNums);//No Cache
			for(int i=0;i<listPayments.Count;i++) {
				if(!listPayments[i].ProcessStatus.In(ProcessStat.OnlinePending,ProcessStat.OnlineProcessed)) {
					continue;
				}
				for(int j=table.Rows.Count-1;j>=0;j--) {//Loop backwards to make it safe to remove by index.
					if(PIn.Long(table.Rows[j]["PayNum"].ToString())!=listPayments[i].PayNum) {
						continue;
					}
					dataTableOnlinePayments.Rows.Add(table.Rows[j].ItemArray);
					table.Rows.RemoveAt(j);
				}
			}
			return dataTableOnlinePayments;
		}

		///<summary>Alters the passed in table to include an afterFee column. Fills the new column for each row with the amt.</summary>
		private void AddAfterFeeColumn(DataTable table) {
			if(table==null) {
				return;
			}
			DataColumn col=table.Columns.Add("afterFee");
			for(int i=0;i<table.Rows.Count;i++) {
				table.Rows[i]["afterFee"]=PIn.Decimal(table.Rows[i]["amt"].ToString());
			}
		}

		///<summary>Alters the passed in table to include a CareCredit Fee column before the Amount column. Fills the new column for each row with the MerchantFee, if necessary. 
		///For CareCredit, the negative of the MerchantFee is used.</summary>
		private void AddCareCreditFeeColumn(DataTable table) {
			if(table==null) {
				return;
			}
			DataColumn col=table.Columns.Add("CareCredit Fee");
			col.SetOrdinal(table.Columns["amt"].Ordinal);
			for(int i=0;i<table.Rows.Count;i++) {
				table.Rows[i]["CareCredit Fee"]=0;
				if(PIn.Long(table.Rows[i]["PaymentSource"].ToString())==(long)CreditCardSource.CareCredit) {
					table.Rows[i]["CareCredit Fee"]=PIn.Decimal(table.Rows[i]["MerchantFee"].ToString())*-1;
					table.Rows[i]["afterFee"]=PIn.Decimal(table.Rows[i]["amt"].ToString())-PIn.Decimal(table.Rows[i]["MerchantFee"].ToString());
				}
			}
		}

		///<summary>Alters the passed in table to include a PayConnect Fee column before the Amount column. Fills the new column for each row with the MerchantFee, if necessary.</summary>
		private void AddPayConnectFeeColumn(DataTable table) {
			if(table==null) {
				return;
			}
			List<long> listPaymentSources=new List<long>();
			listPaymentSources.Add((long)CreditCardSource.PayConnect);
			listPaymentSources.Add((long)CreditCardSource.XServerPayConnect);
			listPaymentSources.Add((long)CreditCardSource.PayConnectPortal);
			listPaymentSources.Add((long)CreditCardSource.PayConnectPortalLogin);
			listPaymentSources.Add((long)CreditCardSource.PayConnectPaymentPortal);
			listPaymentSources.Add((long)CreditCardSource.PayConnectPaymentPortalGuest);
			DataColumn col=table.Columns.Add("PayConnect Fee");
			col.SetOrdinal(table.Columns["amt"].Ordinal);
			for(int i=0;i<table.Rows.Count;i++) {
				table.Rows[i]["PayConnect Fee"]=0;
				if(listPaymentSources.Contains(PIn.Long(table.Rows[i]["PaymentSource"].ToString()))) {
					table.Rows[i]["PayConnect Fee"]=PIn.Decimal(table.Rows[i]["MerchantFee"].ToString());
					table.Rows[i]["afterFee"]=PIn.Decimal(table.Rows[i]["amt"].ToString())+PIn.Decimal(table.Rows[i]["MerchantFee"].ToString());
				}
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!checkAllProv.Checked && listProv.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			if(PrefC.HasClinicsEnabled && !checkAllClin.Checked && listClin.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one clinic must be selected.");
				return;
			}
			if(!checkPatientTypes.Checked && listPatientTypes.SelectedIndices.Count==0
				&& !checkInsuranceTypes.Checked && listInsuranceTypes.SelectedIndices.Count==0)
			{
				MsgBox.Show(this,"At least one payment type must be selected.");
				return;
			}
			if(checkInsuranceTypes.Checked || listInsuranceTypes.SelectedIndices.Count!=0) {
				if(!checkAllClaimPayGroups.Checked && listClaimPayGroups.SelectedIndices.Count==0) {
					MsgBox.Show(this,"At least one claim payment group must be selected when any insurance payment types are selected.");
					return;
				}
			}
			ReportComplex report=new ReportComplex(true,false);
			List<long> listProvNums=new List<long>();
			List<long> listClinicNums=new List<long>();
			List<long> listInsTypes=new List<long>();
			List<long> listPatTypes=new List<long>();
			List<long> listSelectedClaimPayGroupNums=new List<long>();
			if(checkAllProv.Checked) {
				listProvNums=_listProviders.Select(x => x.ProvNum).ToList();
			}
			else {
				listProvNums=listProv.SelectedIndices.Select(x => _listProviders[x].ProvNum).ToList();
			}
			if(PrefC.HasClinicsEnabled) {
				for(int i=0;i<listClin.SelectedIndices.Count;i++) {
					if(Security.CurUser.ClinicIsRestricted) {
						listClinicNums.Add(_listClinics[listClin.SelectedIndices[i]].ClinicNum);//we know that the list is a 1:1 to _listClinics
					}
					else {
						if(listClin.SelectedIndices[i]==0) {
							listClinicNums.Add(0);
						}
						else {
							listClinicNums.Add(_listClinics[listClin.SelectedIndices[i]-1].ClinicNum);//Minus 1 from the selected index
						}
					}
				}
				if(checkAllClin.Checked) {//All Clinics selected; add all visible or hidden unrestricted clinics to the list
					listClinicNums=listClinicNums.Union(Clinics.GetAllForUserod(Security.CurUser).Select(x => x.ClinicNum)).ToList();
				}
			}
			if(checkInsuranceTypes.Checked) {
				listInsTypes=_listInsDefs.Select(x => x.DefNum).ToList();
			}
			else {
				listInsTypes=listInsuranceTypes.SelectedIndices.Select(x => _listInsDefs[x].DefNum).ToList();
			}
			if(checkPatientTypes.Checked) {
				listPatTypes=_listPayDefs.Select(x => x.DefNum).ToList();
			}
			else {
				listPatTypes=listPatientTypes.SelectedIndices.Select(x => _listPayDefs[x].DefNum).ToList();
			}
			if(checkAllClaimPayGroups.Checked) {
				listSelectedClaimPayGroupNums=_listClaimPayGroupDefs.Select(x => x.DefNum).ToList();
			}
			else {
				listSelectedClaimPayGroupNums=listClaimPayGroups.SelectedIndices.Select(x => _listClaimPayGroupDefs[x].DefNum).ToList();
			}
			DataTable tableIns=new DataTable();
			if(listProvNums.Count!=0) {
				tableIns=RpPaySheet.GetInsTable(date1.SelectionStart,date2.SelectionStart,listProvNums,listClinicNums,listInsTypes,
					listSelectedClaimPayGroupNums,checkAllProv.Checked,checkAllClin.Checked,checkInsuranceTypes.Checked,radioPatient.Checked,
					checkAllClaimPayGroups.Checked,checkShowProvSeparate.Checked);
				AddAfterFeeColumn(tableIns);
			}
			int lengthAmount=120;
			int lengthPatientName=270;
			//Both of these can be checked, meaning we'd need to borrow space for two columns instead of just one.
			if(checkShowCareCreditFees.Checked) { //Need to borrow 95 pixels for the CareCredit Fee column.
				lengthAmount-=20;
				lengthPatientName-=75;
			}
			if(checkShowPayConnectFees.Checked) { //Need to borrow 100 pixels for the PayConnect Fee column.
				lengthAmount-=25;
				lengthPatientName-=75;
			}
			DataTable tablePat=RpPaySheet.GetPatTable(date1.SelectionStart,date2.SelectionStart,listProvNums,listClinicNums,listPatTypes,
				checkAllProv.Checked,checkAllClin.Checked,checkPatientTypes.Checked,radioPatient.Checked,checkUnearned.Checked,checkShowProvSeparate.Checked,
				checkReportDisplayUnearnedTP.Checked);
			//Gather a list of PayConnect sources, as there's a handful.
			List<long> listPayConnectPaySources=new List<long>();
			listPayConnectPaySources.Add((long)CreditCardSource.PayConnect);
			listPayConnectPaySources.Add((long)CreditCardSource.XServerPayConnect);
			listPayConnectPaySources.Add((long)CreditCardSource.PayConnectPortal);
			listPayConnectPaySources.Add((long)CreditCardSource.PayConnectPortalLogin);
			listPayConnectPaySources.Add((long)CreditCardSource.PayConnectPaymentPortal);
			listPayConnectPaySources.Add((long)CreditCardSource.PayConnectPaymentPortalGuest);
			//If at least one of the "Show ___ Fees" boxes is checked, we need to take potential MerchantFees into consideration when making our payment rows.
			//We need to add an extra, mostly blanked out fee row to each payment that has a MerchantFee, since we don't want to be sticking them on an arbitrary provider or clinic. That's for offices to figure out.
			if(checkShowCareCreditFees.Checked || checkShowPayConnectFees.Checked) {
				List<long> listPayNumsVisited=new List<long>();
				//Start at the end and go backwards, so our MerchantFee rows are added directly after all of their relevant payment rows, and so we can easily scrub MerchantFees off the actual payment rows.
				//Opted to go backwards since I believe there are circumstances where the same payment can have multiple rows scattered apart from each other.
				for(int i=tablePat.Rows.Count-1;i>=0;i--) {
					DataRow rowPat=tablePat.Rows[i];
					//We only want to bother with this new row if this payment comes from a source that has a box checked for viewing their fees.
					bool isMerchantFeeConsidered=(checkShowCareCreditFees.Checked && PIn.Long(rowPat["PaymentSource"].ToString())==(long)CreditCardSource.CareCredit)
						|| (checkShowPayConnectFees.Checked && listPayConnectPaySources.Contains(PIn.Long(rowPat["PaymentSource"].ToString())));
					//IF this payment comes from a source we want to see fees for, AND it actually has a fee in the first place, AND we haven't already taken care of this payment, THEN we'll make a new row for the fee.
					if(isMerchantFeeConsidered 
						&& PIn.Decimal(rowPat["MerchantFee"].ToString())!=0 
						&& !listPayNumsVisited.Contains(PIn.Long(rowPat["PayNum"].ToString()))) 
					{
						//This new fee row should not have an amt or a provider abbreviation. It's purely for the fee portion of the payment. The other information is still relevant though,
						//so we'll just copy the current row and clear out the amt and provider.
						DataRow rowNew=tablePat.NewRow();
						rowNew.ItemArray=rowPat.ItemArray;
						rowNew["amt"]=0;
						rowNew["GROUP_CONCAT(DISTINCT provider.Abbr)"]="";
						if(PrefC.HasClinicsEnabled) {
							rowNew["clinicAbbr"]="";
						}
						//Insert the new row right after our current one.
						tablePat.Rows.InsertAt(rowNew,i+1);
						//We don't want to make the same row for one payment multiple times, so add the PayNum to our visited list.
						listPayNumsVisited.Add(PIn.Long(rowPat["PayNum"].ToString()));
					}
					//If we care about the MerchantFee, we'll have made a new row for it already. Safe to clear it out of the base row.
					rowPat["MerchantFee"]=0;
				}
			}
			AddAfterFeeColumn(tablePat);
			DataTable tableOnlinePat=new DataTable();
			if(checkShowOnlinePatientPaymentsSeparately.Checked) {
				tableOnlinePat=ModifyRowsForOnlinePayments(tablePat);
			}
			string subtitleProvs="";
			string subtitleClinics="";
			if(checkAllProv.Checked) {
				subtitleProvs=Lan.g(this,"All Providers");
			}
			else {
				subtitleProvs+=string.Join(", ",listProv.SelectedIndices.Select(x => _listProviders[x].Abbr));
			}
			if(PrefC.HasClinicsEnabled) {
				if(checkAllClin.Checked && !Security.CurUser.ClinicIsRestricted) {
					subtitleClinics=Lan.g(this,"All Clinics (includes hidden)");
				}
				else {
					for(int i=0;i<listClin.SelectedIndices.Count;i++) {
						if(i>0) {
							subtitleClinics+=", ";
						}
						if(Security.CurUser.ClinicIsRestricted) {
							subtitleClinics+=_listClinics[listClin.SelectedIndices[i]].Abbr;
						}
						else {
							if(listClin.SelectedIndices[i]==0) {
								subtitleClinics+=Lan.g(this,"Unassigned");
							}
							else {
								subtitleClinics+=_listClinics[listClin.SelectedIndices[i]-1].Abbr;//Minus 1 from the selected index
							}
						}
					}
				}
			}
			Font font=new Font("Tahoma",9);
			Font fontBold=new Font("Tahoma",9,FontStyle.Bold);
			Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
			Font fontSubTitle=new Font("Tahoma",10,FontStyle.Bold);
			report.ReportName=Lan.g(this,"Daily Payments");
			report.AddTitle("Title",Lan.g(this,"Daily Payments"),fontTitle);
			report.AddSubTitle("PracTitle",PrefC.GetString(PrefName.PracticeTitle),fontSubTitle);
			report.AddSubTitle("Providers",subtitleProvs,fontSubTitle);
			if(PrefC.HasClinicsEnabled) {
				report.AddSubTitle("Clinics",subtitleClinics,fontSubTitle);
			}
			Dictionary<long,string> dictInsDefNames=new Dictionary<long,string>();
			Dictionary<long,string> dictPatDefNames=new Dictionary<long,string>();
			List<Def> listDefsInsPayTypes=Defs.GetDefsForCategory(DefCat.InsurancePaymentType);
			List<Def> listDefsPayTypes=Defs.GetDefsForCategory(DefCat.PaymentTypes);
			for(int i=0;i<listDefsInsPayTypes.Count;i++) {
				dictInsDefNames.Add(listDefsInsPayTypes[i].DefNum,Defs.GetNameWithHidden(listDefsInsPayTypes[i].DefNum));
			}
			for(int i=0;i<listDefsPayTypes.Count;i++) {
				dictPatDefNames.Add(listDefsPayTypes[i].DefNum,Defs.GetNameWithHidden(listDefsPayTypes[i].DefNum));
			}
			dictPatDefNames.Add(0,"Income Transfer");//Otherwise income transfers show up with a payment type of "Undefined"
			int[] summaryGroupsInsPayments= { 1 };
			int[] summaryGroupsPatientPayments= { 2 };
			int[] summaryGroupsOnlinePatientPayments= { 3 };
			int[] summaryGroupsAllPayments= { 1,2,3 };
			int[] summaryGroupsAllPatientPayments= { 2,3 };
			QueryObject queryObject=null;
			//Insurance Payments Query-------------------------------------
			if(tableIns.Rows.Count!=0) {
				queryObject=report.AddQuery(tableIns,"Insurance Payments","PayType",SplitByKind.Definition,1,true,dictInsDefNames,fontSubTitle);
				queryObject.AddColumn("Date",90,FieldValueType.Date,font);
				//query.GetColumnDetail("Date").SuppressIfDuplicate = true;
				queryObject.GetColumnDetail("Date").StringFormat="d";
				queryObject.AddColumn("Carrier",150,FieldValueType.String,font);
				queryObject.AddColumn("Patient Name",150,FieldValueType.String,font);
				queryObject.AddColumn("Provider",90,FieldValueType.String,font);
				if(PrefC.HasClinicsEnabled) {
					queryObject.AddColumn("Clinic",120,FieldValueType.String,font);
				}
				queryObject.AddColumn("Check#",75,FieldValueType.String,font);
				queryObject.AddColumn("Amount",90,FieldValueType.Number,font);
				queryObject.AddGroupSummaryField("Total Insurance Payments:","Amount","amt",SummaryOperation.Sum,new List<int>(summaryGroupsInsPayments),Color.Black,fontBold,0,20);
			}
			//Patient Payments Query---------------------------------------
			if(tablePat.Rows.Count!=0) {
				queryObject=report.AddQuery(tablePat,"Patient Payments","PayType",SplitByKind.Definition,2,true,dictPatDefNames,fontSubTitle);
				queryObject.AddColumn("Date",90,FieldValueType.Date,font);
				//query.GetColumnDetail("Date").SuppressIfDuplicate = true;
				queryObject.GetColumnDetail("Date").StringFormat="d";
				queryObject.AddColumn("Paying Patient",lengthPatientName,FieldValueType.String,font);
				queryObject.AddColumn("Provider",90,FieldValueType.String,font);
				if(PrefC.HasClinicsEnabled) {
					queryObject.AddColumn("Clinic",120,FieldValueType.String,font);
				}
				queryObject.AddColumn("Check#",75,FieldValueType.String,font);
				//If either of the "Show Fees" checkboxes is checked, we need to add the respective columns to the query object and to the base table, before the Amount column.
				if(checkShowCareCreditFees.Checked) {
					queryObject.AddColumn("CareCredit Fee",95,FieldValueType.Number,font);
					AddCareCreditFeeColumn(tablePat);
				}
				if(checkShowPayConnectFees.Checked) {
					queryObject.AddColumn("PayConnect Fee",100,FieldValueType.Number,font);
					AddPayConnectFeeColumn(tablePat);
				}
				queryObject.AddColumn("Amount",lengthAmount,FieldValueType.Number,font);
				//Summarize the total amts to include at the end of this query object, placing the summarized value under the Amount column.
				queryObject.AddGroupSummaryField("Total Patient Payments:","Amount","amt",SummaryOperation.Sum,new List<int>(summaryGroupsPatientPayments),Color.Black,fontBold,0,20);
				if(checkShowCareCreditFees.Checked) {
					queryObject.AddGroupSummaryField("Total CareCredit Fees:","Amount","CareCredit Fee",SummaryOperation.Sum,new List<int>(summaryGroupsPatientPayments),Color.Black,fontBold,0,4);
				}
				if(checkShowPayConnectFees.Checked) {
					queryObject.AddGroupSummaryField("Total PayConnect Fees:","Amount","PayConnect Fee",SummaryOperation.Sum,new List<int>(summaryGroupsPatientPayments),Color.Black,fontBold,0,4);
				}
				if(checkShowCareCreditFees.Checked || checkShowPayConnectFees.Checked) {
					queryObject.AddGroupSummaryField("Total Patient Payments After Fees:","Amount","afterFee",SummaryOperation.Sum,new List<int>(summaryGroupsPatientPayments),Color.Black,fontBold,0,4);
				}
			}
			//Credit Card Online Patient Payments Query---------------------------------------
			if(checkShowOnlinePatientPaymentsSeparately.Checked && tableOnlinePat.Rows.Count!=0) {
				queryObject=report.AddQuery(tableOnlinePat,"Online Patient Payments","PayType",SplitByKind.Definition,3,true,dictPatDefNames,fontSubTitle);
				queryObject.AddColumn("Date",90,FieldValueType.Date,font);
				//query.GetColumnDetail("Date").SuppressIfDeuplicate = true;
				queryObject.GetColumnDetail("Date").StringFormat="d";
				queryObject.AddColumn("Paying Patient",lengthPatientName,FieldValueType.String,font);
				queryObject.AddColumn("Provider",90,FieldValueType.String,font);
				if(PrefC.HasClinicsEnabled) {
					queryObject.AddColumn("Clinic",120,FieldValueType.String,font);
				}
				queryObject.AddColumn("Check#",75,FieldValueType.String,font);
				//If either of the "Show Fees" checkboxes is checked, we need to add the respective columns to the query object and to the base table, before the Amount column.
				if(checkShowCareCreditFees.Checked) {
					queryObject.AddColumn("CareCredit Fee",95,FieldValueType.Number,font);
					AddCareCreditFeeColumn(tableOnlinePat);
				}
				if(checkShowPayConnectFees.Checked) {
					queryObject.AddColumn("PayConnect Fee",100,FieldValueType.Number,font);
					AddPayConnectFeeColumn(tableOnlinePat);
				}
				queryObject.AddColumn("Amount",lengthAmount,FieldValueType.Number,font);
				//Summarize the total amts to include at the end of this query object, placing the summarized value under the Amount column.
				queryObject.AddGroupSummaryField("Total Online Patient Payments:","Amount","amt",SummaryOperation.Sum,new List<int>(summaryGroupsOnlinePatientPayments),Color.Black,fontBold,0,20);
				//If either of the "Show Fees" checkboxes is checked, summarize the totals of the fees to include at the end of this query object, placing the summarized value under the Amount column.
				if(checkShowCareCreditFees.Checked) {
					queryObject.AddGroupSummaryField("Total CareCredit Fees:","Amount","CareCredit Fee",SummaryOperation.Sum,new List<int>(summaryGroupsOnlinePatientPayments),Color.Black,fontBold,0,4);
				}
				if(checkShowPayConnectFees.Checked) {
					queryObject.AddGroupSummaryField("Total PayConnect Fees:","Amount","PayConnect Fee",SummaryOperation.Sum,new List<int>(summaryGroupsOnlinePatientPayments),Color.Black,fontBold,0,4);
				}
				if(checkShowCareCreditFees.Checked || checkShowPayConnectFees.Checked) {
					queryObject.AddGroupSummaryField("Total Online Payments After Fees:","Amount","afterFee",SummaryOperation.Sum,new List<int>(summaryGroupsOnlinePatientPayments),Color.Black,fontBold,0,4);
				}
			}
			if(queryObject!=null) {	//If we have any query objects to report, summarize the totals of each of their amt columns combined to include at the end of the report.
				queryObject.AddGroupSummaryField("Total All Payments:","Amount","amt",SummaryOperation.Sum,new List<int>(summaryGroupsAllPayments),Color.Black,fontBold,0,20);
				//If either of the "Show Fees" checkboxes is checked, summarize the totals of the fees from both the Patient Payment and Online Patient Payment groups to include at the end of this query object.
				if(checkShowCareCreditFees.Checked) {
					queryObject.AddGroupSummaryField("Total All CareCredit Fees:","Amount","CareCredit Fee",SummaryOperation.Sum,new List<int>(summaryGroupsAllPatientPayments),Color.Black,fontBold,0,4);
				}
				if(checkShowPayConnectFees.Checked) {
					queryObject.AddGroupSummaryField("Total All PayConnect Fees:","Amount","PayConnect Fee",SummaryOperation.Sum,new List<int>(summaryGroupsAllPatientPayments),Color.Black,fontBold,0,4);
				}
				if(checkShowCareCreditFees.Checked || checkShowPayConnectFees.Checked) {
					queryObject.AddGroupSummaryField("Total All Payments After Fees:","Amount","afterFee",SummaryOperation.Sum,new List<int>(summaryGroupsAllPayments),Color.Black,fontBold,0,4);
				}
			}
			report.AddPageNum(font);
			report.AddGridLines();
			report.AddPageFooterText("PageFooter","*Part of a bulk check, which can be located by going to the listed patient's account",font,0,
				ContentAlignment.MiddleLeft);
			//Try and fill out export tables for each query object, which will display on the report.
			if(!report.SubmitQueries()) {
				return;
			}
			using FormReportComplex formReportComplex=new FormReportComplex(report);
			formReportComplex.ShowDialog();
			DialogResult=DialogResult.OK;
		}

	}
}