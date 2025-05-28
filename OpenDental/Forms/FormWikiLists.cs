using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormWikiLists:FormODBase {

		public FormWikiLists() {
			InitializeComponent();
			InitializeLayoutManager();
			Lan.F(this);
		}

		private void FormWikiLists_Load(object sender,EventArgs e) {
			FillList();
		}

		private void FillList() {
			listWikiLists.Items.Clear();
			listWikiLists.Items.AddList(WikiLists.GetAllLists(),x => x.Substring(9));
		}

		private void listWikiLists_DoubleClick(object sender,EventArgs e) {
			if(listWikiLists.SelectedIndex==-1) {
				return;
			}
			using FormWikiListEdit formWikiListEdit = new FormWikiListEdit();
			formWikiListEdit.WikiListCurName=listWikiLists.Items.GetTextShowingAt(listWikiLists.SelectedIndex);
			formWikiListEdit.ShowDialog();
			FillList();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(EnumPermType.WikiListSetup)) {
				return;
			}
			InputBox inputBox = new InputBox("New List Name");
			inputBox.ShowDialog();
			if(inputBox.IsDialogCancel) {
				return;
			}
			//Format input as it would be saved in the database--------------------------------------------
			string strResult=inputBox.StringResult.ToLower().Replace(" ","");
			//Validate list name---------------------------------------------------------------------------
			if(DbHelper.isMySQLReservedWord(strResult)) {
				//Can become an issue when retrieving column header names.
				MsgBox.Show(this,"List name is a reserved word in MySQL.");
				return;
			}
			if(strResult=="") {
				MsgBox.Show(this,"List name cannot be blank.");
				return;
			}
			//Mysql table names are limited to specific characters, use a regex to enforce this.
			//^Assert the start of the string which can be a letter or underscore. This is then followed by any number of alphanumeric characters or underscrores through to the end of the string denoted by $.
			Regex regexItem=new Regex("^[a-zA-Z_][a-zA-Z0-9_]*$");
			if(!regexItem.IsMatch(strResult)) {
				MsgBox.Show("List name cannot start with a number and must only contain alphanumeric characters and underscores.");
				return;
			}
			if(WikiLists.CheckExists(strResult)) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"List already exists with that name. Would you like to edit existing list?")) {
					return;
				}
			}
			using FormWikiListEdit formWikiListEdit = new FormWikiListEdit();
			formWikiListEdit.WikiListCurName = strResult;
			//FormWLE.IsNew=true;//set within the form.
			formWikiListEdit.ShowDialog();
			FillList();
		}
	}
}