﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDental.UIManagement{
	///<summary>This keeps track of the original control working area bounds, font, anchors, docks, etc, all at 96dpi.</summary>
	public class Contr96Info {
		///<summary></summary>
		public AnchorStyles Anchor;
		///<summary>This takes priority over anchor.</summary>
		public DockStyle Dock;
		///<summary>Helps with troubleshooting. Some controls do not have names, and other names are duplicates.  For example, vertical scrollbars on two different grids.</summary>
		public string Name;
		///<summary>The current calculated bounds of this control at 96dpi pixels. Not yet implemented for the form itself, but we could.</summary>
		public RectangleF RectangleF96Now;
		///<summary>The bounds of this control at 96dpi pixels as it would have been before parent resized or scale applied.  Starts out as the actual numbers from the designer.  Doesn't get changed with any automatic sizing so that it preserves the original spacial relationships.  It does get changed when a programmer intentionally moves a control, but the number stored here is as it would be in the original unsized form.  In a parent-child comparison, this is only useful for the child. For the parent side, use client size.</summary>
		public RectangleF RectangleF96Orig;
		///<summary>ClientSize at 96 dpi.  Children are laid out in relationship to this size. We can't store a calc ahead of time for SizeClient96Now because that wouldn't work for forms. We get that size on the fly.</summary>
		public SizeF SizeClient96Orig;
		public float SizeFont96;

		///<summary>This keeps track of control or form bounds and font, all at 96dpi.  This allows absolute positioning instead of relative positioning.</summary>
		public Contr96Info(){

		}
		
		public void SetFields(Control control){
			if(control.Name==""){
				//Maybe we should throw an exception here?  Seems like all controls must be named because we no longer have references.  But there can still be duplicates when there are double user controls.
				Name="Unnamed "+control.GetType().ToString();
			}
			else{
				Name = control.Name;
			}
			RectangleF96Orig=control.Bounds;
			if(control.Bounds.Height<0 || control.Bounds.Width<0){
				throw new Exception();
			}
			SizeClient96Orig=control.ClientSize;
			SizeFont96=control.Font.Size;
			Anchor=control.Anchor;
			Dock=control.Dock;
		}

		public override string ToString(){
			return Name;
		}
	}
}
