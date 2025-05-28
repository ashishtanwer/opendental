using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenDentBusiness;
using WpfControls.UI;
using CodeBase;
using OpenDental.Drawing;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace OpenDental {
	/// <summary></summary>
	public partial class FrmEFormFillEdit : FrmODBase {
		#region Fields
		///<summary>This is the object we are editing.</summary>
		public EForm EFormCur;
		private bool _isLoaded;
		///<summary>Used to keep track of what masked SSN was shown when the form was loaded, and stop us from storing masked SSNs on accident.</summary>
		private string _maskedSSNOld;
		///<summary>When this form opens or after any save to db, this list gets reset as a copy of the fields. This allows us to compare to see if anything changed before saving.</summary>
		private List<EFormField> _listEFormFieldsOld;
		//these three fields are the result for TryToSaveData().
		private bool _wasError;
		private bool _wasUnchanged;
		private bool _wasSaved;
		#endregion Fields

		#region Constructor
		///<summary></summary>
		public FrmEFormFillEdit() {
			InitializeComponent();
			Load+=FrmEFormFillEdit_Load;
			SizeChanged+=FrmEFormFillEdit_SizeChanged;
			FormClosing+=FrmEFormFillEdit_FormClosing;
		}
		#endregion Constructor

		#region Methods - event handlers
		private void FrmEFormFillEdit_Load(object sender, EventArgs e) {
			_listEFormFieldsOld=EFormFields.GetDeepCopy(EFormCur.ListEFormFields);
			Lang.F(this);
			ctrlEFormFill.ListEFormFields=EFormCur.ListEFormFields;//two references to same list of objects
			ctrlEFormFill.ShowLabelsBold=EFormCur.ShowLabelsBold;
			ctrlEFormFill.SpaceBelowEachField=EFormCur.SpaceBelowEachField;
			ctrlEFormFill.SpaceToRightEachField=EFormCur.SpaceToRightEachField;
			ctrlEFormFill.Zoom=GetZoom();
			ctrlEFormFill.RefreshLayout();
			_maskedSSNOld=EFormCur.ListEFormFields.Find(x=>x.DbLink=="SSN")?.ValueString;//null is ok
			textDescription.Text=EFormCur.Description;
			textDateTime.Text=EFormCur.DateTimeShown.ToShortDateString()+" "+EFormCur.DateTimeShown.ToShortTimeString();
			if(!EFormCur.ListEFormFields.Exists(x=>x.IsRequired)){
				labelRequired.Visible=false;
			}
			if(EFormCur.SaveImageCategory==0){
				groupSaveImages.Visible=false;
			}
			else{
				List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ImageCats,isShort:true);
				comboImageCat.Items.AddDefNone();
				comboImageCat.Items.AddDefs(listDefs);
				comboImageCat.SetSelectedDefNum(EFormCur.SaveImageCategory);
			}
			bool isSigned=false;
			for(int i=0;i<EFormCur.ListEFormFields.Count;i++) {
				if(EFormCur.ListEFormFields[i].FieldType.In(EnumEFormFieldType.SigBox)
					&& EFormCur.ListEFormFields[i].ValueString.Length>1) 
				{
					isSigned=true;
					break;
				}
			}
			if(isSigned) {
				ctrlEFormFill.IsReadOnly=true;
			}
			else{
				butUnlockSig.Visible=false;
			}
			if(EFormCur.PatNum!=0 && !Security.IsAuthorized(EnumPermType.EFormDelete,EFormCur.DateTimeShown,suppressMessage:true,suppressLockDateMessage:true)) {
				butDelete.IsEnabled=false;
			}
			_isLoaded=true;
			SetCtrlWidth();
		}

		private void FrmEFormFillEdit_SizeChanged(object sender,System.Windows.SizeChangedEventArgs e) {
			SetCtrlWidth();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete entire form?")){
				return;
			}
			if(EFormCur.IsNew){
				IsDialogCancel=true;
				return;
			}
			EFormFields.DeleteForForm(EFormCur.EFormNum);
			EForms.Delete(EFormCur.EFormNum,EFormCur.PatNum);
			//There is no need to send any signal to calling form that user deleted.
			IsDialogOK=true;
		}

		//private void butSaveImport_Click(object sender, EventArgs e) {
		//	if(!ValidateAndSave()){
		//		return;
		//	}
		//	EFormCur.ListEFormFields=ListEFormFields;
		//	EFormImport.Import(EFormCur);
		//	IsDialogOK=true;
		//}

		private void butPrint_Click(object sender,EventArgs e) {
			//Unlike sheets, we have no need to save first because we are just printing whatever is on the screen at the moment.
			ctrlEFormFill.FillFieldsFromControls();//so that RefreshLayout will work
			Printout printout=new Printout();
			printout.FuncPrintPage=ctrlEFormFill.Pd_PrintPage;
			printout.thicknessMarginInches=new Thickness(0.5);
			ctrlEFormFill.PagesPrinted=0;
			WpfControls.PrinterL.TryPrintOrDebugClassicPreview(printout);
			ctrlEFormFill.RefreshLayout();//todo: have this run while the preview window is still open?
		}

		private void butUnlockSig_Click(object sender,EventArgs e) {
			ctrlEFormFill.IsReadOnly=false;
			butUnlockSig.Visible=false;
		}

		private void butChangePat_Click(object sender,EventArgs e) {
			FrmPatientSelect frmPatientSelect=new FrmPatientSelect();
			frmPatientSelect.ShowDialog();
			if(frmPatientSelect.IsDialogCancel) {
				return;
			}
			SecurityLogs.MakeLogEntry(EnumPermType.EFormEdit,EFormCur.PatNum,Lang.g(this,"eForm with ID")+" "+EFormCur.EFormNum+" "+Lang.g(this,"moved to PatNum")+" "+frmPatientSelect.PatNumSelected);
			SecurityLogs.MakeLogEntry(EnumPermType.EFormEdit,frmPatientSelect.PatNumSelected,Lang.g(this,"eForm with ID")+" "+EFormCur.EFormNum+" "+Lang.g(this,"moved from PatNum")+" "+EFormCur.PatNum);
			EFormCur.PatNum=frmPatientSelect.PatNumSelected;
		}

		private void butEClipboard_Click(object sender,EventArgs e) {
			EFormCur.Status=EnumEFormStatus.ShowInEClipboard;
			TryToSaveData();
			if(_wasError){
				return;
			}
			MobileNotifications.CI_AddEForm(EFormCur.PatNum,EFormCur.EFormNum);//tells eClipboard to pull the eForm
			SecurityLogs.MakeLogEntry(EnumPermType.EFormEdit,EFormCur.PatNum,EFormCur.Description+" from "+EFormCur.DateTimeShown.ToShortDateString());
			IsDialogOK=true;
		}

		private void butSave_Click(object sender,EventArgs e) {
			TryToSaveData();
			if(_wasError){
				return;
			}
			IsDialogOK=true;
		}

		private void butSaveImage_Click(object sender,EventArgs e) {
			TryToSaveData();//includes whatever's in the combo
			if(_wasError){
				return;
			}
			if(EFormCur.SaveImageCategory==0){
				MsgBox.Show(this,"Please pick a category first.");
				return;
			}
			Cursor=Cursors.Wait;
			ctrlEFormFill.FillFieldsFromControls();//so that RefreshLayout will work
			Printout printout=new Printout();
			printout.FuncPrintPage=ctrlEFormFill.Pd_PrintPage;
			printout.thicknessMarginInches=new Thickness(0.5);
			ctrlEFormFill.PagesPrinted=0;
			WpfControls.PrinterL.CreateFixedDocument(printout);//the document is on the printout
			ctrlEFormFill.RefreshLayout();
			Patient patient=Patients.GetPat(EFormCur.PatNum);
			string tempFilePath=PrefC.GetRandomTempFile(".pdf");
			PdfDocument pdfDocument=new PdfDocument();
			for(int i=0;i<printout.FixedDocument_.Pages.Count;i++){
				PageContent pageContent=printout.FixedDocument_.Pages[i];
				FixedPage fixedPage=pageContent.Child;
				fixedPage.Measure(new Size(fixedPage.Width, fixedPage.Height));
				fixedPage.Arrange(new Rect(new Size(fixedPage.Width, fixedPage.Height)));
				fixedPage.UpdateLayout();
				double dpi=200;
				RenderTargetBitmap renderTarget = new RenderTargetBitmap(
					(int)(fixedPage.ActualWidth*dpi/96),(int)(fixedPage.ActualHeight*dpi/96),dpi,dpi,PixelFormats.Pbgra32);
				renderTarget.Render(fixedPage);
				BitmapSource bitmapSource=renderTarget;
				string tempImgFile=Path.GetTempFileName()+".png";
				using(FileStream fileStream = new FileStream(tempImgFile,FileMode.Create)) {
					PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
					pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
					pngBitmapEncoder.Save(fileStream);
				}
				PdfPage pdfPage =new PdfPage();
				pdfDocument.AddPage(pdfPage);
				using XGraphics xGraphics=XGraphics.FromPdfPage(pdfPage);
				XImage xImage=XImage.FromFile(tempImgFile);
				xGraphics.DrawImage(xImage,0,0,pdfPage.Width,pdfPage.Height);
			}
			pdfDocument.Save(tempFilePath);
			Document document=ImageStore.Import(tempFilePath,EFormCur.SaveImageCategory,patient);
			Cursor=Cursors.Arrow;
			MsgBox.Show(this,"Done");
		}

		private void FrmEFormFillEdit_FormClosing(object sender,CancelEventArgs e) {
			for(int i=0;i<EFormCur.ListEFormFields.Count;i++){
				if(EFormCur.ListEFormFields[i].FieldType!=EnumEFormFieldType.SigBox){
					continue;
				}
				System.Windows.Controls.Border borderBox=EFormCur.ListEFormFields[i].TagOD as System.Windows.Controls.Border;
				System.Windows.Controls.StackPanel stackPanel=borderBox.Child as System.Windows.Controls.StackPanel;
				SignatureBoxWrapper signatureBoxWrapper=stackPanel.Children[1] as SignatureBoxWrapper;
				signatureBoxWrapper?.SetTabletState(0);
			}
		}
		#endregion Methods - event handlers

		#region Methods - private
//todo: This will be called each time we change any field
		private void ClearSigs(){
			for(int i=0;i<EFormCur.ListEFormFields.Count;i++){
				if(EFormCur.ListEFormFields[i].FieldType!=EnumEFormFieldType.SigBox){
					continue;
				}
				//todo:
				//When any field values change, the user is purposefully "invalidating" the old signature. They can resign.
				//But maybe only clear it if it was signed when this form was opened.
				//If it was unsigned when opened, they should be able to sign in any order.
				//Or if they signed it again since opening, that should count too.
				//So maybe the boolean should be if it was signed this session.
				//Use event, and use FrmCommItem as an example.
				//sigBox.ClearSignature(clearTopazTablet:false);
			}
		}

		private void SetCtrlWidth(){
			//This only sets control width, not width of form.
			//We let it grow as much as possible, limited by max width and by space available.
			if(!_isLoaded){
				return;
			}
			int maxWidth=EFormCur.MaxWidth;//no validation of range needed here
			maxWidth+=17+2;//scrollbar plus border width
			int avail=(int)ActualWidth-(int)ctrlEFormFill.Margin.Left-147;//147 is our chosen right margin
			if(maxWidth>avail){
				maxWidth=avail;
			}
			if(maxWidth<50){
				maxWidth=50;
				//if window gets extremely narrow, this control might spill out to the right
			}
			ctrlEFormFill.Width=maxWidth;
			ctrlEFormFill.UpdateLayout();
			ctrlEFormFill.FillFieldsFromControls();
			ctrlEFormFill.RefreshLayout();
			//The one thing this doesn't do perfectly is
			//if already signed in this session, then size changed, then change text,
			//signature disappears when it shouldn't. 
			//I can live with that. User can just sign again. Why would they be resizing that much anyway?
		}

		///<summary>3 variables track the result.</summary>
		private void TryToSaveData() {
			_wasError=false;
			_wasUnchanged=false;
			_wasSaved=false;
			ctrlEFormFill.FillFieldsFromControls();
			if(ODBuild.IsDebug() && Environment.MachineName.ToLower()=="jordanhome"){
				//This is how we test required fields in OD proper.
				//There are certainly other ways of doing it.
				//Med list was tricky because that checkbox is not actually present in the filled field.
				EFormValidation eFormValidationR=EForms.ValidateRequired(EFormCur,ctrlEFormFill.GetMedListIsNoneChecked());
				if(eFormValidationR.ErrorMsg!="") {
					ctrlEFormFill.SetVisibilities(eFormValidationR.PageNum);
					MsgBox.Show(eFormValidationR.ErrorMsg);
					_wasError=true;
					return;
				}
			}
			//Next line no longer enforces required fields. That should only be done in eClipboard, not OD proper.
			EFormValidation eFormValidation = EForms.Validate(EFormCur,_maskedSSNOld);
			if(eFormValidation.ErrorMsg!="") {
				ctrlEFormFill.SetVisibilities(eFormValidation.PageNum);
				MsgBox.Show(eFormValidation.ErrorMsg);
				_wasError=true;
				return;
			}
			DateTime dateTimeShown=DateTime.MinValue;
			try {
				dateTimeShown=DateTime.Parse(textDateTime.Text);
			}
			catch(Exception) {
				MsgBox.Show(this,"Please fix data entry errors first.");
				_wasError=true;
				return;
			}
			long saveImageCategory=0;
			if(groupSaveImages.Visible){
				saveImageCategory=comboImageCat.GetSelectedDefNum();
			}
			//End of validation.
			//Test to see if any changes were made.
			bool isChanged=false;
			if(EFormCur.IsNew){
				isChanged=true;
			}
			if(EFormCur.Description!=textDescription.Text
				|| EFormCur.DateTimeShown!=dateTimeShown
				|| EFormCur.SaveImageCategory!=saveImageCategory)
			{
				isChanged=true;
			}
			isChanged|=EFormFields.IsAnyChanged(EFormCur.ListEFormFields,_listEFormFieldsOld);
			if(!isChanged){
				_wasUnchanged=true;
				return;
			}
			_listEFormFieldsOld=EFormFields.GetDeepCopy(EFormCur.ListEFormFields);
			//from here down we will actually save
			_wasSaved=true;
			EFormCur.Description=textDescription.Text;
			EFormCur.SaveImageCategory=saveImageCategory;
			if(EFormCur.IsNew){
				EFormCur.DateTimeShown=DateTime.Now;//instead of what user might have typed in
				//DateTEdited gets set automatically for insert
				EForms.Insert(EFormCur);
				EFormCur.IsNew=false;//because we frequently stay in this form
				for(int i=0;i<EFormCur.ListEFormFields.Count;i++) {
					EFormCur.ListEFormFields[i].EFormNum=EFormCur.EFormNum;
					EFormCur.ListEFormFields[i].ItemOrder=i;
					EFormFields.Insert(EFormCur.ListEFormFields[i]);
				}
			}
			else{
				EFormCur.DateTimeShown=dateTimeShown;
				EFormCur.DateTEdited=DateTime.Now;
				EForms.Update(EFormCur);
			}
			//Synching here will be very easy compared to other places because user can't delete, add, or reorder. It's all just simple edits.
			//Could do this in a number of different ways.
			//Decided in this case that the simplest approach is to just
			//delete all of the fields that are on the form from the database and re-insert them.
			EFormFields.DeleteForForm(EFormCur.EFormNum);
			for(int i=0;i<EFormCur.ListEFormFields.Count;i++) {
				EFormCur.ListEFormFields[i].EFormNum=EFormCur.EFormNum;
				EFormCur.ListEFormFields[i].ItemOrder=i;
				EFormFields.Insert(EFormCur.ListEFormFields[i]);//ignores any existing PK when inserting
			}
			SecurityLogs.MakeLogEntry(EnumPermType.EFormEdit,EFormCur.PatNum,"EForm: "+EFormCur.Description+" from "+EFormCur.DateTimeShown.ToShortDateString());
		}

		#endregion Methods - private

		
	}
}