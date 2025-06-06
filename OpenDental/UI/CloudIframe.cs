﻿using System;
using System.Windows.Forms;
using CodeBase;
using OpenDental.Thinfinity;

namespace OpenDental.UI {
	public partial class CloudIframe:UserControl {
		private string _frameId;

		public CloudIframe() {
			InitializeComponent();
			Font=LayoutManagerForms.FontInitial;
			this.Visible=false;
		}

		///<summary>Inserts the iFrame into the DOM and initializes it.</summary>
		public void Initialize(IntPtr handle,string url="",bool isHidden=false) {
			_frameId=Browser.InsertIframe(handle,url,isHidden);
		}

		///<summary>Navigates the iFrame to the url.</summary>
		public void Navigate(IntPtr handle,string url) {
			if(_frameId.IsNullOrEmpty()) {
				Initialize(handle,url);
				return;
			}
			ODCloudClient.ODBrowserData odBrowserData=new ODCloudClient.ODBrowserData {
				ElementId=_frameId,
				Url=url
			};
			ODCloudClient.SendToBrowser(odBrowserData,ODCloudClient.BrowserAction.NavigateIframe);
		}

		///<summary>Displays a file in the iFrame.</summary>
		public void DisplayFile(IntPtr handle,string filepath) {
			if(!string.IsNullOrEmpty(filepath) && filepath.StartsWith(@"\\")) {
				filepath=ThinfinityUtils.GetTempLocalPathForFile(filepath);
			}
			string url=Browser.GetSafeUrl(filepath);
			Navigate(handle,url);
		}

		///<summary>Makes the iFrame visible.</summary>
		public void ShowIframe(IntPtr handle) {
			Initialize(handle);
			ODCloudClient.ODBrowserData odBrowserData=new ODCloudClient.ODBrowserData {
				ElementId=_frameId,
				IsVisible=true
			};
			ODCloudClient.SendToBrowser(odBrowserData,ODCloudClient.BrowserAction.SetIframeVisible);
		}

		///<summary>Hides the iFrame.</summary>
		public void HideIframe(IntPtr handle) {
			Initialize(handle,isHidden:true);
			ODCloudClient.ODBrowserData odBrowserData=new ODCloudClient.ODBrowserData {
				ElementId=_frameId,
				IsVisible=false
			};
			ODCloudClient.SendToBrowser(odBrowserData,ODCloudClient.BrowserAction.SetIframeVisible);
		}
	}
}
