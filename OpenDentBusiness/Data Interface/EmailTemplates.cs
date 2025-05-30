using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{
	///<summary>emailtemplates are refreshed as local data.</summary>
	public class EmailTemplates {
		#region CachePattern

		private class EmailTemplateCache : CacheListAbs<EmailTemplate> {
			protected override List<EmailTemplate> GetCacheFromDb() {
				string command="SELECT * from emailtemplate ORDER BY Description";
				return Crud.EmailTemplateCrud.SelectMany(command);
			}
			protected override List<EmailTemplate> TableToList(DataTable table) {
				return Crud.EmailTemplateCrud.TableToList(table);
			}
			protected override EmailTemplate Copy(EmailTemplate emailTemplate) {
				return emailTemplate.Copy();
			}
			protected override DataTable ListToTable(List<EmailTemplate> listEmailTemplates) {
				return Crud.EmailTemplateCrud.ListToTable(listEmailTemplates,"EmailTemplate");
			}
			protected override void FillCacheIfNeeded() {
				EmailTemplates.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EmailTemplateCache _emailTemplateCache=new EmailTemplateCache();

		public static List<EmailTemplate> GetDeepCopy(bool isShort=false) {
			return _emailTemplateCache.GetDeepCopy(isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_emailTemplateCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_emailTemplateCache.FillCacheFromTable(table);
				return table;
			}
			return _emailTemplateCache.GetTableFromCache(doRefreshCache);
		}

		public static void ClearCache() {
			_emailTemplateCache.ClearCache();
		}
		#endregion

		///<summary></summary>
		public static long Insert(EmailTemplate emailTemplate) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				emailTemplate.EmailTemplateNum=Meth.GetLong(MethodBase.GetCurrentMethod(),emailTemplate);
				return emailTemplate.EmailTemplateNum;
			}
			return Crud.EmailTemplateCrud.Insert(emailTemplate);
		}

		///<summary></summary>
		public static void Update(EmailTemplate emailTemplate){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailTemplate);
				return;
			}
			Crud.EmailTemplateCrud.Update(emailTemplate);
		}

		///<summary></summary>
		public static void Delete(EmailTemplate emailTemplate){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailTemplate);
				return;
			}
			string command= "DELETE from emailtemplate WHERE EmailTemplateNum = '"+emailTemplate.EmailTemplateNum.ToString()+"'";
 			Db.NonQ(command);
		}
	}

	
	

}













