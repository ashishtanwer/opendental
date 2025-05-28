using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Globalization;
using Newtonsoft.Json;
using CodeBase;
using OpenDentBusiness.WebTypes;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsFromMobiles{
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SmsFromMobile> Refresh(long patNum){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<SmsFromMobile>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM smsfrommobile WHERE PatNum = "+POut.Long(patNum);
			return Crud.SmsFromMobileCrud.SelectMany(command);
		}

		///<summary>Gets one SmsFromMobile from the db.</summary>
		public static SmsFromMobile GetOne(long smsFromMobileNum){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				return Meth.GetObject<SmsFromMobile>(MethodBase.GetCurrentMethod(),smsFromMobileNum);
			}
			return Crud.SmsFromMobileCrud.SelectOne(smsFromMobileNum);
		}
		


		///<summary></summary>
		public static void Update(SmsFromMobile smsFromMobile){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsFromMobile);
				return;
			}
			Crud.SmsFromMobileCrud.Update(smsFromMobile);
		}

		///<summary></summary>
		public static void Delete(long smsFromMobileNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsFromMobileNum);
				return;
			}
			string command= "DELETE FROM smsfrommobile WHERE SmsFromMobileNum = "+POut.Long(smsFromMobileNum);
			Db.NonQ(command);
		}
		*/

		///<summary>Structured data to be stored as json List in Signalod.MsgValue for InvalidType.SmsTextMsgReceivedUnreadCount.</summary>
		public class SmsNotification {
			[JsonProperty(PropertyName = "A")]
			public long ClinicNum { get; set; }
			[JsonProperty(PropertyName = "B")]
			public int Count { get; set; }

			public static string GetJsonFromList(List<SmsNotification> listSmsNotifications) {
				return JsonConvert.SerializeObject(listSmsNotifications);
			}

			public static List<SmsNotification> GetListFromJson(string json) {
				Meth.NoCheckMiddleTierRole();
				List<SmsNotification> listSmsNotifications=null;
				ODException.SwallowAnyException(() => listSmsNotifications=JsonConvert.DeserializeObject<List<SmsNotification>>(json));
				return listSmsNotifications;
			}
		}

		///<summary>Returns the number of messages which have not yet been read.  If there are no unread messages, then empty string is returned.  If more than 99 messages are unread, then "99" is returned.  The count limit is 99, because only 2 digits can fit in the SMS notification text.</summary>
		public static string GetSmsNotification() {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string command="SELECT COUNT(*) FROM smsfrommobile WHERE SmsStatus="+POut.Int((int)SmsFromStatus.ReceivedUnread);
			int smsUnreadCount=PIn.Int(Db.GetCount(command));
			if(smsUnreadCount==0) {
				return "";
			}
			if(smsUnreadCount>99) {
				return "99";
			}
			return smsUnreadCount.ToString();
		}

		///<summary>Call ProcessInboundSms instead.</summary>
		public static long Insert(SmsFromMobile smsFromMobile) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				smsFromMobile.SmsFromMobileNum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsFromMobile);
				return smsFromMobile.SmsFromMobileNum;
			}
			return Crud.SmsFromMobileCrud.Insert(smsFromMobile);
		}

		///<summary>Gets all SmsFromMobile entries that have been inserted or updated since dateStart, which should be in server time.</summary>
		public static List<SmsFromMobile> GetAllChangedSince(DateTime dateStart) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<SmsFromMobile>>(MethodBase.GetCurrentMethod(),dateStart);
			}
			string command="SELECT * from smsfrommobile WHERE SecDateTEdit >= "+POut.DateT(dateStart);
			return Crud.SmsFromMobileCrud.SelectMany(command);
		}

		///<summary>Gets all SMS incoming messages for the specified filters.</summary>
		///<param name="dateStart">If dateStart is 01/01/0001, then no start date will be used.</param>
		///<param name="dateEnd">If dateEnd is 01/01/0001, then no end date will be used.</param>
		///<param name="listClinicNums">Will filter by clinic only if not empty and patNum is -1.</param>
		///<param name="patNum">If patNum is not -1, then only the messages for the specified patient will be returned, otherwise messages for all 
		///patients will be returned.</param>
		///<param name="isMessageThread">Indicates if this is a message thread.</param>
		///<param name="phoneNumber">The phone number to search by. Should be just the digits, no formatting.</param>
		///<param name="arrayStatuses">Messages with these statuses will be found. If none, all statuses will be returned.</param>
		public static List<SmsFromMobile> GetMessages(DateTime dateStart,DateTime dateEnd,List<long> listClinicNums,long patNum,
			bool isMessageThread,string phoneNumber,List<SmsFromStatus> listSmsFromStatuses) 
		{
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<SmsFromMobile>>(MethodBase.GetCurrentMethod(),dateStart,dateEnd,listClinicNums,patNum,isMessageThread,phoneNumber,listSmsFromStatuses);
			}
			List <string> listCommandFilters=new List<string>();
			if(dateStart>DateTime.MinValue) {
				listCommandFilters.Add(DbHelper.DtimeToDate("DateTimeReceived")+">="+POut.Date(dateStart));
			}
			if(dateEnd>DateTime.MinValue) {
				listCommandFilters.Add(DbHelper.DtimeToDate("DateTimeReceived")+"<="+POut.Date(dateEnd));
			}
			if(patNum==-1) {
				//Only limit clinic if not searching for a particular PatNum.
				if(listClinicNums.Count>0) {
					listCommandFilters.Add("ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+")");
				}
			}
			else {
				listCommandFilters.Add($"PatNum = {POut.Long(patNum)}");
			}
			if(!string.IsNullOrEmpty(phoneNumber)) {
				listCommandFilters.Add($"MobilePhoneNumber='{POut.String(phoneNumber)}'");
			}
			if(!isMessageThread) { //Always show unread in the grid.
				listSmsFromStatuses.Add(SmsFromStatus.ReceivedUnread);
			}
			if(listSmsFromStatuses.Count>0) {
				listCommandFilters.Add("SmsStatus IN ("+string.Join(",",listSmsFromStatuses.GroupBy(x => x).Select(x => POut.Int((int)x.Key)))+")");
			}
			string command="SELECT * FROM smsfrommobile";
			if(listCommandFilters.Count>0) {
				command+=" WHERE "+string.Join(" AND ",listCommandFilters);
			}
			return Crud.SmsFromMobileCrud.SelectMany(command);
		}

		///<summary>Attempts to find exact match for patient. If found, creates commlog, associates Patnum, and inserts into DB.
		///Otherwise, it simply inserts SmsFromMobiles into the DB. ClinicNum should have already been set before calling this function.</summary>
		public static void ProcessInboundSms(List<SmsFromMobile> listSmsFromMobilesMessages) {
			Meth.NoCheckMiddleTierRole();
			if(listSmsFromMobilesMessages==null || listSmsFromMobilesMessages.Count==0) {
				return;
			}
			List<SmsBlockPhone> listSmsBlockPhones=SmsBlockPhones.GetDeepCopy();
			for(int i=0;i<listSmsFromMobilesMessages.Count;i++) {
				SmsFromMobile smsFromMobile=listSmsFromMobilesMessages[i];
				if(listSmsBlockPhones.Any(x => TelephoneNumbers.AreNumbersEqual(x.BlockWirelessNumber,smsFromMobile.MobilePhoneNumber))) {
					continue;//The office has blocked this number.
				}
				smsFromMobile.DateTimeReceived=DateTime.Now;
				string countryCode=CultureInfo.CurrentCulture.Name.Substring(CultureInfo.CurrentCulture.Name.Length-2);
				if(smsFromMobile.SmsPhoneNumber!=SmsPhones.SHORTCODE) {
					SmsPhone smsPhone=SmsPhones.GetByPhone(smsFromMobile.SmsPhoneNumber);
					if(smsPhone!=null) {
						smsFromMobile.ClinicNum=smsPhone.ClinicNum;
						countryCode=smsPhone.CountryCode;
					}
				}
				if(!PrefC.HasClinicsEnabled) {
					//We want customer side records of this message to list SmsPhones.SHORTCODE as the number on which the message was sent.  This ensures we do
					//not record this communication on a different valid SmsPhone/VLN that it didn't truly take place on.  However, on the HQ side, we want 
					//records of this communication to be listed as having taken place on the actual Short Code number.  In the case of a Short Code, 
					//sms.SmsPhoneNumber will read "SHORTCODE", which won't be found in the customer's SmsPhone table.  As a result, the code to use the
					//customer's SmsPhone.ClinicNum and Country code cannot be used.  Since this code was intended to handle the case where the customer had
					//turned clinics on->off, we will specifically check if the customer has disabled clinics and only then change the sms.ClinicNum.  
					//Otherwise, trust HQ sent the correct ClinicNum.  Since we expect to only use Short Codes in the US/Canada, we will trust the server we 
					//are processing inbound sms will have the correct country code, which will be used here.
					smsFromMobile.ClinicNum=0;
				}
				List<Patient> listPatientsAll=Patients.GetPatientsByPhone(smsFromMobile.MobilePhoneNumber,countryCode);
				//First look only for patients that match sms.ClinicNum.
				List<Patient> listPatients=listPatientsAll.FindAll(x => x.ClinicNum==smsFromMobile.ClinicNum);
				if(listPatients.Count==0) {
					//Couldn't find any patients that exactly match sms.ClinicNum.
					listPatients=listPatientsAll;
				}
				smsFromMobile.MatchCount=listPatients.Count;
				long patNum=0;
				if(listPatients.Count==1) {
					patNum=listPatients.First().PatNum;
				}
				else if(listPatients.Count==0) {
					patNum=0;//We could not find definitive match, 0 matches found.
				}
				else if(listPatients.DistinctBy(x => x.Guarantor).ToList().Count!=1) {
					patNum=0;//We could not find definitive match, more than one match found with different guarantors
				}
				else {
					patNum=listPatients.First().Guarantor;//More than one match, but all have the same guarantor.
				}
				if(patNum!=0) {
					smsFromMobile.PatNum=patNum;
					Commlog commlog=new Commlog();
					commlog.CommDateTime=smsFromMobile.DateTimeReceived;
					commlog.Mode_= CommItemMode.Text;
					commlog.Note=smsFromMobile.MsgText;
					commlog.PatNum=patNum;
					commlog.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.TEXT);
					commlog.SentOrReceived= CommSentOrReceived.Received;
					smsFromMobile.CommlogNum=Commlogs.Insert(commlog);
				}
				Insert(smsFromMobile);
				if(MobileAppDevices.IsClinicSignedUpForMobileWeb(smsFromMobile.ClinicNum)){//Check if clinic is signed up for ODMobile, and if so, send notifications.
					//Alert ODMobile where applicable.
					PushNotificationUtils.ODM_NewTextMessage(smsFromMobile,smsFromMobile.PatNum);
					//This is a workaround due to android push notifications no longer being supported for xamarin. Will only notify android users when ODMobile is running.
					MobileNotifications.ODM_NewTextMessage(smsFromMobile,smsFromMobile.PatNum);
				}
			}
			//We used to update the SmsNotification indicator via a queries and a signal here.  Now managed by the eConnector.
		}

		public static string GetSmsFromStatusDescript(SmsFromStatus smsFromStatus) {
			Meth.NoCheckMiddleTierRole();
			if(smsFromStatus==SmsFromStatus.ReceivedUnread) {
				return "Unread";
			}
			if(smsFromStatus==SmsFromStatus.ReceivedRead) {
				return "Read";
			}
			return "";
		}

		///<summary>Updates only the changed fields of the SMS text message (if any).</summary>
		public static bool Update(SmsFromMobile smsFromMobile,SmsFromMobile smsFromMobileOld) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),smsFromMobile,smsFromMobileOld);
			}
			return Crud.SmsFromMobileCrud.Update(smsFromMobile,smsFromMobileOld);
		}

		///<summary>Sets the status of the passed in list of SmsFromMobileNums to read.</summary>
		public static void MarkManyAsRead(List<long> listSmsFromMobileNums) {
			if(listSmsFromMobileNums.IsNullOrEmpty()) {
				return;
			}
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listSmsFromMobileNums);
				return;
			}
			string command="UPDATE smsfrommobile "
				+"SET SmsStatus="+POut.Enum<SmsFromStatus>(SmsFromStatus.ReceivedRead)+" "
				+"WHERE SmsFromMobileNum IN ("+string.Join(",",listSmsFromMobileNums)+")";
			Db.NonQ(command);
		}
	}
}