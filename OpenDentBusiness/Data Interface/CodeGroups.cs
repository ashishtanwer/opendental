using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using CodeBase;

namespace OpenDentBusiness {
	///<summary></summary>
	public class CodeGroups {
		#region Cache Pattern
		private class CodeGroupCache : CacheListAbs<CodeGroup> {
			protected override List<CodeGroup> GetCacheFromDb() {
				string command="SELECT * FROM codegroup ORDER BY ItemOrder";
				return Crud.CodeGroupCrud.SelectMany(command);
			}
			protected override List<CodeGroup> TableToList(DataTable table) {
				return Crud.CodeGroupCrud.TableToList(table);
			}
			protected override CodeGroup Copy(CodeGroup codeGroup) {
				return codeGroup.Copy();
			}
			protected override DataTable ListToTable(List<CodeGroup> listCodeGroups) {
				return Crud.CodeGroupCrud.ListToTable(listCodeGroups,"CodeGroup");
			}
			protected override void FillCacheIfNeeded() {
				CodeGroups.GetTableFromCache(false);
			}
			protected override bool IsInListShort(CodeGroup codeGroup) {
				return codeGroup.IsVisible();
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static CodeGroupCache _codeGroupCache=new CodeGroupCache();

		public static void ClearCache() {
			_codeGroupCache.ClearCache();
		}

		public static List<CodeGroup> GetDeepCopy(bool isShort=false) {
			return _codeGroupCache.GetDeepCopy(isShort);
		}

		public static int GetCount(bool isShort=false) {
			return _codeGroupCache.GetCount(isShort);
		}

		public static bool GetExists(Predicate<CodeGroup> match,bool isShort=false) {
			return _codeGroupCache.GetExists(match,isShort);
		}

		public static int GetFindIndex(Predicate<CodeGroup> match,bool isShort=false) {
			return _codeGroupCache.GetFindIndex(match,isShort);
		}

		public static CodeGroup GetFirst(bool isShort=false) {
			return _codeGroupCache.GetFirst(isShort);
		}

		public static CodeGroup GetFirst(Func<CodeGroup,bool> match,bool isShort=false) {
			return _codeGroupCache.GetFirst(match,isShort);
		}

		public static CodeGroup GetFirstOrDefault(Func<CodeGroup,bool> match,bool isShort=false) {
			return _codeGroupCache.GetFirstOrDefault(match,isShort);
		}

		public static CodeGroup GetLast(bool isShort=false) {
			return _codeGroupCache.GetLast(isShort);
		}

		public static CodeGroup GetLastOrDefault(Func<CodeGroup,bool> match,bool isShort=false) {
			return _codeGroupCache.GetLastOrDefault(match,isShort);
		}

		public static List<CodeGroup> GetWhere(Predicate<CodeGroup> match,bool isShort=false) {
			return _codeGroupCache.GetWhere(match,isShort);
		}

		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_codeGroupCache.FillCacheFromTable(table);
		}

		///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientMT's cache.</summary>
		///<param name="doRefreshCache">If true, will refresh the cache if MiddleTierRole is ClientDirect or ServerWeb.</param> 
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_codeGroupCache.FillCacheFromTable(table);
				return table;
			}
			return _codeGroupCache.GetTableFromCache(doRefreshCache);
		}
		#endregion Cache Pattern

		#region Methods - Get
		///<summary>Determines whether or not a given Procedure Code is contained within a codegroup.</summary>
		public static bool IsProcInCodeGroup(string procCodeString,long codeGroupNum) {
			CodeGroup codeGroup=GetOne(codeGroupNum);
			//May be null when called from ComputeForOrdinal.
			if(codeGroup==null) {
				return false;
			}
			return codeGroup.ProcCodes.Contains(procCodeString);
		}

		///<summary>If true, this codegroup is hidden from the frequency limitations grid. Control of showing in age limitations grid is done separately using ShowInAgeLimit. Returns true if the codeGroupNum passed in is invalid. Returns false if 0 is passed in.</summary>
		public static bool IsHidden(long codeGroupNum) {
			Meth.NoCheckMiddleTierRole();
			if(codeGroupNum==0) {
				return false;
			}
			CodeGroup codeGroup=CodeGroups.GetOne(codeGroupNum);
			if(codeGroup==null) {
				return true;//Invalid CodeGroupNum passed in or database corruption.
			}
			return codeGroup.IsHidden;
		}

		///<summary>If true, this codegroup shows in Age Limitations grid. Control of showing in Freq Lim is done separately using IsHidden. Returns true if the codeGroupNum passed in is invalid. Returns false if 0 is passed in.</summary>
		public static bool IsShownInAgeLimit(long codeGroupNum) {
			Meth.NoCheckMiddleTierRole();
			if(codeGroupNum==0) {
				return false;
			}
			CodeGroup codeGroup=GetOne(codeGroupNum);
			if(codeGroup==null) {
				return true;//Invalid CodeGroupNum passed in or database corruption.
			}
			return codeGroup.ShowInAgeLimit;
		}

		public static long GetCodeGroupNumForCodeGroupFixed(EnumCodeGroupFixed enumCodeGroupFixed,bool isShort=true) {
			Meth.NoCheckMiddleTierRole();
			CodeGroup codeGroup=GetFirstOrDefault(x => x.CodeGroupFixed==enumCodeGroupFixed,isShort);
			if(codeGroup==null) {
				return 0;
			}
			return codeGroup.CodeGroupNum;
		}

		public static List<long> GetCodeNums(long codeGroupNum) {
			Meth.NoCheckMiddleTierRole();
			List<long> listCodeNums=new List<long>();
			CodeGroup codeGroup=GetOne(codeGroupNum);
			if(codeGroup!=null) {
				listCodeNums=ProcedureCodes.GetCodeNumsForProcCodes(codeGroup.ProcCodes);
			}
			return listCodeNums;
		}

		///<summary>Returns the GroupName, including '(hidden)' if isHidden is true, for the CodeGroup passed in.</summary>
		public static string GetGroupName(long codeGroupNum,bool isHidden=false) {
			Meth.NoCheckMiddleTierRole();
			CodeGroup codeGroup=GetOne(codeGroupNum);
			return GetGroupName(codeGroup,isHidden:isHidden);
		}

		///<summary>Returns the GroupName, including '(hidden)' if isHidden is true, for the CodeGroup passed in.</summary>
		public static string GetGroupName(CodeGroup codeGroup,bool isHidden=false) {
			Meth.NoCheckMiddleTierRole();
			string groupName="";
			if(codeGroup==null) {
				return groupName;
			}
			groupName=codeGroup.GroupName;
			if(isHidden) {
				groupName+=" "+Lans.g("CodeGroups","(hidden)");
			}
			return groupName;
		}

		///<summary>Grab one code group from the full list of codegroups in the cache.</summary>
		public static CodeGroup GetOne(long codeGroupNum) {
			Meth.NoCheckMiddleTierRole();
			return GetFirstOrDefault(x => x.CodeGroupNum==codeGroupNum);
		}

		public static CodeGroup GetOneForCodeGroupFixed(EnumCodeGroupFixed codeGroupFixed,bool isShort=true) {
			Meth.NoCheckMiddleTierRole();
			return GetFirstOrDefault(x => x.CodeGroupFixed==codeGroupFixed,isShort);
		}

		///<summary>Gets one CodeGroup from db. Returns null if not found. Used by the API.</summary>
		public static CodeGroup GetOneFromDb(long codeGroupNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<CodeGroup>(MethodBase.GetCurrentMethod(),codeGroupNum);
			}
			if(codeGroupNum==0) {
				return null;
			}
			string command="SELECT * FROM codegroup "
				+"WHERE CodeGroupNum = '"+POut.Long(codeGroupNum)+"'";
			return Crud.CodeGroupCrud.SelectOne(command);
		}

		///<summary>Should only be called when comparing the item order of non-zero codegroup nums.</summary>
		public static int GetOrder(long codeGroupNum) {
			return GetFirst(x=>x.CodeGroupNum==codeGroupNum).ItemOrder;
		}

		///<summary>Gets a list of all codegroups from the db. Returns an empty list if none are found. Used by the API.</summary>
		public static List<CodeGroup> GetManyFromDb() {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<CodeGroup>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM codegroup";
			return Crud.CodeGroupCrud.SelectMany(command);
		}

		///<summary>Used by the API to get a list of codegroups. Returns an empty list if none are found.</summary>
		public static List<CodeGroup> GetCodeGroupsForApi(int limit,int offset) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<CodeGroup>>(MethodBase.GetCurrentMethod(),limit,offset);
			}
			string command="SELECT * FROM codegroup ";
			command+="ORDER BY CodeGroupNum " //Ensure order for limit and offset
				+"LIMIT "+POut.Int(offset)+", "+POut.Int(limit);
			return Crud.CodeGroupCrud.SelectMany(command);
		}

		///<summary>Returns true if there is a code group with at least one valid procedure code associated with each EnumCodeGroupFixed. Otherwise; false.</summary>
		public static bool HasValidCodeGroupFixed(EnumCodeGroupFixed enumCodeGroupFixed) {
			Meth.NoCheckMiddleTierRole();
			CodeGroup codeGroup=GetOneForCodeGroupFixed(enumCodeGroupFixed);
			if(codeGroup==null) {
				return false;
			}
			List<long> listCodeNums=GetCodeNums(codeGroup.CodeGroupNum);
			if(listCodeNums.IsNullOrEmpty()) {
				return false;
			}
			if(listCodeNums.All(x => x==0)) {
				return false;
			}
			//There is a corresponding CodeGroup and it has at least one valid proc code.
			return true;
		}
		#endregion Methods - Get

		#region Methods - Modify
		///<summary></summary>
		public static long Insert(CodeGroup codeGroup) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				codeGroup.CodeGroupNum=Meth.GetLong(MethodBase.GetCurrentMethod(),codeGroup);
				return codeGroup.CodeGroupNum;
			}
			return Crud.CodeGroupCrud.Insert(codeGroup);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Returns true if db changes were made.</summary>
		public static bool Sync(List<CodeGroup> listCodeGroups,List<CodeGroup> listCodeGroupsOld) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listCodeGroups,listCodeGroupsOld);
			}
			return Crud.CodeGroupCrud.Sync(listCodeGroups,listCodeGroupsOld);
		}

		///<summary></summary>
		public static void Update(CodeGroup codeGroup) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),codeGroup);
				return;
			}
			Crud.CodeGroupCrud.Update(codeGroup);
		}
		#endregion Methods - Modify



	}
}