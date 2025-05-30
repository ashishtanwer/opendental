﻿using System;

namespace OpenDentBusiness {

	[Serializable]
	[CrudTable(IsMissingInGeneral=true,CrudExcludePrefC=true)]
	public class WebChatSession:TableBase {

		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long WebChatSessionNum;
		///<summary>Optional.  The name of the person who initiated the chat session.</summary>
		public string UserName;
		///<summary>Optional.  The name of the practice the user belongs to.</summary>
		public string PracticeName;
		///<summary>Optional.  The email address of the user.</summary>
		public string EmailAddress;
		///<summary>Optional.  The phone number of the user.</summary>
		public string PhoneNumber;
		///<summary>Required.  True if the user has stated that they are a current customer, false otherwise.</summary>
		public bool IsCustomer;
		///<summary>Optional.  The subject of the chat session.</summary>
		public string QuestionText;
		///<summary>Optional.  The name of the technician who is in the chat session.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.IsText)]
		public string TechName;
		///<summary>The date and time the chat was created.  Can never be modified.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTcreated;
		///<summary>The date and time the session ended.  Is set to 0001-01-01 if session is active.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTend;
		///<summary>FK to patient.PatNum.  Optional.  Stores the patient the WebChatSession is associated to.</summary>
		public long PatNum;
		///<summary>Stores the client IP address for this session</summary>
		public string IpAddress;
		///<summary>Stores the type of chat session this is, defaults to WebSupport for online chat support.</summary>
		public WebChatSessionType SessionType;

		///<summary></summary>
		public WebChatSession Clone() {
			return (WebChatSession)this.MemberwiseClone();
		}

	}

	public enum WebChatSessionType {
		WebSupport,
		OpenAi
	}
}