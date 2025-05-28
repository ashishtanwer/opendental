﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfControls.UI {
	//instructions for how to use are in Menu file.
	public class MenuItem : System.Windows.Controls.MenuItem{
		#region Fields
		private string _shortcut="";
		private RoutedEventHandler _click;
		#endregion Fields

		#region Constructors
		public MenuItem(){
			VerticalAlignment=VerticalAlignment.Stretch;
		}

		public MenuItem(string text){
			VerticalAlignment=VerticalAlignment.Stretch;
			Header=text;
		}

		public MenuItem(string text,RoutedEventHandler click,string shortcut="",object tag=null){
			VerticalAlignment=VerticalAlignment.Stretch;
			Header=text;
			Shortcut=shortcut;
			Tag=tag;
			_click=click;
			Click+=(sender,routedEventArgs)=>_click?.Invoke(sender,new RoutedEventArgs());
		}
		#endregion Constructors

		#region Properties
		///<summary>Use a single capital letter. Example "W". This will show in the UI as "Ctrl+W" and that key combo will trigger the click event. No support for other modifier keys.</summary>
		public string Shortcut {
			get {
				return _shortcut;
			}
			set {
				_shortcut=value;
				if(string.IsNullOrEmpty(_shortcut)){
					InputGestureText="";
					return;
				}
				InputGestureText="Ctrl+"+_shortcut;//this does nothing except display the text.
			}
		}

		public string Text {
			//There are plans to enhance this so that the header is composed of image-text-shortcut
			get {
				return Header.ToString();
			}
			set {
				Header=value;
			}
		}
		#endregion Properties

		#region Methods public
		public void Add(MenuItem menuItem){
			Items.Add(menuItem);
		}

		public void Add(string text,RoutedEventHandler click){
			MenuItem menuItem=new MenuItem(text,click);
			Items.Add(menuItem);
		}

		public void AddSeparator(){
			Items.Add(new System.Windows.Controls.Separator());
		}

		public void ClearClickEvent(){
			if(_click!=null) {
				Click-=_click;
				_click=null;
			}
		}

		public List<MenuItem> GetMenuItems(){
			List<MenuItem> listMenuItems=new List<MenuItem>();
			for(int i=0;i<Items.Count;i++){
				if(Items[i] is System.Windows.Controls.Separator){
					continue;
				}
				listMenuItems.Add((MenuItem)Items[i]);
			}
			return listMenuItems;
		}
		#endregion Methods public
	}
}
