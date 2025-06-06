using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CodeBase;

namespace OpenDental{
	/// <summary></summary>
	public partial class FormLicense : FormODBase {

		private string _selectedLicense;

		///<summary>If selectedLicense is null, then OpenDentalLicense will be the selected license by default. To ensure correct selection, use Properties.Resources.LicenseName to set selectedLicense value.</summary>
		public FormLicense(string selectedLicense=null) {
			InitializeComponent();
			InitializeLayoutManager();
			Lan.F(this);
			_selectedLicense=Properties.Resources.OpenDentalLicense;
			if(!String.IsNullOrWhiteSpace(selectedLicense)){
				_selectedLicense=selectedLicense;
			}
		}

		private void FormLicense_Load(object sender,EventArgs e) {
			FillListBoxLicense();
			for(int i = 0; i < listBoxLicense.Items.Count; i++) {
				if((string)listBoxLicense.Items.GetObjectAt(i)==_selectedLicense) {
					listBoxLicense.SetSelected(i,true);
				}
			}
		}

		///<summary>Fills the listbox with licenses and the license text as a tag. New Licenses needed to be added in FormRegistrationKey.cs as well.</summary>
		private void FillListBoxLicense() {
			listBoxLicense.Items.Add("OpenDental",Properties.Resources.OpenDentalLicense);
			listBoxLicense.Items.Add("OpenDental API End User",Properties.Resources.OpenDentalApiEndUserLicense);
			listBoxLicense.Items.Add("AForge",Properties.Resources.AForge);
			listBoxLicense.Items.Add("Angular",Properties.Resources.Angular);
			listBoxLicense.Items.Add("Bouncy Castle",Properties.Resources.BouncyCastle);
			listBoxLicense.Items.Add("BSD",Properties.Resources.Bsd);
			listBoxLicense.Items.Add("CDT",Properties.Resources.CDT_Content_End_User_License1);
			listBoxLicense.Items.Add("Dropbox",Properties.Resources.Dropbox_Api);
			listBoxLicense.Items.Add("Drifty",Properties.Resources.Ionic);
			listBoxLicense.Items.Add("Mentalis",Properties.Resources.Mentalis);
			listBoxLicense.Items.Add("Microsoft",Properties.Resources.Microsoft);
			listBoxLicense.Items.Add("MigraDoc",Properties.Resources.MigraDoc);
			listBoxLicense.Items.Add("NDde",Properties.Resources.NDde);
			listBoxLicense.Items.Add("Newton Soft",Properties.Resources.NewtonSoft_Json);
			listBoxLicense.Items.Add("Oracle",Properties.Resources.Oracle);
			listBoxLicense.Items.Add("Pearl",Properties.Resources.Pearl);
			listBoxLicense.Items.Add("PDFSharp",Properties.Resources.PdfSharp);
			listBoxLicense.Items.Add("SharpDX",Properties.Resources.SharpDX);
			listBoxLicense.Items.Add("Sparks3D",Properties.Resources.Sparks3D);
			listBoxLicense.Items.Add("SSHNet",Properties.Resources.SshNet);
			listBoxLicense.Items.Add("Stdole",Properties.Resources.stdole);
			listBoxLicense.Items.Add("Tamir",Properties.Resources.Tamir);
			listBoxLicense.Items.Add("Tao_Freeglut",Properties.Resources.Tao_Freeglut);
			listBoxLicense.Items.Add("Tao_OpenGL",Properties.Resources.Tao_OpenGL);
			listBoxLicense.Items.Add("Twain Group",Properties.Resources.Twain);
			listBoxLicense.Items.Add("Zxing",Properties.Resources.Zxing);
		}

		private void listLicense_SelectedIndexChanged(object sender,EventArgs e) {
			textLicense.Text=listBoxLicense.GetSelected<string>();
		}

	}
}