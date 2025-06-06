using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.WebTypes.WebForms;

namespace OpenDental {
	/// <summary>
	/// This Form is primarily used by the dental office to upload sheetDefs
	/// </summary>
	public partial class FormWebFormSetup:FormODBase {
		#region WebSched
		/*
		This form is called from 3 places.
		2 of them are just ordinary setup links.
		The third is when it's called from UserControlHostedURLs which is on FormEServicesWebSchedAdvanced.
		In this case, the window is not being used for setup but is being used as a Web Form picker
		with the twist that it's also expecting a constructed URL from the lower right section.
		The 4 variables below are only for that situation.
		The first 3 variables get set prior to launching this window,
		and the URLResult is used after closing.
		*/
		///<summary></summary>
		public bool IsPickerForWebSched;
		///<summary>Only used when IsPickerForWebSched.</summary>
		public long ClinicNum;
		///<summary>Only used when IsPickerForWebSched.</summary>
		public string UrlWebForm;
		///<summary>Only used when IsPickerForWebSched. The result when successful Save.</summary>
		public string URLResult="";
		#endregion WebSched
		
		private string _sheetDefAddress="";
		private string _redirectURL="";
		private List<WebForms_SheetDef> _listWebForms_SheetDefs;
		private long _webSheetDefIDSelected=0;
		private long _dentalOfficeID=0;
		private long _registrationKeyID=0;
		private List<long> _listNextFormIdsSelected=new List<long>();
		private WebForms_Preference _webForms_PreferenceOld=new WebForms_Preference();
		private WebForms_Preference _webForms_Preference=new WebForms_Preference();
		private bool _isAutoFillNameBirthday=true;
		private bool _isDisableTypeSignature=false;

		public FormWebFormSetup() {
			InitializeComponent();
			InitializeLayoutManager();
			Lan.F(this);
		}

	private void FormWebFormSetup_Load(object sender,EventArgs e) {
			//Formats currently supported in webforms.
			DateTime DateTimeExample=new DateTime(2024,6,18);
			List<CultureDateFormat> listCultureDateFormats=new List<CultureDateFormat>();
			listCultureDateFormats.Add(new CultureDateFormat("en-US","M/d/yyyy ("+DateTimeExample.ToString("M/d/yyyy")+")"));
			listCultureDateFormats.Add(new CultureDateFormat("en-CA","dd/MM/yyyy ("+DateTimeExample.ToString("dd/MM/yyyy")+")"));
			listCultureDateFormats.Add(new CultureDateFormat("da-DK","dd-MM-yyyy ("+DateTimeExample.ToString("dd-MM-yyyy")+")"));
			listCultureDateFormats.Add(new CultureDateFormat("en-AU","d/MM/yyyy ("+DateTimeExample.ToString("d/MM/yyyy")+")"));
			listCultureDateFormats.Add(new CultureDateFormat("mn-MN","yy.MM.dd ("+DateTimeExample.ToString("yy.MM.dd")+")"));
			listCultureDateFormats.Add(new CultureDateFormat("zh-CN","yyyy/M/d ("+DateTimeExample.ToString("yyyy/M/d")+")"));
			comboDateFormat.Items.AddList(listCultureDateFormats,(cultureDateFormat) => cultureDateFormat.DateFormat);
			comboDateFormat.SelectedIndex=0;
			if(IsPickerForWebSched) {
				//hide everything but grid, ConstructURL, and Save button.
				for(int i = 0;i<PanelClient.Controls.Count;i++) {
					if(PanelClient.Controls[i]==gridMain || PanelClient.Controls[i]==groupConstructURL || PanelClient.Controls[i]==butSave) {
						PanelClient.Controls[i].Visible=true;
						continue;
					}
					PanelClient.Controls[i].Visible=false;
				}
				labelConstructInfo.Visible=false;//these instructions are for general setup, not IsPickerForWebSched
				comboClinic.Enabled=false;
			}
			else{
				butSave.Visible=false;
				return;
			}
			if(IsPickerForWebSched) {
				gridMain.SelectionMode=GridSelectionMode.OneRow;//Allow only one form to be selected
				if(!string.IsNullOrEmpty(UrlWebForm)) {
					//These variables will set the controls on the form during Shown, 
					//and then ConstructURL will make the URL show the same as was passed in.
					string decodeURL=HttpUtility.UrlDecode(UrlWebForm);
					Uri uRIUnparsed=new Uri(decodeURL);
					NameValueCollection nameValueCollection=HttpUtility.ParseQueryString(uRIUnparsed.Query);
					if(!string.IsNullOrEmpty(nameValueCollection["WSDID"])) {
						_webSheetDefIDSelected=PIn.Long(nameValueCollection.Get("WSDID"),false);
					}
					if(!string.IsNullOrEmpty(nameValueCollection["NFID"])) {
						_listNextFormIdsSelected=nameValueCollection.Get("NFID").Split(',').Select(x => PIn.Long(x,false)).ToList();
					}
					if(!string.IsNullOrEmpty(nameValueCollection["ReturnURL"])) {
						_redirectURL=nameValueCollection.Get("ReturnURL");
					}
					if(!string.IsNullOrEmpty(nameValueCollection["AFNAB"])) {
						if(nameValueCollection.Get("AFNAB")=="N") {
							_isAutoFillNameBirthday=false;
						}
					}
					if(!string.IsNullOrEmpty(nameValueCollection["DTS"])) {
						if(nameValueCollection.Get("DTS")=="Y") {
							_isDisableTypeSignature=true;
						}
					}
				}
			}
			if(PrefC.HasClinicsEnabled) {
				if(IsPickerForWebSched){
					comboClinic.ClinicNumSelected=ClinicNum;
				}
				else{
					comboClinic.ClinicNumSelected=Clinics.ClinicNum;
				}
			}
			butSavePrefs.Enabled=false;
			checkAutoFillNameBDPref.Checked=PrefC.GetBool(PrefName.WebFormsAutoFillNameAndBirthdate);//the pref itself
			checkAutoFillNameAndBirthdate.Checked=checkAutoFillNameBDPref.Checked;//and the consequence of the pref
			checkEnableAutoDownload.Checked=PrefC.GetBool(PrefName.WebFormsDownloadAutomcatically);
		}

		private void FormWebFormSetup_Shown(object sender,EventArgs e) {
			#region FetchValuesFromWebServer
			string webHostSynchServerURL=PrefC.GetString(PrefName.WebHostSynchServerURL);
			textboxWebHostAddress.Text=webHostSynchServerURL;
			butSavePrefs.Enabled=false;
			if((webHostSynchServerURL==WebFormL.SynchUrlStaging) || (webHostSynchServerURL==WebFormL.SynchUrlDev)) {
				WebFormL.IgnoreCertificateErrors();
			}
			Cursor=Cursors.WaitCursor;
			_dentalOfficeID=WebUtils.GetDentalOfficeID();
			_registrationKeyID=WebUtils.GetRegKeyID();
			if(_registrationKeyID==0 && _dentalOfficeID==0) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Either the registration key provided by the dental office is incorrect or the Host Server Address cannot be found.");
				return;
			}
			if(WebForms_Preferences.TryGetPreference(out _webForms_Preference)) {
				butWebformBorderColor.BackColor=_webForms_Preference.ColorBorder;
				_sheetDefAddress=WebUtils.GetSheetDefAddress();
				checkDisableSigsPref.Checked=_webForms_Preference.DisableSignatures;
				if(string.IsNullOrEmpty(_webForms_Preference.CultureName)){//Just in case.
					_webForms_Preference.CultureName=System.Globalization.CultureInfo.CurrentCulture.Name;
					WebForms_Preferences.SetPreferences(_webForms_Preference);
				}
				SetComboBoxDateFormat();
				_webForms_PreferenceOld=_webForms_Preference.Copy();
			}
			FillGrid();//Also gets sheet def list from server
			Cursor=Cursors.Default;


			#endregion FetchValuesFromWebServer
			if(_listNextFormIdsSelected.Count > 0) {//If entering form with Next Form Id values, fill the textNextForms box with the corresponding descriptions.
				textNextForms.Text=string.Join(", ",_listWebForms_SheetDefs.Where(x => _listNextFormIdsSelected
					.Contains(x.WebSheetDefID)).Select(y => y.Description));
			}
			if(_webSheetDefIDSelected!=0) {//If entering the form with WSDID, have corresponding grid item selected and URL updated.
				for(int i=0;i<gridMain.ListGridRows.Count;i++) {
					if(((WebForms_SheetDef)gridMain.ListGridRows[i].Tag).WebSheetDefID==_webSheetDefIDSelected) {
						gridMain.SetSelected(i,true);
					}
				}
			}
			if(!string.IsNullOrEmpty(_redirectURL)) {
				textRedirectURL.Text=_redirectURL;
			}
			checkAutoFillNameAndBirthdate.Checked=_isAutoFillNameBirthday;
			checkDisableTypedSig.Checked=_isDisableTypeSignature;
			ConstructURLs();
		}

		///<summary>This also gets a new list of sheet defs from the server.  But it's only called after testing that the web service exists.</summary>
		private void FillGrid() {
			if(!WebForms_SheetDefs.TryDownloadSheetDefs(out _listWebForms_SheetDefs)) {
				MsgBox.Show(this,"Failed to download sheet definitions.");
				_listWebForms_SheetDefs=new List<WebForms_SheetDef>();
			}
			gridMain.Columns.Clear();
			GridColumn gridColumn=new GridColumn(Lan.g(this,"Description"),200);
			gridMain.Columns.Add(gridColumn);
			gridMain.ListGridRows.Clear();
			for(int i=0;i<_listWebForms_SheetDefs.Count;i++) {
				GridRow gridRow=new GridRow();
				gridRow.Tag=_listWebForms_SheetDefs[i];
				gridRow.Cells.Add(_listWebForms_SheetDefs[i].Description);
				gridMain.ListGridRows.Add(gridRow);
			}
			gridMain.EndUpdate();
		}

		private string SheetDefBaseURL(WebForms_SheetDef webForms_SheetDef) {
			if(_registrationKeyID==0) {
				return _sheetDefAddress+"?DOID="+_dentalOfficeID+"&WSDID="+webForms_SheetDef.WebSheetDefID;//The "old way" 
			}
			return _sheetDefAddress+"?DOID="+_dentalOfficeID+"&RKID="+_registrationKeyID+"&WSDID="+webForms_SheetDef.WebSheetDefID;
		}

		private void gridMain_MouseUp(object sender,MouseEventArgs e) {
			ConstructURLs();
		}

		private void OpenBrowser() {
			if(gridMain.SelectedIndices.Length<1) {
				return;
			}
			string[] stringArrayLines=textURLs.Text.Split(
				new[]{ "\r\n","\r","\n" },
				StringSplitOptions.None
			);
			for(int i=0;i<stringArrayLines.Length;i++){
				ODException.SwallowAnyException(() => { 
					if(!ODBuild.IsThinfinity() && ODCloudClient.IsAppStream) {
						try {
							ODCloudClient.LaunchFileWithODCloudClient(stringArrayLines[i]);
						}
						catch(Exception ex) {
							MessageBox.Show(ex.Message);
							return;
						}
					}
					else {
						System.Diagnostics.Process.Start(stringArrayLines[i]);
					}
				});
			}
		}

		/// <summary>Enables the SavePrefs button if any of the preferences/settings have changed from what they were when form was opened or last saved. Otherwise, false</summary>
		private void ResetSavePrefsButton() {
			if(_webForms_PreferenceOld.ColorBorder.ToArgb()!=_webForms_Preference.ColorBorder.ToArgb()
				|| _webForms_PreferenceOld.CultureName!=_webForms_Preference.CultureName
				|| _webForms_PreferenceOld.DisableSignatures!=_webForms_Preference.DisableSignatures
				|| textboxWebHostAddress.Text.Trim()!=PrefC.GetString(PrefName.WebHostSynchServerURL) 
				|| checkAutoFillNameBDPref.Checked!=PrefC.GetBool(PrefName.WebFormsAutoFillNameAndBirthdate)
				|| checkEnableAutoDownload.Checked!=PrefC.GetBool(PrefName.WebFormsDownloadAutomcatically))
			{
				butSavePrefs.Enabled=true;
				return;
			}
			butSavePrefs.Enabled=false;
		}

		///<summary>Sets the selected index for comboDateFormat. Only specific formats are supported, if current culture is not supported default will be set to en-US.</summary>
		private void SetComboBoxDateFormat() {
			int index=0;
			switch(_webForms_Preference.CultureName) { 
				case "ar-JO":
				case "en-CA":
				case "en-GB":
				case "en-ES":
				case "en-MX":
				case "en-PR":
				case "nl-NL":
					index=1;
					break;
				case "da-DK":
				case "en-IN":
					index=2;
					break;
				case "en-AU":
				case "en-NZ":
					index=3;
					break;
				case "mn-MN":
					index=4;
					break;
				case "zh-CN":
					index=5;
					break;
				default:
					//en-US M/d/yyyy
					index=0;
					break;
			}
			comboDateFormat.SelectedIndex=index;
		}

		private void textboxWebHostAddress_TextChanged(object sender,EventArgs e) {
			ResetSavePrefsButton();
		}

		private void butSavePrefs_Click(object sender,EventArgs e) {
			//disabled unless user changed prefs in UI
			Cursor=Cursors.WaitCursor;
			if(!WebForms_Preferences.SetPreferences(_webForms_Preference,urlOverride:textboxWebHostAddress.Text.Trim())) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Either the registration key provided by the dental office is incorrect or the Host Server Address cannot be found.");
				//leave the button enabled
				return;
			}
			_webForms_PreferenceOld=_webForms_Preference.Copy();
			int freqDownloadAlert=3600000;//Default, which is every 1 hour
			if(checkEnableAutoDownload.Checked) {
				freqDownloadAlert=120000;//Every 2 minutes
			}
			bool changed=false;
			changed |=Prefs.UpdateString(PrefName.WebHostSynchServerURL,textboxWebHostAddress.Text.Trim());
			changed |=Prefs.UpdateBool(PrefName.WebFormsAutoFillNameAndBirthdate,checkAutoFillNameBDPref.Checked);
			changed |=Prefs.UpdateBool(PrefName.WebFormsDownloadAutomcatically,checkEnableAutoDownload.Checked);
			changed |=Prefs.UpdateInt(PrefName.WebFormsDownloadAlertFrequency,freqDownloadAlert);
			if(changed){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			Cursor=Cursors.Default;
			butSavePrefs.Enabled=false;
		}

		private void butWebformBorderColor_Click(object sender,EventArgs e) {
			ShowColorDialog();
		}

		private void butChange_Click(object sender,EventArgs e) {
			ShowColorDialog();
		}

		private void ShowColorDialog(){
			using ColorDialog colorDialog=new ColorDialog();
			colorDialog.Color=butWebformBorderColor.BackColor;
			if(colorDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			butWebformBorderColor.BackColor=colorDialog.Color;
			_webForms_Preference.ColorBorder=butWebformBorderColor.BackColor;
			ResetSavePrefsButton();
		}

		private void textRedirectURL_TextChanged(object sender,EventArgs e) {
			ConstructURLs();
		}

		private void butNextForms_Click(object sender,EventArgs e) {
			if(_listWebForms_SheetDefs==null) {
				MsgBox.Show(this,"No selected Available Web Forms");
				return;
			}
			if(gridMain.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Please pick only one Web Form first.");
				return;
			}
			List<string> listSheetDefDescriptionsSelected=textNextForms.Text.Split(", ",StringSplitOptions.RemoveEmptyEntries).ToList();
			List<WebForms_SheetDef> listWebForms_SheetDefsSelected=new List<WebForms_SheetDef>();
			for(int i=0;i < listSheetDefDescriptionsSelected.Count;i++) {//Loop through each selected description to preserve order.
				listWebForms_SheetDefsSelected.Add(_listWebForms_SheetDefs.Find(x => x.Description==listSheetDefDescriptionsSelected[i]));
			}
			using FormWebFormNextForms formWebFormNextForms=new FormWebFormNextForms(_listWebForms_SheetDefs,listWebForms_SheetDefsSelected);
			if(formWebFormNextForms.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_listNextFormIdsSelected=formWebFormNextForms.ListWebForms_SheetDefsSelected.Select(x => x.WebSheetDefID).ToList();
			textNextForms.Text=string.Join(", ",formWebFormNextForms.ListWebForms_SheetDefsSelected.Select(x => x.Description));
			ConstructURLs();
		}

		private void ComboClinic_SelectionChangeCommitted(object sender, EventArgs e){
			ConstructURLs();
		}

		private void comboDateFormat_SelectionChangeCommitted(object sender,EventArgs e) {
			CultureDateFormat cultureDateFormat=comboDateFormat.GetSelected<CultureDateFormat>();
			if(cultureDateFormat==null) {
				return;
			}
			_webForms_Preference.CultureName=cultureDateFormat.CultureName;
			ResetSavePrefsButton();
		}

		private void checkAutoFillNameBDPref_CheckedChanged(object sender,EventArgs e) {
			//this event handler is for the checkbox in the pres section, not down in the construct URL section
			checkAutoFillNameAndBirthdate.Checked=checkAutoFillNameBDPref.Checked;//the consequence of the pref
			ConstructURLs();
		}

		private void checkAutoFillNameAndBirthdate_CheckedChanged(object sender,EventArgs e) {
			//this event handler is for the check down in the construct URL section, not the one up in the prefs section
			ConstructURLs();
		}

		private void checkDisableTypedSig_CheckChanged(object sender,EventArgs e) {
			ConstructURLs();
		}

		///<summary>Build a URL based on selected forms. Should be run after gridMain is filled.</summary>
		private void ConstructURLs() {
			textURLs.Clear();
			List<string> listURLs=new List<string>();
			for(int i=0;i<gridMain.SelectedTags<WebForms_SheetDef>().Count();i++) {
				string url=SheetDefBaseURL(gridMain.SelectedTags<WebForms_SheetDef>()[i]);
				if(_listNextFormIdsSelected.Count>0) {
					url+="&NFID="+string.Join("&NFID=",_listNextFormIdsSelected.FindAll(x => x!=gridMain.SelectedTags<WebForms_SheetDef>()[i].WebSheetDefID));//Will remove ids that match the id of the sheet def we are currently constructing a url for.
				}
				if(comboClinic.ClinicNumSelected > 0) {//'None' is not selected
					url+="&CID="+comboClinic.ClinicNumSelected.ToString(); 
				}
				if(!checkAutoFillNameAndBirthdate.Checked) {
					url+="&AFNAB=N";
				}
				if(checkDisableTypedSig.Enabled && checkDisableTypedSig.Checked) {
					url+="&DTS=Y";
				}
				if(textRedirectURL.Text != "") {
					url+="&ReturnURL="+HttpUtility.UrlEncode(textRedirectURL.Text);
				}
				listURLs.Add(url);
			}
			textURLs.AppendText(string.Join("\r\n",listURLs));
			ResetSavePrefsButton();
		}
		
		private void checkDisableWebFormSignatures_CheckedChanged(object sender,EventArgs e) {
			//Do not allow user to disable typed signatures if they have already disabled all signatures.
			_webForms_Preference.DisableSignatures=checkDisableSigsPref.Checked;
			checkDisableTypedSig.Enabled=!checkDisableSigsPref.Checked;
			ResetSavePrefsButton();
		}

		private void checkDisableAutoDownload_CheckedChanged(object sender,EventArgs e) {
			ResetSavePrefsButton();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FrmSheetPicker frmSheetPicker=new FrmSheetPicker();
			frmSheetPicker.IsWebForm=true;
			frmSheetPicker.AllowMultiSelect=true;
			frmSheetPicker.SheetType=SheetTypeEnum.PatientForm;
			frmSheetPicker.HideKioskButton=true;
			frmSheetPicker.ShowDialog();
			if(!frmSheetPicker.IsDialogOK) {
				return;
			}
			//Make sure each selected sheet contains FName, LName, and Birthdate.
			List<SheetDef> listSheetDefs=frmSheetPicker.ListSheetDefsSelected;
			for(int i=0;i<listSheetDefs.Count;i++) {
				if(!WebFormL.VerifyRequiredFieldsPresent(listSheetDefs[i])) {
					return;
				}
			}
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<listSheetDefs.Count;i++) {
				WebFormL.LoadImagesToSheetDef(listSheetDefs[i]);
				WebFormL.TryAddOrUpdateSheetDef(this,listSheetDefs[i],isNew:true);
			}
			FillGrid();
			Cursor=Cursors.Default;
		}

		private void butUpdate_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length < 1) {
				MsgBox.Show(this,"Please select an item from the grid first.");
				return;
			}
			if(gridMain.SelectedIndices.Length > 1) {
				MsgBox.Show(this,"Please select one web form at a time.");
				return;
			}
			WebForms_SheetDef webForms_sheetDef=gridMain.SelectedTag<WebForms_SheetDef>();
			SheetDef sheetDef=SheetDefs.GetFirstOrDefault(x => x.SheetDefNum==webForms_sheetDef.SheetDefNum);
			if(sheetDef==null) {//This web form has never had a SheetDefNum assigned or the sheet has been deleted.
				MsgBox.Show(this,"This Web Form is not linked to a valid Sheet.  Please select the correct Sheet that this Web Form should be linked to.");
				FrmSheetPicker frmSheetPicker=new FrmSheetPicker();
				frmSheetPicker.IsWebForm=true;
				frmSheetPicker.SheetType=SheetTypeEnum.PatientForm;
				frmSheetPicker.HideKioskButton=true;
				frmSheetPicker.ShowDialog();
				if(!frmSheetPicker.IsDialogOK || frmSheetPicker.ListSheetDefsSelected.Count==0) {
					return;
				}
				sheetDef=frmSheetPicker.ListSheetDefsSelected.FirstOrDefault();
			}
			else {//sheetDef not null
				SheetDefs.GetFieldsAndParameters(sheetDef);
			}
			if(!WebFormL.VerifyRequiredFieldsPresent(sheetDef)) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			WebFormL.LoadImagesToSheetDef(sheetDef);
			WebFormL.TryAddOrUpdateSheetDef(this,sheetDef,isNew:false,new List<WebForms_SheetDef> { webForms_sheetDef });
			FillGrid();
			Cursor=Cursors.Default;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an item from the grid first.");
				return;
			}
			int failures=0;
			List<WebForms_SheetDef> listWebForms_SheetDefs=gridMain.SelectedTags<WebForms_SheetDef>().ToList();//Sheets to be deleted
			List<WebForms_SheetDef> listWebForms_SheetDefsNextForms=listWebForms_SheetDefs.FindAll(x => _listNextFormIdsSelected.Contains(x.WebSheetDefID));//Sheets currently in the 'Next Forms' field.
			if(listWebForms_SheetDefsNextForms.Count>0) {//If a sheet that is to be deleted is currently in the 'Next Forms' field.
				string sheetDescriptions=string.Join(",\n",listWebForms_SheetDefsNextForms.Select(x => x.Description));
				//Prompt user if they want to continue with delete. If no, simply return.
				if(MessageBox.Show(this,Lan.g(this,"The following sheet(s) will also be deleted from the 'Next Forms' field:")+"\n"+sheetDescriptions+"\n"+Lan.g(this,"Do you want to continue?")
					,Lan.g(this,"Warning"),MessageBoxButtons.YesNo)==DialogResult.No)
				{ 
					return;
				}
			}
			string unlinkWebForm=Lan.g(this,"Click 'Preferences' in the eServices Automated Messaging window to change web form selections.");
			//check if any web form to be deleted is linked to ApptNewPatientThankYouWebSheetDefID ClinicPref by WebSheetDefID
			if(PrefC.HasClinicsEnabled) {
				List<ClinicPref> listClinicPrefs=ClinicPrefs.GetPrefAllClinics(PrefName.ApptNewPatientThankYouWebSheetDefID,includeDefault:false);
				listClinicPrefs.RemoveAll(x => string.IsNullOrWhiteSpace(x.ValueString) || x.ValueString=="0"); //remove any clinics where the pref is not set
				List<ClinicPref> listClinicPrefMatches=listClinicPrefs
					//check if any of the web form IDs in clinicprefs match the web form IDs to be deleted
					.Where(x => listWebForms_SheetDefs.Any(y => POut.Long(y.WebSheetDefID)==x.ValueString))
					//Don't bother blocking if webform is linked to a hidden clinic.
					.Where(x => !(Clinics.GetClinic(x.ClinicNum)??new Clinic(){IsHidden=true}).IsHidden)
					.ToList();
				if(listClinicPrefMatches.Count!=0) {
					List<string> listDescriptions=new List<string>();
					for(int i = 0;i<listWebForms_SheetDefs.Count;i++) {
						//only add the sheet ids that are also in clinic prefs
						if(listClinicPrefMatches.Any(x => PIn.Long(x.ValueString)==listWebForms_SheetDefs[i].WebSheetDefID)) {
							//store the descriptions for each web form to access in error msgs
							listDescriptions.Add(listWebForms_SheetDefs[i].Description);
						}
					}
					//skip logic below if 10 or more clinics have these webforms in use
					if(listClinicPrefMatches.Count>=10) {
						MsgBox.Show(Lan.g(this,"Cannot delete New Patient Thank-You Web Form(s) '")+string.Join(", ",listDescriptions)+Lan.g(this,"'. More than 10 clinics are linked.\r\n\r\n")+unlinkWebForm);
						return;
					}
					List<Clinic> listClinics=new List<Clinic>();
					for(int i = 0;i<listClinicPrefMatches.Count;i++) {
						//get the clinics associated with each clinic pref
						listClinics.Add(Clinics.GetFirstOrDefault(x => x.ClinicNum==listClinicPrefMatches[i].ClinicNum));
					}
					List<ClinicPref> listClinicPrefDefaults=ClinicPrefs.GetPrefAllClinics(PrefName.AutoMsgingUseDefaultPref,includeDefault:false);
					listClinicPrefDefaults.RemoveAll(x => string.IsNullOrWhiteSpace(x.ValueString) || x.ValueString=="0"); //remove any clinics where the pref is not set
					listClinicPrefDefaults.RemoveAll(x => !listClinicPrefMatches.Any(y => y.ClinicNum==x.ClinicNum)); //remove any clinic prefs not associated with a web form to be deleted
					//store clinics which have 'use defaults' set but have a separate webform selected which will be deleted
					List<Clinic> listClinicDefaults=listClinics.FindAll(x => listClinicPrefDefaults.Any(y => y.ClinicNum==x.ClinicNum));
					if(listClinicPrefDefaults.Count>0) {
						MsgBox.Show(Lan.g(this,"Cannot delete New Patient Thank-You Web Form(s) '")+string.Join(", ",listDescriptions)+Lan.g(this,"'.\r\n\r\nThe following clinics are set to use the default web form, you will need to disable 'use defaults' and change the underlying web form selection:\r\n")+string.Join(", ",listClinicDefaults.Select(x => x.Abbr))+"\r\n\r\n"+unlinkWebForm);
						return;
					}
					MsgBox.Show(Lan.g(this,"Cannot delete New Patient Thank-You Web Form(s) '")+string.Join(", ",listDescriptions)+Lan.g(this,"'.\r\n\r\nChange the web form selection for the following clinics:\r\n")+string.Join(", ",listClinics.Select(x => x.Abbr))+"\r\n\r\n"+unlinkWebForm);
					return;
				}
			}
			//check if any web form to be deleted is linked to ApptNewPatientThankYouWebSheetDefID Pref by WebSheetDefID
			List<long> listApptNewPatientThankYouWebSheetDefIDs=PrefC.GetString(PrefName.ApptNewPatientThankYouWebSheetDefID)
				.Split(",",StringSplitOptions.RemoveEmptyEntries)
				.Select(x => PIn.Long(x))
				.ToList();
			//Find the WebSheetDefIDs that are not okay to delete.
			List<long> listWebSheetDefIDsInUse=listWebForms_SheetDefs
				.Select(x => x.WebSheetDefID)
				.Intersect(listApptNewPatientThankYouWebSheetDefIDs)
				.ToList();
			if(listWebSheetDefIDsInUse.Count>0) {
				List<string> listWebFormDescriptions=listWebForms_SheetDefs
					.FindAll(x => listWebSheetDefIDsInUse.Any(y => y==x.WebSheetDefID))
					.Select(z => z.Description)
					.ToList();
				MsgBox.Show(Lan.g(this,"Cannot delete New Patient Thank-You Web Form(s):")+"\r\n  "
					+string.Join("\r\n  ",listWebFormDescriptions)+"\r\n"
					+Lan.g(this,"Unlink the shown form(s) before continuing.")+"\r\n\r\n"
					+unlinkWebForm);
				return;
			}
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<listWebForms_SheetDefs.Count;i++) {
				if(!WebForms_SheetDefs.DeleteSheetDef(listWebForms_SheetDefs[i].WebSheetDefID)) {
					failures++;
				}
			}
			if(failures>0) {
				Cursor=Cursors.Default;
				MessageBox.Show(this,Lan.g(this,"Error deleting")+" "+POut.Int(failures)+" "+Lan.g(this,"web form(s). Either the web service is not available or "
					+"the Host Server Address cannot be found."));
			}
			FillGrid();
			//Remove all the NextForm ids that belong to SheetDefs that were deleted.
			_listNextFormIdsSelected=_listNextFormIdsSelected.FindAll(x => !listWebForms_SheetDefs.Any(y => y.WebSheetDefID==x));
			//Recreate the textNextForms control's text.
			List<string> listNextFormsDescriptions=new List<string>();
			for(int i=0;i<_listNextFormIdsSelected.Count;i++) {
				WebForms_SheetDef webForms_SheetDef=_listWebForms_SheetDefs.Find(x => x.WebSheetDefID==_listNextFormIdsSelected[i]);
				if(webForms_SheetDef!=null) {
					listNextFormsDescriptions.Add(webForms_SheetDef.Description);
				}
			}
			textNextForms.Text=string.Join(", ",listNextFormsDescriptions);
			ConstructURLs();
			Cursor=Cursors.Default;
		}

		private void butNavigateTo_Click(object sender,EventArgs e) {
			OpenBrowser();
		}

		private void butCopyToClipboard_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				return;
			}
			WebForms_SheetDef webSheetDef=gridMain.SelectedTag<WebForms_SheetDef>();
			try {
				ODClipboard.SetClipboard(textURLs.Text);
			}
			catch(Exception ex) {
				MsgBox.Show(this,"Could not copy contents to the clipboard.  Please try again.");
				ex.DoNothing();
			}
		}

		private void butSave_Click(object sender,EventArgs e) {
			//button only visible when IsPickerForWebSched
			if(!string.IsNullOrWhiteSpace(textRedirectURL.Text) && !Uri.IsWellFormedUriString(textRedirectURL.Text,UriKind.Absolute)) {
				string error=label3.Text+Lan.g(this," should use the following format: http://www.opendental.com or https://www.opendental.com");
				MsgBox.Show(this,error);
				return;
			}
			URLResult=textURLs.Text;
			DialogResult=DialogResult.OK;
		}

		private class CultureDateFormat {
			public string CultureName="";
			public string DateFormat="";

			public CultureDateFormat(string cultureName,string dateFormat) {
				CultureName=cultureName;
				DateFormat=dateFormat;
			}
		}
	}
}