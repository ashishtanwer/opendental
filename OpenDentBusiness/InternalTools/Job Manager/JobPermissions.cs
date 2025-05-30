using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobPermissions{

		///<summary>JobRoles cannot be hidden so there is only one list.</summary>
		private static List<JobPermission> _list;
		private static object _lockObj=new object();

		public static List<JobPermission> List {
			//No need to check MiddleTierRole; no call to db.
			get {
				return GetList();
			}
			set {
				lock(_lockObj) {
					_list=value;
				}
			}
		}

		public static List<JobPermission> GetList() {
			Meth.NoCheckMiddleTierRole();
			bool isListNull=false;
			lock(_lockObj) {
				if(_list==null) {
					isListNull=true;
				}
			}
			if(isListNull) {
				RefreshCache();
			}
			List<JobPermission> jobRoles;
			lock(_lockObj) {
				jobRoles=new List<JobPermission>();
				for(int i=0;i<_list.Count;i++) {
					jobRoles.Add(_list[i].Copy());
				}
			}
			return jobRoles;
		}

		///<summary>Refresh all jobroles.  Not actually part of official cache.</summary>
		public static DataTable RefreshCache() {
			Meth.NoCheckMiddleTierRole();//Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM jobpermission";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="JobRole";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			Meth.NoCheckMiddleTierRole();
			List=Crud.JobPermissionCrud.TableToList(table);
		}

		public static void ClearCache() {
			List=null;
		}
		/////<summary>Checks to see if current user is authorized.  It also checks any date restrictions.  If not authorized, it gives a Message box saying so and returns false.</summary>
		//public static bool IsAuthorized(JobRoleType jobRole) {
		//	Meth.NoCheckMiddleTierRole();
		//	return IsAuthorized(jobRole,false);
		//}

		///<summary>Checks to see if user is authorized, if no user provided checks currently logged in user.  If not authorized and not suppressed, it gives a Message box saying so and returns false.</summary>
		public static bool IsAuthorized(JobPerm jobPerm,bool suppressMessage=false,long userNum=-1) {
			Meth.NoCheckMiddleTierRole();
			if(Security.CurUser==null) {
				return false;
			}
			if(userNum==-1) {//no user passed in
				userNum=Security.CurUser.UserNum;
			}
			if(userNum==0) {//"Unassigned" user passed in.
				return true;
			}
			if(GetList().Any(x => x.UserNum==userNum && x.JobPermType==jobPerm)) {
				return true;
			}
			if(!suppressMessage) {
				MessageBox.Show(Lans.g("Security","A user with the SecurityAdmin permission must grant you access for job role")+"\r\n"+jobPerm.ToString());
			}
			return false;
		}

		///<summary></summary>
		public static List<JobPermission> GetForUser(long userNum){
			return GetList().FindAll(x => x.UserNum==userNum); 
		}

		///<summary>Gets one JobRole from the db.</summary>
		public static JobPermission GetOne(long jobRoleNum){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				return Meth.GetObject<JobPermission>(MethodBase.GetCurrentMethod(),jobRoleNum);
			}
			return Crud.JobPermissionCrud.SelectOne(jobRoleNum);
		}

		///<summary></summary>
		public static long Insert(JobPermission jobRole){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				jobRole.JobPermissionNum=Meth.GetLong(MethodBase.GetCurrentMethod(),jobRole);
				return jobRole.JobPermissionNum;
			}
			return Crud.JobPermissionCrud.Insert(jobRole);
		}

		///<summary></summary>
		public static void Update(JobPermission jobRole){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobRole);
				return;
			}
			Crud.JobPermissionCrud.Update(jobRole);
		}

		///<summary>Inserts, updates, or deletes the passed in list against rows for the passed in user.  Returns true if db changes were made.</summary>
		public static bool Sync(List<JobPermission> listNew,long userNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,userNum);
			}
			List<JobPermission> listDB=GetForUser(userNum);
			return Crud.JobPermissionCrud.Sync(listNew,listDB);
		}

		///<summary></summary>
		public static void Delete(long jobRoleNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobRoleNum);
				return;
			}
			Crud.JobPermissionCrud.Delete(jobRoleNum);
		}

		///<summary>If no JobPerm is supplied, returns true if the user has JobPerm.QueryTech, QueryCoordinator, or SeniorCoordinator. If a JobPerm is supplied, will return true if the user has that permission 'or better'. i.e. if you supply QueryCoordinator, it will return true if the user has QueryCoordinator or SeniorQueryCoordinator permission, but false if they only have QueryTech.</summary>

		public static bool HasQueryPermission(JobPerm minPermReq=JobPerm.QueryTech,long userNum=-1) {
			Meth.NoCheckMiddleTierRole();
			if(Security.CurUser==null) {
				return false;
			}
			if(userNum==-1) {//no user passed in
				userNum=Security.CurUser.UserNum;
			}
			List<JobPermission> listJobPermissions=GetList().Where(x=>x.UserNum==userNum).ToList();
			if(minPermReq==JobPerm.SeniorQueryCoordinator) {
				return listJobPermissions.Any(x => x.JobPermType==JobPerm.SeniorQueryCoordinator);
			}
			if(minPermReq==JobPerm.QueryCoordinator) {
				return listJobPermissions.Any(x => x.JobPermType==JobPerm.QueryCoordinator || x.JobPermType==JobPerm.SeniorQueryCoordinator);
			}
			return listJobPermissions.Any(x => x.JobPermType==JobPerm.QueryTech || x.JobPermType==JobPerm.QueryCoordinator || x.JobPermType==JobPerm.SeniorQueryCoordinator);
		}

	}
}