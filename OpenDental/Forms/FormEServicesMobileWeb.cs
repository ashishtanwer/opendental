using CodeBase;
using Microsoft.Win32;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using System.Data;
using System.Linq;
using System.IO;
using WebServiceSerializer;
using OpenDentBusiness.WebServiceMainHQ;
using OpenDentBusiness.WebTypes.WebSched.TimeSlot;

namespace OpenDental {

	public partial class FormEServicesMobileWeb:FormODBase {
		private WebServiceMainHQProxy.EServiceSetup.SignupOut _signupOut;
		
		public FormEServicesMobileWeb(WebServiceMainHQProxy.EServiceSetup.SignupOut signupOut=null) {
			InitializeComponent();
			InitializeLayoutManager();
			Lan.F(this);
			_signupOut=signupOut;
		}

		private void FormEServicesMobileWeb_Load(object sender,EventArgs e) {
			//If user does not have EServicesSetup permission, disable all controls except the Setup Mobile Web Users button, which opens
			//FormSecurity and checks the SecurityAdmin permission. FormSecurity is normally reachable without EServiceSetup permission,
			//so no reason to block it here.
			if(!Security.IsAuthorized(EnumPermType.EServicesSetup,suppressMessage:true)) {
				DisableAllExcept(butSetupMobileWebUsers,groupBox5);
			}
			if(_signupOut==null){
				_signupOut=FormEServicesSetup.GetSignupOut();
			}
			WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService signupOutEService=
				WebServiceMainHQProxy.GetSignups<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>(_signupOut,eServiceCode.MobileWeb).FirstOrDefault();
			if(signupOutEService==null) {
				signupOutEService=new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService();
				signupOutEService.HostedUrl="";
			}
			string urlFromHQ=signupOutEService.HostedUrl;
			textHostedUrlMobileWeb.Text=urlFromHQ;
		}
	
		private void butSetupMobileWebUsers_Click(object sender,EventArgs e) {
			FormOpenDental.S_MenuItemSecurity_Click(sender,e);
		}

	}
}