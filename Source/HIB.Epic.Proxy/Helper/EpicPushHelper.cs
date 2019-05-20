using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acc = schemas.appliedsystems.com.epic.sdk._2009._07._account;
using SDK = schemas.appliedsystems.com.epic.sdk._2009._07;
using HIB.Outlook.Model;
using System.IO;
using HIB.Outlook.Model.Activities;
using schemas.appliedsystems.com.epic.sdk._2009._07._common;
using schemas.appliedsystems.com.epic.sdk._2009._07;
using schemas.appliedsystems.com.epic.sdk._2009._07._account;
using schemas.appliedsystems.com.epic.sdk._2009._07._account._client;
using schemas.appliedsystems.com.epic.sdk._2009._07._common._lookup;
using schemas.appliedsystems.com.epic.sdk._2009._07._get;
using schemas.appliedsystems.com.epic.sdk._2011._01._get;
using HIB.Outlook.Sync.Common;
using HIB.Outlook.Helper.Common;

namespace HIB.Outlook.Helper.Helper
{
    public class EpicPushHelper : IDisposable
    {


        public Model.AttachmentInfo SaveAttachmentInfo(Model.AttachmentInfo attachmentInfo)
        {

            try
            {
                Logger.InfoLog("Save Attachment service start for UserName:" + attachmentInfo.UserName + " FileName:" + attachmentInfo.FileDetails.FileName + "." + attachmentInfo.FileDetails.FileExtension, typeof(EpicPushHelper), Logger.SourceType.WindowsService, attachmentInfo.EmployeeCode);
                attachmentInfo = SaveAttachment(attachmentInfo);
              
                Logger.InfoLog("Save Attachment service end  for UserName:" + attachmentInfo.UserName + " FileName:" + attachmentInfo.FileDetails.FileName + "." + attachmentInfo.FileDetails.FileExtension, typeof(EpicPushHelper), Logger.SourceType.WindowsService, attachmentInfo.EmployeeCode);
                GC.Collect();

            }
            catch (Exception ex)
            {
                attachmentInfo.Status = new AttachmentStatus();
                attachmentInfo.Status.ErrorMessage = ex.Message;
                attachmentInfo.Status.Status = Status.Failed;
                
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, attachmentInfo.EmployeeCode);
                GC.Collect();
            }
            finally
            {
                Logger.save();
            }

            return attachmentInfo;
        }

        private AttachmentInfo SaveAttachment(Model.AttachmentInfo attachmentInfo)
        {
            EpicSDK_2014_11Client oService = new EpicSDK_2014_11Client();
            EpicSDKFileTransferClient oStreamingClient = new EpicSDKFileTransferClient(Common.Common.TypeOfServiceBinding);
            SDK.MessageHeader oHeader = new SDK.MessageHeader();
            attachmentInfo.Status = new AttachmentStatus();
            try
            {
                oHeader.DatabaseName = Common.Common.DatabaseName;
                oHeader.AuthenticationKey = Common.Common.AuthenticationKey;
                int AccountID = (int)attachmentInfo.ClientId;
                Acc.Attachment oAttachment = new Acc.Attachment();

                oAttachment.AccountID = AccountID;
                oAttachment.AccountTypeCode = Common.Common.AccountTypeCode;

                SDK._account._attachment.AgencyStructureItem oAgency = new Acc._attachment.AgencyStructureItem();
                SDK._account._attachment.AgencyStructureItems oAgencies = new Acc._attachment.AgencyStructureItems();

                oAgency.AgencyCode = attachmentInfo.AgencyCode;
                oAgencies.Add(oAgency);
                oAttachment.AgencyStructures = oAgencies;

                oAttachment.AttachedDate = DateTime.Now;

                SDK._account._attachment.AttachedToItems oAttachedTos = new SDK._account._attachment.AttachedToItems();
                SDK._account._attachment.AttachedToItem oAttachedToItem = new SDK._account._attachment.AttachedToItem();

                var _with1 = oAttachedToItem;
                _with1.AttachedToID = Convert.ToInt32(attachmentInfo.ActivityId);
                _with1.AttachedToType = Common.Common.AttachedToType;
                oAttachedTos.Add(oAttachedToItem);
                oAttachment.AttachedTos = oAttachedTos;
                oAttachment.ClientAccessible = false;

                Acc._attachment.FileDetailItem oFile = new Acc._attachment.FileDetailItem();
                Acc._attachment.FileDetailItems oFileDetails = new Acc._attachment.FileDetailItems();

                attachmentInfo.FileDetails.NewFileName = Common.Common.RemoveSpecialCharacters(attachmentInfo.FileDetails.NewFileName);
                attachmentInfo.FileDetails.FileName = Common.Common.RemoveSpecialCharacters(attachmentInfo.FileDetails.FileName);
                

                attachmentInfo.FileDetails.NewFileName = Common.Common.EmptyFileName(attachmentInfo.FileDetails.NewFileName);
                attachmentInfo.FileDetails.FileName = Common.Common.EmptyFileName(attachmentInfo.FileDetails.FileName);
                attachmentInfo.Subject = Common.Common.EmptyFileName(attachmentInfo.Subject);

                using (MemoryStream stream = new MemoryStream(attachmentInfo.FileDetails.FileContentMemStream))
                {
                    oFile.TicketName = oStreamingClient.Upload_Attachment_File(oHeader, stream);
                    oFile.FileName = attachmentInfo.FileDetails.FileName;
                    oFile.Extension = attachmentInfo.FileDetails.FileExtension;
                    oFile.Length = (int)attachmentInfo.FileDetails.FileContentMemStream.Length;
                    oFileDetails.Add(oFile);


                    if (attachmentInfo.FileDetails.FileContentMemStream != null)
                    {
                        Array.Clear(attachmentInfo.FileDetails.FileContentMemStream, 0, attachmentInfo.FileDetails.FileContentMemStream.Length);
                        attachmentInfo.FileDetails.FileContentMemStream = null;
                        GC.Collect();
                    }
                    stream.Close();
                    stream.Dispose();
                    GC.Collect();
                }

                oAttachment.Files = oFileDetails;
                oAttachment.Description = attachmentInfo.FileDetails.FileName;

                oAttachment.Description = Common.Common.TruncateLongString(oAttachment.Description, 100);
                var mailBody = Common.Common.TruncateLongString(attachmentInfo.MailBody, 500);
                if (attachmentInfo.IsEmail)
                {
                    oAttachment.Comments = string.Format("{0}{1}/{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}",
                                            "Attached by ",
                                            attachmentInfo.DomainName,
                                            $"{attachmentInfo.UserName}({attachmentInfo.EmployeeCode})",
                                            " on ",
                                            DateTime.Now.ToString("MM/dd/yyyy"),
                                            " at ",
                                            DateTime.Now.ToString("HH:MM"),
                                            ".",
                                            Environment.NewLine,
                                            "To: ",
                                            $"{attachmentInfo.EmailToDisplayName} ({attachmentInfo.EmailToAddress})",
                                            Environment.NewLine,
                                            "From: ",
                                            $"{attachmentInfo.EmailFromDisplayName} ({attachmentInfo.EmailFromAddress})",
                                            Environment.NewLine,
                                            "CC ",
                                             $"{attachmentInfo.EmailCCDisplayName} ({attachmentInfo.EmailCCAddress})",
                                            Environment.NewLine,
                                            "Subject: ",
                                            attachmentInfo.FileDetails.FileName,
                                            Environment.NewLine,
                                            mailBody);
                    oAttachment.ReceivedDate = Convert.ToDateTime(attachmentInfo.ReceivedDate);

                }
                else
                {
                    oAttachment.Comments = string.Format("{0}{1}\\{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                                            "Attached by ",
                                            attachmentInfo.DomainName,
                                            attachmentInfo.UserName,
                                            " on ",
                                            DateTime.Now.ToString("MM/dd/yyyy"),
                                            " at ",
                                            DateTime.Now.ToString("HH:MM"),
                                            ".",
                                            Environment.NewLine,
                                            "File Item",
                                            Environment.NewLine,
                                            "Subject: ",
                                             attachmentInfo.FileDetails.FileName + attachmentInfo.FileDetails.FileExtension);
                    oAttachment.ReceivedDate = DateTime.Now;
                }


                oAttachment.Comments = Common.Common.TruncateLongString(oAttachment.Comments, 500);
                oAttachment.Folder = attachmentInfo.FolderDetails.ParentFolderName;
                oAttachment.IsInactive = false;
                
                oAttachment.SecurityAccessLevelCode = Common.Common.SecurityAccessLevelCode;
                oAttachment.SubFolder1 = attachmentInfo.FolderDetails.FolderName;
                oAttachment.SubFolder2 = attachmentInfo.FolderDetails.SubFolderName;
                oAttachment.Summary = attachmentInfo.FileDetails.FileName + " " + Common.Common.FileAttachSummary;
                oAttachment.ClientAccessible = attachmentInfo.IsClientAccessible;
                oAttachment.Description = attachmentInfo.Description;
                oAttachment.ClientAccessExpireOnDate = string.IsNullOrEmpty(attachmentInfo.ClientAccessible) ? default(Nullable<DateTime>) : Convert.ToDateTime(attachmentInfo.ClientAccessible);
                oAttachment.ActivityID = attachmentInfo.ActivityId;
                oAttachment.ClientId = attachmentInfo.ClientId;
                oAttachment.PolicyCode = attachmentInfo.PolicyCode;
                oAttachment.PolicyType = attachmentInfo.PolicyType;
                oAttachment.PolicyYear = attachmentInfo.PolicyYear;



                int[] attachmentID = oService.Insert_Attachment(oHeader, oAttachment);
                oStreamingClient.Close();
                oService.Close();
                GC.Collect();

                attachmentInfo.Status.ErrorMessage = null;
                attachmentInfo.Status.Status = Status.Success;



            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, "");
                oStreamingClient.Close();
                oService.Abort();               
                GC.Collect();
                attachmentInfo.Status.ErrorMessage = ex.Message;
                attachmentInfo.Status.Status = Status.Failed;

            }
            finally
            {
                Logger.save();
            }
            return attachmentInfo;
        }


        public void Dispose()
        {

        }

    }
}
