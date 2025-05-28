using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class HL7DefSegments{
		#region CachePattern

		private class HL7DefSegmentCache : CacheListAbs<HL7DefSegment> {
			protected override List<HL7DefSegment> GetCacheFromDb() {
				string command="SELECT * FROM hl7defsegment ORDER BY ItemOrder";
				return Crud.HL7DefSegmentCrud.SelectMany(command);
			}
			protected override List<HL7DefSegment> TableToList(DataTable table) {
				return Crud.HL7DefSegmentCrud.TableToList(table);
			}
			protected override HL7DefSegment Copy(HL7DefSegment HL7DefSegment) {
				return HL7DefSegment.Clone();
			}
			protected override DataTable ListToTable(List<HL7DefSegment> listHL7DefSegments) {
				return Crud.HL7DefSegmentCrud.ListToTable(listHL7DefSegments,"HL7DefSegment");
			}
			protected override void FillCacheIfNeeded() {
				HL7DefSegments.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static HL7DefSegmentCache _HL7DefSegmentCache=new HL7DefSegmentCache();

		public static List<HL7DefSegment> GetDeepCopy(bool isShort=false) {
			return _HL7DefSegmentCache.GetDeepCopy(isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_HL7DefSegmentCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_HL7DefSegmentCache.FillCacheFromTable(table);
				return table;
			}
			return _HL7DefSegmentCache.GetTableFromCache(doRefreshCache);
		}

		public static void ClearCache() {
			_HL7DefSegmentCache.ClearCache();
		}
		#endregion

		/// <summary>Gets it straight from the database instead of from cache. No child objects included.</summary>
		public static List<HL7DefSegment> GetShallowFromDb(long hL7DefMessageNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<HL7DefSegment>>(MethodBase.GetCurrentMethod(),hL7DefMessageNum);
			}
			string command="SELECT * FROM hl7defsegment WHERE HL7DefMessageNum='"+POut.Long(hL7DefMessageNum)+"' ORDER BY ItemOrder";
			return Crud.HL7DefSegmentCrud.SelectMany(command);
		}

		///<summary>Gets deep list from cache.</summary>
		public static List<HL7DefSegment> GetDeepFromCache(long hL7DefMessageNum) {
			List<HL7DefSegment> listHL7DefSegmentsRet=new List<HL7DefSegment>();
			List<HL7DefSegment> listHL7DefSegments=GetDeepCopy(false);
			for(int i=0;i<listHL7DefSegments.Count;i++) {
				if(listHL7DefSegments[i].HL7DefMessageNum==hL7DefMessageNum) {
					listHL7DefSegmentsRet.Add(listHL7DefSegments[i]);
					listHL7DefSegmentsRet[listHL7DefSegmentsRet.Count-1].hl7DefFields=HL7DefFields.GetFromCache(listHL7DefSegments[i].HL7DefSegmentNum);
				}
			}
			return listHL7DefSegmentsRet;
		}

		///<summary>Gets a full deep list of all Segments for this message from the database.</summary>
		public static List<HL7DefSegment> GetDeepFromDb(long hL7DefMessageNum) {
			List<HL7DefSegment> listHL7DefSegments=new List<HL7DefSegment>();
			listHL7DefSegments=GetShallowFromDb(hL7DefMessageNum);
			for(int i=0;i<listHL7DefSegments.Count;i++) {
				listHL7DefSegments[i].hl7DefFields=HL7DefFields.GetFromDb(listHL7DefSegments[i].HL7DefSegmentNum);
			}
			return listHL7DefSegments;
		}

		///<summary></summary>
		public static long Insert(HL7DefSegment hL7DefSegment) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				hL7DefSegment.HL7DefSegmentNum=Meth.GetLong(MethodBase.GetCurrentMethod(),hL7DefSegment);
				return hL7DefSegment.HL7DefSegmentNum;
			}
			return Crud.HL7DefSegmentCrud.Insert(hL7DefSegment);
		}

		///<summary></summary>
		public static void Update(HL7DefSegment hL7DefSegment) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),hL7DefSegment);
				return;
			}
			Crud.HL7DefSegmentCrud.Update(hL7DefSegment);
		}

		///<summary></summary>
		public static void Delete(long hL7DefSegmentNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),hL7DefSegmentNum);
				return;
			}
			string command= "DELETE FROM hl7defsegment WHERE HL7DefSegmentNum = "+POut.Long(hL7DefSegmentNum);
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<HL7DefSegment> Refresh(long patNum){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<HL7DefSegment>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM hl7defsegment WHERE PatNum = "+POut.Long(patNum);
			return Crud.HL7DefSegmentCrud.SelectMany(command);
		}

		///<summary>Gets one HL7DefSegment from the db.</summary>
		public static HL7DefSegment GetOne(long hL7DefSegmentNum){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				return Meth.GetObject<HL7DefSegment>(MethodBase.GetCurrentMethod(),hL7DefSegmentNum);
			}
			return Crud.HL7DefSegmentCrud.SelectOne(hL7DefSegmentNum);
		}

		*/
	}
}
