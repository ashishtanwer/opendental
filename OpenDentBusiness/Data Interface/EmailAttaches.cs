using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using CodeBase;
using OpenDentBusiness.FileIO;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EmailAttaches{
		public static long Insert(EmailAttach emailAttach) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				emailAttach.EmailAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),emailAttach);
				return emailAttach.EmailAttachNum;
			}
			return Crud.EmailAttachCrud.Insert(emailAttach);
		}

		public static void InsertMany(List<EmailAttach> listEmailAttaches) {
			if(listEmailAttaches.Count==0) {
				return;
			}
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listEmailAttaches);
				return;
			}
			Crud.EmailAttachCrud.InsertMany(listEmailAttaches);
		}

		public static List<EmailAttach> GetForEmail(long emailMessageNum) {
			Meth.NoCheckMiddleTierRole();
			List<long> listEmailMessageNums=new List<long>() { emailMessageNum };
			return GetForEmails(listEmailMessageNums);
		}

		public static List<EmailAttach> GetForEmails(List<long> listEmailMessageNums) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<EmailAttach>>(MethodBase.GetCurrentMethod(),listEmailMessageNums);
			}
			List<EmailAttach> listEmailAttaches=new List<EmailAttach>();
			if(listEmailMessageNums==null || listEmailMessageNums.Count==0) {
				return listEmailAttaches;
			}
			//Skip all attachments that are for EmailMessageNum of 0 because those are meant for Templates, not emails.
			//Use GetForTemplate() instead of GetForEmails if you want all template attachments.
			List<long> listEmailMessageNumsFiltered=listEmailMessageNums.FindAll(x => x!=0);
			if(listEmailMessageNumsFiltered.Count < 1) {
				return listEmailAttaches;
			}
			string command="SELECT * FROM emailattach WHERE EmailMessageNum IN("+string.Join(",",listEmailMessageNums)+")";
			return Crud.EmailAttachCrud.SelectMany(command);
		}

		///<summary>Gets one EmailAttach from the db. Used by Patient Portal.</summary>
		public static EmailAttach GetOne(long emailAttachNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<EmailAttach>(MethodBase.GetCurrentMethod(),emailAttachNum);
			}
			return Crud.EmailAttachCrud.SelectOne(emailAttachNum);
		}

		///<summary>Throws exceptions.  Creates a new file within the Out subfolder of the email attachment path (inside OpenDentImages) and returns an EmailAttach object referencing the new file.  The displayFileName will always contain valid file name characters, because it is either a hard coded value or is based on an existing valid file name.  The actual file name will end with the displayFileName, so that the actual files are easier to locate and have the same file extension as the displayedFileName.</summary>
		public static EmailAttach CreateAttach(string displayedFileName,byte[] byteArrayData) {
			Meth.NoCheckMiddleTierRole();
			return CreateAttach(displayedFileName,"",byteArrayData,true);
		}

		///<summary>Throws exceptions.  Creates a new file inside of the email attachment path (inside OpenDentImages) and returns an EmailAttach object 
		///referencing the new file.  If isOutbound is true, then the file will be saved to the "Out" subfolder, otherwise the file will be saved to the 
		///"In" subfolder.  The displayFileName will always contain valid file name characters, because it is either a hard coded value or is based on an 
		///existing valid file name.  If a file already exists matching the actualFileName, then an exception will occur.  Set actualFileName to empty 
		///string to generate a unique actual file name.  If the actual file name is generated, then actual file name will end with the displayFileName, 
		///so that the actual files are easier to locate and have the same file extension as the displayedFileName.</summary>
		public static EmailAttach CreateAttach(string displayedFileName,string actualFileName,byte[] byteArrayData,bool isOutbound) {
			Meth.NoCheckMiddleTierRole();
			EmailAttach emailAttach=new EmailAttach();
			emailAttach.DisplayedFileName=displayedFileName;
			actualFileName=ODFileUtils.CleanFileName(actualFileName);//Clean the actual file name for the OS.
			if(String.IsNullOrEmpty(emailAttach.DisplayedFileName)) {
				//This could only happen for malformed incoming emails, but should not happen.  Name uniqueness is virtually guaranteed below.
				//The actual file name will not have an extension, so the user will be asked to pick the program to open the attachment with when
				//the attachment is double-clicked.
				emailAttach.DisplayedFileName="attach";
			}
			string attachDir=GetAttachPath();
			string subDir="In";
			if(isOutbound) {
				subDir="Out";
			}			
			if(!CloudStorage.IsCloudStorage && !Directory.Exists(ODFileUtils.CombinePaths(attachDir,subDir))) {
				Directory.CreateDirectory(ODFileUtils.CombinePaths(attachDir,subDir));
			}
			if(String.IsNullOrEmpty(actualFileName)) {
				while(true) {
					if(!String.IsNullOrEmpty(emailAttach.ActualFileName)) {
						if(!FileAtoZ.Exists(FileAtoZ.CombinePaths(attachDir,emailAttach.ActualFileName))) {
							break;
						}
					}
					//Display name is tacked onto actual file name last as to ensure file extensions are the same.
					emailAttach.ActualFileName=FileAtoZ.CombinePaths(subDir,
						DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.TimeOfDay.Ticks.ToString()
							+"_"+MiscUtils.CreateRandomAlphaNumericString(4)+"_"+ODFileUtils.CleanFileName(emailAttach.DisplayedFileName));
				}
			}
			else {
				//The caller wants a specific actualFileName.  Use the given name as is.
				emailAttach.ActualFileName=FileAtoZ.CombinePaths(subDir,actualFileName);
			}
			string attachFilePath=FileAtoZ.CombinePaths(attachDir,emailAttach.ActualFileName);
			if(FileAtoZ.Exists(attachFilePath)) {
				throw new ApplicationException("Email attachment could not be saved because a file with the same name already exists.");
			}
			try {
				FileAtoZ.WriteAllBytes(attachFilePath,byteArrayData);
			}
			catch {
				if(!FileAtoZ.Exists(attachFilePath)) { 
					throw;//Show the initial error message
				}
				try {
					FileAtoZ.Delete(attachFilePath);
				}
				catch {
					//We tried our best to delete the file, and there is nothing else to try.
				}
				throw;//Show the initial error message, even if the Delete() failed.
			}
			return emailAttach;
		}

		///<summary>Returns patient's AtoZ path if local AtoZ is used.  Returns Cloud AtoZ path if Dropbox is used. Returns temp path if in database.</summary>
		public static string GetAttachPath() {
			Meth.NoCheckMiddleTierRole();
			string attachPath;
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				attachPath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"EmailAttachments");
				if(!Directory.Exists(attachPath)) {
					Directory.CreateDirectory(attachPath);
				}
				return attachPath;
			}
			if(CloudStorage.IsCloudStorage) {
				attachPath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"EmailAttachments",'/');//Gets Cloud path with EmailAttachments folder.
				return attachPath;
			}
			//For users who have the A to Z folders disabled, there is no defined image path, so we
			//have to use a temp path.  This means that the attachments might be available immediately afterward,
			//but probably not later.
			attachPath=ODFileUtils.CombinePaths(Path.GetTempPath(),"opendental");//Have to use Path.GetTempPath() here instead of PrefL.GetTempPathFolder() because we can't access PrefL.
			return attachPath;
		}

		///<summary>Returns all EmailAttaches assocaited to a specific EmailTemplateNum.</summary>
		public static List<EmailAttach> GetForTemplate(long emailTemplateNum) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				return Meth.GetObject<List<EmailAttach>>(MethodBase.GetCurrentMethod(),emailTemplateNum);
			}
			string command="SELECT * FROM emailattach WHERE EmailTemplateNum="+POut.Long(emailTemplateNum);
			return Crud.EmailAttachCrud.SelectMany(command);
		}

		///<summary>Syncs a given list of EmailAttaches to a list of old EmailAttaches.
		///If emailAttachOld is not provided, it will use the emailMessageNum passed in to get the "old" attachments from the database.</summary>
		public static void Sync(long emailMessageNum,List<EmailAttach> listEmailAttachesNew,List<EmailAttach> listEmailAttachesOld=null) {
			if(RemotingClient.MiddleTierRole==MiddleTierRole.ClientMT) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailMessageNum,listEmailAttachesNew,listEmailAttachesOld);
				return;
			}
			if(listEmailAttachesOld==null) {
				listEmailAttachesOld=GetForEmail(emailMessageNum);//Get attachments from the database.
			}
			Crud.EmailAttachCrud.Sync(listEmailAttachesNew,listEmailAttachesOld);
		}
	}
}