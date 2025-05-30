using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{

	///<summary>Letters are refreshed as local data.</summary>
	public class Letters{
		#region CachePattern

		private class LetterCache : CacheListAbs<Letter> {
			protected override List<Letter> GetCacheFromDb() {
				string command="SELECT * from letter ORDER BY Description";
				return Crud.LetterCrud.SelectMany(command);
			}
			protected override List<Letter> TableToList(DataTable table) {
				return Crud.LetterCrud.TableToList(table);
			}
			protected override Letter Copy(Letter Letter) {
				return Letter.Copy();
			}
			protected override DataTable ListToTable(List<Letter> listLetters) {
				return Crud.LetterCrud.ListToTable(listLetters,"Letter");
			}
			protected override void FillCacheIfNeeded() {
				Letters.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static LetterCache _LetterCache=new LetterCache();

		public static List<Letter> GetDeepCopy(bool isShort=false) {
			return _LetterCache.GetDeepCopy(isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_LetterCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_LetterCache.FillCacheFromTable(table);
				return table;
			}
			return _LetterCache.GetTableFromCache(doRefreshCache);
		}

		public static void ClearCache() {
			_LetterCache.ClearCache();
		}
		#endregion

		///<summary></summary>
		public static void Update(Letter letter){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),letter);
				return;
			}
			Crud.LetterCrud.Update(letter);
		}

		///<summary></summary>
		public static long Insert(Letter letter){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				letter.LetterNum=Meth.GetLong(MethodBase.GetCurrentMethod(),letter);
				return letter.LetterNum;
			}
			return Crud.LetterCrud.Insert(letter);
		}

		///<summary></summary>
		public static void Delete(Letter letter){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),letter);
				return;
			}
			string command="DELETE from letter WHERE LetterNum = '"+letter.LetterNum.ToString()+"'";
			Db.NonQ(command);
		}
	}

	
	

}













