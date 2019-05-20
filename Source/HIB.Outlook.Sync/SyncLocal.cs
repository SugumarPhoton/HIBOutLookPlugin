using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq;
using System.Configuration;
using HIB.Outlook.Model;
using HIB.Outlook.SQLite.Repository;
using HIB.Outlook.SQLite.Repository.IRepository;
using HIB.Outlook.Sync.Common;
using HIB.Outlook.SQLite;
using HIB.Outlook.Helper.Common;
using System.Diagnostics;
using HIB.Outlook.Model.Activities;
using System.IO;
using System.Globalization;
using HIB.Outlook.Helper.Helper;
using System.Threading;

namespace HIB.Outlook.Sync
{

    public class SyncLocal
    {

        IClientRepository _clientRepository;
        ILogRepository _logRepository;
        IActivityRepository _activityRepository;
        IFolderRepository _folderRepository;
        IPolicyLineTypeRepository _policyLineTypeRepository;
        IFavouriteRepository _favouriteRepository;
        private string ServiceURL;

        public delegate void SyncCompletedDelegate(bool status, bool isValidUser);

        public event SyncCompletedDelegate SyncCompleted;

        public delegate void SyncCompletedForClientDelegate(bool status, bool isValidUser);

        public event SyncCompletedForClientDelegate SyncCompletedForClient;
        public SyncLocal()
        {

            _favouriteRepository = new FavouriteRepository();
            _clientRepository = new ClientRepository();
            _logRepository = new LogRepository();
            _activityRepository = new ActivityRepository();
            _folderRepository = new FolderRepository();
            _policyLineTypeRepository = new PolicyLineTypeRepository();
            ServiceURL = ConfigurationManager.AppSettings["ServiceURL"];
        }

        private bool IsOutlookOpen
        {
            get
            {
                return CheckOutlookOpened();
            }
        }
        private bool CheckOutlookOpened()
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

        private void CreateXMLForNotification(string titleMessage, string contentMessage, Int32 notificationType, string lookupCode)
        {
            try
            {
                var notificationFileName = string.Empty;
                var NotificationCollection = new List<NotificationInfo>();
                NotificationInfo info = new NotificationInfo
                {
                    TitleMessage = titleMessage,
                    ContentMessage = contentMessage,
                    NotificationType = notificationType,
                    LookupCode = lookupCode
                };
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

        public List<string> GetValidEmployees()
        {
            List<string> ValidlookUpCodes = new List<string>();
            try
            {
                string domain = ConfigurationManager.AppSettings["Domains"];
                string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
                List<string> userList = CommonHelper.GetUserandDomains(domain);
                string serviceMethodURL = string.Empty;
                List<string> users = new List<string>();
                foreach (var userDomain in userList)
                {
                    string[] UserandDomain = userDomain.Split('-');
                    string userName = UserandDomain[0];
                    string domainName = UserandDomain[1];
                    string UserNameWithDomain = string.Format(@"{0}\{1}", domainName, userName);
                    users.Add(UserNameWithDomain);
                }
                List<string> lookUpCodes = CommonHelper.GetLookUpCodeByUser(users);
                if (lookUpCodes != null && lookUpCodes.Any())
                {
                    lookUpCodes = lookUpCodes.Distinct().ToList();
                    List<SyncLog> syncLog = _logRepository.GetSyncLog();
                    if (syncLog != null && syncLog.Any())
                    {
                        var syncParam = new SyncParams
                        {
                            UserId = string.Join(",", lookUpCodes.ToArray()),
                            LastSyncDate = (from s in syncLog where s.Fields == Enums.ServiceMethod.ActivityEmployee.ToString() select s.SyncDate).FirstOrDefault()?.ConvertToUTC()
                        };
                        serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityEmployee);
                        List<EmployeeInfo> activityEmployeeInfoCollection = SyncActivityEmployee(serviceMethodURL, syncParam, lookUpCodes);
                        List<string> inValidUsersList = activityEmployeeInfoCollection.Where(m => m.Status == 0).Select(m => m.LookupCode).ToList();

                        foreach (var lookupCode in inValidUsersList)
                        {
                            CreateXMLForNotification(Constants.ToastNotifications.PermissionHeaderText, Constants.ToastNotifications.PermissionContentText, 2, lookupCode);
                        }
                        if (activityEmployeeInfoCollection.Where(m => m.Status == 1 && m.IsAdmin == 0).Any())
                        {
                            List<string> lookUpCodeList = activityEmployeeInfoCollection.Where(m => m.Status == 1 && m.IsAdmin == 0).Select(m => m.LookupCode).ToList();
                            if (lookUpCodeList != null && lookUpCodeList.Any())
                            {
                                foreach (var lookupcode in lookUpCodeList)
                                {
                                    ValidlookUpCodes.Add(lookupcode);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            return ValidlookUpCodes;
        }

        public bool SyncOnlyClient(List<string> ValidEmployees, List<DeltaSyncObjectInfo> deltaSyncObjects)
        {
            string serviceMethodURL = string.Empty;
            var syncParam = new SyncParams();
            string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
            bool isValidUser = false;
            try
            {
                List<SyncLog> syncLog = _logRepository.GetSyncLog();
                foreach (var lookUpCode in ValidEmployees)
                {
                    if (IsOutlookOpen)
                    {
                        CreateXMLForNotification(Constants.ToastNotifications.DataSyncHeaderText, Constants.ToastNotifications.DataSyncStartedContentText, 0, lookUpCode);
                    }

                    DateTime lastlogSyncDate = DateTime.MinValue;
                    syncParam = new SyncParams { UserId = lookUpCode };
                    syncParam.IPAddress = CommonProperties.LocalIPAddress();

                    var ClientObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetClientDetails_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                    if (ClientObject != null && (bool)ClientObject.IsDeltaFlag)
                    {
                        syncParam.LastSyncDate = ClientObject.LastSyncDate?.ConvertDateTimeFormat();
                        serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.Client);
                        SyncClients(serviceMethodURL, syncParam);
                    }

                    var ClientEmployeeObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetClientEmployee_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                    if (ClientEmployeeObject != null && (bool)ClientEmployeeObject.IsDeltaFlag)
                    {
                        syncParam.LastSyncDate = ClientEmployeeObject.LastSyncDate?.ConvertDateTimeFormat();
                        serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ClientEmployee);
                        SyncClientEmployee(serviceMethodURL, syncParam);
                    }
                }

                SyncCompletedForClient?.Invoke(true, isValidUser);

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
            return isValidUser;
        }

        ///<summary>
        ///Sync data from centralized to local
        /// </summary>
        /// <returns></returns>
        ///     
        public void SyncData(List<string> ValidEmployees, List<DeltaSyncObjectInfo> deltaSyncObjects)
        {
            string serviceMethodURL = string.Empty;
            var syncParam = new SyncParams();
            string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
            try
            {
                List<SyncLog> syncLog = _logRepository.GetSyncLog();
                DateTime lastlogSyncDate = DateTime.MinValue;

                try
                {
                    lastlogSyncDate = Convert.ToDateTime((from s in syncLog where s.Fields == Enums.ServiceMethod.Favourite.ToString() select s.SyncDate)?.FirstOrDefault());

                    serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.Favourite);
                    GetFavouriteDetails(serviceMethodURL, lastlogSyncDate);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, ex.StackTrace);
                }

                foreach (var lookUpCode in ValidEmployees)
                {
                    try
                    {
                        // Logger.InfoLog("Sync Started for " + lookUpCode, typeof(SyncLocal), Logger.SourceType.WindowsService, "");
                        //if (IsOutlookOpen)
                        //{
                        //    CreateXMLForNotification(Constants.ToastNotifications.DataSyncHeaderText, Constants.ToastNotifications.DataSyncStartedContentText, 0, lookUpCode);
                        //}
                        if (!string.IsNullOrEmpty(lookUpCode))
                        {
                            syncParam = new SyncParams { UserId = lookUpCode };
                            syncParam.IPAddress = CommonProperties.LocalIPAddress();

                            //syncParam.LastSyncDate = (from s in syncLog where s.Fields == Enums.ServiceMethod.Client.ToString() && s.UserName == lookUpCode select s.UserSyncDate).FirstOrDefault()?.ConvertToUTC();
                            //serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.Client);
                            //SyncClients(serviceMethodURL, syncParam);

                            //serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ClientEmployee);
                            //SyncClientEmployee(serviceMethodURL, syncParam);
                            var folderObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetFolders_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (folderObject != null && (bool)folderObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = folderObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.Folder);
                                SyncFolders(serviceMethodURL, syncParam);
                            }

                            var PolicyLineTypeObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetPolicyLineType_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (PolicyLineTypeObject != null && (bool)PolicyLineTypeObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = PolicyLineTypeObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.PolicyLineType);
                                SyncPolicyLineTypes(serviceMethodURL, syncParam);
                            }

                            var ActivityCommonLookUpObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetCommonLookUp_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityCommonLookUpObject != null && (bool)ActivityCommonLookUpObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityCommonLookUpObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityCommonLookUp);
                                SyncActivityCommonLookUp(serviceMethodURL, syncParam);
                            }

                            var ActivityOwnerListObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityOwnerList_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityOwnerListObject != null && (bool)ActivityOwnerListObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityOwnerListObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityOwnerList);
                                SyncActivityOwnerList(serviceMethodURL, syncParam);
                            }

                            var ActivityObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityDetails_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityObject != null && (bool)ActivityObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.Activity);
                                SyncActivities(serviceMethodURL, syncParam);
                            }

                            var ActivityEmployeeObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityEmployee_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityEmployeeObject != null && (bool)ActivityEmployeeObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityEmployeeObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityEmployees);
                                SyncActivityEmployees(serviceMethodURL, syncParam);
                            }

                            var ActivityClaimObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityClaim_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityClaimObject != null && (bool)ActivityClaimObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityClaimObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityClaim);
                                SyncActivityClaims(serviceMethodURL, syncParam);
                            }

                            var ActivityServiceObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityServices_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityServiceObject != null && (bool)ActivityServiceObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityServiceObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityService);
                                SyncActivityService(serviceMethodURL, syncParam);
                            }

                            var ActivityLineObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityLine_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityLineObject != null && (bool)ActivityLineObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityLineObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityLine);
                                SyncActivityLines(serviceMethodURL, syncParam);
                            }


                            var ActivityOpportunityObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityOpportunity_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityOpportunityObject != null && (bool)ActivityOpportunityObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityOpportunityObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityOpportunity);
                                SyncActivityOpportunities(serviceMethodURL, syncParam);
                            }


                            var ActivityAccountObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityAccount_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityAccountObject != null && (bool)ActivityAccountObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityAccountObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityAccount);
                                SyncActivityAccounts(serviceMethodURL, syncParam);
                            }

                            var ActivityMarketingObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityMarketing_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityMarketingObject != null && (bool)ActivityMarketingObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityMarketingObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityMarketing);
                                SyncActivityMarketing(serviceMethodURL, syncParam);
                            }


                            var ActivityClientContactObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityClientContacts_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityClientContactObject != null && (bool)ActivityClientContactObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityClientContactObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityClientContact);
                                SyncActivityClientContacts(serviceMethodURL, syncParam);
                            }


                            var ActivityListObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityList_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityListObject != null && (bool)ActivityListObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityListObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityList);
                                SyncActivityList(serviceMethodURL, syncParam);
                            }


                            var ActivityBillObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityBill_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityBillObject != null && (bool)ActivityBillObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityBillObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityBill);
                                SyncActivityBill(serviceMethodURL, syncParam);
                            }


                            var ActivityCarrierSubmissionObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityCarrierSubmission_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityCarrierSubmissionObject != null && (bool)ActivityCarrierSubmissionObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityCarrierSubmissionObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityCarrierSubmission);
                                SyncActivityCarrier(serviceMethodURL, syncParam);
                            }


                            var ActivityTransactionObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityTransaction_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityTransactionObject != null && (bool)ActivityTransactionObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityTransactionObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityTransaction);
                                SyncActivityTransaction(serviceMethodURL, syncParam);
                            }


                            var ActivityCertificateObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityCertificate_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityCertificateObject != null && (bool)ActivityCertificateObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityCertificateObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityCertificate);
                                SyncActivityCertificate(serviceMethodURL, syncParam);
                            }


                            var ActivityEvidenceObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityEvidence_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityEvidenceObject != null && (bool)ActivityEvidenceObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityEvidenceObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityEvidence);
                                SyncActivityEvidence(serviceMethodURL, syncParam);
                            }

                            var ActivityEmployeeAgencyObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetEmployeeAgency_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityEmployeeAgencyObject != null && (bool)ActivityEmployeeAgencyObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityEmployeeAgencyObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityEmployeeAgency);
                                SyncActivityEmployeeAgency(serviceMethodURL, syncParam);
                            }

                            var ActivityPolicyObject = deltaSyncObjects.Where(m => m.SpName == Enums.StoredProcedures.HIBOPGetActivityPolicy_SP.ToString() && m.UserLookupCode == lookUpCode)?.FirstOrDefault();
                            if (ActivityPolicyObject != null && (bool)ActivityPolicyObject.IsDeltaFlag)
                            {
                                syncParam.LastSyncDate = ActivityPolicyObject.LastSyncDate?.ConvertDateTimeFormat();
                                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ActivityPolicy);
                                SyncActivityPolicies(serviceMethodURL, syncParam);
                            }
                            else
                            {
                                SyncCompletedForSpecificUser(true, lookUpCode);
                            }
                            UpdateSyncStatusInEmployeeTable(lookUpCode);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex.StackTrace, Logger.SourceType.WindowsService, lookUpCode);
                        SyncCompletedForSpecificUser(false, lookUpCode);
                    }

                }


                lastlogSyncDate = Convert.ToDateTime((from s in syncLog where s.Fields == Enums.ServiceMethod.AuditLog.ToString() select s.SyncDate)?.FirstOrDefault());
                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.AuditLog);
                GetAuditLogDetails(serviceMethodURL, lastlogSyncDate, syncParam.UserId);

                lastlogSyncDate = Convert.ToDateTime((from s in syncLog where s.Fields == Enums.ServiceMethod.ErrorLog.ToString() select s.SyncDate)?.FirstOrDefault());
                serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.ErrorLog);
                GetErrorLogDetails(serviceMethodURL, lastlogSyncDate, syncParam.UserId);

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, ex.StackTrace);
            }
            finally
            {
                Logger.save();
                SyncCompleted?.Invoke(true, true);
            }

        }

        public List<DeltaSyncObjectInfo> GetDeltaSyncObjects(List<string> ValidEmployees, bool? isClient, bool? isFirstSync)
        {
            List<DeltaSyncObjectInfo> deltaSyncObjectInfos = new List<DeltaSyncObjectInfo>();
            try
            {
                var ipAddress = CommonProperties.LocalIPAddress();
                foreach (var lookupCode in ValidEmployees)
                {
                    var syncParam = new SyncParams { UserId = lookupCode };
                    syncParam.IPAddress = ipAddress;
                    syncParam.IsClient = isClient;
                    syncParam.isFirstSync = isFirstSync == true ? isFirstSync : IsFirstSyncDiscoveredWithLocalDB(lookupCode);
                    string serviceURL = ConfigurationManager.AppSettings["ServiceURL"];
                    var serviceMethodURL = serviceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.SyncObjects);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(syncParam), Encoding.UTF8, "application/json");
                    string syncObjectsResponse = GetWebAPIResponse(serviceMethodURL, content);
                    if (!string.IsNullOrEmpty(syncObjectsResponse))
                    {
                        List<DeltaSyncObjectInfo> deltaSyncObjects = JsonConvert.DeserializeObject<List<DeltaSyncObjectInfo>>(syncObjectsResponse);
                        if (deltaSyncObjects != null && deltaSyncObjects.Any())
                        {
                            deltaSyncObjectInfos.AddRange(deltaSyncObjects);
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
            return deltaSyncObjectInfos;
        }

        public bool IsFirstSyncDiscoveredWithLocalDB(string lookupCode)
        {
            var result = false;
            try
            {
                List<SyncLog> syncLog = _logRepository.GetSyncLog();
                var ClientObjectSyncDate = (from s in syncLog where s.Fields == Enums.ServiceMethod.Client.ToString() && s.UserName == lookupCode select s.UserSyncDate).FirstOrDefault()?.ConvertToUTC();
                var ClientEmployeeObjectSyncDate = (from s in syncLog where s.Fields == Enums.ServiceMethod.ClientEmployee.ToString() && s.UserName == lookupCode select s.UserSyncDate).FirstOrDefault()?.ConvertToUTC();
                var ActivityObjectSyncDate = (from s in syncLog where s.Fields == Enums.ServiceMethod.Activity.ToString() && s.UserName == lookupCode select s.UserSyncDate).FirstOrDefault()?.ConvertToUTC();
                var ActivityEmployeesSyncDate = (from s in syncLog where s.Fields == Enums.ServiceMethod.ActivityEmployees.ToString() && s.UserName == lookupCode select s.UserSyncDate).FirstOrDefault()?.ConvertToUTC();
                var ActivityClaimSyncDate = (from s in syncLog where s.Fields == Enums.ServiceMethod.ActivityClaim.ToString() && s.UserName == lookupCode select s.UserSyncDate).FirstOrDefault()?.ConvertToUTC();
                var ActivityServiceSyncDate = (from s in syncLog where s.Fields == Enums.ServiceMethod.ActivityService.ToString() && s.UserName == lookupCode select s.UserSyncDate).FirstOrDefault()?.ConvertToUTC();
                var ActivityLineSyncDate = (from s in syncLog where s.Fields == Enums.ServiceMethod.ActivityLine.ToString() && s.UserName == lookupCode select s.UserSyncDate).FirstOrDefault()?.ConvertToUTC();
                var ActivityOpportunitySyncDate = (from s in syncLog where s.Fields == Enums.ServiceMethod.ActivityOpportunity.ToString() && s.UserName == lookupCode select s.UserSyncDate).FirstOrDefault()?.ConvertToUTC();

                if (ClientObjectSyncDate == null && ClientEmployeeObjectSyncDate == null && ActivityObjectSyncDate == null && ActivityEmployeesSyncDate == null && ActivityClaimSyncDate == null && ActivityServiceSyncDate == null && ActivityLineSyncDate == null && ActivityOpportunitySyncDate == null)
                    result = true;
                else
                    result = false;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            return result;
        }

        private void SyncCompletedForSpecificUser(bool Status, String lookupCode)
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
        private void UpdateSyncStatusInEmployeeTable(string employeeLookupCode)
        {
            try
            {
                using (var context = new HIB.Outlook.SQLite.HIBOutlookEntities())
                {
                    var result = context.HIBOPEmployees.Where(d => d.LookupCode == employeeLookupCode)?.FirstOrDefault();
                    if (result.SyncStatusNotified == 0)
                    {
                        result.SyncStatusNotified = 1;
                        context.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, ex.StackTrace.ToString());
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        ///Get error log details
        /// </summary>
        /// <returns></returns>
        public bool GetErrorLogDetails(string errorLogURL, DateTime lastSyncDate, string lookUpCode)
        {
            try
            {

                List<ErrorLogInfo> errorLogList = _logRepository.GetErrorLogDetails(lastSyncDate, lookUpCode);
                if (errorLogList != null && errorLogList.Any())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(errorLogList), Encoding.UTF8, "application/json");
                    Task<int> postResponse = PostWebAPIResponse(errorLogURL, content);
                    postResponse.Wait();
                    if (postResponse.Result == 1)
                    {
                        XMLSerializeHelper.Serialize<string>(new List<string>() { "ErrorLog" }, XMLFolderType.Service, "UpdateErrorLog");

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, lookUpCode);
                return false;
            }
            finally
            {
                Logger.save();
            }


        }

        ///<summary>
        ///Get list of Audit log details
        /// </summary>
        /// <returns></returns>
        public bool GetAuditLogDetails(string auditLogURL, DateTime lastAuditSyncDate, string lookUpCode)
        {
            try
            {

                List<LogInfo> auditLogList = _logRepository.GetAuditLogDetails(lastAuditSyncDate);

                if (auditLogList != null && auditLogList.Any())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(auditLogList), Encoding.UTF8, "application/json");
                    Task<int> postResponse = PostWebAPIResponse(auditLogURL, content);
                    postResponse.Wait();
                    if (postResponse.Result == 1)
                    {
                        XMLSerializeHelper.Serialize<string>(new List<string>() { "AuditLog" }, XMLFolderType.Service, "UpdateAuditLog");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, lookUpCode);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }

        ///<summary>
        ///Get list of Favourite Details
        /// </summary>
        /// <returns></returns>

        public bool GetFavouriteDetails(string favouriteURL, DateTime lastAuditSyncDate)
        {
            try
            {
                List<FavouriteInfo> favourateList = _favouriteRepository.GetFavouriteDetails(lastAuditSyncDate);
                if (favourateList != null && favourateList.Any())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(favourateList), Encoding.UTF8, "application/json");
                    Task<int> postResponse = PostWebAPIResponse(favouriteURL, content);
                    postResponse.Wait();
                    if (postResponse.Result == 1)
                    {
                        XMLSerializeHelper.Serialize<string>(new List<string>(), XMLFolderType.Service, "UpdateFavorites");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                return false;
            }
            finally
            {
                Logger.save();
            }

        }

        /// <summary>
        /// Get List of user client detail to sync local Database
        /// </summary>
        /// <param name="clientURL"></param>
        /// <returns></returns>
        public bool SyncClients(string clientURL, SyncParams syncparam)
        {
            try
            {
                int activityCount = Convert.ToInt32(ConfigurationManager.AppSettings["ClientCount"]);
                syncparam.PageNumber = 1;
                syncparam.RowsPerPage = activityCount;

                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string clientResponse = GetWebAPIResponse(clientURL, content);
                if (!string.IsNullOrEmpty(clientResponse))
                {
                    ClientDetail clientDetail = JsonConvert.DeserializeObject<ClientDetail>(clientResponse);
                    if (clientDetail != null)
                    {
                        List<ClientInfo> clientList = clientDetail.Clients;
                        long totalCount = clientDetail.RowCount;

                        if (clientList != null && clientList.Count >= activityCount)
                        {
                            clientList[0].UserLookupCode = syncparam.UserId;
                            XMLSerializeHelper.Serialize(clientList, XMLFolderType.Service);

                            long activityPageNumber = totalCount / activityCount;
                            activityPageNumber = activityPageNumber + 1;
                            for (int i = 2; i <= activityPageNumber; i++)
                            {
                                try
                                {
                                    syncparam.PageNumber = i;
                                    syncparam.RowsPerPage = activityCount;
                                    content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                                    clientResponse = GetWebAPIResponse(clientURL, content);
                                    if (!string.IsNullOrEmpty(clientResponse))
                                    {
                                        clientDetail = JsonConvert.DeserializeObject<ClientDetail>(clientResponse);
                                        if (clientDetail != null)
                                        {
                                            clientList = clientDetail?.Clients;
                                            if (clientList != null && clientList.Any())
                                            {
                                                clientList[0].UserLookupCode = syncparam.UserId;
                                                XMLSerializeHelper.Serialize(clientList, XMLFolderType.Service);
                                            }
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex.StackTrace, Logger.SourceType.WindowsService, syncparam.UserId);
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                                }

                            }
                        }
                        else
                        {
                            if (clientList != null && clientList.Any())
                            {
                                clientList[0].UserLookupCode = syncparam.UserId;
                                XMLSerializeHelper.Serialize(clientList, XMLFolderType.Service);
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.StackTrace, Logger.SourceType.WindowsService, syncparam.UserId);
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }


        /// <summary>
        /// Get List of user activity detail to sync local Database
        /// </summary>
        /// <param name="activityURL"></param>
        /// <returns></returns>
        public bool SyncActivities(string activityURL, SyncParams syncParams)
        {
            try
            {

                int activityCount = Convert.ToInt32(ConfigurationManager.AppSettings["ActivityCount"]);
                syncParams.PageNumber = 1;
                syncParams.RowsPerPage = activityCount;

                StringContent content = new StringContent(JsonConvert.SerializeObject(syncParams), Encoding.UTF8, "application/json");
                string activityResponse = GetWebAPIResponse(activityURL, content);
                if (!string.IsNullOrEmpty(activityResponse))
                {

                    ActivityDetail activityDetail = JsonConvert.DeserializeObject<ActivityDetail>(activityResponse);
                    List<ActivityInfo> activityList = activityDetail.Activity;
                    long totalCount = activityDetail.RowCount;

                    if (activityList != null && activityList.Count >= activityCount)
                    {
                        activityList[0].UserLookupCode = syncParams.UserId;
                        XMLSerializeHelper.Serialize(activityList, XMLFolderType.Service);
                        long activityPageNumber = totalCount / activityCount;
                        activityPageNumber = activityPageNumber + 1;
                        for (int i = 2; i <= activityPageNumber; i++)
                        {
                            syncParams.PageNumber = i;
                            syncParams.RowsPerPage = activityCount;
                            content = new StringContent(JsonConvert.SerializeObject(syncParams), Encoding.UTF8, "application/json");
                            activityResponse = GetWebAPIResponse(activityURL, content);
                            if (!string.IsNullOrEmpty(activityResponse))
                            {
                                activityDetail = JsonConvert.DeserializeObject<ActivityDetail>(activityResponse);
                                activityList = activityDetail.Activity;
                                if (activityList != null && activityList.Any())
                                {
                                    activityList[0].UserLookupCode = syncParams.UserId;
                                    XMLSerializeHelper.Serialize(activityList, XMLFolderType.Service);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (activityList != null && activityList.Any())
                        {
                            activityList[0].UserLookupCode = syncParams.UserId;
                            XMLSerializeHelper.Serialize(activityList, XMLFolderType.Service);
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncParams.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }


        /// <summary>
        /// Get List of user policy detail to sync local Database
        /// </summary>
        /// <param name="policyURL"></param>
        /// <returns></returns>
        public bool SyncPolicyLineTypes(string policyURL, SyncParams syncParams)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncParams), Encoding.UTF8, "application/json");
                string policyLineTypeResponse = GetWebAPIResponse(policyURL, content);
                if (!string.IsNullOrEmpty(policyLineTypeResponse))
                {
                    List<PolicyTypeInfo> policyList = JsonConvert.DeserializeObject<List<PolicyTypeInfo>>(policyLineTypeResponse);
                    if (policyList != null && policyList.Any())
                    {
                        XMLSerializeHelper.Serialize<PolicyTypeInfo>(policyList, XMLFolderType.Service);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncParams.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }

        /// <summary>
        /// Get List of user folder detail to sync local Database
        /// </summary>
        /// <param name="folderURL"></param>
        /// <returns></returns>
        public bool SyncFolders(string folderURL, SyncParams syncParams)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncParams), Encoding.UTF8, "application/json");
                string folderResponse = GetWebAPIResponse(folderURL, content);
                if (!string.IsNullOrEmpty(folderResponse))
                {
                    List<FolderInfo> folderList = JsonConvert.DeserializeObject<List<FolderInfo>>(folderResponse);

                    if (folderList != null && folderList.Any())
                    {

                        XMLSerializeHelper.Serialize<FolderInfo>(folderList, XMLFolderType.Service);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncParams.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }
        /// <summary>
        /// Get List of activity claim details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityClaims(string activityClaimURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityClaimResponse = GetWebAPIResponse(activityClaimURL, content);
                if (!string.IsNullOrEmpty(activityClaimResponse))
                {
                    List<ActivityClaimInfo> activityClaimList = JsonConvert.DeserializeObject<List<ActivityClaimInfo>>(activityClaimResponse);

                    if (activityClaimList != null && activityClaimList.Any())
                    {
                        activityClaimList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize<ActivityClaimInfo>(activityClaimList, XMLFolderType.Service);
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }
        }



        /// <summary>
        /// Get List of activity claim details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityPolicies(string activityPolicyURL, SyncParams syncparam)
        {
            try
            {
                int activityPolicyCount = Convert.ToInt32(ConfigurationManager.AppSettings["ActivityCount"]);
                syncparam.PageNumber = 1;
                syncparam.RowsPerPage = activityPolicyCount;

                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityPolicyResponse = GetWebAPIResponse(activityPolicyURL, content);
                if (!string.IsNullOrEmpty(activityPolicyResponse))
                {
                    ActivityPolicyDetail activityPolicyDetail = JsonConvert.DeserializeObject<ActivityPolicyDetail>(activityPolicyResponse);
                    List<ActivityPolicyInfo> activityPolicyList = activityPolicyDetail.ActivityPolicies;
                    long totalCount = activityPolicyDetail.RowCount;
                    if (activityPolicyList != null && activityPolicyList.Count >= activityPolicyCount)
                    {
                        activityPolicyList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize<ActivityPolicyInfo>(activityPolicyList, XMLFolderType.Service);
                        long activityPolicyPageNumber = totalCount / activityPolicyCount;
                        activityPolicyPageNumber = activityPolicyPageNumber + 1;
                        for (int i = 2; i <= activityPolicyPageNumber; i++)
                        {
                            syncparam.PageNumber = i;
                            syncparam.RowsPerPage = activityPolicyCount;
                            content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                            activityPolicyResponse = GetWebAPIResponse(activityPolicyURL, content);
                            if (!string.IsNullOrEmpty(activityPolicyResponse))
                            {
                                activityPolicyDetail = JsonConvert.DeserializeObject<ActivityPolicyDetail>(activityPolicyResponse);
                                activityPolicyList = activityPolicyDetail.ActivityPolicies;
                                if (activityPolicyList != null && activityPolicyList.Any())
                                {
                                    activityPolicyList[0].UserLookupCode = syncparam.UserId;
                                    XMLSerializeHelper.Serialize(activityPolicyList, XMLFolderType.Service);
                                }
                            }
                        }
                    }
                    else if (activityPolicyList != null && activityPolicyList.Any())
                    {
                        activityPolicyList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize<ActivityPolicyInfo>(activityPolicyList, XMLFolderType.Service);
                    }
                    else
                    {
                        XmlToSqliteHandler.UpdateSyncLogUserTable(syncparam.UserId, Enums.ServiceMethod.ActivityPolicy);
                        SyncCompletedForSpecificUser(true, syncparam.UserId);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                SyncCompletedForSpecificUser(true, syncparam.UserId);
                Logger.save();
            }
            return true;
        }





        /// <summary>
        /// Get List of activity service details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityService(string activityServiceURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityServiceResponse = GetWebAPIResponse(activityServiceURL, content);
                if (!string.IsNullOrEmpty(activityServiceResponse))
                {
                    List<ActivityServiceInfo> activityServiceList = JsonConvert.DeserializeObject<List<ActivityServiceInfo>>(activityServiceResponse);
                    if (activityServiceList != null && activityServiceList.Any())
                    {
                        activityServiceList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityServiceList, XMLFolderType.Service);
                    }
                    else
                    {
                        XmlToSqliteHandler.UpdateSyncLogUserTable(syncparam.UserId, Enums.ServiceMethod.ActivityService);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }



        /// <summary>
        /// Get List of activity line details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityLines(string activityLineURL, SyncParams syncparam)
        {
            try
            {

                int activityLineCount = Convert.ToInt32(ConfigurationManager.AppSettings["LineCount"]);
                syncparam.PageNumber = 1;
                syncparam.RowsPerPage = activityLineCount;

                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityLineResponse = GetWebAPIResponse(activityLineURL, content);

                if (!string.IsNullOrEmpty(activityLineResponse))
                {
                    ActivityLineDetail activityLineDetail = JsonConvert.DeserializeObject<ActivityLineDetail>(activityLineResponse);
                    List<ActivityLineInfo> activityLineList = activityLineDetail?.ActivityLines;
                    long totalCount = activityLineDetail.RowCount;
                    if (activityLineDetail != null && activityLineList != null && activityLineList.Count >= activityLineCount)
                    {
                        activityLineList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityLineList, XMLFolderType.Service);
                        long activityPageNumber = totalCount / activityLineCount;
                        activityPageNumber = activityPageNumber + 1;
                        for (int i = 2; i <= activityPageNumber; i++)
                        {

                            try
                            {
                                syncparam.PageNumber = i;
                                syncparam.RowsPerPage = activityLineCount;
                                content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                                activityLineResponse = GetWebAPIResponse(activityLineURL, content);
                                if (!string.IsNullOrEmpty(activityLineResponse))
                                {
                                    activityLineDetail = JsonConvert.DeserializeObject<ActivityLineDetail>(activityLineResponse);
                                    activityLineList = activityLineDetail.ActivityLines;
                                    if (activityLineList != null && activityLineList.Any())
                                    {
                                        activityLineList[0].UserLookupCode = syncparam.UserId;
                                        XMLSerializeHelper.Serialize(activityLineList, XMLFolderType.Service);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                            }
                        }

                    }
                    else if (activityLineList != null && activityLineList.Any())
                    {
                        activityLineList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityLineList, XMLFolderType.Service);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }






        /// <summary>
        /// Get List of activity line details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityOpportunities(string activityOpportunityURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityOpportunityResponse = GetWebAPIResponse(activityOpportunityURL, content);
                if (!string.IsNullOrEmpty(activityOpportunityResponse))
                {
                    List<ActivityOpportunityInfo> activityOpportunityList = JsonConvert.DeserializeObject<List<ActivityOpportunityInfo>>(activityOpportunityResponse);
                    if (activityOpportunityList != null && activityOpportunityList.Any())
                    {
                        activityOpportunityList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityOpportunityList, XMLFolderType.Service);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }
        }


        /// <summary>
        /// Get List of activity line details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityAccounts(string activityAccountURL, SyncParams syncparam)
        {
            try
            {
                int activityAccountCount = Convert.ToInt32(ConfigurationManager.AppSettings["ActivityCount"]);
                syncparam.PageNumber = 1;
                syncparam.RowsPerPage = activityAccountCount;

                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityAccountResponse = GetWebAPIResponse(activityAccountURL, content);
                if (!string.IsNullOrEmpty(activityAccountResponse))
                {
                    ActivityAccountDetail activityAccountDetail = JsonConvert.DeserializeObject<ActivityAccountDetail>(activityAccountResponse);
                    List<ActivityAccountInfo> activityAccountList = activityAccountDetail.ActivityAccounts;
                    long totalCount = activityAccountDetail.RowCount;
                    if (activityAccountList != null && activityAccountList.Count >= activityAccountCount)
                    {
                        activityAccountList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityAccountList, XMLFolderType.Service);
                        long activityPageNumber = totalCount / activityAccountCount;
                        activityPageNumber = activityPageNumber + 1;
                        for (int i = 2; i <= activityPageNumber; i++)
                        {
                            syncparam.PageNumber = i;
                            syncparam.RowsPerPage = activityAccountCount;
                            content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                            activityAccountResponse = GetWebAPIResponse(activityAccountURL, content);
                            if (!string.IsNullOrEmpty(activityAccountResponse))
                            {
                                activityAccountDetail = JsonConvert.DeserializeObject<ActivityAccountDetail>(activityAccountResponse);
                                activityAccountList = activityAccountDetail.ActivityAccounts;
                                if (activityAccountList != null && activityAccountList.Any())
                                {
                                    activityAccountList[0].UserLookupCode = syncparam.UserId;
                                    XMLSerializeHelper.Serialize(activityAccountList, XMLFolderType.Service);
                                }
                            }
                        }

                    }
                    else if (activityAccountList != null && activityAccountList.Any())
                    {
                        activityAccountList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityAccountList, XMLFolderType.Service);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }



        /// <summary>
        /// Get List of activity line details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityMarketing(string activityyMarketingURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityMarketingResponse = GetWebAPIResponse(activityyMarketingURL, content);
                if (!string.IsNullOrEmpty(activityMarketingResponse))
                {
                    List<ActivityMarketingInfo> activityMarketingList = JsonConvert.DeserializeObject<List<ActivityMarketingInfo>>(activityMarketingResponse);
                    if (activityMarketingList != null && activityMarketingList.Any())
                    {
                        activityMarketingList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityMarketingList, XMLFolderType.Service);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }



        /// <summary>
        /// Get List of activity line details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityClientContacts(string activityClientContactURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityClientContactResponse = GetWebAPIResponse(activityClientContactURL, content);
                if (!string.IsNullOrEmpty(activityClientContactResponse))
                {
                    List<ActivityClientContactInfo> activityClientContactList = JsonConvert.DeserializeObject<List<ActivityClientContactInfo>>(activityClientContactResponse);
                    if (activityClientContactList != null && activityClientContactList.Any())
                    {
                        activityClientContactList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityClientContactList, XMLFolderType.Service);

                    }
                    else
                    {
                        XmlToSqliteHandler.UpdateSyncLogUserTable(syncparam.UserId, Enums.ServiceMethod.ActivityClientContact);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }

        /// <summary>
        /// Get List of activity line details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityCommonLookUp(string activityCommonLookUpURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityCommonLookUpResponse = GetWebAPIResponse(activityCommonLookUpURL, content);
                if (!string.IsNullOrEmpty(activityCommonLookUpResponse))
                {
                    List<ActivityCommonLookUpInfo> activityCommonLookUpList = JsonConvert.DeserializeObject<List<ActivityCommonLookUpInfo>>(activityCommonLookUpResponse);

                    if (activityCommonLookUpList != null && activityCommonLookUpList.Any())
                    {
                        XMLSerializeHelper.Serialize<ActivityCommonLookUpInfo>(activityCommonLookUpList, XMLFolderType.Service);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }


        /// <summary>
        /// Get List of owner list details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityOwnerList(string activityOwnerListURL, SyncParams syncparam)
        {
            try
            {
                int activityOwnerListCount = Convert.ToInt32(ConfigurationManager.AppSettings["ActivityCount"]);
                syncparam.PageNumber = 1;
                syncparam.RowsPerPage = activityOwnerListCount;

                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityOwnerListResponse = GetWebAPIResponse(activityOwnerListURL, content);
                if (!string.IsNullOrEmpty(activityOwnerListResponse))
                {
                    ActivityOwnerListDetail activityOwnerDetail = JsonConvert.DeserializeObject<ActivityOwnerListDetail>(activityOwnerListResponse);
                    if (activityOwnerDetail != null)
                    {
                        List<ActivityOwnerListInfo> activityOwnerList = activityOwnerDetail.ActivityOwnerLists;
                        long totalCount = activityOwnerDetail.RowCount;

                        if (activityOwnerList != null && activityOwnerList.Count >= activityOwnerListCount)
                        {
                            XMLSerializeHelper.Serialize(activityOwnerList, XMLFolderType.Service);
                            long activityOwnerListPageNumber = totalCount / activityOwnerListCount;
                            activityOwnerListPageNumber = activityOwnerListPageNumber + 1;
                            for (int i = 2; i <= activityOwnerListPageNumber; i++)
                            {
                                try
                                {
                                    syncparam.PageNumber = i;
                                    syncparam.RowsPerPage = activityOwnerListCount;
                                    content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                                    activityOwnerListResponse = GetWebAPIResponse(activityOwnerListURL, content);
                                    if (!string.IsNullOrEmpty(activityOwnerListResponse))
                                    {
                                        activityOwnerDetail = JsonConvert.DeserializeObject<ActivityOwnerListDetail>(activityOwnerListResponse);
                                        activityOwnerList = activityOwnerDetail.ActivityOwnerLists;
                                        if (activityOwnerList != null && activityOwnerList.Any())
                                        {
                                            XMLSerializeHelper.Serialize(activityOwnerList, XMLFolderType.Service);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                                }
                            }
                        }
                        else
                        {
                            if (activityOwnerList != null && activityOwnerList.Any())
                            {
                                XMLSerializeHelper.Serialize(activityOwnerList, XMLFolderType.Service);
                            }
                        }
                    }


                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }


        /// <summary>
        /// Get List of activity list details to sync local Database
        /// </summary>
        /// <param name="activityClaimURL"></param>
        /// <returns></returns>
        public bool SyncActivityList(string activityListURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityListResponse = GetWebAPIResponse(activityListURL, content);
                if (!string.IsNullOrEmpty(activityListResponse))
                {
                    List<ActivityListInfo> activityList = JsonConvert.DeserializeObject<List<ActivityListInfo>>(activityListResponse);
                    if (activityList != null && activityList.Any())
                    {
                        activityList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityList, XMLFolderType.Service);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }

        /// <summary>
        /// Get List of activity bill details to sync local Database
        /// </summary>
        /// <param name="activityBillURL"></param>
        /// <returns></returns>
        public bool SyncActivityBill(string activityBillURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityBillResponse = GetWebAPIResponse(activityBillURL, content);
                if (!string.IsNullOrEmpty(activityBillResponse))
                {
                    List<ActivityBillInfo> activityBillList = JsonConvert.DeserializeObject<List<ActivityBillInfo>>(activityBillResponse);
                    if (activityBillList != null && activityBillList.Any())
                    {
                        activityBillList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityBillList, XMLFolderType.Service);
                    }
                    else
                    {
                        XmlToSqliteHandler.UpdateSyncLogUserTable(syncparam.UserId, Enums.ServiceMethod.ActivityBill);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }

        /// <summary>
        /// Get List of activity carrier details to sync local Database
        /// </summary>
        /// <param name="activityCarrierURL"></param>
        /// <returns></returns>
        public bool SyncActivityCarrier(string activityCarrierURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityBillResponse = GetWebAPIResponse(activityCarrierURL, content);
                if (!string.IsNullOrEmpty(activityBillResponse))
                {
                    List<ActivityCarrierInfo> activityCarrierList = JsonConvert.DeserializeObject<List<ActivityCarrierInfo>>(activityBillResponse);
                    if (activityCarrierList != null && activityCarrierList.Any())
                    {
                        activityCarrierList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityCarrierList, XMLFolderType.Service);
                    }
                    else
                    {
                        XmlToSqliteHandler.UpdateSyncLogUserTable(syncparam.UserId, Enums.ServiceMethod.ActivityCarrierSubmission);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }


        /// <summary>
        /// Get List of activity Transaction details to sync local Database
        /// </summary>
        /// <param name="activityTransactionURL"></param>
        /// <returns></returns>
        public bool SyncActivityTransaction(string activityTransactionURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityTransactionResponse = GetWebAPIResponse(activityTransactionURL, content);
                if (!string.IsNullOrEmpty(activityTransactionResponse))
                {
                    List<ActivityTransactionInfo> activityTransactionList = JsonConvert.DeserializeObject<List<ActivityTransactionInfo>>(activityTransactionResponse);
                    if (activityTransactionList != null && activityTransactionList.Any())
                    {
                        activityTransactionList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityTransactionList, XMLFolderType.Service);
                    }
                    else
                    {
                        XmlToSqliteHandler.UpdateSyncLogUserTable(syncparam.UserId, Enums.ServiceMethod.ActivityTransaction);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }



        /// <summary>
        /// Get List of activity Certificate details to sync local Database
        /// </summary>
        /// <param name="activityCertificateURL"></param>
        /// <returns></returns>
        public bool SyncActivityCertificate(string activityCertificateURL, SyncParams syncparam)
        {

            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityCertificateResponse = GetWebAPIResponse(activityCertificateURL, content);
                if (!string.IsNullOrEmpty(activityCertificateResponse))
                {
                    List<ActivityCertificateInfo> activityCertificateList = JsonConvert.DeserializeObject<List<ActivityCertificateInfo>>(activityCertificateResponse);
                    if (activityCertificateList != null && activityCertificateList.Any())
                    {
                        activityCertificateList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize(activityCertificateList, XMLFolderType.Service);
                    }
                    else
                    {
                        XmlToSqliteHandler.UpdateSyncLogUserTable(syncparam.UserId, Enums.ServiceMethod.ActivityCertificate);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }

        /// <summary>
        /// Get List of activity Evidence details to sync local Database
        /// </summary>
        /// <param name="activityEvidenceURL"></param>
        /// <returns></returns>
        public bool SyncActivityEvidence(string activityEvidenceURL, SyncParams syncparam)
        {

            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityEvidenceResponse = GetWebAPIResponse(activityEvidenceURL, content);
                if (!string.IsNullOrEmpty(activityEvidenceResponse))
                {
                    List<ActivityEvidenceInfo> activityEvidenceList = JsonConvert.DeserializeObject<List<ActivityEvidenceInfo>>(activityEvidenceResponse);
                    if (activityEvidenceList != null && activityEvidenceList.Any())
                    {
                        activityEvidenceList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize<ActivityEvidenceInfo>(activityEvidenceList, XMLFolderType.Service);
                    }
                    else
                    {
                        XmlToSqliteHandler.UpdateSyncLogUserTable(syncparam.UserId, Enums.ServiceMethod.ActivityEvidence);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }



        /// <summary>
        /// Get List of activity look up details to sync local Database
        /// </summary>
        /// <param name="activityLookUpURL"></param>
        /// <returns></returns>
        public bool SyncActivityLookUp(string activityLookUpURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityLookUpResponse = GetWebAPIResponse(activityLookUpURL, content);
                if (!string.IsNullOrEmpty(activityLookUpResponse))
                {
                    List<ActivityLookUpInfo> activityLookUpList = JsonConvert.DeserializeObject<List<ActivityLookUpInfo>>(activityLookUpResponse);
                    if (activityLookUpList != null && activityLookUpList.Any())
                    {
                        activityLookUpList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize<ActivityLookUpInfo>(activityLookUpList, XMLFolderType.Service);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }



        /// <summary>
        /// Get List of activity employee agency details to sync local Database
        /// </summary>
        /// <param name="activityLookUpURL"></param>
        /// <returns></returns>
        public bool SyncActivityEmployeeAgency(string activityEmployeeAgencyURL, SyncParams syncparam)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityEmployeeAgencyResponse = GetWebAPIResponse(activityEmployeeAgencyURL, content);
                if (!string.IsNullOrEmpty(activityEmployeeAgencyResponse))
                {
                    List<EmployeeAgencyInfo> activityEmployeeAgencyList = JsonConvert.DeserializeObject<List<EmployeeAgencyInfo>>(activityEmployeeAgencyResponse);
                    if (activityEmployeeAgencyList != null && activityEmployeeAgencyList.Any())
                    {
                        activityEmployeeAgencyList[0].UserLookupCode = syncparam.UserId;
                        XMLSerializeHelper.Serialize<EmployeeAgencyInfo>(activityEmployeeAgencyList, XMLFolderType.Service);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }

        /// <summary>
        /// Get List of activity employee agency details to sync local Database
        /// </summary>
        /// <param name="activityLookUpURL"></param>
        /// <returns></returns>
        public bool SyncActivityEmployees(string activityEmployeeURL, SyncParams syncparam)
        {
            try
            {
                int activityEmployeeCount = Convert.ToInt32(ConfigurationManager.AppSettings["ClientCount"]);
                syncparam.PageNumber = 1;
                syncparam.RowsPerPage = activityEmployeeCount;

                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityEmployeeResponse = GetWebAPIResponse(activityEmployeeURL, content);
                if (!string.IsNullOrEmpty(activityEmployeeResponse))
                {

                    ActivityEmployeeDetail activityEmployeeDetail = JsonConvert.DeserializeObject<ActivityEmployeeDetail>(activityEmployeeResponse);
                    if (activityEmployeeDetail != null)
                    {
                        List<ActivityEmployee> activityEmployeeList = activityEmployeeDetail.ActivityEmployees;
                        long totalCount = activityEmployeeDetail.RowCount;

                        if (activityEmployeeList != null && activityEmployeeList.Count >= activityEmployeeCount)
                        {
                            XMLSerializeHelper.Serialize(activityEmployeeList, XMLFolderType.Service);
                            long activityPageNumber = totalCount / activityEmployeeCount;
                            activityPageNumber = activityPageNumber + 1;
                            for (int i = 2; i <= activityPageNumber; i++)
                            {
                                try
                                {
                                    syncparam.PageNumber = i;
                                    syncparam.RowsPerPage = activityEmployeeCount;
                                    content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                                    activityEmployeeResponse = GetWebAPIResponse(activityEmployeeURL, content);
                                    if (!string.IsNullOrEmpty(activityEmployeeResponse))
                                    {
                                        activityEmployeeDetail = JsonConvert.DeserializeObject<ActivityEmployeeDetail>(activityEmployeeResponse);
                                        activityEmployeeList = activityEmployeeDetail.ActivityEmployees;
                                        if (activityEmployeeList != null && activityEmployeeList.Any())
                                        {
                                            XMLSerializeHelper.Serialize(activityEmployeeList, XMLFolderType.Service);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                                }
                            }
                        }
                        else
                        {
                            if (activityEmployeeList != null && activityEmployeeList.Any())
                            {
                                XMLSerializeHelper.Serialize(activityEmployeeList, XMLFolderType.Service);
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }

        /// <summary>
        /// Get List of client employee agency details to sync local Database
        /// </summary>
        /// <param name="activityLookUpURL"></param>
        /// <returns></returns>
        public bool SyncClientEmployee(string clientEmployeeURL, SyncParams syncparam)
        {
            try
            {
                int clientEmployeeCount = Convert.ToInt32(ConfigurationManager.AppSettings["ClientCount"]);
                syncparam.PageNumber = 1;
                syncparam.RowsPerPage = clientEmployeeCount;

                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string clientEmployeeResponse = GetWebAPIResponse(clientEmployeeURL, content);
                if (!string.IsNullOrEmpty(clientEmployeeResponse))
                {

                    ClientEmployeeDetail clientEmployeeDetail = JsonConvert.DeserializeObject<ClientEmployeeDetail>(clientEmployeeResponse);
                    if (clientEmployeeDetail != null)
                    {
                        List<ClientEmployee> clientEmployeeList = clientEmployeeDetail.ClientEmployees;
                        long totalCount = clientEmployeeDetail.RowCount;

                        if (clientEmployeeList != null && clientEmployeeList.Count >= clientEmployeeCount)
                        {
                            XMLSerializeHelper.Serialize(clientEmployeeList, XMLFolderType.Service);
                            long activityPageNumber = totalCount / clientEmployeeCount;
                            activityPageNumber = activityPageNumber + 1;
                            for (int i = 2; i <= activityPageNumber; i++)
                            {
                                try
                                {
                                    syncparam.PageNumber = i;
                                    syncparam.RowsPerPage = clientEmployeeCount;
                                    content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                                    clientEmployeeResponse = GetWebAPIResponse(clientEmployeeURL, content);
                                    if (!string.IsNullOrEmpty(clientEmployeeResponse))
                                    {
                                        clientEmployeeDetail = JsonConvert.DeserializeObject<ClientEmployeeDetail>(clientEmployeeResponse);
                                        clientEmployeeList = clientEmployeeDetail.ClientEmployees;
                                        if (clientEmployeeList != null && clientEmployeeList.Any())
                                        {
                                            XMLSerializeHelper.Serialize(clientEmployeeList, XMLFolderType.Service);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                                }
                            }
                        }
                        else
                        {
                            if (clientEmployeeList != null && clientEmployeeList.Any())
                            {
                                XMLSerializeHelper.Serialize(clientEmployeeList, XMLFolderType.Service);
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return false;
            }
            finally
            {
                Logger.save();
            }

        }



        /// <summary>
        /// Get List of activity employee agency details to sync local Database
        /// </summary>
        /// <param name="activityLookUpURL"></param>
        /// <returns></returns>
        public List<EmployeeInfo> SyncActivityEmployee(string activityEmployeeURL, SyncParams syncparam, List<string> lookupCodes)
        {
            List<EmployeeInfo> activityEmployeeList = new List<EmployeeInfo>();
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(syncparam), Encoding.UTF8, "application/json");
                string activityEmployeeResponse = GetWebAPIResponse(activityEmployeeURL, content);
                if (!string.IsNullOrEmpty(activityEmployeeResponse))
                {
                    activityEmployeeList = JsonConvert.DeserializeObject<List<EmployeeInfo>>(activityEmployeeResponse);
                    using (var context = new HIBOutlookEntities())
                    {
                        if (activityEmployeeList != null && activityEmployeeList.Any())
                        {
                            foreach (var employeeInfo in activityEmployeeList)
                            {
                                try
                                {
                                    var isAdd = false;
                                    var activityEmployeeItem = context.HIBOPEmployees.FirstOrDefault(a => a.LookupCode == employeeInfo.LookupCode);
                                    if (activityEmployeeItem == null)
                                    {
                                        isAdd = true;
                                        activityEmployeeItem = new HIBOPEmployee
                                        {
                                            SyncStatusNotified = 0
                                        };
                                    }
                                    activityEmployeeItem.UniqEntity = employeeInfo.EntityId;
                                    activityEmployeeItem.LookupCode = employeeInfo.LookupCode;
                                    activityEmployeeItem.EmployeeName = employeeInfo.EmployeeName;
                                    activityEmployeeItem.Department = employeeInfo.Department;
                                    activityEmployeeItem.JobTitle = employeeInfo.JobTitle;
                                    activityEmployeeItem.InactiveDate = employeeInfo.InactiveDate;
                                    activityEmployeeItem.RoleFlags = employeeInfo.RoleFlags;
                                    activityEmployeeItem.Flags = employeeInfo.Flags;
                                    activityEmployeeItem.InsertedDate = employeeInfo.InsertedDate;
                                    activityEmployeeItem.UpdatedDate = employeeInfo.UpdatedDate;

                                    if (employeeInfo.IsAdmin == 1)
                                        activityEmployeeItem.Status = 2;
                                    else
                                        activityEmployeeItem.Status = 1;

                                    employeeInfo.Status = 1;

                                    if (isAdd)
                                        context.HIBOPEmployees.Add(activityEmployeeItem);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                                }

                            }
                            context.SaveChanges();
                        }
                        else
                        {
                            activityEmployeeList = new List<EmployeeInfo>();
                            foreach (var lookupCode in lookupCodes)
                            {
                                activityEmployeeList.Add(new EmployeeInfo() { LookupCode = lookupCode, Status = 0 });
                            }
                        }

                        var inValidLookUpCodes = lookupCodes.Except(activityEmployeeList?.Where(m => m.Status == 1 || m.Status == 2).Select(a => a.LookupCode)).ToList();
                        foreach (var lookUpCode in inValidLookUpCodes)
                        {
                            try
                            {
                                var isAdd = false;
                                var activityEmployeeItem = context.HIBOPEmployees.FirstOrDefault(a => a.LookupCode == lookUpCode);
                                if (activityEmployeeItem == null)
                                {
                                    isAdd = true;
                                    activityEmployeeItem = new HIBOPEmployee
                                    {
                                        SyncStatusNotified = 0
                                    };
                                }

                                activityEmployeeItem.LookupCode = lookUpCode;
                                activityEmployeeItem.Status = 0;
                                if (isAdd)
                                    context.HIBOPEmployees.Add(activityEmployeeItem);
                                if (activityEmployeeList.Where(m => m.LookupCode == lookUpCode).Count() == 0)
                                    activityEmployeeList.Add(new EmployeeInfo() { LookupCode = lookUpCode, Status = 0 });
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                            }
                        }
                        context.SaveChanges();
                    }


                    var EmployeeList = XMLSerializeHelper.DeSerialize<EmployeeInfo>(XMLFolderType.Notification);
                    foreach (var employee in activityEmployeeList)
                    {
                        if (!EmployeeList.Any(m => m.LookupCode == employee.LookupCode))
                        {
                            EmployeeList.Add(employee);
                        }
                    }
                    XMLSerializeHelper.SerializeOnly<EmployeeInfo>(EmployeeList, XMLFolderType.Notification);
                }
                return activityEmployeeList;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, syncparam.UserId);
                return activityEmployeeList;
            }
            finally
            {
                Logger.save();
            }


        }

        /// <summary>
        /// Service Get Request
        /// </summary>
        /// <param name="serviceUrl"></param>
        /// <returns></returns>
        private string GetWebAPIResponse(string serviceUrl, StringContent Inputcontent)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                HttpResponseMessage response = client.PostAsync(serviceUrl, Inputcontent).Result;
                string result = "";
                using (HttpContent content = response.Content)
                {
                    Task<string> responseResult = content.ReadAsStringAsync();
                    result = responseResult.Result;
                }
                return result;
            }
        }

        /// <summary>
        /// Service post Request
        /// </summary>
        /// <param name="requestURL"></param>
        /// <param name="content"></param>
        /// <returns>response</returns>
        /// 
        private static async Task<int> PostWebAPIResponse(string requestURL, StringContent content)
        {
            int responseResult = -1;
            try
            {

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(requestURL, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        responseResult = Convert.ToInt32(result);

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

            return responseResult;
        }

        /// <summary>
        /// Get List of Sync Log
        /// </summary>
        /// <returns>SyncLog List</returns>
        public List<SyncLog> GetSyncLog()
        {
            List<SyncLog> syncLogList = _logRepository.GetSyncLog();
            return syncLogList;

        }
        /// <summary>
        /// Get Audit Log Details to Excel
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetExcelAuditLogDetails(string userLookUpcode)
        {
            DataTable auditLogTable = _logRepository.GetExcelAuditLogDetails(userLookUpcode);
            return auditLogTable;
        }

        /// <summary>
        /// Get Error Log Details to Excel
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetExcelErrorLogDetails(string userLookUpcode)
        {
            DataTable errorLogTable = _logRepository.GetExcelErrorLogDetails(userLookUpcode);
            return errorLogTable;
        }

        /// <summary>
        /// Save activity
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public Model.Activities.ResultInfo SaveActivityToEpic(AddActivity activity)
        {
            var str = JsonConvert.SerializeObject(activity);
            var content = new StringContent(JsonConvert.SerializeObject(activity), Encoding.UTF8, "application/json");
            var activityUrl = ServiceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.Createactivity);
            var postResponse = PostRequest(activityUrl, content);
            postResponse.Wait();
            return postResponse.Result;
        }

        static bool IsFileExist(bool _fileexists, int Count, string filePath)
        {
            try
            {
                Count++;
                Thread.Sleep(3000);
                return (_fileexists == false && Count <= 3) ? IsFileExist(File.Exists(filePath), Count, filePath) : true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            return false;
        }

        //private bool IsFileExist(string filePath)
        //{
        //    var IsExist = false;
        //    try
        //    {
        //        while (!IsExist)
        //        {
        //            IsExist = File.Exists(filePath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
        //    }
        //    return IsExist;
        //}

        public byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            return buffer;
        }

        public async Task<Model.AttachmentInfo> UploadingAttachmentToWebApi(Model.AttachmentInfo attachmentInfo, byte[] fileByte)
        {
            var uploadUrl = ServiceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.UploadAttachment);
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    string fileName = Path.GetFileName(attachmentInfo.FileDetails.FilePath);
                    content.Add(new StreamContent(new MemoryStream(fileByte)), "attachmentMetaData", fileName);
                    client.Timeout = new TimeSpan(0, 5, 0);
                    using (var message = await client.PostAsync(uploadUrl, content))
                    {
                        if (message.IsSuccessStatusCode || message.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var postResponse = await PostAttachmentRequest(attachmentInfo);
                            attachmentInfo = postResponse;
                        }
                        else
                        {
                            attachmentInfo.Status.Status = Status.Failed;
                            Logger.ErrorLog(string.Format("Mail Item not uploaded in Web API because of this error '{0}'", message.RequestMessage), Logger.SourceType.WindowsService, attachmentInfo.EmployeeCode);
                        }
                    }
                }
            }
            return attachmentInfo;
        }

        /// <summary>
        /// Save activity
        /// </summary>
        /// <param name="attachmentInfo"></param>
        /// <returns></returns>
        public async Task<Model.AttachmentInfo> SaveAttachmentsToEpic(Model.AttachmentInfo attachmentInfo)
        {
            attachmentInfo.Status = new AttachmentStatus();
            var attchmentInfo = new Model.AttachmentInfo
            {
                Status = new AttachmentStatus()
            };

            try
            {
                var attachmentPath = attachmentInfo.AttachmentFilePath;
                var IsExist = IsFileExist(false, 0, attachmentPath);
                if (IsExist)
                {
                    Thread.Sleep(3000);
                    var fileByte = ReadAllBytes(attachmentPath);
                    if (fileByte != null)
                    {
                        attchmentInfo = await UploadingAttachmentToWebApi(attachmentInfo, fileByte);
                    }
                    else
                    {
                        attchmentInfo.Status.Status = Status.Failed;
                        Logger.ErrorLog("Mail Item Can't be readable", Logger.SourceType.WindowsService, attachmentInfo.EmployeeCode);
                    }
                }
                else
                {
                    attchmentInfo.Status.Status = Status.Failed;
                    Logger.ErrorLog(string.Format("Mail Item not found in the physical location - {0}", attachmentInfo.AttachmentIdentifier), Logger.SourceType.WindowsService, attachmentInfo.EmployeeCode);
                }

            }
            catch (Exception ex)
            {
                attchmentInfo.Status.Status = Status.Failed;
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, attachmentInfo.EmployeeCode);
            }
            finally
            {
                Logger.save();
            }
            return attchmentInfo;
        }

        /// <summary>
        /// Post request
        /// </summary>
        /// <param name="requestURL"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static async Task<Model.Activities.ResultInfo> PostRequest(string requestURL, StringContent content)
        {
            var result = new Model.Activities.ResultInfo();
            try
            {

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(requestURL, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var resultInfo = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<Model.Activities.ResultInfo>(resultInfo);
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

            return result;
        }

        /// <summary>
        /// Post Attachment request
        /// </summary>
        /// <param name="requestURL"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task<Model.AttachmentInfo> PostAttachmentRequest(Model.AttachmentInfo attachmentInfo)
        {
            var result = attachmentInfo;

            var content = new StringContent(JsonConvert.SerializeObject(attachmentInfo), Encoding.UTF8, "application/json");
            var activityUrl = ServiceURL + EnumExtension.GetEnumDescription(Enums.ServiceMethod.CreateAttachment);

            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 5, 0);
                try
                {
                    var response = await client.PostAsync(activityUrl, content);
                    if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var resultInfo = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<Model.AttachmentInfo>(resultInfo);
                        result.Status.Status = Status.Success;
                    }
                    else
                    {
                        Logger.ErrorLog(string.Format("Attachment details not uploaded in Web API because of this error '{0}'", response.RequestMessage), Logger.SourceType.WindowsService, attachmentInfo.EmployeeCode);
                        result.Status.Status = Status.Failed;
                    }
                }
                catch (TaskCanceledException ex)
                {
                    result.Status.Status = Status.Failed;
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                }
            }

            return result;
        }


    }

}
