using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using OutlookNS = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Windows;
using Microsoft.Win32;
using HIB.Outlook.Sync.Common;
using System.Configuration;
using OutlookAddIn1.UserControls;
using OutlookAddIn1;
using HIB.Outlook.Model;
using HIB.Outlook.Helper.Common;
using HIB.Outlook.UI;
using System.IO;
using HIB.Outlook.Helper;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Threading;
using HIB.Outlook.Model.Activities;
using HIB.Outlook.Helper.Helper;

namespace AttachmentBridge
{
    public partial class ThisAddIn
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        internal static string EmployeeLookupCode;
        internal OutlookNS.Explorer CurrentExplorer = null;
        //public static OutlookNS.MailItem InlineresponseMailItem;
        internal static bool IsComposeOpen { get; set; }
        //  public static bool IsComposeOpen = false;
        internal static bool isInlineResponseActive = false;
        internal AttachToEpic attachToEpic = null;
        internal FailedAttachments failedAttachments = null;

        public delegate void MailItemSentDelegate(object mailitem, bool status);
        public static event MailItemSentDelegate MailItemSentCompleted;
        internal string _FilePathForReplyMail = string.Empty;


        public delegate void EmployeeValidDelegate(bool isValidEmployee);
        public delegate void UpdateFolderDelegate(bool isupdateNeeded);
        public static event UpdateFolderDelegate UpdateFolderinUI;
        public static event EmployeeValidDelegate IsValidEmployeEvent;
        public delegate void MaitemChangedDelegate();
        //public static event MaitemChangedDelegate OnMailItemChangedEvent;
        internal List<OwnerCode> ownerCodeCollection = new List<OwnerCode>();
        internal List<HIB.Outlook.UI.FolderInfo> MainFolderInfoCollection = new List<HIB.Outlook.UI.FolderInfo>();
        internal List<ActivityCommonLookUpInfo> CommonLookupCollection = new List<ActivityCommonLookUpInfo>();
        internal List<HIB.Outlook.UI.ClientInfo> ClientInfoCollection = new List<HIB.Outlook.UI.ClientInfo>();
        internal List<ActivityClientContactInfo> ActivityClientContactInfoCollection = new List<ActivityClientContactInfo>();
        internal List<EmployeeAgencyInfo> employeeAgencyInfoCodeCollection = new List<EmployeeAgencyInfo>();

        System.Timers.Timer timerDelay = new System.Timers.Timer();
        System.Timers.Timer employeeCodeTimer = new System.Timers.Timer();


        public SynchronizationContext TheWindowsFormsSynchronizationContext { get; private set; }
        internal OutlookNS.Explorer explorer = null;
        internal bool IsMailItemSelected = true;
        OutlookNS.Items oFolders = null;
        OutlookNS.Items sentfolder1 = null;
        OutlookNS.Items sentfolder2 = null;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {

            #region Add-in Express Regions generated code - do not modify

            this.FormsManager = AddinExpress.OL.ADXOlFormsManager.CurrentInstance;
            this.FormsManager.OnInitialize +=
                new AddinExpress.OL.ADXOlFormsManager.OnComponentInitialize_EventHandler(this.FormsManager_OnInitialize);
            this.FormsManager.Initialize(this);
            #endregion
            try
            {

                EmployeeLookupCode = CommonHelper.GetLookUpCode();
                // SaveUserDetialInfoForToastNotification();
                double interval = Convert.ToDouble(ConfigurationManager.AppSettings["ZFolderItemDeleteInterval"]);
                IsValidUser();

                timerDelay.Interval = (interval != 0) ? (interval * 60 * 1000) : (5 * 60 * 1000);
                timerDelay.Elapsed -= new System.Timers.ElapsedEventHandler(DeleteMailItems);
                timerDelay.Elapsed += new System.Timers.ElapsedEventHandler(DeleteMailItems);
                timerDelay.Start();

                CurrentExplorer = this.Application.ActiveExplorer();

                this.Application.ItemSend -= Application_ItemSend;
                this.Application.ItemSend += Application_ItemSend;
                CurrentExplorer.SelectionChange -= CurrentExplorer_SelectionChange;
                CurrentExplorer.SelectionChange += CurrentExplorer_SelectionChange;
                CurrentExplorer.InlineResponse -= CurrentExplorer_InlineResponse;
                CurrentExplorer.InlineResponse += CurrentExplorer_InlineResponse;
                CurrentExplorer.InlineResponseClose -= CurrentExplorer_InlineResponseClose;
                CurrentExplorer.InlineResponseClose += CurrentExplorer_InlineResponseClose;


                initialiseFileWatcher(ConfigurationManager.AppSettings["NotificationFolderPath"]?.ToString());
                this.TheWindowsFormsSynchronizationContext = null;
                this.TheWindowsFormsSynchronizationContext = WindowsFormsSynchronizationContext.Current
                                          ?? new WindowsFormsSynchronizationContext();

                foreach (var store in Globals.ThisAddIn.Application.Session.Stores.Cast<OutlookNS.Store>())
                {
                    if (store.DisplayName.Trim() == Globals.ThisAddIn.Application.Session.Stores.Cast<OutlookNS.Store>().First().DisplayName.Trim())
                    {
                        sentfolder1 = store.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderSentMail).Items;
                        sentfolder1.ItemAdd -= Items_SentFolder1;
                        sentfolder1.ItemAdd += Items_SentFolder1;
                    }
                    else
                    {
                        sentfolder2 = store.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderSentMail).Items;
                        sentfolder2.ItemAdd -= Items_SentFolder2;
                        sentfolder2.ItemAdd += Items_SentFolder2;
                    }

                }


                //oFolders = this.Application.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderSentMail).Items;
                //oFolders.ItemAdd -= Items_ItemAdd;
                //oFolders.ItemAdd += Items_ItemAdd;



                //if (File.Exists(notificationFilePath))
                //{
                //    var notificationCollection = XMLSerializeHelper.DeSerialize<NotificationInfo>(XMLFolderType.Notification);
                //    var dataSyncNotification = DistinctHelper.DistinctBy(notificationCollection.Where(m => m.NotificationType == 1), m => m.ContentMessage).ToList(); ///Data Sync Completed Check

                //    if (dataSyncNotification != null && dataSyncNotification.Count >= 1)
                //    {
                //        UpdateEmployeeTable();
                //        if (UpdateFolderinUI != null)
                //        {
                //            UpdateFolderinUI.Invoke(true);
                //            OnMailItemChangedEvent?.Invoke();
                //            //foreach (Delegate d in UpdateFolderinUI.GetInvocationList())
                //            //{
                //            //    UpdateFolderinUI -= (UpdateFolderDelegate)d;
                //            //}
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }


        }
        private bool FindEmail(OutlookNS.MailItem SentmailItem, OutlookNS.MAPIFolder folder)
        {
            foreach (OutlookNS.MailItem mailItem in folder.Items)
            {
                if (mailItem != null)
                {
                    if (mailItem.Subject != null)
                    {
                        if (string.Compare(SentmailItem?.Subject.Trim(), mailItem?.Subject.Trim(), StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(SentmailItem?.CC.Trim(), mailItem?.CC.Trim(), StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(SentmailItem?.To.Trim(), mailItem?.To.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private void Items_SentFolder1(object Item)
        {
            SentFolderOperation(Item);
        }
        private void Items_SentFolder2(object Item)
        {
            SentFolderOperation(Item);
        }
        private void Items_ItemAdd(object Item)
        {


        }
        private void SentFolderOperation(object Item)
        {
            try
            {
                if (IsValidEmployeEvent != null)
                {
                    attachmentControl = IsValidEmployeEvent.Target as AttachmentControls;
                }
                if (attachmentControl != null)
                {
                    string FileName = string.Empty;
                    var sentMailItem = Item as OutlookNS.MailItem;
                    OutlookNS.UserProperty isComposeMail = null;
                    OutlookNS.UserProperty replyMail = null;
                    OutlookNS.UserProperty isSAD = null;
                    OutlookNS.UserProperty attachmentFilePath = null;

                    if (sentMailItem?.UserProperties != null)
                    {
                        try
                        {
                            isComposeMail = sentMailItem?.UserProperties["IsComposeMail"];
                            replyMail = sentMailItem?.UserProperties["ReplyMail"];
                            isSAD = sentMailItem?.UserProperties["IsSendAttachDelete"];
                            attachmentFilePath = sentMailItem?.UserProperties["AttachmentFilePath"];
                        }
                        catch (System.Exception ex)
                        {
                            Logger.ErrorLog(ex.StackTrace.ToString(), Logger.SourceType.AddIn, EmployeeLookupCode);
                        }
                    }

                    if (replyMail != null)
                    {
                        if (!string.IsNullOrEmpty(_FilePathForReplyMail))
                        {
                            //OutlookNS.MailItem mail = sentMailItem.Copy() as OutlookNS.MailItem;
                            sentMailItem.SaveAs(_FilePathForReplyMail);
                            //var emailAlreadyMoved = FindEmail(sentMailItem, attachmentControl.GetCustomFolder(attachmentControl.ProcessingfolderName));
                            //if (!emailAlreadyMoved)
                            //{
                            //    (sentMailItem.Copy() as OutlookNS.MailItem).Move(attachmentControl.GetCustomFolder(attachmentControl.ProcessingfolderName));
                            //}

                            _FilePathForReplyMail = string.Empty;
                            // sentMailItem.Close(Microsoft.Office.Interop.Outlook.OlInspectorClose.olDiscard);

                        }
                        else if (attachmentFilePath != null)
                        {
                            try
                            {
                                string filePath = Convert.ToString(attachmentFilePath.Value);
                                sentMailItem.SaveAs(filePath);
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex.StackTrace.ToString(), Logger.SourceType.AddIn, EmployeeLookupCode);
                            }
                        }
                    }
                    else
                    {
                        if (isComposeMail != null)
                        {
                            if (attachmentControl != null)
                            {
                                var mailItemInfo = attachmentControl._mailItems[0];
                                if (mailItemInfo != null)
                                {
                                    mailItemInfo.MailItem = sentMailItem;
                                }
                                attachmentControl.SaveAttachmentInfo(attachmentControl._IsSendAttachAndDelete, true, false);
                                //var emailAlreadyMoved = FindEmail(sentMailItem, attachmentControl.GetCustomFolder(attachmentControl.ProcessingfolderName));
                                //if (!emailAlreadyMoved)
                                //{
                                //    (sentMailItem.Copy() as OutlookNS.MailItem).Move(attachmentControl.GetCustomFolder(attachmentControl.ProcessingfolderName));
                                //}
                                attachmentControl._IsSendAttachAndDelete = false;
                                attachmentControl.ResetAllData();
                                attachmentControl.ShowStatus(AttachmentControls.Attachment_Status);
                                Globals.ThisAddIn.FormsManager.Items[0]?.GetCurrentForm()?.Refresh();
                                //var activeExp = Globals.ThisAddIn.Application.ActiveExplorer();
                                //activeExp?.ClearSelection();
                                attachmentControl.StartSearchClients();
                            }

                        }
                    }
                    if (isSAD != null)
                    {
                        //var emailAlreadyMoved = FindEmail(sentMailItem, attachmentControl.OLA.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderDeletedItems));
                        //if (!emailAlreadyMoved)
                        //{
                        //    var deleteMailItem = sentMailItem?.Copy() as OutlookNS.MailItem;
                        //    deleteMailItem.Move(attachmentControl.OLA.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderDeletedItems));
                        //}
                        sentMailItem.Move(attachmentControl.OLA.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderDeletedItems));
                    }



                }

            }
            catch (System.Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            finally
            {

                Logger.save();
            }
        }

        private void CurrentExplorer_InlineResponseClose()
        {
            isInlineResponseActive = false;
            //var addinControl = (MailItemSentCompleted.Target) as AttachmentControls;
            //if (addinControl != null)
            //{
            //    addinControl.ResetAllData();
            //    addinControl.ControlsVisibilityBasedOnMailMode();
            //}
            SelectionChangeEvent();
            //OnMailItemChangedEvent?.Invoke();
        }

        private void CurrentExplorer_InlineResponse(object Item)
        {
            isInlineResponseActive = true;
            if (MailItemSentCompleted != null)
            {
                SelectionChangeEvent();
                // OnMailItemChangedEvent?.Invoke();
                //IsComposeOpen = true;
                //var addinControl = (MailItemSentCompleted.Target) as AttachmentControls;
                //addinControl.ControlsVisibilityBasedOnMailMode();
            }
        }
        FileSystemWatcher FileWatcher = null;
        private void initialiseFileWatcher(string filePath)
        {
            try
            {
                FileWatcher = new FileSystemWatcher();
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                FileWatcher.Path = filePath;
                /* Watch for changes in LastAccess and LastWrite times, and  the renaming of files or directories. */
                FileWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.Security | NotifyFilters.DirectoryName;
                // Only watch text files.
                FileWatcher.Filter = "*.xml";
                // Add event handlers.
                FileWatcher.Changed -= new FileSystemEventHandler(OnChanged);
                FileWatcher.Changed += new FileSystemEventHandler(OnChanged);
                // Begin watching.
                FileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
        }
        private object _lockObj = new object();
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            lock (_lockObj)
            {
                try
                {
                    if (File.Exists(e.FullPath))
                    {
                        if (e.ChangeType == WatcherChangeTypes.Changed)
                        {
                            var isDataSyncStartStatusNeedToShow = true;
                            var isDataSyncCompletedStatusNeedtoShow = true;

                            try
                            {
                                var clientExistsQuery = string.Format("Select * from HIBOPSyncLogUserDetails where SyncLogField='{0}' and EmployeeLookupcode='{1}'", Enums.ServiceMethod.Client.ToString(), EmployeeLookupCode);
                                var sqliteDataReader = HIB.Outlook.SQLite.SQLiteHandler.ExecuteSelectQuery(clientExistsQuery);
                                if (sqliteDataReader != null && sqliteDataReader.HasRows)
                                {
                                    isDataSyncStartStatusNeedToShow = false;
                                }
                                else
                                {
                                    isDataSyncStartStatusNeedToShow = true;
                                }

                                var activityOwnerQuery = string.Format("Select * from HIBOPSyncLogUserDetails where SyncLogField='{0}' and EmployeeLookupcode='{1}'", Enums.ServiceMethod.ActivityPolicy.ToString(), EmployeeLookupCode);
                                var activityOwnerSqliteDataReader = HIB.Outlook.SQLite.SQLiteHandler.ExecuteSelectQuery(activityOwnerQuery);
                                if (activityOwnerSqliteDataReader != null && activityOwnerSqliteDataReader.HasRows)
                                {
                                    isDataSyncCompletedStatusNeedtoShow = false;
                                }
                                else
                                {
                                    isDataSyncCompletedStatusNeedtoShow = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
                            }
                            finally
                            {
                                Logger.save();
                            }


                            var employeeInfoCollection = XMLSerializeHelper.DeSerializeOnly<EmployeeInfo>(XMLFolderType.Notification);
                            if (employeeInfoCollection != null)
                            {
                                var employee = employeeInfoCollection.Where(m => m.LookupCode == EmployeeLookupCode).FirstOrDefault();
                                if (employee != null)
                                {
                                    if (IsValidEmployeEvent != null)
                                    {
                                        attachmentControl = IsValidEmployeEvent.Target as AttachmentControls;
                                    }

                                    var notificationFileName = string.Empty;
                                    if (!string.IsNullOrEmpty(EmployeeLookupCode))
                                    {
                                        notificationFileName = string.Format("NotificationInfo - {0}", EmployeeLookupCode);
                                    }
                                    else
                                    {
                                        notificationFileName = "NotificationInfo";
                                    }

                                    var notificationCollection = XMLSerializeHelper.DeSerialize<NotificationInfo>(XMLFolderType.Notification, notificationFileName);
                                    List<NotificationInfo> dataSyncCollection = new List<NotificationInfo>();
                                    var dataSyncNotification = DistinctHelper.DistinctBy(notificationCollection.Where(m => m.NotificationType != 2 && m.NotificationType != 3 && m.LookupCode == EmployeeLookupCode), m => m.ContentMessage).ToList(); ///Data Sync Started

                                    if (isDataSyncStartStatusNeedToShow && employee.SyncStatusNotified != 1)//|| employee.SyncStatusNotified == 0
                                    {
                                        dataSyncCollection.AddRange(dataSyncNotification.Where(m => m.ContentMessage == Constants.ToastNotifications.DataSyncStartedContentText).ToList());
                                    }
                                    if (!isDataSyncCompletedStatusNeedtoShow && employee.CompletedSyncStatusNotified != 1)//|| employee.CompletedSyncStatusNotified == 0 //&& (attachmentControl?.ClientInfoCollection == null || attachmentControl?.ClientInfoCollection.Count <= 0)
                                    {
                                        dataSyncCollection.AddRange(dataSyncNotification.Where(m => m.ContentMessage == Constants.ToastNotifications.DataSyncCompletedContentText).ToList());
                                    }
                                    var errorNotification = notificationCollection.Where(m => (m.NotificationType == 2 || m.NotificationType == 3 || m.NotificationType == 4) && m.LookupCode == EmployeeLookupCode).ToList(); ///Error Cases
                                    errorNotification.AddRange(dataSyncCollection);
                                    foreach (var notification in errorNotification)
                                    {
                                        try
                                        {
                                            if (notification.NotificationType == 2 || notification.NotificationType == 3)
                                            {
                                                CreateToastNotification(notification.TitleMessage, notification.ContentMessage, true);
                                                if (notification.NotificationType == 2)
                                                {
                                                    attachmentControl?.ThisAddIn_IsValidEmployeEvent(ThisAddIn.IsValidUser(true));
                                                }
                                            }
                                            else if (notification.NotificationType == 0 || notification.NotificationType == 1)
                                            {
                                                CreateToastNotification(notification.TitleMessage, notification.ContentMessage, false);
                                            }

                                            if (notification.NotificationType == 3 || notification.NotificationType == 4)
                                            {
                                                if (attachmentControl != null)
                                                {
                                                    attachmentControl?.RefreshFailedListAttachments();
                                                    attachmentControl?.UpdateFailedCountForAttachment();
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
                                        }
                                        finally
                                        {
                                            Logger.save();
                                        }
                                    }

                                    if (dataSyncCollection.Exists(m => m.ContentMessage == Constants.ToastNotifications.DataSyncStartedContentText) && employee.SyncStatusNotified == 0)
                                    {
                                        employee.SyncStatusNotified = 1;
                                        XMLSerializeHelper.SerializeOnly<EmployeeInfo>(employeeInfoCollection, XMLFolderType.Notification);
                                    }
                                    if (dataSyncCollection.Exists(m => m.ContentMessage == Constants.ToastNotifications.DataSyncCompletedContentText) && employee.CompletedSyncStatusNotified == 0)
                                    {
                                        employee.SyncStatusNotified = 1;
                                        employee.CompletedSyncStatusNotified = 1;
                                        XMLSerializeHelper.SerializeOnly<EmployeeInfo>(employeeInfoCollection, XMLFolderType.Notification);
                                        attachmentControl?.ThisAddIn_IsValidEmployeEvent(ThisAddIn.IsValidUser(true));
                                        try
                                        {

                                            // UpdateEmployeeTable();
                                            if (UpdateFolderinUI != null)
                                            {
                                                UpdateFolderinUI.Invoke(true);
                                                //foreach (Delegate d in UpdateFolderinUI.GetInvocationList())
                                                //{
                                                //    UpdateFolderinUI -= (UpdateFolderDelegate)d;
                                                //}
                                            }
                                            SelectionChangeEvent();
                                            //OnMailItemChangedEvent?.Invoke();
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                                        }

                                    }
                                    if (dataSyncCollection.Exists(m => m.ContentMessage == Constants.ToastNotifications.DataSyncCompletedContentText))
                                    {
                                        attachmentControl?.ThisAddIn_IsValidEmployeEvent(ThisAddIn.IsValidUser(true));
                                    }

                                }

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.AddIn, "");
                }
                finally
                {
                    Logger.save();
                }
            }

        }
        private static void UpdateEmployeeTable()
        {
            try
            {
                using (var context = new HIB.Outlook.SQLite.HIBOutlookEntities())
                {
                    var result = context.HIBOPEmployees.Where(d => d.LookupCode == EmployeeLookupCode)?.FirstOrDefault();
                    result.SyncStatusNotified = 1;
                    List<EmployeeInfo> employeeCollection = new List<EmployeeInfo>();
                    EmployeeInfo employeeInfo = new EmployeeInfo
                    {
                        LookupCode = result.LookupCode,
                        SyncStatusNotified = result.SyncStatusNotified.Value
                    };
                    employeeCollection.Add(employeeInfo);
                    XMLSerializeHelper.Serialize<EmployeeInfo>(employeeCollection, XMLFolderType.AddIn, "UpdateActivityEmployee");
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, "");
            }
            finally
            {
                Logger.save();
            }

        }

        private static object notificationLock = new object();
        private static void CreateToastNotification(string TitleText, string ContentText, bool isErrorMessage)
        {
            try
            {
                lock (notificationLock)
                {
                    using (Notifications.NotificationsManager manager = new Notifications.NotificationsManager(ContentText, OutlookAddIn1.Properties.Resources.icon_16x16))
                    {
                        if (isErrorMessage)
                        {
                            manager.ShowBalloonToolTip(TitleText, ContentText, 3, System.Windows.Forms.ToolTipIcon.Error);
                        }
                        else
                        {
                            manager.ShowBalloonToolTip(TitleText, ContentText, 3, System.Windows.Forms.ToolTipIcon.Info);
                        }
                        Task.Delay(3000).Wait();

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

        public static bool IsValidUser(bool isDataSyncCompleted = false)
        {
            IsValidEmployee = false;
            IsAdmin = false;
            IsdataSynCompleted = false;

            try
            {
                using (var context = new HIB.Outlook.SQLite.HIBOutlookEntities())
                {
                    var result = context.HIBOPEmployees.Where(d => d.LookupCode == EmployeeLookupCode)?.FirstOrDefault();
                    if (result != null)
                    {
                        if (result.SyncStatusNotified == 1 || isDataSyncCompleted)
                        {
                            IsdataSynCompleted = true;
                        }
                        if (result.Status == 1)
                        {
                            IsValidEmployee = true;
                        }
                        else if (result.Status == 2)
                        {
                            IsAdmin = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IsValidEmployee = false;
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return IsValidEmployee;
        }


        private bool isClientRefreshed = false;
        private object removeLockObject = new object();
        AttachmentControls attachmentControl = null;

        public static void CheckWindowsServiceStatus(AttachmentControls attachmentControls)
        {
            Int32 isPaneToBeDisabled = 0;
            string activityIdQuery = $"select IsPaneToBeDisabled from HIBOPDisablePane";
            var sqliteDataReader = SqliteHelper.ExecuteSelectQuery(activityIdQuery);
            while (sqliteDataReader.Read())
            {
                try
                {
                    isPaneToBeDisabled = Convert.ToInt32(sqliteDataReader["IsPaneToBeDisabled"]);
                    ThisAddIn.IsPaneToBeDisabled = isPaneToBeDisabled;
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.AddIn, "");
                }

            }
            if (attachmentControls != null)
            {
                if (isPaneToBeDisabled == 1)
                {
                    attachmentControls.grdError.Visibility = Visibility.Visible;
                    attachmentControls.TblError.Visibility = Visibility.Visible;
                    attachmentControls.TblError.Text = ConfigurationManager.AppSettings["WindowsServiceStatus"]?.ToString();
                }
                else if (ThisAddIn.IsdataSynCompleted && ThisAddIn.IsValidEmployee)
                {
                    attachmentControls.grdError.Visibility = Visibility.Collapsed;
                    attachmentControls.TblError.Visibility = Visibility.Collapsed;
                    var employeeInfoCollection = XMLSerializeHelper.DeSerializeOnly<EmployeeInfo>(XMLFolderType.Notification);
                    if (employeeInfoCollection != null)
                    {
                        var employee = employeeInfoCollection.Where(m => m.LookupCode == EmployeeLookupCode).FirstOrDefault();
                        if (employee != null && employee.CompletedSyncStatusNotified == 0)
                        {
                            CreateToastNotification(Constants.ToastNotifications.DataSyncHeaderText, Constants.ToastNotifications.DataSyncCompletedContentText, false);
                            employee.SyncStatusNotified = 1;
                            employee.CompletedSyncStatusNotified = 1;
                            XMLSerializeHelper.SerializeOnly<EmployeeInfo>(employeeInfoCollection, XMLFolderType.Notification);
                        }
                    }
                }

            }
        }
        private void DeleteMailItems(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (removeLockObject)
                {
                    if (!IsValidEmployee || !IsdataSynCompleted) IsValidUser();

                    if (IsValidEmployeEvent != null)
                    {
                        attachmentControl = IsValidEmployeEvent.Target as AttachmentControls;
                    }

                    Globals.ThisAddIn.TheWindowsFormsSynchronizationContext.Send(d =>
                    {
                        CheckWindowsServiceStatus(attachmentControl);
                    }, null);

                    OutlookNS.MAPIFolder odeleteFolders = this.Application.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderDeletedItems);
                    if (odeleteFolders != null)
                    {
                        foreach (var deleteitem in odeleteFolders?.Items)
                        {
                            var deletemailitem = deleteitem as OutlookNS.MailItem;
                            var attachmentIdentifier = deletemailitem?.UserProperties["AttachmentIdentifier"];
                            if (attachmentIdentifier != null)
                            {
                                string queryString = string.Format("Select * From AttachmentInfo where IsPushedToEpic = 1 and IsAttachDelete = 1 and AttachmentIdentifier='{0}' and EmployeeCode ='{1}'", attachmentIdentifier.Value, ThisAddIn.EmployeeLookupCode);
                                var isPushedToEpic = IsAttachmentPushedToEpic(queryString);
                                if (isPushedToEpic)
                                {
                                    deletemailitem.Delete();
                                }
                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.StackTrace, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
        }
        private bool IsAttachmentPushedToEpic(string queryString)
        {
            var sqliteDataReaderDataTable = HIB.Outlook.SQLite.SQLiteHandler.ExecuteSelecttQueryWithAdapter(queryString);
            if (sqliteDataReaderDataTable.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void RemoveMailItemFromzEpicFolder(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (removeLockObject)
                {
                    if (!IsValidEmployee) IsValidUser();
                    //IsValidEmployeEvent?.Invoke(IsValidEmployee);
                    if (!isClientRefreshed)
                    {
                        if (IsValidEmployeEvent != null)
                        {
                            attachmentControl = IsValidEmployeEvent.Target as AttachmentControls;
                            //if (attachmentControl != null && (attachmentControl.ClientInfoCollection == null || attachmentControl.ClientInfoCollection.Count <= 0))
                            //{
                            //    attachmentControl.GetClientInformationFromSQLite(String.Format(AttachmentControls._listOfClientsQuery, EmployeeLookupCode));
                            //    isClientRefreshed = true;
                            //}
                        }
                    }
                    if (IsValidEmployee && attachmentControl != null)
                    {
                        Globals.ThisAddIn.TheWindowsFormsSynchronizationContext.Send(d =>
                        {
                            CheckWindowsServiceStatus(attachmentControl);
                        }, null);

                        List<AttachmentInfo> attachmentInfoCollection = new List<AttachmentInfo>();
                        OutlookNS.MAPIFolder oFolders = this.Application.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderInbox);

                        foreach (var item in oFolders.Folders)
                        {
                            var currentFolder = item as OutlookNS.Folder;
                            if (currentFolder.Name.Equals("zEpicprocessing", StringComparison.OrdinalIgnoreCase))
                            {
                                var mailItems = currentFolder.Items;
                                var attachmentlist = GetAttachmentInfoDetails($"Select * From AttachmentInfo where IsDeletedFromZFolder = 0 and IsPushedToEpic = 1 and EmployeeCode ='{ThisAddIn.EmployeeLookupCode}'");

                                foreach (var Attachmentitem in attachmentlist)
                                {
                                    foreach (var curentitem in mailItems)
                                    {
                                        try
                                        {
                                            var mailitm = curentitem as OutlookNS.MailItem;
                                            var Sub = mailitm.Subject;
                                            var attachmentIdentifier = mailitm.UserProperties["AttachmentIdentifier"];
                                            // var To = attachmentControl.GetListOfToMailId(mailitm, Microsoft.Office.Interop.Outlook.OlMailRecipientType.olTo);
                                            if ((string.IsNullOrWhiteSpace(mailitm.Subject) || string.Equals(Attachmentitem.Subject, mailitm.Subject)) && string.Equals(Attachmentitem.AttachmentIdentifier, attachmentIdentifier?.Value))
                                            {
                                                try
                                                {
                                                    OutlookNS.MAPIFolder odeleteFolders = this.Application.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderDeletedItems);
                                                    mailitm.UserProperties.Add("IsAttchDeleted", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                                                    mailitm.Save();
                                                    mailitm.Move(odeleteFolders);

                                                    foreach (var deleteitem in odeleteFolders.Items)
                                                    {
                                                        var deletemailitem = deleteitem as OutlookNS.MailItem;
                                                        var userPreference = deletemailitem?.UserProperties["IsAttchDeleted"];
                                                        if (userPreference != null)
                                                        {
                                                            deletemailitem.Delete();
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
                                                }

                                                attachmentInfoCollection.Add(Attachmentitem);
                                                break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
                                        }
                                    }
                                }

                            }
                        }
                        XMLSerializeHelper.Serialize<AttachmentInfo>(attachmentInfoCollection, XMLFolderType.AddIn, "UpdateZProcessingFolderStatus");

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }
        private void CurrentExplorer_SelectionChange()
        {
            try
            {
                Task.Run(async () =>
                {
                    await SelectionChange();
                });


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }
        private async Task SelectionChange()
        {
            try
            {
                Globals.ThisAddIn.TheWindowsFormsSynchronizationContext.Send(d =>
                {
                    var addinControl = (IsValidEmployeEvent.Target) as AttachmentControls;
                    if (IsValidEmployeEvent != null && addinControl != null)
                    {
                        if (Globals.ThisAddIn.ClientInfoCollection != null && Globals.ThisAddIn.ClientInfoCollection.Count == 0)
                        {
                            Globals.ThisAddIn.ClientInfoCollection = addinControl.GetClientInformationFromSQLite(String.Format(AttachmentControls._listOfClientsQuery, ThisAddIn.EmployeeLookupCode));
                            addinControl.OnPropertyChanged("MyActiveClientFilteredItems");
                            addinControl.OnPropertyChanged("MyInActiveClientFilteredItems");
                        }

                        if (addinControl.policyList != null && (addinControl.policyList.PolicyTypeInfoCollection == null || addinControl.policyList.PolicyTypeInfoCollection.Count <= 0))
                            addinControl.policyList.GetPolicyTypeListFromSQLite();
                        if (CommonLookupCollection == null || CommonLookupCollection.Count <= 0)
                        {
                            addinControl.GetAllCommonLookupFromLocalDatabase();
                            addinControl.LoadAllValuesFromCommonLookup();
                        }
                        if (ActivityClientContactInfoCollection == null || ActivityClientContactInfoCollection.Count <= 0)
                            addinControl.GetAllActivityClientContactInfoFromLocalDatabase();

                        if (employeeAgencyInfoCodeCollection == null && employeeAgencyInfoCodeCollection.Count <= 0)
                            addinControl.LoadAllEmployeeWithAgency();
                        if (addinControl.addactivityMainPage != null && addinControl.addactivityMainPage.addactivitySecondPage != null && addinControl.addactivityMainPage.addactivitySecondPage.AddActivityCode.Items.Count <= 0)
                            addinControl.LoadAddActivityCodeWithDesc();
                        if (ownerCodeCollection == null || ownerCodeCollection.Count <= 0)
                            addinControl.LoadOwnerCodeList();

                        if ((MainFolderInfoCollection != null && MainFolderInfoCollection.Count == 0) || addinControl.mainFolderComboBox.Items.Count <= 0)
                        {
                            MainFolderInfoCollection = addinControl.GetFoldersFromSQLite(AttachmentControls._mainFolderQuery);
                            MainFolderInfoCollection = MainFolderInfoCollection.OrderBy(m => m.FolderName).ToList();
                            if (MainFolderInfoCollection.Count > 0)
                            {
                                addinControl.mainFolderComboBox.ItemsSource = null;
                                addinControl.mainFolderComboBox.ItemsSource = MainFolderInfoCollection;
                            }
                        }

                        //if (ThisAddIn.IsComposeOpen)
                        //{
                        //    ThisAddIn.IsComposeOpen = false;
                        //}
                        //else
                        if (!string.IsNullOrEmpty(addinControl.GetAllSelectedEmails().FirstOrDefault()?.Identifier))
                        {
                            if (addinControl.attachToEpicMainPage == null)
                            {
                                addinControl.TbtnAttachmentAssistActive.Visibility = Visibility.Visible;
                                addinControl.TbtnAttachmentAssist.Visibility = Visibility.Collapsed;
                            }
                        }

                    }
                    if (attachToEpic != null && attachToEpic.ribbon != null)
                        attachToEpic.ribbon.InvalidateControl("MyButton");
                    addinControl.UpdateFailedCountForAttachment();
                    // OnMailItemChangedEvent?.Invoke();
                    SelectionChangeEvent();
                }, null);
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
        }

        private void SelectionChangeEvent()
        {
            try
            {
                var currentForm = Globals.ThisAddIn.PushToEpicFormRegionItem.GetCurrentForm() as PushToEpicFormRegion;
                var addinControl = currentForm?.attachmentControls1;
                if (addinControl != null)
                {
                    try
                    {
                        CheckWindowsServiceStatus(addinControl);
                        addinControl.ResetAllData();
                        var exchangeConnectionMode = Globals.ThisAddIn.Application.Session.ExchangeConnectionMode;
                        var showInfoErrorLog = ConfigurationManager.AppSettings["ShowInfoLog"]?.ToString();
                        if (ThisAddIn.IsdataSynCompleted)
                        {
                            if (showInfoErrorLog == "1")
                                Logger.InfoLog("Data Sync Completed", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                            if (Globals.ThisAddIn.CurrentExplorer.Selection.Count > 1)
                            {
                                if (showInfoErrorLog == "1")
                                    Logger.InfoLog("Selection  Count greater than 1", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                                addinControl.grdError.Visibility = Visibility.Visible;
                                addinControl.TblError.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                if (showInfoErrorLog == "1")
                                    Logger.InfoLog("Selection  Count equals 1", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                                addinControl.grdError.Visibility = Visibility.Collapsed;
                                addinControl.TblError.Visibility = Visibility.Collapsed;
                            }
                            if (!ThisAddIn.IsValidEmployee && !ThisAddIn.IsAdmin)
                            {
                                if (showInfoErrorLog == "1")
                                    Logger.InfoLog("Not a Valid Employee", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                                addinControl.grdError.Visibility = Visibility.Visible;
                                addinControl.TblError.Visibility = Visibility.Visible;
                                addinControl.TblError.Text = ConfigurationManager.AppSettings["NoAccess"]?.ToString();
                            }
                            else if (ThisAddIn.IsAdmin)
                            {
                                if (showInfoErrorLog == "1")
                                    Logger.InfoLog("Admin User", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                                addinControl.grdError.Visibility = Visibility.Visible;
                                addinControl.TblError.Visibility = Visibility.Visible;
                                addinControl.TblError.Text = ConfigurationManager.AppSettings["AdminAccess"]?.ToString();
                            }
                            else if (Globals.ThisAddIn.CurrentExplorer.Selection.Count == 1)
                            {
                                if (showInfoErrorLog == "1")
                                    Logger.InfoLog("Select Mail Count 1", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                                addinControl.grdError.Visibility = Visibility.Collapsed;
                            }
                            if (ThisAddIn.IsPaneToBeDisabled == 1)
                            {
                                addinControl.grdError.Visibility = Visibility.Visible;
                                addinControl.TblError.Visibility = Visibility.Visible;
                                addinControl.TblError.Text = ConfigurationManager.AppSettings["WindowsServiceStatus"]?.ToString();
                            }
                            if (exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olCachedDisconnected || exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olCachedOffline || exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olDisconnected || exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olOffline)
                            {
                                addinControl.grdError.Visibility = Visibility.Visible;
                                addinControl.TblError.Visibility = Visibility.Visible;
                                addinControl.TblError.Text = ConfigurationManager.AppSettings["NoExchangeServer"]?.ToString();
                            }
                            if (IsMailItemSelected)
                            {
                                if (exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olCachedConnectedFull || exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olNoExchange || exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olOnline)
                                {
                                    if (Globals.ThisAddIn.CurrentExplorer.Selection.Count == 0)
                                    {
                                        CurrentExplorer.ClearSelection();
                                        IsMailItemSelected = false;
                                        addinControl.grdError.Visibility = Visibility.Collapsed;
                                        addinControl.TblError.Visibility = Visibility.Collapsed;
                                    }
                                }

                            }
                            if (addinControl.attachToEpicMainPage == null && addinControl.sendAndAttachGrid.Visibility == Visibility.Collapsed)
                            {
                                if (Globals.ThisAddIn.CurrentExplorer.Selection.Count == 0)
                                {
                                    addinControl.grdError.Visibility = Visibility.Visible;
                                    addinControl.TblError.Visibility = Visibility.Visible;
                                    addinControl.TblError.Text = ConfigurationManager.AppSettings["NoMailSelection"]?.ToString();
                                }
                            }

                        }
                        else
                        {
                            if (showInfoErrorLog == "1")
                                Logger.InfoLog("Data Sync In progress", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                            addinControl.grdError.Visibility = Visibility.Visible;
                            addinControl.TblError.Visibility = Visibility.Visible;
                            addinControl.TblError.Text = ConfigurationManager.AppSettings["DataSync"]?.ToString();
                        }

                        addinControl.ControlsVisibilityBasedOnMailMode();
                        if (!string.IsNullOrEmpty(addinControl.GetAllSelectedEmails().FirstOrDefault()?.Identifier))
                        {
                            if (addinControl.attachToEpicMainPage == null)
                            {
                                addinControl.TbtnAttachmentAssistActive.Visibility = Visibility.Visible;
                                addinControl.TbtnAttachmentAssist.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                addinControl.TbtnAttachmentAssistActive.Visibility = Visibility.Collapsed;
                                addinControl.TbtnAttachmentAssist.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            addinControl.TbtnAttachmentAssistActive.Visibility = Visibility.Collapsed;
                            addinControl.TbtnAttachmentAssist.Visibility = Visibility.Visible;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
        }
        private List<AttachmentInfo> GetAttachmentInfoDetails(string queryString)
        {
            List<AttachmentInfo> attachmentInfoList = new List<AttachmentInfo>();
            try
            {

                var sqliteDataReaderDataTable = HIB.Outlook.SQLite.SQLiteHandler.ExecuteSelecttQueryWithAdapter(queryString);

                if (sqliteDataReaderDataTable != null)
                {
                    for (int i = 0; i < sqliteDataReaderDataTable.Rows.Count; i++)
                    {
                        try
                        {
                            var attachmentInfoItem = new AttachmentInfo
                            {
                                AttachmentId = Convert.ToInt32(sqliteDataReaderDataTable.Rows[i]["AttachmentId"]),
                                EmailToAddress = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailToAddress"]),
                                EmailToDisplayName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailToDisplayName"]),
                                Subject = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["Subject"]),
                                IsPushedToEpic = Convert.ToBoolean(sqliteDataReaderDataTable.Rows[i]["IsPushedToEpic"]),
                                IsDeletedFromZFolder = Convert.ToBoolean(sqliteDataReaderDataTable.Rows[i]["IsDeletedFromZFolder"]),
                                AttachmentFilePath = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentFilePath"]),
                                IsAttachDelete = Convert.ToBoolean(sqliteDataReaderDataTable.Rows[i]["IsAttachDelete"]),
                                AttachmentIdentifier = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentIdentifier"])

                            };
                            attachmentInfoList.Add(attachmentInfoItem);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            return attachmentInfoList;
        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            attachToEpic = new AttachToEpic();
            return attachToEpic;
        }

        private void Application_ItemSend(object Item, ref bool Cancel)
        {
            MailItemSentCompleted?.Invoke(Item, Cancel);
        }



        private static bool _isValidEmployee;

        public static bool IsValidEmployee
        {
            get { return _isValidEmployee; }
            set { _isValidEmployee = value; }
        }

        public static Int32 IsPaneToBeDisabled { get; set; }

        private static bool _isAdmin;

        public static bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; }
        }


        private static bool _isdataSynCompleted;

        public static bool IsdataSynCompleted
        {
            get { return _isdataSynCompleted; }
            set { _isdataSynCompleted = value; }
        }

        private static bool _dataSyncNotified;
        public static bool IsDataSyncNotified
        {
            get { return _dataSyncNotified; }
            set { _dataSyncNotified = value; }
        }
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

            #region Add-in Express Regions generated code - do not modify

            this.FormsManager.Finalize(this);

            #endregion
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see http://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup -= new System.EventHandler(ThisAddIn_Startup);
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown -= new System.EventHandler(ThisAddIn_Shutdown);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);

        }

        #endregion
    }
}