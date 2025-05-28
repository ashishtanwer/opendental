namespace OpenDental{
	partial class FormUnschedListSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUnschedListSetup));
			this.butSave = new OpenDental.UI.Button();
			this.groupBox3 = new OpenDental.UI.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textDaysFuture = new OpenDental.ValidNum();
			this.textDaysPast = new OpenDental.ValidNum();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// butSave
			// 
			this.butSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSave.Location = new System.Drawing.Point(190, 143);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(75, 24);
			this.butSave.TabIndex = 3;
			this.butSave.Text = "&Save";
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.textDaysFuture);
			this.groupBox3.Controls.Add(this.textDaysPast);
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Controls.Add(this.label15);
			this.groupBox3.Location = new System.Drawing.Point(12, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(262, 112);
			this.groupBox3.TabIndex = 55;
			this.groupBox3.Text = "Default Dates";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 61);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(228, 39);
			this.label1.TabIndex = 67;
			this.label1.Text = "Leaving these fields blank will result in the date\r\n filter not being set when th" +
    "e unscheduled list\r\n is first opened.";
			// 
			// textDaysFuture
			// 
			this.textDaysFuture.Location = new System.Drawing.Point(192, 38);
			this.textDaysFuture.MaxVal = 10000;
			this.textDaysFuture.Name = "textDaysFuture";
			this.textDaysFuture.ShowZero = false;
			this.textDaysFuture.Size = new System.Drawing.Size(53, 20);
			this.textDaysFuture.TabIndex = 66;
			// 
			// textDaysPast
			// 
			this.textDaysPast.Location = new System.Drawing.Point(192, 16);
			this.textDaysPast.MaxVal = 10000;
			this.textDaysPast.Name = "textDaysPast";
			this.textDaysPast.ShowZero = false;
			this.textDaysPast.Size = new System.Drawing.Size(53, 20);
			this.textDaysPast.TabIndex = 65;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(6, 16);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(184, 20);
			this.label14.TabIndex = 50;
			this.label14.Text = "Days Past (e.g. 365, blank, etc)";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(9, 37);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(181, 20);
			this.label15.TabIndex = 52;
			this.label15.Text = "Days Future (e.g. 7)";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormUnschedListSetup
			// 
			this.ClientSize = new System.Drawing.Size(286, 180);
			this.Controls.Add(this.butSave);
			this.Controls.Add(this.groupBox3);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormUnschedListSetup";
			this.Text = "Unscheduled Setup";
			this.Load += new System.EventHandler(this.FormUnschedListSetup_Load);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butSave;
		private OpenDental.UI.GroupBox groupBox3;
		private ValidNum textDaysFuture;
		private ValidNum textDaysPast;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label1;
	}
}