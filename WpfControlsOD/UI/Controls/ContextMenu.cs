﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfControls.UI {
	//instructions for how to use are in Menu file.
	public class ContextMenu : System.Windows.Controls.ContextMenu{

		#region Constructor
		///<summary>Pass in a control if you only want the shortcuts to work from one control with focus. Pass in a Frm if you want the shortcuts to work for that entire Frm. Null is fine if you don't care about shortcuts.</summary>
		public ContextMenu(FrameworkElement frameworkElementParent=null){
			if(frameworkElementParent is null){
				return;
			}
			frameworkElementParent.PreviewKeyDown+=frameworkElementParent_PreviewKeyDown;
			PreviewKeyDown+=frameworkElementParent_PreviewKeyDown;//both are needed
			frameworkElementParent.Unloaded+=(sender,e)=>ClearMenuItemsClickEvent();
		}
		#endregion Constructor

		#region Methods - public
		public void Add(MenuItem menuItem){
			Items.Add(menuItem);
		}

		public void Add(string text,RoutedEventHandler click,object tag=null){
			MenuItem menuItem=new MenuItem(text,click);
			menuItem.Tag=tag;
			Items.Add(menuItem);
		}

		/// <summary>Typically disregard the return obj because you don't need reference to it.</summary>
		public System.Windows.Controls.Separator AddSeparator(){
			System.Windows.Controls.Separator separator=new System.Windows.Controls.Separator();
			Items.Add(separator);
			return separator;
		}

		public void ClearMenuItemsClickEvent(){
			for(int i=0;i<Items.Count;i++) {
				if(Items[i] is MenuItem menuItem) {
					menuItem.ClearClickEvent();
				}
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

		public void Remove(MenuItem menuItem){
			Items.Remove(menuItem);
		}

		public void RemoveAt(int idx){
			Items.RemoveAt(idx);
		}
		#endregion Methods - public

		#region Methods - private
		private void frameworkElementParent_PreviewKeyDown(object sender,KeyEventArgs e) {
			if(Keyboard.Modifiers!=ModifierKeys.Control){
				return;
			}
			if(e.Key==Key.LeftCtrl || e.Key==Key.RightCtrl){
				return;//just the Ctrl key is pressed, not additional key.
			}
			//MessageBox.Show("Key");
			List<MenuItem> listMenuItems=GetFlatListMenuItems(this);
			for(int i=0;i<listMenuItems.Count;i++){
				if(e.Key.ToString().ToLower()!=listMenuItems[i].Shortcut.ToLower()){
					continue;
				}
				listMenuItems[i].RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
				e.Handled=true;
				return;
			}
		}

		///<summary>Gets a flat list of all menu items. Recursive</summary>
		private List<MenuItem> GetFlatListMenuItems(System.Windows.Controls.Control control){
			List<MenuItem> listMenuItems=new List<MenuItem>();
			List<MenuItem> listMenuItemsDirect=new List<MenuItem>();
			if (control is ContextMenu contextMenu){
				listMenuItemsDirect.AddRange(contextMenu.GetMenuItems());
			}
			else if(control is MenuItem menuItem){
				listMenuItemsDirect.AddRange(menuItem.GetMenuItems());
			}
			else{
				return listMenuItems;
			}
			for(int i=0;i<listMenuItemsDirect.Count;i++){
				listMenuItems.Add(listMenuItemsDirect[i]);
				List<MenuItem> listMenuItemsChildren=GetFlatListMenuItems(listMenuItemsDirect[i]);
				listMenuItems.AddRange(listMenuItemsChildren);
			}
			return listMenuItems;
		}

		/*
		private void ContextMenu_KeyDown(object sender,KeyEventArgs e) {
			if(Keyboard.Modifiers!=ModifierKeys.Control){
				return;
			}
			if(e.Key==Key.LeftCtrl || e.Key==Key.RightCtrl){
				return;//just the Ctrl key is pressed, not additional key.
			}
			//MessageBox.Show("Key");
			List<MenuItem> listMenuItems=GetFlatListMenuItems(this);
			for(int i=0;i<listMenuItems.Count;i++){
				if(e.Key.ToString().ToLower()!=listMenuItems[i].Shortcut.ToLower()){
					continue;
				}
				listMenuItems[i].RaiseClickEvent();
			}
		}*/
		#endregion Methods - private
	}
}
