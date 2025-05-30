using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class HL7DefMessages{
		#region CachePattern

		private class HL7DefMessageCache : CacheListAbs<HL7DefMessage> {
			protected override List<HL7DefMessage> GetCacheFromDb() {
				string command="SELECT * FROM hl7defmessage ORDER BY ItemOrder";
				return Crud.HL7DefMessageCrud.SelectMany(command);
			}
			protected override List<HL7DefMessage> TableToList(DataTable table) {
				return Crud.HL7DefMessageCrud.TableToList(table);
			}
			protected override HL7DefMessage Copy(HL7DefMessage HL7DefMessage) {
				return HL7DefMessage.Clone();
			}
			protected override DataTable ListToTable(List<HL7DefMessage> listHL7DefMessages) {
				return Crud.HL7DefMessageCrud.ListToTable(listHL7DefMessages,"HL7DefMessage");
			}
			protected override void FillCacheIfNeeded() {
				HL7DefMessages.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static HL7DefMessageCache _HL7DefMessageCache=new HL7DefMessageCache();

		private static List<HL7DefMessage> GetDeepCopy(bool isShort=false) {
			return _HL7DefMessageCache.GetDeepCopy(isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_HL7DefMessageCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_HL7DefMessageCache.FillCacheFromTable(table);
				return table;
			}
			return _HL7DefMessageCache.GetTableFromCache(doRefreshCache);
		}

		public static void ClearCache() {
			_HL7DefMessageCache.ClearCache();
		}
		#endregion

		///<summary>Gets a list of all Messages for this def from the database. No child objects included.</summary>
		public static List<HL7DefMessage> GetShallowFromDb(long hL7DefNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<HL7DefMessage>>(MethodBase.GetCurrentMethod(),hL7DefNum);
			}
			string command="SELECT * FROM hl7defmessage WHERE HL7DefNum='"+POut.Long(hL7DefNum)+"' ORDER BY ItemOrder";
			return Crud.HL7DefMessageCrud.SelectMany(command);
		}

		///<summary>Gets a full deep list of all Messages for this def from cache.</summary>
		public static List<HL7DefMessage> GetDeepFromCache(long hL7DefNum) {
			List<HL7DefMessage> listHL7DefMessagesRet=new List<HL7DefMessage>();
			List<HL7DefMessage> listHL7DefMessages=GetDeepCopy();
			for(int i=0;i<listHL7DefMessages.Count;i++) {
				if(listHL7DefMessages[i].HL7DefNum==hL7DefNum) {
					listHL7DefMessagesRet.Add(listHL7DefMessages[i]);
					listHL7DefMessagesRet[listHL7DefMessagesRet.Count-1].ListHL7DefSegments=HL7DefSegments.GetDeepFromCache(listHL7DefMessages[i].HL7DefMessageNum);
				}
			}
			return listHL7DefMessagesRet;
		}

		///<summary>Gets a full deep list of all Messages for this def from the database.</summary>
		public static List<HL7DefMessage> GetDeepFromDb(long hL7DefNum) {
				List<HL7DefMessage> listHL7DefMessages=new List<HL7DefMessage>();
				listHL7DefMessages=GetShallowFromDb(hL7DefNum);
				for(int i=0;i<listHL7DefMessages.Count;i++) {
					listHL7DefMessages[i].ListHL7DefSegments=HL7DefSegments.GetDeepFromDb(listHL7DefMessages[i].HL7DefMessageNum);
				}
				return listHL7DefMessages;
		}

		///<summary></summary>
		public static long Insert(HL7DefMessage hL7DefMessage) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				hL7DefMessage.HL7DefMessageNum=Meth.GetLong(MethodBase.GetCurrentMethod(),hL7DefMessage);
				return hL7DefMessage.HL7DefMessageNum;
			}
			return Crud.HL7DefMessageCrud.Insert(hL7DefMessage);
		}

		///<summary></summary>
		public static void Update(HL7DefMessage hL7DefMessage) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),hL7DefMessage);
				return;
			}
			Crud.HL7DefMessageCrud.Update(hL7DefMessage);
		}

		///<summary></summary>
		public static void Delete(long hL7DefMessageNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),hL7DefMessageNum);
				return;
			}
			string command= "DELETE FROM hl7defmessage WHERE HL7DefMessageNum = "+POut.Long(hL7DefMessageNum);
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<HL7DefMessage> Refresh(long patNum){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<HL7DefMessage>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM hl7defmessage WHERE PatNum = "+POut.Long(patNum);
			return Crud.HL7DefMessageCrud.SelectMany(command);
		}

		///<summary>Gets one HL7DefMessage from the db.</summary>
		public static HL7DefMessage GetOne(long hL7DefMessageNum){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				return Meth.GetObject<HL7DefMessage>(MethodBase.GetCurrentMethod(),hL7DefMessageNum);
			}
			return Crud.HL7DefMessageCrud.SelectOne(hL7DefMessageNum);
		}

		*/
	}
}
