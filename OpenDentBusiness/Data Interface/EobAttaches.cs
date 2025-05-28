using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EobAttaches{
		///<summary>Gets all EobAttaches for a given claimpayment.</summary>
		public static List<EobAttach> Refresh(long claimPaymentNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<EobAttach>>(MethodBase.GetCurrentMethod(),claimPaymentNum);
			}
			string command="SELECT * FROM eobattach WHERE ClaimPaymentNum="+POut.Long(claimPaymentNum)+" "
				+"ORDER BY DateTCreated";
			return Crud.EobAttachCrud.SelectMany(command);
		}

		///<summary>Gets all EobAttaches for a given claimpaymentnum. For used by Api Team, please notify before changing.
		///Returns an empty list if not found.</summary>
		public static List<EobAttach> GetEobAttachesForApi(int limit,int offset,long claimPaymentNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<EobAttach>>(MethodBase.GetCurrentMethod(),limit,offset,claimPaymentNum);
			}
			string command="SELECT * FROM eobattach WHERE ClaimPaymentNum="+POut.Long(claimPaymentNum)+" "
				+"ORDER BY ClaimPaymentNum "
				+"LIMIT "+POut.Int(offset)+", "+POut.Int(limit);
			return Crud.EobAttachCrud.SelectMany(command);
		}

		///<summary>Gets one EobAttach from the db.</summary>
		public static EobAttach GetOne(long eobAttachNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<EobAttach>(MethodBase.GetCurrentMethod(),eobAttachNum);
			}
			return Crud.EobAttachCrud.SelectOne(eobAttachNum);
		}

		///<summary>Returns the filepath of the eobattach if using AtoZfolder. If storing files in DB or third party storage, saves eobattach to local temp file and returns its filepath.
		///Empty string if not found. This is used by the API Team, please notify before modifying.</summary>
		public static string GetPath(long eobAttachNum) {
			EobAttach eobAttach=EobAttaches.GetOne(eobAttachNum);
			string fileExt=Path.GetExtension(eobAttach.FileName);
			string eobFolderPath=ImageStore.GetEobFolder();
			string filePath;
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {//EOBs/filename in AtoZ
				filePath=CodeBase.ODFileUtils.CombinePaths(eobFolderPath,eobAttach.FileName);
			}
			else if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {//Use RawBase64
				//Some programs require a file on disk and cannot open in memory files. Save to temp file from DB.
				filePath=PrefC.GetRandomTempFile(fileExt);
				File.WriteAllBytes(filePath,Convert.FromBase64String(eobAttach.RawBase64));
			}
			else {//Cloud storage
				//Download file to temp directory
				byte[] byteArray=CloudStorage.Download(eobFolderPath,eobAttach.FileName);
				filePath=PrefC.GetRandomTempFile(fileExt);
				File.WriteAllBytes(filePath,byteArray);
			}
			return filePath;
		}

		///<summary>Tests to see whether an attachment exists on this claimpayment.</summary>
		public static bool Exists(long claimPaymentNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),claimPaymentNum);
			}
			string command="SELECT COUNT(*) FROM eobattach WHERE ClaimPaymentNum="+POut.Long(claimPaymentNum);
			if(Db.GetScalar(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>Set the extension before calling.  Inserts a new eobattach into db, creates a filename based on EobAttachNum, and then updates the db with this filename.  Should always refresh the eobattach after calling this method in order to get the correct filename for RemotingRole.ClientWeb.</summary>
		public static long Insert(EobAttach eobAttach) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				eobAttach.EobAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),eobAttach);
				return eobAttach.EobAttachNum;
			}
			eobAttach.EobAttachNum=Crud.EobAttachCrud.Insert(eobAttach);
			//If the current filename is just an extension, then assign it a unique name.
			if(eobAttach.FileName==Path.GetExtension(eobAttach.FileName)) {
				string extension=eobAttach.FileName;
				eobAttach.FileName=DateTime.Now.ToString("yyyyMMdd_HHmmss_")+eobAttach.EobAttachNum.ToString()+extension;
				Update(eobAttach);
			}
			return eobAttach.EobAttachNum;
		}

		///<summary></summary>
		public static void Update(EobAttach eobAttach) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),eobAttach);
				return;
			}
			Crud.EobAttachCrud.Update(eobAttach);
		}

		///<summary></summary>
		public static void Delete(long eobAttachNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),eobAttachNum);
				return;
			}
			string command= "DELETE FROM eobattach WHERE EobAttachNum = "+POut.Long(eobAttachNum);
			Db.NonQ(command);
		}
	}
}