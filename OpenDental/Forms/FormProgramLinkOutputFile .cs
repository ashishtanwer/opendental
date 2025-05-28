using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormProgramLinkOutputFile:FormODBase {

		private Program _program;

		public FormProgramLinkOutputFile(Program program) {
			InitializeComponent();
			InitializeLayoutManager();
			Lan.F(this);
			_program=program;
		}

		private void FormProgramLinkOutputFile_Load(object sender,EventArgs e) {
			textTemplate.Text=_program.FileTemplate;
			textPath.Text=_program.FilePath;
		}

		private void butBrowse_Click(object sender,EventArgs e) {
			string[] stringArrayFileNames;
			if(!ODBuild.IsThinfinity() && ODCloudClient.IsAppStream) {
				stringArrayFileNames=new string[]{ODCloudClient.GetFileFolderPath()};
				if(stringArrayFileNames[0].IsNullOrEmpty()) {
					return;
				}
			}
			else {
				OpenFileDialog openFileDialog=new OpenFileDialog();
				if(openFileDialog.ShowDialog()!=DialogResult.OK) {
					return;
				}
				stringArrayFileNames=openFileDialog.FileNames;
			}
			if(stringArrayFileNames.Length<1) {
				return;
			}
			textPath.Text=stringArrayFileNames[0];
		}

		private void butSave_Click(object sender,EventArgs e) {
			_program.FilePath=textPath.Text;
			_program.FileTemplate=textTemplate.Text;
			DialogResult=DialogResult.OK;
		}

		private void butReplacements_Click(object sender,EventArgs e) {
			List<MessageReplaceType> listMessageReplaceTypes=new List<MessageReplaceType>();
			listMessageReplaceTypes.Add(MessageReplaceType.Patient);
			listMessageReplaceTypes.Add(MessageReplaceType.Referral);
			listMessageReplaceTypes.Add(MessageReplaceType.Misc);
			FrmMessageReplacements frmMessageReplacements=new FrmMessageReplacements(listMessageReplaceTypes);
			frmMessageReplacements.IsSelectionMode=true;
			frmMessageReplacements.ShowDialog();
			if(frmMessageReplacements.IsDialogCancel) {
				return;
			}
			textTemplate.Focus();
			int indexCursor=textTemplate.SelectionStart;
			textTemplate.Text=textTemplate.Text.Insert(indexCursor,frmMessageReplacements.ReplacementTextSelected);
			textTemplate.SelectionStart=indexCursor+frmMessageReplacements.ReplacementTextSelected.Length;
		}

	}
}