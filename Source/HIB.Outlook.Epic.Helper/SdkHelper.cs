#region Namespaces

using HIB.Outlook.Model;
using HIB.Outlook.Model.Activities;
using log4net;
using schemas.appliedsystems.com.epic.sdk._2009._07;
using schemas.appliedsystems.com.epic.sdk._2009._07._common;
using schemas.appliedsystems.com.epic.sdk._2011._01._get;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Acc = schemas.appliedsystems.com.epic.sdk._2009._07._account;
using SDK = schemas.appliedsystems.com.epic.sdk._2009._07;
using System.Linq;
#endregion

namespace HIB.Outlook.Epic.Helper
{
    public class SdkHelper : IDisposable
    {
        #region Private Fields

        private const string DatabaseName = "DatabaseName";
        private const string AuthenticationKey = "AuthenticationKey";
        EpicSDK_2016_01Client EpicSDKService = new EpicSDK_2016_01Client();
        EpicSDKFileTransferClient oStreamingClient = new EpicSDKFileTransferClient();
        MessageHeader mHeader = new MessageHeader();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SdkHelper));
        private Int32 numberOfAttempts = 0;

        #endregion

        #region Constructor

        public SdkHelper()
        {
            mHeader.DatabaseName = Convert.ToString(ConfigurationManager.AppSettings[DatabaseName]);
            mHeader.AuthenticationKey = Convert.ToString(ConfigurationManager.AppSettings[AuthenticationKey]);
        }

        public void Dispose()
        {

        }

        #endregion

        #region Public Members

        /// <summary>
        /// Save Activities to EPIC using SDK
        /// </summary>
        /// <param name="activityInfo"></param>
        /// <returns></returns>
        public Model.Activities.ResultInfo SaveActivities(AddActivity activityInfo, string activityDescription)
        {
            var saveResult = new Model.Activities.ResultInfo();
            try
            {
                var activity = new schemas.appliedsystems.com.epic.sdk._2011._01._common.Activity();
                activity.AccountID = Convert.ToInt32(activityInfo.ClientId);
                activity.AccountTypeCode = "CUST";

                if (activityInfo.AddtoType == "Account")
                {
                    activity.AssociatedToID = Convert.ToInt32(activityInfo.ClientId);
                    activity.AssociatedToType = activityInfo.AddtoType;
                    activity.AgencyCode = activityInfo.AgencyCode;
                    activity.BranchCode = activityInfo.BranchCode;
                }
                else
                {
                    activity.AssociatedToID = Convert.ToInt32(activityInfo.AddToTypeId);
                    activity.AssociatedToType = activityInfo.AddtoType;
                }
                DateTime startDateTime = DateTime.MinValue;
                DateTime endDateTime = DateTime.MinValue;
                DateTime reminderDateTime = DateTime.MinValue;

                try
                {
                    string date = activityInfo.StartDate;
                    string time = activityInfo.StartTime;
                    startDateTime = DateTime.Parse(date + " " + time);

                    if (!string.IsNullOrEmpty(activityInfo.EndDate))
                    {
                        string endDate = activityInfo.EndDate;
                        string endTime = activityInfo.EndTime;
                        endDateTime = DateTime.Parse(endDate + " " + endTime);
                    }

                    if (!string.IsNullOrEmpty(activityInfo.ReminderDate))
                    {
                        string reminderdate = activityInfo.ReminderDate;
                        string reminderTime = activityInfo.ReminderTime;
                        reminderDateTime = DateTime.Parse(reminderdate + " " + reminderTime);
                    }

                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }

                activity.DetailValue = new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity.Detail();

                if (startDateTime != DateTime.MinValue)
                {
                    // if (IsDateTime(activityInfo.StartDate))
                    //startDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(startDateTime, TimeZoneInfo.Local.Id, "Eastern Standard Time");
                    startDateTime = startDateTime.ToUniversalTime();
                    activity.DetailValue.FollowUpStartDate = startDateTime;
                    var StartTime = new DateTime(1900, DateTime.MinValue.Month, DateTime.MinValue.Day, startDateTime.Hour, startDateTime.Minute, startDateTime.Second);
                    activity.DetailValue.FollowUpStartTime = StartTime;
                }

                if (endDateTime != DateTime.MinValue)
                {
                    // if (IsDateTime(activityInfo.EndDate))
                    // endDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(endDateTime, TimeZoneInfo.Local.Id, "Eastern Standard Time");
                    endDateTime = endDateTime.ToUniversalTime();
                    activity.DetailValue.FollowUpEndDate = endDateTime;
                    var EndTime = new DateTime(1900, DateTime.MinValue.Month, DateTime.MinValue.Day, endDateTime.Hour, endDateTime.Minute, endDateTime.Second);
                    activity.DetailValue.FollowUpEndTime = EndTime;

                }
                //activity.DetailValue.FollowUpStartDate = DateTime.Today;
                if (reminderDateTime != DateTime.MinValue)
                {
                    //reminderDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(reminderDateTime, TimeZoneInfo.Local.Id, "Eastern Standard Time");
                    reminderDateTime = reminderDateTime.ToUniversalTime();
                    activity.DetailValue.ReminderDate = reminderDateTime;
                    var reminderTime = new DateTime(1900, DateTime.MinValue.Month, DateTime.MinValue.Day, reminderDateTime.Hour, reminderDateTime.Minute, reminderDateTime.Second);
                    // reminderTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(reminderTime, TimeZoneInfo.Local.Id, "Eastern Standard Time");
                    activity.DetailValue.ReminderTime = reminderTime;
                }

                activity.DetailValue.ContactName = activityInfo.WhoToContactName;
                activity.DetailValue.ContactVia = activityInfo.ContactMode;
                activity.DetailValue.ContactNumberEmail = activityInfo.ContactDetail;

                activity.ActivityCode = activityInfo.AddActivityCode;
                activity.Description = activityDescription;
                activity.WhoOwnerCode = activityInfo.OwnerCode;
                activity.Priority = activityInfo.Priority;

                //if (string.IsNullOrEmpty(activityInfo.EndDate))
                //{
                //    activityInfo.EndDate = activityInfo.StartDate;
                //}
                if (string.Equals(activityInfo.Status, "Closed"))
                {
                    var optionlist = GetEpicOptiontypes(schemas.appliedsystems.com.epic.sdk._2009._07._common._optiontype.OptionTypes.ActivityStatusOptions);
                    activity.StatusOption = optionlist[1];
                    if (activity.CloseDetailValue == null)
                    {
                        activity.CloseDetailValue = new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity.CloseDetail();
                    }
                    activity.CloseDetailValue.ClosedStatus = "Successful";
                }

                activity.Tasks = new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity.TaskItems
                {
                            new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity.TaskItem {
                               // Description = activityInfo.AddActivityDescription,
                                Description = Common.TruncateLongString(activityDescription, 70),
                                Owner = activityInfo.CurrentlyLoggedUserCode,
                                Status = "In Progress",
                                StartDate=Convert.ToDateTime(activityInfo.StartDate),
                                StartTime=startDateTime,
                                DueDate =string.IsNullOrEmpty(activityInfo.EndDate)? default(Nullable<DateTime>): Convert.ToDateTime(activityInfo.EndDate),
                                DueTime = endDateTime == DateTime.MinValue ? default(Nullable<DateTime>) : endDateTime,
                                TaskNotes = new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity._common1.NoteItems
                                {
                                    new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity._common1.NoteItem
                                    {
                                        AccessLevel=activityInfo.AccessLevel,
                                        NoteText = activityInfo.Description,
                                    }
                                },
                            }
                 };
                Logger.Error(string.Format("Activity - [{0}] SDk Push Started at {1}", activity.ActivityCode, DateTime.Now.ToString()));
                if (activityInfo.IsPushToEpic == 0)
                    saveResult.Id = EpicSDKService.Insert_Activity(mHeader, activity);
                Logger.Error(string.Format("Activity - [{0}] SDk Push Completed at {1}", activity.ActivityCode, DateTime.Now.ToString()));
                if (saveResult.Id != null)
                {
                    saveResult.IsSuccess = true;
                    saveResult.HasError = false;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Field = Description"))
                {
                    if (numberOfAttempts == 0)
                    {
                        numberOfAttempts++;
                        saveResult = SaveActivities(activityInfo, activityInfo.AddActivityDescription);
                        if (saveResult.Id != null)
                        {
                            saveResult.IsSuccess = true;
                            saveResult.HasError = false;
                        }
                    }
                    else
                    {
                        numberOfAttempts = 0;
                        Logger.Error(ex);
                    }
                }
                else
                {
                    saveResult.ErrorMessage = ex.Message;
                    saveResult.HasError = true;
                    Logger.Error(ex);
                }
            }
            return saveResult;
        }



        public List<OptionType> GetEpicOptiontypes(schemas.appliedsystems.com.epic.sdk._2009._07._common._optiontype.OptionTypes opType)
        {
            var optionTypes = new List<OptionType>();
            try
            {
                var optiontype = EpicSDKService.Get_Option(mHeader, opType);
                optionTypes = optiontype.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return optionTypes;
        }


        public void GetActivities()
        {

            var taskEventEPICId = 2532842;//2531808;
            try
            {
                var oActivityResult = EpicSDKService.Get_Activity(mHeader, new ActivityFilter { ActivityID = taskEventEPICId }, 0);
                schemas.appliedsystems.com.epic.sdk._2011._01._common.Activity oActivity = oActivityResult.Activities[0];
                oActivity.DetailValue.ReminderDate = oActivity.DetailValue.ReminderDate.Value.AddDays(1);
                oActivity.DetailValue.ReminderTime = oActivity.DetailValue.ReminderTime.Value.AddMinutes(5);
                EpicSDKService.Update_Activity(mHeader, oActivity);
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Save Attachments to EPIC using SDK
        /// </summary>
        /// <param name="attachmentInfo"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public AttachmentInfo SaveAttachmentInfo(AttachmentInfo attachmentInfo, string filePath)
        {
            try
            {
                Logger.Info("Save Attachment service start for UserName:" + attachmentInfo.UserName + " FileName:" + attachmentInfo.FileDetails.FileName + "." + attachmentInfo.FileDetails.FileExtension);
                attachmentInfo = SaveAttachment(attachmentInfo, filePath);
                Logger.Info("Save Attachment service end  for UserName:" + attachmentInfo.UserName + " FileName:" + attachmentInfo.FileDetails.FileName + "." + attachmentInfo.FileDetails.FileExtension);

            }
            catch (Exception ex)
            {
                attachmentInfo.Status = new AttachmentStatus();
                attachmentInfo.Status.ErrorMessage = ex.Message;
                attachmentInfo.Status.Status = Status.Failed;
                Logger.Error(ex);
            }
            return attachmentInfo;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Save Attachments to EPIC through SDK
        /// </summary>
        /// <param name="attachmentInfo"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private AttachmentInfo SaveAttachment(AttachmentInfo attachmentInfo, string filePath)
        {
            using (var oService = new EpicSDK_2014_11Client())
            {
                using (var oStreamingClient = new EpicSDKFileTransferClient(Common.TypeOfServiceBinding))
                {
                    attachmentInfo.Status = new AttachmentStatus();
                    attachmentInfo.Status.Status = Status.InProgress;
                    try
                    {
                        int AccountID = (int)attachmentInfo.ClientId;
                        Acc.Attachment oAttachment = new Acc.Attachment();

                        oAttachment.AccountID = AccountID;
                        oAttachment.AccountTypeCode = Common.AccountTypeCode;

                        var oAgency = new Acc._attachment.AgencyStructureItem();
                        var oAgencies = new Acc._attachment.AgencyStructureItems();

                        oAgency.AgencyCode = attachmentInfo.AgencyCode;
                        oAgencies.Add(oAgency);
                        oAttachment.AgencyStructures = oAgencies;

                        oAttachment.AttachedDate = DateTime.Now;

                        var oAttachedTos = new SDK._account._attachment.AttachedToItems();
                        var oAttachedToItem = new SDK._account._attachment.AttachedToItem();

                        var _with1 = oAttachedToItem;
                        _with1.AttachedToID = Convert.ToInt32(attachmentInfo.ActivityId);
                        _with1.AttachedToType = Common.AttachedToType;
                        oAttachedTos.Add(oAttachedToItem);
                        oAttachment.AttachedTos = oAttachedTos;
                        oAttachment.ClientAccessible = false;

                        Acc._attachment.FileDetailItem oFile = new Acc._attachment.FileDetailItem();
                        Acc._attachment.FileDetailItems oFileDetails = new Acc._attachment.FileDetailItems();

                        attachmentInfo.FileDetails.NewFileName = Common.RemoveSpecialCharacters(attachmentInfo.FileDetails.NewFileName);
                        attachmentInfo.FileDetails.FileName = Common.RemoveSpecialCharacters(attachmentInfo.FileDetails.FileName);

                        attachmentInfo.FileDetails.NewFileName = Common.EmptyFileName(attachmentInfo.FileDetails.NewFileName);
                        attachmentInfo.FileDetails.FileName = Common.EmptyFileName(attachmentInfo.FileDetails.FileName);
                        attachmentInfo.Subject = Common.EmptyFileName(attachmentInfo.Subject);

                        filePath = Path.Combine(filePath, Path.GetFileName(attachmentInfo.FileDetails.FilePath));
                        Logger.Info("File Path for Attachment " + filePath);
                        IsFileExist(filePath);
                        System.Threading.Thread.Sleep(5000);
                        var fileByte = File.ReadAllBytes(filePath);
                        using (MemoryStream stream = new MemoryStream(fileByte))
                        {
                            oFile.TicketName = oStreamingClient.Upload_Attachment_File(mHeader, stream);
                            oFile.FileName = attachmentInfo.FileDetails.FileName;
                            oFile.Extension = attachmentInfo.FileDetails.FileExtension;
                            oFile.Length = (int)fileByte.Length;
                            oFileDetails.Add(oFile);

                            if (fileByte != null)
                            {
                                Array.Clear(fileByte, 0, fileByte.Length);
                                attachmentInfo.FileDetails.FileContentMemStream = null;
                                GC.Collect();
                            }
                            stream.Close();
                            stream.Dispose();
                        }
                        DateTime pacificTime = DateTime.MinValue;

                        try
                        {
                            var zone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                            var utcNow = DateTime.UtcNow;
                            pacificTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, zone);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }

                        oAttachment.Files = oFileDetails;
                        var mailBody = Common.TruncateLongString(attachmentInfo.MailBody, 500);
                        if (attachmentInfo.IsEmail)
                        {
                            var cc = string.IsNullOrEmpty(attachmentInfo.EmailCCDisplayName) ? null : $"{attachmentInfo.EmailCCDisplayName} ({attachmentInfo.EmailCCAddress})";

                            oAttachment.Comments = string.Format(@"{0}{1}\{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}",
                                                  "Attached by ",
                                                  attachmentInfo.DomainName,
                                                  $"{attachmentInfo.UserName}({attachmentInfo.EmployeeCode})",
                                                  " on ",
                                                 pacificTime.ToString("MM/dd/yyyy"),
                                                  " at ",
                                                 pacificTime.ToString("HH:mm"),
                                                  ".",
                                                  Environment.NewLine,
                                                  "To: ",
                                                  $"{attachmentInfo.EmailToDisplayName} ({attachmentInfo.EmailToAddress})",
                                                  Environment.NewLine,
                                                  "From: ",
                                                  $"{attachmentInfo.EmailFromDisplayName} ({attachmentInfo.EmailFromAddress})",
                                                  Environment.NewLine,
                                                  "CC:",
                                                  cc,
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
                                                   pacificTime.ToString("MM/dd/yyyy"),
                                                    " at ",
                                                   pacificTime.ToString("HH:MM"),
                                                    ".",
                                                    Environment.NewLine,
                                                    "File Item",
                                                    Environment.NewLine,
                                                    "Subject: ",
                                                     attachmentInfo.FileDetails.FileName);
                            oAttachment.ReceivedDate = DateTime.Now;
                        }


                        oAttachment.Comments = Common.TruncateLongString(oAttachment.Comments, 500);
                        oAttachment.Folder = attachmentInfo.FolderDetails.ParentFolderName;
                        oAttachment.IsInactive = false;
                        oAttachment.SecurityAccessLevelCode = Common.SecurityAccessLevelCode;
                        oAttachment.SubFolder1 = attachmentInfo.FolderDetails.FolderName;
                        oAttachment.SubFolder2 = attachmentInfo.FolderDetails.SubFolderName;
                        oAttachment.Summary = attachmentInfo.FileDetails.FileName + " " + Common.FileAttachSummary;
                        oAttachment.ClientAccessible = attachmentInfo.IsClientAccessible;
                        oAttachment.Description = Common.TruncateLongString(attachmentInfo.Description, 120);
                        oAttachment.ClientAccessExpireOnDate = string.IsNullOrEmpty(attachmentInfo.ClientAccessible) ? default(Nullable<DateTime>) : Convert.ToDateTime(attachmentInfo.ClientAccessible);
                        oAttachment.ActivityID = attachmentInfo.ActivityId;
                        oAttachment.ClientId = attachmentInfo.ClientId;
                        oAttachment.PolicyCode = attachmentInfo.PolicyCode;
                        oAttachment.PolicyType = attachmentInfo.PolicyType;
                        oAttachment.PolicyYear = attachmentInfo.PolicyYear;
                        Logger.Error(string.Format("Attachment - [{0}] SDk Push Started at {1}", oAttachment.Description, DateTime.Now.ToString()));
                        int[] attachmentID = oService.Insert_Attachment(mHeader, oAttachment);
                        Logger.Error(string.Format("Attachment - [{0}] SDk Push Completed at {1} with attachment ID - {2}", oAttachment.Description, DateTime.Now.ToString(), string.Join(",", attachmentID)));
                        attachmentInfo.Status.ErrorMessage = null;
                        attachmentInfo.Status.Status = Status.Success;
                        if (File.Exists(filePath))
                        {
                            try
                            {
                                File.Delete(filePath);
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        attachmentInfo.Status.ErrorMessage = ex.Message;
                        attachmentInfo.Status.Status = Status.Failed;
                    }
                }
            }
            return attachmentInfo;
        }


        private bool IsFileExist(string filePath)
        {
            var IsExist = false;
            try
            {
                while (!IsExist)
                {
                    IsExist = File.Exists(filePath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return IsExist;
        }


        //public static bool IsDateTime(string txtDate)
        //{
        //    DateTime tempDate;
        //    return DateTime.TryParse(txtDate, out tempDate);
        //}
        #endregion


    }
}
