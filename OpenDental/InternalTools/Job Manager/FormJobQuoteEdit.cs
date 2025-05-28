using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormJobQuoteEdit:FormODBase {
		private JobQuote _jobQuote;

		///<summary>Used for existing Reviews. Pass in the jobNum and the jobReviewNum.</summary>
		public FormJobQuoteEdit(JobQuote jobQuote) {
			_jobQuote=jobQuote.Copy();
			InitializeComponent();
			InitializeLayoutManager();
			Lan.F(this);
		}

		public JobQuote JobQuoteCur {
			get {
				return _jobQuote;
			}
		}

		private void FormJobQuoteEdit_Load(object sender,EventArgs e) {
			//If you're not authorized for quotes and you're not a query coordinator for a query job
			if(!JobPermissions.IsAuthorized(JobPerm.Quote,true) && 
				!(Jobs.GetOne(_jobQuote.JobNum).Category==JobCategory.Query && JobPermissions.HasQueryPermission(JobPerm.QueryCoordinator))) 
			{
				textAmount.ReadOnly=true;
				textApprovedAmount.ReadOnly=true;
				textQuoteHours.ReadOnly=true;
				textNote.ReadOnly=true;
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
			if(_jobQuote.PatNum>0) {
				Patient pat=Patients.GetPat(_jobQuote.PatNum);
				if(pat!=null) {
					textPatient.Text=pat.GetNameFL();
				}
				else {
					textPatient.Text="Missing Patient - "+_jobQuote.PatNum;
				}
			}
			textNote.Text=_jobQuote.Note;
			textAmount.Text=_jobQuote.Amount;
			textApprovedAmount.Text=_jobQuote.ApprovedAmount;
			textQuoteHours.Text=_jobQuote.Hours;
			checkIsApproved.Checked=_jobQuote.IsCustomerApproved;
		}

		private void butPatPicker_Click(object sender,EventArgs e) {
			FrmPatientSelect frmPatientSelect=new FrmPatientSelect();
			if(_jobQuote.PatNum!=0) {
				frmPatientSelect.ListPatNumsExplicit=new List<long> {_jobQuote.PatNum};
			}
			frmPatientSelect.ShowDialog();
			if(frmPatientSelect.IsDialogCancel) {
				return;
			}
			Patient pat=Patients.GetPat(frmPatientSelect.PatNumSelected);
			if(pat!=null) {
				_jobQuote.PatNum=pat.PatNum;
				textPatient.Text=pat.GetNameFL();
			}
			else {
				_jobQuote.PatNum=0;
				textPatient.Text="Missing Patient - "+_jobQuote.PatNum;
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Delete the current job quote?")) {
				return;
			}
			_jobQuote=null;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			_jobQuote.Note=textNote.Text;
			_jobQuote.Amount=PIn.Double(textAmount.Text).ToString("F");
			_jobQuote.Hours=textQuoteHours.Text;
			_jobQuote.ApprovedAmount=PIn.Double(textApprovedAmount.Text).ToString("F");
			_jobQuote.IsCustomerApproved=checkIsApproved.Checked;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel; //removing new jobs from the DB is taken care of in FormClosing
		}

	}
}