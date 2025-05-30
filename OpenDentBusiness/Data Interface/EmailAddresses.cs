using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EmailAddresses{
		public static readonly char[] ADDRESS_DELIMITERS=new char[] { ';',',' };
		//Jordan 9/24/2021 This is a bad cache pattern.  Cache should include all emailaddresses.
		//There should also be methods that return certain subsets of the cache instead of this being done ad hoc in forms, etc.
		//BrittanyM 1/21/2022 Cache now includes all email addresses 
		#region CachePattern

		private class EmailAddressCache : CacheListAbs<EmailAddress> {
			protected override List<EmailAddress> GetCacheFromDb() {
				string command="SELECT * FROM emailaddress ORDER BY EmailUsername";
				return Crud.EmailAddressCrud.SelectMany(command);
			}
			protected override List<EmailAddress> TableToList(DataTable table) {
				return Crud.EmailAddressCrud.TableToList(table);
			}
			protected override EmailAddress Copy(EmailAddress emailAddress) {
				return emailAddress.Clone();
			}
			protected override DataTable ListToTable(List<EmailAddress> listEmailAddresses) {
				return Crud.EmailAddressCrud.ListToTable(listEmailAddresses,"EmailAddress");
			}
			protected override void FillCacheIfNeeded() {
				EmailAddresses.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EmailAddressCache _emailAddressCache=new EmailAddressCache();

		public static List<EmailAddress> GetDeepCopy(bool isShort=false) {
			return _emailAddressCache.GetDeepCopy(isShort);
		}

		public static EmailAddress GetFirstOrDefault(Func<EmailAddress,bool> match,bool isShort=false) {
			return _emailAddressCache.GetFirstOrDefault(match,isShort);
		}

		public static List<EmailAddress> GetWhere(Predicate<EmailAddress> match,bool isShort=false) {
			return _emailAddressCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_emailAddressCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_emailAddressCache.FillCacheFromTable(table);
				return table;
			}
			return _emailAddressCache.GetTableFromCache(doRefreshCache);
		}

		public static void ClearCache() {
			_emailAddressCache.ClearCache();
		}
		#endregion

		///<summary>Gets the default email address for the clinic/practice. Takes a clinic num. 
		///If clinic num is 0 or there is no default for that clinic, it will get practice default. 
		///May return a new blank object.</summary>
		public static EmailAddress GetByClinic(long clinicNum,bool allowNullReturn=false) {
			Meth.NoCheckMiddleTierRole();
			EmailAddress emailAddress=null;
			Clinic clinic=Clinics.GetClinic(clinicNum);
			if(!PrefC.HasClinicsEnabled || clinic==null) {//No clinic, get practice default
				emailAddress=GetOne(PrefC.GetLong(PrefName.EmailDefaultAddressNum));
			}
			else {
				emailAddress=GetOne(clinic.EmailAddressNum);
				if(emailAddress==null) {//clinic.EmailAddressNum 0. Use default.
					emailAddress=GetOne(PrefC.GetLong(PrefName.EmailDefaultAddressNum));
				}
			}
			if(emailAddress!=null) {
				return emailAddress;
			}
			emailAddress=GetFirstOrDefault(x=> x.UserNum==0);//user didn't set a default, so just pick the first non-user email in the list.
			if(emailAddress!=null) {
				return emailAddress;
			}
			if(allowNullReturn) {
				return emailAddress;
			}
			//If there are no email addresses AT ALL!!
			emailAddress=new EmailAddress();//To avoid null checks.
			emailAddress.EmailPassword="";
			emailAddress.EmailUsername="";
			emailAddress.Pop3ServerIncoming="";
			emailAddress.SenderAddress="";
			emailAddress.SMTPserver="";
			return emailAddress;
		}

		///<summary>Executes a query to the database to get the email address associated to the passed-in user.  
		///Does not use the cache.  Returns null if no email address in the database matches the passed-in user.</summary>
		public static EmailAddress GetForUserDb(long userNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<EmailAddress>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM emailaddress WHERE emailaddress.UserNum = "+POut.Long(userNum);
			return Crud.EmailAddressCrud.SelectOne(command);
		}

		///<summary>Gets the default email address for new outgoing emails.
		///Will attempt to get the current user's email address first. 
		///If it can't find one, will return the clinic/practice default.
		///Can return a new blank email address if no email addresses are defined for the clinic/practice.</summary>
		public static EmailAddress GetNewEmailDefault(long userNum,long clinicNum) {
			Meth.NoCheckMiddleTierRole();
			EmailAddress emailAddress=GetForUserDb(userNum);
			if(emailAddress==null) {
				return GetByClinic(clinicNum);
			}
			return emailAddress;
		}

		///<summary>Gets one EmailAddress from the cached listt.  Might be null.</summary>
		public static EmailAddress GetOne(long emailAddressNum) {
			Meth.NoCheckMiddleTierRole();
			return GetFirstOrDefault(x => x.EmailAddressNum==emailAddressNum);
		}

		/// <summary>Gets an EmailAddress directly from the database. This should be used very rarely.</summary>
		public static EmailAddress GetOneFromDb(long emailAddressNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<EmailAddress>(MethodBase.GetCurrentMethod(),emailAddressNum);
			}
			return Crud.EmailAddressCrud.SelectOne(emailAddressNum);
		}
		
		///<summary>Overrides the emailAddress.SenderAddress with the EmailAliasOverride of the passed in clinic, if present. Only overrides for clinics
		///with the EmailAliasOverride field set. Overridden SenderAddress is in "Alias <name@email.com>" format. 
		///Will not override blank SenderAddress fields or those already using an alias. </summary>
		public static EmailAddress OverrideSenderAddressClinical(EmailAddress emailAddress,long clinicNum) {
			Meth.NoCheckMiddleTierRole();
			Clinic clinic=Clinics.GetClinic(clinicNum);
			if(clinic==null) {
				return emailAddress;
			}
			if(clinic.EmailAliasOverride=="") {
				return emailAddress;
			}
			if(!Regex.IsMatch(emailAddress.SenderAddress,"^[^<>]+$")) {//Contains '<' or '>', and is not blank
				return emailAddress;
			}
			emailAddress.SenderAddress=clinic.EmailAliasOverride+" <"+emailAddress.SenderAddress+">";
			return emailAddress;
		}

		///<summary>Returns true if the passed-in email username already exists in the cached list of non-user email addresses.</summary>
		public static bool AddressExists(string emailUserName,long emailAddressNumSkip=0) {
			Meth.NoCheckMiddleTierRole();
			List<EmailAddress> listEmailAddresses=GetWhere(x => x.UserNum==0 //skip any non-user emails
				&& x.EmailAddressNum!=emailAddressNumSkip //skip any emailAddressNums
				&& x.EmailUsername.Trim().ToLower()==emailUserName.Trim().ToLower());
			if(listEmailAddresses.Count > 0) {
				return true;
			}
			return false;
		}

		///<summary>Gets all email addresses, including those email addresses which are not in the cache.</summary>
		public static List<EmailAddress> GetAll() {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<EmailAddress>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM emailaddress";
			return Crud.EmailAddressCrud.SelectMany(command);
		}

		///<summary>Gets email addresses that have been set up for downloading from the service</summary>
		public static List<EmailAddress> GetAllForDownloadFromCache() {
			Meth.NoCheckMiddleTierRole();
			return _emailAddressCache.GetWhere(x=>!string.IsNullOrWhiteSpace(x.Pop3ServerIncoming)).ToList();
		}

		///<summary>Checks to make sure at least one non-user email address has a valid (not blank) SMTP server.</summary>
		public static bool ExistsValidEmail() {
			Meth.NoCheckMiddleTierRole();
			EmailAddress emailAddress=GetFirstOrDefault(x => x.UserNum==0 && x.SMTPserver!="");
			return (emailAddress!=null);
		}

		///<summary>If the given email address is a valid email address, it returns the email address as a MailAddress. If it's not valid, it returns null.</summary>
		public static MailAddress GetValidMailAddress(string strEmailAddress) {
			Meth.NoCheckMiddleTierRole();
			if(string.IsNullOrWhiteSpace(strEmailAddress)) {
				return null;
			}
			try {
				//Normalize the domain
				strEmailAddress=Regex.Replace(strEmailAddress,@"(@)(.+)$",MatchEvaluatorDomain,RegexOptions.None,TimeSpan.FromMilliseconds(100));
			}
			catch(Exception) {
				return null;
			}
			strEmailAddress=strEmailAddress.Trim().ToLower();
			MailAddress mailAddress;
			try {
				mailAddress=new MailAddress(strEmailAddress);//Try needed in case of malformed email address. MailAddress ctor will throw UE
			}
			catch {
				return null;
			}
			//We only want to look at the actual email address, and exclude any aliases.
			if(!HasValidChars(mailAddress.Address)) {
				return null;
			}
			if(Regex.IsMatch(mailAddress.Address,
				@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z_]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z_])@))" +
				@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
				RegexOptions.IgnoreCase,TimeSpan.FromMilliseconds(100)))
			{
				return mailAddress;
			}
			return null;
		}

		///<summary>Helper function for GetValidMailAddress(). Examines the domain part of the email and normalizes it.</summary>
		private static string MatchEvaluatorDomain(Match match) {
			//Per a previous summary comment, the following DomainMapper and RegularExpresion "Comes from Microsoft itself".
			//Use IdnMapping class to convert Unicode domain names.
			IdnMapping idnMapping=new IdnMapping();
			//Pull out and process domain name (throws ArgumentException on invalid)
			string domainNormalized=idnMapping.GetAscii(match.Groups[2].Value);
			return match.Groups[1].Value+domainNormalized;
		}

		///<summary>Central method for getting email address for combo boxes in various forms. This method will return in the order listed:
		///1. A dummy email used to represent the default Practice/Clinic
		///2. The current user's email
		///3. Any email addresses that are not associated to other users or clinics</summary>
		public static List<EmailAddress> GetEmailAddressesForComboBoxes(long userNum) {
			Meth.NoCheckMiddleTierRole();
			//Email Address to include:
			//~Current user's email
			//~Emails that aren't associated with a clinic
			//~Emails that aren't the default
			//~Emails not associated to a user
			List<long> listEmailAddressNumsToExclude=new List<long>();
			if(PrefC.HasClinicsEnabled) {
				listEmailAddressNumsToExclude.AddRange(Clinics.GetDeepCopy().Select(x=>x.EmailAddressNum).Distinct());
			}
			//Exclude default practice email address.
			listEmailAddressNumsToExclude.Add(PrefC.GetLong(PrefName.EmailDefaultAddressNum));
			//Exclude web mail notification email address.
			listEmailAddressNumsToExclude.Add(PrefC.GetLong(PrefName.EmailNotifyAddressNum));
			//include a dummy default
			List<EmailAddress> listEmailAddresses=new List<EmailAddress>();
			EmailAddress emailAddress=new EmailAddress();
			emailAddress.EmailUsername=Lans.g("EmailAddress","Practice/Clinic");
			listEmailAddresses.Add(emailAddress);
			//include user email and emails not associated with a user.
			listEmailAddresses.AddRange(GetWhere(x=>!listEmailAddressNumsToExclude.Contains(x.EmailAddressNum) && 
				(x.UserNum==0 || x.UserNum==userNum))
				.OrderByDescending(x=>x.UserNum==userNum));
			return listEmailAddresses;
		}

		public static string GetDisplayStringForComboBox(EmailAddress emailAddress,long userNum,long emailAddressNumDefault=0) {
			Meth.NoCheckMiddleTierRole();
			if(emailAddressNumDefault!=0 && emailAddress.EmailAddressNum==emailAddressNumDefault) {
				return Lans.g("","Default")+" <"+emailAddress.EmailUsername+">";
			}
			if(userNum==emailAddress.UserNum) {
				return Lans.g("","Me")+" <"+emailAddress.EmailUsername+">";
			}	
			return emailAddress.EmailUsername;
		}

		///<summary>Compares the email address before and after ASII encoding to make sure we don't have characters that would be considered invalid</summary>
		public static bool HasValidChars(string strEmailAddress) {
			Meth.NoCheckMiddleTierRole();
			string addressBeforeEncoding=strEmailAddress;
			byte[] byteArray=Encoding.ASCII.GetBytes(addressBeforeEncoding);
			string addressAfterEncoding=Encoding.ASCII.GetString(byteArray);
			return string.Compare(addressBeforeEncoding,addressAfterEncoding,ignoreCase:true)==0;
		}

		///<summary>Splits and validates EmailAddress string into list of valid individual addresses.</summary>
		public static List<string> GetValidAddresses(string strEmailAddresses) {
			Meth.NoCheckMiddleTierRole();
			if(strEmailAddresses==null) {
				strEmailAddresses="";
			}
			List<string> listEmails=strEmailAddresses
				.Split(ADDRESS_DELIMITERS,StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Trim().ToLower())
				.ToList();
			List<string> listEmailsRet=new List<string>();
			MailAddress mailAddress=null;
			for(int i=0;i<listEmails.Count();i++) {
				mailAddress=GetValidMailAddress(listEmails[i]);
				if(mailAddress==null) {
					continue;
				}
				if(listEmailsRet.Contains(mailAddress.Address)){
					continue;//Only include unique addresses.
				}
				listEmailsRet.Add(mailAddress.Address);
			}
			return listEmailsRet;
		}

		///<summary></summary>
		public static long Insert(EmailAddress emailAddress) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				emailAddress.EmailAddressNum=Meth.GetLong(MethodBase.GetCurrentMethod(),emailAddress);
				return emailAddress.EmailAddressNum;
			}
			return Crud.EmailAddressCrud.Insert(emailAddress);
		}

		///<summary></summary>
		public static void Update(EmailAddress emailAddress){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailAddress);
				return;
			}
			Crud.EmailAddressCrud.Update(emailAddress);
		}

		///<summary></summary>
		public static void Delete(long emailAddressNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailAddressNum);
				return;
			}
			string command= "DELETE FROM emailaddress WHERE EmailAddressNum = "+POut.Long(emailAddressNum);
			Db.NonQ(command);
		}
	}
}