﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormRpAccountingGenLedg {
		private System.ComponentModel.IContainer components = null;

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpAccountingGenLedg));
			this.butOK = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.labelTO = new System.Windows.Forms.Label();
			this.monthCalendarEnd = new OpenDental.UI.MonthCalendarOD();
			this.monthCalendarStart = new OpenDental.UI.MonthCalendarOD();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(444, 243);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(269, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 18);
			this.label1.TabIndex = 34;
			this.label1.Text = "To";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelTO
			// 
			this.labelTO.Location = new System.Drawing.Point(27, 34);
			this.labelTO.Name = "labelTO";
			this.labelTO.Size = new System.Drawing.Size(72, 18);
			this.labelTO.TabIndex = 33;
			this.labelTO.Text = "From";
			this.labelTO.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// monthCalendarEnd
			// 
			this.monthCalendarEnd.Location = new System.Drawing.Point(270, 56);
			this.monthCalendarEnd.Name = "monthCalendarEnd";
			this.monthCalendarEnd.Size = new System.Drawing.Size(227, 162);
			this.monthCalendarEnd.TabIndex = 32;
			this.monthCalendarEnd.Text = "monthCalendarEnd";
			// 
			// monthCalendarStart
			// 
			this.monthCalendarStart.Location = new System.Drawing.Point(30, 56);
			this.monthCalendarStart.Name = "monthCalendarStart";
			this.monthCalendarStart.Size = new System.Drawing.Size(227, 162);
			this.monthCalendarStart.TabIndex = 31;
			this.monthCalendarStart.Text = "monthCalendarStart";
			// 
			// FormRpAccountingGenLedg
			// 
			this.ClientSize = new System.Drawing.Size(531, 281);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelTO);
			this.Controls.Add(this.monthCalendarEnd);
			this.Controls.Add(this.monthCalendarStart);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRpAccountingGenLedg";
			this.ShowInTaskbar = false;
			this.Text = "General Ledger Report";
			this.Load += new System.EventHandler(this.FormRpAccountingGenLedg_Load);
			this.ResumeLayout(false);

		}
		#endregion
		private OpenDental.UI.Button butOK;
		private Label label1;
		private Label labelTO;
		private UI.MonthCalendarOD monthCalendarEnd;
		private UI.MonthCalendarOD monthCalendarStart;
	}
}
