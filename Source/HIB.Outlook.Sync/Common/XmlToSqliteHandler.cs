using HIB.Outlook.Helper.Common;
using HIB.Outlook.Helper.Helper;
using HIB.Outlook.Model;
using HIB.Outlook.Model.Activities;
using HIB.Outlook.SQLite;
using HIB.Outlook.SQLite.Repository;
using HIB.Outlook.SQLite.Repository.IRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HIB.Outlook.Sync.Common
{
    public class XmlToSqliteHandler
    {
        private static readonly object lockObject = new object();
        public OrderedDictionary MappingCollection = new OrderedDictionary();
        static IClientRepository _clientRepository;
        static ILogRepository _logRepository;
        static IActivityRepository _activityRepository;
        static IFolderRepository _folderRepository;
        static IPolicyLineTypeRepository _policyLineTypeRepository;

        static XmlToSqliteHandler()
        {
            _clientRepository = new ClientRepository();
            _logRepository = new LogRepository();
            _activityRepository = new ActivityRepository();
            _folderRepository = new FolderRepository();
            _policyLineTypeRepository = new PolicyLineTypeRepository();
        }
        private static Account GetAccountForActivity(long? accountId)
        {
            var account = new Account();
            try
            {
                string accountInfoQuery = string.Format("Select * from HIBOPActivityAccount where AccountId={0}", accountId);
                var sqliteDataReader = Helper.Helper.SqliteHelper.ExecuteSelectQuery(accountInfoQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        account.AgencyCode = Convert.ToString(sqliteDataReader["AgencyCode"]);
                        account.AgencyName = Convert.ToString(sqliteDataReader["AgencyName"]);
                        account.BranchCode = Convert.ToString(sqliteDataReader["BranchCode"]);
                        account.BranchName = Convert.ToString(sqliteDataReader["BranchName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
            return account;
        }

        private static Policy GetPolicyDetails(string policyActivitiesQuery, AddActivity newActivity)
        {
            var policyInfo = new Policy();
            var sqliteDataReader = Helper.Helper.SqliteHelper.ExecuteSelectQuery(policyActivitiesQuery);

            if (sqliteDataReader != null)
            {
                while (sqliteDataReader.Read())
                {
                    try
                    {
                        policyInfo.PolicyId = Convert.ToInt32(sqliteDataReader["UniqPolicy"]);
                        policyInfo.ClientId = Convert.ToInt64(sqliteDataReader["UniqEntity"]);
                        policyInfo.Type = Convert.ToString(sqliteDataReader["CdPolicyLineTypeCode"]);
                        policyInfo.Status = Convert.ToString(sqliteDataReader["PolicyStatus"]);
                        policyInfo.Effective = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EffectiveDate"));
                        policyInfo.Expiration = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate"));
                        policyInfo.PolicyNumber = Convert.ToString(sqliteDataReader["PolicyNumber"]);
                        policyInfo.PolicyDescription = Convert.ToString(sqliteDataReader["DescriptionOf"]);
                        policyInfo.Flags = Convert.ToInt32(sqliteDataReader["Flags"]);
                        //policyInfo.UniqAgency = Convert.ToInt32(sqliteDataReader["UniqAgency"]);
                        //policyInfo.UniqBranch = Convert.ToInt32(sqliteDataReader["UniqBranch"]);

                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                    }

                }
                sqliteDataReader.Close();
            }
            return policyInfo;
        }
        private static Line GetActivityLineDetails(string linesActivitiesQuery)
        {
            Line LineInfo = new Line();
            var sqlitePolicyDataReader = SqliteHelper.ExecuteSelectQuery(linesActivitiesQuery);

            if (sqlitePolicyDataReader != null)
            {
                while (sqlitePolicyDataReader.Read())
                {
                    try
                    {
                        LineInfo.LineId = Convert.ToInt64(sqlitePolicyDataReader["UniqLine"]);
                        LineInfo.ClientId = Convert.ToInt64(sqlitePolicyDataReader["UniqEntity"]);
                        LineInfo.LineCode = Convert.ToString(sqlitePolicyDataReader["LineCode"]);
                        LineInfo.UniqPolicy = Convert.ToInt64(sqlitePolicyDataReader["UniqPolicy"]);
                        LineInfo.LineOfBusiness = Convert.ToString(sqlitePolicyDataReader["LineOfBusiness"]);
                        LineInfo.Status = Convert.ToString(sqlitePolicyDataReader["LineStatus"]);
                        LineInfo.PolicyNumber = Convert.ToString(sqlitePolicyDataReader["PolicyNumber"]);
                        LineInfo.ICO = Convert.ToString(sqlitePolicyDataReader["IOC"]);
                        LineInfo.Billing = Convert.ToString(sqlitePolicyDataReader["BillModeCode"]);
                        LineInfo.PolicyDescription = Convert.ToString(sqlitePolicyDataReader["PolicyDesc"]);
                        LineInfo.Flags = Convert.ToInt32(sqlitePolicyDataReader["Flags"]);
                        LineInfo.Effective = Convert.ToDateTime(sqlitePolicyDataReader["EffectiveDate"]).ToShortDateString();
                        LineInfo.Expiration = Convert.ToDateTime(sqlitePolicyDataReader["ExpirationDate"]).ToShortDateString();

                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                    }

                }
                sqlitePolicyDataReader.Close();
            }
            return LineInfo;
        }
        private static Claim GetClaimDetails(string claimDetailQuery)
        {
            var ClaimInfo = new Claim();
            var sqliteDataReader = Helper.Helper.SqliteHelper.ExecuteSelectQuery(claimDetailQuery);
            if (sqliteDataReader != null)
            {
                while (sqliteDataReader.Read())
                {
                    try
                    {
                        ClaimInfo.ClaimId = Convert.ToInt64(sqliteDataReader["UniqClaim"]);
                        ClaimInfo.ClientId = Convert.ToInt64(sqliteDataReader["UniqEntity"]);
                        ClaimInfo.DateOfLoss = Convert.ToString(sqliteDataReader["LossDate"]);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                    }


                }
                sqliteDataReader.Close();
            }
            return ClaimInfo;
        }
        private static HIBOPActivity PrepareActivityObjectForDBUpdate(dynamic UniqActivity, AddActivity newActivity, Policy policyInfo, Line LineInfo, Claim ClaimInfo)
        {
            HIBOPActivity addActivity = new HIBOPActivity();
            try
            {
                addActivity.UniqActivity = Convert.ToInt32(UniqActivity);
                addActivity.UniqEntity = Convert.ToInt64(newActivity.ClientId);
                addActivity.ActivityCode = newActivity.AddActivityCode;
                addActivity.DescriptionOf = newActivity.AddActivityDisplayDescription;
                addActivity.AssociationType = newActivity.AddtoType;
                addActivity.InsertedDate = DateTime.UtcNow;
                addActivity.UpdatedDate = DateTime.UtcNow;
                addActivity.ClosedDate = DateTime.UtcNow;
                addActivity.OwnerCode = newActivity.OwnerCode;
                addActivity.OwnerDescription = newActivity.OwnerDecription;
                addActivity.UniqAssociatedItem = Convert.ToInt32(newActivity.AddToTypeId);
                addActivity.UniqDepartment = 0;
                addActivity.UniqProfitCenter = 0;
                addActivity.Status = newActivity.Status;

                if (policyInfo != null)
                {
                    try
                    {
                        addActivity.UniqPolicy = Convert.ToInt32(policyInfo.PolicyId);
                        addActivity.PolicyNumber = policyInfo.PolicyNumber;
                        addActivity.PolicyDescription = policyInfo.PolicyDescription;
                        addActivity.UniqCdPolicyLineType = policyInfo.Type;
                        addActivity.EffectiveDate = Convert.ToDateTime(policyInfo.Effective);
                        addActivity.ExpirationDate = Convert.ToDateTime(policyInfo.Expiration);
                        addActivity.UniqAgency = Convert.ToInt32(policyInfo.UniqAgency);
                        addActivity.UniqBranch = Convert.ToInt32(policyInfo.UniqBranch);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                    }
                }

                if (LineInfo != null && LineInfo.LineId != 0)
                {
                    addActivity.UniqLine = Convert.ToInt32(LineInfo.LineId);
                    addActivity.LineCode = LineInfo.LineCode;
                    addActivity.LineDescription = LineInfo.LineOfBusiness;
                    addActivity.LineEffectiveDate = Convert.ToDateTime(LineInfo.Effective);
                    addActivity.LineExpirationDate = Convert.ToDateTime(LineInfo.Expiration);
                }

                if (ClaimInfo != null && ClaimInfo.ClaimId != 0)
                {
                    addActivity.UniqClaim = Convert.ToInt32(ClaimInfo.ClaimId);
                    addActivity.LossDate = Convert.ToDateTime(ClaimInfo.DateOfLoss);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            return addActivity;
        }
        static void InsertingActivityInLocalDB(AddActivity newActivity, dynamic UniqActivity, HIBOPActivity addActivity)
        {
            string activityInsertQuery = "INSERT OR REPLACE INTO HIBOPActivity(UniqActivity,UniqEntity,ActivityCode,DescriptionOf,UniqCdPolicyLineType,PolicyNumber,InsertedDate,UpdatedDate,ClosedDate,ExpirationDate,EffectiveDate,UniqAgency,UniqBranch,UniqAssociatedItem,AssociationType,OwnerCode,OwnerDescription,Status,UniqPolicy,UniqLine,UniqClaim,LossDate,PolicyDescription,LineCode,LineDescription,ICO,LineEffectiveDate,LineExpirationDate) VALUES ('" + addActivity.UniqActivity + "','" + addActivity.UniqEntity + "','" + addActivity.ActivityCode?.Replace("'", "''") + "','" + addActivity.DescriptionOf?.Replace("'", "''") + "','" + addActivity.UniqCdPolicyLineType?.Replace("'", "''") + "','" + addActivity.PolicyNumber?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(addActivity.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(addActivity.UpdatedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(addActivity.ClosedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(addActivity.ExpirationDate) + "','" + ExtensionClass.SqliteDateTimeFormat(addActivity.EffectiveDate) + "','" + addActivity.UniqAgency + "','" + addActivity.UniqBranch + "','" + addActivity.UniqAssociatedItem + "','" + addActivity.AssociationType?.Replace("'", "''") + "','" + addActivity.OwnerCode?.Replace("'", "''") + "','" + addActivity.OwnerDescription?.Replace("'", "''") + "','" + addActivity.Status?.Replace("'", "''") + "','" + addActivity.UniqPolicy + "','" + addActivity.UniqLine + "','" + addActivity.UniqClaim + "','" + addActivity.LossDate + "','" + addActivity.PolicyDescription?.Replace("'", "''") + "','" + addActivity.LineCode?.Replace("'", "''") + "','" + addActivity.LineDescription?.Replace("'", "''") + "','" + addActivity.ICO?.Replace("'", "''") + "','" + addActivity.LineEffectiveDate + "','" + addActivity.LineExpirationDate + "')";
            var activityInsertRsult = SqliteHelper.ExecuteCreateOrInsertQuery(activityInsertQuery);

            string attachmentInfoStatusUpdateQuery = $"update HIBOPAddActivity set IsPushedToEpic = 1 , IsEpicPushInProgress= 0 , TaskEventEpicId= {UniqActivity} where ActivityGuid = '{newActivity.ActivityGuid}'";
            var UpdatedeRsult = Helper.Helper.SqliteHelper.ExecuteCreateOrInsertQuery(attachmentInfoStatusUpdateQuery);

            Int32 UniqEmployee = 0;
            string uniEmployeeQuery = string.Format("Select UniqEntity from HIBOPEmployee where LookupCode='{0}'", newActivity.CurrentlyLoggedUserCode);
            var sqliteDataReader = SqliteHelper.ExecuteSelectQuery(uniEmployeeQuery);
            if (sqliteDataReader != null)
            {
                while (sqliteDataReader.Read())
                {
                    try
                    {
                        UniqEmployee = Convert.ToInt32(sqliteDataReader["UniqEntity"]);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, newActivity.CurrentlyLoggedUserCode);
                    }
                }
            }

            string activityEmployeeQuery = "INSERT OR REPLACE INTO HIBOPActivityEmployee(EmployeeLookupCode,UniqEmployee,UniqEntity,UniqActivity) VALUES ('" + newActivity.CurrentlyLoggedUserCode + "', '" + UniqEmployee + "', '" + addActivity.UniqEntity + "', '" + addActivity.UniqActivity + "')";
            var activityemployeeRsult = SqliteHelper.ExecuteCreateOrInsertQuery(activityEmployeeQuery);


            string favouriteUpdateQuery = $"update HIBOPFavourites set UniqActivity={UniqActivity} where ActivityGuid ='{newActivity.ActivityGuid}' and UniqActivity = 0";
            SqliteHelper.ExecuteCreateOrInsertQuery(favouriteUpdateQuery);
            //string attachmentInfoActivityUpdateQuery = $"update AttachmentInfo set ActivityId = {UniqActivity} where ActivityId = {newActivity.Id}";
            //SqliteHelper.ExecuteCreateOrInsertQuery(attachmentInfoActivityUpdateQuery);
            string attachmentInfoActivityGuidUpdateQuery = $"update AttachmentInfo set ActivityId = {UniqActivity} where ActivityGuid = '{newActivity.ActivityGuid}'";
            SqliteHelper.ExecuteCreateOrInsertQuery(attachmentInfoActivityGuidUpdateQuery);
        }
        static void UpdateActivitySuccessStatus(AddActivity newActivity, dynamic UniqActivity)
        {
            Policy policyInfo = null;
            Line LineInfo = null;
            Claim ClaimInfo = null;
            if (newActivity.AddtoType == "Policy")
            {
                string policyActivitiesQuery = string.Format("Select * From HIBOPPolicy where UniqPolicy={0} limit 1", newActivity.AddToTypeId);
                policyInfo = GetPolicyDetails(policyActivitiesQuery, newActivity);

                string linesActivitiesQuery = string.Format("Select * From HIBOPActivityLine where UniqPolicy={0} limit 1", policyInfo.PolicyId);
                LineInfo = GetActivityLineDetails(linesActivitiesQuery);
            }
            else if (newActivity.AddtoType == "Line")
            {
                using (var context = new HIB.Outlook.SQLite.HIBOutlookEntities())
                {
                    string linesActivitiesQuery = string.Format("Select * From HIBOPActivityLine where UniqLine={0} limit 1", newActivity.AddToTypeId);
                    LineInfo = GetActivityLineDetails(linesActivitiesQuery);

                    string policyActivitiesQuery = string.Format("Select * From HIBOPPolicy where UniqPolicy={0} limit 1", LineInfo.UniqPolicy);
                    policyInfo = GetPolicyDetails(policyActivitiesQuery, newActivity);
                }

            }
            else if (newActivity.AddtoType == "Claim")
            {
                string claimDetailQuery = string.Format("Select * From HIBOPClaim where UniqClaim={0} limit 1", newActivity.AddToTypeId);
                ClaimInfo = GetClaimDetails(claimDetailQuery);
            }

            HIBOPActivity addActivity = PrepareActivityObjectForDBUpdate(UniqActivity, newActivity, policyInfo, LineInfo, ClaimInfo);
            InsertingActivityInLocalDB(newActivity, UniqActivity, addActivity);
        }
        private static void PushingActivityToEpic(AddActivity newActivity)
        {
            try
            {
                if (newActivity.AddtoType == "Account")
                {
                    var accountDetails = GetAccountForActivity(newActivity.AddToTypeId);
                    newActivity.AgencyCode = accountDetails.AgencyCode;
                    newActivity.BranchCode = accountDetails.BranchCode;
                    if (string.IsNullOrEmpty(newActivity.AgencyCode) && string.IsNullOrEmpty(newActivity.BranchCode))
                    {
                        Logger.ErrorLog(string.Format("Agency Code and Branch Code is empty for this activity Description '{0}' with Owner Lookup Code '{1}'", newActivity.Description, newActivity.OwnerCode), Logger.SourceType.WindowsService, "");
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(newActivity.AddtoType) && newActivity.AddToTypeId != null && newActivity.ClientId != null)
                {
                    if (string.IsNullOrEmpty(newActivity.EndDate))
                    {
                        newActivity.EndDate = newActivity.StartDate;
                    }
                    var resultInfo = new SyncLocal().SaveActivityToEpic(newActivity);

                    if (resultInfo.IsSuccess)
                    {
                        UpdateActivitySuccessStatus(newActivity, resultInfo.Id);
                    }
                    else
                    {
                        string attachmentInfoStatusUpdateQuery = $"update HIBOPAddActivity set IsEpicPushInProgress= 0 where ActivityGuid = '{newActivity.ActivityGuid}'";
                        var UpdatedeRsult = Helper.Helper.SqliteHelper.ExecuteCreateOrInsertQuery(attachmentInfoStatusUpdateQuery);

                        Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, newActivity.CurrentlyLoggedUserCode);
                        if (IsOutlookOpen)
                        {
                            var TitleText = string.Format("Activity Addition Failed | {0}", newActivity.AddActivityCode);
                            var ContentText = "Please refer the log.";
                            CreateXMLForNotification(TitleText, ContentText, 3, newActivity.CurrentlyLoggedUserCode);
                        }

                    }
                }
                else
                {
                    Logger.ErrorLog(string.Format("AddToType or Client Id is null for this activity Description '{0}' with Owner Lookup Code '{1}'", newActivity.Description, newActivity.OwnerCode), Logger.SourceType.WindowsService, "");
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.StackTrace, Logger.SourceType.WindowsService, newActivity.CurrentlyLoggedUserCode);
                if (IsOutlookOpen)
                {
                    var TitleText = string.Format("Exception occurred during Activity Addition | {0}", newActivity.AddActivityCode);
                    var ContentText = "Please refer the log.";
                    CreateXMLForNotification(TitleText, ContentText, 3, newActivity.CurrentlyLoggedUserCode);
                }
            }
        }
        private static Int32 GetActivityId(Model.AttachmentInfo item)
        {
            Int32 activityId = 0;
            if (item.ActivityId == 0)
            {
                string activityIdQuery = $"select TaskEventEpicId from HIBOPAddActivity where ActivityGuid = '{item.ActivityGuid}'";
                var sqliteDataReader = Helper.Helper.SqliteHelper.ExecuteSelectQuery(activityIdQuery);
                while (sqliteDataReader != null && sqliteDataReader.Read())
                {
                    try
                    {
                        activityId = Convert.ToInt32(sqliteDataReader["TaskEventEpicId"]);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex.InnerException.Message, Logger.SourceType.WindowsService, item.EmployeeCode);
                    }

                }
                if (activityId != 0)
                {
                    string attachmentInfoActivityGuidUpdateQuery = $"update AttachmentInfo set ActivityId = {activityId} where ActivityGuid = '{item.ActivityGuid}'";
                    SqliteHelper.ExecuteCreateOrInsertQuery(attachmentInfoActivityGuidUpdateQuery);
                }
            }
            else
            {
                activityId = Convert.ToInt32(item.ActivityId);
            }
            return activityId;
        }
        static void UpdateAttachmentStatus(Model.AttachmentInfo DBAttachmentitem, Model.AttachmentInfo attachmentItem)
        {
            string AuditLog = string.Empty;
            if (attachmentItem.Status?.Status == Status.Success)
            {
                string attachmentInfoStatusUpdateQuery = $"update AttachmentInfo set IsPushedToEpic = 1,IsEpicPushInProgress = 0 where AttachmentIdentifier ='{DBAttachmentitem.AttachmentIdentifier}'";
                var UpdatedeRsult = Helper.Helper.SqliteHelper.ExecuteCreateOrInsertQuery(attachmentInfoStatusUpdateQuery);
                if (!string.IsNullOrEmpty(attachmentItem.FileDetails.FilePath) && File.Exists(attachmentItem.FileDetails.FilePath))
                {
                    File.Delete(attachmentItem.FileDetails.FilePath);
                }
                AuditLog = $"Insert into HIBOPOutlookPluginLog (UniqId,AttachmentInfoId,UniqEmployee,UniqEntity,UniqActivity,PolicyYear,PolicyType,DescriptionType,Description,FolderId,SubFolder1Id,SubFolder2Id,ClientAccessibleDate,EmailAction,InsertedDate,UpdatedDate,EmailSubject,ClientCode,Status,ErrorMessage) Values ('{DBAttachmentitem.Identifier}',{DBAttachmentitem.AttachmentId},'{DBAttachmentitem.EmployeeCode}',{DBAttachmentitem.ClientId},{DBAttachmentitem.ActivityId},'{DBAttachmentitem.PolicyYear}','{DBAttachmentitem.PolicyType?.Replace("'", "''")}','{DBAttachmentitem.DescriptionFrom?.Replace("'", "''")}','{DBAttachmentitem.Description?.Replace("'", "''")}',{DBAttachmentitem.FolderDetails.ParentFolderId},{DBAttachmentitem.FolderDetails.FolderId},{DBAttachmentitem.FolderDetails.SubFolderId},'{DBAttachmentitem.ClientAccessible}','{""}','{ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}','{ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}','{DBAttachmentitem.Subject}','{DBAttachmentitem.EmployeeCode}',{1},'{attachmentItem.Status?.Status.ToString()}')";

                if (IsOutlookOpen)
                {
                    CreateXMLForNotification(string.Format("Push to Epic Sucess  | {0}", DBAttachmentitem.Description), " ", 4, DBAttachmentitem.EmployeeCode);
                }
            }
            else if (attachmentItem.Status?.Status == Status.Failed || attachmentItem.Status?.Status == Status.InProgress)
            {
                Logger.ErrorLog(attachmentItem.Status.ErrorMessage, Logger.SourceType.WindowsService, DBAttachmentitem?.EmployeeCode);
                string attachmentInfoStatusUpdateQuery = $"update AttachmentInfo set IsEpicPushInProgress = 0,ErrorMessage = '{attachmentItem.Status?.ErrorMessage?.Replace("'", "''")}' where AttachmentIdentifier ='{DBAttachmentitem.AttachmentIdentifier}'";
                var UpdatedeRsult = Helper.Helper.SqliteHelper.ExecuteCreateOrInsertQuery(attachmentInfoStatusUpdateQuery);
                AuditLog = $"Insert into HIBOPOutlookPluginLog (UniqId,AttachmentInfoId,UniqEmployee,UniqEntity,UniqActivity,PolicyYear,PolicyType,DescriptionType,Description,FolderId,SubFolder1Id,SubFolder2Id,ClientAccessibleDate,EmailAction,InsertedDate,UpdatedDate,EmailSubject,ClientCode,Status,ErrorMessage) Values ('{DBAttachmentitem.Identifier}',{DBAttachmentitem.AttachmentId},'{DBAttachmentitem.EmployeeCode}',{DBAttachmentitem.ClientId},{DBAttachmentitem.ActivityId},'{DBAttachmentitem.PolicyYear}','{DBAttachmentitem.PolicyType?.Replace("'", "''")}','{DBAttachmentitem.DescriptionFrom?.Replace("'", "''")}','{DBAttachmentitem.Description?.Replace("'", "''")}',{DBAttachmentitem.FolderDetails.ParentFolderId},{DBAttachmentitem.FolderDetails.FolderId},{DBAttachmentitem.FolderDetails.SubFolderId},'{DBAttachmentitem.ClientAccessible}','{""}','{ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}','{ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}','{DBAttachmentitem.Subject}','{DBAttachmentitem.EmployeeCode}',{0},'{attachmentItem.Status?.ErrorMessage?.Replace("'", "''")}')";

                if (IsOutlookOpen)
                {
                    CreateXMLForNotification(string.Format("New Notification for Epic Attachment Plugin"), "Please check the notification list.", 3, DBAttachmentitem.EmployeeCode);
                }
            }

            Helper.Helper.SqliteHelper.ExecuteCreateOrInsertQuery(AuditLog);
        }
        private static async Task PushingAttachmentToEpic(Model.AttachmentInfo item)
        {
            try
            {
                var helper = new SyncLocal();

                item.ActivityId = GetActivityId(item);

                if (item.ActivityId == 0)
                {
                    string attachmentInfoStatusUpdateQuery = $"update AttachmentInfo set IsEpicPushInProgress = 0 where AttachmentIdentifier ='{item.AttachmentIdentifier}'";
                    var UpdatedeRsult = Helper.Helper.SqliteHelper.ExecuteCreateOrInsertQuery(attachmentInfoStatusUpdateQuery);
                }
                else
                {
                    var attachmentItem = await helper.SaveAttachmentsToEpic(item);
                    UpdateAttachmentStatus(item, attachmentItem);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(string.Format("Push to Epic Failed  | {0} due to {1}", item.Description, ex.StackTrace), Logger.SourceType.WindowsService, item.EmployeeCode);
                string attachmentInfoStatusUpdateQuery = $"update AttachmentInfo set IsEpicPushInProgress = 0,ErrorMessage = '{ex.Message?.Replace("'", "''")}' where AttachmentIdentifier ='{item.AttachmentIdentifier}'";
                var UpdatedeRsult = Helper.Helper.SqliteHelper.ExecuteCreateOrInsertQuery(attachmentInfoStatusUpdateQuery);
                if (IsOutlookOpen)
                {
                    CreateXMLForNotification(string.Format("New Notification for Epic Attachment Plugin"), "Please check the notification list.", 3, item.EmployeeCode);
                }
                Logger.ErrorLog(ex.InnerException.Message, Logger.SourceType.WindowsService, item.EmployeeCode);
            }
        }
        private void DeleteFailedAttachment(DictionaryEntry filePath)
        {
            var attachmentCollection = XMLSerializeHelper.DeSerialize<Model.AttachmentInfo>(XMLFolderType.Service, "DeleteFailedAttachments");
            MappingCollection.Remove(filePath.Key);
            foreach (var attachmentInfo in attachmentCollection)
            {
                try
                {
                    var attachmentFilePathQuery = string.Format("Select AttachmentFilePath From AttachmentInfo where AttachmentId={0}", attachmentInfo.AttachmentId);
                    var attachmentFilePath = GetMailItemFilePath(attachmentFilePathQuery);
                    if (File.Exists(attachmentFilePath))
                    {
                        File.Delete(attachmentFilePath);
                    }
                    var attachmentQuery = string.Format("Delete From AttachmentInfo where AttachmentId={0}", attachmentInfo.AttachmentId);
                    var DeleteResult = SQLiteHandler.ExecuteCreateOrInsertQuery(attachmentQuery);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }
            }
        }
        private void RetryFailedAttachment(DictionaryEntry filePath, Task task)
        {
            var attachmentCollection = XMLSerializeHelper.DeSerialize<Model.AttachmentInfo>(XMLFolderType.Service, "RetryFailedAttachments");
            MappingCollection.Remove(filePath.Key);
            foreach (var attachmentInfo in attachmentCollection)
            {
                try
                {
                    var attachmentQuery = string.Format("Select * From AttachmentInfo where AttachmentId={0}", attachmentInfo.AttachmentId);
                    var attachment = GetAttachmentInfoDetail(attachmentQuery);
                    task = Task.Run(async () =>
                    {
                        await PushingAttachmentToEpic(attachment);
                    });
                    task.Wait();
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }
            }

        }
        private void AddAttachment(DictionaryEntry filePath, Task task)
        {
            var attachmentCollection = XMLSerializeHelper.DeSerialize<Model.AttachmentInfo>(XMLFolderType.AddIn);
            MappingCollection.Remove(filePath.Key);
            foreach (var attachmentInfo in attachmentCollection)
            {
                try
                {
                    //var Identifier = Guid.NewGuid().ToString();
                    //attachmentInfo.AttachmentIdentifier = Identifier;
                    string queryString = $"Insert into AttachmentInfo (ClientId,ActivityId,Description,FileExtension,FileName,ParentFolderId,ParentFolderName,FolderId,FolderName,SubFolderId,SubFolderName,PolicyCode,PolicyType,PolicyYear,ClientAccessible,EmailFromAddress,EmailToAddress,Subject,ReceivedDate,AttachmentFilePath,IsPushedToEpic,IsAttachDelete,Identifier,IsActiveClient,IsActiveActivity,DescFrom,EmployeeCode,CreatedDate,ModifiedDate,AttachmentMailBody,IsDeletedFromZFolder,EmailFromDisplayName,EmailToDisplayName,EmailCCAddress,EmailCCDisplayName,ActivityGuid,UserName,DomainName,IsEpicPushInProgress,AttachmentIdentifier,EntryId,RecievedDateWithTime,DisplayMailBody) Values ({attachmentInfo.ClientId},{attachmentInfo.ActivityId},'{attachmentInfo.Description?.Replace("'", "''")}','{attachmentInfo.FileDetails.FileExtension}','{attachmentInfo.FileDetails.FileName?.Replace("'", "''")}',{attachmentInfo.FolderDetails.ParentFolderId},'{attachmentInfo.FolderDetails.ParentFolderName?.Replace("'", "''")}',{attachmentInfo.FolderDetails.FolderId},'{attachmentInfo.FolderDetails.FolderName?.Replace("'", "''")}','{attachmentInfo.FolderDetails.SubFolderId}','{attachmentInfo.FolderDetails.SubFolderName?.Replace("'", "''")}','{attachmentInfo.PolicyCode?.Replace("'", "''")}','{attachmentInfo.PolicyType?.Replace("'", "''")}','{attachmentInfo.PolicyYear}','{attachmentInfo.ClientAccessible}','{attachmentInfo.EmailFromAddress?.Replace("'", "''")}','{attachmentInfo.EmailToAddress?.Replace("'", "''")}','{attachmentInfo.Subject?.Replace("'", "''")}','{attachmentInfo.ReceivedDate}','{attachmentInfo.AttachmentFilePath}',{0},{Convert.ToInt32(attachmentInfo.IsAttachDelete)},'{attachmentInfo.Identifier}',{Convert.ToInt32(attachmentInfo.IsActiveClient)},{Convert.ToInt32(attachmentInfo.IsClosedActivity)}, '{attachmentInfo.DescriptionFrom?.Replace("'", "''")}','{attachmentInfo.EmployeeCode}','{attachmentInfo.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")}','{attachmentInfo.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")}','{attachmentInfo.AttachmentMailBody?.Replace("'", "''")}',{0},'{attachmentInfo.EmailFromDisplayName?.Replace("'", "")}','{attachmentInfo.EmailToDisplayName?.Replace("'", "")}','{attachmentInfo.EmailCCAddress?.Replace("'", "")}','{attachmentInfo.EmailCCDisplayName}','{attachmentInfo.ActivityGuid}','{attachmentInfo.UserName}','{attachmentInfo.DomainName}',{1},'{attachmentInfo.AttachmentIdentifier}','{attachmentInfo.EntryId}','{attachmentInfo.ReceivedDateWithTime}','{attachmentInfo.DisplayMailBody}')";
                    var result = SQLiteHandler.ExecuteCreateOrInsertQuery(queryString);

                    string lookupCode = string.Empty;
                    if (attachmentInfo.ClientId != 0)
                    {
                        string clientCodeString = string.Format("Select LookupCode from HIBOPClient where UniqEntity='{0}' limit 1", attachmentInfo.ClientId);
                        var ClientCodeDataReader = SQLiteHandler.ExecuteSelectQuery(clientCodeString);
                        while (ClientCodeDataReader.Read())
                        {
                            try
                            {
                                lookupCode = Convert.ToString(ClientCodeDataReader["LookupCode"]);
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                            }
                        }
                    }

                    string logAuditqueryString = $"Insert into HIBOPOutlookPluginLog (UniqId,AttachmentInfoId,UniqEmployee,UniqEntity,UniqActivity,PolicyYear,PolicyType,DescriptionType,Description,FolderId,SubFolder1Id,SubFolder2Id,ClientAccessibleDate,EmailAction,InsertedDate,UpdatedDate,EmailSubject,ClientCode) Values ('{attachmentInfo.Identifier}',{result.RowId},'{attachmentInfo.EmployeeCode}',{attachmentInfo.ClientId},{attachmentInfo.ActivityId},'{attachmentInfo.PolicyYear}','{attachmentInfo.PolicyType?.Replace("'", "''")}','{attachmentInfo.DescriptionFrom?.Replace("'", "''")}','{attachmentInfo.Description?.Replace("'", "''")}',{attachmentInfo.FolderDetails.ParentFolderId},{attachmentInfo.FolderDetails.FolderId},{attachmentInfo.FolderDetails.SubFolderId},'{attachmentInfo.ClientAccessible}','{""}','{ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}','{ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}','{attachmentInfo.Subject}','{lookupCode}')";
                    var Auditresult = SQLiteHandler.ExecuteCreateOrInsertQuery(logAuditqueryString);

                    attachmentInfo.FileDetails.FilePath = attachmentInfo.AttachmentFilePath;

                    var policyYear = string.Equals(attachmentInfo.PolicyYear, "(none)", StringComparison.InvariantCultureIgnoreCase) ? string.Empty : attachmentInfo.PolicyYear;
                    var policyCode = string.Equals(attachmentInfo.PolicyCode, "(none)", StringComparison.InvariantCultureIgnoreCase) ? string.Empty : attachmentInfo.PolicyCode;
                    attachmentInfo.Description = $"{policyYear} {policyCode} {attachmentInfo.Description}";
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }
            }
            task = Task.Run(async () =>
            {
                foreach (var attachmentInfo in attachmentCollection)
                {
                    await PushingAttachmentToEpic(attachmentInfo);
                }
            });

        }
        private void UpdateActivityEmployee(DictionaryEntry filePath)
        {
            var employeeCollection = XMLSerializeHelper.DeSerialize<EmployeeInfo>(XMLFolderType.AddIn, "UpdateActivityEmployee");
            MappingCollection.Remove(filePath.Key);
            var employee = employeeCollection.FirstOrDefault();
            if (employee != null)
            {
                string attachmentInfoStatusUpdateQuery = $"update HIBOPEmployee set SyncStatusNotified = 1";//where LookupCode ='{employee.LookupCode}'
                var UpdatedeRsult = SQLiteHandler.ExecuteCreateOrInsertQuery(attachmentInfoStatusUpdateQuery);
            }
        }
        private void UpdateAttachmentInfo(DictionaryEntry filePath)
        {
            var attachmentCollection = XMLSerializeHelper.DeSerialize<Model.AttachmentInfo>(XMLFolderType.Service, "UpdateAttachmentInfo");
            MappingCollection.Remove(filePath.Key);
            var attachmentItem = attachmentCollection.FirstOrDefault();
            string AuditLog = string.Empty;
            if (attachmentItem != null & attachmentItem.Status.Status == Status.Success)
            {
                string attachmentInfoStatusUpdateQuery = $"update AttachmentInfo set IsPushedToEpic = 1 where AttachmentId ={attachmentItem.AttachmentId}";
                var UpdatedeRsult = SQLiteHandler.ExecuteCreateOrInsertQuery(attachmentInfoStatusUpdateQuery);
                if (!string.IsNullOrEmpty(attachmentItem.FileDetails.FilePath) && File.Exists(attachmentItem.FileDetails.FilePath))
                {
                    File.Delete(attachmentItem.FileDetails.FilePath);
                }
                AuditLog = $"Update HIBOPOutlookPluginLog set Status = 1,UpdatedDate='{ ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}' where AttachmentInfoId ={attachmentItem.AttachmentId}";
            }
            else
            {
                AuditLog = $"Update HIBOPOutlookPluginLog set Status = 0,UpdatedDate='{ ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}' where AttachmentInfoId ={attachmentItem.AttachmentId}";
            }
            SQLiteHandler.ExecuteCreateOrInsertQuery(AuditLog);
        }
        private void UpdateZProcessingFolderStatus(DictionaryEntry filePath)
        {
            var attachmentCollection = XMLSerializeHelper.DeSerialize<Model.AttachmentInfo>(XMLFolderType.AddIn, "UpdateZProcessingFolderStatus");
            MappingCollection.Remove(filePath.Key);
            foreach (var Attachmentitem in attachmentCollection)
            {
                try
                {
                    string attachmentInfoStatusUpdateQuery = $"update AttachmentInfo set IsDeletedFromZFolder = 1 where AttachmentId ={Attachmentitem.AttachmentId}";
                    SQLiteHandler.ExecuteCreateOrInsertQuery(attachmentInfoStatusUpdateQuery);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, Attachmentitem.EmployeeCode);
                }
            }

        }
        private void AddFavourite(DictionaryEntry filePath)
        {
            var favoriteCollection = XMLSerializeHelper.DeSerialize<FavouriteInfo>(XMLFolderType.AddIn);
            MappingCollection.Remove(filePath.Key);
            foreach (var favouriteInfo in favoriteCollection)
            {
                try
                {
                    string queryString = $"Insert into HIBOPFavourites (FavouriteName,UniqEmployee,UniqEntity,IsActiveClient,UniqActivity,IsClosedActivity,PolicyYear,PolicyType,DescriptionType,Description,FolderId,SubFolder1Id,SubFolder2Id,ClientAccessibleDate,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,ActivityGuid) Values ('{favouriteInfo.FavourtieName}','{favouriteInfo.UniqEmployee}',{favouriteInfo.UniqEntity},{favouriteInfo.IsActiveClient},{favouriteInfo.UniqActivity},{favouriteInfo.IsClosedActivity},'{favouriteInfo.PolicyYear}','{favouriteInfo.PolicyType?.Replace("'", "''")}','{favouriteInfo.DescriptionType}','{favouriteInfo.Description?.Replace("'", "''")}',{favouriteInfo.FolderId},{favouriteInfo.SubFolder1Id},{favouriteInfo.SubFolder2Id},'{favouriteInfo.ClientAccessibleDate}','{favouriteInfo.CreatedBy}','{favouriteInfo.CreatedDate}','{favouriteInfo.ModifiedBy}','{favouriteInfo.ModifiedDate}','{favouriteInfo.ActivityGuid}')";
                    var result = SQLiteHandler.ExecuteCreateOrInsertQuery(queryString);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }
            }
        }
        private void UpdateFavourite(DictionaryEntry filePath)
        {
            var favoriteCollection = XMLSerializeHelper.DeSerialize<FavouriteInfo>(XMLFolderType.AddIn, "UpdateFavouriteInfo");
            MappingCollection.Remove(filePath.Key);
            foreach (var favouriteInfo in favoriteCollection)
            {
                try
                {
                    string queryString = $"update HIBOPFavourites set ModifiedDate = '{favouriteInfo.ModifiedDate}' , UniqActivity = {favouriteInfo.UniqActivity} where FavId = {favouriteInfo.FavId}";
                    var result = SQLiteHandler.ExecuteCreateOrInsertQuery(queryString);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }
            }

        }
        private void AddActivity(DictionaryEntry filePath)
        {
            var addActivityCollection = XMLSerializeHelper.DeSerialize<AddActivity>(XMLFolderType.AddIn);
            MappingCollection.Remove(filePath.Key);
            foreach (var addActivity in addActivityCollection)
            {
                try
                {
                    string queryString = $"Insert into HIBOPAddActivity (UniqEntity,ClientLookupCode,AddToType,AddToTypeId,AddActivityCode,AddActivityDescription,AddActivityDisplayDescription,AddActivityId,OwnerCode,OwnerDecription,PriorityId,Priority,UpdateId,[Update],ReminderDate,ReminderTime,StartDate,StartTime,EndDate,EndTime,WhoToContactName,WhoToContactId,ContactMode,ContactModeId,ContactDetail,ContactDetailId,AccessLevel,AccessLevelId,Description,IsPushedToEpic,CurrentlyLoggedLookupCode,TaskEventEpicId,ActivityGuid,Status,IsEpicPushInProgress) Values ({addActivity.ClientId},'{addActivity.ClientLookupCode?.Replace("'", "''")}','{addActivity.AddtoType?.Replace("'", "''")}',{addActivity.AddToTypeId},'{addActivity.AddActivityCode?.Replace("'", "''")}','{addActivity.AddActivityDescription?.Replace("'", "''")}','{addActivity.AddActivityDisplayDescription?.Replace("'", "''")}',{addActivity.AddActivityId},'{addActivity.OwnerCode?.Replace("'", "''")}','{addActivity.OwnerDecription?.Replace("'", "''")}','{addActivity.PriorityId}','{addActivity.Priority?.Replace("'", "''")}','{addActivity.UpdateId}','{addActivity.Update?.Replace("'", "''")}','{addActivity.ReminderDate}','{addActivity.ReminderTime}','{addActivity.StartDate}','{addActivity.StartTime}','{addActivity.EndDate}','{addActivity.EndTime}','{addActivity.WhoToContactName?.Replace("'", "''")}','{addActivity.WhoToContactId}','{addActivity.ContactMode?.Replace("'", "''")}',{addActivity.ContactModeId},'{addActivity.ContactDetail}',{addActivity.ContactDetailId},'{addActivity.AccessLevel?.Replace("'", "''")}',{addActivity.AccessLevelId},'{addActivity.Description?.Replace("'", "''")}',{addActivity.IsPushToEpic},'{addActivity.CurrentlyLoggedUserCode}',{addActivity.TaskEventEpicId},'{addActivity.ActivityGuid}','{addActivity.Status}',{1})";
                    var result = SQLiteHandler.ExecuteCreateOrInsertQuery(queryString);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }

                PushingActivityToEpic(addActivity);
            }
        }
        private void AddErrorLogFromAddin(DictionaryEntry filePath)
        {
            var addinErrorLog = XMLSerializeHelper.DeSerialize<ErrorLogInfo>(XMLFolderType.AddIn, "AddInErrorLog");
            MappingCollection.Remove(filePath.Key);
            foreach (var item in addinErrorLog)
            {
                try
                {
                    string queryString = $"Insert into HIBOPErrorLog (Source,Thread,Level,Logger,Message,Exception,LoggedBy,LogDate) Values ( 'Outlook Addin','{item.Thread}','{item.Level}','{item.Logger}','{item.Message?.Replace("'", "''")}','{item.Exception?.Replace("'", "''")}','{item.LoggedBy?.Replace("'", "''")}',datetime('now','localtime'))";
                    var result = SQLiteHandler.ExecuteCreateOrInsertQuery(queryString);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }
            }
        }
        private void DeleteFavourite(DictionaryEntry filePath)
        {
            var favouriteCollection = XMLSerializeHelper.DeSerialize<FavouriteInfo>(XMLFolderType.AddIn, "DeleteFavouriteInfo");
            foreach (var item in favouriteCollection)
            {
                try
                {
                    string _deleteQuery = string.Format("Delete from HIBOPFavourites where FavouriteName = '{0}'", item.FavourtieName);
                    var result = SQLiteHandler.ExecuteCreateOrInsertQuery(_deleteQuery);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }
            }
        }
        private void AddErrorLogFromService(DictionaryEntry filePath)
        {
            var addinErrorLog = XMLSerializeHelper.DeSerialize<ErrorLogInfo>(XMLFolderType.Service, "ServiceErrorLog");
            MappingCollection.Remove(filePath.Key);
            foreach (var item in addinErrorLog)
            {
                try
                {
                    string queryString = $"Insert into HIBOPErrorLog (Source,Thread,Level,Logger,Message,Exception,LoggedBy,LogDate) Values ('Windows Service','{item.Thread}','{item.Level}','{item.Logger}','{item.Message?.Replace("'", "''")}','{item.Exception?.Replace("'", "''")}','{item.LoggedBy?.Replace("'", "''")}',datetime('now','localtime'))";
                    var result = SQLiteHandler.ExecuteCreateOrInsertQuery(queryString);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }
            }
        }
        private void GetClientInfo(DictionaryEntry filePath)
        {
            var clientList = XMLSerializeHelper.DeSerialize<ClientInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (clientList != null && clientList.Any())
            {
                Model.ResultInfo resultInfo = _clientRepository.SyncClient(clientList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = clientList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.Client);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityInfo(DictionaryEntry filePath)
        {
            MappingCollection.Remove(filePath.Key);
            var activityList = XMLSerializeHelper.DeSerialize<ActivityInfo>(XMLFolderType.Service);
            if (activityList != null && activityList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivity(activityList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.Activity);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityEmployees(DictionaryEntry filePath)
        {
            MappingCollection.Remove(filePath.Key);
            var activityEmployeeList = XMLSerializeHelper.DeSerialize<ActivityEmployee>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityEmployeeList != null && activityEmployeeList.Any())
            {

                Model.ResultInfo resultInfo = _activityRepository.SyncActivityEmployees(activityEmployeeList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityEmployeeList.FirstOrDefault()?.EmployeeLookupcode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityEmployees);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }

        }
        private void GetClientEmployees(DictionaryEntry filePath)
        {
            MappingCollection.Remove(filePath.Key);
            var clientEmployeeList = XMLSerializeHelper.DeSerialize<ClientEmployee>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (clientEmployeeList != null && clientEmployeeList.Any())
            {

                Model.ResultInfo resultInfo = _clientRepository.SyncClientEmployee(clientEmployeeList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = clientEmployeeList.FirstOrDefault()?.EmployeeLookupcode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ClientEmployee);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetEmployeeInfo(DictionaryEntry filePath)
        {
            MappingCollection.Remove(filePath.Key);
            var activityEmployeeList = XMLSerializeHelper.DeSerialize<EmployeeInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityEmployeeList != null && activityEmployeeList.Any())
            {
                var firstItem = activityEmployeeList.FirstOrDefault();
                if (firstItem != null && firstItem.EntityId != 0)
                {
                    Model.ResultInfo resultInfo = _activityRepository.SyncActivityEmployee(activityEmployeeList);
                    if (resultInfo.IsSuccess)
                    {
                        _logRepository.SaveSyncLog(Enums.ServiceMethod.ActivityEmployee.ToString(), DateTime.Now);

                    }
                    else
                    {
                        Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
                    }
                }

                else
                {
                    List<string> LookUpCodeList = new List<string>() { firstItem.LookupCode };
                    Model.ResultInfo resultInfo = _activityRepository.InsertEmployeeLookUpCode(LookUpCodeList, false);
                    if (!resultInfo.IsSuccess)
                        Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
                }
            }

        }
        private void GetFolderInfo(DictionaryEntry filePath)
        {
            var activityList = XMLSerializeHelper.DeSerialize<FolderInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityList != null && activityList.Any())
            {
                Model.ResultInfo resultInfo = _folderRepository.SyncDBFolder(activityList);
                if (resultInfo.IsSuccess)
                    _logRepository.SaveSyncLog(Enums.ServiceMethod.Folder.ToString(), DateTime.Now);
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetPolicyLineTypeInfo(DictionaryEntry filePath)
        {
            var policyList = XMLSerializeHelper.DeSerialize<PolicyTypeInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (policyList != null && policyList.Any())
            {
                Model.ResultInfo resultInfo = _policyLineTypeRepository.SyncDBPolicyLineType(policyList);
                if (resultInfo.IsSuccess)
                    _logRepository.SaveSyncLog(Enums.ServiceMethod.PolicyLineType.ToString(), DateTime.Now);
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));

            }
        }
        private void GetActivityClaims(DictionaryEntry filePath)
        {
            var activityClaimList = XMLSerializeHelper.DeSerialize<ActivityClaimInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityClaimList != null && activityClaimList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityClaim(activityClaimList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityClaimList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityClaim);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityServices(DictionaryEntry filePath)
        {
            var activityServiceList = XMLSerializeHelper.DeSerialize<ActivityServiceInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityServiceList != null && activityServiceList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityService(activityServiceList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityServiceList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityService);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityLines(DictionaryEntry filePath)
        {
            MappingCollection.Remove(filePath.Key);
            var activityLineList = XMLSerializeHelper.DeSerialize<ActivityLineInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityLineList != null && activityLineList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityLine(activityLineList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityLineList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityLine);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityOpportunities(DictionaryEntry filePath)
        {
            var activityOpportunityList = XMLSerializeHelper.DeSerialize<ActivityOpportunityInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityOpportunityList != null && activityOpportunityList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityOpportunity(activityOpportunityList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityOpportunityList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityOpportunity);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }

        }
        private void GetActivityOwners(DictionaryEntry filePath)
        {
            var activityOwnerList = XMLSerializeHelper.DeSerialize<ActivityOwnerListInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityOwnerList != null && activityOwnerList.Any())
            {

                Model.ResultInfo resultInfo = _activityRepository.SyncActivityOwnerList(activityOwnerList);
                if (resultInfo.IsSuccess)
                    _logRepository.SaveSyncLog(Enums.ServiceMethod.ActivityOwnerList.ToString(), DateTime.Now);
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityPolicys(DictionaryEntry filePath)
        {
            var activityPolicyList = XMLSerializeHelper.DeSerialize<ActivityPolicyInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityPolicyList != null && activityPolicyList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityPolicy(activityPolicyList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityPolicyList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityPolicy);
                    SyncCompletedForSpecificUser(true, userLookupCode);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityEmployeeAgencys(DictionaryEntry filePath)
        {
            var activityEmployeeAgencyList = XMLSerializeHelper.DeSerialize<EmployeeAgencyInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityEmployeeAgencyList != null && activityEmployeeAgencyList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityEmployeeAgency(activityEmployeeAgencyList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityEmployeeAgencyList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityEmployeeAgency);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityLookups(DictionaryEntry filePath)
        {
            var activityLookUpList = XMLSerializeHelper.DeSerialize<ActivityLookUpInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityLookUpList != null && activityLookUpList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityLookUp(activityLookUpList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityLookUpList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityLookUp);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityEvidences(DictionaryEntry filePath)
        {
            var activityEvidenceList = XMLSerializeHelper.DeSerialize<ActivityEvidenceInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityEvidenceList != null && activityEvidenceList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityEvidence(activityEvidenceList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityEvidenceList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityEvidence);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityCommonLookups(DictionaryEntry filePath)
        {
            var activityCommonLookUpList = XMLSerializeHelper.DeSerialize<ActivityCommonLookUpInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);

            if (activityCommonLookUpList != null && activityCommonLookUpList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityCommonLookUp(activityCommonLookUpList);
                if (resultInfo.IsSuccess)
                    _logRepository.SaveSyncLog(Enums.ServiceMethod.ActivityCommonLookUp.ToString(), DateTime.Now);
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));

            }
        }
        private void GetActivityAccounts(DictionaryEntry filePath)
        {
            var activityAccountInfoList = XMLSerializeHelper.DeSerialize<ActivityAccountInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityAccountInfoList != null && activityAccountInfoList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityAccount(activityAccountInfoList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityAccountInfoList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityAccount);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityMarketingDetails(DictionaryEntry filePath)
        {
            var activityMarketingList = XMLSerializeHelper.DeSerialize<ActivityMarketingInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityMarketingList != null && activityMarketingList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityMarketing(activityMarketingList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityMarketingList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityMarketing);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityClientContacts(DictionaryEntry filePath)
        {
            var activityClientContactList = XMLSerializeHelper.DeSerialize<ActivityClientContactInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityClientContactList != null && activityClientContactList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityClientContact(activityClientContactList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityClientContactList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityClientContact);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }

        }
        private void GetActivityLists(DictionaryEntry filePath)
        {
            var activityList = XMLSerializeHelper.DeSerialize<ActivityListInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityList != null && activityList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityList(activityList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityList);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityBills(DictionaryEntry filePath)
        {
            var activityBillList = XMLSerializeHelper.DeSerialize<ActivityBillInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityBillList != null && activityBillList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityBill(activityBillList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityBillList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityBill);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityCarrierSubmission(DictionaryEntry filePath)
        {
            var activityCarrierList = XMLSerializeHelper.DeSerialize<ActivityCarrierInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityCarrierList != null && activityCarrierList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityCarrierSubmission(activityCarrierList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityCarrierList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityCarrierSubmission);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityTransactions(DictionaryEntry filePath)
        {
            var activityTransactionList = XMLSerializeHelper.DeSerialize<ActivityTransactionInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityTransactionList != null && activityTransactionList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityTransaction(activityTransactionList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityTransactionList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityTransaction);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void GetActivityCertificate(DictionaryEntry filePath)
        {
            var activityCertificateList = XMLSerializeHelper.DeSerialize<ActivityCertificateInfo>(XMLFolderType.Service);
            MappingCollection.Remove(filePath.Key);
            if (activityCertificateList != null && activityCertificateList.Any())
            {
                Model.ResultInfo resultInfo = _activityRepository.SyncActivityCertificate(activityCertificateList);
                if (resultInfo.IsSuccess)
                {
                    var userLookupCode = activityCertificateList.FirstOrDefault()?.UserLookupCode;
                    UpdateSyncLogUserTable(userLookupCode, Enums.ServiceMethod.ActivityCertificate);
                }
                else
                    Logger.ErrorLog(resultInfo.ErrorMessage, Logger.SourceType.WindowsService, "", typeof(XmlToSqliteHandler));
            }
        }
        private void UpdateSyncLog(DictionaryEntry filePath, string fieldName)
        {
            MappingCollection.Remove(filePath.Key);
            _logRepository.SaveSyncLog(fieldName, DateTime.Now);
        }

        public void UpdateXmltoSqlite()
        {
            try
            {
                Task task = null;
                if (MappingCollection.Count > 0)
                {
                    if (System.Threading.Monitor.TryEnter(lockObject, 900000))
                    {
                        try
                        {
                            var filePath = MappingCollection.Cast<DictionaryEntry>().FirstOrDefault();

                            switch (filePath.Key?.ToString())
                            {
                                case "AttachmentInfo":
                                    {
                                        AddAttachment(filePath, task);
                                        break;
                                    }
                                case "RetryFailedAttachments":
                                    {
                                        RetryFailedAttachment(filePath, task);
                                        break;
                                    }
                                case "DeleteFailedAttachments":
                                    {
                                        DeleteFailedAttachment(filePath);
                                        break;
                                    }
                                case "UpdateActivityEmployee":
                                    {
                                        UpdateActivityEmployee(filePath);
                                        break;
                                    }
                                case "UpdateAttachmentInfo":
                                    {
                                        UpdateAttachmentInfo(filePath);
                                        break;
                                    }
                                case "UpdateZProcessingFolderStatus":
                                    {
                                        UpdateZProcessingFolderStatus(filePath);
                                        break;
                                    }
                                case "FavouriteInfo":
                                    {
                                        AddFavourite(filePath);
                                        break;
                                    }
                                case "UpdateFavouriteInfo":
                                    {
                                        UpdateFavourite(filePath);
                                        break;
                                    }
                                case "AddActivity":
                                    {
                                        AddActivity(filePath);
                                        break;
                                    }

                                case "AddInErrorLog":
                                    {
                                        AddErrorLogFromAddin(filePath);
                                        break;
                                    }
                                case "DeleteFavouriteInfo":
                                    {
                                        DeleteFavourite(filePath);
                                        break;
                                    }
                                case "ServiceErrorLog":
                                    {
                                        AddErrorLogFromService(filePath);
                                        break;
                                    }

                                case "ClientInfo":
                                    {
                                        GetClientInfo(filePath);
                                        break;
                                    }
                                case "ActivityInfo":
                                    {
                                        GetActivityInfo(filePath);
                                        break;
                                    }
                                case "ActivityEmployee":
                                    {
                                        GetActivityEmployees(filePath);
                                        break;
                                    }

                                case "ClientEmployee":
                                    {
                                        GetClientEmployees(filePath);
                                        break;
                                    }
                                case "EmployeeInfo":
                                    {
                                        GetEmployeeInfo(filePath);
                                        break;
                                    }
                                case "FolderInfo":
                                    {
                                        GetFolderInfo(filePath);
                                        break;
                                    }

                                case "PolicyTypeInfo":
                                    {
                                        GetPolicyLineTypeInfo(filePath);
                                        break;
                                    }
                                case "ActivityClaimInfo":
                                    {
                                        GetActivityClaims(filePath);
                                        break;
                                    }
                                case "ActivityServiceInfo":
                                    {
                                        GetActivityServices(filePath);
                                        break;
                                    }
                                case "ActivityLineInfo":
                                    {
                                        GetActivityLines(filePath);
                                        break;
                                    }
                                case "ActivityOpportunityInfo":
                                    {
                                        GetActivityOpportunities(filePath);
                                        break;
                                    }

                                case "ActivityOwnerListInfo":
                                    {
                                        GetActivityOwners(filePath);
                                        break;
                                    }

                                case "ActivityPolicyInfo":
                                    {
                                        GetActivityPolicys(filePath);
                                        break;
                                    }

                                case "EmployeeAgencyInfo":
                                    {
                                        GetActivityEmployeeAgencys(filePath);
                                        break;
                                    }
                                case "ActivityLookUpInfo":
                                    {
                                        GetActivityLookups(filePath);
                                        break;
                                    }

                                case "ActivityEvidenceInfo":
                                    {
                                        GetActivityEvidences(filePath);
                                        break;
                                    }

                                case "ActivityCommonLookUpInfo":
                                    {
                                        GetActivityCommonLookups(filePath);
                                        break;
                                    }
                                case "ActivityAccountInfo":
                                    {
                                        GetActivityAccounts(filePath);
                                        break;
                                    }
                                case "ActivityMarketingInfo":
                                    {
                                        GetActivityMarketingDetails(filePath);
                                        break;
                                    }
                                case "ActivityClientContactInfo":
                                    {
                                        GetActivityClientContacts(filePath);
                                        break;
                                    }
                                case "ActivityListInfo":
                                    {
                                        GetActivityLists(filePath);
                                        break;
                                    }
                                case "ActivityBillInfo":
                                    {
                                        GetActivityBills(filePath);
                                        break;
                                    }
                                case "ActivityCarrierInfo":
                                    {
                                        GetActivityCarrierSubmission(filePath);
                                        break;
                                    }
                                case "ActivityTransactionInfo":
                                    {
                                        GetActivityTransactions(filePath);
                                        break;
                                    }
                                case "ActivityCertificateInfo":
                                    {
                                        GetActivityCertificate(filePath);
                                        break;
                                    }
                                case "UpdateAuditLog":
                                    {
                                        UpdateSyncLog(filePath, Enums.ServiceMethod.AuditLog.ToString());
                                        break;
                                    }
                                case "UpdateErrorLog":
                                    {
                                        UpdateSyncLog(filePath, Enums.ServiceMethod.ErrorLog.ToString());
                                        break;
                                    }
                                case "UpdateFavorites":
                                    {
                                        UpdateSyncLog(filePath, Enums.ServiceMethod.Favourite.ToString());
                                        break;
                                    }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                        }
                        finally
                        {
                            System.Threading.Monitor.Exit(lockObject);
                            if (task != null)
                            {
                                task.Wait();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
        }
        private static void SyncCompletedForSpecificUser(bool Status, String lookupCode)
        {
            try
            {
                if (IsOutlookOpen)
                {
                    if (Status)
                    {
                        CreateXMLForNotification(Constants.ToastNotifications.DataSyncHeaderText, Constants.ToastNotifications.DataSyncCompletedContentText, 1, lookupCode);
                    }
                    else
                    {
                        CreateXMLForNotification(Constants.ToastNotifications.DataSyncHeaderText, Constants.ToastNotifications.DataSyncFailedContentText, 3, lookupCode);
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, lookupCode);
            }
        }
        private static void CreateXMLForNotification(string titleMessage, string contentMessage, Int32 notificationType, string lookupCode)
        {
            try
            {
                var notificationFileName = string.Empty;
                var NotificationCollection = new List<NotificationInfo>();
                NotificationInfo info = new NotificationInfo();
                info.TitleMessage = titleMessage;
                info.ContentMessage = contentMessage;
                info.NotificationType = notificationType;
                info.LookupCode = lookupCode;
                NotificationCollection.Add(info);
                if (!string.IsNullOrEmpty(lookupCode))
                {
                    notificationFileName = string.Format("NotificationInfo - {0}", lookupCode);
                }
                else
                {
                    notificationFileName = "NotificationInfo";
                }
                XMLSerializeHelper.Serialize<NotificationInfo>(NotificationCollection, XMLFolderType.Notification, notificationFileName);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
        }
        private static bool IsOutlookOpen
        {
            get
            {
                return CheckOutlookOpened();
            }
        }
        private static bool CheckOutlookOpened()
        {
            var result = false;
            try
            {
                int procCount = 0;

                Process[] processlist = Process.GetProcessesByName("OUTLOOK").ToArray();//Where(m => m.StartInfo.UserName == CurrentlyLoggedUserName)
                foreach (Process theprocess in processlist)
                {
                    procCount++;
                }
                if (procCount > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
            return result;
        }
        private static string GetMailItemFilePath(string attachmentInfoQuery)
        {
            var attachmentFilePath = "";
            var sqliteDataReaderDataTable = SqliteHelper.ExecuteSelecttQueryWithAdapter(attachmentInfoQuery);//ExecuteSelectQuery(attachmentInfoQuery);
            if (sqliteDataReaderDataTable != null)
            {
                for (int i = 0; i < sqliteDataReaderDataTable.Rows.Count; i++)
                {
                    try
                    {
                        attachmentFilePath = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentFilePath"]);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                    }
                }
            }
            return attachmentFilePath;
        }
        private static Model.AttachmentInfo GetAttachmentInfoDetail(string attachmentInfoQuery)
        {
            var attachmentInfoItem = new Model.AttachmentInfo();
            var sqliteDataReaderDataTable = SqliteHelper.ExecuteSelecttQueryWithAdapter(attachmentInfoQuery);//ExecuteSelectQuery(attachmentInfoQuery);
            if (sqliteDataReaderDataTable != null)
            {
                for (int i = 0; i < sqliteDataReaderDataTable.Rows.Count; i++)
                {
                    try
                    {
                        attachmentInfoItem.AttachmentId = Convert.ToInt32(sqliteDataReaderDataTable.Rows[i]["AttachmentId"]);
                        attachmentInfoItem.ClientId = Convert.ToInt64(sqliteDataReaderDataTable.Rows[i]["ClientId"]);
                        attachmentInfoItem.ActivityId = Convert.ToInt64(sqliteDataReaderDataTable.Rows[i]["ActivityId"]);
                        attachmentInfoItem.FileDetails = new Model.FileInfo();
                        attachmentInfoItem.FileDetails.FileName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["FileName"]);
                        attachmentInfoItem.FileDetails.FilePath = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentFilePath"]);
                        attachmentInfoItem.FileDetails.FileContentMemStream = GetFileBytes(attachmentInfoItem.FileDetails.FilePath);
                        attachmentInfoItem.FileDetails.FileExtension = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["FileExtension"]);
                        attachmentInfoItem.PolicyType = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["PolicyType"]);
                        attachmentInfoItem.FolderDetails = new Model.FolderInfo();
                        attachmentInfoItem.IsEmail = true;
                        attachmentInfoItem.FolderDetails.ParentFolderId = (long)sqliteDataReaderDataTable.Rows[i]["ParentFolderId"];
                        attachmentInfoItem.FolderDetails.ParentFolderName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["ParentFolderName"]);
                        attachmentInfoItem.FolderDetails.FolderId = (long)sqliteDataReaderDataTable.Rows[i]["FolderId"];
                        attachmentInfoItem.FolderDetails.FolderName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["FolderName"]);
                        attachmentInfoItem.FolderDetails.SubFolderId = (long)sqliteDataReaderDataTable.Rows[i]["SubFolderId"];
                        attachmentInfoItem.FolderDetails.SubFolderName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["SubFolderName"]);
                        attachmentInfoItem.PolicyCode = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["PolicyCode"]);
                        attachmentInfoItem.PolicyYear = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["PolicyYear"]);
                        attachmentInfoItem.ClientAccessible = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["ClientAccessible"]);
                        attachmentInfoItem.ReceivedDate = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["ReceivedDate"]);
                        attachmentInfoItem.EmailFromAddress = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailFromAddress"]);
                        attachmentInfoItem.EmailToAddress = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailToAddress"]);
                        attachmentInfoItem.Subject = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["Subject"]);
                        attachmentInfoItem.AttachmentFilePath = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentFilePath"]);
                        attachmentInfoItem.IsPushedToEpic = Convert.ToBoolean(sqliteDataReaderDataTable.Rows[i]["IsPushedToEpic"]);
                        attachmentInfoItem.IsAttachDelete = Convert.ToBoolean(sqliteDataReaderDataTable.Rows[i]["IsAttachDelete"]);
                        attachmentInfoItem.DomainName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["DomainName"]);
                        attachmentInfoItem.UserName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["UserName"]); // "Photon1";// Environment.UserName;
                        attachmentInfoItem.EmployeeCode = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmployeeCode"]);
                        attachmentInfoItem.MailBody = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentMailBody"]);
                        attachmentInfoItem.ActivityGuid = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["ActivityGuid"]);
                        attachmentInfoItem.EmailFromDisplayName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailFromDisplayName"]);
                        attachmentInfoItem.EmailToDisplayName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailToDisplayName"]);
                        attachmentInfoItem.EmailCCAddress = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailCCAddress"]);
                        attachmentInfoItem.EmailCCDisplayName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailCCDisplayName"]);
                        attachmentInfoItem.IsEpicPushInProgress = Convert.ToBoolean(sqliteDataReaderDataTable.Rows[i]["IsEpicPushInProgress"]);
                        attachmentInfoItem.AttachmentIdentifier = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentIdentifier"]);
                        var policyYear = string.Equals(attachmentInfoItem.PolicyYear, "(none)", StringComparison.InvariantCultureIgnoreCase) ? string.Empty : attachmentInfoItem.PolicyYear;
                        var policyCode = string.Equals(attachmentInfoItem.PolicyCode, "(none)", StringComparison.InvariantCultureIgnoreCase) ? string.Empty : attachmentInfoItem.PolicyCode;
                        attachmentInfoItem.Description = $"{policyYear} {policyCode} {Convert.ToString(sqliteDataReaderDataTable.Rows[i]["Description"])}";
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmployeeCode"]));
                    }
                }
            }
            return attachmentInfoItem;
        }
        private static byte[] GetFileBytes(string filename)
        {
            var fileBytes = new byte[] { };
            try
            {
                fileBytes = File.ReadAllBytes(filename);
            }

            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, "");

            }
            finally
            {
                Logger.save();
            }
            return fileBytes;
        }
        public static void UpdateSyncLogUserTable(string userLookupCode, Enums.ServiceMethod FieldName)
        {
            try
            {
                using (var context = new HIB.Outlook.SQLite.HIBOutlookEntities())
                {
                    var fildeName = FieldName.ToString();
                    var sync = context.HIBOPSyncLogs.FirstOrDefault(x => x.Fields == fildeName);
                    var syncUser = context.HIBOPSyncLogUserDetails.FirstOrDefault(x => x.SyncLogId == sync.SLId && x.EmployeeLookupcode == userLookupCode);
                    if (syncUser == null)
                    {
                        context.HIBOPSyncLogUserDetails.Add(new HIBOPSyncLogUserDetail
                        {
                            SyncLogId = sync.SLId,
                            EmployeeLookupcode = userLookupCode,
                            SyncDate = DateTime.Now,
                            SyncLogField = sync.Fields
                        });
                    }
                    else
                    {
                        syncUser.SyncDate = DateTime.Now;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }

        }

    }
}
