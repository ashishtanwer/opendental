using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenDentBusiness;
using WpfControls.UI;

namespace OpenDental {
	///<summary></summary>
	public partial class FrmSupplyNeededEdit:FrmODBase {
		public SupplyNeeded SupplyNeededCur;

		public FrmSupplyNeededEdit() {
			InitializeComponent();
			KeyDown+=Frm_KeyDown;
			Load+=FrmSupplyNeededEdit_Load;
			PreviewKeyDown+=FrmSupplyNeededEdit_PreviewKeyDown;
		}

		private void FrmSupplyNeededEdit_Load(object sender,EventArgs e) {
			Lang.F(this);
			textVDate.Text=SupplyNeededCur.DateAdded.ToShortDateString();
			textDescription.Text=SupplyNeededCur.Description;
		}

		private void Frm_KeyDown(object sender,KeyEventArgs e) {
			if(e.Key==Key.Enter) {
				butSave_Click(this,new EventArgs());
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(SupplyNeededCur.IsNew){
				IsDialogOK=false;
			}
			SupplyNeededs.DeleteObject(SupplyNeededCur);
			IsDialogOK=true;
		}

		private void FrmSupplyNeededEdit_PreviewKeyDown(object sender,KeyEventArgs e) {
			if(butsave.IsAltKey(Key.S,e)) {
				butSave_Click(this,new EventArgs());
			}
		}

		private void butSave_Click(object sender,EventArgs e) {
			if(!textVDate.IsValid()){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			SupplyNeededCur.DateAdded=PIn.Date(textVDate.Text);
			SupplyNeededCur.Description=textDescription.Text;
			if(SupplyNeededCur.IsNew) {
				SupplyNeededs.Insert(SupplyNeededCur);
			}
			else {
				SupplyNeededs.Update(SupplyNeededCur);
			}
			IsDialogOK=true;
		}

	}
}