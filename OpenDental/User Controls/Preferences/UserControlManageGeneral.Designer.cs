﻿
namespace OpenDental {
	partial class UserControlManageGeneral {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
		if(disposing && (components != null)) {
		components.Dispose();
		}
		base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.checkScheduleProvEmpSelectAll = new OpenDental.UI.CheckBox();
			this.checkAccountingInvoiceAttachmentsSaveInDatabase = new OpenDental.UI.CheckBox();
			this.groupBoxTimeCards = new OpenDental.UI.GroupBox();
			this.comboTimeCardOvertimeFirstDayOfWeek = new OpenDental.UI.ComboBox();
			this.checkTimeCardUseLocal = new OpenDental.UI.CheckBox();
			this.label16 = new System.Windows.Forms.Label();
			this.checkTimeCardADP = new OpenDental.UI.CheckBox();
			this.checkClockEventAllowBreak = new OpenDental.UI.CheckBox();
			this.groupBoxDeposits = new OpenDental.UI.GroupBox();
			this.comboDepositSoftware = new OpenDental.UI.ComboBox();
			this.checkShowAutoDeposit = new OpenDental.UI.CheckBox();
			this.labelDepositSoftware = new System.Windows.Forms.Label();
			this.groupBoxPrescriptions = new OpenDental.UI.GroupBox();
			this.checkRxHideProvsWithoutDEA = new OpenDental.UI.CheckBox();
			this.groupBoxClaims = new OpenDental.UI.GroupBox();
			this.textClaimsReceivedDays = new OpenDental.ValidNum();
			this.checkClaimsSendWindowValidateOnLoad = new OpenDental.UI.CheckBox();
			this.labelClaimsReceivedDays = new System.Windows.Forms.Label();
			this.checkClaimPaymentBatchOnly = new OpenDental.UI.CheckBox();
			this.groupBoxERA = new OpenDental.UI.GroupBox();
			this.comboEraWriteoff = new OpenDental.UI.ComboBox();
			this.textEraNoAutoProcessCarcCodes = new System.Windows.Forms.TextBox();
			this.comboEraDefaultPaymentType = new OpenDental.UI.ComboBox();
			this.comboFwtPaymentType = new OpenDental.UI.ComboBox();
			this.comboAchPaymentType = new OpenDental.UI.ComboBox();
			this.comboEraCheckPaymentType = new OpenDental.UI.ComboBox();
			this.comboEraAutomation = new OpenDental.UI.ComboBox();
			this.labelWriteOffs = new System.Windows.Forms.Label();
			this.labelEraNoAutoProcessCarcCodes = new System.Windows.Forms.Label();
			this.labelERADefault = new System.Windows.Forms.Label();
			this.labelFWT = new System.Windows.Forms.Label();
			this.labelACH = new System.Windows.Forms.Label();
			this.labelCHK = new System.Windows.Forms.Label();
			this.checkEraOneClaimPerPage = new OpenDental.UI.CheckBox();
			this.checkIncludeEraWOPercCoPay = new OpenDental.UI.CheckBox();
			this.labelEraAutomation = new System.Windows.Forms.Label();
			this.checkEraAllowTotalPayment = new OpenDental.UI.CheckBox();
			this.groupBoxTimeCards.SuspendLayout();
			this.groupBoxDeposits.SuspendLayout();
			this.groupBoxPrescriptions.SuspendLayout();
			this.groupBoxClaims.SuspendLayout();
			this.groupBoxERA.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkScheduleProvEmpSelectAll
			// 
			this.checkScheduleProvEmpSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkScheduleProvEmpSelectAll.Location = new System.Drawing.Point(39, 610);
			this.checkScheduleProvEmpSelectAll.Name = "checkScheduleProvEmpSelectAll";
			this.checkScheduleProvEmpSelectAll.Size = new System.Drawing.Size(421, 17);
			this.checkScheduleProvEmpSelectAll.TabIndex = 201;
			this.checkScheduleProvEmpSelectAll.Text = "Automatically select all providers/employees when loading schedules";
			// 
			// checkAccountingInvoiceAttachmentsSaveInDatabase
			// 
			this.checkAccountingInvoiceAttachmentsSaveInDatabase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAccountingInvoiceAttachmentsSaveInDatabase.Location = new System.Drawing.Point(25, 632);
			this.checkAccountingInvoiceAttachmentsSaveInDatabase.Name = "checkAccountingInvoiceAttachmentsSaveInDatabase";
			this.checkAccountingInvoiceAttachmentsSaveInDatabase.Size = new System.Drawing.Size(435, 17);
			this.checkAccountingInvoiceAttachmentsSaveInDatabase.TabIndex = 308;
			this.checkAccountingInvoiceAttachmentsSaveInDatabase.Text = "Save accounting invoice attachments in database";
			// 
			// groupBoxTimeCards
			// 
			this.groupBoxTimeCards.Controls.Add(this.comboTimeCardOvertimeFirstDayOfWeek);
			this.groupBoxTimeCards.Controls.Add(this.checkTimeCardUseLocal);
			this.groupBoxTimeCards.Controls.Add(this.label16);
			this.groupBoxTimeCards.Controls.Add(this.checkTimeCardADP);
			this.groupBoxTimeCards.Controls.Add(this.checkClockEventAllowBreak);
			this.groupBoxTimeCards.Location = new System.Drawing.Point(20, 508);
			this.groupBoxTimeCards.Name = "groupBoxTimeCards";
			this.groupBoxTimeCards.Size = new System.Drawing.Size(450, 100);
			this.groupBoxTimeCards.TabIndex = 307;
			this.groupBoxTimeCards.Text = "Time Cards";
			// 
			// comboTimeCardOvertimeFirstDayOfWeek
			// 
			this.comboTimeCardOvertimeFirstDayOfWeek.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboTimeCardOvertimeFirstDayOfWeek.Location = new System.Drawing.Point(270, 12);
			this.comboTimeCardOvertimeFirstDayOfWeek.Name = "comboTimeCardOvertimeFirstDayOfWeek";
			this.comboTimeCardOvertimeFirstDayOfWeek.Size = new System.Drawing.Size(170, 21);
			this.comboTimeCardOvertimeFirstDayOfWeek.TabIndex = 198;
			// 
			// checkTimeCardUseLocal
			// 
			this.checkTimeCardUseLocal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTimeCardUseLocal.Location = new System.Drawing.Point(20, 77);
			this.checkTimeCardUseLocal.Name = "checkTimeCardUseLocal";
			this.checkTimeCardUseLocal.Size = new System.Drawing.Size(420, 20);
			this.checkTimeCardUseLocal.TabIndex = 296;
			this.checkTimeCardUseLocal.Text = "Time Cards use local time";
			// 
			// label16
			// 
			this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label16.Location = new System.Drawing.Point(51, 15);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(218, 17);
			this.label16.TabIndex = 202;
			this.label16.Text = "Time Card first day of week for overtime";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkTimeCardADP
			// 
			this.checkTimeCardADP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkTimeCardADP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTimeCardADP.Location = new System.Drawing.Point(81, 35);
			this.checkTimeCardADP.Name = "checkTimeCardADP";
			this.checkTimeCardADP.Size = new System.Drawing.Size(359, 17);
			this.checkTimeCardADP.TabIndex = 199;
			this.checkTimeCardADP.Text = "ADP export includes employee name";
			// 
			// checkClockEventAllowBreak
			// 
			this.checkClockEventAllowBreak.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkClockEventAllowBreak.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClockEventAllowBreak.Location = new System.Drawing.Point(19, 56);
			this.checkClockEventAllowBreak.Name = "checkClockEventAllowBreak";
			this.checkClockEventAllowBreak.Size = new System.Drawing.Size(421, 17);
			this.checkClockEventAllowBreak.TabIndex = 295;
			this.checkClockEventAllowBreak.Text = "Allow paid 30 minute breaks";
			// 
			// groupBoxDeposits
			// 
			this.groupBoxDeposits.Controls.Add(this.comboDepositSoftware);
			this.groupBoxDeposits.Controls.Add(this.checkShowAutoDeposit);
			this.groupBoxDeposits.Controls.Add(this.labelDepositSoftware);
			this.groupBoxDeposits.Location = new System.Drawing.Point(20, 437);
			this.groupBoxDeposits.Name = "groupBoxDeposits";
			this.groupBoxDeposits.Size = new System.Drawing.Size(450, 64);
			this.groupBoxDeposits.TabIndex = 306;
			this.groupBoxDeposits.Text = "Deposits";
			// 
			// comboDepositSoftware
			// 
			this.comboDepositSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboDepositSoftware.Location = new System.Drawing.Point(310, 33);
			this.comboDepositSoftware.Name = "comboDepositSoftware";
			this.comboDepositSoftware.Size = new System.Drawing.Size(130, 21);
			this.comboDepositSoftware.TabIndex = 298;
			this.comboDepositSoftware.SelectionChangeCommitted += new System.EventHandler(this.comboDepositSoftware_SelectionChangeCommitted);
			// 
			// checkShowAutoDeposit
			// 
			this.checkShowAutoDeposit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowAutoDeposit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAutoDeposit.Location = new System.Drawing.Point(90, 10);
			this.checkShowAutoDeposit.Name = "checkShowAutoDeposit";
			this.checkShowAutoDeposit.Size = new System.Drawing.Size(350, 17);
			this.checkShowAutoDeposit.TabIndex = 294;
			this.checkShowAutoDeposit.Text = "Insurance payments create auto deposit";
			// 
			// labelDepositSoftware
			// 
			this.labelDepositSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDepositSoftware.Location = new System.Drawing.Point(117, 36);
			this.labelDepositSoftware.Name = "labelDepositSoftware";
			this.labelDepositSoftware.Size = new System.Drawing.Size(192, 17);
			this.labelDepositSoftware.TabIndex = 300;
			this.labelDepositSoftware.Text = "Deposit Software";
			this.labelDepositSoftware.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxPrescriptions
			// 
			this.groupBoxPrescriptions.Controls.Add(this.checkRxHideProvsWithoutDEA);
			this.groupBoxPrescriptions.Location = new System.Drawing.Point(20, 372);
			this.groupBoxPrescriptions.Name = "groupBoxPrescriptions";
			this.groupBoxPrescriptions.Size = new System.Drawing.Size(450, 60);
			this.groupBoxPrescriptions.TabIndex = 305;
			this.groupBoxPrescriptions.Text = "Prescriptions";
			// 
			// checkRxHideProvsWithoutDEA
			// 
			this.checkRxHideProvsWithoutDEA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkRxHideProvsWithoutDEA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRxHideProvsWithoutDEA.Location = new System.Drawing.Point(20, 33);
			this.checkRxHideProvsWithoutDEA.Name = "checkRxHideProvsWithoutDEA";
			this.checkRxHideProvsWithoutDEA.Size = new System.Drawing.Size(420, 17);
			this.checkRxHideProvsWithoutDEA.TabIndex = 302;
			this.checkRxHideProvsWithoutDEA.Text = "Hide providers without DEA number from making (non-electronic)  prescriptions";
			// 
			// groupBoxClaims
			// 
			this.groupBoxClaims.Controls.Add(this.textClaimsReceivedDays);
			this.groupBoxClaims.Controls.Add(this.checkClaimsSendWindowValidateOnLoad);
			this.groupBoxClaims.Controls.Add(this.labelClaimsReceivedDays);
			this.groupBoxClaims.Controls.Add(this.checkClaimPaymentBatchOnly);
			this.groupBoxClaims.Location = new System.Drawing.Point(20, 279);
			this.groupBoxClaims.Name = "groupBoxClaims";
			this.groupBoxClaims.Size = new System.Drawing.Size(450, 86);
			this.groupBoxClaims.TabIndex = 304;
			this.groupBoxClaims.Text = "Claims";
			// 
			// textClaimsReceivedDays
			// 
			this.textClaimsReceivedDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textClaimsReceivedDays.Location = new System.Drawing.Point(380, 33);
			this.textClaimsReceivedDays.MaxVal = 50000;
			this.textClaimsReceivedDays.MinVal = 1;
			this.textClaimsReceivedDays.Name = "textClaimsReceivedDays";
			this.textClaimsReceivedDays.ShowZero = false;
			this.textClaimsReceivedDays.Size = new System.Drawing.Size(60, 20);
			this.textClaimsReceivedDays.TabIndex = 290;
			this.textClaimsReceivedDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkClaimsSendWindowValidateOnLoad
			// 
			this.checkClaimsSendWindowValidateOnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkClaimsSendWindowValidateOnLoad.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimsSendWindowValidateOnLoad.Location = new System.Drawing.Point(60, 10);
			this.checkClaimsSendWindowValidateOnLoad.Name = "checkClaimsSendWindowValidateOnLoad";
			this.checkClaimsSendWindowValidateOnLoad.Size = new System.Drawing.Size(380, 17);
			this.checkClaimsSendWindowValidateOnLoad.TabIndex = 200;
			this.checkClaimsSendWindowValidateOnLoad.Text = "Claims Send window validate on load (can cause slowness)";
			// 
			// labelClaimsReceivedDays
			// 
			this.labelClaimsReceivedDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelClaimsReceivedDays.Location = new System.Drawing.Point(18, 36);
			this.labelClaimsReceivedDays.MaximumSize = new System.Drawing.Size(1000, 300);
			this.labelClaimsReceivedDays.Name = "labelClaimsReceivedDays";
			this.labelClaimsReceivedDays.Size = new System.Drawing.Size(361, 17);
			this.labelClaimsReceivedDays.TabIndex = 299;
			this.labelClaimsReceivedDays.Text = "Show $0 claim payments in Batch Ins for number days";
			this.labelClaimsReceivedDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimPaymentBatchOnly
			// 
			this.checkClaimPaymentBatchOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkClaimPaymentBatchOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimPaymentBatchOnly.Location = new System.Drawing.Point(19, 59);
			this.checkClaimPaymentBatchOnly.Name = "checkClaimPaymentBatchOnly";
			this.checkClaimPaymentBatchOnly.Size = new System.Drawing.Size(421, 17);
			this.checkClaimPaymentBatchOnly.TabIndex = 291;
			this.checkClaimPaymentBatchOnly.Text = "Finalize claim payments in Batch Insurance window only";
			// 
			// groupBoxERA
			// 
			this.groupBoxERA.Controls.Add(this.comboEraWriteoff);
			this.groupBoxERA.Controls.Add(this.textEraNoAutoProcessCarcCodes);
			this.groupBoxERA.Controls.Add(this.comboEraDefaultPaymentType);
			this.groupBoxERA.Controls.Add(this.comboFwtPaymentType);
			this.groupBoxERA.Controls.Add(this.comboAchPaymentType);
			this.groupBoxERA.Controls.Add(this.comboEraCheckPaymentType);
			this.groupBoxERA.Controls.Add(this.comboEraAutomation);
			this.groupBoxERA.Controls.Add(this.labelWriteOffs);
			this.groupBoxERA.Controls.Add(this.labelEraNoAutoProcessCarcCodes);
			this.groupBoxERA.Controls.Add(this.labelERADefault);
			this.groupBoxERA.Controls.Add(this.labelFWT);
			this.groupBoxERA.Controls.Add(this.labelACH);
			this.groupBoxERA.Controls.Add(this.labelCHK);
			this.groupBoxERA.Controls.Add(this.checkEraOneClaimPerPage);
			this.groupBoxERA.Controls.Add(this.checkIncludeEraWOPercCoPay);
			this.groupBoxERA.Controls.Add(this.labelEraAutomation);
			this.groupBoxERA.Controls.Add(this.checkEraAllowTotalPayment);
			this.groupBoxERA.Location = new System.Drawing.Point(20, 10);
			this.groupBoxERA.Name = "groupBoxERA";
			this.groupBoxERA.Size = new System.Drawing.Size(450, 263);
			this.groupBoxERA.TabIndex = 303;
			this.groupBoxERA.Text = "ERA";
			// 
			// comboEraWriteoff
			// 
			this.comboEraWriteoff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboEraWriteoff.Location = new System.Drawing.Point(310, 74);
			this.comboEraWriteoff.Name = "comboEraWriteoff";
			this.comboEraWriteoff.Size = new System.Drawing.Size(130, 21);
			this.comboEraWriteoff.TabIndex = 301;
			// 
			// textEraNoAutoProcessCarcCodes
			// 
			this.textEraNoAutoProcessCarcCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textEraNoAutoProcessCarcCodes.Location = new System.Drawing.Point(262, 234);
			this.textEraNoAutoProcessCarcCodes.Name = "textEraNoAutoProcessCarcCodes";
			this.textEraNoAutoProcessCarcCodes.Size = new System.Drawing.Size(178, 20);
			this.textEraNoAutoProcessCarcCodes.TabIndex = 312;
			// 
			// comboEraDefaultPaymentType
			// 
			this.comboEraDefaultPaymentType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboEraDefaultPaymentType.Location = new System.Drawing.Point(310, 204);
			this.comboEraDefaultPaymentType.Name = "comboEraDefaultPaymentType";
			this.comboEraDefaultPaymentType.Size = new System.Drawing.Size(130, 21);
			this.comboEraDefaultPaymentType.TabIndex = 308;
			// 
			// comboFwtPaymentType
			// 
			this.comboFwtPaymentType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboFwtPaymentType.Location = new System.Drawing.Point(310, 178);
			this.comboFwtPaymentType.Name = "comboFwtPaymentType";
			this.comboFwtPaymentType.Size = new System.Drawing.Size(130, 21);
			this.comboFwtPaymentType.TabIndex = 306;
			// 
			// comboAchPaymentType
			// 
			this.comboAchPaymentType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboAchPaymentType.Location = new System.Drawing.Point(310, 152);
			this.comboAchPaymentType.Name = "comboAchPaymentType";
			this.comboAchPaymentType.Size = new System.Drawing.Size(130, 21);
			this.comboAchPaymentType.TabIndex = 304;
			// 
			// comboEraCheckPaymentType
			// 
			this.comboEraCheckPaymentType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboEraCheckPaymentType.Location = new System.Drawing.Point(310, 126);
			this.comboEraCheckPaymentType.Name = "comboEraCheckPaymentType";
			this.comboEraCheckPaymentType.Size = new System.Drawing.Size(130, 21);
			this.comboEraCheckPaymentType.TabIndex = 302;
			// 
			// comboEraAutomation
			// 
			this.comboEraAutomation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboEraAutomation.Location = new System.Drawing.Point(310, 100);
			this.comboEraAutomation.Name = "comboEraAutomation";
			this.comboEraAutomation.Size = new System.Drawing.Size(130, 21);
			this.comboEraAutomation.TabIndex = 297;
			// 
			// labelWriteOffs
			// 
			this.labelWriteOffs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWriteOffs.Location = new System.Drawing.Point(21, 76);
			this.labelWriteOffs.Name = "labelWriteOffs";
			this.labelWriteOffs.Size = new System.Drawing.Size(288, 17);
			this.labelWriteOffs.TabIndex = 312;
			this.labelWriteOffs.Text = "Auto-processing posts write-offs on non-primary claims";
			this.labelWriteOffs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelEraNoAutoProcessCarcCodes
			// 
			this.labelEraNoAutoProcessCarcCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelEraNoAutoProcessCarcCodes.Location = new System.Drawing.Point(7, 236);
			this.labelEraNoAutoProcessCarcCodes.Name = "labelEraNoAutoProcessCarcCodes";
			this.labelEraNoAutoProcessCarcCodes.Size = new System.Drawing.Size(254, 17);
			this.labelEraNoAutoProcessCarcCodes.TabIndex = 311;
			this.labelEraNoAutoProcessCarcCodes.Text = "Don\'t auto-process claims w/ these CARCs";
			this.labelEraNoAutoProcessCarcCodes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelERADefault
			// 
			this.labelERADefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelERADefault.Location = new System.Drawing.Point(120, 207);
			this.labelERADefault.Name = "labelERADefault";
			this.labelERADefault.Size = new System.Drawing.Size(189, 17);
			this.labelERADefault.TabIndex = 309;
			this.labelERADefault.Text = "Default payment type";
			this.labelERADefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelFWT
			// 
			this.labelFWT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelFWT.Location = new System.Drawing.Point(120, 181);
			this.labelFWT.Name = "labelFWT";
			this.labelFWT.Size = new System.Drawing.Size(189, 17);
			this.labelFWT.TabIndex = 307;
			this.labelFWT.Text = "FWT payment type";
			this.labelFWT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelACH
			// 
			this.labelACH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelACH.Location = new System.Drawing.Point(120, 155);
			this.labelACH.Name = "labelACH";
			this.labelACH.Size = new System.Drawing.Size(189, 17);
			this.labelACH.TabIndex = 305;
			this.labelACH.Text = "ACH payment type";
			this.labelACH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCHK
			// 
			this.labelCHK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelCHK.Location = new System.Drawing.Point(120, 129);
			this.labelCHK.Name = "labelCHK";
			this.labelCHK.Size = new System.Drawing.Size(189, 17);
			this.labelCHK.TabIndex = 303;
			this.labelCHK.Text = "CHK payment type";
			this.labelCHK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEraOneClaimPerPage
			// 
			this.checkEraOneClaimPerPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkEraOneClaimPerPage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEraOneClaimPerPage.Location = new System.Drawing.Point(40, 10);
			this.checkEraOneClaimPerPage.Name = "checkEraOneClaimPerPage";
			this.checkEraOneClaimPerPage.Size = new System.Drawing.Size(400, 17);
			this.checkEraOneClaimPerPage.TabIndex = 292;
			this.checkEraOneClaimPerPage.Text = "ERA prints one page per claim";
			// 
			// checkIncludeEraWOPercCoPay
			// 
			this.checkIncludeEraWOPercCoPay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkIncludeEraWOPercCoPay.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeEraWOPercCoPay.Location = new System.Drawing.Point(40, 32);
			this.checkIncludeEraWOPercCoPay.Name = "checkIncludeEraWOPercCoPay";
			this.checkIncludeEraWOPercCoPay.Size = new System.Drawing.Size(400, 17);
			this.checkIncludeEraWOPercCoPay.TabIndex = 293;
			this.checkIncludeEraWOPercCoPay.Text = "ERA posts write-offs for Category Percentage and Medicaid/Flat Copay";
			// 
			// labelEraAutomation
			// 
			this.labelEraAutomation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelEraAutomation.Location = new System.Drawing.Point(120, 103);
			this.labelEraAutomation.Name = "labelEraAutomation";
			this.labelEraAutomation.Size = new System.Drawing.Size(189, 17);
			this.labelEraAutomation.TabIndex = 301;
			this.labelEraAutomation.Text = "ERA Automation";
			this.labelEraAutomation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEraAllowTotalPayment
			// 
			this.checkEraAllowTotalPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkEraAllowTotalPayment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEraAllowTotalPayment.Location = new System.Drawing.Point(19, 54);
			this.checkEraAllowTotalPayment.Name = "checkEraAllowTotalPayment";
			this.checkEraAllowTotalPayment.Size = new System.Drawing.Size(421, 17);
			this.checkEraAllowTotalPayment.TabIndex = 296;
			this.checkEraAllowTotalPayment.Text = "ERA allow total payments";
			// 
			// UserControlManageGeneral
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.checkAccountingInvoiceAttachmentsSaveInDatabase);
			this.Controls.Add(this.groupBoxTimeCards);
			this.Controls.Add(this.groupBoxDeposits);
			this.Controls.Add(this.groupBoxPrescriptions);
			this.Controls.Add(this.groupBoxClaims);
			this.Controls.Add(this.groupBoxERA);
			this.Controls.Add(this.checkScheduleProvEmpSelectAll);
			this.Name = "UserControlManageGeneral";
			this.Size = new System.Drawing.Size(494, 660);
			this.groupBoxTimeCards.ResumeLayout(false);
			this.groupBoxDeposits.ResumeLayout(false);
			this.groupBoxPrescriptions.ResumeLayout(false);
			this.groupBoxClaims.ResumeLayout(false);
			this.groupBoxClaims.PerformLayout();
			this.groupBoxERA.ResumeLayout(false);
			this.groupBoxERA.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.CheckBox checkScheduleProvEmpSelectAll;
		private OpenDental.UI.CheckBox checkClaimsSendWindowValidateOnLoad;
		private OpenDental.UI.CheckBox checkTimeCardADP;
		private UI.ComboBox comboTimeCardOvertimeFirstDayOfWeek;
		private System.Windows.Forms.Label label16;
		private OpenDental.UI.CheckBox checkRxHideProvsWithoutDEA;
		private UI.ComboBox comboEraAutomation;
		private System.Windows.Forms.Label labelEraAutomation;
		private UI.ComboBox comboDepositSoftware;
		private System.Windows.Forms.Label labelDepositSoftware;
		private OpenDental.UI.CheckBox checkEraAllowTotalPayment;
		private OpenDental.UI.CheckBox checkIncludeEraWOPercCoPay;
		private OpenDental.UI.CheckBox checkClockEventAllowBreak;
		private ValidNum textClaimsReceivedDays;
		private OpenDental.UI.CheckBox checkShowAutoDeposit;
		private OpenDental.UI.CheckBox checkEraOneClaimPerPage;
		private OpenDental.UI.CheckBox checkClaimPaymentBatchOnly;
		private System.Windows.Forms.Label labelClaimsReceivedDays;
		private UI.GroupBox groupBoxERA;
		private UI.GroupBox groupBoxClaims;
		private UI.GroupBox groupBoxPrescriptions;
		private UI.GroupBox groupBoxDeposits;
		private UI.GroupBox groupBoxTimeCards;
		private OpenDental.UI.CheckBox checkAccountingInvoiceAttachmentsSaveInDatabase;
		private System.Windows.Forms.Label labelERADefault;
		private UI.ComboBox comboEraDefaultPaymentType;
		private System.Windows.Forms.Label labelFWT;
		private UI.ComboBox comboFwtPaymentType;
		private System.Windows.Forms.Label labelACH;
		private UI.ComboBox comboAchPaymentType;
		private System.Windows.Forms.Label labelCHK;
		private UI.ComboBox comboEraCheckPaymentType;
		private UI.ComboBox comboEraWriteoff;
		private System.Windows.Forms.Label labelWriteOffs;
		private System.Windows.Forms.Label labelEraNoAutoProcessCarcCodes;
		private System.Windows.Forms.TextBox textEraNoAutoProcessCarcCodes;
		private UI.CheckBox checkTimeCardUseLocal;
	}
}
