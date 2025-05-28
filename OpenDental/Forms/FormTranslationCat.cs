using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using OpenDental.Thinfinity;

namespace OpenDental{
///<summary></summary>
	public partial class FormTranslationCat : FormODBase {

		///<summary></summary>
		public FormTranslationCat(){
			InitializeComponent();
			InitializeLayoutManager();
			Lan.F(this);
		}

		private void FormTranslation_Load(object sender, System.EventArgs e) {
			//MessageBox.Show(CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
			if(CultureInfo.CurrentCulture.Name=="en-US"){
				MessageBox.Show("You must change your culture in Windows first to something other than English-US.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			labelLanguage.Text+="\r\n"+CultureInfo.CurrentCulture.DisplayName;
			FillList();
		}

		private void FillList(){
			listCats.Items.Clear();
			listCats.Items.AddList(Lans.GetListCat(),x => x.ToString());
		}

		private void listCats_DoubleClick(object sender, System.EventArgs e){
			if(listCats.SelectedIndex==-1){
				return;
			}
			using FormTranslation formTranslation=new FormTranslation(listCats.GetSelected<string>()); 
			formTranslation.ShowDialog();
		}

		private void butDownload_Click(object sender, System.EventArgs e) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				MessageBox.Show("Not allowed when using client web.");
				return;
			}
			string remoteUri = "http://www.opendental.com/cultures/";
			string fileName = CultureInfo.CurrentCulture.Name+".txt";//eg. en-US.txt
			string myStringWebResource = null;
			WebClient webClient = new WebClient();
			myStringWebResource=remoteUri+fileName;
			Cursor=Cursors.WaitCursor;
			try{
				webClient.DownloadFile(myStringWebResource,fileName);
			}
			catch{
				Cursor=Cursors.Default;
				MessageBox.Show("Either you do not have internet access, or no translations are available for "+CultureInfo.CurrentCulture.DisplayName);
				return;
			}
			//ClassConvertDatabase ConvertDB=new ClassConvertDatabase();
			try{
				//ConvertDB.ExecuteFile(fileName);
				string content = File.ReadAllText(fileName).Trim();
				Lans.LoadTranslationsFromTextFile(content);
			}
			catch{
				Cursor=Cursors.Default;
				MessageBox.Show("Translations not installed properly.");
				return;
			}
			LanguageForeigns.RefreshCache();
			Cursor=Cursors.Default;
			MessageBox.Show("Done");
		}

		///<summary>Only exports for the current culture.</summary>
		private void butExport_Click(object sender, System.EventArgs e) {
			string fileName=CultureInfo.CurrentCulture.Name+".sql";//eg en-US.sql
			string filePath=ODFileUtils.CombinePaths(Path.GetTempPath(),fileName);
			if(ODEnvironment.IsCloudServer) {
				//file download dialog will come up later for Thinfinity, after file is created.
			}
			else {
				saveFileDialog1.InitialDirectory=Application.StartupPath;
				saveFileDialog1.FileName=fileName;
				if(saveFileDialog1.ShowDialog()!=DialogResult.OK) {
					saveFileDialog1.Dispose();
					return;
				}
				filePath=saveFileDialog1.FileName;
				saveFileDialog1.Dispose();
			}
			using StreamWriter streamWriter=new StreamWriter(filePath,false,System.Text.Encoding.UTF8);
			streamWriter.WriteLine("DELETE FROM languageforeign WHERE Culture='"+CultureInfo.CurrentCulture.Name+"';");
			List<LanguageForeign> listLanguageForeigns=LanguageForeigns.GetListForCurrentCulture();
			for(int i=0;i<listLanguageForeigns.Count;i++){
				streamWriter.WriteLine(
					"INSERT INTO languageforeign (ClassType,English,Culture,Translation,Comments) VALUES ('"+POut.String(listLanguageForeigns[i].ClassType)
					+"', '"+POut.String(listLanguageForeigns[i].English)
					+"', '"+POut.String(listLanguageForeigns[i].Culture)
					+"', '"+POut.String(listLanguageForeigns[i].Translation)
					+"', '"+POut.String(listLanguageForeigns[i].Comments)+"');"
				);
			}//for
			streamWriter.Close();
			if(ODBuild.IsThinfinity()) {
				ThinfinityUtils.ExportForDownload(filePath);
				return;
			}
			else if(ODCloudClient.IsAppStream) {
				CloudClientL.ExportForCloud(filePath);
				return;
			}
			MessageBox.Show("Done");
		}

	}
}
