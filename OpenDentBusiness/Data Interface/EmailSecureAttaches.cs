using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EmailSecureAttaches{
		///<summary></summary>
		public static long Insert(EmailSecureAttach emailSecureAttach){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				emailSecureAttach.EmailSecureAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),emailSecureAttach);
				return emailSecureAttach.EmailSecureAttachNum;
			}
			return Crud.EmailSecureAttachCrud.Insert(emailSecureAttach);
		}

		public static void InsertMany(List<EmailSecureAttach> listEmailSecureAttaches){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listEmailSecureAttaches);
				return;
			}
			Crud.EmailSecureAttachCrud.InsertMany(listEmailSecureAttaches);
		}

		///<summary></summary>
		public static void Update(EmailSecureAttach emailSecureAttach){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailSecureAttach);
				return;
			}
			Crud.EmailSecureAttachCrud.Update(emailSecureAttach);
		}

		///<summary>Gets EmailSecureAttaches that have not been successfully downloaded yet.</summary>
		public static List<EmailSecureAttach> GetOutstanding(List<long> listClinicNums){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<EmailSecureAttach>>(MethodBase.GetCurrentMethod(),listClinicNums);
			}
			//Has not been linked to an EmailAttach yet, therefore, not downloaded yet.
			string command="SELECT * FROM emailsecureattach WHERE EmailAttachNum=0 ";
			if(!listClinicNums.IsNullOrEmpty()) {
				command+="AND emailsecureattach.ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+")";
			}
			return Crud.EmailSecureAttachCrud.SelectMany(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		#region Get Methods
		
		
		///<summary>Gets one EmailSecureAttach from the db.</summary>
		public static EmailSecureAttach GetOne(long emailSecureAttachNum){
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT){
				return Meth.GetObject<EmailSecureAttach>(MethodBase.GetCurrentMethod(),emailSecureAttachNum);
			}
			return Crud.EmailSecureAttachCrud.SelectOne(emailSecureAttachNum);
		}
		#endregion Get Methods
		#region Modification Methods
		
		///<summary></summary>
		public static void Delete(long emailSecureAttachNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailSecureAttachNum);
				return;
			}
			Crud.EmailSecureAttachCrud.Delete(emailSecureAttachNum);
		}
		#endregion Modification Methods
		#region Misc Methods
		

		
		#endregion Misc Methods
		*/



	}
}