using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using OpenDentBusiness;
using System.Linq;

namespace OpenDentBusiness{
	///<summary>Handles database commands related to the apptviewitem table in the database.</summary>
	public class ApptViewItems{
		#region CachePattern

		private class ApptViewItemCache : CacheListAbs<ApptViewItem> {
			protected override List<ApptViewItem> GetCacheFromDb() {
				string command="SELECT * from apptviewitem ORDER BY ElementOrder";
				return Crud.ApptViewItemCrud.SelectMany(command);
			}
			protected override List<ApptViewItem> TableToList(DataTable table) {
				return Crud.ApptViewItemCrud.TableToList(table);
			}
			protected override ApptViewItem Copy(ApptViewItem apptViewItem) {
				return apptViewItem.Clone();
			}
			protected override DataTable ListToTable(List<ApptViewItem> listApptViewItems) {
				return Crud.ApptViewItemCrud.ListToTable(listApptViewItems,"ApptViewItem");
			}
			protected override void FillCacheIfNeeded() {
				ApptViewItems.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ApptViewItemCache _apptViewItemCache=new ApptViewItemCache();

		///<summary>Gets a deep copy of all matching items from the cache via ListLong.  Set isShort true to search through ListShort instead.</summary>
		public static List<ApptViewItem> GetWhere(Predicate<ApptViewItem> match,bool isShort=false) {
			return _apptViewItemCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_apptViewItemCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_apptViewItemCache.FillCacheFromTable(table);
				return table;
			}
			return _apptViewItemCache.GetTableFromCache(doRefreshCache);
		}

		public static void ClearCache() {
			_apptViewItemCache.ClearCache();
		}
		#endregion

		///<summary></summary>
		public static long Insert(ApptViewItem apptViewItem){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				apptViewItem.ApptViewItemNum=Meth.GetLong(MethodBase.GetCurrentMethod(),apptViewItem);
				return apptViewItem.ApptViewItemNum;
			}
			return Crud.ApptViewItemCrud.Insert(apptViewItem);
		}

		///<summary></summary>
		public static void Update(ApptViewItem apptViewItem) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),apptViewItem);
				return;
			}
			Crud.ApptViewItemCrud.Update(apptViewItem);
		}

		///<summary></summary>
		public static void Delete(ApptViewItem apptViewItem) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),apptViewItem);
				return;
			}
			string command="DELETE from apptviewitem WHERE ApptViewItemNum = '"
				+POut.Long(apptViewItem.ApptViewItemNum)+"'";
			Db.NonQ(command);
		}

		///<summary>Deletes all apptviewitems for the current apptView. If isMobile, only deletes IsMobile apptviewitems and will not change basic appointment view items</summary>
		public static void DeleteAllForView(ApptView apptView, bool isMobile=false){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),apptView,isMobile);
				return;
			}
			string command=$@"DELETE from apptviewitem WHERE ApptViewNum = {POut.Long(apptView.ApptViewNum)}
			 AND {(isMobile? "":"!")}apptviewitem.IsMobile"; //only delete mobile items if isMobile, otherwise don't delete mobile items.
			Db.NonQ(command);
		}

		///<summary>Returns a list of ApptViewItem for a given Provider.</summary>
		public static List<ApptViewItem> GetForProvider(long provNum) {
			Meth.NoCheckMiddleTierRole();
			return ApptViewItems.GetWhere(x => x.ProvNum==provNum);
		}

		///<summary>Gets all operatories for the appointment view passed in.  Pass 0 to get all ops associated with the 'none' view.
		///Only returns operatories that are associated to the currently selected clinic.</summary>
		public static List<long> GetOpsForView(long apptViewNum) {
			Meth.NoCheckMiddleTierRole();
			List<long> listOpNums=new List<long>();
			if(apptViewNum==0) {
				bool hasClinicsEnabled=PrefC.HasClinicsEnabled;
				//Simply return all visible ops.  These are the ops that the 'none' appointment view currently displays.
				//Do not consider operatories that are not associated with the currently selected clinic.
				return Operatories.GetWhere(x => !hasClinicsEnabled || Clinics.ClinicNum==0 || x.ClinicNum==Clinics.ClinicNum,true)
					.Select(x => x.OperatoryNum).ToList();
			}
			return ApptViewItems.GetWhere(x => x.ApptViewNum==apptViewNum && x. OpNum!=0).Select(x => x.OpNum).ToList();
		}

		///<summary>Gets all views that contain the operatory num that is passed in. Can return an empty list.</summary>
		public static List<long> GetViewsByOp(long opNum) {
			Meth.NoCheckMiddleTierRole();
			List<long> listApptViewNums=new List<long>();
			listApptViewNums=GetWhere(x=>x.OpNum==opNum).Select(x=>x.ApptViewNum).ToList();
			return listApptViewNums;
		}

		///<summary>Gets all providers for the appointment view passed in.  Pass 0 to get all provs associated with the 'none' view.</summary>
		public static List<long> GetProvsForView(long apptViewNum) {
			Meth.NoCheckMiddleTierRole();
			if(apptViewNum==0) {
				//Simply return all visible ops.  These are the ops that the 'none' appointment view currently displays.
				List<Operatory> listOperatoriesVis=Operatories.GetWhere(x => !PrefC.HasClinicsEnabled || Clinics.ClinicNum==0 || x.ClinicNum==Clinics.ClinicNum
					,true);
				List<long> listProvNums=listOperatoriesVis.Where(x=>x.ProvDentist!=0).Select(x => x.ProvDentist).ToList();
				listProvNums.AddRange(listOperatoriesVis.Where(x=>x.ProvHygienist!=0).Select(x => x.ProvHygienist));
				return listProvNums.Distinct().ToList();
			}
			return GetWhere(x => x.ApptViewNum==apptViewNum && x.ProvNum!=0)
				.Select(x => x.ProvNum).ToList();
		}

	}
	
}