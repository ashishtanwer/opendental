using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormLanguageAndRegion:FormODBase {
		private List<CultureInfo> _listCultureInfos;

		public FormLanguageAndRegion() {
			InitializeComponent();
			InitializeLayoutManager();
			Lan.F(this);
		}

		private void FormLanguageAndRegion_Load(object sender,EventArgs e) {
			CultureInfo cultureInfo=PrefC.GetLanguageAndRegion();
			_listCultureInfos=CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => !x.IsNeutralCulture).OrderBy(x => x.DisplayName).ToList();
			textLARLocal.Text=CultureInfo.CurrentCulture.DisplayName;
			if(PrefC.GetString(PrefName.LanguageAndRegion)=="") {
				textLARDB.Text="None";
			}
			else {
				textLARDB.Text=cultureInfo.DisplayName;
			}
			comboLanguageAndRegion.Items.Clear();
			for(int i=0;i<_listCultureInfos.Count;i++){
				comboLanguageAndRegion.Items.Add(_listCultureInfos[i].DisplayName);
			}
			comboLanguageAndRegion.SelectedIndex=_listCultureInfos.FindIndex(x => x.DisplayName==cultureInfo.DisplayName);
			checkNoShow.Checked=ComputerPrefs.LocalComputer.NoShowLanguage;
			if(!Security.IsAuthorized(EnumPermType.Setup,true)) {
				comboLanguageAndRegion.Visible=false;
				labelNewLAR.Visible=false;
				butSave.Enabled=false;
				checkNoShow.Enabled=false;
			}
		}

		private void butSave_Click(object sender,EventArgs e) {
			if(comboLanguageAndRegion.SelectedIndex==-1) {
				MsgBox.Show(this,"Select a language and region.");
				return;
			}
			if(!Security.IsAuthorized(EnumPermType.Setup, true)){
				DialogResult=DialogResult.OK;
				return;
			}
			//_cultureCur=_listAllCultures[comboLanguageAndRegion.SelectedIndex];
			if(Prefs.UpdateString(PrefName.LanguageAndRegion,_listCultureInfos[comboLanguageAndRegion.SelectedIndex].Name)) {
				MsgBox.Show(this,"Program must be restarted for changes to take full effect.");
			}
			ComputerPrefs.LocalComputer.NoShowLanguage=checkNoShow.Checked;
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			DialogResult=DialogResult.OK;
		}

	}
}