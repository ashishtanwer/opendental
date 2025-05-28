using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	public partial class FrmCommItemUserPrefs:FrmODBase {
		///<summary>Helper variable that gets set to Security.CurUser.UserNum on load.</summary>
		private long _userNum;
		private UserOdPref _userOdPrefClearNote;
		private UserOdPref _userOdPrefEndDate;
		private UserOdPref _userOdPrefUpdateDateTimeNewPat;

		public FrmCommItemUserPrefs() {
			InitializeComponent();
			this.Load+=FrmCommItemUserPrefs_Load;
			PreviewKeyDown+=FrmCommItemUserPrefs_PreviewKeyDown;
		}

		private void FrmCommItemUserPrefs_Load(object sender,EventArgs e) {
			Lang.F(this);
			if(Security.CurUser==null || Security.CurUser.UserNum < 1) {
				MsgBox.Show(this,"Invalid user currently logged in.  No user preferences can be saved.");
				IsDialogOK=false;
				return;
			}
			_userNum=Security.CurUser.UserNum;
			//Add the user name of the user currently logged in to the title of this window much like we do for FormOpenDental.
			this.Text+=" {"+Security.CurUser.UserName+"}";
			_userOdPrefClearNote=UserOdPrefs.GetByUserAndFkeyType(_userNum,UserOdFkeyType.CommlogPersistClearNote).FirstOrDefault();
			_userOdPrefEndDate=UserOdPrefs.GetByUserAndFkeyType(_userNum,UserOdFkeyType.CommlogPersistClearEndDate).FirstOrDefault();
			_userOdPrefUpdateDateTimeNewPat=UserOdPrefs.GetByUserAndFkeyType(_userNum,UserOdFkeyType.CommlogPersistUpdateDateTimeWithNewPatient).FirstOrDefault();
			checkCommlogPersistClearNote.Checked=true;
			checkCommlogPersistClearEndDate.Checked=true;
			checkCommlogPersistUpdateDateTimeWithNewPatient.Checked=true;
			if(_userOdPrefClearNote!=null) {
				checkCommlogPersistClearNote.Checked=PIn.Bool(_userOdPrefClearNote.ValueString);
			}
			if(_userOdPrefEndDate!=null) {
				checkCommlogPersistClearEndDate.Checked=PIn.Bool(_userOdPrefEndDate.ValueString);
			}
			if(_userOdPrefUpdateDateTimeNewPat!=null) {
				checkCommlogPersistUpdateDateTimeWithNewPatient.Checked=PIn.Bool(_userOdPrefUpdateDateTimeNewPat.ValueString);
			}
		}

		///<summary>Helper method to update or insert the passed in UserOdPref utilizing the specified valueString and keyType.
		///If the user pref passed in it null then a new user pref will be inserted.  Otherwise the user pref is updated.</summary>
		private void UpsertUserOdPref(UserOdPref userOdPref,UserOdFkeyType userOdFkeyType,string valueString) {
			if(userOdPref==null) {
				UserOdPref userOdPrefTemp=new UserOdPref();
				userOdPrefTemp.Fkey=0;
				userOdPrefTemp.FkeyType=userOdFkeyType;
				userOdPrefTemp.UserNum=_userNum;
				userOdPrefTemp.ValueString=valueString;
				UserOdPrefs.Insert(userOdPrefTemp);
				DataValid.SetInvalid(InvalidType.UserOdPrefs);
			}
			else {
				UserOdPref userOdPrefOld=userOdPref.Clone();
				userOdPref.FkeyType=userOdFkeyType;
				userOdPref.ValueString=valueString;
				if(UserOdPrefs.Update(userOdPref,userOdPrefOld)) {
					//Only need to signal cache refresh on change.
					DataValid.SetInvalid(InvalidType.UserOdPrefs);
				}
			}
		}

		private void FrmCommItemUserPrefs_PreviewKeyDown(object sender,KeyEventArgs e) {
			if(butSave.IsAltKey(Key.S,e)) {
				butSave_Click(this,new EventArgs());
			}
		}

		private void butSave_Click(object sender,EventArgs e) {
			UpsertUserOdPref(_userOdPrefClearNote
				,UserOdFkeyType.CommlogPersistClearNote
				,POut.Bool(checkCommlogPersistClearNote.Checked==true));
			UpsertUserOdPref(_userOdPrefEndDate
				,UserOdFkeyType.CommlogPersistClearEndDate
				,POut.Bool(checkCommlogPersistClearEndDate.Checked==true));
			UpsertUserOdPref(_userOdPrefUpdateDateTimeNewPat
				,UserOdFkeyType.CommlogPersistUpdateDateTimeWithNewPatient
				,POut.Bool(checkCommlogPersistUpdateDateTimeWithNewPatient.Checked==true));
			IsDialogOK=true;
		}

	}
}