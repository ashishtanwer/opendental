namespace OpenDental{
	partial class FormERoutingDefEdit {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormERoutingDefEdit));
			this.butSave = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.textBoxDescription = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.gridERoutingActions = new OpenDental.UI.GridOD();
			this.butAdd = new OpenDental.UI.Button();
			this.groupBoxActions = new OpenDental.UI.GroupBox();
			this.groupBoxOD2 = new OpenDental.UI.GroupBox();
			this.butRemoveLinkType = new OpenDental.UI.Button();
			this.gridLinkTypes = new OpenDental.UI.GridOD();
			this.groupBoxOD3 = new OpenDental.UI.GroupBox();
			this.labelGenAppts = new System.Windows.Forms.Label();
			this.butAddLinkType = new OpenDental.UI.Button();
			this.butAddSpecificTypes = new OpenDental.UI.Button();
			this.comboLinkType = new OpenDental.UI.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBoxActions.SuspendLayout();
			this.groupBoxOD2.SuspendLayout();
			this.groupBoxOD3.SuspendLayout();
			this.SuspendLayout();
			// 
			// butSave
			// 
			this.butSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSave.Location = new System.Drawing.Point(925, 377);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(75, 24);
			this.butSave.TabIndex = 8;
			this.butSave.Text = "&Save";
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Icon = OpenDental.UI.EnumIcons.DeleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(12, 377);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 7;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textBoxDescription
			// 
			this.textBoxDescription.Location = new System.Drawing.Point(12, 35);
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.Size = new System.Drawing.Size(200, 20);
			this.textBoxDescription.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(156, 18);
			this.label1.TabIndex = 11;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butDown
			// 
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(436, 137);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(75, 24);
			this.butDown.TabIndex = 6;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(436, 107);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(75, 24);
			this.butUp.TabIndex = 5;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// gridERoutingActions
			// 
			this.gridERoutingActions.Location = new System.Drawing.Point(3, 3);
			this.gridERoutingActions.Name = "gridERoutingActions";
			this.gridERoutingActions.Size = new System.Drawing.Size(427, 246);
			this.gridERoutingActions.TabIndex = 5;
			this.gridERoutingActions.Title = "Actions";
			this.gridERoutingActions.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridERoutingActions_CellDoubleClick);
			// 
			// butAdd
			// 
			this.butAdd.Icon = OpenDental.UI.EnumIcons.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(436, 3);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 2;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// groupBoxActions
			// 
			this.groupBoxActions.Controls.Add(this.butAdd);
			this.groupBoxActions.Controls.Add(this.gridERoutingActions);
			this.groupBoxActions.Controls.Add(this.butDown);
			this.groupBoxActions.Controls.Add(this.butUp);
			this.groupBoxActions.Location = new System.Drawing.Point(12, 111);
			this.groupBoxActions.Name = "groupBoxActions";
			this.groupBoxActions.Size = new System.Drawing.Size(516, 252);
			this.groupBoxActions.TabIndex = 15;
			this.groupBoxActions.Text = "";
			// 
			// groupBoxOD2
			// 
			this.groupBoxOD2.Controls.Add(this.butRemoveLinkType);
			this.groupBoxOD2.Controls.Add(this.gridLinkTypes);
			this.groupBoxOD2.Controls.Add(this.groupBoxOD3);
			this.groupBoxOD2.Location = new System.Drawing.Point(534, 111);
			this.groupBoxOD2.Name = "groupBoxOD2";
			this.groupBoxOD2.Size = new System.Drawing.Size(466, 252);
			this.groupBoxOD2.TabIndex = 16;
			this.groupBoxOD2.Text = "";
			// 
			// butRemoveLinkType
			// 
			this.butRemoveLinkType.Icon = OpenDental.UI.EnumIcons.DeleteX;
			this.butRemoveLinkType.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRemoveLinkType.Location = new System.Drawing.Point(260, 225);
			this.butRemoveLinkType.Name = "butRemoveLinkType";
			this.butRemoveLinkType.Size = new System.Drawing.Size(75, 24);
			this.butRemoveLinkType.TabIndex = 15;
			this.butRemoveLinkType.Text = "&Remove";
			this.butRemoveLinkType.Click += new System.EventHandler(this.butRemoveLinkType_Click);
			// 
			// gridLinkTypes
			// 
			this.gridLinkTypes.Location = new System.Drawing.Point(3, 3);
			this.gridLinkTypes.Name = "gridLinkTypes";
			this.gridLinkTypes.Size = new System.Drawing.Size(248, 246);
			this.gridLinkTypes.TabIndex = 17;
			this.gridLinkTypes.Title = "eRouting Triggers";
			// 
			// groupBoxOD3
			// 
			this.groupBoxOD3.Controls.Add(this.labelGenAppts);
			this.groupBoxOD3.Controls.Add(this.butAddLinkType);
			this.groupBoxOD3.Controls.Add(this.butAddSpecificTypes);
			this.groupBoxOD3.Controls.Add(this.comboLinkType);
			this.groupBoxOD3.Location = new System.Drawing.Point(257, 3);
			this.groupBoxOD3.Name = "groupBoxOD3";
			this.groupBoxOD3.Size = new System.Drawing.Size(206, 143);
			this.groupBoxOD3.TabIndex = 17;
			this.groupBoxOD3.Text = "Add Trigger Type";
			// 
			// labelGenAppts
			// 
			this.labelGenAppts.Location = new System.Drawing.Point(81, 40);
			this.labelGenAppts.Name = "labelGenAppts";
			this.labelGenAppts.Size = new System.Drawing.Size(115, 36);
			this.labelGenAppts.TabIndex = 19;
			this.labelGenAppts.Text = "Add with no Appt Type";
			this.labelGenAppts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butAddLinkType
			// 
			this.butAddLinkType.Icon = OpenDental.UI.EnumIcons.Add;
			this.butAddLinkType.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddLinkType.Location = new System.Drawing.Point(3, 46);
			this.butAddLinkType.Name = "butAddLinkType";
			this.butAddLinkType.Size = new System.Drawing.Size(75, 24);
			this.butAddLinkType.TabIndex = 7;
			this.butAddLinkType.Text = "&Add";
			this.butAddLinkType.Click += new System.EventHandler(this.butAddLinkType_Click);
			// 
			// butAddSpecificTypes
			// 
			this.butAddSpecificTypes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddSpecificTypes.Location = new System.Drawing.Point(3, 76);
			this.butAddSpecificTypes.Name = "butAddSpecificTypes";
			this.butAddSpecificTypes.Size = new System.Drawing.Size(94, 24);
			this.butAddSpecificTypes.TabIndex = 6;
			this.butAddSpecificTypes.Text = "Add Appt Types";
			this.butAddSpecificTypes.Click += new System.EventHandler(this.butAddSpecificTypes_Click);
			// 
			// comboLinkType
			// 
			this.comboLinkType.Location = new System.Drawing.Point(3, 17);
			this.comboLinkType.Name = "comboLinkType";
			this.comboLinkType.Size = new System.Drawing.Size(174, 23);
			this.comboLinkType.TabIndex = 18;
			this.comboLinkType.Text = "comboBoxOD1";
			this.comboLinkType.SelectionChangeCommitted += new System.EventHandler(this.comboLinkTypes_SelectionChangeCommitted);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 81);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(430, 24);
			this.label2.TabIndex = 17;
			this.label2.Text = "Determines what actions should take place in this eRouting Def and their order.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(534, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(413, 40);
			this.label3.TabIndex = 18;
			this.label3.Text = "Specifies the situations for which this eRouting Def will be available. If none e" +
    "ntered, the route is treated as \"General\".";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// FormERoutingDefEdit
			// 
			this.ClientSize = new System.Drawing.Size(1015, 413);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupBoxOD2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxDescription);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butSave);
			this.Controls.Add(this.groupBoxActions);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormERoutingDefEdit";
			this.Text = "eRouting Def Edit";
			this.Load += new System.EventHandler(this.FormPatientFlowEdit_Load);
			this.groupBoxActions.ResumeLayout(false);
			this.groupBoxOD2.ResumeLayout(false);
			this.groupBoxOD3.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butSave;
		private UI.Button butDelete;
		private System.Windows.Forms.TextBox textBoxDescription;
		private System.Windows.Forms.Label label1;
		private UI.Button butDown;
		private UI.Button butUp;
		private UI.GridOD gridERoutingActions;
		private UI.Button butAdd;
		private UI.GroupBox groupBoxActions;
		private UI.GroupBox groupBoxOD2;
		private UI.GridOD gridLinkTypes;
		private UI.GroupBox groupBoxOD3;
		private UI.Button butAddLinkType;
		private UI.ComboBox comboLinkType;
		private UI.Button butRemoveLinkType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private UI.Button butAddSpecificTypes;
		private System.Windows.Forms.Label labelGenAppts;
	}
}