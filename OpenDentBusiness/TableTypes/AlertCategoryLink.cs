﻿using System;

namespace OpenDentBusiness {
	///<summary>Links an alert type to a category.</summary>
	[Serializable()]
	[CrudTable(IsSynchable=true)]
	public class AlertCategoryLink:TableBase{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long AlertCategoryLinkNum;
		///<summary>FK to AlertCategory.AlertCategoryNum.</summary>
		public long AlertCategoryNum;
		///<summary>Enum:AlertType Identifies what types of alert this row is associated to.</summary>
		public AlertType AlertType;

		public AlertCategoryLink() {
			
		}

		public AlertCategoryLink(long alertCategoryNum, AlertType alertType) {
			this.AlertCategoryNum=alertCategoryNum;
			this.AlertType=alertType;
		}

		///<summary></summary>
		public AlertCategoryLink Copy() {
			return (AlertCategoryLink)this.MemberwiseClone();
		}
	}
}

