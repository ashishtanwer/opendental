using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class UserGroupAttaches{
		//If this table type will exist as cached data, uncomment the Cache Pattern region below and edit.

		#region Cache Pattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add GetTableFromCache and FillCacheFromTable to the Cache.cs file with all the other Cache types.
		//Also, consider making an invalid type for this class in Cache.GetAllCachedInvalidTypes() if needed.

		private class UserGroupAttachCache : CacheListAbs<UserGroupAttach> {
			protected override List<UserGroupAttach> GetCacheFromDb() {
				string command="SELECT * FROM usergroupattach";
				return Crud.UserGroupAttachCrud.SelectMany(command);
			}
			protected override List<UserGroupAttach> TableToList(DataTable table) {
				return Crud.UserGroupAttachCrud.TableToList(table);
			}
			protected override UserGroupAttach Copy(UserGroupAttach userGroupAttach) {
				return userGroupAttach.Copy();
			}
			protected override DataTable ListToTable(List<UserGroupAttach> listUserGroupAttaches) {
				return Crud.UserGroupAttachCrud.ListToTable(listUserGroupAttaches,"UserGroupAttach");
			}
			protected override void FillCacheIfNeeded() {
				UserGroupAttaches.GetTableFromCache(false);
			}
			//protected override bool IsInListShort(UserGroupAttach userGroupAttach) {
			//	return true;//Either change this method or delete it.
			//}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static UserGroupAttachCache _userGroupAttachCache=new UserGroupAttachCache();

		public static List<UserGroupAttach> GetWhere(Predicate<UserGroupAttach> match,bool isShort=false) {
			return _userGroupAttachCache.GetWhere(match,isShort);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			Meth.NoCheckMiddleTierRole();
			_userGroupAttachCache.FillCacheFromTable(table);
		}

		///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
		///<param name="refreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
		public static DataTable GetTableFromCache(bool refreshCache) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),refreshCache);
				_userGroupAttachCache.FillCacheFromTable(table);
				return table;
			}
			return _userGroupAttachCache.GetTableFromCache(refreshCache);
		}

		public static void ClearCache() {
			_userGroupAttachCache.ClearCache();
		}

		public static void RefreshCache() {
			GetTableFromCache(true);
		}
		#endregion Cache Pattern
		
		#region Get Methods
		///<summary>Returns all usergroupattaches for a single user from the cache.</summary>
		public static List<UserGroupAttach> GetForUser(long userNum) {
			Meth.NoCheckMiddleTierRole();
			return GetWhere(x => x.UserNum == userNum);
		}

		public static List<UserGroupAttach> GetForUserGroup(long userGroupNum) {
			Meth.NoCheckMiddleTierRole();
			return GetWhere(x => x.UserGroupNum== userGroupNum);
		}

		///<summary>Gets a list of UserGroupAttaches from the database with filters for userGroupNum and userNum. 
		///This method is for use by the API, please notify the API team before changing.</summary>
		public static List<UserGroupAttach> GetGroupAttachesForApi(int limit,int offset,long userGroupNum,long userNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<UserGroupAttach>>(MethodBase.GetCurrentMethod(),limit,offset,userGroupNum,userNum);
			}
			string command="SELECT * FROM usergroupattach ";
			if(userGroupNum>0) {
				command+="WHERE UserGroupNum="+POut.Long(userGroupNum)+" ";
			}
			if(userNum>0) {
				command+="WHERE UserNum="+POut.Long(userNum)+" ";
			}
			command+="ORDER BY UserGroupAttachNum "
				+"LIMIT "+POut.Int(offset)+", "+POut.Int(limit);
			return Crud.UserGroupAttachCrud.SelectMany(command);
		}

		///<summary>Gets all UserGroupAttaches from the database where the associated users or usergroups' CEMTNums are not 0.</summary>
		public static List<UserGroupAttach> GetForCEMTUsersAndUserGroups() {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<UserGroupAttach>>(MethodBase.GetCurrentMethod());
			}
			string command = @"
				SELECT usergroupattach.* 
				FROM usergroupattach
				INNER JOIN userod ON userod.UserNum = usergroupattach.UserNum
					AND userod.UserNumCEMT != 0";
			return Crud.UserGroupAttachCrud.SelectMany(command);
		}

		///<summary>Pass in a list of CEMT usergroupattaches, and this will return a list of corresponding local usergroupattaches.</summary>
		public static List<UserGroupAttach> TranslateCEMTToLocal(List<UserGroupAttach> listUserGroupAttachesCEMT) {
			List<UserGroupAttach> listUserGroupAttaches = new List<UserGroupAttach>();
			List<Userod> listUserodsRemote = Userods.GetUsersNoCache();
			List<UserGroup> listUserGroupsRemote = UserGroups.GetCEMTGroupsNoCache();
			for(int i=0;i<listUserGroupAttachesCEMT.Count;i++) {
				Userod userod = listUserodsRemote.FirstOrDefault(x => listUserGroupAttachesCEMT[i].UserNum == x.UserNumCEMT);
				UserGroup userGroup = listUserGroupsRemote.FirstOrDefault(x => listUserGroupAttachesCEMT[i].UserGroupNum == x.UserGroupNumCEMT);
				if(userod == null || userGroup == null) {
					continue;
				}
				UserGroupAttach userGroupAttachNew = new UserGroupAttach(); 
				userGroupAttachNew.UserNum = userod.UserNum;
				userGroupAttachNew.UserGroupNum = userGroup.UserGroupNum;
				listUserGroupAttaches.Add(userGroupAttachNew);
			}
			return listUserGroupAttaches;
		}

		///<summary>Pass in a list of UserGroups and return a distinct list of longs for the UserNums</summary>
		public static List<long> GetUserNumsForUserGroups(List<UserGroup> listUserGroups) {
			Meth.NoCheckMiddleTierRole();
			return GetUserNumsForUserGroups(listUserGroups.Select(x => x.UserGroupNum).ToList());
		}

		///<summary>Pass in a list of UserGroupNums and return a distinct list of longs for the UserNums</summary>
		public static List<long> GetUserNumsForUserGroups(List<long> listUserGroupNums) {
			Meth.NoCheckMiddleTierRole();
			return GetWhere(x => listUserGroupNums.Contains(x.UserGroupNum)).Select(x => x.UserNum).Distinct().ToList();
		}

		#endregion

		#region Insert
		///<summary></summary>
		public static long Insert(UserGroupAttach userGroupAttach) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				userGroupAttach.UserGroupAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),userGroupAttach);
				return userGroupAttach.UserGroupAttachNum;
			}
			return Crud.UserGroupAttachCrud.Insert(userGroupAttach);
		}
		#endregion

		#region Update
		///<summary>Manually sync the database on the lists passed in. This does not check the PKs of the items in either list.
		///Instead, it only cares about info in the UserGroupNum and UserNum columns.
		///Returns the number of rows that were changed. Currently only used in the CEMT tool.</summary>
		public static long SyncCEMT(List<UserGroupAttach> listUserGroupAttachesNew,List<UserGroupAttach> listUseGroupAttachesOld) {
			//This remoting role check isn't necessary but will save on network traffic
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),listUserGroupAttachesNew,listUseGroupAttachesOld);
			}
			//the users and usergroups in listNew correspond to UserNumCEMTs and UserGroupNumCEMTs.
			// - If a row with the same UserGroupNum and UserNum exists in ListNew that does not exist in list Old, add it to listAdd.
			// - If a row with the same UserGroupNum and UserNum exists in ListOld that does not exist in ListNew, add it to listDel.
			List<UserGroupAttach> listUserGroupAttachesAdd = new List<UserGroupAttach>();
			List<UserGroupAttach> listUserGroupAttachesDel = new List<UserGroupAttach>();
			long rowsChanged = 0;
			for(int i=0;i<listUserGroupAttachesNew.Count;i++) {
				if(!listUseGroupAttachesOld.Exists(x => x.UserGroupNum == listUserGroupAttachesNew[i].UserGroupNum 
					&& x.UserNum == listUserGroupAttachesNew[i].UserNum)) 
				{
					listUserGroupAttachesAdd.Add(listUserGroupAttachesNew[i]);
				}
			}
			for(int i=0;i<listUseGroupAttachesOld.Count;i++) {
				if(!listUserGroupAttachesNew.Exists(x => x.UserGroupNum == listUseGroupAttachesOld[i].UserGroupNum 
					&& x.UserNum == listUseGroupAttachesOld[i].UserNum))
				{
					listUserGroupAttachesDel.Add(listUseGroupAttachesOld[i]);
				}
			}
			//make sure that there is only one unique (UserGroup, UserGroupNum) row in the add list. (this is precautionary)
			listUserGroupAttachesAdd = listUserGroupAttachesAdd.GroupBy(x => new { x.UserNum,x.UserGroupNum }).Select(x => x.First()).ToList();
			//Get users and user groups from remote db to compare against for log entrys
			List<Userod> listUserodsRemote = Userods.GetUsersNoCache();
			List<UserGroup> listUserGroupsRemote = UserGroups.GetCEMTGroupsNoCache();
			for(int i=0;i<listUserGroupAttachesAdd.Count;i++) {
				rowsChanged++;
				UserGroupAttaches.Insert(listUserGroupAttachesAdd[i]);
				Userod userod=listUserodsRemote.FirstOrDefault(x => x.UserNum==listUserGroupAttachesAdd[i].UserNum);
				UserGroup userGroup=listUserGroupsRemote.FirstOrDefault(x => x.UserGroupNum==listUserGroupAttachesAdd[i].UserGroupNum);
				SecurityLogs.MakeLogEntryNoCache(EnumPermType.SecurityAdmin,0,"User: "+userod.UserName+" added to user group: "
					+userGroup.Description+" by CEMT user: "+Security.CurUser.UserName);
			}
			for(int i=0;i<listUserGroupAttachesDel.Count;i++) {
				rowsChanged++;
				UserGroupAttaches.Delete(listUserGroupAttachesDel[i]);
				Userod userod=listUserodsRemote.FirstOrDefault(x => x.UserNum==listUserGroupAttachesDel[i].UserNum);
				UserGroup userGroup=listUserGroupsRemote.FirstOrDefault(x => x.UserGroupNum==listUserGroupAttachesDel[i].UserGroupNum);
				SecurityLogs.MakeLogEntryNoCache(EnumPermType.SecurityAdmin,0,"User: "+userod.UserName+" removed from user group: "
					+userGroup.Description+" by CEMT user: "+Security.CurUser.UserName);
			}
			return rowsChanged;
		}

		#endregion

		#region Delete
		public static void Delete(UserGroupAttach userGroupAttach) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userGroupAttach);
				return;
			}
			Crud.UserGroupAttachCrud.Delete(userGroupAttach.UserGroupAttachNum);
		}

		///<summary>Does not add a new usergroupattach if the passed-in userCur is already attached to userGroup.</summary>
		public static void AddForUser(Userod userod,long userGroupNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userod,userGroupNum);
				return;
			}
			if(!userod.IsInUserGroup(userGroupNum)) {
				UserGroupAttach userGroupAttach = new UserGroupAttach();
				userGroupAttach.UserGroupNum = userGroupNum;
				userGroupAttach.UserNum = userod.UserNum;
				Crud.UserGroupAttachCrud.Insert(userGroupAttach);
			}
		}

		///<summary>Pass in the user and all of the userGroups that the user should be attached to.
		///Detaches the userCur from any usergroups that are not in the given list.
		///Returns a count of how many user group attaches were affected.</summary>
		public static long SyncForUser(Userod userod,List<long> listUserGroupNums) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),userod,listUserGroupNums);
			}
			long rowsChanged=0;
			for(int i=0;i<listUserGroupNums.Count;i++) {
				if(!userod.IsInUserGroup(listUserGroupNums[i])) {
					UserGroupAttach userGroupAttach = new UserGroupAttach();
					userGroupAttach.UserGroupNum = listUserGroupNums[i];
					userGroupAttach.UserNum = userod.UserNum;
					Crud.UserGroupAttachCrud.Insert(userGroupAttach);
					rowsChanged++;
				}
			}
			List<UserGroupAttach> listUserGroupAttaches=UserGroupAttaches.GetForUser(userod.UserNum);
			for(int i=0;i<listUserGroupAttaches.Count;i++) {
				if(!listUserGroupNums.Contains(listUserGroupAttaches[i].UserGroupNum)) {
					Crud.UserGroupAttachCrud.Delete(listUserGroupAttaches[i].UserGroupAttachNum);
					rowsChanged++;
				}
			}
			return rowsChanged;
		}

		#endregion
	}
}