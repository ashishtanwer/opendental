﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControls.UI{
	/*
	Jordan is the only one allowed to edit this file.

	How to use the TextBox control:
	-Height of a single line textbox should be 20
	-Newlines in Text in code are handled just like normal: "\r\n"
	-Newlines in Text in XAML require &#10; (or the hex equivalent &#xA;)
	-Click event handler usually look like this:
			private void butEdit_Click(object sender,EventArgs e) { etc.
	-TextChanged event handler:
			private void butEdit_TextChanged(object sender,EventArgs e) { etc.

Naming:
	Name these text... If we were starting over, we would name them textBox..., but we're already stuck with the old names.
	Other similar controls like textRich, TextVInt, etc. all have new naming conventions.
Some notes about how tabbing and clicking work with selection of text:
This applies to all of our text boxes, including TextBox, TextRich, TextVInt, etc.
First, we will discuss what happens when a box is the first tab index on a window.
	In old WF TextBox, the entire text is selected.
	In old WF RichTextBox, the caret starts at 0, with nothing selected.
	We have chosen the WF TextBox behavior as the standard for all our custom boxes. Select entire text.
	If you want a different behavior, then in the Loaded event, call textBox.Focus(). This prevents automatic focus which would also have performed a SelectAll.
	If you are manually selecting a textbox on load, generally use SelectAll instead of Focus.
Secondly, as you tab into a new box:
	Behavior in old WF boxes is the same as above with the starting tab index.
	Again, we have chosen the WF TextBox behavior as our standard. Select entire text.
Clicking into a new box:
	In all old WF boxes, you just get a caret where you click.
	We keep the same behavior in all our boxes.
Losing focus when text in a box is currently selected:
	All old WF boxes have the same behavior:
	When keyboard focus is lost, like clicking on another window, it looks like it gets deselected.
	But logical focus and selection are preserved because coming back to the window shows the text highlighted again.
	We preserve this behavior and don't deselect the text. This works, but does cause an annoying flicker when reselecting.
	The flicker is present in plain vanilla WPF textBoxes, so I think it's a non-issue.
	Right clicking on selected text should especially not deselect the text.
AcceptsTab is an option on all old WF and all new WPF built-in boxes.
	But it's not very useful. For now, we just set it to false in all our textboxes.
	False here matches the old false behavior in the System.Windows.Forms.TextBoxes.
	This is handled differently for ODtextBoxes getting converted to textRich.
	See those notes over in TextRich.xaml.cs.
	We might need to add it as a property some day.
Validating/Validated events are not supported.
	But you can use the LostFocus event instead.
	*/
	///<summary></summary>
	public partial class TextBox : UserControl{
		private Color _colorBack=Colors.White;
		private Color _colorText=Colors.Black;
		private bool _isChangingFocusToTextBox;
		private bool _isMultiline=false;
		private HorizontalAlignment _hAlign=HorizontalAlignment.Left;	
		private bool _isUnderline=false;
		private Thickness _paddingOD;
		private bool _readOnly;
		
		public TextBox(){
			InitializeComponent();
			//Width=75;
			//Height=20;
			Focusable=true;//so that .Focus() will work, but then we manually change focus to textBox
			textBox.GotKeyboardFocus+=TextBox_GotKeyboardFocus;
			textBox.LostFocus+=TextBox_LostFocus;
			textBox.LostKeyboardFocus+=TextBox_LostKeyboardFocus;
			textBox.TextChanged+=TextBox_TextChanged;
			IsEnabledChanged+=This_IsEnabledChanged;
			GotKeyboardFocus+=This_GotKeyboardFocus;
			LostFocus+=This_LostFocus;
			LostKeyboardFocus+=This_LostKeyboardFocus;
			PreviewMouseLeftButtonDown+=This_PreviewMouseLeftButtonDown;
			Unloaded+=TextBox_Unloaded;
			border1.Visibility=Visibility.Collapsed;
			border2.Visibility=Visibility.Collapsed;
		}

		#region Events
		public event EventHandler Click;
		[Category("OD")]
		[Description("Try not to use this because it will also fire when changing the value programmatically, like on load. This can cause infinite loops. One good pattern to avoid this is a class level boolean to disable the code inside this event handler during certain situations like load.")]
		public event EventHandler TextChanged;
		#endregion Events

		#region Properties
		[Category("OD")]
		[DefaultValue(typeof(Color),"White")]
		public Color ColorBack {
			get {
				return _colorBack;
			}
			set {
				_colorBack = value;
				textBox.Background=new SolidColorBrush(value);
				border1.Background=new SolidColorBrush(value);
			}
		}

		[Category("OD")]
		[DefaultValue(typeof(Color),"Black")]
		public Color ColorText {
			get {
				return _colorText;
			}
			set {
				_colorText = value;
				textBox.Foreground=new SolidColorBrush(value);
			}
		}

		[Category("OD")]
		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment HAlign {
			get {
				return _hAlign;
			}
			set {
				_hAlign = value;
				textBox.HorizontalContentAlignment=_hAlign;
			}
		}

		[Category("OD")]
		[DefaultValue(true)]
		public new bool IsEnabled{
			//This doesn't actually ever get hit. 
			//It's just here to move IsEnabled down into the OD category.
			get{
				return base.IsEnabled;
			}
			set{
				base.IsEnabled=value;
			}
		}

		[Category("OD")]
		[Description("This one property handles wrap and acceptsReturn.")]
		public bool IsMultiline {
			get{
				return _isMultiline;
			}
			set{
				_isMultiline=value;
				if(_isMultiline){
					textBox.TextWrapping=TextWrapping.Wrap;
					textBox.AcceptsReturn=true;
					textBox.VerticalScrollBarVisibility=ScrollBarVisibility.Auto;
					return;
				}
				textBox.TextWrapping=TextWrapping.NoWrap;
				textBox.AcceptsReturn=false;
					textBox.VerticalScrollBarVisibility=ScrollBarVisibility.Hidden;
			}
		}

		[Category("OD")]
		[Description("Set this true to hide the left, top, and right sides of the textbox. This makes it look more like a web textbox.")]
		[DefaultValue(false)]
		public bool IsUnderline{
			get{
				return _isUnderline;
			}
			set{
				_isUnderline=value;
				if(_isUnderline){
					textBox.Margin=new Thickness(0.5,0.5,0.5,bottom:0);//or it bleeds out
					border1.Visibility=Visibility.Visible;
					border2.Visibility=Visibility.Visible;
				}
				else{
					textBox.Margin=new Thickness(0);//back to the normal position
					border1.Visibility=Visibility.Collapsed;
					border2.Visibility=Visibility.Collapsed;
				}
			}
		}

		[Category("OD")]
		[Description("Default of 0 indicates no limit.")]
		[DefaultValue(0)]
		public int MaxLength { 
			get{
				return textBox.MaxLength;
			}
			set{
				textBox.MaxLength=value;
			}
		}

		///<summary>Sets padding on the text for odd situations. Default is 0. Can be negative.</summary>
		[Browsable(false)]
		public Thickness PaddingOD{
			get{
				return _paddingOD;
			}
			set{
				_paddingOD=value;
				textBox.Padding=_paddingOD;
			}
		}

		[Category("OD")]
		[Description("Set true to prevent user from editing. Enabled false also does this, but that also grays out the text. In WinForms, setting this to true also automatically changed the background color to 240 gray (#F0F0F0). In WPF, this is not paired, but can be done separately.")]
		public bool ReadOnly {
			//No need to do this for TextV... because the whole point of those is to validate input, so they would never be ReadOnly.
			get{
				return _readOnly;
			}
			set{
				//The MSWF textbox also changes backColor when changing this.  In WPF, they are independent.
				_readOnly=value;
				if(_readOnly){
					textBox.IsReadOnly=true;
					return;
				}
				textBox.IsReadOnly=false;
			}
		}

		[Browsable(false)]
		public string SelectedText{
			get {
				return textBox.SelectedText;
			}
			set {
				textBox.SelectedText=value;
			}
		}

		[Browsable(false)]
		public int SelectionLength { 
			get{
				return textBox.SelectionLength;
			}
			set{
				textBox.SelectionLength=value;
			}
		}

		[Browsable(false)]
		public int SelectionStart { 
			get{
				return textBox.SelectionStart;
			}
			set{
				textBox.SelectionStart=value;
			}
		}

		[Category("OD")]
		[DefaultValue(int.MaxValue)]
		[Description("Use this instead of TabIndex.")]
		public int TabIndexOD{
			get{
				return textBox.TabIndex;
			}
			set{
				textBox.TabIndex=value;
				//InvalidateVisual();//if we want the new TabIndex value to show immediately. But there's a performance hit, so no. Also no longer relevant.
			}
		}

		[Category("OD")]
		public string Text {
			get {
				return textBox.Text;
			}
			set {
				textBox.Text=value;
			}
		}

		///<summary>This property is for convenience. It toggles the Visibility property between Visible and Collapsed.</summary>
		[Browsable(false)]
		public bool Visible{
			get{
				if(Visibility==Visibility.Visible){
					return true;
				}
				return false;//Hidden or Collapsed
			}
			set{
				if(value){
					Visibility=Visibility.Visible;
					return;
				}
				Visibility=Visibility.Collapsed;
			}
		}
		#endregion Properties

		#region Methods - public
		///<summary>If you're looking for the overload with zero arguments, it's different in WPF. Use SelectAll() or Focus(), depending on your intent. Use SelectAll() to change focus and select all text. In WinForms, Select() was used to change focus without changing the selection (supposedly). The equivalent in WPF for that purpose is Focus().</summary>
		public void Select(int start,int length){
			SelectionStart=start;
			SelectionLength=length;
		}

		///<summary>Also sets focus</summary>
		public void SelectAll(){
			bool gotFocus=textBox.Focus();
			textBox.SelectAll();
		}
		#endregion Methods - public

		#region Methods - private event handlers
		private void TextBox_GotKeyboardFocus(object sender,KeyboardFocusChangedEventArgs e) {
			//string name=Name;
			//Debug.WriteLine("GOT KBFocus: nested textBox within "+name);
			if(e.KeyboardDevice.IsKeyDown(Key.Tab)){
				((System.Windows.Controls.TextBox)sender).SelectAll(); 
			}
		}

		private void TextBox_LostFocus(object sender,RoutedEventArgs e) {
			//rarely, a parent might need to subscribe to the LostFocus event, typically where Validating event was used in WinForms.
			//But we don't need to manually raise the LostFocus event for our usercontrol. 
			//When textbox loses focus, that event bubbles up through the usercontrol automatically.
		}

		private void TextBox_LostKeyboardFocus(object sender,KeyboardFocusChangedEventArgs e) {
			//string name=Name;
			//Debug.WriteLine("LOST KBFocus: nested textBox within "+name);
			//((System.Windows.Controls.TextBox)sender).Select(0,0);
		}

		private void TextBox_TextChanged(object sender,TextChangedEventArgs e) {
			TextChanged?.Invoke(this,new EventArgs());
		}

		private void TextBox_Unloaded(object sender,RoutedEventArgs e) {
			ScrollViewer scrollViewer=textBox.Template.FindName("PART_ContentHost",textBox) as ScrollViewer;
			if(scrollViewer!=null) {
				ScrollBar scrollBarVertical=scrollViewer.Template.FindName("PART_VerticalScrollBar",scrollViewer) as ScrollBar;
				if(scrollBarVertical!=null) {
					scrollBarVertical.Template=null;
				}
			}
		}

		private void This_GotKeyboardFocus(object sender,KeyboardFocusChangedEventArgs e) {
			//This does fire when user clicks on textBox
			//Somehow, this is also currently firing on startup, even though I don't think I set it.
			//string name=Name;//for debugging
			if(textBox.IsFocused){
				//Debug.WriteLine("GOT KBFocus: "+name+". Nested textBox already has focus.");
				return;
			}
			//else{
			//	Debug.WriteLine("GOT KBFocus: "+name+". Setting focus on nested textBox.");
			//}
			_isChangingFocusToTextBox=true;
			bool isFocused=textBox.Focus();
			//unfortunately, the above line causes a LostFocus to fire. We try to intercept and cancel that down in This_LostFocus.
			//if(isFocused){
			//	Debug.WriteLine("Setting focus was successful.");
			////}
			//else{
			//	Debug.WriteLine("Setting focus failed.");
			//}
		}
		
		private void This_IsEnabledChanged(object sender,DependencyPropertyChangedEventArgs e) {
			//This is nice because it gets hit when changing the property in the designer.
			SetColors();
		}

		private void This_LostFocus(object sender,RoutedEventArgs e) {
			if(_isChangingFocusToTextBox) {
				e.Handled=true;
				_isChangingFocusToTextBox=false;
			}
		}

		private void This_LostKeyboardFocus(object sender,KeyboardFocusChangedEventArgs e) {
			//string name=Name;
			//Debug.WriteLine("LOST KBFocus: "+name);
		}

		private void This_PreviewMouseLeftButtonDown(object sender,MouseButtonEventArgs e) {
			Click?.Invoke(this,new EventArgs());
		}
		#endregion Methods - private event handlers

		#region Methods - private
		///<summary></summary>
		private void SetColors(){
			//Nothing to do here.  The textbox that this control wraps already turns gray on is own.
		}
		#endregion Methods - private
	}
}
