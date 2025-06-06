﻿using OpenDentBusiness.WebTypes.AutoComm;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>AutoComm object for MsgToPay messages that have been queued or sent by the eConnector. The HQ version of this object is MsgToPayActive where a record is kept for ShortGuid/redirect purposes.
	///Inherits IAutoCommApptGuid since they will all be attached to appointments.</summary>
	public class MsgToPaySent:TableBase,IAutoCommApptGuid {
		///<summary>PK.</summary>
		[CrudColumn(IsPriKey=true)]
		public long MsgToPaySentNum;
		///<summary>FK to patient.PatNum for the corresponding patient.</summary>
		public long PatNum;
		///<summary>FK to patient.ClinicNum for the corresponding patient.</summary>
		public long ClinicNum;
		///<summary>FK to Appointment.AptNum</summary>
		public long ApptNum;
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime ApptDateTime;
		[CrudColumn(SpecialType=CrudSpecialColType.TimeSpanLong)]
		public TimeSpan TSPrior;
		///<summary>FK to Statement.StatementNum</summary>
		public long StatementNum;
		///<summary>Indicates status of message.</summary>
		public AutoCommStatus SendStatus;
		///<summary>Source of this object. Can be Manual (implemented) or EConnectorAutoComm (not yet implemented).</summary>
		public MsgToPaySource Source;
		///<summary></summary>
		public CommType MessageType=CommType.Invalid;
		///<summary>FK to primary key of appropriate table.</summary>
		[XmlIgnore]
		public long MessageFk;
		///<summary>Subject of the message.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.IsText)]
		public string Subject;
		///<summary>Content of the message.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.IsText)]
		public string Message;
		///<summary>Only used for manually sent emails.</summary>
		public EmailType EmailType;
		///<summary>Generated by OD. Timestamp when row is created.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		[XmlIgnore]
		public DateTime DateTimeEntry;
		///<summary>DateTime the message was sent.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		[XmlIgnore]
		public DateTime DateTimeSent;
		///<summary>Generated by OD in some cases and HQ in others. Any human readable error message generated by either HQ or EConnector. Used for debugging.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.IsText)]
		public string ResponseDescript;
		///<summary>FK to apptreminderrule.ApptReminderRuleNum. Allows us to look up the rules to determine how to send this apptcomm out.</summary>
		public long ApptReminderRuleNum;
		///<summary>Generated by HQ. Identifies this AutoCommGuid in future transactions between HQ and OD.</summary>
		public string ShortGUID;
		///<summary>Deprecated.  Use MessageFK and MessageType instead.FK to message table, ex. smstomobile.GuidMessage. Generated at HQ.  References 
		///'Mobile' to limit schema changes, since that field already existed and is serialized for payloads sent to WebServiceMainHQ.  May not necessarily 
		///be an identifier in the smstomobile table, ex. could be an EmailMessage.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string GuidMessageToMobile;
		///<summary>Contact information used for sending a message.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string Contact;
		///<summary>The template that will be used when creating the message.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string TemplateMessage;
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		[XmlIgnore]
		public DateTime DateTimeSendFailed;

		DateTime IAutoCommAppt.ApptDateTime {
			get {
				return ApptDateTime;
			}
			set {
				ApptDateTime=value;
			}
		}

		TimeSpan IAutoCommAppt.TSPrior {
			get {
				return TSPrior;
			}
			set {
				TSPrior=value;
			}
		}

		public List<CalendarIcsInfo> ListCalIcsInfos { get; set; }

		#region AutoCommSent
		string IAutoCommSent.GuidMessageToMobile {
			get {
				return GuidMessageToMobile;
			}
			set {
				GuidMessageToMobile=value;
			}
		}

		long IAutoCommSent.PatNum {
			get {
				return PatNum;
			} 
			set {
				PatNum=value;
			}
		}

		long IAutoCommSent.ClinicNum {
			get {
				return ClinicNum;
			}
			set {
				ClinicNum=value;
			}
		}

		string IAutoCommSent.Contact {
			get {
				return Contact;
			}
			set {
				Contact=value;
			}
		}
		AutoCommStatus IAutoCommSent.SendStatus {
			get {
				return SendStatus;
			}
			set {
				SendStatus=value;
			}
		}
		CommType IAutoCommSent.MessageType {
			get {
				return MessageType;
			}
			set {
				MessageType=value;
			}
		}
		string IAutoCommSent.TemplateMessage {
			get {
				return TemplateMessage;
			}
			set {
				TemplateMessage=value;
			}
		}
		long IAutoCommSent.MessageFk {
			get {
				return MessageFk;
			}
			set {
				MessageFk=value;
			}
		}
		string IAutoCommSent.Subject {
			get {
				return Subject;
			}
			set {
				Subject=value;
			}
		}
		string IAutoCommSent.Message {
			get {
				return Message;
			}
			set {
				Message=value;
			}
		}
		DateTime IAutoCommSent.DateTimeEntry {
			get {
				return DateTimeEntry;
			}
			set {
				DateTimeEntry=value;
			}
		}
		DateTime IAutoCommSent.DateTimeSent {
			get {
				return DateTimeSent;
			}
			set {
				DateTimeSent=value;
			}
		}
		string IAutoCommSent.ResponseDescript {
			get {
				return ResponseDescript;
			}
			set {
				ResponseDescript=value;
			}
		}
		long IAutoCommSent.ApptReminderRuleNum {
			get {
				return ApptReminderRuleNum;
			}
			set {
				ApptReminderRuleNum=value;
			}
		}
		string IAutoCommApptGuid.ShortGUID {
			get {
				return ShortGUID;
			}
			set {
				ShortGUID=value;
			}
		}

		long IAutoCommAppt.ApptNum {
			get {
				return ApptNum;
			}
			set {
				ApptNum=value;
			}
		}
		#endregion AutoCommSent

		public IAutoCommSent Copy() {
			return (MsgToPaySent)MemberwiseClone();
		}

		public bool HasSubject() {
			return false;
		}
	}

	///<summary>Source of a MsgToPaySent's creation. Currently only implemented for Manual messages.</summary>
	public enum MsgToPaySource {
		Undefined,
		Manual,
		EConnectorAutoComm,
	}
}
