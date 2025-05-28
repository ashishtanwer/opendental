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
	/// <summary> </summary>
	public partial class FrmQuickPasteCat : FrmODBase{
		public QuickPasteCat QuickPasteCatCur;

		///<summary>The QuickPasteCat passed into this constructor is directly manipulated.</summary>
		public FrmQuickPasteCat(QuickPasteCat quickPasteCat){
			QuickPasteCatCur=quickPasteCat.Copy();
			InitializeComponent();
			Load+=FrmQuickPasteCat_Load;
			PreviewKeyDown+=FrmQuickPasteCat_PreviewKeyDown;
		}

		private void FrmQuickPasteCat_Load(object sender, EventArgs e) {
			Lang.F(this);
			listType.Items.Clear();
			string[] stringArrayTypes;
			if(QuickPasteCatCur.DefaultForTypes==null
				|| QuickPasteCatCur.DefaultForTypes==""){
				stringArrayTypes=new string[0];
			}
			else{
				stringArrayTypes=QuickPasteCatCur.DefaultForTypes.Split(',');
			}
			for(int i=0;i<Enum.GetNames(typeof(EnumQuickPasteType)).Length;i++){
				if((i==(int)EnumQuickPasteType.WebChat || 
					i==(int)EnumQuickPasteType.ProviderSearchFilter ||
					i==(int)EnumQuickPasteType.JobManager ||
					i==(int)EnumQuickPasteType.PhoneEmpDefaultStatus||
					i==(int)EnumQuickPasteType.FAQ)
					&& !PrefC.IsODHQ) 
				{
					continue;
				}
				listType.Items.Add(Enum.GetNames(typeof(EnumQuickPasteType))[i],(EnumQuickPasteType)i);
			}
			for(int i=0;i<stringArrayTypes.Length;i++) {
				if(listType.Items.Contains((EnumQuickPasteType)PIn.Int(stringArrayTypes[i]))) {
					listType.SetSelectedEnum((EnumQuickPasteType)PIn.Int(stringArrayTypes[i]));
				}
			}
			textDescription.Text=QuickPasteCatCur.Description;
			textDescription.SelectAll();
		}

		private void FrmQuickPasteCat_PreviewKeyDown(object sender,KeyEventArgs e) {
			if(butSave.IsAltKey(Key.S,e)) {
				butSave_Click(this,new EventArgs());
			}
		}

		private void butSave_Click(object sender, System.EventArgs e) {
			if(textDescription.Text=="") {
				MsgBox.Show(this,"Description cannot be blank");
				return;
			}
			QuickPasteCatCur.Description=textDescription.Text;
			QuickPasteCatCur.DefaultForTypes="";
			QuickPasteCatCur.DefaultForTypes=string.Join(",",listType.GetListSelected<int>());
			IsDialogOK=true;
		}

	}
}