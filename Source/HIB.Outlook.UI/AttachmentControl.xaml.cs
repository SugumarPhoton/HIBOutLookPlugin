using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using OutlookNS = Microsoft.Office.Interop.Outlook;
using HIB.Outlook.Model;
using System.Threading.Tasks;
using CustomControls;
using HIB.Outlook.Model.Activities;
using System.Windows.Data;
using System.Configuration;
using HIB.Outlook.Sync;
using HIB.Outlook.Sync.Common;
using OutlookAddIn1.UserControls;
using System.Windows.Media.Effects;
using System.Windows.Interop;
using CustomControls.Controls;
using OutlookAddIn1;
using HIB.Outlook.Common;
using AttachmentBridge;
using System.Data.SqlTypes;
using System.IO;
using HIB.Outlook.Helper.Common;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Threading;
using System.DirectoryServices.AccountManagement;

namespace HIB.Outlook.UI
{
    /// <summary>
    /// Interaction logic for AttachmentControls.xaml
    /// </summary>
    /// 
    public static class DistinctHelper
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
       (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
    public partial class AttachmentControls : UserControl, INotifyPropertyChanged
    {
        #region Static Property

        #endregion

        #region Constant Variable
        private const string Valid_Favorite = "Please enter a valid favourite name";
        private const string Favorite_Exists = "This name already exists.";
        internal const string Attachment_Status = "Attachment will be done shortly";
        public const string CUSTOM = "Custom";
        public const string USE_EMAIL = "Use Email Subj.";
        public const string USE_ACTIVITY = "Use Activity Desc.";
        public const string GET_LATEST_ATTACHMENTINFO = "Select * from AttachmentInfo where EmployeeCode ='{0}' order by ModifiedDate desc LIMIT 1";
        public const string GET_FAILED_ATTACHMENTINFO = "Select * from AttachmentInfo where EmployeeCode ='{0}' and IsEpicPushInProgress=0 and IsPushedToEpic=0  order by ModifiedDate desc";

        #endregion

        #region Private and Internal Properties
        private string SyncFilePath = string.Empty;
        //internal List<OwnerCode> ownerCodeCollection = new List<OwnerCode>();
        internal AttachToEpicMainPage attachToEpicMainPage = null;
        Window addActivityWindow = null;
        Window policyTypeWindow = null;

        Window failedAttachmentWindow = null;
        internal AddActivityMainPage addactivityMainPage = null;
        internal PolicyTypeList policyList = null;
        List<Model.FavouriteInfo> FavouriteInfoCollection = new List<Model.FavouriteInfo>();
        //internal List<FolderInfo> MainFolderInfoCollection = new List<FolderInfo>();
        List<FolderInfo> SubFolder1InfoCollection = new List<FolderInfo>();
        List<FolderInfo> SubFolder2InfoCollection = new List<FolderInfo>();

        internal List<SelectedEmailInfo> selectedEmailInfoCollection = new List<SelectedEmailInfo>();
        //internal List<Model.ActivityCommonLookUpInfo> CommonLookupCollection = new List<Model.ActivityCommonLookUpInfo>();

        //internal List<Model.ActivityClientContactInfo> ActivityClientContactInfoCollection = new List<Model.ActivityClientContactInfo>();
        //internal List<EmployeeAgencyInfo> employeeAgencyInfoCodeCollection = new List<EmployeeAgencyInfo>();


        internal List<EmployeeInfo> employeeInfoCollection = new List<EmployeeInfo>();

        #region Add Activities

        List<Policy> policyCollection = new List<Policy>();
        List<Line> lineCollection = new List<Line>();
        List<Opportunities> opportunitiesCollection = new List<Opportunities>();
        List<Services> servicesCollection = new List<Services>();
        List<MasterMarketingSubmission> masterMarketingCollection = new List<MasterMarketingSubmission>();
        #endregion

        public const string _listOfClientsQuery = "Select * From HIBOPClient C inner join HIBOPClientEmployee E ON C.UniqEntity = E.UniqEntity WHERE E.EmployeeLookupcode = '{0}'";


        private const string _subfolderQuery = "SELECT * FROM HIBOPFolderAttachment where ParentFolderId={0}";
        private const string _allFoldersQuery = "SELECT * FROM HIBOPFolderAttachment";
        private const string _favouriteNameExistsQuery = "SELECT Count(*) FROM HIBOPFavourites Where FavouriteName='{0}'";
        private const string _attachmentIdentifierExistsQuery = "SELECT Count(*) FROM AttachmentInfo Where Identifier='{0}' and EmployeeCode ='{1}'";
        private const string _allFavouritesQuery = "SELECT * FROM HIBOPFavourites where UniqEmployee='{0}' order by ModifiedDate desc";
        private const string _allCommonLookupQuery = "SELECT * FROM HIBOPCommonLookup";
        private const string _allActivityListQuery = "SELECT * FROM HIBOPActivityList";
        private const string _allEmployeeAgencyInfoQuery = "SELECT * FROM HIBOPEmployeeAgency";
        private const string _allEmployeeDetailsInfoQuery = "SELECT * FROM HIBOPEmployee";
        private const string _allOwnerListQuery = "SELECT Lookupcode,EmployeeName FROM HIBOPActivityOwnerList";
        private const string _allActivityContactInfoQuery = "SELECT * FROM HIBOPActivityClientContacts";
        //private const string _allActivityLookUpDetailsQuery = "SELECT * FROM HIBOPActivityLookupDetails";
        internal const string _mainFolderQuery = "SELECT * FROM HIBOPFolderAttachment where FolderType='MN'";
        private const string _favouriteDeleteQuery = "Delete from HIBOPFavourites where FavouriteName='{0}'";
        private const string _failedAttachementsCountQuery = "SELECT Count(*) FROM AttachmentInfo Where IsPushedToEpic=0 and IsEpicPushInProgress=0 and EmployeeCode ='{0}'";

        internal List<MailItemInfo> _mailItems = new List<MailItemInfo>();
        internal OutlookNS.Explorer explorer = null;
        internal OutlookNS._Application OLA = new OutlookNS.Application();
        // internal string ProcessingfolderName = "zEpicprocessing";//"Processing Folder";
        OutlookNS.MAPIFolder customFolder = null;
        internal string _FilePath = string.Empty;

        #endregion

        #region Public Properties

        public bool _IsDeleteAttached { get; set; }
        public bool _IsSendAttachAndDelete { get; set; }
        public List<PolicyInfo> OpenActivitiesInfoCollection
        {
            get; set;
        }


        public List<PolicyInfo> ClosedActivitiesInfoCollection
        {
            get; set;
        }

        public IEnumerable<PolicyInfo> OpenActivitiesFilteredItems
        {
            get
            {
                if (OpenActivitiesInfoCollection != null)
                {
                    if (string.IsNullOrEmpty(activityFilterTextBox.Text?.Trim())) return OpenActivitiesInfoCollection.OrderByDescending(x => x.InsertedDate);
                    if (activityFilterTextBox.Text.Length >= 3)
                    {
                        var searchedOpenActivitiesInfoCollection = OpenActivitiesInfoCollection.Where(x => x.PolicyDesc.ToUpper().Contains(activityFilterTextBox.Text?.Trim().ToUpper()) || x.PolicyCode.ToUpper().Contains(activityFilterTextBox.Text?.Trim().ToUpper()));
                        var openActivities = OpenActivitiesInfoCollection.Where(x => x.PolicyDesc.ToUpper().Contains(activityFilterTextBox.Text?.Trim().ToUpper()) || x.PolicyCode.ToUpper().Contains(activityFilterTextBox.Text?.Trim().ToUpper())).OrderBy(x => x.PolicyDesc);
                        openActivities = openActivities.OrderByDescending(m => m.InsertedDate);
                        return openActivities;
                    }
                    else
                    {
                        var openActivities = OpenActivitiesInfoCollection.OrderBy(x => x.PolicyDesc);
                        openActivities = openActivities.OrderByDescending(x => x.InsertedDate);
                        return openActivities;
                    }
                }
                else
                {
                    return default(IEnumerable<PolicyInfo>);
                }



            }
        }


        public IEnumerable<PolicyInfo> ClosedActivitiesFilteredItems
        {
            get
            {
                if (string.IsNullOrEmpty(activityFilterTextBox.Text?.Trim())) return ClosedActivitiesInfoCollection?.OrderByDescending(x => x.InsertedDate);
                if (activityFilterTextBox.Text?.Trim().Length >= 3)
                {
                    var searchedClosedActivitiesInfoCollection = ClosedActivitiesInfoCollection.Where(x => x.PolicyDesc.ToUpper().Contains(activityFilterTextBox.Text?.Trim().ToUpper()) || x.PolicyCode.ToUpper().Contains(activityFilterTextBox.Text?.Trim().ToUpper()));
                    searchedClosedActivitiesInfoCollection = searchedClosedActivitiesInfoCollection.OrderByDescending(m => m.InsertedDate);
                    return searchedClosedActivitiesInfoCollection;
                }
                else
                {
                    var closedActivites = ClosedActivitiesInfoCollection.OrderByDescending(m => m.InsertedDate);
                    return closedActivites;
                }
            }
        }
        public int _failedNotificationCount = 0;
        public int FailedNotificationCount
        {
            get { return _failedNotificationCount; }
        }


        // public static readonly DependencyProperty FailedNotificationCountProperty =
        //DependencyProperty.Register("FailedNotificationCount", typeof(int), typeof(AttachmentControls), new UIPropertyMetadata(20));
        public ObservableCollection<ClientInfo> MyActiveClientFilteredItems
        {
            get
            {
                return GetClientsFilter(true);
            }
            //get
            //{
            //    if (string.IsNullOrEmpty(waterMarkTextBox.Text)) return ClientInfoCollection.Where(m => m.IsActive).ToList();
            //    if (waterMarkTextBox.Text.Length >= 3)
            //    {
            //        var searchedActiveInfoCollection = ClientInfoCollection.Where(x => x.IsActive && (x.ClientDescription.ToUpper().StartsWith(waterMarkTextBox.Text.ToUpper()) || x.EpicCode.ToUpper().StartsWith(waterMarkTextBox.Text.ToUpper())));
            //        return searchedActiveInfoCollection;
            //    }
            //    else
            //    {
            //        //MessageBox.Show("Please enter at-least 3 characters");
            //        return ClientInfoCollection;
            //    }
            //}
        }
        public IEnumerable<ClientInfo> MyInActiveClientFilteredItems
        {
            get
            {
                return GetClientsFilter(false);
            }
            //get
            //{
            //    if (string.IsNullOrEmpty(waterMarkTextBox.Text)) return ClientInfoCollection.Where(m => !m.IsActive).ToList();
            //    if (waterMarkTextBox.Text.Length >= 3)
            //    {
            //        var searchedInActiveClientInfoCollection = ClientInfoCollection.Where(x => !x.IsActive && x.ClientDescription.ToUpper().StartsWith(waterMarkTextBox.Text.ToUpper()) || x.EpicCode.ToUpper().StartsWith(waterMarkTextBox.Text.ToUpper()));
            //        return searchedInActiveClientInfoCollection;
            //    }
            //    else
            //    {

            //        return ClientInfoCollection;
            //    }
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Static Data
        static int Currentyr = DateTime.Now.Year;
        List<PolicyYear> policyYearLst = new List<PolicyYear>() { new PolicyYear { IsSelected = false, Description = "(none)" }, new PolicyYear { IsSelected = false, Description = Convert.ToString(Currentyr - 2) }, new PolicyYear { IsSelected = false, Description = Convert.ToString(Currentyr - 1) }, new PolicyYear { IsSelected = true, Description = Convert.ToString(Currentyr) }, new PolicyYear { IsSelected = false, Description = Convert.ToString(Currentyr + 1) } };
        #endregion

        #region Constructor

        public AttachmentControls()
        {
            try
            {
                InitializeComponent();
                SyncFilePath = ConfigurationManager.AppSettings["DBFolderPath"]?.ToString();
                if (Globals.ThisAddIn != null && Globals.ThisAddIn.Application != null)
                    explorer = Globals.ThisAddIn.Application.ActiveExplorer();
                if (explorer != null)
                {
                    //((OutlookNS.ExplorerEvents_10_Event)explorer).Activate -=
                    //     new OutlookNS.ExplorerEvents_10_ActivateEventHandler(
                    //     Explorer_Activate);
                    //((OutlookNS.ExplorerEvents_10_Event)explorer).Activate +=
                    //     new OutlookNS.ExplorerEvents_10_ActivateEventHandler(
                    //     Explorer_Activate);
                }
                this.DataContext = this;
                //ThisAddIn.MailItemSentCompleted -= ThisAddIn_MailItemSentCompleted;
                //ThisAddIn.MailItemSentCompleted += ThisAddIn_MailItemSentCompleted;
                ThisAddIn.IsValidEmployeEvent -= ThisAddIn_IsValidEmployeEvent;
                ThisAddIn.IsValidEmployeEvent += ThisAddIn_IsValidEmployeEvent;
                ThisAddIn.UpdateFolderinUI -= ThisAddIn_UpdateFolderinUI;
                ThisAddIn.UpdateFolderinUI += ThisAddIn_UpdateFolderinUI;

                _FilePath = ConfigurationManager.AppSettings["MailItemFolderPath"]?.ToString();
                LstPolicyList.ItemsSource = policyYearLst;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString(), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        private void Explorer_Activate()
        {
            try
            {
                var activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                if (activeWindow is OutlookNS.Explorer)
                {
                    var currentFormRegion = Globals.ThisAddIn.PushToEpicFormRegionItem.GetCurrentForm() as PushToEpicFormRegion;
                    if (currentFormRegion != null && attachToEpicMainPage == null)
                    {
                        currentFormRegion.attachmentControls1.sendAndAttachGrid.Visibility = Visibility.Collapsed;
                        currentFormRegion.attachmentControls1.attachanddeleteonly.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }

        }

        public static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }
        void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var dp = sender as DatePicker;
                if (dp == null) return;

                var tb = GetChildOfType<DatePickerTextBox>(dp);
                if (tb == null) return;

                var wm = tb.Template.FindName("PART_Watermark", tb) as ContentControl;
                if (wm == null) return;

                wm.Content = "MM/DD/YYYY";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        private void ThisAddIn_UpdateFolderinUI(bool isupdateNeeded)
        {
            try
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {

                    try
                    {
                        if (Globals.ThisAddIn.ClientInfoCollection == null || Globals.ThisAddIn.ClientInfoCollection.Count <= 0)
                        {
                            Globals.ThisAddIn.ClientInfoCollection = GetClientInformationFromSQLite(String.Format(_listOfClientsQuery, ThisAddIn.EmployeeLookupCode));
                            OnPropertyChanged("MyActiveClientFilteredItems");
                            OnPropertyChanged("MyInActiveClientFilteredItems");
                        }

                        if (mainFolderComboBox.Items.Count <= 0 || (Globals.ThisAddIn.MainFolderInfoCollection != null && Globals.ThisAddIn.MainFolderInfoCollection.Count == 0))
                        {
                            Globals.ThisAddIn.MainFolderInfoCollection = GetFoldersFromSQLite(AttachmentControls._mainFolderQuery);
                            Globals.ThisAddIn.MainFolderInfoCollection = Globals.ThisAddIn.MainFolderInfoCollection.OrderBy(m => m.FolderName).ToList();

                            mainFolderComboBox.ItemsSource = null;
                            mainFolderComboBox.ItemsSource = Globals.ThisAddIn.MainFolderInfoCollection;
                        }

                        if (policyList?.PolicyTypeInfoCollection == null || policyList?.PolicyTypeInfoCollection.Count <= 0)
                            policyList?.GetPolicyTypeListFromSQLite();
                        if (Globals.ThisAddIn.CommonLookupCollection == null || Globals.ThisAddIn.CommonLookupCollection.Count <= 0)
                        {
                            GetAllCommonLookupFromLocalDatabase();
                            LoadAllValuesFromCommonLookup();
                        }
                        if (Globals.ThisAddIn.ActivityClientContactInfoCollection == null || (Globals.ThisAddIn.ActivityClientContactInfoCollection.Count <= 0))
                            GetAllActivityClientContactInfoFromLocalDatabase();
                        if (Globals.ThisAddIn.employeeAgencyInfoCodeCollection == null && Globals.ThisAddIn.employeeAgencyInfoCodeCollection.Count <= 0)
                            LoadAllEmployeeWithAgency();

                        if (addactivityMainPage != null && addactivityMainPage.addactivitySecondPage != null)
                            LoadAddActivityCodeWithDesc();
                        if (Globals.ThisAddIn.ownerCodeCollection == null || Globals.ThisAddIn.ownerCodeCollection.Count <= 0)
                            LoadOwnerCodeList();

                        ThisAddIn_IsValidEmployeEvent(ThisAddIn.IsValidUser(true));
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                    }
                }));

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }


        }

        internal void ThisAddIn_IsValidEmployeEvent(bool isValidEmployee)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                try
                {
                    var exchangeConnectionMode = Globals.ThisAddIn.Application.Session.ExchangeConnectionMode;
                    var showInfoErrorLog = ConfigurationManager.AppSettings["ShowInfoLog"]?.ToString();
                    IsEnabled = isValidEmployee;

                    if (ThisAddIn.IsdataSynCompleted)
                    {
                        if (Globals.ThisAddIn.CurrentExplorer.Selection.Count > 1 && attachToEpicMainPage == null)
                        {
                            if (showInfoErrorLog == "1")
                                Logger.InfoLog("Selection  Count greater than 1", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                            grdError.Visibility = Visibility.Visible;
                            TblError.Visibility = Visibility.Collapsed;
                        }
                        else if (exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olCachedDisconnected || exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olCachedOffline || exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olDisconnected || exchangeConnectionMode == Microsoft.Office.Interop.Outlook.OlExchangeConnectionMode.olOffline)
                        {
                            grdError.Visibility = Visibility.Visible;
                            TblError.Visibility = Visibility.Visible;
                            TblError.Text = ConfigurationManager.AppSettings["NoExchangeServer"]?.ToString();
                        }
                        else if (!isValidEmployee && !ThisAddIn.IsAdmin)
                        {
                            if (showInfoErrorLog == "1")
                                Logger.InfoLog("Not a Valid User From Notification", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);

                            grdError.Visibility = Visibility.Visible;
                            TblError.Visibility = Visibility.Visible;
                            TblError.Text = ConfigurationManager.AppSettings["NoAccess"]?.ToString();
                        }
                        else if (ThisAddIn.IsAdmin)
                        {
                            if (showInfoErrorLog == "1")
                                Logger.InfoLog("Admin User From Notification", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);

                            grdError.Visibility = Visibility.Visible;
                            TblError.Visibility = Visibility.Visible;
                            TblError.Text = ConfigurationManager.AppSettings["AdminAccess"]?.ToString();
                        }
                        else
                        {
                            if (showInfoErrorLog == "1")
                                Logger.InfoLog("Sync Success for Valid Employee From Notification", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);

                            grdError.Visibility = Visibility.Collapsed;
                            TblError.Visibility = Visibility.Collapsed;
                        }
                        if (ThisAddIn.IsPaneToBeDisabled == 1)
                        {
                            grdError.Visibility = Visibility.Visible;
                            TblError.Visibility = Visibility.Visible;
                            TblError.Text = ConfigurationManager.AppSettings["WindowsServiceStatus"]?.ToString();
                        }
                        if (attachToEpicMainPage == null && sendAndAttachGrid.Visibility == Visibility.Collapsed)
                        {
                            if (Globals.ThisAddIn.CurrentExplorer.Selection.Count == 0)
                            {
                                grdError.Visibility = Visibility.Visible;
                                TblError.Visibility = Visibility.Visible;
                                TblError.Text = ConfigurationManager.AppSettings["NoMailSelection"]?.ToString();

                            }
                        }

                    }
                    else
                    {
                        if (showInfoErrorLog == "1")
                            Logger.InfoLog("Sync in progress From Notification", typeof(AttachmentControls), Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        grdError.Visibility = Visibility.Visible;
                        TblError.Visibility = Visibility.Visible;
                        TblError.Text = ConfigurationManager.AppSettings["DataSync"]?.ToString();
                    }

                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                }
                finally
                {
                    Logger.save();
                }

            }));

        }


        private void ThisAddIn_MailItemSentCompleted(object mailitem, bool status)
        {
            //try
            //{
            //    var sentMailItem = mailitem as OutlookNS.MailItem;

            //    if (_IsDeleteAttached)
            //    {
            //        var deleteMailItem = sentMailItem.Copy() as OutlookNS.MailItem;
            //        deleteMailItem.Move(OLA.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderDeletedItems));
            //        _IsDeleteAttached = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            //}
            //finally
            //{
            //    Logger.save();
            //}
        }

        public void ControlsVisibilityBasedOnMailMode()
        {
            try
            {
                if (attachToEpicMainPage == null)
                {
                    var currentFormRegion = Globals.ThisAddIn.PushToEpicFormRegionItem.GetCurrentForm() as PushToEpicFormRegion;
                    var currentIntPtr = OfficeWin32Window.GetActiveWindow();
                    StringBuilder s = new StringBuilder(50);
                    int count = 50;
                    OfficeWin32Window.GetWindowText(currentIntPtr, s, count);
                    var windowTitle = s.ToString();
                    var explorer = Globals.ThisAddIn.Application.ActiveExplorer();
                    var activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                    var inspector = Globals.ThisAddIn.Application.ActiveInspector();
                    if (activeWindow != null && activeWindow is OutlookNS.Explorer && currentFormRegion != null)
                    {
                        if (explorer != null)
                        {
                            if (currentFormRegion != null)
                            {
                                if (ThisAddIn.isInlineResponseActive || string.Equals(windowTitle.Trim(), "Untitled - Message (HTML)"))
                                {
                                    currentFormRegion.attachmentControls1.sendAndAttachGrid.Visibility = Visibility.Visible;
                                    currentFormRegion.attachmentControls1.attachanddeleteonly.Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    currentFormRegion.attachmentControls1.sendAndAttachGrid.Visibility = Visibility.Collapsed;
                                    currentFormRegion.attachmentControls1.attachanddeleteonly.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                if (ThisAddIn.isInlineResponseActive)
                                {
                                    sendAndAttachGrid.Visibility = Visibility.Visible;
                                    attachanddeleteonly.Visibility = Visibility.Collapsed;

                                }
                                else
                                {
                                    sendAndAttachGrid.Visibility = Visibility.Collapsed;
                                    attachanddeleteonly.Visibility = Visibility.Visible;
                                }
                            }


                        }
                    }
                    else
                    {

                        inspector = Globals.ThisAddIn.Application.ActiveInspector();
                        var currentMailItem = inspector?.CurrentItem as OutlookNS.MailItem;
                        if (currentFormRegion != null && currentMailItem != null)
                        {
                            if (!currentMailItem.Sent || ThisAddIn.isInlineResponseActive)
                            {
                                currentFormRegion.attachmentControls1.sendAndAttachGrid.Visibility = Visibility.Visible;
                                currentFormRegion.attachmentControls1.attachanddeleteonly.Visibility = Visibility.Collapsed;
                                sendAndAttachGrid.Visibility = Visibility.Visible;
                                attachanddeleteonly.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                currentFormRegion.attachmentControls1.sendAndAttachGrid.Visibility = Visibility.Collapsed;
                                currentFormRegion.attachmentControls1.attachanddeleteonly.Visibility = Visibility.Visible;
                                sendAndAttachGrid.Visibility = Visibility.Collapsed;
                                attachanddeleteonly.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            if ((currentMailItem != null && !(bool)currentMailItem?.Sent) || ThisAddIn.isInlineResponseActive)
                            {
                                TbtnAttachmentAssist.Visibility = Visibility.Visible;
                                sendAndAttachGrid.Visibility = Visibility.Visible;
                                attachanddeleteonly.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                sendAndAttachGrid.Visibility = Visibility.Collapsed;
                                attachanddeleteonly.Visibility = Visibility.Visible;
                            }
                        }
                        if (currentMailItem != null)
                        {
                            Marshal.ReleaseComObject(currentMailItem);
                            currentMailItem = null;
                        }


                    }
                    //else
                    //{
                    //    if (currentFormRegion != null)
                    //    {
                    //        currentFormRegion.attachmentControls1.sendAndAttachGrid.Visibility = Visibility.Collapsed;
                    //        currentFormRegion.attachmentControls1.attachanddeleteonly.Visibility = Visibility.Visible;
                    //    }
                    //    sendAndAttachGrid.Visibility = Visibility.Collapsed;
                    //    attachanddeleteonly.Visibility = Visibility.Visible;
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode, typeof(AttachmentControls));
            }
            finally
            {
                Logger.save();
            }
        }

        #endregion

        #region Custom Events

        public event RoutedEventHandler RBPolicyYear_CheckedEvent;
        //public event RoutedEventHandler BtnPolicyTypeEvent;
        public event RoutedEventHandler BtnAddAcctivityEvent;
        #endregion

        #region Events
        ///<summary>
        /// writing all the columns from selected Add To Type in Data Grid with generic type for multiple attachment .
        /// </summary>
        /// <returns>void</returns>
        private void WriteColumnsForAttachingToActivityTypeFromClass<T>()
        {
            try
            {
                if (attachToEpicMainPage != null)
                {
                    attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.Columns.Clear();
                    PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(typeof(T));
                    foreach (PropertyDescriptor property in collection)
                    {
                        try
                        {
                            if (property.DisplayName != "No display")
                            {
                                DataGridTextColumn column = new DataGridTextColumn
                                {
                                    Header = property.DisplayName,
                                    Binding = new Binding(property.Name),
                                    Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto)
                                };
                                attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.Columns.Add(column);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.Source is TabControl)
                {
                    StartSearchClients();
                    if (Tab.SelectedIndex == 0)
                    {
                        var selectActiveClient = LstActiveClient.SelectedItem as ClientInfo;
                        RefreshActivitiesForSelectedClient(selectActiveClient);
                    }
                    else if (Tab.SelectedIndex == 1)
                    {
                        var selectInActiveClient = LstInActiveClient.SelectedItem as ClientInfo;
                        RefreshActivitiesForSelectedClient(selectInActiveClient);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
        }

        private void ActivitiesTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.Source is TabControl)
                {
                    StartSearchActivities();
                    if (ActivitiesTab.SelectedIndex == 0)
                    {
                        var selectedActivity = ActiveActivityList.SelectedItem as PolicyInfo;
                        RefreshPolicyYearAndDescription(selectedActivity);
                        EnableAttachmentsWithActivitySelection(selectedActivity, true);
                    }
                    else if (ActivitiesTab.SelectedIndex == 1)
                    {
                        var selectedInActivity = InActiveActivityList.SelectedItem as PolicyInfo;
                        RefreshPolicyYearAndDescription(selectedInActivity);
                        EnableAttachmentsWithActivitySelection(selectedInActivity, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }

        }

        private void EnableAttachmentsWithActivitySelection(PolicyInfo selectedActivity, bool IsOpenActivity)
        {
            try
            {

                if (selectedActivity != null)
                {
                    if (IsOpenActivity)
                    {
                        InActiveActivityList.SelectedItem = null;
                    }
                    else
                    {
                        ActiveActivityList.SelectedItem = null;
                    }
                    if (attachanddeleteonly.Visibility == Visibility.Collapsed)
                    {
                        btnSendAttachment.IsEnabled = true;
                        btnSendDeleteAndAttach.IsEnabled = true;
                    }
                    else
                    {
                        btnAttachmentOnly.IsEnabled = true;
                        btnAttachmentAndDeleteOnly.IsEnabled = true;
                    }
                }
                else
                {

                    if (attachanddeleteonly.Visibility == Visibility.Collapsed)
                    {
                        btnSendAttachment.IsEnabled = false;
                        btnSendDeleteAndAttach.IsEnabled = false;
                    }
                    else
                    {
                        btnAttachmentOnly.IsEnabled = false;
                        btnAttachmentAndDeleteOnly.IsEnabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var formRegions = Globals.ThisAddIn?.FormsManager?.Items[0];
                if (formRegions != null)
                {
                    if (formRegions.GetCurrentForm().RegionState == AddinExpress.OL.ADXRegionState.Minimized)
                        formRegions.GetCurrentForm().RegionState = AddinExpress.OL.ADXRegionState.Normal;
                    else
                        formRegions.GetCurrentForm().RegionState = AddinExpress.OL.ADXRegionState.Minimized;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
        }

        private void DpAccesibility_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var dp = sender as DatePicker;
                if (dp == null) return;

                var tb = GetChildOfType<DatePickerTextBox>(dp);
                if (tb == null) return;

                var wm = tb.Template.FindName("PART_Watermark", tb) as ContentControl;
                if (wm == null) return;

                wm.Content = "MM/DD/YYYY";
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                requiredFieldAlert.Visibility = Visibility.Collapsed;
                if (ValidateAllFields(true))
                {
                    if (attachToEpicMainPage != null)
                    {
                        attachToEpicMainPage.attachmentControls.Visibility = Visibility.Collapsed;
                        attachToEpicMainPage._multipleEmailAttachmentWindow.Height = 650;
                        attachToEpicMainPage.multipleEmailAttachment.Visibility = Visibility.Visible;
                    }

                    var clientSelectedItem = ActiveTab.IsSelected ? (LstActiveClient.SelectedItem as ClientInfo) : (LstInActiveClient.SelectedItem as ClientInfo);
                    var activitySelectedItem = OpenActivityTab.IsSelected ? (ActiveActivityList.SelectedItem as PolicyInfo) : (InActiveActivityList.SelectedItem as PolicyInfo);
                    if (clientSelectedItem != null)
                    {
                        attachToEpicMainPage.multipleEmailAttachment.clientCodeTextBlock.Text = clientSelectedItem.EpicCode;
                        attachToEpicMainPage.multipleEmailAttachment.clientCodeTextBlock.ToolTip = clientSelectedItem.EpicCode;

                        attachToEpicMainPage.multipleEmailAttachment.clientDescriptionTextBlock.Text = clientSelectedItem.ClientDescription;
                        attachToEpicMainPage.multipleEmailAttachment.clientDescriptionTextBlock.ToolTip = clientSelectedItem.ClientDescription;
                    }
                    if (activitySelectedItem != null)
                    {
                        attachToEpicMainPage.multipleEmailAttachment.activityCodeTextBlock.Text = activitySelectedItem.PolicyCode;
                        attachToEpicMainPage.multipleEmailAttachment.activityCodeTextBlock.ToolTip = activitySelectedItem.PolicyCode;

                        attachToEpicMainPage.multipleEmailAttachment.activityDescTextBlock.Text = activitySelectedItem.PolicyDisplayDesc;
                        attachToEpicMainPage.multipleEmailAttachment.activityDescTextBlock.ToolTip = activitySelectedItem.PolicyDisplayDesc;

                        GetActivityForSelectedActivity(activitySelectedItem);
                        //List<PolicyInfo> selectedActivityCollection = new List<PolicyInfo>();
                        //selectedActivityCollection.Add(activitySelectedItem);
                        //attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = selectedActivityCollection;
                    }
                    attachToEpicMainPage.multipleEmailAttachment.activityType.Text = activitySelectedItem.AssociationType + ":";

                    var folderSelectedItem = mainFolderComboBox.SelectedItem as FolderInfo;
                    var subFolder1SelectedItem = subFolder1ComboBox.SelectedItem as FolderInfo;
                    var subFolder2SelectedItem = subFolder2ComboBox.SelectedItem as FolderInfo;

                    if (folderSelectedItem != null)
                    {
                        attachToEpicMainPage.multipleEmailAttachment.mainfolderTextBlock.Text = folderSelectedItem.FolderName;
                    }
                    if (subFolder1SelectedItem != null)
                    {
                        attachToEpicMainPage.multipleEmailAttachment.subfolder1TextBlock.Text = subFolder1SelectedItem.FolderName;
                    }
                    if (subFolder2SelectedItem != null)
                    {
                        attachToEpicMainPage.multipleEmailAttachment.subfolder2TextBlock.Text = subFolder2SelectedItem.FolderName;
                    }
                    else
                    {
                        attachToEpicMainPage.multipleEmailAttachment.subfolder2TextBlock.Text = "<none>";

                    }

                    var clientAccessibleDate = chkUntilDate.IsChecked == true ? (DpAccesibility.SelectedDate != null ? DpAccesibility.SelectedDate.Value.ToShortDateString() : string.Empty) : string.Empty;
                    if (!string.IsNullOrEmpty(clientAccessibleDate))
                    {
                        attachToEpicMainPage.multipleEmailAttachment.clientAccessibleDate.Text = clientAccessibleDate;
                    }
                    else
                    {
                        attachToEpicMainPage.multipleEmailAttachment.clientAccessibleDate.Text = "<none>";
                    }

                    if (selectedEmailInfoCollection == null || selectedEmailInfoCollection.Count <= 0)
                        GetAllSelectedMailsFromOutlook(activitySelectedItem.PolicyDesc);
                    else
                    {
                        string policyYear = (LstPolicyList.SelectedItem as PolicyYear)?.Description;
                        selectedEmailInfoCollection.ForEach(x => { x.PolicyType = string.IsNullOrEmpty(lblPolicyType.Text) ? "(none)" : lblPolicyType.Text; x.PolicyYear = policyYear; x.ActivityDesc = activitySelectedItem.PolicyDesc; });
                    }

                    if (attachToEpicMainPage != null && _mailItems.Count > 0)
                    {
                        selectedEmailInfoCollection = selectedEmailInfoCollection.OrderBy(m => m.MailItem?.ReceivedTime).ToList();
                        if (selectedEmailInfoCollection.Count > 0)
                            selectedEmailInfoCollection[0].IsApplyToAllNeedToBeVisible = true;
                        attachToEpicMainPage.multipleEmailAttachment.SelectedMailList.ItemsSource = null;
                        attachToEpicMainPage.multipleEmailAttachment.SelectedMailList.ItemsSource = selectedEmailInfoCollection;
                        attachToEpicMainPage.multipleEmailAttachment.SelectedDescMailList.ItemsSource = null;
                        attachToEpicMainPage.multipleEmailAttachment.SelectedDescMailList.ItemsSource = selectedEmailInfoCollection;
                    }

                    if (attachToEpicMainPage != null)
                        attachToEpicMainPage.multipleEmailAttachment.SelectedMailList.SelectedIndex = 0;



                }
                else
                {
                    requiredFieldAlert.Visibility = Visibility.Visible;
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void LblPolicyType_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                policyTypeTextBlock.Text = lblPolicyType.Text;
                if (!string.IsNullOrEmpty(policyTypeTextBlock.Text))
                {
                    policyTypeTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    if (policyList != null)
                    {
                        policyList.DgPolicyType.SelectedItem = null;
                        policyList.btnDonePolicyType.IsEnabled = false;
                    }


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }
        private void ActiveActivityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedActivity = ActiveActivityList.SelectedItem as PolicyInfo;
            RefreshPolicyYearAndDescription(selectedActivity);
            EnableAttachmentsWithActivitySelection(selectedActivity, true);
        }
        private void RBPolicyYear_Checked(object sender, RoutedEventArgs e)
        {
            RBPolicyYear_CheckedEvent?.Invoke(sender, e);
        }


        private void TbtnAttachmentAssist_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string attachmentInfoQuery = $"Select * From AttachmentInfo where Identifier = '{_mailItems.FirstOrDefault()?.Identifier}' and EmployeeCode ='{ThisAddIn.EmployeeLookupCode}' order by AttachmentId desc";
                var attachmentinfoList = GetAttachmentInfoDetails(attachmentInfoQuery);
                var InActiveActivity = InActiveActivityList.SelectedItem as PolicyInfo;
                var ActiveActivity = ActiveActivityList.SelectedItem as PolicyInfo;
                if (attachmentinfoList.Any())
                {
                    var attachmentinfo = attachmentinfoList.FirstOrDefault();
                    if (attachmentinfo != null)
                    {
                        if (attachmentinfo.ClientId > 0)
                        {
                            var favouriteClient = Globals.ThisAddIn.ClientInfoCollection.FirstOrDefault(x => x.IsActive == attachmentinfo.IsActiveClient && x.ClientId == attachmentinfo.ClientId);
                            if (favouriteClient != null)
                            {
                                waterMarkTextBox.Text = favouriteClient.EpicCode;
                                if (attachmentinfo.IsActiveClient)
                                {
                                    LstActiveClient.SelectedItem = null;
                                    LstActiveClient.SelectedItem = favouriteClient;
                                    var selectedClient = LstActiveClient.SelectedItem as ClientInfo;
                                    if (LstActiveClient.SelectedItem == null || selectedClient != null && favouriteClient.EpicCode != selectedClient.EpicCode)
                                    {
                                        Tab.SelectedIndex = 0;
                                        StartSearchClients();
                                        LstActiveClient.SelectedItem = null;
                                        LstActiveClient.SelectedItem = favouriteClient;
                                    }
                                    LstActiveClient.ScrollIntoView(LstActiveClient.SelectedItem);
                                }
                                else
                                {
                                    LstInActiveClient.SelectedItem = null;
                                    LstInActiveClient.SelectedItem = favouriteClient;
                                    var selectedClient = LstInActiveClient.SelectedItem as ClientInfo;
                                    if (LstInActiveClient.SelectedItem == null || selectedClient != null && favouriteClient.EpicCode != selectedClient.EpicCode)
                                    {
                                        Tab.SelectedIndex = 1;
                                        StartSearchClients();
                                        LstInActiveClient.SelectedItem = null;
                                        LstInActiveClient.SelectedItem = favouriteClient;
                                    }
                                    LstInActiveClient.ScrollIntoView(LstInActiveClient.SelectedItem);

                                }
                            }
                        }

                        if (attachmentinfo.ActivityId > 0 || (attachmentinfo.ActivityId == 0 && !string.IsNullOrEmpty(attachmentinfo.ActivityGuid)))
                        {
                            if (attachmentinfo.IsClosedActivity)
                            {
                                ActivitiesTab.SelectedIndex = 1;
                                if (ClosedActivitiesInfoCollection != null)
                                {
                                    if (attachmentinfo.ActivityId == 0)
                                        InActiveActivityList.SelectedItem = ClosedActivitiesInfoCollection.FirstOrDefault(x => x.ActivityGuid == attachmentinfo.ActivityGuid);
                                    else
                                        InActiveActivityList.SelectedItem = ClosedActivitiesInfoCollection.FirstOrDefault(x => x.ActivityId == attachmentinfo.ActivityId);
                                    if (InActiveActivityList.SelectedItem != null)
                                        InActiveActivityList.ScrollIntoView(InActiveActivityList.SelectedItem);

                                }
                            }
                            else
                            {
                                ActivitiesTab.SelectedIndex = 0;
                                if (OpenActivitiesInfoCollection != null)
                                {
                                    if (attachmentinfo.ActivityId == 0)
                                        ActiveActivityList.SelectedItem = OpenActivitiesInfoCollection.FirstOrDefault(x => x.ActivityGuid == attachmentinfo.ActivityGuid);
                                    else
                                        ActiveActivityList.SelectedItem = OpenActivitiesInfoCollection.FirstOrDefault(x => x.ActivityId == attachmentinfo.ActivityId);
                                    if (ActiveActivityList.SelectedItem != null)
                                        ActiveActivityList.ScrollIntoView(ActiveActivityList.SelectedItem);

                                }
                            }
                        }
                        else
                        {
                            if (ActiveActivity != null)
                            {
                                ActiveActivityList.SelectedItem = ActiveActivity;
                                if (ActiveActivityList.SelectedItem != null)
                                    ActiveActivityList.ScrollIntoView(ActiveActivityList.SelectedItem);
                            }
                            else
                            {
                                InActiveActivityList.SelectedItem = InActiveActivity;
                                if (InActiveActivityList.SelectedItem != null)
                                    InActiveActivityList.ScrollIntoView(InActiveActivityList.SelectedItem);
                            }

                        }

                        var ClosedActivityselectedItem = InActiveActivityList.SelectedItem as PolicyInfo;
                        var OpenActivityselectedItem = ActiveActivityList.SelectedItem as PolicyInfo;
                        if ((attachmentinfo.IsClosedActivity && ClosedActivityselectedItem == null) || (!attachmentinfo.IsClosedActivity && OpenActivityselectedItem == null))
                        {
                            var policyYr = policyYearLst.FirstOrDefault(x => x.Description == attachmentinfo.PolicyYear);
                            policyYr.IsSelected = true;
                            LstPolicyList.SelectedItem = policyYr;
                        }
                        if (attachmentinfo.PolicyType == "(none)")
                        {
                            RBCustomPolicyType.IsChecked = true;
                            lblPolicyType.Text = string.Empty;
                            policyTypeTextBlock.Text = string.Empty;
                        }
                        else
                        {
                            RBPolicyType.IsChecked = true;
                            lblPolicyType.Text = attachmentinfo.PolicyCode;
                            policyTypeTextBlock.Text = attachmentinfo.PolicyCode;
                            if (policyList != null)
                                policyList.DgPolicyType.SelectedItem = policyList.PolicyTypeFilteredItems.FirstOrDefault(x => x.PolicyTypeCode == attachmentinfo.PolicyCode);
                            policyTypeTextBlock.Visibility = Visibility.Visible;
                        }

                        if (attachmentinfo.DescriptionFrom == CUSTOM)
                            rbCustom.IsChecked = true;
                        else if (attachmentinfo.DescriptionFrom == USE_ACTIVITY)
                            rbActivityDec.IsChecked = true;
                        else if (attachmentinfo.DescriptionFrom == USE_EMAIL)
                            rbEmailDesc.IsChecked = true;
                        txtDesc.Text = attachmentinfo.Description;

                        if (attachToEpicMainPage != null && attachToEpicMainPage.multipleEmailAttachment != null)
                        {
                            if (attachmentinfo.DescriptionFrom == CUSTOM)
                                attachToEpicMainPage.multipleEmailAttachment.rbCustom.IsChecked = true;
                            else if (attachmentinfo.DescriptionFrom == USE_ACTIVITY)
                                attachToEpicMainPage.multipleEmailAttachment.rbActivityDec.IsChecked = true;
                            else if (attachmentinfo.DescriptionFrom == USE_EMAIL)
                                attachToEpicMainPage.multipleEmailAttachment.rbEmailDesc.IsChecked = true;
                        }

                        mainFolderComboBox.SelectedItem = Globals.ThisAddIn.MainFolderInfoCollection.FirstOrDefault(x => x.FolderId == attachmentinfo.FolderDetails.ParentFolderId);
                        subFolder1ComboBox.SelectedItem = SubFolder1InfoCollection.FirstOrDefault(x => x.FolderId == attachmentinfo.FolderDetails.FolderId);
                        Task.Delay(10);
                        subFolder2ComboBox.SelectedItem = SubFolder2InfoCollection.FirstOrDefault(x => x.FolderId == attachmentinfo.FolderDetails.SubFolderId);

                        if (!string.IsNullOrEmpty(attachmentinfo.ClientAccessible))
                        {
                            chkUntilDate.IsChecked = true;
                            DpAccesibility.SelectedDate = Convert.ToDateTime(attachmentinfo.ClientAccessible);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void btnFavouriteList_Click(object sender, RoutedEventArgs e)
        {
            //var openButton = sender as Button;
            //Point mousePositionInApp = Mouse.GetPosition(openButton);
            //Point mousePositionInScreenCoordinates = openButton.PointToScreen(mousePositionInApp);

            if (!BookMarkListPopupDetails.IsOpen)
                BookMarkListPopupDetails.IsOpen = true;
            GetAllFavouritesFromLocalDataBase();
            if (LstFavouriteList.Items.Count > 0)
                LstActiveClient.ScrollIntoView(LstFavouriteList.Items[0]);

            LstFavouriteList.SelectedItem = null;
            //   grdBackOverLay.Visibility = Visibility.Visible;

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseAllPopups();
        }



        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CloseAllPopups();
        }

        private void TbtnFavouites_Click(object sender, RoutedEventArgs e)
        {
            BookMarkListPopup.IsOpen = true;

            //   grdBackOverLay.Visibility = Visibility.Visible;
        }

        private void BtnPolicyType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblPolicyType.Text))
                {
                    policyList.DgPolicyType.SelectedItem = policyList.PolicyTypeInfoCollection.Where(x => x.PolicyTypeCode == lblPolicyType.Text).FirstOrDefault();
                    if (policyList.DgPolicyType.SelectedItem != null)
                        policyList.DgPolicyType.ScrollIntoView(policyList.DgPolicyType.SelectedItem);
                }
                if (OLA.ActiveInspector() != null)
                {
                    dynamic activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                    using (var officeWin32activeWindow = new OfficeWin32Window(activeWindow))
                    {
                        IntPtr outlookHwnd = officeWin32activeWindow.Handle;
                        WindowInteropHelper wih = new WindowInteropHelper(policyTypeWindow)
                        {
                            Owner = outlookHwnd
                        };
                    }

                }
                else if (attachToEpicMainPage != null && attachToEpicMainPage._multipleEmailAttachmentWindow != null)
                {
                    policyTypeWindow.Owner = attachToEpicMainPage._multipleEmailAttachmentWindow;
                }
                policyTypeWindow.ShowDialog();



                //PolicyTypePopup.IsOpen = true;
                //grdBackOverLay.Visibility = Visibility.Visible;
                // BtnPolicyTypeEvent?.Invoke(sender, e);
                // { System.Windows.Forms.Integration.AvalonAdapter}
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        private void PolicyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var selectedItem = PolicyList.SelectedItem as PolicyInfo;
            //if (selectedItem != null)
            //{
            //    lblPolicyType.Text = selectedItem.PolicyCode;
            //    PolicyTypePopup.IsOpen = false;
            //}
        }

        private void RBCustomPolicyType_Click(object sender, RoutedEventArgs e)
        {
            if (RBCustomPolicyType.IsChecked == true)
            {
                lblPolicyType.Text = string.Empty;
                policyTypeTextBlock.Text = string.Empty;
                //  policyTypeTextBlock.Visibility = Visibility.Collapsed;
                UpdateDescription();
                // PolicyList.SelectedItem = null;
            }
        }
        private void RefreshAddActivityWindow()
        {
            try
            {
                try
                {
                    addActivityWindow = new Window();
                    dynamic activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                    using (var officeWin32activeWindow = new OfficeWin32Window(activeWindow))
                    {
                        IntPtr outlookHwnd = officeWin32activeWindow.Handle;
                        WindowInteropHelper wih = new WindowInteropHelper(addActivityWindow)
                        {
                            Owner = outlookHwnd
                        };
                    }

                    addActivityWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    addActivityWindow.WindowStyle = WindowStyle.None;
                    addActivityWindow.AllowsTransparency = true;
                    addActivityWindow.Background = Brushes.Transparent;
                    Grid mainGrid = new Grid();
                    mainGrid.Children.Add(addactivityMainPage);
                    mainGrid.Effect = new DropShadowEffect
                    {
                        Color = (Color)ColorConverter.ConvertFromString("#0072C6"),
                        Direction = 320,
                        ShadowDepth = 0,
                        Opacity = 1
                    };
                    addActivityWindow.Content = mainGrid;
                    addActivityWindow.Height = 630;
                    addActivityWindow.Width = 780;

                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                }
                finally
                {
                    Logger.save();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
        }

        private void BtnAddActivity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (addactivityMainPage != null)
                {
                    if (Globals.ThisAddIn.ownerCodeCollection.Count > 0)
                    {
                        addactivityMainPage.addactivitySecondPage.ownerComboBox.ItemsSource = null;
                        addactivityMainPage.addactivitySecondPage.ownerComboBox.ItemsSource = Globals.ThisAddIn.ownerCodeCollection;
                    }

                    addactivityMainPage.RefreshDateControls();

                    if (addactivityMainPage.addactivitySecondPage != null)
                    {
                        var currentSelectedItem = addactivityMainPage.addactivitySecondPage?.updateComboBox.SelectedItem as HIB.Outlook.Model.ActivityCommonLookUpInfo;
                        if (currentSelectedItem == null)
                        {
                            currentSelectedItem = addactivityMainPage.addactivitySecondPage?.updateComboBox.Items[0] as HIB.Outlook.Model.ActivityCommonLookUpInfo;
                        }
                        if (currentSelectedItem != null)
                        {
                            if (string.Equals(currentSelectedItem.CommonLkpCode, "UEC", StringComparison.OrdinalIgnoreCase))
                            {
                                addactivityMainPage.addactivitySecondPage.lblEndDateMandatory.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                addactivityMainPage.addactivitySecondPage.lblEndDateMandatory.Visibility = Visibility.Collapsed;
                            }
                        }
                        LoadAddActivityCodeWithDesc();
                    }

                    if (Globals.ThisAddIn.CommonLookupCollection == null || Globals.ThisAddIn.CommonLookupCollection.Count <= 0)
                    {
                        GetAllCommonLookupFromLocalDatabase();
                    }
                    if (addactivityMainPage.addactivityFirstPage?.AddToActivityTypeComboBox.Items.Count == 0)
                        LoadAllValuesFromCommonLookup();

                    if (addactivityMainPage.addactivityMainPageGrid.RowDefinitions.Count >= 2)
                        addactivityMainPage.addactivityMainPageGrid.RowDefinitions[1].Height = new GridLength(0);
                    addactivityMainPage.addactivityFirstPage.Visibility = Visibility.Visible;
                    addactivityMainPage.addactivitySecondPage.Visibility = Visibility.Collapsed;
                    addactivityMainPage.addactivitySecondPage.accessLevelDescription.Text = string.Empty;
                    addactivityMainPage.addactivityFirstPage.btnAddActivityContinue.IsEnabled = false;
                    addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.IsChecked = false;
                    BtnAddAcctivityEvent?.Invoke(sender, e);
                    //AddActivityPopup.IsOpen = true;
                    //scrAddActivity.Visibility = Visibility.Collapsed;
                    //grdAddActivity.Visibility = Visibility.Visible;
                    //grdBackOverLay.Visibility = Visibility.Visible;

                    if (Globals.ThisAddIn.CommonLookupCollection.Count > 0)
                        addactivityMainPage.addactivityFirstPage.AddToActivityTypeComboBox.SelectedItem = Globals.ThisAddIn.CommonLookupCollection.Where(m => m.CommonLkpCode == "P").Select(m => m.CommonLkpName).FirstOrDefault();

                    var selectedItem = addactivityMainPage.addactivityFirstPage.AddToActivityTypeComboBox.SelectedItem as string;
                    if (!string.IsNullOrEmpty(selectedItem))
                    {
                        GetActivity(selectedItem);
                    }


                    var currentLoggedUser = ThisAddIn.EmployeeLookupCode;
                    if (Globals.ThisAddIn.ownerCodeCollection.Where(m => m.Code == currentLoggedUser).Any())
                    {
                        var currentloggedOwner = Globals.ThisAddIn.ownerCodeCollection.FirstOrDefault(m => m.Code == currentLoggedUser);
                        if (currentLoggedUser != null)
                        {
                            addactivityMainPage.addactivitySecondPage.ownerComboBox.SelectedItem = currentloggedOwner;
                        }
                    }
                    addactivityMainPage.addactivitySecondPage.accessLevelComboBox.SelectedItem = Globals.ThisAddIn.CommonLookupCollection.FirstOrDefault(m => m.CommonLkpTypeCode == "ActivityAccessLevel" && m.CommonLkpCode == "PUB");
                    if (OLA.ActiveInspector() != null)
                    {
                        dynamic activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                        using (var officeWin32activeWindow = new OfficeWin32Window(activeWindow))
                        {
                            IntPtr outlookHwnd = officeWin32activeWindow.Handle;
                            WindowInteropHelper wih = new WindowInteropHelper(addActivityWindow)
                            {
                                Owner = outlookHwnd
                            };
                        }
                        //dynamic activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                        //IntPtr outlookHwnd = new OfficeWin32Window(activeWindow).Handle;
                        //WindowInteropHelper wih = new WindowInteropHelper(addActivityWindow)
                        //{
                        //    Owner = outlookHwnd
                        //};
                    }
                    else if (attachToEpicMainPage != null && attachToEpicMainPage._multipleEmailAttachmentWindow != null)
                    {
                        addActivityWindow.Owner = attachToEpicMainPage._multipleEmailAttachmentWindow;
                    }

                    addActivityWindow.ShowDialog();

                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }



        private void btnActivitySearch_Click(object sender, RoutedEventArgs e)
        {
            StartSearchActivities();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DgPolicyType_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void PolicyTypePopup_Closed(object sender, EventArgs e)
        {
            grdBackOverLay.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Tag != null)
                {
                    attachToEpicMainPage = this.Tag as AttachToEpicMainPage;
                }
                // AttachAssistBtnEnabled();
                ThisAddIn.CheckWindowsServiceStatus(this);
                if (ThisAddIn.IsValidEmployee)
                {
                    IsEnabled = true;
                    ThisAddIn_IsValidEmployeEvent(ThisAddIn.IsValidEmployee);
                }
                //TblError.Text = ConfigurationManager.AppSettings["DataSync"]?.ToString();
                ////TblError.Visibility = Visibility.Visible;
                //if (!ThisAddIn.IsValidEmployee)
                //{
                //    grdError.Visibility = Visibility.Visible;
                //    TblError.Text = ConfigurationManager.AppSettings["NoAccess"]?.ToString();
                //    //Visibility = Visibility.Collapsed;
                //    IsEnabled = false;
                //}
                //else
                //{
                //    grdError.Visibility = Visibility.Collapsed;
                //    TblError.Visibility = Visibility.Collapsed;
                //}
                Task.Run(async () =>
                {
                    await LoadAllControls();
                });
                ControlsVisibilityBasedOnMailMode();
                UpdateFailedCountForAttachment();
                if (attachToEpicMainPage == null && sendAndAttachGrid.Visibility == Visibility.Collapsed)
                {
                    if (Globals.ThisAddIn.CurrentExplorer.Selection.Count == 0)
                    {
                        grdError.Visibility = Visibility.Visible;
                        TblError.Visibility = Visibility.Visible;
                        TblError.Text = ConfigurationManager.AppSettings["NoMailSelection"]?.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        public void RefreshFailedListAttachments()
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                var failedEmailInfos = GetFailedAttachmentsFromLocalDB();
                if (Globals.ThisAddIn.failedAttachments != null)
                {
                    Globals.ThisAddIn.failedAttachments.LstFailedAttachment.ItemsSource = null;
                    Globals.ThisAddIn.failedAttachments.LstFailedAttachment.ItemsSource = failedEmailInfos;
                    if (failedEmailInfos.Count > 0)
                    {
                        if (Globals.ThisAddIn.failedAttachments.LstFailedAttachment.Items.Count > 0)
                        {
                            Globals.ThisAddIn.failedAttachments.LstFailedAttachment.SelectedIndex = 0;
                        }
                        Globals.ThisAddIn.failedAttachments.rightPartMainGrid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Globals.ThisAddIn.failedAttachments.rightPartMainGrid.Visibility = Visibility.Collapsed;
                    }
                    Globals.ThisAddIn.failedAttachments.failureTextBlock.Visibility = Visibility.Collapsed;
                }
            }));
        }

        public void UpdateFailedCountForAttachment([Optional] Int32? deleteAttachmentForUI)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                var failedCount = GetFailedAttachmentsCount();
                if (deleteAttachmentForUI != null)
                {
                    failedCount = deleteAttachmentForUI.Value;
                }
                var formInstances = Globals.ThisAddIn.PushToEpicFormRegionItem.FormInstanceCount;
                for (int i = 0; i < formInstances; i++)
                {
                    PushToEpicFormRegion form = (PushToEpicFormRegion)Globals.ThisAddIn.PushToEpicFormRegionItem.FormInstances(i);
                    if (failedCount != 0 && form != null)
                    {
                        form.attachmentControls1.FailedNotification.Text = failedCount.ToString();
                        form.attachmentControls1.failedNotificationBorder.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        form.attachmentControls1.failedNotificationBorder.Visibility = Visibility.Collapsed;
                    }
                }
                if (failedCount != 0)
                {
                    FailedNotification.Text = failedCount.ToString();
                    failedNotificationBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    failedNotificationBorder.Visibility = Visibility.Collapsed;
                }
            }));
        }


        private async Task LoadAllControls()
        {
            try
            {
                await Dispatcher.BeginInvoke((Action)(() =>
                {
                    DpAccesibility.DisplayDateStart = DateTime.Now;
                    if (Globals.ThisAddIn.ClientInfoCollection == null || Globals.ThisAddIn.ClientInfoCollection.Count <= 0)
                    {
                        Globals.ThisAddIn.ClientInfoCollection = GetClientInformationFromSQLite(String.Format(_listOfClientsQuery, ThisAddIn.EmployeeLookupCode));
                        OnPropertyChanged("MyActiveClientFilteredItems");
                        OnPropertyChanged("MyInActiveClientFilteredItems");
                    }
                    if (mainFolderComboBox.Items.Count <= 0 || (Globals.ThisAddIn.MainFolderInfoCollection != null && Globals.ThisAddIn.MainFolderInfoCollection.Count == 0))
                    {
                        Globals.ThisAddIn.MainFolderInfoCollection = GetFoldersFromSQLite(_mainFolderQuery);
                        Globals.ThisAddIn.MainFolderInfoCollection = Globals.ThisAddIn.MainFolderInfoCollection.OrderBy(m => m.FolderName).ToList();
                        if (Globals.ThisAddIn.MainFolderInfoCollection.Count > 0)
                            mainFolderComboBox.ItemsSource = Globals.ThisAddIn.MainFolderInfoCollection;
                    }
                    UpdateFolders();
                    PrepareAddActivityWindow();
                    PreparePolicyListTypeWindow();
                    if (Globals.ThisAddIn.CommonLookupCollection == null || Globals.ThisAddIn.CommonLookupCollection.Count <= 0)
                    {
                        GetAllCommonLookupFromLocalDatabase();
                    }
                    if (Globals.ThisAddIn.ActivityClientContactInfoCollection == null || (Globals.ThisAddIn.ActivityClientContactInfoCollection.Count <= 0))
                        GetAllActivityClientContactInfoFromLocalDatabase();
                    GetAllFavouritesFromLocalDataBase();
                    LoadAllValuesFromCommonLookup();

                    LoadAllEmployeeWithAgency();
                    LoadAddActivityCodeWithDesc();
                    if (Globals.ThisAddIn.ownerCodeCollection == null || Globals.ThisAddIn.ownerCodeCollection.Count <= 0)
                        LoadOwnerCodeList();
                    OnPropertyChanged("MyActiveClientFilteredItems");
                    OnPropertyChanged("MyInActiveClientFilteredItems");
                }));
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }
        private void rbEmailDesc_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentFormRegion = Globals.ThisAddIn.PushToEpicFormRegionItem.GetCurrentForm() as PushToEpicFormRegion;
                //Microsoft.Office.Interop.Outlook.Application outlookApp = new Microsoft.Office.Interop.Outlook.Application();
                var activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                if (activeWindow is OutlookNS.Explorer && currentFormRegion != null && !ThisAddIn.isInlineResponseActive)
                {
                    var items = Globals.ThisAddIn.Application.ActiveExplorer()?.Selection;
                    if (items.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            try
                            {
                                if (item is OutlookNS.MailItem)
                                {
                                    var firstSelectedMailItem = item as OutlookNS.MailItem;
                                    txtDesc.Text = firstSelectedMailItem.Subject;

                                    Marshal.ReleaseComObject(firstSelectedMailItem);
                                    firstSelectedMailItem = null;

                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                            }
                        }
                    }
                }
                else if (activeWindow is OutlookNS.Inspector)
                {
                    var currentItem = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as OutlookNS.MailItem;
                    txtDesc.Text = currentItem.Subject;

                    Marshal.ReleaseComObject(currentItem);
                    currentItem = null;
                }
                else
                {
                    if (ThisAddIn.isInlineResponseActive)
                    {
                        txtDesc.Text = (explorer?.ActiveInlineResponse as OutlookNS.MailItem).Subject;
                    }
                    else
                    {
                        GetAllSelectedEmails();
                        if (_mailItems.Count > 0)
                        {
                            UpdateDescription();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void rbCustom_Checked(object sender, RoutedEventArgs e)
        {
            if (txtDesc != null)
                txtDesc.Text = "";
        }

        private void rbActivityDec_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateDescription();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }


        private void LstActiveClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //FailureAlert.BeginInit();
                //var storyBoard = FailureAlert.Resources[0] as Storyboard;
                //FailureAlert.BeginStoryboard(storyBoard);
                var selectActiveClient = LstActiveClient.SelectedItem as ClientInfo;
                RefreshActivitiesForSelectedClient(selectActiveClient);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void RefreshActivitiesForSelectedClient(ClientInfo selectedClient)
        {
            try
            {
                if (selectedClient != null)
                {
                    if (addactivityMainPage != null)
                    {
                        addactivityMainPage.addactivityFirstPage.AddActivityClientCodeLabel.Content = selectedClient.EpicCode;
                        addactivityMainPage.addactivityFirstPage.AddActivityClientDescLabel.Content = selectedClient.ClientName;
                    }
                    BtnAddActivity.IsEnabled = true;
                    GetActivitiesFromClient(selectedClient);
                }
                else
                {
                    OpenActivitiesInfoCollection = new List<PolicyInfo>();
                    ClosedActivitiesInfoCollection = new List<PolicyInfo>();
                    // OnPropertyChanged("OpenActivitiesFilteredItems");
                    ActiveActivityList.ItemsSource = null;
                    ActiveActivityList.ItemsSource = OpenActivitiesFilteredItems;
                    //OnPropertyChanged("ClosedActivitiesFilteredItems");
                    InActiveActivityList.ItemsSource = null;
                    InActiveActivityList.ItemsSource = ClosedActivitiesFilteredItems;
                }
                EnableSaveButons();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void LstInActiveClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectInActiveClient = LstInActiveClient.SelectedItem as ClientInfo;
                RefreshActivitiesForSelectedClient(selectInActiveClient);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        private void LstPolicyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UpdateDescription();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void btnAttachmentOnly_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                requiredFieldAlert.Visibility = Visibility.Collapsed;
                var mailItem = GetAllSelectedEmails();
                if (mailItem.Count > 0)
                {
                    if (ValidateAllFields())
                    {
                        mailItem.ForEach(x =>
                        {
                            var attachmentIdentifier = x.MailItem.UserProperties.Add("AttachmentIdentifier", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                            attachmentIdentifier.Value = Guid.NewGuid().ToString();
                            AddACategory(x.MailItem);
                            x.MailItem.Save();
                            //var processingcopy = x.MailItem.Copy() as OutlookNS.MailItem;
                            //var zfolder = GetCustomFolder(ProcessingfolderName);
                            //processingcopy.Move(zfolder);
                        });
                        _mailItems = mailItem;
                        SaveAttachmentInfo(false, false);
                        //await Task.Delay(2000);
                        System.Threading.Thread.Sleep(1000);
                        ShowStatus("Attachment will be done shortly");
                        ResetAllData();
                    }
                    else
                    {
                        requiredFieldAlert.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    requiredFieldAlert.Content = "Please select a mail item";
                    requiredFieldAlert.Visibility = Visibility.Visible;
                }




            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        private async void btnAttachmentAndDeleteOnly_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                requiredFieldAlert.Visibility = Visibility.Collapsed;
                var mailItem = GetAllSelectedEmails();
                if (mailItem.Count > 0)
                {
                    if (ValidateAllFields())
                    {
                        //var folder = GetCustomFolder(ProcessingfolderName);
                        mailItem.ForEach(x =>
                        {
                            var attachmentIdentifier = x.MailItem.UserProperties.Add("AttachmentIdentifier", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                            attachmentIdentifier.Value = Guid.NewGuid().ToString();
                            AddACategory(x.MailItem);
                            x.MailItem.Save();
                            x.MailItem.Move(OLA.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderDeletedItems));
                            //var deletecopy = x.MailItem.Copy() as OutlookNS.MailItem; deletecopy.Move(OLA.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderDeletedItems));
                            //x.MailItem.Move(folder);
                        });
                        _mailItems = mailItem;
                        SaveAttachmentInfo(true, false);
                        await Task.Delay(1000);
                        await Dispatcher.BeginInvoke((Action)(() =>
                        {
                            ShowStatus(Attachment_Status);
                            ResetAllData();
                        }));
                    }
                    else
                    {
                        requiredFieldAlert.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    requiredFieldAlert.Content = "Please select a mail item";
                    requiredFieldAlert.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }


        internal void AddACategory(OutlookNS.MailItem mailItem)
        {
            try
            {
                OutlookNS.Categories categories = Globals.ThisAddIn.Application.Session.Categories;
                string categoryName = "Attached to Epic";
                if (OLA.Session.Categories[categoryName] == null)
                {
                    categories.Add(categoryName, OutlookNS.OlCategoryColor.olCategoryColorPeach);
                }
                OutlookNS.Category category = OLA.Session.Categories[categoryName];
                mailItem.Categories = category.Name;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }


        //private void ShowSatus(string message)
        //{
        //    TextBlock text = new TextBlock();
        //    text.Foreground = System.Windows.Media.Brushes.Red;
        //    text.Text = message;
        //    text.Style = this.TryFindResource("FaderStyle") as Style;
        //    FailureAlertGrid.Children.Clear();
        //    FailureAlertGrid.Children.Add(text);
        //}

        private void TbtnAttachmentAssist_Loaded(object sender, RoutedEventArgs e)
        {
            //if (ThisAddIn.IsComposeOpen)
            //{
            //    ThisAddIn.IsComposeOpen = false;
            //}
            //else
            AttachAssistBtnEnabled();

        }
        private void AttachAssistBtnEnabled()
        {
            try
            {
                var currentIntPtr = OfficeWin32Window.GetActiveWindow();
                StringBuilder s = new StringBuilder(50);
                int count = 50;
                OfficeWin32Window.GetWindowText(currentIntPtr, s, count);
                var windowTitle = s.ToString();
                var currentItem = Globals.ThisAddIn.Application.ActiveInspector()?.CurrentItem as OutlookNS.MailItem;
                var activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                var currentFormRegion = Globals.ThisAddIn.PushToEpicFormRegionItem.GetCurrentForm() as PushToEpicFormRegion;
                if (activeWindow is OutlookNS.Explorer && currentFormRegion != null && currentItem == null && !string.Equals(windowTitle.Trim(), "Untitled - Message (HTML)"))
                {
                    if (!string.IsNullOrEmpty(GetAllSelectedEmails().FirstOrDefault()?.Identifier))
                    {
                        if (attachToEpicMainPage == null)
                        {
                            TbtnAttachmentAssistActive.Visibility = Visibility.Visible;
                            TbtnAttachmentAssist.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            TbtnAttachmentAssistActive.Visibility = Visibility.Collapsed;
                            TbtnAttachmentAssist.Visibility = Visibility.Visible;
                        }

                    }
                }
                else if (activeWindow is OutlookNS.Inspector)
                {
                    var currentItemNewCompose = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as OutlookNS.MailItem;
                    if (!string.IsNullOrEmpty(GetIdentifierFromMailItem(currentItemNewCompose)))
                    {
                        if (attachToEpicMainPage == null)
                        {
                            TbtnAttachmentAssistActive.Visibility = Visibility.Visible;
                            TbtnAttachmentAssist.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            TbtnAttachmentAssistActive.Visibility = Visibility.Collapsed;
                            TbtnAttachmentAssist.Visibility = Visibility.Visible;
                        }

                    }
                }
                else
                {
                    var currentItemInlineMail = explorer?.ActiveInlineResponse as OutlookNS.MailItem;
                    if (!string.IsNullOrEmpty(GetIdentifierFromMailItem(currentItemInlineMail)))
                    {
                        if (attachToEpicMainPage == null)
                        {
                            TbtnAttachmentAssistActive.Visibility = Visibility.Visible;
                            TbtnAttachmentAssist.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            TbtnAttachmentAssistActive.Visibility = Visibility.Collapsed;
                            TbtnAttachmentAssist.Visibility = Visibility.Visible;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            ResetAllData();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(SyncFilePath))
            {
                File.Create(SyncFilePath);
                MessageBox.Show("Data sync in progress. This may take a few minutes. When complete, you will be notified.");
                var ValidEmployees = new List<string>();
                //SyncLocal syncLocal = new SyncLocal(ConfigurationManager.ConnectionStrings["HIBOutlookSQLite"].ConnectionString);
                SyncLocal syncLocal = new SyncLocal();
                syncLocal.SyncCompleted -= SyncLocal_SyncCompleted;
                syncLocal.SyncCompleted += SyncLocal_SyncCompleted;
                Task.Run(() =>
                {
                    ValidEmployees = syncLocal.GetValidEmployees();
                    var deltaSyncObjects = syncLocal.GetDeltaSyncObjects(ValidEmployees, null, false);
                    syncLocal.SyncOnlyClient(ValidEmployees, deltaSyncObjects);
                    syncLocal.SyncData(ValidEmployees, deltaSyncObjects);
                });
            }
            else
            {
                MessageBox.Show("Background Sync in progress, This may take a few minutes.");
            }
        }



        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string favouriteName = favouriteNameTextBox.Text;
                if (!string.IsNullOrEmpty(favouriteName))
                {
                    if (!CheckFavouriteAlreadyExists(favouriteName))
                    {
                        var favouriteInfo = SaveFavouritesInfoToLocalDataBase();
                        Thread.Sleep(1000);
                        if (!string.IsNullOrEmpty(favouriteInfo.FavourtieName))
                        {
                            BookMarkListPopup.IsOpen = false;
                            GetAllFavouritesFromLocalDataBase();
                            if (FavouriteInfoCollection.Where(m => m.FavourtieName == favouriteInfo.FavourtieName).Count() == 0)
                                FavouriteInfoCollection.Add(favouriteInfo);
                            FavouriteInfoCollection = FavouriteInfoCollection.OrderByDescending(m => m.ModifiedDate).ToList();
                            LstFavouriteList.ItemsSource = null;
                            LstFavouriteList.ItemsSource = FavouriteInfoCollection;

                            favouriteNameTextBox.Text = "";
                            ShowStatus(string.Format("{0} successfully added ", favouriteName));
                        }
                        else
                        {
                            ShowStatus(string.Format("{0} failed to add. Please try again. ", favouriteName), true);
                        }
                    }
                    else
                    {
                        ShowStatus(Favorite_Exists, true);
                    }
                }
                else
                {
                    ShowStatus(Valid_Favorite, true);

                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }



        }
        private void waterMarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                if (waterMarkTextBox.Text.Length > 0 && waterMarkTextBox.Text.Length <= 2)
                {
                    validationFilterLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    validationFilterLabel.Visibility = Visibility.Collapsed;
                }
                if (string.IsNullOrEmpty(waterMarkTextBox.Text?.Trim()))
                {
                    OnPropertyChanged("MyActiveClientFilteredItems");
                    OnPropertyChanged("MyInActiveClientFilteredItems");
                    if ((Tab.SelectedIndex == 0 && LstActiveClient.Items.Count == 0) || (Tab.SelectedIndex == 1 && LstInActiveClient.Items.Count == 0))
                    {
                        BtnAddActivity.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }
        private void waterMarkTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    StartSearchClients();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }


        }
        private async void btnSendAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OutlookNS.MailItem outlookMsg = null;
                requiredFieldAlert.Visibility = Visibility.Collapsed;
                var activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                if (ValidateAllFields())
                {
                    if (activeWindow is OutlookNS.Explorer && ThisAddIn.isInlineResponseActive)
                    {
                        SendReply();
                    }
                    else if (Globals.ThisAddIn.Application.ActiveInspector() != null)
                    {
                        outlookMsg = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as OutlookNS.MailItem;
                        await SendMail(outlookMsg, false);
                    }
                    else
                    {
                        SendReply();
                    }

                }
                else
                {
                    requiredFieldAlert.Visibility = Visibility.Visible;
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }
        private async Task SendMail(OutlookNS.MailItem outlookMsg, bool isSendAttachDelete)
        {
            try
            {
                if (!string.IsNullOrEmpty(outlookMsg.To))
                {
                    var attachmentIdentifier = outlookMsg.UserProperties.Add("AttachmentIdentifier", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                    attachmentIdentifier.Value = Guid.NewGuid().ToString();
                    AddACategory(outlookMsg);
                    outlookMsg.Save();
                    if (isSendAttachDelete)
                    {
                        _IsSendAttachAndDelete = true;
                        var isSAD = outlookMsg.UserProperties.Add("IsSendAttachDelete", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                        isSAD.Value = true.ToString();
                        _IsDeleteAttached = true;
                    }
                    var guidAddedMail = AddGuidInSelectedMailItem(outlookMsg);
                    if (string.IsNullOrEmpty(outlookMsg.Subject))
                    {
                        outlookMsg.Subject = " ";
                    }
                    //(guidAddedMail.Copy() as OutlookNS.MailItem).Move(GetCustomFolder(ProcessingfolderName));
                    guidAddedMail.UserProperties.Add("IsComposeMail", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                    guidAddedMail.Send();
                    await Task.Delay(1000);

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }




        //private void SetCurrentFolder(string folderName)
        //{

        //    OLA.ActiveExplorer().CurrentFolder = (OutlookNS.MAPIFolder)OLA.ActiveExplorer().Session.GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderInbox);
        //    //try
        //    //{
        //    //    OLA.ActiveExplorer().CurrentFolder = inBox.Folders[folderName];
        //    //    OLA.ActiveExplorer().CurrentFolder.Display();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    log.Error(ex);
        //    //}
        //}


        void Forward_Message_Inspector(Microsoft.Office.Interop.Outlook.Inspector Inspector)
        {
            //how do I check here if current window is a forward message window?
            //and then do something
        }

        private async void btnSendDeleteAndAttach_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OutlookNS.MailItem outlookMsg = null;
                var activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                requiredFieldAlert.Visibility = Visibility.Collapsed;
                if (ValidateAllFields())
                {
                    if (activeWindow is OutlookNS.Explorer && ThisAddIn.isInlineResponseActive)
                    {
                        SendReply(true);
                    }
                    else if (Globals.ThisAddIn.Application.ActiveInspector() != null)
                    {
                        outlookMsg = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as OutlookNS.MailItem;
                        await SendMail(outlookMsg, true);
                    }
                    else
                    {
                        SendReply(true);
                    }
                }
                else
                {
                    requiredFieldAlert.Visibility = Visibility.Visible;
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }



        private void mainFolderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var folderInfo = mainFolderComboBox.SelectedItem as FolderInfo;
                if (folderInfo != null && folderInfo.FolderId > 0)
                {
                    subFolder1ComboBox.IsEnabled = true;
                    string folderQuery = string.Format(_subfolderQuery, folderInfo.FolderId);
                    SubFolder1InfoCollection = GetFoldersFromSQLite(folderQuery);
                    SubFolder1InfoCollection = SubFolder1InfoCollection.OrderBy(m => m.FolderName).ToList();
                    subFolder2ComboBox.ItemsSource = null;
                    subFolder2ComboBox.SelectedItem = null;
                    subFolder1ComboBox.ItemsSource = null;
                    subFolder1ComboBox.SelectedItem = null;
                    if (SubFolder1InfoCollection.Count > 0)
                    {
                        subFolder1ComboBox.ItemsSource = SubFolder1InfoCollection;

                    }
                }
                EnableSaveButons();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }




        private void subFolder1ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var folderInfo = subFolder1ComboBox.SelectedItem as FolderInfo;

                if (folderInfo != null && folderInfo.FolderId > 0)
                {
                    string folderQuery = string.Format(_subfolderQuery, folderInfo.FolderId);
                    SubFolder2InfoCollection = GetFoldersFromSQLite(folderQuery);
                    SubFolder2InfoCollection = SubFolder2InfoCollection.OrderBy(m => m.FolderName).ToList();
                    subFolder2ComboBox.IsEnabled = true;
                    subFolder2ComboBox.ItemsSource = null;
                    subFolder2ComboBox.SelectedItem = null;
                    if (SubFolder2InfoCollection.Count > 0)
                    {
                        subFolder2ComboBox.ItemsSource = SubFolder2InfoCollection;

                    }
                }
                EnableSaveButons();


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void activityFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (activityFilterTextBox.Text.Length > 0 && activityFilterTextBox.Text.Length <= 2)
                {
                    validationFilterLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    validationFilterLabel.Visibility = Visibility.Collapsed;
                }
                if (string.IsNullOrEmpty(activityFilterTextBox.Text?.Trim()))
                {
                    StartSearchActivities();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void activityFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    StartSearchActivities();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }




        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            favouriteNameTextBox.Text = string.Empty;
            BookMarkListPopup.IsOpen = false;
        }

        private void LstFavouriteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var favouriteInfo = LstFavouriteList.SelectedItem as Model.FavouriteInfo;
                if (favouriteInfo != null)
                {
                    BookMarkListPopupDetails.IsOpen = false;
                    var isErrorOccured = false;
                    var InActiveActivity = InActiveActivityList.SelectedItem as PolicyInfo;
                    var ActiveActivity = ActiveActivityList.SelectedItem as PolicyInfo;

                    activityFilterTextBox.Clear();

                    if (favouriteInfo.UniqEntity > 0)
                    {
                        if (favouriteInfo.IsActiveClient == 1)
                        {

                            var favouriteClient = Globals.ThisAddIn.ClientInfoCollection.FirstOrDefault(x => x.IsActive && x.ClientId == favouriteInfo.UniqEntity);
                            if (favouriteClient != null)
                            {
                                LstActiveClient.SelectedItem = null;
                                LstActiveClient.SelectedItem = favouriteClient;
                                var selectedClient = LstActiveClient.SelectedItem as ClientInfo;
                                if (LstActiveClient.SelectedItem == null || selectedClient != null && favouriteClient.EpicCode != selectedClient.EpicCode)
                                {
                                    Tab.SelectedIndex = 0;
                                    waterMarkTextBox.Text = favouriteClient.EpicCode;
                                    StartSearchClients();
                                    LstActiveClient.SelectedItem = null;
                                    LstActiveClient.SelectedItem = favouriteClient;
                                }
                                LstActiveClient.ScrollIntoView(LstActiveClient.SelectedItem);
                            }
                            else
                            {
                                isErrorOccured = true;
                            }

                        }
                        else if (favouriteInfo.IsActiveClient == 0)
                        {
                            var favouriteClient = Globals.ThisAddIn.ClientInfoCollection.FirstOrDefault(x => !x.IsActive && x.ClientId == favouriteInfo.UniqEntity);
                            if (favouriteClient != null)
                            {
                                LstInActiveClient.SelectedItem = null;
                                LstInActiveClient.SelectedItem = favouriteClient;
                                var selectedClient = LstInActiveClient.SelectedItem as ClientInfo;
                                if (LstInActiveClient.SelectedItem == null || selectedClient != null && favouriteClient.EpicCode != selectedClient.EpicCode)
                                {
                                    Tab.SelectedIndex = 1;
                                    waterMarkTextBox.Text = favouriteClient.EpicCode;
                                    StartSearchClients();
                                    LstInActiveClient.SelectedItem = null;
                                    LstInActiveClient.SelectedItem = favouriteClient;
                                }
                                LstInActiveClient.ScrollIntoView(LstInActiveClient.SelectedItem);
                            }
                            else
                            {
                                isErrorOccured = true;
                            }

                        }
                    }



                    if (favouriteInfo.UniqActivity > 0)
                    {
                        if (favouriteInfo.IsClosedActivity == 1)
                        {
                            ActivitiesTab.SelectedIndex = 1;
                            if (ClosedActivitiesInfoCollection != null)
                            {
                                InActiveActivityList.SelectedItem = ClosedActivitiesInfoCollection.FirstOrDefault(x => x.ActivityId == favouriteInfo.UniqActivity);
                                if (InActiveActivityList.SelectedItem != null)
                                    InActiveActivityList.ScrollIntoView(InActiveActivityList.SelectedItem);
                                else
                                    isErrorOccured = true;
                            }
                            else
                            {
                                isErrorOccured = true;
                            }
                        }
                        else if (favouriteInfo.IsClosedActivity == 0)
                        {
                            ActivitiesTab.SelectedIndex = 0;
                            if (OpenActivitiesInfoCollection != null)
                            {
                                ActiveActivityList.SelectedItem = OpenActivitiesInfoCollection.FirstOrDefault(x => x.ActivityId == favouriteInfo.UniqActivity);
                                if (ActiveActivityList.SelectedItem != null)
                                    ActiveActivityList.ScrollIntoView(ActiveActivityList.SelectedItem);
                                else
                                    isErrorOccured = true;
                            }
                            else
                            {
                                isErrorOccured = true;
                            }

                        }
                    }
                    else if (favouriteInfo.UniqActivity == 0 && !string.IsNullOrEmpty(favouriteInfo.ActivityGuid))
                    {
                        if (favouriteInfo.IsClosedActivity == 1)
                        {
                            ActivitiesTab.SelectedIndex = 1;
                            if (ClosedActivitiesInfoCollection != null)
                            {
                                InActiveActivityList.SelectedItem = ClosedActivitiesInfoCollection.FirstOrDefault(x => x.ActivityGuid == favouriteInfo.ActivityGuid);
                                if (InActiveActivityList.SelectedItem != null)
                                    InActiveActivityList.ScrollIntoView(InActiveActivityList.SelectedItem);
                                else
                                    isErrorOccured = true;
                            }
                            else
                            {
                                isErrorOccured = true;
                            }
                        }
                        else if (favouriteInfo.IsClosedActivity == 0)
                        {
                            ActivitiesTab.SelectedIndex = 0;
                            if (OpenActivitiesInfoCollection != null)
                            {
                                ActiveActivityList.SelectedItem = OpenActivitiesInfoCollection.FirstOrDefault(x => x.ActivityGuid == favouriteInfo.ActivityGuid);
                                if (ActiveActivityList.SelectedItem != null)
                                    ActiveActivityList.ScrollIntoView(ActiveActivityList.SelectedItem);
                                else
                                    isErrorOccured = true;
                            }
                            else
                            {
                                isErrorOccured = true;
                            }

                        }

                    }
                    else
                    {
                        if (ActiveActivity != null)
                        {
                            ActiveActivityList.SelectedItem = ActiveActivity;
                            if (ActiveActivityList.SelectedItem != null)
                                ActiveActivityList.ScrollIntoView(ActiveActivityList.SelectedItem);
                        }
                        else
                        {
                            InActiveActivityList.SelectedItem = InActiveActivity;
                            if (InActiveActivityList.SelectedItem != null)
                                InActiveActivityList.ScrollIntoView(InActiveActivityList.SelectedItem);
                        }

                    }


                    var ClosedActivityselectedItem = InActiveActivityList.SelectedItem as PolicyInfo;
                    var OpenActivityselectedItem = ActiveActivityList.SelectedItem as PolicyInfo;
                    if ((favouriteInfo.IsClosedActivity == 1 && ClosedActivityselectedItem == null) || (favouriteInfo.IsClosedActivity == 0 && OpenActivityselectedItem == null))
                    {
                        var policyYr = policyYearLst.FirstOrDefault(x => x.Description == favouriteInfo.PolicyYear);
                        policyYr.IsSelected = true;
                        LstPolicyList.SelectedItem = policyYr;
                    }

                    if (favouriteInfo.PolicyType == "(none)")
                    {
                        RBCustomPolicyType.IsChecked = true;
                        lblPolicyType.Text = string.Empty;
                        policyTypeTextBlock.Text = string.Empty;
                    }
                    else
                    {
                        RBPolicyType.IsChecked = true;
                        lblPolicyType.Text = favouriteInfo.PolicyType;
                        policyTypeTextBlock.Text = favouriteInfo.PolicyType;
                        if (policyList != null)
                            policyList.DgPolicyType.SelectedItem = policyList.PolicyTypeFilteredItems.FirstOrDefault(x => x.PolicyTypeCode == favouriteInfo.PolicyType);
                        policyTypeTextBlock.Visibility = Visibility.Visible;
                    }


                    if (favouriteInfo.DescriptionType == CUSTOM)
                        rbCustom.IsChecked = true;
                    else if (favouriteInfo.DescriptionType == USE_ACTIVITY)
                        rbActivityDec.IsChecked = true;
                    else if (favouriteInfo.DescriptionType == USE_EMAIL)
                        rbEmailDesc.IsChecked = true;

                    txtDesc.Text = favouriteInfo.Description;

                    if (attachToEpicMainPage != null && attachToEpicMainPage.multipleEmailAttachment != null)
                    {
                        if (favouriteInfo.DescriptionType == CUSTOM)
                            attachToEpicMainPage.multipleEmailAttachment.rbCustom.IsChecked = true;
                        else if (favouriteInfo.DescriptionType == USE_ACTIVITY)
                            attachToEpicMainPage.multipleEmailAttachment.rbActivityDec.IsChecked = true;
                        else if (favouriteInfo.DescriptionType == USE_EMAIL)
                            attachToEpicMainPage.multipleEmailAttachment.rbEmailDesc.IsChecked = true;
                    }
                    var mainFolderCollection = mainFolderComboBox.Items.SourceCollection as List<FolderInfo>;
                    mainFolderComboBox.SelectedItem = mainFolderCollection.FirstOrDefault(x => x.FolderId == favouriteInfo.FolderId);
                    subFolder1ComboBox.SelectedItem = null;
                    subFolder1ComboBox.SelectedItem = SubFolder1InfoCollection.FirstOrDefault(x => x.FolderId == favouriteInfo.SubFolder1Id);
                    subFolder2ComboBox.SelectedItem = null;
                    subFolder2ComboBox.SelectedItem = SubFolder2InfoCollection.FirstOrDefault(x => x.FolderId == favouriteInfo.SubFolder2Id);

                    if ((mainFolderComboBox.SelectedItem == null && favouriteInfo.FolderId != 0) || (subFolder1ComboBox.SelectedItem == null && favouriteInfo.SubFolder1Id != 0) || (subFolder2ComboBox.SelectedItem == null && favouriteInfo.SubFolder2Id != 0))
                    {
                        isErrorOccured = true;
                    }

                    if (!string.IsNullOrEmpty(favouriteInfo.ClientAccessibleDate))
                    {
                        chkUntilDate.IsChecked = true;
                        var selectedDate = Convert.ToDateTime(favouriteInfo.ClientAccessibleDate);
                        DpAccesibility.SelectedDate = selectedDate;
                        if (selectedDate.Date < DateTime.Now.Date)
                        {
                            ShowStatus("Please enter a valid future date");
                        }
                    }
                    else
                    {
                        chkUntilDate.IsChecked = false;
                        DpAccesibility.SelectedDate = null;
                        DpAccesibility.Text = string.Empty;
                    }


                    if (isErrorOccured)
                    {
                        ShowStatus("Favorited values are no longer valid ", true);
                    }
                    List<FavouriteInfo> favouriteInfoCollection = new List<FavouriteInfo>();
                    string format = "yyyy-MM-dd HH:mm:ss.fff";
                    favouriteInfo.ModifiedDate = DateTime.Now.ToString(format);
                    favouriteInfoCollection.Add(favouriteInfo);
                    XMLSerializeHelper.Serialize<FavouriteInfo>(favouriteInfoCollection, XMLFolderType.AddIn, "UpdateFavouriteInfo");

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }


        private void RBPolicyType_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnFavouriteDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var customButton = sender as CustomButton;
                var contentPresenter = customButton.TemplatedParent as ContentPresenter;
                var selectedItem = contentPresenter.Content as Model.FavouriteInfo;
                if (selectedItem != null)
                {
                    var favoriteCollection = new List<Model.FavouriteInfo>
                    {
                        selectedItem
                    };
                    XMLSerializeHelper.Serialize<FavouriteInfo>(favoriteCollection, XMLFolderType.AddIn, "DeleteFavouriteInfo");

                    //string _deleteQuery = string.Format(_favouriteDeleteQuery, selectedItem.FavourtieName);
                    //await Task.Delay(2000);
                    FavouriteInfoCollection.Remove(selectedItem);
                    FavouriteInfoCollection = FavouriteInfoCollection.OrderByDescending(m => m.FavId).ToList();
                    LstFavouriteList.ItemsSource = null;
                    LstFavouriteList.ItemsSource = FavouriteInfoCollection;
                    ShowStatus(string.Format("{0} deleted successfully ", selectedItem.FavourtieName));
                    //var result = SQLite.SQLiteHandler.ExecuteCreateOrInsertQuery(_deleteQuery);
                    //if (result.Status)
                    //{
                    //    ShowStatus(string.Format("{0} deleted successfully ", selectedItem.FavourtieName));
                    //}
                    //else
                    //{
                    //    ShowStatus(string.Format("{0} deletion failed. Please try again ", selectedItem.FavourtieName));
                    //}
                    // GetAllFavouritesFromLocalDataBase();
                }


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }


        //private void GetSelectedValueFromMainGrid()
        //{
        //    try
        //    {
        //        var selectedItem = dgAddActivity.SelectedItem;
        //        var addtoActivityComboboxValue = AddToActivityTypeComboBox.SelectedItem as string;

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //    }
        //}

        private void btnAddActivityCancel_Click(object sender, RoutedEventArgs e)
        {
            //AddActivityPopup.IsOpen = false;
            //grdBackOverLay.Visibility = Visibility.Collapsed;
        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RefreshPolicyYearAndDescription(PolicyInfo selectedActivity)
        {
            try
            {
                EnableSaveButons();
                policyTypeTextBlock.Text = string.Empty;
                if (!string.IsNullOrEmpty(selectedActivity?.PolicyType))
                {
                    if (selectedActivity?.PolicyType == "(none)")
                    {
                        RBCustomPolicyType.IsChecked = true;
                        lblPolicyType.Text = string.Empty;
                        policyTypeTextBlock.Text = string.Empty;
                    }
                    else
                    {
                        RBPolicyType.IsChecked = true;
                        lblPolicyType.Text = selectedActivity?.PolicyType;
                        policyTypeTextBlock.Text = selectedActivity?.PolicyType;
                        policyTypeTextBlock.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    RBCustomPolicyType.IsChecked = true;
                    lblPolicyType.Text = string.Empty;
                }
                if (selectedActivity != null)
                {
                    if (selectedActivity?.Effective != null && selectedActivity?.Effective != DateTime.MinValue)
                    {
                        var effectiveDate = selectedActivity?.Effective.Year;
                        var policySelectedItem = policyYearLst.FirstOrDefault(m => m.Description == effectiveDate.ToString());
                        if (policySelectedItem != null)
                        {
                            LstPolicyList.SelectedItem = policySelectedItem;
                            RBCustomPolicyType.IsChecked = false;
                            RBPolicyType.IsChecked = true;
                            lblPolicyType.Text = selectedActivity?.PolicyType;

                        }
                        else
                        {
                            LstPolicyList.SelectedIndex = 0;
                            RBCustomPolicyType.IsChecked = true;
                            RBPolicyType.IsChecked = false;
                            lblPolicyType.Text = string.Empty;
                            policyTypeTextBlock.Text = string.Empty;

                        }

                    }
                    else
                    {
                        var currentDate = DateTime.Now.Year;
                        var policySelectedItem = policyYearLst.FirstOrDefault(m => m.Description == currentDate.ToString());
                        LstPolicyList.SelectedItem = policySelectedItem;
                    }
                }

                if (rbActivityDec.IsChecked == true)
                    txtDesc.Text = selectedActivity?.PolicyDisplayDesc;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }
        private void InActiveActivityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedInActiveActivity = InActiveActivityList.SelectedItem as PolicyInfo;
            EnableAttachmentsWithActivitySelection(selectedInActiveActivity, false);
            RefreshPolicyYearAndDescription(selectedInActiveActivity);
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                var grd = sender as Grid;
                if (grd != null)
                {
                    var selectedItem = grd.DataContext as PolicyInfo;
                    if (selectedItem != null)
                    {
                        if (selectedItem != null && !string.IsNullOrEmpty(selectedItem.PolicyType))
                        {
                            var popUp = grd.FindName("PopupDetails") as Popup;
                            if (popUp != null)
                                popUp.IsOpen = true;
                        }
                        else if (selectedItem != null && string.IsNullOrEmpty(selectedItem.PolicyType))
                        {
                            var popUp = grd.FindName("PopupDetailsForNonPolicy") as Popup;
                            if (popUp != null)
                                popUp.IsOpen = true;
                        }

                    }
                }

                //PopupDetailsForNonPolicy
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }


        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                var grd = sender as Grid;
                if (grd != null)
                {
                    var popUp = grd.FindName("PopupDetails") as Popup;
                    var NonpolicyPopup = grd.FindName("PopupDetailsForNonPolicy") as Popup;
                    if (popUp != null)
                        popUp.IsOpen = false;
                    if (NonpolicyPopup != null)
                        NonpolicyPopup.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        //private void AddToActivityTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        var selectedItem = AddToActivityTypeComboBox.SelectedItem as string;
        //        if (!string.IsNullOrEmpty(selectedItem))
        //        {
        //            GetActivity(selectedItem);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //    }
        //}

        #endregion

        #region Methods
        /// <summary>
        /// Get the newly added Activity 
        /// </summary>
        /// <param name="UniqEntity"></param>
        /// <returns></returns>
        public List<PolicyInfo> GetNewAddedActivities(AddActivity addactivity)
        {
            List<PolicyInfo> PolicyInfoCollection = new List<PolicyInfo>();
            try
            {

                var policyInfo = new PolicyInfo
                {
                    ClientId = Convert.ToInt32(addactivity.ClientId),
                    ActivityId = Convert.ToInt64(addactivity.TaskEventEpicId)
                };
                if (!string.IsNullOrEmpty(addactivity.ActivityGuid))
                {
                    policyInfo.ActivityGuid = addactivity.ActivityGuid;
                }


                policyInfo.PolicyCode = addactivity.AddActivityCode;
                policyInfo.PolicyDisplayDesc = addactivity.AddActivityDisplayDescription;
                policyInfo.PolicyDesc = addactivity.AddActivityDescription;
                policyInfo.AddToType = addactivity.AddtoType;
                policyInfo.OwnerCode = addactivity.OwnerCode;
                policyInfo.OwnerDescription = addactivity.OwnerDecription;
                policyInfo.Status = addactivity.Status;
                policyInfo.InsertedDate = DateTime.UtcNow;
                policyInfo.UniqAssociatedItem = Convert.ToInt32(addactivity.AddToTypeId);
                policyInfo.AssociationType = policyInfo.AddToType;
                policyInfo.UniqAgency = addactivity.UniqAgency;
                policyInfo.UniqBranch = addactivity.UniqBranch;
                if (!string.IsNullOrEmpty(policyInfo.Status))
                {
                    if (policyInfo.Status == "Open")
                    {
                        policyInfo.IsClosed = false;
                    }
                    else
                    {
                        policyInfo.IsClosed = true;
                    }
                }
                if (string.Equals(policyInfo.AddToType, "Policy", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        string policyActivitiesQuery = string.Format("Select * From HIBOPPolicy where UniqEntity={0} and UniqPolicy={1} limit 1", addactivity.ClientId, addactivity.AddToTypeId);
                        var sqliteDataReader = Helper.Helper.SqliteHelper.ExecuteSelectQuery(policyActivitiesQuery);
                        var policy = new Policy();
                        if (sqliteDataReader != null)
                        {
                            while (sqliteDataReader.Read())
                            {
                                try
                                {
                                    policy.PolicyId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqPolicy"));
                                    policy.ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity"));
                                    policy.Type = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CdPolicyLineTypeCode"));
                                    policy.Status = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyStatus"));
                                    policy.Effective = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EffectiveDate"));
                                    policy.Expiration = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate"));
                                    policy.PolicyNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyNumber"));
                                    policy.PolicyDescription = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("DescriptionOf"));
                                    policy.Flags = Convert.ToInt32(sqliteDataReader["Flags"]);

                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }

                            }
                            sqliteDataReader.Close();
                        }

                        policyInfo.PolicyNumber = policy.PolicyNumber;
                        policyInfo.PolicyType = policy.Type; // Convert.ToString(sqliteDataReaderDataTable.Rows[i]["CdPolicyLineTypeCode"]);
                        policyInfo.Effective = policy.Effective != null ? Convert.ToDateTime(policy.Effective).Date : default(DateTime); // SQLite.SQLiteHandler.CheckNull<DateTime>(sqliteDataReaderDataTable.Rows[i]["EffectiveDate"]);
                        policyInfo.ShowEffectiveDate = policyInfo.Effective.ToShortDateString();
                        policyInfo.Expiration = policy.Expiration != null ? Convert.ToDateTime(policy.Expiration).Date : default(DateTime); //SQLite.SQLiteHandler.CheckNull<DateTime>(sqliteDataReaderDataTable.Rows[i]["ExpirationDate"]);
                        policyInfo.ShowExpirationDate = policyInfo.Expiration.ToShortDateString();
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                    }


                }
                PolicyInfoCollection.Add(policyInfo);


                if (addactivity.AddActivityTypeClosedStatus == 1)
                {
                    ClosedActivitiesInfoCollection.InsertRange(0, PolicyInfoCollection);
                    InActiveActivityList.ItemsSource = null;
                    InActiveActivityList.ItemsSource = ClosedActivitiesFilteredItems;
                }
                else
                {
                    OpenActivitiesInfoCollection.InsertRange(0, PolicyInfoCollection);
                    ActiveActivityList.ItemsSource = null;
                    ActiveActivityList.ItemsSource = OpenActivitiesFilteredItems;
                }


                //}
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return PolicyInfoCollection;
        }


        public List<PolicyInfo> GetNewAddedActivitiesFromSqlite(string activitiesQuery)
        {
            List<PolicyInfo> PolicyInfoCollection = new List<PolicyInfo>();
            try
            {
                var clientSelectedItem = ActiveTab.IsSelected ? (LstActiveClient.SelectedItem as ClientInfo) : (LstInActiveClient.SelectedItem as ClientInfo);
                var Query = string.Format(activitiesQuery, clientSelectedItem.ClientId, ThisAddIn.EmployeeLookupCode);
                var sqliteDataReaderDataTable = SQLite.SQLiteHandler.ExecuteSelecttQueryWithAdapter(Query);
                if (sqliteDataReaderDataTable != null)
                {
                    for (int i = 0; i < sqliteDataReaderDataTable.Rows.Count; i++)
                    {
                        try
                        {
                            var policyInfo = new PolicyInfo
                            {
                                ClientId = Convert.ToInt32(sqliteDataReaderDataTable.Rows[i]["UniqEntity"]),
                                ActivityId = Convert.ToInt32(sqliteDataReaderDataTable.Rows[i]["TaskEventEpicId"]),
                                PolicyCode = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AddActivityCode"]),
                                PolicyDesc = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AddActivityDescription"]),
                                PolicyDisplayDesc = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AddActivityDisplayDescription"]),
                                AddToType = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AddtoType"]),
                                ActivityGuid = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["ActivityGuid"]),
                                OwnerDescription = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["OwnerDecription"]),
                                Status = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["Status"]),
                                InsertedDate = DateTime.UtcNow
                            };
                            policyInfo.AssociationType = policyInfo.AddToType;
                            if (!string.IsNullOrEmpty(policyInfo.Status))
                            {
                                if (policyInfo.Status == "Open")
                                {
                                    policyInfo.IsClosed = false;
                                }
                                else
                                {
                                    policyInfo.IsClosed = true;
                                }
                            }
                            var AddToTypeId = Convert.ToInt32(sqliteDataReaderDataTable.Rows[i]["AddToTypeId"]);
                            policyInfo.UniqAssociatedItem = AddToTypeId;

                            if (string.Equals(policyInfo.AddToType, "Policy", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string policyActivitiesQuery = string.Format("Select * From HIBOPPolicy where UniqEntity={0} and UniqPolicy={1} limit 1", policyInfo.ClientId, AddToTypeId);
                                    var sqliteDataReader = Helper.Helper.SqliteHelper.ExecuteSelectQuery(policyActivitiesQuery);
                                    var policy = new Policy();
                                    if (sqliteDataReader != null)
                                    {
                                        while (sqliteDataReader.Read())
                                        {
                                            try
                                            {
                                                policy.PolicyId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqPolicy"));
                                                policy.ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity"));
                                                policy.Type = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CdPolicyLineTypeCode"));
                                                policy.Status = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyStatus"));
                                                policy.Effective = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EffectiveDate"));
                                                policy.Expiration = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate"));
                                                policy.PolicyNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyNumber"));
                                                policy.PolicyDescription = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("DescriptionOf"));
                                                policy.Flags = Convert.ToInt32(sqliteDataReader["Flags"]);
                                                //policy.UniqAgency = Convert.ToInt32(sqliteDataReader["UniqAgency"]);
                                                //policy.UniqBranch = Convert.ToInt32(sqliteDataReader["UniqBranch"]);

                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                            }

                                        }
                                        sqliteDataReader.Close();
                                    }

                                    policyInfo.PolicyNumber = policy.PolicyNumber;
                                    policyInfo.PolicyType = policy.Type;
                                    policyInfo.Effective = policy.Effective != null ? Convert.ToDateTime(policy.Effective).Date : default(DateTime); // SQLite.SQLiteHandler.CheckNull<DateTime>(sqliteDataReaderDataTable.Rows[i]["EffectiveDate"]);
                                    policyInfo.ShowEffectiveDate = policyInfo.Effective.ToShortDateString();
                                    policyInfo.Expiration = policy.Expiration != null ? Convert.ToDateTime(policy.Expiration).Date : default(DateTime); //SQLite.SQLiteHandler.CheckNull<DateTime>(sqliteDataReaderDataTable.Rows[i]["ExpirationDate"]);
                                    policyInfo.ShowExpirationDate = policyInfo.Expiration.ToShortDateString();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                                }

                            }
                            else if (string.Equals(policyInfo.AddToType, "Account", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string accountActivitiesQuery = string.Format("Select * From HIBOPActivityAccount where AccountId={0} and LookupCode='{1}' limit 1", policyInfo.UniqAssociatedItem, ThisAddIn.EmployeeLookupCode);
                                    var accountInfo = new Account();
                                    var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(accountActivitiesQuery);
                                    if (sqliteDataReader != null)
                                    {
                                        while (sqliteDataReader.Read())
                                        {
                                            try
                                            {
                                                accountInfo.AccountId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("AccountId"));
                                                accountInfo.UniqAgency = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqAgency"));
                                                accountInfo.UniqBranch = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqBranch"));
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                                            }


                                        }
                                        sqliteDataReader.Close();
                                    }
                                    policyInfo.UniqAgency = accountInfo.UniqAgency;
                                    policyInfo.UniqBranch = accountInfo.UniqBranch;
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                                }
                            }

                            // policyInfo.IsClosed = false;
                            PolicyInfoCollection.Add(policyInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return PolicyInfoCollection;
        }

        private OutlookNS.MailItem AddGuidInSelectedMailItem(OutlookNS.MailItem outlookMsg, bool isReplyMail = false)
        {
            try
            {
                StringBuilder strBuilder = new StringBuilder();
                var identifier = Guid.NewGuid().ToString();
                var htmlBody = outlookMsg.HTMLBody;
                strBuilder.Append(htmlBody);
                strBuilder.AppendFormat("<span Name='IdentifierKey' style='color: white; font - size:5px'>{0}</span>", identifier);
                outlookMsg.HTMLBody = strBuilder.ToString();
                _mailItems = new List<MailItemInfo>();
                if (isReplyMail)
                {
                    _mailItems.Add(new MailItemInfo { Identifier = identifier, MailItem = outlookMsg });
                }
                else
                {
                    _mailItems.Add(new MailItemInfo { Identifier = identifier });
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

            return outlookMsg;
        }

        private void SyncLocal_SyncCompleted(bool status, bool isValidUser)
        {
            try
            {
                if (!string.IsNullOrEmpty(SyncFilePath) && File.Exists(SyncFilePath))
                {
                    File.Delete(SyncFilePath);
                }
                MessageBox.Show(status ? "Sync completed Sucessfully." : "Sync failed .Please contact your administrator.");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        ///<summary>
        /// send a reply to an email when user clicks on delete attach button.
        /// </summary>
        /// <returns>void</returns>
        public async void SendReply(bool isAttachDelete = false)
        {
            try
            {
                var newGuid = Guid.NewGuid().ToString();
                string fileName = string.Empty;
                _IsSendAttachAndDelete = isAttachDelete;
                _IsDeleteAttached = isAttachDelete;
                var activeExp = Globals.ThisAddIn.Application.ActiveExplorer();
                if (activeExp.Selection != null && activeExp.Selection.Count > 0)
                {
                    OutlookNS.MailItem guidMailItem = null;
                    var currentMailItem = (activeExp.Selection[1] as OutlookNS.MailItem);
                    var currentmail = explorer.ActiveInlineResponse as OutlookNS.MailItem;
                    if (!string.IsNullOrEmpty(currentmail.To))
                    {
                        try
                        {
                            var attachmentIdentifier = currentmail.UserProperties.Add("AttachmentIdentifier", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                            attachmentIdentifier.Value = newGuid;
                            guidMailItem = AddGuidInSelectedMailItem(currentmail, true);
                            var mailItemInfo = _mailItems[0];
                            if (mailItemInfo != null)
                            {
                                mailItemInfo.MailItem = guidMailItem;
                            }
                            SaveAttachmentInfo(isAttachDelete, true, true);
                            AddACategory(currentMailItem);
                            guidMailItem.UserProperties.Add("IsComposeMail", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                            guidMailItem.UserProperties.Add("ReplyMail", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                            if (isAttachDelete)
                            {
                                var isSAD = guidMailItem.UserProperties.Add("IsSendAttachDelete", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                                isSAD.Value = true.ToString();
                            }
                            guidMailItem.Send();
                        }
                        catch
                        {
                            // We get an error "this method can't be used with an inline response mail item"
                            // trying to display InlineResponse
                            try
                            {
                                currentMailItem.Display(false);
                            }
                            catch
                            { }
                            //wait a bit
                            await Task.Delay(10);
                            // trying to close and save InlineResponse
                            try
                            {
                                var myinspector = OLA.ActiveInspector();
                                (myinspector.CurrentItem as OutlookNS.MailItem)?.Close(Microsoft.Office.Interop.Outlook.OlInspectorClose.olSave);
                                OLA.ActiveExplorer().CurrentFolder = (OutlookNS.MAPIFolder)OLA.ActiveExplorer().Session.GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderInbox);
                            }
                            catch
                            { }
                            // now it has to send OK
                            try
                            {

                                var replyitem = currentMailItem.Reply() as OutlookNS.MailItem;
                                //  var guidMailItem = AddGuidInSelectedMailItem(currentMailItem);
                                replyitem = guidMailItem;
                                //   SaveAttachmentInfo(isAttachDelete);
                                if (string.IsNullOrEmpty(replyitem.Subject))
                                {
                                    replyitem.Subject = " ";
                                }
                                var copiedMail = replyitem?.CopyTo(currentMailItem) as OutlookNS.MailItem;
                                if (copiedMail?.UserProperties["AttachmentIdentifier"] == null)
                                {
                                    var attachmentIdentifier = copiedMail?.UserProperties.Add("AttachmentIdentifier", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                                    attachmentIdentifier.Value = newGuid;
                                }
                                if (copiedMail?.UserProperties["IsSendAttachDelete"] == null)
                                {
                                    var isSAD = copiedMail?.UserProperties.Add("IsSendAttachDelete", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                                    isSAD.Value = true.ToString();
                                }
                                //copiedMail?.Move(GetCustomFolder(ProcessingfolderName));
                                replyitem.Send();

                            }

                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting all the activities(Open/Closed) from local database which are related to selected client
        /// </summary>
        /// <returns>void</returns>
        private void GetActivitiesFromClient(ClientInfo selectedClient)
        {
            try
            {
                ClosedActivitiesInfoCollection = new List<PolicyInfo>();
                OpenActivitiesInfoCollection = new List<PolicyInfo>();
                StartSearchActivities();
                if (selectedClient != null)
                {
                    var PolicyInfoCollection = GetAcitivtiesFromSQLite(selectedClient.UniqEntity);

                    var query = "Select *  From HIBOPAddActivity where HIBOPAddActivity.TaskEventEpicId= 0 and HIBOPAddActivity.IsPushedToEpic = 0 and HIBOPAddActivity.UniqEntity ={0} and HIBOPAddActivity.CurrentlyLoggedLookupCode ='{1}'";
                    //var query = "Select * From HIBOPAddActivity where  HIBOPAddActivity.TaskEventEpicId = 0 and HIBOPAddActivity.IsPushedToEpic = 0 and HIBOPAddActivity.UniqEntity = 122230 and HIBOPAddActivity.CurrentlyLoggedLookupCode = 'FAGJO1'";
                    var newActivityCollection = GetNewAddedActivitiesFromSqlite(query);

                    OpenActivitiesInfoCollection = PolicyInfoCollection.Where(x => !x.IsClosed).ToList();

                    ClosedActivitiesInfoCollection = PolicyInfoCollection.Where(x => x.IsClosed).ToList();

                    OpenActivitiesInfoCollection.AddRange(newActivityCollection.Where(m => !m.IsClosed));
                    OpenActivitiesInfoCollection = OpenActivitiesInfoCollection.OrderByDescending(x => x.InsertedDate).ToList();
                    ActiveActivityList.ItemsSource = null;
                    ActiveActivityList.ItemsSource = OpenActivitiesFilteredItems;

                    var currentDate = DateTime.Now.ToLocalTime();
                    var ConditionDate = DateTime.Now.ToLocalTime().AddYears(-1);

                    ClosedActivitiesInfoCollection.AddRange(newActivityCollection.Where(m => m.IsClosed));
                    ClosedActivitiesInfoCollection = ClosedActivitiesInfoCollection.Where(x => x.IsClosed && x.ClosedDate.Date >= ConditionDate.Date && x.ClosedDate.Date <= currentDate.Date).OrderByDescending(x => x.InsertedDate).ToList();

                    InActiveActivityList.ItemsSource = null;
                    InActiveActivityList.ItemsSource = ClosedActivitiesFilteredItems;


                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        ///<summary>
        /// getting add to type for selected activity from local database which are related to selected activity
        /// </summary>
        /// <returns>void</returns>
        private void GetActivityForSelectedActivity(PolicyInfo selectedActivity)
        {
            try
            {
                attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = null;
                switch (selectedActivity.AssociationType)
                {
                    case "Account":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<Account>();
                            List<Account> tempAccountCollection = new List<Account>();

                            var selectedAccount = GetAccountDetailForActivityFromLocalDatabase(selectedActivity);
                            if (selectedAccount != null)
                            {
                                tempAccountCollection.Add(selectedAccount);
                                attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempAccountCollection;
                            }
                            break;
                        }
                    case "Policy":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<Policy>();
                            List<Policy> tempPolicyCollection = new List<Policy>();
                            var selectedPolicy = GetPolicyDetailsForActivityFromLocalDatabase(selectedActivity);
                            if (selectedPolicy != null)
                            {
                                tempPolicyCollection.Add(selectedPolicy);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempPolicyCollection;

                            break;
                        }
                    case "Claim":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<Claim>();
                            List<Claim> tempClaimsCollection = new List<Claim>();
                            var selectedClaim = GetClaimsDetailsForActivityFromLocalDatabase(selectedActivity);
                            if (selectedClaim != null)
                            {
                                tempClaimsCollection.Add(selectedClaim);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempClaimsCollection;


                            break;
                        }
                    case "Line":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<HIB.Outlook.Model.Activities.Line>();
                            List<HIB.Outlook.Model.Activities.Line> tempLinesOfBusinessCollection = new List<HIB.Outlook.Model.Activities.Line>();
                            HIB.Outlook.Model.Activities.Line selectedLine = GetLineDetailsForActivityFromLocalDatabase(selectedActivity);
                            if (selectedLine != null)
                            {
                                tempLinesOfBusinessCollection.Add(selectedLine);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempLinesOfBusinessCollection;


                            break;
                        }
                    case "Opportunity":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<Opportunities>();
                            List<Opportunities> tempOpportunitiesCollection = new List<Opportunities>();
                            var selectedOpportunity = GetOpportunitiesForActivityFromLocalDatabase(selectedActivity);
                            if (selectedOpportunity != null)
                            {
                                tempOpportunitiesCollection.Add(selectedOpportunity);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempOpportunitiesCollection;
                            break;
                        }
                    case "Services":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<Services>();
                            List<Services> tempServicesCollection = new List<Services>();
                            var selectedService = GetServiceDetailsForActivityFromLocalDatabase(selectedActivity);
                            if (selectedService != null)
                            {
                                tempServicesCollection.Add(selectedService);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempServicesCollection;
                            break;
                        }
                    case "Marketing":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<MasterMarketingSubmission>();
                            List<MasterMarketingSubmission> tempMasterMarketingSubmissionCollection = new List<MasterMarketingSubmission>();
                            var selectedMasterMarketingSubmission = GetMasterMarketingForActivityFromLocalDatabase(selectedActivity);
                            if (selectedMasterMarketingSubmission != null)
                            {
                                tempMasterMarketingSubmissionCollection.Add(selectedMasterMarketingSubmission);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempMasterMarketingSubmissionCollection;
                            break;
                        }
                    case "Bill":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<ActivityBillInfo>();
                            List<ActivityBillInfo> tempBillInfoCollection = new List<ActivityBillInfo>();
                            var selectedBill = GetActivityBillForActivityFromLocalDatabase(selectedActivity);
                            if (selectedBill != null)
                            {
                                tempBillInfoCollection.Add(selectedBill);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempBillInfoCollection;

                            break;
                        }
                    case "Carrier":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<ActivityCarrierInfo>();
                            List<ActivityCarrierInfo> tempCarrierInfoCollection = new List<ActivityCarrierInfo>();
                            var selectedCarrier = GetActivityCarrierSubmissionForActivityFromLocalDatabase(selectedActivity);
                            if (selectedCarrier != null)
                            {
                                tempCarrierInfoCollection.Add(selectedCarrier);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempCarrierInfoCollection;

                            break;
                        }
                    case "Transaction":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<ActivityTransactionInfo>();
                            List<ActivityTransactionInfo> tempTransactionInfoCollection = new List<ActivityTransactionInfo>();
                            var selectedTransaction = GetActivityTransactionForActivityFromLocalDatabase(selectedActivity);
                            if (selectedTransaction != null)
                            {
                                tempTransactionInfoCollection.Add(selectedTransaction);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempTransactionInfoCollection;

                            break;
                        }
                    case "Certificate":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<ActivityCertificateInfo>();
                            List<ActivityCertificateInfo> tempCertificateInfoCollection = new List<ActivityCertificateInfo>();
                            var selectedCertificate = GetActivityCertificateForActivityFromLocalDatabase(selectedActivity);
                            if (selectedCertificate != null)
                            {
                                tempCertificateInfoCollection.Add(selectedCertificate);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempCertificateInfoCollection;

                            break;
                        }
                    case "Evidence":
                        {
                            WriteColumnsForAttachingToActivityTypeFromClass<ActivityEvidenceInfo>();
                            List<ActivityEvidenceInfo> tempEvidenceInfoCollection = new List<ActivityEvidenceInfo>();
                            var selectedEvidence = GetActivityEvidenceForActivityFromLocalDatabase(selectedActivity);
                            if (selectedEvidence != null)
                            {
                                tempEvidenceInfoCollection.Add(selectedEvidence);
                            }
                            attachToEpicMainPage.multipleEmailAttachment.SelectedActivityValueGrid.ItemsSource = tempEvidenceInfoCollection;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting last expired 18 months data for selected Add to type in add activity when clicking include history option in UI.
        /// </summary>
        /// <returns>void</returns>
        internal void IncludeHistoryForOldValues()
        {
            try
            {
                if (addactivityMainPage != null)
                {
                    ClientInfo selectedClient = null;
                    if (Tab.SelectedIndex == 0)
                    {
                        selectedClient = LstActiveClient.SelectedItem as ClientInfo;
                    }
                    else if (Tab.SelectedIndex == 1)
                    {
                        selectedClient = LstInActiveClient.SelectedItem as ClientInfo;
                    }
                    var selectedItem = addactivityMainPage?.addactivityFirstPage.AddToActivityTypeComboBox.SelectedItem as string;
                    var currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                    var ConditionDate = DateTime.Now.AddMonths(-18).ToString("yyyy/MM/dd");
                    if (!string.IsNullOrEmpty(selectedItem))
                    {
                        switch (selectedItem)
                        {
                            case "Policy":
                                {
                                    string policyActivitiesQuery = string.Format("Select * From HIBOPPolicy where UniqEntity={0} and Flags={1} and (ExpirationDate >='{2}' or ExpirationDate is null) and EmployeeLookUpCode='{3}'", selectedClient.UniqEntity, 0, ConditionDate, ThisAddIn.EmployeeLookupCode);
                                    GetPolicyDetailsFromLocalDatabase(policyActivitiesQuery, false);
                                    break;
                                }
                            case "Line":
                                {
                                    string linesActivitiesQuery = string.Format("Select * From HIBOPActivityLine where UniqEntity={0} and Flags={1} and (ExpirationDate >='{2}' or ExpirationDate is null) and EmployeeLookUpCode='{3}'", selectedClient.UniqEntity, 0, ConditionDate, ThisAddIn.EmployeeLookupCode);
                                    GetLinesDetailsFromLocalDatabase(linesActivitiesQuery, false);
                                    break;
                                }
                            case "Opportunities":
                                {
                                    string OpportunityActivitiesQuery = string.Format("Select * From HIBOPActivityOpportunity where UniqEntity={0} and Flags={1} and (ExpirationDate >='{2}' or ExpirationDate is null) and EmployeeLookUpCode='{3}'", selectedClient.UniqEntity, 0, ConditionDate, ThisAddIn.EmployeeLookupCode);
                                    GetOpportunitiesDetailsFromLocalDatabase(OpportunityActivitiesQuery, false);
                                    break;
                                }
                            case "Services":
                                {
                                    string servicesActivitiesQuery = string.Format("Select * From HIBOPActivityServices where UniqEntity={0} and Flags={1} and (ExpirationDate >='{2}' or ExpirationDate is null) and EmployeeLookUpCode='{3}'", selectedClient.UniqEntity, 0, ConditionDate, ThisAddIn.EmployeeLookupCode);
                                    GetServiceDetailsFromLocalDatabase(servicesActivitiesQuery, false);
                                    break;
                                }
                            case "Master Marketing Submission":
                                {
                                    string masterMarketingActivitiesQuery = string.Format("Select * From HIBOPActivityMasterMarketing where UniqEntity={0} and Flags={1} and (ExpirationDate >='{2}' or ExpirationDate is null) and EmployeeLookUpCode='{3}'", selectedClient.UniqEntity, 0, ConditionDate, ThisAddIn.EmployeeLookupCode);
                                    GetMasterMarketingSubmissionDetailsFromLocalDatabase(masterMarketingActivitiesQuery, false);
                                    break;
                                }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// showing only the selected Add to type in add activity when clicking exclude history option in UI.
        /// </summary>
        /// <returns>void</returns>
        internal void ExcludeHistoryForOldValues()
        {
            try
            {
                if (addactivityMainPage != null)
                {
                    ClientInfo selectedClient = null;
                    if (Tab.SelectedIndex == 0)
                    {
                        selectedClient = LstActiveClient.SelectedItem as ClientInfo;
                    }
                    else if (Tab.SelectedIndex == 1)
                    {
                        selectedClient = LstInActiveClient.SelectedItem as ClientInfo;
                    }
                    var selectedItem = addactivityMainPage.addactivityFirstPage.AddToActivityTypeComboBox.SelectedItem as string;
                    if (!string.IsNullOrEmpty(selectedItem))
                    {
                        switch (selectedItem)
                        {
                            case "Policy":
                                {
                                    if (policyCollection == null)
                                    {
                                        policyCollection = new List<Policy>();
                                        string policyActivitiesQuery = string.Format("Select * From HIBOPPolicy where UniqEntity={0} and Flags={1} and EmployeeLookUpCode='{2}'", selectedClient.ClientId, 1, ThisAddIn.EmployeeLookupCode);
                                        GetPolicyDetailsFromLocalDatabase(policyActivitiesQuery, true);
                                    }
                                    else
                                    {
                                        if (policyCollection.Count > 0)
                                        {
                                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = policyCollection.Where(m => m.Flags == 1).ToList();
                                        }
                                    }

                                    break;
                                }

                            case "Line":
                                {
                                    if (lineCollection == null || lineCollection.Count == 0)
                                    {
                                        lineCollection = new List<Line>();
                                        string linesActivitiesQuery = string.Format("Select * From HIBOPActivityLine where UniqEntity={0} and Flags={1} and EmployeeLookUpCode='{2}'", selectedClient.ClientId, 1, ThisAddIn.EmployeeLookupCode);
                                        GetLinesDetailsFromLocalDatabase(linesActivitiesQuery, true);
                                    }
                                    else
                                    {
                                        if (lineCollection.Count > 0)
                                        {
                                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = lineCollection.Where(m => m.Flags == 1).ToList();
                                        }
                                    }


                                    break;
                                }

                            case "Opportunities":
                                {
                                    if (opportunitiesCollection == null || opportunitiesCollection.Count == 0)
                                    {
                                        opportunitiesCollection = new List<Opportunities>();
                                        string OpportunityActivitiesQuery = string.Format("Select * From HIBOPActivityOpportunity where UniqEntity={0} and Flags={1} and EmployeeLookUpCode='{2}'", selectedClient.ClientId, 1, ThisAddIn.EmployeeLookupCode);
                                        GetOpportunitiesDetailsFromLocalDatabase(OpportunityActivitiesQuery, true);
                                    }
                                    else
                                    {
                                        if (opportunitiesCollection.Count > 0)
                                        {
                                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = opportunitiesCollection.Where(m => m.Flags == 1).ToList();
                                        }
                                    }
                                    break;
                                }
                            case "Services":
                                {
                                    if (servicesCollection == null || servicesCollection.Count == 0)
                                    {
                                        servicesCollection = new List<Services>();
                                        string servicesActivitiesQuery = string.Format("Select * From HIBOPActivityServices where UniqEntity={0} and Flags={1} and EmployeeLookUpCode='{2}'", selectedClient.ClientId, 1, ThisAddIn.EmployeeLookupCode);
                                        GetServiceDetailsFromLocalDatabase(servicesActivitiesQuery, true);
                                    }
                                    else
                                    {
                                        if (servicesCollection.Count > 0)
                                        {
                                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = servicesCollection.Where(m => m.Flags == 1).ToList();
                                        }

                                    }

                                    break;
                                }
                            case "Master Marketing Submission":
                                {
                                    if (masterMarketingCollection == null || masterMarketingCollection.Count == 0)
                                    {
                                        masterMarketingCollection = new List<MasterMarketingSubmission>();
                                        string masterMarketingActivitiesQuery = string.Format("Select * From HIBOPActivityMasterMarketing where UniqEntity={0} and Flags={1} and EmployeeLookUpCode='{2}'", selectedClient.ClientId, 1, ThisAddIn.EmployeeLookupCode);
                                        GetMasterMarketingSubmissionDetailsFromLocalDatabase(masterMarketingActivitiesQuery, true);
                                    }
                                    else
                                    {
                                        if (masterMarketingCollection.Count > 0)
                                        {
                                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = masterMarketingCollection.Where(m => m.Flags == 1).ToList();
                                        }

                                    }
                                    break;
                                }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }


        ///<summary>
        /// getting all the selected mails from outlook with selected options
        /// </summary>
        /// <returns>void</returns>
        public void GetAllSelectedMailsFromOutlook(string activityDesc)
        {
            try
            {
                if (_mailItems.Count <= 1)
                {
                    GetAllSelectedEmails();
                }
                string policyType = string.Empty;
                policyType = string.IsNullOrEmpty(lblPolicyType.Text) ? "(none)" : lblPolicyType.Text;
                string policyYear = (LstPolicyList.SelectedItem as PolicyYear)?.Description;
                selectedEmailInfoCollection = new List<SelectedEmailInfo>();

                if (_mailItems.Count > 0)
                {
                    foreach (MailItemInfo selectedMail in _mailItems)
                    {

                        try
                        {
                            SelectedEmailInfo mailInfo = new SelectedEmailInfo
                            {
                                From = selectedMail.MailItem.SenderName,
                                Identifier = selectedMail.Identifier,
                                MailItem = selectedMail.MailItem,
                                EntryId = selectedMail.MailItem.EntryID,
                                RecievedDate = selectedMail.MailItem.ReceivedTime.Date.ToShortDateString(),
                                MailRecievedDateWithTime = selectedMail.MailItem.ReceivedTime.ToString("ddd MM/dd/yyyy hh:mm tt"),
                                Subject = selectedMail.MailItem.Subject,
                                To = selectedMail.MailItem.To,
                                Cc = selectedMail.MailItem.CC,
                                HtmlBody = selectedMail.MailItem?.HTMLBody.Replace("'", "").Trim(),
                                //if (selectedMail.MailItem.EntryID == _mailItems[0].MailItem.EntryID)
                                //{
                                //    mailInfo.IsApplyToAllNeedToBeVisible = true;
                                //}
                                //This is for multiple email attachment while clicking favorite.
                                TextBoxValue = txtDesc.Text,
                                ActivityDesc = activityDesc,
                                PolicyYear = policyYear,
                                PolicyType = policyType
                            };
                            selectedEmailInfoCollection.Add(mailInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }

                }



            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private ObservableCollection<ClientInfo> GetClientsFilter(bool isActive)
        {
            List<ClientInfo> tempCollection = new List<ClientInfo>();
            tempCollection = Globals.ThisAddIn.ClientInfoCollection;

            if (tempCollection != null && tempCollection.Count == 0)
            {
                tempCollection = GetClientInformationFromSQLite(String.Format(_listOfClientsQuery, ThisAddIn.EmployeeLookupCode));
            }
            List<ClientInfo> clientlist = new List<ClientInfo>();
            try
            {
                var attachmentinfoList = GetAttachmentInfoDetails($"Select * from AttachmentInfo Where EmployeeCode ='{ThisAddIn.EmployeeLookupCode}' order by CreatedDate desc");
                var lastweekdate = DateTime.Now.AddDays(-14);
                attachmentinfoList = attachmentinfoList.Where(x => x.CreatedDate > lastweekdate).ToList();
                foreach (var item in attachmentinfoList)
                {
                    foreach (var clientitem in tempCollection.Where(x => x.IsActive == isActive))
                    {
                        if (item.ClientId == Convert.ToInt32(clientitem.UniqEntity))
                        {
                            if (!clientlist.Any(x => x.ClientId == clientitem.ClientId))
                                clientlist.Add(clientitem);
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(waterMarkTextBox.Text?.Trim()))
                    return new ObservableCollection<ClientInfo>(clientlist);
                if (waterMarkTextBox.Text.Length >= 3)
                {
                    var searchedActiveInfoCollection = tempCollection.Where(x => x.IsActive == isActive && (x.ClientDescription.ToUpper().Contains(waterMarkTextBox.Text?.Trim().ToUpper()) || x.EpicCode.ToUpper().Contains(waterMarkTextBox.Text?.Trim().ToUpper()))).ToList();
                    clientlist = clientlist.Where(x => (x.ClientDescription.ToUpper().Contains(waterMarkTextBox.Text?.Trim().ToUpper()) || x.EpicCode.ToUpper().Contains(waterMarkTextBox.Text?.Trim().ToUpper()))).ToList();
                    clientlist.AddRange(searchedActiveInfoCollection.Except(clientlist));
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            clientlist = clientlist.OrderBy(m => m.EpicCode).ToList();
            return new ObservableCollection<ClientInfo>(clientlist);
        }


        ///<summary>
        /// showing only the selected Add to type in add activity when clicking exclude history option in UI.
        /// </summary>
        /// <returns>void</returns>
        private void UpdateDescription()
        {
            try
            {
                string result = string.Empty;
                var policyYearInfo = LstPolicyList.SelectedItem as PolicyYear;
                if (policyYearInfo != null && policyYearInfo.Description != "(none)")
                {
                    policyYearTextBlock.Text = policyYearInfo.Description;
                    policyYearTextBlock.Visibility = Visibility.Visible;

                }
                else
                {
                    policyYearTextBlock.Text = string.Empty;
                    policyYearTextBlock.Visibility = Visibility.Collapsed;
                }
                if ((bool)rbEmailDesc.IsChecked && OLA.ActiveInspector() == null && _mailItems.Count > 0)
                {
                    var subject = _mailItems[0]?.MailItem?.Subject;

                    //if (policyYearInfo != null && policyYearInfo.Description != "(none)" && !string.IsNullOrEmpty(lblPolicyType.Text))
                    //{
                    //    //result = string.Format("{0} - {1} - {2}", policyYearInfo.Description, lblPolicyType.Text, subject);
                    //    result = subject;
                    //}
                    //else if (string.IsNullOrEmpty(lblPolicyType.Text) && policyYearInfo.Description == "(none)")
                    //{
                    //    result = subject;
                    //}
                    //else if (policyYearInfo.Description == "(none)" && !string.IsNullOrEmpty(lblPolicyType.Text) && !string.IsNullOrEmpty(subject))
                    //{
                    //    result = string.Format("{0} - {1}", lblPolicyType.Text, subject);
                    //}
                    //else if (string.IsNullOrEmpty(lblPolicyType.Text) && !string.IsNullOrEmpty(subject))
                    //{
                    //    result = string.Format("{0} - {1}", policyYearInfo.Description, subject);
                    //}

                    txtDesc.Text = subject;
                }
                else if ((bool)rbActivityDec.IsChecked)
                {
                    var activity = string.Empty;
                    PolicyInfo activityInfo = null;
                    if (ActivitiesTab != null && ActivitiesTab.SelectedIndex == 0)
                    {
                        activityInfo = ActiveActivityList.SelectedItem as PolicyInfo;
                    }
                    else
                    {
                        activityInfo = InActiveActivityList.SelectedItem as PolicyInfo;
                    }
                    if (activityInfo != null)
                        activity = activityInfo.PolicyDisplayDesc;

                    //if (policyYearInfo != null && policyYearInfo.Description != "(none)" && !string.IsNullOrEmpty(lblPolicyType.Text))
                    //{
                    //    result = string.Format("{0} - {1} - {2}", policyYearInfo.Description, lblPolicyType.Text, activity);
                    //}
                    //else if (string.IsNullOrEmpty(lblPolicyType.Text) && policyYearInfo.Description == "(none)")
                    //{
                    //    result = activity;
                    //}
                    //else if (policyYearInfo.Description == "(none)" && !string.IsNullOrEmpty(lblPolicyType.Text) && !string.IsNullOrEmpty(activity))
                    //{
                    //    result = string.Format("{0} - {1}", lblPolicyType.Text, activity);
                    //}
                    //else if (string.IsNullOrEmpty(lblPolicyType.Text) && !string.IsNullOrEmpty(activity))
                    //{
                    //    result = string.Format("{0} - {1}", policyYearInfo.Description, activity);
                    //}
                    txtDesc.Text = activity;

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// showing status in right corner of UI.
        /// </summary>
        /// <returns>void</returns>
        internal void ShowStatus(string message, bool isErrorMessage = false)
        {
            TextBlock text = new TextBlock();
            if (!isErrorMessage)
                text.Foreground = System.Windows.Media.Brushes.Green;
            else
                text.Foreground = System.Windows.Media.Brushes.Red;
            text.Text = message;
            text.Style = this.TryFindResource("FaderStyle") as Style;
            FailureAlertGrid.Children.Clear();
            FailureAlertGrid.Children.Add(text);
        }

        private void CloseAllPopups()
        {
            BookMarkListPopupDetails.IsOpen = false;
            BookMarkListPopup.IsOpen = false;
            // AddActivityPopup.IsOpen = false;
            //  grdBackOverLay.Visibility = Visibility.Collapsed;
        }

        private void LoadAllListOfActivityTypes()
        {
            try
            {
                //List<string> ListOfTypesCollection = new List<string>();
                //ListOfTypesCollection.Add("Policy");
                //ListOfTypesCollection.Add("Account");
                //ListOfTypesCollection.Add("Claim");
                //ListOfTypesCollection.Add("Line");
                //ListOfTypesCollection.Add("Opportunities");
                //ListOfTypesCollection.Add("Services");
                //ListOfTypesCollection.Add("Master Marketing Submission");

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        internal void LoadAllValuesFromCommonLookup()
        {
            try
            {
                if (addactivityMainPage != null && addactivityMainPage.addactivityFirstPage != null && addactivityMainPage.addactivitySecondPage != null)
                {
                    var listOfTypes = Globals.ThisAddIn.CommonLookupCollection.Where(m => m.CommonLkpTypeCode == "ActivityType").ToList();
                    addactivityMainPage.addactivityFirstPage.AddToActivityTypeComboBox.ItemsSource = null;
                    addactivityMainPage.addactivityFirstPage.AddToActivityTypeComboBox.ItemsSource = listOfTypes.Select(m => m.CommonLkpName).ToList();
                    if (listOfTypes.Count > 0)
                        addactivityMainPage.addactivityFirstPage.AddToActivityTypeComboBox.SelectedItem = listOfTypes.Where(m => m.CommonLkpCode == "P").Select(m => m.CommonLkpName).FirstOrDefault();


                    var contactModes = Globals.ThisAddIn.CommonLookupCollection.Where(m => m.CommonLkpTypeCode == "ActivityContact").Select(m => m.CommonLkpName).ToList();
                    addactivityMainPage.addactivitySecondPage.contactModeComboBox.ItemsSource = null;
                    addactivityMainPage.addactivitySecondPage.contactModeComboBox.ItemsSource = contactModes;

                    var accessLevel = Globals.ThisAddIn.CommonLookupCollection.Where(m => m.CommonLkpTypeCode == "ActivityAccessLevel").ToList();
                    addactivityMainPage.addactivitySecondPage.accessLevelComboBox.ItemsSource = null;
                    addactivityMainPage.addactivitySecondPage.accessLevelComboBox.ItemsSource = accessLevel;

                    addactivityMainPage.addactivitySecondPage.accessLevelComboBox.SelectedItem = accessLevel.FirstOrDefault(m => m.CommonLkpCode == "PUB");

                    var priority = Globals.ThisAddIn.CommonLookupCollection.Where(m => m.CommonLkpTypeCode == "ActivityPriority").ToList();
                    addactivityMainPage.addactivitySecondPage.PriorityComboBox.ItemsSource = null;
                    addactivityMainPage.addactivitySecondPage.PriorityComboBox.ItemsSource = priority;

                    var updateCollection = Globals.ThisAddIn.CommonLookupCollection.Where(m => m.CommonLkpTypeCode == "ActivityUpdate" && !string.IsNullOrEmpty(m.CommonLkpName)).ToList();
                    if (updateCollection == null)
                        updateCollection = new List<ActivityCommonLookUpInfo>();
                    updateCollection.Insert(0, new ActivityCommonLookUpInfo { CommonLkpCode = "DefaultValue" });
                    addactivityMainPage.addactivitySecondPage.updateComboBox.ItemsSource = updateCollection;

                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting all the employee details from local database which is security policy.
        /// </summary>
        /// <returns>void</returns>
        private void LoadAllEmployeeDetails()
        {
            try
            {
                employeeInfoCollection = new List<EmployeeInfo>();
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(_allEmployeeDetailsInfoQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            EmployeeInfo employeeDetailInfo = new EmployeeInfo
                            {
                                EntityId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqEntity")),
                                LookupCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LookupCode")),
                                EmployeeName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EmployeeName")),
                                Department = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Department")),
                                JobTitle = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("JobTitle")),
                                InactiveDate = SQLite.SQLiteHandler.CheckNull<DateTime>(sqliteDataReader["InactiveDate"]),
                                RoleFlags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("RoleFlags")),
                                Flags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("Flags")),
                                InsertedDate = sqliteDataReader.GetDateTime(sqliteDataReader.GetOrdinal("InsertedDate")),
                                UpdatedDate = sqliteDataReader.GetDateTime(sqliteDataReader.GetOrdinal("UpdatedDate"))
                            };
                            employeeInfoCollection.Add(employeeDetailInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                }
                var isEmployeeExists = employeeInfoCollection.Exists(m => m.LookupCode == ThisAddIn.EmployeeLookupCode);
                if (!isEmployeeExists)
                {
                    Visibility = Visibility.Collapsed;
                }


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting all the employee agency details from local database.
        /// </summary>
        /// <returns>void</returns>
        internal void LoadAllEmployeeWithAgency()
        {
            try
            {
                Globals.ThisAddIn.employeeAgencyInfoCodeCollection = new List<EmployeeAgencyInfo>();
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(_allEmployeeAgencyInfoQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            EmployeeAgencyInfo employeeAgencyInfo = new EmployeeAgencyInfo
                            {
                                EntityId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqEntity")),
                                EmployeeName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EmployeeName")),
                                LookupCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LookupCode")),
                                AgencyId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqAgency")),
                                AgencyCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("AgencyCode")),
                                AgencyName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("AgencyName")),
                                BranchCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("BranchCode")),
                                BranchId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqBranch")),
                                BranchName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("BranchName")),
                                DepartmentCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("DepartmentCode")),
                                Departmetname = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Departmetname")),
                                ProfitCenterCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ProfitCenterCode")),
                                ProfitCenterName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ProfitCenterNAme"))
                            };
                            Globals.ThisAddIn.employeeAgencyInfoCodeCollection.Add(employeeAgencyInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting all the Activity Code and description details from local database.
        /// </summary>
        /// <returns>void</returns>
        internal void LoadAddActivityCodeWithDesc()
        {
            try
            {
                List<AddActivityCode> addActivityCodeCollection = new List<AddActivityCode>();
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(_allActivityListQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            AddActivityCode activityList = new AddActivityCode
                            {
                                ActivityId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqActivityCode")),
                                Code = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ActivityCode")),
                                Description = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ActivityName")),
                                IsClosedStatus = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("IsClosedStatus"))
                            };
                            addActivityCodeCollection.Add(activityList);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                }


                List<AddActivityCode> filteredaddActivityCodeCollection = new List<AddActivityCode>();
                var currentLoggedLookupCode = ThisAddIn.EmployeeLookupCode;

                filteredaddActivityCodeCollection = addActivityCodeCollection.Where(m => !m.Code.StartsWith("S") && !m.Code.StartsWith("T") && !m.Code.StartsWith("H")).ToList();
                if (!string.IsNullOrEmpty(currentLoggedLookupCode))
                {
                    var currentLoggedAgency = DistinctHelper.DistinctBy(Globals.ThisAddIn.employeeAgencyInfoCodeCollection.Where(m => m.LookupCode == currentLoggedLookupCode), m => m.AgencyCode).ToList();
                    if (currentLoggedAgency.Exists(a => a.AgencyCode == "A01"))
                    {
                        filteredaddActivityCodeCollection.AddRange(addActivityCodeCollection.Where(m => m.Code.StartsWith("H") || m.Code.Equals("SYNC")).ToList());
                    }
                    if (currentLoggedAgency.Exists(a => a.AgencyCode == "A02"))
                    {
                        filteredaddActivityCodeCollection.AddRange(addActivityCodeCollection.Where(m => m.Code.StartsWith("S") || m.Code.Equals("SYNC")).ToList());
                    }
                    if (currentLoggedAgency.Exists(a => a.AgencyCode == "A03"))
                    {
                        filteredaddActivityCodeCollection.AddRange(addActivityCodeCollection.Where(m => m.Code.StartsWith("T") || m.Code.Equals("SYNC")).ToList());
                    }
                }
                filteredaddActivityCodeCollection = filteredaddActivityCodeCollection.OrderBy(m => m.Code).ToList();
                if (filteredaddActivityCodeCollection.Count > 0)
                {
                    if (addactivityMainPage != null)
                        addactivityMainPage.addactivitySecondPage.AddActivityCode.ItemsSource = filteredaddActivityCodeCollection;
                    //AddActivityCode.SelectedIndex = 0;
                }


                //addActivityCodeCollection.Add(new AddActivityCode() { Code = "IACT", Description = "Accounting-Financing-Checks-Invoices" });
                //addActivityCodeCollection.Add(new AddActivityCode() { Code = "IAUD", Description = "Audit &LineCode& &LineEffDate&" });
                //addActivityCodeCollection.Add(new AddActivityCode() { Code = "ICO2", Description = "Communication (detail)" });
                //addActivityCodeCollection.Add(new AddActivityCode() { Code = "IFIN", Description = "Financials" });
                //AddActivityCode.ItemsSource = addActivityCodeCollection;
                //AddActivityCode.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// load all owners list for add activity  
        /// </summary>
        /// <returns>void</returns>
        internal void LoadOwnerCodeList()
        {
            try
            {

                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(_allOwnerListQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        OwnerCode activityList = new OwnerCode
                        {
                            Code = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Lookupcode")),
                            Description = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EmployeeName")),
                        };
                        Globals.ThisAddIn.ownerCodeCollection.Add(activityList);
                    }
                    sqliteDataReader.Close();
                }
                if (Globals.ThisAddIn.ownerCodeCollection.Count > 0)
                {
                    if (addactivityMainPage != null)
                    {
                        addactivityMainPage.addactivitySecondPage.ownerComboBox.ItemsSource = null;
                        addactivityMainPage.addactivitySecondPage.ownerComboBox.ItemsSource = Globals.ThisAddIn.ownerCodeCollection;
                    }

                    //  ownerComboBox.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        internal void UpdateFolders()
        {
            var attachmentitemList = GetAttachmentInfoDetails(string.Format(GET_LATEST_ATTACHMENTINFO, ThisAddIn.EmployeeLookupCode));
            if (attachmentitemList.Count > 0)
            {
                var attachmentitem = attachmentitemList.FirstOrDefault();
                if (attachmentitem != null)
                {
                    mainFolderComboBox.SelectedItem = Globals.ThisAddIn.MainFolderInfoCollection.FirstOrDefault(x => x.FolderId == attachmentitem.FolderDetails.ParentFolderId);
                    subFolder1ComboBox.SelectedItem = SubFolder1InfoCollection.FirstOrDefault(x => x.FolderId == attachmentitem.FolderDetails.FolderId);
                    subFolder2ComboBox.SelectedItem = SubFolder2InfoCollection.FirstOrDefault(x => x.FolderId == attachmentitem.FolderDetails.SubFolderId);
                }
            }
            else
            {
                mainFolderComboBox.SelectedItem = null;
                subFolder1ComboBox.SelectedItem = null;
                subFolder1ComboBox.IsEnabled = false;
                subFolder2ComboBox.SelectedItem = null;
                subFolder2ComboBox.IsEnabled = false;
            }

        }

        internal List<SelectedEmailInfo> GetFailedAttachmentsFromLocalDB()
        {
            List<SelectedEmailInfo> FailedEmailInfos = new List<SelectedEmailInfo>();
            var attachmentitemList = GetAttachmentInfoDetails(string.Format(GET_FAILED_ATTACHMENTINFO, ThisAddIn.EmployeeLookupCode));
            foreach (var attachment in attachmentitemList)
            {
                try
                {
                    SelectedEmailInfo selectedEmailInfo = new SelectedEmailInfo();
                    selectedEmailInfo.From = attachment.EmailFromDisplayName;
                    selectedEmailInfo.MailRecievedDateWithTime = attachment.ReceivedDateWithTime;
                    selectedEmailInfo.To = attachment.EmailToDisplayName;
                    selectedEmailInfo.Cc = attachment.EmailCCDisplayName;
                    selectedEmailInfo.Description = attachment.Description;
                    selectedEmailInfo.HtmlBody = attachment.DisplayMailBody;
                    selectedEmailInfo.ErrorMessage = attachment.ErrorMessage;
                    selectedEmailInfo.AttachmentId = attachment.AttachmentId;
                    var clientInfo = GetClientDetailForSpecificAttachment(attachment.ClientId);
                    selectedEmailInfo.Client = string.Format("{0} ({1})", clientInfo.EpicCode, clientInfo.ClientName);
                    selectedEmailInfo.ClientEpicCode = clientInfo.EpicCode;
                    var activityInfo = GetActivityDetailForSpecificAttachment(attachment.ActivityId);
                    selectedEmailInfo.Activity = string.Format("{0} ({1})", activityInfo.ActivityCode, activityInfo.DescriptionOf);
                    FailedEmailInfos.Add(selectedEmailInfo);
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                }
            }

            return FailedEmailInfos;
        }

        public ClientInfo GetClientDetailForSpecificAttachment(long ClientId)
        {
            var queryString = string.Format("select * from HIBOPClient where UniqEntity={0}", ClientId);
            var clientInfo = new ClientInfo();
            var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(queryString);
            if (sqliteDataReader != null)
            {
                while (sqliteDataReader.Read())
                {
                    try
                    {
                        clientInfo.ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity"));
                        clientInfo.ClientName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("NameOf"));
                        clientInfo.EpicCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LookupCode"));
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                    }
                    finally
                    {
                        Logger.save();
                    }
                }
            }
            return clientInfo;
        }

        public ActivityInfo GetActivityDetailForSpecificAttachment(long ActivityId)
        {
            var queryString = string.Format("select * from HIBOPActivity where UniqActivity={0}", ActivityId);
            var activityInfo = new ActivityInfo();
            var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(queryString);
            if (sqliteDataReader != null)
            {
                while (sqliteDataReader.Read())
                {
                    try
                    {
                        activityInfo.ActivityCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ActivityCode"));
                        activityInfo.DescriptionOf = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("DescriptionOf"));
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                    }
                    finally
                    {
                        Logger.save();
                    }
                }
            }
            return activityInfo;
        }

        ///<summary>
        /// getting all the policy years in people pane region for policy year field.
        /// </summary>
        /// <returns>void</returns>
        private void GetPolicyYears()
        {
            try
            {
                List<PolicyYear> policyYearLst = new List<PolicyYear>();
                var currentYear = DateTime.Now.Year;
                var previousYear = DateTime.Now.AddYears(-1).Year;
                var secondpreviousYear = DateTime.Now.AddYears(-2).Year;
                var nextYear = DateTime.Now.AddYears(1).Year;


                var secondPreviousYearValue = new PolicyYear() { Description = secondpreviousYear.ToString(), IsSelected = false };
                policyYearLst.Add(secondPreviousYearValue);

                var previousYearValue = new PolicyYear() { Description = previousYear.ToString(), IsSelected = false };
                policyYearLst.Add(previousYearValue);

                var currentYearValue = new PolicyYear() { Description = currentYear.ToString(), IsSelected = true };
                policyYearLst.Add(currentYearValue);

                var nextYearValue = new PolicyYear() { Description = nextYear.ToString(), IsSelected = false };
                policyYearLst.Add(nextYearValue);

                LstPolicyList.ItemsSource = policyYearLst;

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// Resetting all the values selected by user, when reset button clicks
        /// </summary>
        /// <returns>void</returns>
        internal void ResetAllData()
        {
            try
            {
                BtnAddActivity.IsEnabled = false;
                activityFilterTextBox.Text = "";
                LstActiveClient.SelectedItem = null;
                LstInActiveClient.SelectedItem = null;
                ActiveActivityList.SelectedItem = null;
                InActiveActivityList.SelectedItem = null;
                RBCustomPolicyType.IsChecked = true;
                txtDesc.Text = string.Empty;
                lblPolicyType.Text = "";
                rbCustom.IsChecked = true;
                chkUntilDate.IsChecked = false;
                foreach (var policy in policyYearLst)
                {
                    if (policy.Description == Currentyr.ToString())
                    {
                        policy.IsSelected = true;
                    }
                    else
                    {
                        policy.IsSelected = false;
                    }
                }
                LstPolicyList.ItemsSource = null;
                LstPolicyList.ItemsSource = policyYearLst;
                DpAccesibility.SelectedDate = null;
                //  mainFolderComboBox.SelectedItem = null;
                //subFolder1ComboBox.SelectedItem = null;
                //subFolder1ComboBox.IsEnabled = false;
                //subFolder1ComboBox.ItemsSource = null;
                //subFolder2ComboBox.IsEnabled = false;
                //subFolder2ComboBox.ItemsSource = null;
                UpdateFolders();
                requiredFieldAlert.Visibility = Visibility.Collapsed;
                btnSendAttachment.IsEnabled = false;
                btnSendDeleteAndAttach.IsEnabled = false;
                btnAttachmentOnly.IsEnabled = false;
                btnAttachmentAndDeleteOnly.IsEnabled = false;
                policyTypeTextBlock.Text = string.Empty;
                ClearErrors();

                //OpenActivitiesInfoCollection = null;
                //var v = OpenActivitiesFilteredItems;
                //ClosedActivitiesInfoCollection = null;
                //var v1 = ClosedActivitiesFilteredItems;

                ActiveActivityList.ItemsSource = null;
                ActiveActivityList.ItemsSource = OpenActivitiesFilteredItems;

                InActiveActivityList.ItemsSource = null;
                InActiveActivityList.ItemsSource = ClosedActivitiesFilteredItems;

                if (addactivityMainPage != null)
                    addactivityMainPage.addactivitySecondPage.ClearErrorsforAddActivity();

                LstActiveClient.ItemsSource = null;
                Binding myBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("MyActiveClientFilteredItems")
                };
                BindingOperations.SetBinding(LstActiveClient, ListBox.ItemsSourceProperty, myBinding);

                LstInActiveClient.ItemsSource = null;
                Binding myInActiveClientBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("MyInActiveClientFilteredItems")
                };
                BindingOperations.SetBinding(LstInActiveClient, ListBox.ItemsSourceProperty, myInActiveClientBinding);
                waterMarkTextBox.Text = "";
                ActivitiesTab.SelectedIndex = 0;
                Tab.SelectedIndex = 0;

                //OnPropertyChanged("MyActiveClientFilteredItems");
                //OnPropertyChanged("MyInActiveClientFilteredItems");
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// after attaching,all the selected values will be cleared using this method.
        /// </summary>
        /// <returns>bool</returns>
        private void ClearErrors()
        {
            DpAccesibility.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B8B8B"));
            lblPolicyType.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B8B8B"));
            txtDesc.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B8B8B"));
            mainFolderComboBox.Margin = new Thickness(0);
            brdmainFolderComboBox.Visibility = Visibility.Collapsed;
            subFolder1ComboBox.Margin = new Thickness(0);
            brdsubFolder1ComboBox.Visibility = Visibility.Collapsed;
            subFolder2ComboBox.Margin = new Thickness(0);
            brdsubFolder2ComboBox.Visibility = Visibility.Collapsed;

        }
        ///<summary>
        /// Validate all the fields which are selected by user before doing attachment
        /// </summary>
        /// <returns>bool</returns>
        private bool ValidateAllFields(bool isMultipleEmailAttachment = false)
        {
            var isErrorNotExists = true;
            requiredFieldAlert.Content = "Please fill required fields";
            ClearErrors();
            try
            {
                if (chkUntilDate.IsChecked == true && DpAccesibility.SelectedDate == null)
                {

                    DpAccesibility.BorderBrush = System.Windows.Media.Brushes.Red;
                    isErrorNotExists = false;
                }
                else if (DpAccesibility.SelectedDate != null && DpAccesibility.SelectedDate.Value.Date < DateTime.Now.Date)
                {
                    requiredFieldAlert.Content = "Please enter a valid future date";
                    DpAccesibility.BorderBrush = System.Windows.Media.Brushes.Red;
                    isErrorNotExists = false;
                }
                //if (RBPolicyType.IsChecked == true && string.IsNullOrEmpty(lblPolicyType.Text))
                //{
                //    lblPolicyType.BorderBrush = System.Windows.Media.Brushes.Red;
                //    isErrorNotExists = false;
                //}
                if (!isMultipleEmailAttachment)
                {
                    if (string.IsNullOrEmpty(txtDesc.Text))
                    {
                        txtDesc.BorderBrush = System.Windows.Media.Brushes.Red;
                        isErrorNotExists = false;
                    }
                }

                if (mainFolderComboBox.SelectedItem == null)
                {
                    mainFolderComboBox.Margin = new Thickness(1);
                    brdmainFolderComboBox.Visibility = Visibility.Visible;
                    isErrorNotExists = false;
                }
                if (subFolder1ComboBox.SelectedItem == null)
                {

                    subFolder1ComboBox.Margin = new Thickness(1);
                    brdsubFolder1ComboBox.Visibility = Visibility.Visible;
                    isErrorNotExists = false;
                }
                //if (subFolder2ComboBox.SelectedItem == null)
                //{
                //    subFolder2ComboBox.Margin = new Thickness(1);
                //    brdsubFolder2ComboBox.Visibility = Visibility.Visible;
                //    isErrorNotExists = false;
                //}

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return isErrorNotExists;
        }



        ///<summary>
        /// Check favorites value is already existing in local database
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckFavouriteAlreadyExists(string favouriteName)
        {
            try
            {
                string favouriteExistsQuery = string.Format(_favouriteNameExistsQuery, favouriteName);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(favouriteExistsQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var count = Convert.ToInt32(sqliteDataReader["Count(*)"]);
                            if (count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                return true;
            }
            finally
            {
                Logger.save();
            }
            //catch (Exception)
            //{
            //    return true;
            //    throw;
            //}
            return false;
        }

        private bool CheckAttachmentIdentifierAlreadyExists(string identifierName)
        {
            try
            {
                string attachmentIdentifierQuery = string.Format(_attachmentIdentifierExistsQuery, identifierName, ThisAddIn.EmployeeLookupCode);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelecttQueryWithAdapter(attachmentIdentifierQuery);
                if (sqliteDataReader != null)
                {
                    for (int i = 0; i < sqliteDataReader.Rows.Count; i++)
                    {
                        try
                        {
                            // var count = Convert.ToInt32(sqliteDataReader["Count(*)"]);
                            var count = Convert.ToInt32(sqliteDataReader.Rows[i]["Count(*)"]);
                            if (count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                return true;
            }
            finally
            {
                Logger.save();
            }
            //catch (Exception)
            //{
            //    return true;
            //    throw;
            //}
            return false;
        }

        public Int32 GetFailedAttachmentsCount()
        {
            Int32 FailedCount = 0;
            string attachmentIdentifierQuery = string.Format(_failedAttachementsCountQuery, ThisAddIn.EmployeeLookupCode);
            var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelecttQueryWithAdapter(attachmentIdentifierQuery);
            if (sqliteDataReader != null)
            {
                for (int i = 0; i < sqliteDataReader.Rows.Count; i++)
                {
                    try
                    {
                        FailedCount = Convert.ToInt32(sqliteDataReader.Rows[i]["Count(*)"]);
                        if (FailedCount < 0)
                        {
                            FailedCount = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                    }
                }
            }
            return FailedCount;
        }

        ///<summary>
        /// save favorites data selected by user in local database
        /// </summary>
        /// <returns>bool</returns>
        private Model.FavouriteInfo SaveFavouritesInfoToLocalDataBase()
        {
            //var reult = false;
            var favouriteInfo = new Model.FavouriteInfo();
            try
            {
                var clientSelectedItem = ActiveTab.IsSelected ? (LstActiveClient.SelectedItem as ClientInfo) : (LstInActiveClient.SelectedItem as ClientInfo);
                var activitySelectedItem = OpenActivityTab.IsSelected ? (ActiveActivityList.SelectedItem as PolicyInfo) : (InActiveActivityList.SelectedItem as PolicyInfo);
                var policyTypeSelectedItem = lblPolicyType.Text;
                var folderSelectedItem = mainFolderComboBox.SelectedItem as FolderInfo;
                var subFolder1SelectedItem = subFolder1ComboBox.SelectedItem as FolderInfo;
                var subFolder2SelectedItem = subFolder2ComboBox.SelectedItem as FolderInfo;
                string DescriptionType = string.Empty;
                if ((bool)rbCustom.IsChecked)
                {
                    DescriptionType = CUSTOM;
                }
                else if ((bool)rbEmailDesc.IsChecked)
                {
                    DescriptionType = USE_EMAIL;
                }
                else if ((bool)rbActivityDec.IsChecked)
                {
                    DescriptionType = USE_ACTIVITY;
                }


                favouriteInfo.UniqEmployee = ThisAddIn.EmployeeLookupCode;
                if (clientSelectedItem != null)
                {
                    favouriteInfo.UniqEntity = clientSelectedItem.ClientId;
                    if (clientSelectedItem.IsActive)
                        favouriteInfo.IsActiveClient = 1;
                    else
                        favouriteInfo.IsActiveClient = 0;
                }

                if (activitySelectedItem != null)
                {
                    favouriteInfo.UniqActivity = activitySelectedItem.ActivityId;
                    if (activitySelectedItem.IsClosed)
                        favouriteInfo.IsClosedActivity = 1;
                    else
                        favouriteInfo.IsClosedActivity = 0;
                    favouriteInfo.ActivityGuid = activitySelectedItem.ActivityGuid;
                }

                favouriteInfo.FavourtieName = favouriteNameTextBox.Text;
                favouriteInfo.PolicyYear = (LstPolicyList.SelectedItem as PolicyYear)?.Description;
                favouriteInfo.PolicyType = string.IsNullOrEmpty(policyTypeSelectedItem) ? "(none)" : policyTypeSelectedItem;
                favouriteInfo.DescriptionType = DescriptionType;
                favouriteInfo.Description = txtDesc.Text;
                if (folderSelectedItem != null)
                    favouriteInfo.FolderId = folderSelectedItem.FolderId;
                if (subFolder1SelectedItem != null)
                    favouriteInfo.SubFolder1Id = subFolder1SelectedItem.FolderId;
                if (subFolder2SelectedItem != null)
                    favouriteInfo.SubFolder2Id = subFolder2SelectedItem.FolderId;
                string format = "yyyy-MM-dd HH:mm:ss.fff";
                var clientAccessibleDate =
                favouriteInfo.ClientAccessibleDate = chkUntilDate.IsChecked == true ? (DpAccesibility.SelectedDate != null ? DpAccesibility.SelectedDate.Value.ToString(format) : string.Empty) : string.Empty;
                favouriteInfo.CreatedBy = string.Empty;
                favouriteInfo.CreatedDate = DateTime.Now.ToString(format);
                favouriteInfo.ModifiedBy = string.Empty;
                favouriteInfo.ModifiedDate = DateTime.Now.ToString(format);

                var favouriteInfoCollection = new List<FavouriteInfo>
                {
                    favouriteInfo
                };
                XMLSerializeHelper.Serialize<FavouriteInfo>(favouriteInfoCollection, XMLFolderType.AddIn);

                //string queryString = $"Insert into HIBOPFavourites (FavouriteName,UniqEmployee,UniqEntity,IsActiveClient,UniqActivity,IsClosedActivity,PolicyYear,PolicyType,DescriptionType,Description,FolderId,SubFolder1Id,SubFolder2Id,ClientAccessibleDate,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) Values ('{favouriteInfo.FavourtieName}','{favouriteInfo.UniqEmployee}',{favouriteInfo.UniqEntity},{favouriteInfo.IsActiveClient},{favouriteInfo.UniqActivity},{favouriteInfo.IsClosedActivity},'{favouriteInfo.PolicyYear}','{favouriteInfo.PolicyType}','{favouriteInfo.DescriptionType}','{favouriteInfo.Description}',{favouriteInfo.FolderId},{favouriteInfo.SubFolder1Id},{favouriteInfo.SubFolder2Id},'{favouriteInfo.ClientAccessibleDate}','{favouriteInfo.CreatedBy}','{favouriteInfo.CreatedDate}','{favouriteInfo.ModifiedBy}','{favouriteInfo.ModifiedDate}')";

                //var result = SQLite.SQLiteHandler.ExecuteCreateOrInsertQuery(queryString);
                // reult = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return favouriteInfo;
        }
        ///<summary>
        /// getting all favorites saved in local database to show in Favorite List.
        /// </summary>
        /// <returns>void</returns>
        private void GetAllFavouritesFromLocalDataBase()
        {
            try
            {
                List<FavouriteInfo> favouriteInfoUpdateCollection = new List<FavouriteInfo>();
                string favouriteQuery = string.Format(_allFavouritesQuery, ThisAddIn.EmployeeLookupCode);
                if (FavouriteInfoCollection == null)
                    FavouriteInfoCollection = new List<Model.FavouriteInfo>();
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(favouriteQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            if (sqliteDataReader["FavouriteName"] != null)
                            {
                                Model.FavouriteInfo favouriteInfo = new Model.FavouriteInfo
                                {
                                    FavId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("FavId")),
                                    FavourtieName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("FavouriteName")),
                                    UniqEmployee = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("UniqEmployee")),
                                    UniqEntity = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity")),
                                    IsActiveClient = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("IsActiveClient")),
                                    UniqActivity = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqActivity")),
                                    IsClosedActivity = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("IsClosedActivity")),
                                    PolicyYear = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyYear")),
                                    PolicyType = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyType")),
                                    DescriptionType = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("DescriptionType")),
                                    Description = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Description")),
                                    FolderId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("FolderId")),
                                    SubFolder1Id = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("SubFolder1Id")),
                                    SubFolder2Id = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("SubFolder2Id")),
                                    ClientAccessibleDate = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ClientAccessibleDate")),
                                    CreatedBy = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CreatedBy")),
                                    CreatedDate = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CreatedDate")),
                                    ModifiedBy = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ModifiedBy")),
                                    ModifiedDate = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ModifiedDate")),
                                    ActivityGuid = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ActivityGuid"))
                                };
                                var Favorite = FavouriteInfoCollection.FirstOrDefault(m => m.FavourtieName.ToLower() == favouriteInfo.FavourtieName.ToLower());
                                try
                                {
                                    if (favouriteInfo.UniqActivity == 0)
                                    {
                                        Int32 activityId = 0;
                                        string activityIdQuery = $"select TaskEventEpicId from HIBOPAddActivity where ActivityGuid = '{favouriteInfo.ActivityGuid}'";
                                        var addActivitySqlReader = Helper.Helper.SqliteHelper.ExecuteSelectQuery(activityIdQuery);
                                        while (addActivitySqlReader.Read())
                                        {
                                            try
                                            {
                                                activityId = addActivitySqlReader.GetInt32(addActivitySqlReader.GetOrdinal("TaskEventEpicId"));
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, ThisAddIn.EmployeeLookupCode);
                                            }
                                        }
                                        favouriteInfo.UniqActivity = activityId;
                                        if (activityId != 0)
                                        {
                                            string format = "yyyy-MM-dd HH:mm:ss.fff";
                                            favouriteInfo.ModifiedDate = DateTime.Now.ToString(format);
                                            favouriteInfoUpdateCollection.Add(favouriteInfo);
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, ThisAddIn.EmployeeLookupCode);
                                }

                                if (Favorite != null)
                                {
                                    Favorite.UniqActivity = favouriteInfo.UniqActivity;
                                    Favorite.FavId = favouriteInfo.FavId;
                                    Favorite.ModifiedDate = favouriteInfo.ModifiedDate;
                                    Favorite.ActivityGuid = favouriteInfo.ActivityGuid;
                                }
                                else
                                {
                                    FavouriteInfoCollection.Add(favouriteInfo);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }

                    //if (FavouriteInfoCollection.Count > 0)
                    //{
                    FavouriteInfoCollection = FavouriteInfoCollection.OrderByDescending(m => m.ModifiedDate).ToList();
                    LstFavouriteList.ItemsSource = null;
                    LstFavouriteList.ItemsSource = FavouriteInfoCollection;
                    // }
                    sqliteDataReader.Close();

                    if (favouriteInfoUpdateCollection.Count > 0)
                        XMLSerializeHelper.Serialize<FavouriteInfo>(favouriteInfoUpdateCollection, XMLFolderType.AddIn, "UpdateFavouriteInfo");
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        internal void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        ///<summary>
        /// getting all common lookup which is displayed in add actiivty for Contact Mode, Priority, Access Level, Update.
        /// </summary>
        /// <returns>void</returns>
        internal void GetAllCommonLookupFromLocalDatabase()
        {
            try
            {
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(_allCommonLookupQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            Model.ActivityCommonLookUpInfo commonLookup = new Model.ActivityCommonLookUpInfo
                            {
                                CommonLkpId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("CommonLkpId")),
                                CommonLkpTypeCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CommonLkpTypeCode")),
                                CommonLkpCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CommonLkpCode")),
                                CommonLkpName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CommonLkpName")),
                                CommonLkpDescription = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CommonLkpDescription")),
                                SortOrder = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("SortOrder"))
                            };
                            Globals.ThisAddIn.CommonLookupCollection.Add(commonLookup);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting all type of activity client info details from local database which is used in Add Activity.
        /// </summary>
        /// <returns>void</returns>
        internal void GetAllActivityClientContactInfoFromLocalDatabase()
        {
            try
            {
                Globals.ThisAddIn.ActivityClientContactInfoCollection = new List<Model.ActivityClientContactInfo>();
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(_allActivityContactInfoQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            Model.ActivityClientContactInfo clientContactInfo = new Model.ActivityClientContactInfo
                            {
                                EntityId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqEntity")),
                                ContactNameId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqContactName")),
                                ContactName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ContactName")),
                                ContactType = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ContactType")),
                                ContactValue = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ContactValue"))
                            };
                            Globals.ThisAddIn.ActivityClientContactInfoCollection.Add(clientContactInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        internal List<ClientInfo> GetClientInformationFromSQLite(string queryString)
        {
            var ClientInfoCollection = new List<ClientInfo>();
            try
            {
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(queryString);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var clientinfo = new ClientInfo
                            {
                                ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity")),
                                ClientName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("NameOf")),
                                UniqEntity = Convert.ToString(sqliteDataReader["UniqEntity"]),
                                ClientDescription = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("NameOf")),
                                EpicCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LookupCode")),
                                City = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("City")),
                                Street = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Address")),
                                ZipCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PostalCode")),
                                Status = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Status")),
                                Contact = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PrimaryContactName")),
                                AgencyCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("AgencyCode")),
                                AgencyName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("AgencyName")),
                                State = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("StateName"))
                            };
                            //var clientinfo = new ClientInfo();

                            //clientinfo.ClientId = Convert.ToInt64(sqliteDataReader["UniqEntity"]);
                            //clientinfo.ClientName = Convert.ToString(sqliteDataReader["NameOf"]);
                            //clientinfo.UniqEntity = Convert.ToString(sqliteDataReader["UniqEntity"]);
                            //clientinfo.ClientDescription = Convert.ToString(sqliteDataReader["NameOf"]);
                            //clientinfo.EpicCode = Convert.ToString(sqliteDataReader["LookupCode"]);
                            //clientinfo.City = Convert.ToString(sqliteDataReader["City"]);
                            //clientinfo.Street = Convert.ToString(sqliteDataReader["Address"]);
                            //clientinfo.ZipCode = Convert.ToString(sqliteDataReader["PostalCode"]);
                            //clientinfo.Status = Convert.ToString(sqliteDataReader["Status"]);
                            //clientinfo.Contact = Convert.ToString(sqliteDataReader["PrimaryContactName"]);
                            //clientinfo.AgencyCode = Convert.ToString(sqliteDataReader["AgencyCode"]);
                            //clientinfo.AgencyName = Convert.ToString(sqliteDataReader["AgencyName"]);
                            //clientinfo.State = Convert.ToString(sqliteDataReader["StateName"]);

                            if (!string.IsNullOrEmpty(clientinfo.Status))
                            {
                                if (clientinfo.Status == "Active")
                                {
                                    clientinfo.IsActive = true;
                                }
                                else
                                {
                                    clientinfo.IsActive = false;
                                }
                            }

                            ClientInfoCollection.Add(clientinfo);

                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

            return ClientInfoCollection;
        }

        ///<summary>
        /// getting all type of folders from local database.
        /// </summary>
        /// <returns>void</returns>
        internal List<FolderInfo> GetFoldersFromSQLite(string folderQuery)
        {
            List<FolderInfo> folderInfoCollection = new List<FolderInfo>();
            try
            {

                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(folderQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var folderInfo = new FolderInfo
                            {
                                FolderId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("FolderId")),
                                ParentFolderId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("ParentFolderId")),
                                FolderType = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("FolderType")),
                                FolderName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("FolderName"))
                            };
                            folderInfoCollection.Add(folderInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return folderInfoCollection;
        }

        ///<summary>
        /// getting all the activities for selected client from local database.
        /// </summary>
        /// <returns>void</returns>
        private List<PolicyInfo> GetAcitivtiesFromSQLite(string UniqEntity)
        {
            List<PolicyInfo> PolicyInfoCollection = new List<PolicyInfo>();
            try
            {
                string activitiesQuery = string.Format("Select * From HIBOPActivity A inner join HIBOPActivityEmployee E ON A.UniqActivity=E.UniqActivity where A.UniqEntity={0} and E.EmployeeLookupCode='{1}'", UniqEntity, ThisAddIn.EmployeeLookupCode);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(activitiesQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var policyInfo = new PolicyInfo
                            {
                                ClientId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqEntity")),
                                ActivityId = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqActivity")),
                                PolicyCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ActivityCode")),
                                PolicyDesc = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("DescriptionOf")),
                                PolicyNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyNumber")),
                                PolicyType = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("UniqCdPolicyLineType")),
                                Status = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Status")),
                                Effective = string.IsNullOrEmpty(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EffectiveDate"))) ? DateTime.MinValue : Convert.ToDateTime(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EffectiveDate"))).Date
                            };
                            policyInfo.ShowEffectiveDate = policyInfo.Effective.ToShortDateString();
                            policyInfo.Expiration = string.IsNullOrEmpty(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate"))) ? DateTime.MinValue : Convert.ToDateTime(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate"))).Date;
                            policyInfo.ShowExpirationDate = policyInfo.Expiration.ToShortDateString();
                            policyInfo.UniqAssociatedItem = SQLite.SQLiteHandler.CheckNull<Int32>(sqliteDataReader["UniqAssociatedItem"]);
                            policyInfo.AssociationType = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("AssociationType"));
                            policyInfo.AddToType = policyInfo.AssociationType;
                            policyInfo.UniqAgency = SQLite.SQLiteHandler.CheckNull<Int32>(sqliteDataReader["UniqAgency"]);
                            policyInfo.UniqBranch = SQLite.SQLiteHandler.CheckNull<Int32>(sqliteDataReader["UniqBranch"]);
                            policyInfo.InsertedDate = string.IsNullOrEmpty(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("InsertedDate"))) ? DateTime.MinValue : Convert.ToDateTime(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("InsertedDate")));
                            policyInfo.ClosedDate = string.IsNullOrEmpty(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ClosedDate"))) ? DateTime.MinValue : Convert.ToDateTime(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ClosedDate")));
                            policyInfo.OwnerDescription = SQLite.SQLiteHandler.CheckNull<string>(sqliteDataReader["OwnerDescription"]);
                            policyInfo.UniqPolicy = SQLite.SQLiteHandler.CheckNull<Int32>(Convert.ToInt32(sqliteDataReader["UniqPolicy"]));
                            policyInfo.UniqLine = SQLite.SQLiteHandler.CheckNull<Int32>(Convert.ToInt32(sqliteDataReader["UniqLine"]));
                            policyInfo.UniqClaim = SQLite.SQLiteHandler.CheckNull<Int32>(Convert.ToInt32(sqliteDataReader["UniqClaim"]));
                            policyInfo.LossDate = string.IsNullOrEmpty(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LossDate"))) ? DateTime.MinValue : Convert.ToDateTime(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LossDate")));
                            policyInfo.Policydescription = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyDescription"));
                            policyInfo.LineCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineCode"));
                            policyInfo.LineDescription = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineDescription"));
                            policyInfo.ICO = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ICO"));
                            policyInfo.LineEffectiveDate = string.IsNullOrEmpty(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineEffectiveDate"))) ? DateTime.MinValue : Convert.ToDateTime(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineEffectiveDate"))).Date;
                            policyInfo.LineExpirationDate = string.IsNullOrEmpty(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineExpirationDate"))) ? DateTime.MinValue : Convert.ToDateTime(sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineExpirationDate"))).Date;
                            if (!string.IsNullOrEmpty(policyInfo.Status))
                            {
                                if (policyInfo.Status == "Open")
                                {
                                    policyInfo.IsClosed = false;
                                }
                                else
                                {
                                    policyInfo.IsClosed = true;
                                }
                            }
                            //Task.Run(() =>
                            //{
                            policyInfo.PolicyDisplayDesc = GetDisplayDescforAllActivity(policyInfo);
                            //});
                            PolicyInfoCollection.Add(policyInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }

                    sqliteDataReader.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return PolicyInfoCollection;
        }
        private string GetDisplayDescforAllActivity(PolicyInfo activity)
        {
            string result = string.Empty;
            try
            {
                result = activity.PolicyDesc;
                string clientName = string.Empty;
                if (result.Contains("&AcctName&") || result.Contains("&AccName&"))
                {
                    var ClientQuery = string.Format("Select NameOf From HIBOPClient where UniqEntity={0}", activity.ClientId);
                    var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(ClientQuery);
                    if (sqliteDataReader != null)
                    {
                        while (sqliteDataReader.Read())
                        {
                            try
                            {
                                clientName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("NameOf"));
                                result = result.Replace("&AcctName&", " " + clientName + " ");
                                result = result.Replace("&AccName&", " " + clientName + " ");
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                            }
                        }
                    }
                }

                result = result.Replace("&AttachDesc&", " ");

                switch (activity.AssociationType)
                {
                    case "Policy":
                    case "Line":
                        {
                            if (result.Contains("&PolicyType&") || result.Contains("&PolType&") || result.Contains("&Policy&") || result.Contains("&PolicyExpDate&") || result.Contains("&PolExpDate&") || result.Contains("&PolicyDesc&") || result.Contains("&PolDesc&") || result.Contains("&PolicyEffDate&") || result.Contains("&PolEffDate&"))
                            {
                                result = result.Replace("&PolicyType&", " " + activity.PolicyType + " ");
                                result = result.Replace("&PolType&", " " + activity.PolicyType + " ");
                                //result = result.Replace("&Policy&", " " + linkedPolicy.PolicyNumber + " ");
                                result = result.Replace("&Policy#&", " " + activity.PolicyNumber + " ");
                                result = result.Replace("&PolicyExpDate&", " " + activity.Expiration.ToShortDateString() + " ");
                                result = result.Replace("&PolExpDate&", " " + activity.Expiration.ToShortDateString() + " ");
                                result = result.Replace("&PolicyDesc&", " " + activity.Policydescription + " ");
                                result = result.Replace("&PolDesc&", " " + activity.Policydescription + " ");
                                result = result.Replace("&PolicyEffDate&", " " + activity.Effective.ToShortDateString() + " ");
                                result = result.Replace("&PolEffDate&", " " + activity.Effective.ToShortDateString() + " ");

                            }
                            if (result.Contains("&LineCode&") || result.Contains("&LineEffDate&") || result.Contains("&LineExpDate&") || result.Contains("&LineDesc&") || result.Contains("&ICOName&"))
                            {
                                result = result.Replace("&LineCode&", " " + activity.LineCode + " ");
                                result = result.Replace("&LineEffDate&", " " + activity.LineEffectiveDate.ToShortDateString() + " ");
                                result = result.Replace("&LineExpDate&", " " + activity.LineExpirationDate.ToShortDateString() + " ");
                                result = result.Replace("&LineDesc&", " " + activity.LineDescription + " ");
                                result = result.Replace("&ICOName&", " " + activity.ICO + " ");
                            }
                            result = result.Replace("&DateLoss&", " ");
                            break;
                        }
                    case "Claim":
                        {
                            if (result.Contains("&DateLoss&"))
                            {
                                result = result.Replace("&DateLoss&", " " + activity.LossDate?.ToShortDateString() + " ");
                            }
                            break;
                        }
                    case "Account":
                        {
                            result = result.Replace("&LineCode&", " ");
                            result = result.Replace("&LineEffDate&", " ");
                            result = result.Replace("&LineExpDate&", " ");
                            result = result.Replace("&LineDesc&", " ");
                            result = result.Replace("&ICOName&", " ");
                            result = result.Replace("&PolicyType&", " ");
                            result = result.Replace("&Policy#&", " ");
                            result = result.Replace("&PolicyExpDate&", " ");
                            result = result.Replace("&PolExpDate&", " ");
                            result = result.Replace("&PolicyDesc&", " ");
                            result = result.Replace("&PolDesc&", " ");
                            result = result.Replace("&PolicyEffDate&", " ");
                            result = result.Replace("&DateLoss&", " ");
                            result = result.Replace("&PolType&", " ");
                            result = result.Replace("&PolEffDate&", " ");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return result;
        }


        ///<summary>
        /// creating add activity window
        /// </summary>
        /// <returns>void</returns>
        private void PrepareAddActivityWindow()
        {
            try
            {
                addActivityWindow = new Window();
                addActivityWindow.Closing -= AddActivityWindow_Closing;
                addActivityWindow.Closing += AddActivityWindow_Closing;

                dynamic activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                using (var officeWin32activeWindow = new OfficeWin32Window(activeWindow))
                {
                    IntPtr outlookHwnd = officeWin32activeWindow.Handle;
                    WindowInteropHelper wih = new WindowInteropHelper(addActivityWindow)
                    {
                        Owner = outlookHwnd
                    };
                }

                addActivityWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                addActivityWindow.WindowStyle = WindowStyle.None;
                addActivityWindow.AllowsTransparency = true;
                addActivityWindow.Background = Brushes.Transparent;
                addactivityMainPage = new AddActivityMainPage(addActivityWindow, this);
                Grid mainGrid = new Grid();
                mainGrid.Children.Add(addactivityMainPage);
                mainGrid.Effect = new DropShadowEffect
                {
                    Color = (Color)ColorConverter.ConvertFromString("#0072C6"),
                    Direction = 320,
                    ShadowDepth = 0,
                    Opacity = 1
                };
                addActivityWindow.Content = mainGrid;
                addActivityWindow.Height = 630;
                addActivityWindow.Width = 780;

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void AddActivityWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (sender as Window).Visibility = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }

        }

        ///<summary>
        /// creating Policy List Type window
        /// </summary>
        /// <returns>void</returns>
        internal void PreparePolicyListTypeWindow()
        {
            try
            {
                policyTypeWindow = new Window();
                policyTypeWindow.Closing -= AddActivityWindow_Closing;
                policyTypeWindow.Closing += AddActivityWindow_Closing;

                dynamic activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                using (var officeWin32activeWindow = new OfficeWin32Window(activeWindow))
                {
                    IntPtr outlookHwnd = officeWin32activeWindow.Handle;
                    WindowInteropHelper wih = new WindowInteropHelper(policyTypeWindow)
                    {
                        Owner = outlookHwnd
                    };
                }
                policyTypeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                policyTypeWindow.WindowStyle = WindowStyle.None;
                policyTypeWindow.AllowsTransparency = true;
                policyTypeWindow.Background = Brushes.Transparent;
                policyList = new PolicyTypeList(policyTypeWindow, this);
                policyList.GetPolicyTypeListFromSQLite();
                Grid mainGrid = new Grid();
                mainGrid.Children.Add(policyList);
                mainGrid.Effect = new DropShadowEffect
                {
                    Color = (Color)ColorConverter.ConvertFromString("#0072C6"),
                    Direction = 320,
                    ShadowDepth = 0,
                    Opacity = 1
                };
                policyTypeWindow.Content = mainGrid;
                policyTypeWindow.Height = 400;
                policyTypeWindow.Width = 580;

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting attachment info details from local database which will be used assit attach.
        /// </summary>
        /// <returns>void</returns>
        private List<Model.AttachmentInfo> GetAttachmentInfoDetails(string queryString)
        {
            List<Model.AttachmentInfo> attachmentInfoList = new List<AttachmentInfo>();
            var sqliteDataReaderDataTable = SQLite.SQLiteHandler.ExecuteSelecttQueryWithAdapter(queryString);
            if (sqliteDataReaderDataTable != null)
            {
                for (int i = 0; i < sqliteDataReaderDataTable.Rows.Count; i++)
                {
                    try
                    {
                        var attachmentInfoItem = new Model.AttachmentInfo
                        {
                            AttachmentId = Convert.ToInt32(sqliteDataReaderDataTable.Rows[i]["AttachmentId"]),
                            ClientId = Convert.ToInt64(sqliteDataReaderDataTable.Rows[i]["ClientId"]),
                            IsActiveClient = Convert.ToInt32(sqliteDataReaderDataTable.Rows[i]["IsActiveClient"]) == 0 ? false : true,
                            ActivityId = Convert.ToInt64(sqliteDataReaderDataTable.Rows[i]["ActivityId"]),
                            IsClosedActivity = Convert.ToInt32(sqliteDataReaderDataTable.Rows[i]["IsActiveActivity"]) == 0 ? false : true,
                            Description = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["Description"]),
                            DescriptionFrom = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["DescFrom"]),
                            FileDetails = new Model.FileInfo()
                        };
                        attachmentInfoItem.FileDetails.FileName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["FileName"]);
                        attachmentInfoItem.FileDetails.FilePath = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentFilePath"]);
                        attachmentInfoItem.AttachmentFilePath = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentFilePath"]);
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
                        attachmentInfoItem.ReceivedDateWithTime = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["RecievedDateWithTime"]);
                        attachmentInfoItem.EmailFromAddress = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailFromAddress"]);
                        attachmentInfoItem.EmailToAddress = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailToAddress"]);
                        attachmentInfoItem.Subject = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["Subject"]);
                        attachmentInfoItem.AttachmentFilePath = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentFilePath"]);
                        attachmentInfoItem.IsPushedToEpic = Convert.ToBoolean(sqliteDataReaderDataTable.Rows[i]["IsPushedToEpic"]);
                        attachmentInfoItem.CreatedDate = Convert.ToDateTime(sqliteDataReaderDataTable.Rows[i]["CreatedDate"]);
                        attachmentInfoItem.DomainName = Environment.UserDomainName;
                        attachmentInfoItem.UserName = Environment.UserName;
                        attachmentInfoItem.EntryId = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EntryId"]);
                        attachmentInfoItem.ErrorMessage = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["ErrorMessage"]);
                        attachmentInfoItem.MailBody = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["AttachmentMailBody"]);
                        attachmentInfoItem.ActivityGuid = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["ActivityGuid"]);
                        attachmentInfoItem.EmailFromDisplayName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailFromDisplayName"]);
                        attachmentInfoItem.EmailToDisplayName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailToDisplayName"]);
                        attachmentInfoItem.EmailCCAddress = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailCCAddress"]);
                        attachmentInfoItem.EmailCCDisplayName = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["EmailCCDisplayName"]);
                        attachmentInfoItem.DisplayMailBody = Convert.ToString(sqliteDataReaderDataTable.Rows[i]["DisplayMailBody"]);
                        attachmentInfoList.Add(attachmentInfoItem);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                    }
                }
            }
            return attachmentInfoList;
        }
        private void clientLookupSearch_Click(object sender, RoutedEventArgs e)
        {
            StartSearchClients();
        }

        ///<summary>
        /// getting all the selected mails from outlook application.
        /// </summary>
        /// <returns>List<MailItemInfo></returns>
        internal List<MailItemInfo> GetAllSelectedEmails()
        {
            _mailItems = new List<MailItemInfo>();
            try
            {
                if (explorer != null && explorer.Selection != null && explorer.Selection.Count > 0)
                {

                    var items = explorer.Selection;
                    for (int i = 1; i < items.Count + 1; i++)
                    {
                        if (items[i] is OutlookNS.MailItem)
                        {

                            MailItemInfo mailitem = new MailItemInfo
                            {
                                MailItem = items[i] as OutlookNS.MailItem
                            };
                            //string regex = "<span [^>]*Name=(\"|')IdentifierKey(\"|')>(.*?)</span>";

                            //if (mailitem.MailItem != null && !string.IsNullOrEmpty(mailitem.MailItem.Body))
                            //{
                            //    MatchCollection matches = Regex.Matches(mailitem.MailItem.Body, @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}");
                            //    // string strRegex = @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b";
                            //    //  MatchCollection matches = new Regex(strRegex, RegexOptions.Singleline | RegexOptions.Compiled).Matches(mailitem.MailItem.HTMLBody);

                            //    //if (matches.Count > 0)
                            //    //    mailitem.Identifier = matches[matches.Count - 1].Value;

                            //    for (int y = matches.Count - 1; y >= 0; y--)
                            //    {
                            //        try
                            //        {
                            //            var match = matches[y];
                            //            if (CheckAttachmentIdentifierAlreadyExists(match.Value))
                            //            {
                            //                mailitem.Identifier = match.Value;
                            //                break;
                            //            }
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                            //        }
                            //    }

                            //}
                            mailitem.Identifier = GetIdentifierFromMailItem(mailitem.MailItem);
                            _mailItems.Add(mailitem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

            return _mailItems;
        }

        public string GetIdentifierFromMailItem(OutlookNS.MailItem mailItem)
        {
            string Identifier = string.Empty;
            try
            {
                if (mailItem != null && !string.IsNullOrEmpty(mailItem.Body))
                {
                    MatchCollection matches = Regex.Matches(mailItem.Body, @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}");

                    for (int y = matches.Count - 1; y >= 0; y--)
                    {
                        try
                        {
                            var match = matches[y];
                            if (CheckAttachmentIdentifierAlreadyExists(match.Value))
                            {
                                Identifier = match.Value;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            return Identifier;
        }

        ///<summary>
        /// searching will be done on matching the search text from client collection which will be triggred after clicking search client button
        /// </summary>
        /// <returns>void</returns>
        public void StartSearchClients()
        {
            try
            {
                if (Tab.SelectedIndex == 0)
                {
                    OnPropertyChanged("MyActiveClientFilteredItems");
                }
                else if (Tab.SelectedIndex == 1)
                {
                    OnPropertyChanged("MyInActiveClientFilteredItems");
                }



            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        ///<summary>
        /// searching will be done on matching the search text from activity collection which will be triggred after clicking search activity button
        /// </summary>
        /// <returns>void</returns>
        private void StartSearchActivities()
        {
            try
            {

                if (ActivitiesTab.SelectedIndex == 0)
                {
                    OnPropertyChanged("OpenActivitiesFilteredItems");
                    var selectedItem = ActiveActivityList.SelectedItem;
                    ActiveActivityList.ItemsSource = null;
                    ActiveActivityList.ItemsSource = OpenActivitiesFilteredItems;
                    ActiveActivityList.SelectedItem = selectedItem;
                }
                else if (ActivitiesTab.SelectedIndex == 1)
                {
                    OnPropertyChanged("ClosedActivitiesFilteredItems");
                    var selectedItem = InActiveActivityList.SelectedItem;
                    InActiveActivityList.ItemsSource = null;
                    InActiveActivityList.ItemsSource = ClosedActivitiesFilteredItems;
                    InActiveActivityList.SelectedItem = selectedItem;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }


        ///<summary>
        /// saving the attachment details to local database which will be used later by windows service to push it in epic.
        /// </summary>
        /// <returns>void</returns>
        /// 

        internal string GetDisplayNameFromActiveDirectory()
        {
            string name = "";
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    var usr = UserPrincipal.FindByIdentity(context, Environment.UserName);
                    if (usr != null)
                        name = usr.DisplayName;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return name;

        }
        public void SaveAttachmentInfo(bool isAttachDlete = false, bool isComposeNewMail = false, bool isReplyMail = false)
        {
            try
            {

                var clientSelectedItem = ActiveTab.IsSelected ? (LstActiveClient.SelectedItem as ClientInfo) : (LstInActiveClient.SelectedItem as ClientInfo);
                var activitySelectedItem = OpenActivityTab.IsSelected ? (ActiveActivityList.SelectedItem as PolicyInfo) : (InActiveActivityList.SelectedItem as PolicyInfo);
                var policyTypeSelectedItem = RBPolicyType.IsChecked == true ? (policyList?.DgPolicyType.SelectedItem as PolicyTypeInfo) : null;

                if (policyTypeSelectedItem == null && RBPolicyType.IsChecked == true)
                {
                    policyTypeSelectedItem = policyList.PolicyTypeInfoCollection.FirstOrDefault(x => x.PolicyTypeCode == lblPolicyType.Text);
                }

                var folderSelectedItem = mainFolderComboBox.SelectedItem as FolderInfo;
                var subFolder1SelectedItem = subFolder1ComboBox.SelectedItem as FolderInfo;
                var subFolder2SelectedItem = subFolder2ComboBox.SelectedItem as FolderInfo;
                if (!Directory.Exists(_FilePath))
                {
                    Directory.CreateDirectory(_FilePath);
                }
                var displayName = GetDisplayNameFromActiveDirectory();
                foreach (var item in _mailItems)
                {
                    try
                    {
                        var attachmentIdentifierProperty = item.MailItem?.UserProperties["AttachmentIdentifier"];
                        var MailBody = RemoveHiddenlinksInHtmlMailBody(item.MailItem?.Body);

                        var description = SQLite.SQLiteHandler.DoQuotes(txtDesc.Text);
                        var descriptionFrom = rbCustom.IsChecked == true ? rbCustom.Content.ToString() : rbEmailDesc.IsChecked == true ? rbEmailDesc.Content.ToString() : rbActivityDec.IsChecked == true ? rbActivityDec.Content.ToString() : string.Empty;
                        var filename = string.Format("{0}\\MailItem_{1}.msg", _FilePath, DateTime.Now.Ticks);
                        Globals.ThisAddIn._FilePathForReplyMail = filename;
                        var attachmentInfo = new Model.AttachmentInfo
                        {
                            ClientId = clientSelectedItem.ClientId,
                            ActivityId = activitySelectedItem.ActivityId,
                            ActivityGuid = activitySelectedItem.ActivityGuid,
                            IsClosedActivity = activitySelectedItem.IsClosed,
                            IsActiveClient = clientSelectedItem.IsActive,
                            DescriptionFrom = descriptionFrom,
                            Description = description.Length > Convert.ToInt32(ConfigurationManager.AppSettings["DescriptionLength"]) ? description.Remove(Convert.ToInt32(ConfigurationManager.AppSettings["DescriptionLength"]))?.Replace("'", "") : description.Replace("'", ""),// rbCustom.IsChecked == true ? txtDesc.Text : rbEmailDesc.IsChecked == true ? item?.MailItem.Subject : rbActivityDec.IsChecked == true ? clientSelectedItem.ClientDescription : string.Empty;
                            //FileDetails = new Model.FileInfo { FileExtension = ".msg", FileName = SQLite.SQLiteHandler.DoQuotes(item.MailItem.Subject?.Replace("'", "")) },
                            FileDetails = new Model.FileInfo { FileExtension = ".msg", FileName = string.IsNullOrEmpty(item.MailItem.Subject?.Trim()) ? SQLite.SQLiteHandler.DoQuotes(description.Replace("'", "")) : SQLite.SQLiteHandler.DoQuotes(item.MailItem.Subject?.Replace("'", "")) },
                            FolderDetails = new Model.FolderInfo { ParentFolderId = folderSelectedItem?.FolderId, ParentFolderName = folderSelectedItem?.FolderName, FolderId = subFolder1SelectedItem?.FolderId, FolderName = subFolder1SelectedItem?.FolderName, SubFolderId = subFolder2SelectedItem == null ? 0 : subFolder2SelectedItem.FolderId, SubFolderName = subFolder2SelectedItem?.FolderName },
                            PolicyCode = RBPolicyType.IsChecked == true ? lblPolicyType.Text : string.Empty,
                            PolicyType = RBPolicyType.IsChecked == false ? "(none)" : policyTypeSelectedItem == null ? string.Empty : policyTypeSelectedItem?.PolicyTypeDescription,
                            PolicyYear = (LstPolicyList.SelectedItem as PolicyYear)?.Description,
                            ClientAccessible = chkUntilDate.IsChecked == true ? (DpAccesibility?.SelectedDate != null ? ExtensionClass.SqliteDateTimeFormat(DpAccesibility?.SelectedDate.Value) : ExtensionClass.SqliteDateTimeFormat(default(Nullable<DateTime>))) : ExtensionClass.SqliteDateTimeFormat(default(Nullable<DateTime>)),
                            IsClientAccessible = (bool)chkUntilDate.IsChecked,
                            EmailFromAddress = GetSenderMailId(item.MailItem, true)?.Replace("'", ""),
                            EmailFromDisplayName = GetSenderMailId(item.MailItem)?.Replace("'", ""),
                            EmailToAddress = GetListOfToMailId(item.MailItem, OutlookNS.OlMailRecipientType.olTo, true)?.Replace("'", ""),//item.MailItem.To,
                            EmailToDisplayName = GetListOfToMailId(item.MailItem, OutlookNS.OlMailRecipientType.olTo)?.Replace("'", ""),
                            EmailCCAddress = GetListOfToMailId(item.MailItem, OutlookNS.OlMailRecipientType.olCC, true)?.Replace("'", ""),
                            EmailCCDisplayName = GetListOfToMailId(item.MailItem, OutlookNS.OlMailRecipientType.olCC)?.Replace("'", ""),
                            Subject = SQLite.SQLiteHandler.DoQuotes(item.MailItem.Subject)?.Replace("'", ""),
                            MailBody = MailBody?.Length > Convert.ToInt32(ConfigurationManager.AppSettings["SubjectLeng"]) ? MailBody?.Remove(Convert.ToInt32(ConfigurationManager.AppSettings["SubjectLeng"]))?.Replace("'", "") : MailBody.Replace("'", ""),
                            CreatedDate = Convert.ToDateTime(Common.Common.UniversalDateTimeConversionToSQLite(DateTime.Now)),
                            ModifiedDate = Convert.ToDateTime(Common.Common.UniversalDateTimeConversionToSQLite(DateTime.Now)),
                            IsAttachDelete = isAttachDlete,
                            ReceivedDate = String.Format("{0:MM/dd/yyyy}", item.MailItem.ReceivedTime),
                            ReceivedDateWithTime = item.MailItem.ReceivedTime.ToString("ddd MM/dd/yyyy hh:mm tt"),
                            AttachmentFilePath = filename,
                            Identifier = item.Identifier,
                            EmployeeCode = ThisAddIn.EmployeeLookupCode,
                            AttachmentMailBody = MailBody?.Length > Convert.ToInt32(ConfigurationManager.AppSettings["SubjectLeng"]) ? MailBody?.Remove(Convert.ToInt32(ConfigurationManager.AppSettings["SubjectLeng"]))?.Replace("'", "") : MailBody.Replace("'", ""),
                            DomainName = Environment.UserDomainName,
                            UserName = string.IsNullOrEmpty(displayName) ? Environment.UserName : displayName,
                            AttachmentIdentifier = attachmentIdentifierProperty?.Value?.ToString(),
                            EntryId = item.MailItem?.EntryID,
                            DisplayMailBody = item.MailItem?.HTMLBody.Replace("'", "").Trim()
                        };

                        try
                        {
                            if (isComposeNewMail)
                            {
                                var outlookversion = Globals.ThisAddIn.Application.Version;
                                if (!outlookversion.StartsWith("10"))
                                {
                                    DateTime sentOnDateTime = DateTime.UtcNow;
                                    string PR_CLIENT_SUBMIT_TIME = "http://schemas.microsoft.com/mapi/proptag/0x00390040";
                                    if (item.MailItem.PropertyAccessor != null)
                                        item.MailItem.PropertyAccessor.SetProperty(PR_CLIENT_SUBMIT_TIME, sentOnDateTime);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        if (!isReplyMail)
                            item.MailItem.SaveAs(filename);

                        if (isReplyMail)
                        {
                            var attachmentFilePath = item.MailItem.UserProperties.Add("AttachmentFilePath", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                            attachmentFilePath.Value = filename;
                        }

                        var attachmentInfoCollection = new List<AttachmentInfo>
                        {
                            attachmentInfo
                        };
                        XMLSerializeHelper.Serialize<AttachmentInfo>(attachmentInfoCollection, XMLFolderType.AddIn);
                        if (policyList != null)
                            policyList.DgPolicyType.SelectedItem = null;
                        //string queryString = $"Insert into AttachmentInfo (ClientId,ActivityId,Description,FileExtension,FileName,ParentFolderId,ParentFolderName,FolderId,FolderName,SubFolderId,SubFolderName,PolicyCode,PolicyType,PolicyYear,ClientAccessible,EmailFromAddress,EmailToAddress,Subject,ReceivedDate,AttachmentFilePath,IsPushedToEpic,IsAttachDelete,Identifier,IsActiveClient,IsActiveActivity,DescFrom,EmployeeCode,CreatedDate,ModifiedDate,AttachmentMailBody,IsDeletedFromZFolder) Values ({attachmentInfo.ClientId},{attachmentInfo.ActivityId},'{attachmentInfo.Description}','{attachmentInfo.FileDetails.FileExtension}','{attachmentInfo.FileDetails.FileName}',{attachmentInfo.FolderDetails.ParentFolderId},'{attachmentInfo.FolderDetails.ParentFolderName}',{attachmentInfo.FolderDetails.FolderId},'{attachmentInfo.FolderDetails.FolderName}','{attachmentInfo.FolderDetails.SubFolderId}','{attachmentInfo.FolderDetails.SubFolderName}','{attachmentInfo.PolicyCode}','{attachmentInfo.PolicyType}','{attachmentInfo.PolicyYear}','{attachmentInfo.ClientAccessible}','{attachmentInfo.EmailFromAddress}','{attachmentInfo.EmailToAddress}','{attachmentInfo.Subject}','{attachmentInfo.ReceivedDate}','{attachmentInfo.AttachmentFilePath}',{0},{Convert.ToInt32(attachmentInfo.IsAttachDelete)},'{attachmentInfo.Identifier}',{Convert.ToInt32(attachmentInfo.IsActiveClient)},{Convert.ToInt32(attachmentInfo.IsClosedActivity)}, '{attachmentInfo.DescriptionFrom}','{ThisAddIn.EmployeeLookupCode}','{Common.Common.DateTimeSQLite(DateTime.Now)}','{Common.Common.DateTimeSQLite(DateTime.Now)}','{attachmentInfo.AttachmentMailBody}',{0})";
                        //var result = SQLite.SQLiteHandler.ExecuteCreateOrInsertQuery(queryString);

                        //string logAuditqueryString = $"Insert into HIBOPOutlookPluginLog (UniqId,AttachmentInfoId,UniqEmployee,UniqEntity,UniqActivity,PolicyYear,PolicyType,DescriptionType,Description,FolderId,SubFolder1Id,SubFolder2Id,ClientAccessibleDate,EmailAction,InsertedDate,UpdatedDate,EmailSubject) Values ('{attachmentInfo.Identifier}',{result.RowId},'{ThisAddIn.EmployeeLookupCode}',{attachmentInfo.ClientId},{attachmentInfo.ActivityId},'{attachmentInfo.PolicyYear}','{attachmentInfo.PolicyType}','{attachmentInfo.DescriptionFrom}','{attachmentInfo.Description}',{attachmentInfo.FolderDetails.ParentFolderId},{attachmentInfo.FolderDetails.FolderId},{attachmentInfo.FolderDetails.SubFolderId},'{SqlDateTime.Null}','{""}','{ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}','{ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}','{attachmentInfo.Subject}')";
                        //var Auditresult = SQLite.SQLiteHandler.ExecuteCreateOrInsertQuery(logAuditqueryString);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex.StackTrace, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.StackTrace, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        public OutlookNS.Account GetAccountForEmailAddress(OutlookNS.Application application, string smtpAddress)
        {
            try
            {
                // Loop over the Accounts collection of the current Outlook session.
                OutlookNS.Accounts accounts = application.Session.Accounts;
                foreach (OutlookNS.Account account in accounts)
                {
                    // When the e-mail address matches, return the account.
                    if (account.SmtpAddress == smtpAddress)
                    {
                        return account;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            return default(OutlookNS.Account);

        }

        internal string RemoveHiddenlinksInHtmlMailBody(string htmlBody)
        {
            var result = string.Empty;
            try
            {
                htmlBody = htmlBody.Trim().Replace("\r", " ");
                htmlBody = htmlBody.Trim().Replace("\n", " ");
                /*For removing anchor tags <a href>*/
                result = Regex.Replace(htmlBody, "</?(a|A).*?>", "");


                Regex removemailToRegex = new Regex(@"(?<=\<mailto:).*(?=\>)");

                result = removemailToRegex.Replace(result, string.Empty);

                result = result.Replace("<mailto:>", string.Empty);

                Regex removehttpRegex = new Regex(@"(?<=\<http:).*(?=\>)");

                result = removehttpRegex.Replace(result, string.Empty);

                result = result.Replace("<http:>", string.Empty);

                Regex removehttpsRegex = new Regex(@"(?<=\<https:).*(?=\>)");

                result = removehttpsRegex.Replace(result, string.Empty);

                result = result.Replace("<https:>", string.Empty);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return result;
        }

        internal string GetMailAddressFromDistributionList(OutlookNS.Recipient recip)
        {
            string result = string.Empty;
            try
            {
                OutlookNS.NameSpace nameSpace = Globals.ThisAddIn?.Application?.GetNamespace("MAPI");
                nameSpace.Logon("", "", false, false);
                if (recip.AddressEntry.Type.ToLower() == "EX".ToLower())
                {
                    string entryId = recip.EntryID;
                    string Address = nameSpace.GetAddressEntryFromID(entryId)?.GetExchangeDistributionList()?.PrimarySmtpAddress;
                    result = Address;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return result;


        }

        internal string GetListOfToMailId(OutlookNS.MailItem currentMailItem, OutlookNS.OlMailRecipientType addressType, bool isMailItem = false)
        {
            List<string> toMailId = new List<string>();

            var recipList = currentMailItem?.Recipients;
            foreach (OutlookNS.Recipient recip in recipList)
            {
                try
                {
                    var bb = recip.Resolve();
                    var cc = currentMailItem?.Recipients?.ResolveAll();
                    if (recip.Type == (int)addressType)
                    {
                        if (recip.AddressEntry.Type?.ToLower() == "ex")
                        {
                            if (isMailItem)
                            {

                                var x1 = recip.AddressEntry.GetContact();


                                if (recip.AddressEntry.GetExchangeUser() == null)
                                {
                                    toMailId.Add(recip.AddressEntry.GetExchangeDistributionList()?.PrimarySmtpAddress);
                                }
                                else
                                {
                                    toMailId.Add(recip.AddressEntry.GetExchangeUser()?.PrimarySmtpAddress);
                                }
                            }
                            else
                            {
                                if (recip.AddressEntry.GetExchangeUser() == null)
                                {
                                    toMailId.Add(recip.AddressEntry.GetExchangeDistributionList()?.Name);
                                }
                                else
                                {
                                    toMailId.Add(recip.AddressEntry?.GetExchangeUser()?.Name);
                                }
                            }
                        }
                        else if (recip.AddressEntry.Type?.ToLower() == "smtp")
                        {
                            if (isMailItem)
                                toMailId.Add(recip.Address);
                            else
                                toMailId.Add(recip.Name);
                        }
                        else
                        {
                            if (isMailItem)
                                toMailId.Add(currentMailItem?.SendUsingAccount?.SmtpAddress);
                            else
                                toMailId.Add(currentMailItem?.SendUsingAccount?.UserName);
                        }

                    }
                }
                catch (Exception ex)
                {

                    Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                }
            }
            return string.Join(",", toMailId?.Distinct().ToArray());
        }


        internal string GetSenderMailId(OutlookNS.MailItem currentMailItem, bool isMailItem = false)
        {
            var fromMailId = string.Empty;
            try
            {
                if (currentMailItem.SenderEmailType?.ToLower() == "ex")
                {
                    OutlookNS.AddressEntry sender = currentMailItem.Sender;
                    if (sender != null)
                    {
                        if (sender.AddressEntryUserType == OutlookNS.OlAddressEntryUserType.olExchangeUserAddressEntry || sender.AddressEntryUserType == OutlookNS.OlAddressEntryUserType.olExchangeRemoteUserAddressEntry)
                        {
                            OutlookNS.ExchangeUser exchUser = sender.GetExchangeUser();
                            if (exchUser != null)
                            {
                                if (isMailItem)
                                    fromMailId = exchUser.PrimarySmtpAddress;
                                else
                                    fromMailId = exchUser.Name;
                            }
                        }
                    }

                }
                else if (currentMailItem.SenderEmailType?.ToLower() == "smtp")
                {
                    if (isMailItem)
                        fromMailId = currentMailItem?.SenderEmailAddress;
                    else
                        fromMailId = currentMailItem?.SenderName;
                }
                else
                {
                    if (isMailItem)
                        fromMailId = currentMailItem?.SendUsingAccount?.SmtpAddress;
                    else
                        fromMailId = currentMailItem?.SendUsingAccount?.UserName;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return fromMailId;
        }

        internal OutlookNS.MAPIFolder GetCustomFolder(string folderName)
        {

            try
            {
                // var   ns = OL.GetNamespace("MAPI");
                //  inboxFolder = ns.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);

                OutlookNS.MAPIFolder oFolders = OLA.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderInbox);
                if (!IsCustomFolderExisits(oFolders, folderName))
                {
                    customFolder = oFolders.Folders.Add(folderName);
                }
                else
                {
                    foreach (OutlookNS.MAPIFolder item in oFolders.Folders)
                    {
                        if (item.Name == folderName)
                        {

                            customFolder = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return customFolder;
        }

        public bool IsCustomFolderExisits(OutlookNS.MAPIFolder rootFolder, string folderName)
        {
            bool isFolderExist = false;
            try
            {
                OutlookNS._Application outlookObj = new OutlookNS.Application();
                foreach (OutlookNS.MAPIFolder subFolder in rootFolder.Folders)
                {
                    if (subFolder.Name == folderName)
                    {
                        isFolderExist = true;
                        // customFolder = subFolder;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return isFolderExist;
        }

        private void EnableSaveButons()
        {
            if ((LstActiveClient.SelectedItem != null || LstInActiveClient.SelectedItem != null) && (ActiveActivityList.SelectedItem != null || InActiveActivityList.SelectedItem != null))
            {
                btnNext.IsEnabled = true;
                if (attachanddeleteonly.Visibility == Visibility.Collapsed)
                {
                    btnSendAttachment.IsEnabled = true;
                    btnSendDeleteAndAttach.IsEnabled = true;
                }
                else
                {
                    btnAttachmentOnly.IsEnabled = true;
                    btnAttachmentAndDeleteOnly.IsEnabled = true;
                }
            }
            else
            {
                btnNext.IsEnabled = false;
                if (attachanddeleteonly.Visibility == Visibility.Collapsed)
                {
                    btnSendAttachment.IsEnabled = false;
                    btnSendDeleteAndAttach.IsEnabled = false;
                }
                else
                {
                    btnAttachmentOnly.IsEnabled = false;
                    btnAttachmentAndDeleteOnly.IsEnabled = false;
                }
            }
        }

        #endregion

        #region Add Activity

        ///<summary>
        /// showing only the selected Add to type in add activity when Add to dropdown selected.
        /// </summary>
        /// <returns>void</returns>
        internal void GetActivity(string selectedActivity)
        {
            try
            {
                if (addactivityMainPage != null && addactivityMainPage.addactivityFirstPage != null)
                {
                    addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                    ClientInfo selectedClient = null;
                    if (Tab.SelectedIndex == 0)
                    {
                        selectedClient = LstActiveClient.SelectedItem as ClientInfo;
                    }
                    else if (Tab.SelectedIndex == 1)
                    {
                        selectedClient = LstInActiveClient.SelectedItem as ClientInfo;
                    }
                    var currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                    var ConditionDate = DateTime.Now.AddMonths(-18).ToString("yyyy/MM/dd");
                    addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.Visibility = Visibility.Visible;
                    switch (selectedActivity)
                    {
                        case "Account":
                            {
                                WriteColumnsToAddActivityGridFromClass<Account>(selectedActivity);
                                GetAccountDetailsFromLocalDatabase(selectedClient);
                                addactivityMainPage.addactivityFirstPage.tableHeader.Content = "Account Structures";
                                addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.Visibility = Visibility.Collapsed;
                                break;
                            }
                        case "Claim":
                            {
                                WriteColumnsToAddActivityGridFromClass<Claim>(selectedActivity);
                                GetClaimsDetailsFromLocalDatabase(selectedClient);
                                addactivityMainPage.addactivityFirstPage.tableHeader.Content = "Claims";
                                addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.Visibility = Visibility.Collapsed;
                                break;
                            }
                        case "Policy":
                            {
                                string policyActivitiesQuery = string.Empty;

                                policyActivitiesQuery = string.Format("Select * From HIBOPPolicy where UniqEntity={0} and Flags={1} and EmployeeLookUpCode='{2}'", selectedClient.UniqEntity, 1, ThisAddIn.EmployeeLookupCode);

                                WriteColumnsToAddActivityGridFromClass<Policy>(selectedActivity);
                                GetPolicyDetailsFromLocalDatabase(policyActivitiesQuery, true);
                                addactivityMainPage.addactivityFirstPage.tableHeader.Content = "Policies";
                                addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.Content = "Include history";

                                if ((bool)addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.IsChecked)
                                {
                                    policyActivitiesQuery = string.Format("Select * From HIBOPPolicy where UniqEntity={0} and Flags={1} and (ExpirationDate >='{2}' or ExpirationDate is null) and EmployeeLookUpCode='{3}'", selectedClient.UniqEntity, 0, ConditionDate, ThisAddIn.EmployeeLookupCode);
                                    GetPolicyDetailsFromLocalDatabase(policyActivitiesQuery, false);
                                }
                                break;
                            }

                        case "Line":
                            {
                                string linesActivitiesQuery = string.Empty;

                                linesActivitiesQuery = string.Format("Select * From HIBOPActivityLine where UniqEntity={0} and Flags={1} and EmployeeLookUpCode='{2}'", selectedClient.UniqEntity, 1, ThisAddIn.EmployeeLookupCode);
                                WriteColumnsToAddActivityGridFromClass<Line>(selectedActivity);
                                GetLinesDetailsFromLocalDatabase(linesActivitiesQuery, true);
                                addactivityMainPage.addactivityFirstPage.tableHeader.Content = "Lines of Business";
                                addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.Content = "Include history";


                                if ((bool)addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.IsChecked)
                                {
                                    linesActivitiesQuery = string.Format("Select * From HIBOPActivityLine where UniqEntity={0} and Flags={1} and (ExpirationDate >='{2}' or ExpirationDate is null) and EmployeeLookUpCode='{3}'", selectedClient.UniqEntity, 0, ConditionDate, ThisAddIn.EmployeeLookupCode);
                                    GetLinesDetailsFromLocalDatabase(linesActivitiesQuery, false);
                                }
                                break;
                            }
                        case "Opportunities":
                            {
                                string OpportunityActivitiesQuery = string.Empty;

                                OpportunityActivitiesQuery = string.Format("Select * From HIBOPActivityOpportunity where UniqEntity={0} and Flags={1} and EmployeeLookUpCode='{2}'", selectedClient.UniqEntity, 1, ThisAddIn.EmployeeLookupCode);
                                WriteColumnsToAddActivityGridFromClass<Opportunities>(selectedActivity);
                                GetOpportunitiesDetailsFromLocalDatabase(OpportunityActivitiesQuery, true);
                                addactivityMainPage.addactivityFirstPage.tableHeader.Content = "Opportunities";
                                addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.Content = "Include closed";

                                if ((bool)addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.IsChecked)
                                {
                                    OpportunityActivitiesQuery = string.Format("Select * From HIBOPActivityOpportunity where UniqEntity={0} and Flags={1} and (ExpirationDate >='{2}' or ExpirationDate is null) and EmployeeLookUpCode='{3}'", selectedClient.UniqEntity, 0, ConditionDate, ThisAddIn.EmployeeLookupCode);
                                    GetOpportunitiesDetailsFromLocalDatabase(OpportunityActivitiesQuery, false);
                                }
                                break;
                            }
                        case "Services":
                            {
                                string servicesActivitiesQuery = string.Empty;

                                servicesActivitiesQuery = string.Format("Select * From HIBOPActivityServices where UniqEntity={0} and Flags={1} and EmployeeLookUpCode='{2}'", selectedClient.UniqEntity, 1, ThisAddIn.EmployeeLookupCode);
                                WriteColumnsToAddActivityGridFromClass<Services>(selectedActivity);
                                GetServiceDetailsFromLocalDatabase(servicesActivitiesQuery, true);
                                addactivityMainPage.addactivityFirstPage.tableHeader.Content = "Services";
                                addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.Content = "Include closed";

                                if ((bool)addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.IsChecked)
                                {
                                    servicesActivitiesQuery = string.Format("Select * From HIBOPActivityServices where UniqEntity={0} and Flags={1} and (ExpirationDate >='{2}' or ExpirationDate is null) and EmployeeLookUpCode='{3}'", selectedClient.UniqEntity, 0, ConditionDate, ThisAddIn.EmployeeLookupCode);
                                    GetServiceDetailsFromLocalDatabase(servicesActivitiesQuery, false);
                                }
                                break;
                            }
                        case "Master Marketing Submission":
                            {
                                string masterMarketingActivitiesQuery = string.Empty;

                                masterMarketingActivitiesQuery = string.Format("Select * From HIBOPActivityMasterMarketing where UniqEntity={0} and Flags={1} and EmployeeLookUpCode='{2}'", selectedClient.UniqEntity, 1, ThisAddIn.EmployeeLookupCode);

                                WriteColumnsToAddActivityGridFromClass<MasterMarketingSubmission>(selectedActivity);
                                GetMasterMarketingSubmissionDetailsFromLocalDatabase(masterMarketingActivitiesQuery, true);
                                addactivityMainPage.addactivityFirstPage.tableHeader.Content = "Master Marketing Submissions";
                                addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.Content = "Include history";

                                if ((bool)addactivityMainPage.addactivityFirstPage.includeHistoryorClosed.IsChecked)
                                {
                                    masterMarketingActivitiesQuery = string.Format("Select * From HIBOPActivityMasterMarketing where UniqEntity={0} and Flags={1} and (ExpirationDate >='{2}' or ExpirationDate is null) and EmployeeLookUpCode='{3}'", selectedClient.UniqEntity, 0, ConditionDate, ThisAddIn.EmployeeLookupCode);
                                    GetMasterMarketingSubmissionDetailsFromLocalDatabase(masterMarketingActivitiesQuery, false);
                                }

                                break;
                            }
                            //default:
                            //    {
                            //        WriteColumnsToAddActivityGridFromClass<Policy>();
                            //        GetPolicyDetailsFromLocalDatabase(selectedClient);
                            //        tableHeader.Content = "Policies";
                            //        includeHistoryorClosed.Content = "Include history";
                            //        break;
                            //    }

                    }

                    //addactivityMainPage.addactivityFirstPage.dgAddActivity.SelectedIndex = 0;

                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }

        ///<summary>
        /// writing all the columns from selected Add To Type in Data Grid with generic type.
        /// </summary>
        /// <returns>void</returns>
        private void WriteColumnsToAddActivityGridFromClass<T>(string selectedActivity)
        {
            try
            {
                if (addactivityMainPage != null)
                {
                    addactivityMainPage.addactivityFirstPage.dgAddActivity.Columns.Clear();
                    PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(typeof(T));
                    foreach (PropertyDescriptor property in collection)
                    {
                        try
                        {
                            if (property.DisplayName != "No display")
                            {
                                DataGridTextColumn column = new DataGridTextColumn
                                {
                                    Header = property.DisplayName,
                                    Binding = new Binding(property.Name),
                                    Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto)
                                };
                                if (selectedActivity == "Line")
                                {
                                    column.MinWidth = 60;
                                }
                                else
                                {
                                    column.MinWidth = 100;
                                }

                                addactivityMainPage.addactivityFirstPage.dgAddActivity.Columns.Add(column);
                            }

                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting the account detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>Account</returns>
        private Account GetAccountDetailForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string accountActivitiesQuery = string.Format("Select * From HIBOPActivityAccount where UniqEntity={0} and UniqAgency={1} and UniqBranch={2} and LookupCode='{3}' limit 1", selectedActivity.ClientId, selectedActivity.UniqAgency, selectedActivity.UniqBranch, ThisAddIn.EmployeeLookupCode);
                var accountInfo = new Account();
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(accountActivitiesQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            accountInfo.AccountId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("AccountId"));
                            accountInfo.ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity"));
                            accountInfo.UniqAgency = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqAgency"));
                            accountInfo.UniqBranch = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqBranch"));
                            accountInfo.AgencyCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("AgencyCode"));
                            accountInfo.AgencyName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("AgencyName"));
                            accountInfo.BranchCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("BranchCode"));
                            accountInfo.BranchName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("BranchName"));
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    return accountInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(Account);
        }

        //Include History not applicable
        ///<summary>
        /// getting all the account details for selected add to type in add activity.
        /// </summary>
        /// <returns>void</returns>
        private void GetAccountDetailsFromLocalDatabase(ClientInfo selectedClient)
        {
            try
            {
                if (selectedClient != null)
                {
                    List<Account> accountCollection = new List<Account>();
                    string accountActivitiesQuery = string.Format("Select * From HIBOPActivityAccount where UniqEntity={0} and LookupCode='{1}'", selectedClient.UniqEntity, ThisAddIn.EmployeeLookupCode);
                    var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(accountActivitiesQuery);
                    if (sqliteDataReader != null)
                    {
                        while (sqliteDataReader.Read())
                        {
                            try
                            {
                                var accountInfo = new Account
                                {
                                    AccountId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("AccountId")),
                                    ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity")),
                                    UniqAgency = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqAgency")),
                                    UniqBranch = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("UniqBranch")),
                                    AgencyCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("AgencyCode")),
                                    AgencyName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("AgencyName")),
                                    BranchCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("BranchCode")),
                                    BranchName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("BranchName")),
                                    LookupCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LookupCode"))
                                };
                                accountCollection.Add(accountInfo);
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                            }


                        }
                        sqliteDataReader.Close();
                        if (accountCollection.Count > 0)
                        {
                            if (addactivityMainPage != null)
                            {
                                addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                                var distinctactivityAccounts = accountCollection.GroupBy(item => new { item.ClientId, item.AgencyCode, item.BranchCode, item.LookupCode }).Select(group => group.First()).ToList();
                                if (distinctactivityAccounts?.Count > 0)
                                    addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = distinctactivityAccounts;
                                else
                                    addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = accountCollection;
                            }

                        }

                    }
                    //accountCollection.Add(new Account { AgencyCode = "A01", AgencyName = "Heffernan Insurance Brokers", BranchCode = "101", BranchName = "(WF)Heffernan Insurance Brokers" });
                    //accountCollection.Add(new Account { AgencyCode = "A02", AgencyName = "Heffernan Insurance Brokers", BranchCode = "103", BranchName = "(SF)Heffernan Insurance Brokers" });
                    //dgAddActivity.ItemsSource = accountCollection;
                }


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting the policy detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>Policy</returns>
        private Policy GetPolicyDetailsForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string policyActivitiesQuery = string.Format("Select * From HIBOPPolicy where UniqPolicy={0} and EmployeeLookUpCode='{1}' limit 1", selectedActivity.UniqAssociatedItem, ThisAddIn.EmployeeLookupCode);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(policyActivitiesQuery);
                var policyInfo = new Policy();
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            policyInfo.PolicyId = SQLite.SQLiteHandler.CheckNull<Int32>(sqliteDataReader["UniqPolicy"]);
                            policyInfo.ClientId = SQLite.SQLiteHandler.CheckNull<Int64>(sqliteDataReader["UniqEntity"]);
                            policyInfo.Type = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CdPolicyLineTypeCode"));
                            policyInfo.Status = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyStatus"));
                            policyInfo.Effective = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EffectiveDate"));
                            policyInfo.Expiration = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate"));
                            policyInfo.PolicyNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyNumber"));
                            policyInfo.PolicyDescription = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("DescriptionOf"));
                            policyInfo.Flags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("Flags"));

                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                    return policyInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(Policy);
        }

        //Include History not applicable
        ///<summary>
        /// getting all the policy details for selected add to type in add activity.
        /// </summary>
        /// <returns>void</returns>
        private void GetPolicyDetailsFromLocalDatabase(string policyActivitiesQuery, bool isCollectionRefreshNeeded)
        {
            try
            {

                if (policyCollection == null || isCollectionRefreshNeeded)
                {
                    policyCollection = new List<Policy>();
                }

                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(policyActivitiesQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var policyInfo = new Policy
                            {
                                PolicyId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqPolicy")),
                                ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity")),
                                Type = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CdPolicyLineTypeCode")),
                                Status = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyStatus")),
                                Effective = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EffectiveDate")),
                                Expiration = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate")),
                                PolicyNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyNumber")),
                                PolicyDescription = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("DescriptionOf")),
                                Flags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("Flags")),
                                DelFlag = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("DelFlag"))
                            };
                            if (policyCollection.Where(m => m.PolicyId == policyInfo.PolicyId).Count() == 0)
                                policyCollection.Add(policyInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    if (policyCollection.Count > 0)
                    {
                        if (addactivityMainPage != null)
                        {
                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = policyCollection.Where(m => m.DelFlag != -1).ToList();

                        }

                    }

                }

                //policyCollection.Add(new Policy { Type = "CBND", Status = "Contracted", Effective = "1/20/2016", Expiration = "1/20/2018", PolicyNumber = "POLIYC#", PolicyDescription = "Test Bond" });
                //policyCollection.Add(new Policy { Type = "CBOP", Status = "Prospective", Effective = "11/15/2016", Expiration = "11/15/2017", PolicyNumber = "TEST", PolicyDescription = "Business Owners Policy" });
                //policyCollection.Add(new Policy { Type = "CGLI", Status = "Contracted", Effective = "10/26/2017", Expiration = "11/15/2018", PolicyNumber = "QUOTING", PolicyDescription = "General Liability" });
                //dgAddActivity.ItemsSource = policyCollection;


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting the claim detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>Claim</returns>
        private Claim GetClaimsDetailsForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string policyActivitiesQuery = string.Format("Select * From HIBOPClaim where UniqClaim={0} and EmployeeLookUpCode='{1}' limit 1", selectedActivity.UniqAssociatedItem, ThisAddIn.EmployeeLookupCode);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(policyActivitiesQuery);
                var claimInfo = new Claim();
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            claimInfo.ClaimId = Convert.ToInt64(sqliteDataReader["UniqClaim"]);
                            claimInfo.ClientId = Convert.ToInt64(sqliteDataReader["UniqEntity"]);
                            claimInfo.DateOfLoss = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LossDate"));
                            claimInfo.DateReported = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ReportedDate"));
                            claimInfo.AgencyClaimNumber = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("ClaimNumber"));
                            claimInfo.CoClaimNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CompanyClaimNumber"));
                            claimInfo.DateClosed = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ClosedDate"));

                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    return claimInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(Claim);
        }

        //Include History not applicable
        ///<summary>
        /// getting all the claim details for selected add to type in add activity.
        /// </summary>
        /// <returns>void</returns>
        private void GetClaimsDetailsFromLocalDatabase(ClientInfo selectedClient)
        {
            try
            {
                if (selectedClient != null)
                {
                    List<Claim> claimCollection = new List<Claim>();

                    string policyActivitiesQuery = "Select * From HIBOPClaim where UniqEntity=" + selectedClient.UniqEntity;
                    var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(policyActivitiesQuery);
                    if (sqliteDataReader != null)
                    {
                        while (sqliteDataReader.Read())
                        {
                            try
                            {
                                var claimInfo = new Claim();
                                claimInfo.ClaimId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqClaim"));
                                claimInfo.ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity"));
                                claimInfo.DateOfLoss = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LossDate"));
                                claimInfo.DateReported = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ReportedDate"));
                                claimInfo.AgencyClaimNumber = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("ClaimNumber"));
                                claimInfo.CoClaimNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("CompanyClaimNumber"));
                                claimInfo.DateClosed = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ClosedDate"));
                                claimInfo.DelFlag = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("DelFlag"));
                                claimCollection.Add(claimInfo);
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                            }


                        }
                        sqliteDataReader.Close();
                        if (claimCollection.Count > 0)
                        {
                            if (addactivityMainPage != null)
                            {
                                addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                                addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = claimCollection.Where(m => m.DelFlag != -1).ToList();
                            }
                        }

                    }

                    //claimCollection.Add(new Claim { DateOfLoss = "6/14/2012", DateReported = "8/5/2013", AgencyClaimNumber = "3341", CoClaimNumber = "5647", DateClosed = "8/10/2013" });
                    //claimCollection.Add(new Claim { DateOfLoss = "3/14/2014", DateReported = "3/14/2014", AgencyClaimNumber = "5058", CoClaimNumber = "7678", DateClosed = "3/18/2014" });
                    //claimCollection.Add(new Claim { DateOfLoss = "4/21/2014", DateReported = "3/21/2014", AgencyClaimNumber = "5400", CoClaimNumber = "4657", DateClosed = "3/25/2014" });
                    //dgAddActivity.ItemsSource = claimCollection;
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting the Line detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>Line</returns>
        private Line GetLineDetailsForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string linesActivitiesQuery = string.Format("Select * From HIBOPActivityLine where UniqLine={0} and EmployeeLookUpCode='{1}' limit 1", selectedActivity.UniqAssociatedItem, ThisAddIn.EmployeeLookupCode);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(linesActivitiesQuery);
                var lineInfo = new Line();
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            lineInfo.LineId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqLine"));
                            lineInfo.UniqPolicy = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqPolicy"));
                            lineInfo.ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity"));
                            lineInfo.PolicyDescription = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyDesc"));
                            lineInfo.LineCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineCode"));
                            lineInfo.LineOfBusiness = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineOfBusiness"));
                            lineInfo.Status = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineStatus"));
                            lineInfo.PolicyNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyNumber"));
                            lineInfo.ICO = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("IOC"));
                            lineInfo.Billing = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("BillModeCode"));
                            lineInfo.Expiration = sqliteDataReader.GetDateTime(sqliteDataReader.GetOrdinal("ExpirationDate")).ToShortDateString();
                            lineInfo.Effective = sqliteDataReader.GetDateTime(sqliteDataReader.GetOrdinal("EffectiveDate")).ToShortDateString();
                            lineInfo.Flags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("Flags"));
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                    return lineInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(Line);

        }

        ///<summary>
        /// getting all the line details for selected add to type in add activity.
        /// </summary>
        /// <returns>void</returns>
        private void GetLinesDetailsFromLocalDatabase(string linesActivitiesQuery, bool isCollectionRefreshNeeded)
        {
            try
            {
                if (lineCollection == null || isCollectionRefreshNeeded)
                {
                    lineCollection = new List<Line>();
                }

                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(linesActivitiesQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var lineInfo = new Line
                            {
                                LineId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqLine")),
                                ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity")),
                                UniqPolicy = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqPolicy")),
                                LineCode = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineCode")),
                                LineOfBusiness = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineOfBusiness")),
                                Status = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineStatus")),
                                Effective = Convert.ToDateTime(sqliteDataReader["EffectiveDate"]).ToShortDateString(),
                                Expiration = Convert.ToDateTime(sqliteDataReader["ExpirationDate"]).ToShortDateString(),
                                PolicyNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyNumber")),
                                ICO = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("IOC")),
                                Billing = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("BillModeCode")),
                                PolicyDescription = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("PolicyDesc")),
                                Flags = Convert.ToInt32(sqliteDataReader["Flags"]),
                                DelFlag = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("DelFlag"))
                            };
                            if (lineCollection.Where(m => m.LineId == lineInfo.LineId).Count() == 0)
                                lineCollection.Add(lineInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    if (lineCollection.Count > 0)
                    {
                        if (addactivityMainPage != null)
                        {
                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = lineCollection.Where(m => m.DelFlag != -1).ToList();
                        }
                    }

                }
                //lineCollection.Add(new Line { LineCode = "CAUT", LineOfBusiness = "Commerce", Status = "Quoting", Effective = "11/1/2016", Expiration = "11/1/2017", PolicyNumber = "MALIKO#", ICO = "TRACAI", Billing = "A", PolicyDescription = "Commercial Purpose" });
                //lineCollection.Add(new Line { LineCode = "CBND", LineOfBusiness = "Bond", Status = "Personnel", Effective = "11/1/2016", Expiration = "11/1/2017", PolicyNumber = "POLIYC#", ICO = "TRACAI", Billing = "A", PolicyDescription = "Test Bond" });
                //lineCollection.Add(new Line { LineCode = "CBOP", LineOfBusiness = "Business", Status = "Quoting", Effective = "11/1/2016", Expiration = "11/1/2017", PolicyNumber = "TEST#", ICO = "QUOTED", Billing = "A", PolicyDescription = "Business Owner" });
                //dgAddActivity.ItemsSource = lineCollection;

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting all the opportunity details for selected add to type in add activity.
        /// </summary>
        /// <returns>void</returns>
        private void GetOpportunitiesDetailsFromLocalDatabase(string OpportunityActivitiesQuery, bool isCollectionRefreshNeeded)
        {
            try
            {
                if (lineCollection == null || isCollectionRefreshNeeded)
                {
                    List<Opportunities> opportunitiesCollection = new List<Opportunities>();
                }
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(OpportunityActivitiesQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var opportunityInfo = new Opportunities();
                            opportunityInfo.OpportunityId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqOpportunity"));
                            opportunityInfo.ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity"));
                            opportunityInfo.Description = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("OppDesc"));
                            opportunityInfo.TargetCloseDate = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("TargetedDate"));
                            opportunityInfo.Owner = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("OwnerName"));
                            //opportunityInfo.SalesTeam = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("SalesTeam"));
                            opportunityInfo.SalesManager = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("SalesManager"));
                            opportunityInfo.Stage = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Stage"));
                            opportunityInfo.Flags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("Flags"));
                            opportunityInfo.DelFlag = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("DelFlag"));

                            if (opportunitiesCollection.Where(m => m.OpportunityId == opportunityInfo.OpportunityId).Count() == 0)
                                opportunitiesCollection.Add(opportunityInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    if (opportunitiesCollection.Count > 0)
                    {
                        if (addactivityMainPage != null)
                        {
                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = opportunitiesCollection.Where(m => m.DelFlag != -1).ToList();
                        }
                    }

                }
                //opportunitiesCollection.Add(new Opportunities { Description = "Commercial Lines", TargetCloseDate = "3/18/2013", Owner = "Heffernan", SalesTeam = "Employee Benefits", SalesManager = "Mike Heffernan", Stage = "Diaired for Response" });
                //opportunitiesCollection.Add(new Opportunities { Description = "Professional Liability", TargetCloseDate = "3/18/2013", Owner = "Heffernan", SalesTeam = "Employee Benefits", SalesManager = "Mike Heffernan", Stage = "Diaired for Response" });
                //opportunitiesCollection.Add(new Opportunities { Description = "2014 Package", TargetCloseDate = "3/18/2013", Owner = "Heffernan", SalesTeam = "Employee Benefits", SalesManager = "Mike Heffernan", Stage = "Diaired for Response" });
                //dgAddActivity.ItemsSource = opportunitiesCollection;


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting the Opportunity detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>Opportunities</returns>
        private Opportunities GetOpportunitiesForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string OpportunityActivitiesQuery = string.Format("Select * From HIBOPActivityOpportunity where UniqOpportunity={0} and EmployeeLookUpCode='{1}' limit 1", selectedActivity.UniqAssociatedItem, ThisAddIn.EmployeeLookupCode);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(OpportunityActivitiesQuery);
                var opportunityInfo = new Opportunities();
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {

                            opportunityInfo.OpportunityId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqOpportunity"));
                            opportunityInfo.ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity"));
                            opportunityInfo.Description = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("OppDesc"));
                            opportunityInfo.TargetCloseDate = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("TargetedDate"));
                            opportunityInfo.Owner = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("OwnerName"));
                            // opportunityInfo.SalesTeam = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("SalesTeam"));
                            opportunityInfo.SalesManager = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("SalesManager"));
                            opportunityInfo.Stage = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Stage"));
                            opportunityInfo.Flags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("Flags"));

                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    return opportunityInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(Opportunities);
        }

        ///<summary>
        /// getting the Service detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>Services</returns>
        private Services GetServiceDetailsForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string servicesActivitiesQuery = string.Format("Select * From HIBOPActivityServices where UniqServiceHead={0} and EmployeeLookUpCode='{1}' limit 1", selectedActivity.UniqAssociatedItem, ThisAddIn.EmployeeLookupCode);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(servicesActivitiesQuery);

                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var serviceInfo = new Services
                            {
                                ServiceHeaderId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqServiceHead")),
                                ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity")),
                                ServiceId = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ServiceNumber")),
                                Code = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("UniqCdServiceCode")),
                                Description = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Description")),
                                ContactNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ContractNumber")),
                                InceptionDate = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("InceptionDate")),
                                Expiration = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate")),
                                Flags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("Flags"))
                            };
                            return serviceInfo;
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    return default(Services);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(Services);
        }

        ///<summary>
        /// getting all the service details for selected add to type in add activity.
        /// </summary>
        /// <returns>void</returns>
        private void GetServiceDetailsFromLocalDatabase(string servicesActivitiesQuery, bool isCollectionRefreshNeeded)
        {
            try
            {

                if (servicesCollection == null || isCollectionRefreshNeeded)
                {
                    servicesCollection = new List<Services>();
                }
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(servicesActivitiesQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var serviceInfo = new Services
                            {
                                ServiceHeaderId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqServiceHead")),
                                ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity")),
                                ServiceId = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ServiceNumber")),
                                Code = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("UniqCdServiceCode")),
                                Description = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("Description")),
                                ContactNumber = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ContractNumber")),
                                InceptionDate = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("InceptionDate")),
                                Expiration = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate")),
                                Flags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("Flags")),
                                DelFlag = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("DelFlag"))
                            };
                            if (servicesCollection.Where(m => m.ServiceHeaderId == serviceInfo.ServiceHeaderId).Count() == 0)
                                servicesCollection.Add(serviceInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    if (servicesCollection.Count > 0)
                    {
                        if (addactivityMainPage != null)
                        {
                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = servicesCollection.Where(m => m.DelFlag != -1).ToList();
                        }
                    }

                }

                //servicesCollection.Add(new Services { ServiceId = "1", Code = "HBRK", Description = "HIB Broker Fee", ContactNumber = "9600677858", InceptionDate = "10/20/2017", Expiration = "10/31/2018" });
                //servicesCollection.Add(new Services { ServiceId = "1", Code = "HBRK", Description = "HIB Broker Fee", ContactNumber = "9600677858", InceptionDate = "10/20/2017", Expiration = "10/31/2018" });
                //servicesCollection.Add(new Services { ServiceId = "1", Code = "HBRK", Description = "HIB Broker Fee", ContactNumber = "9600677858", InceptionDate = "10/20/2017", Expiration = "10/31/2018" });
                //dgAddActivity.ItemsSource = servicesCollection;


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting the master marketing detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>MasterMarketingSubmission</returns>
        private MasterMarketingSubmission GetMasterMarketingForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string masterMarketingActivitiesQuery = string.Format("Select * From HIBOPActivityMasterMarketing where UniqMarketingSubmission={0} and EmployeeLookUpCode='{1}' limit 1", selectedActivity.UniqAssociatedItem, ThisAddIn.EmployeeLookupCode);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(masterMarketingActivitiesQuery);

                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var masterMarketingInfo = new MasterMarketingSubmission
                            {
                                MasterMarketingId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqMarketingSubmission")),
                                ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity")),
                                MasterMarketing = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("DescriptionOf")),
                                // LinesOfBusiness = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LineOfBusiness")),
                                Effective = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EffectiveDate")),
                                Expiration = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate")),
                                LastSubmitted = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LastSubmittedDate")),
                                Flags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("Flags"))
                            };
                            return masterMarketingInfo;
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    return default(MasterMarketingSubmission);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(MasterMarketingSubmission);
        }

        ///<summary>
        /// getting all the master marketing details for selected add to type in add activity.
        /// </summary>
        /// <returns>void</returns>
        private void GetMasterMarketingSubmissionDetailsFromLocalDatabase(string masterMarketingActivitiesQuery, bool isCollectionRefreshNeeded)
        {
            try
            {
                if (masterMarketingCollection == null || isCollectionRefreshNeeded)
                {
                    masterMarketingCollection = new List<MasterMarketingSubmission>();
                }

                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(masterMarketingActivitiesQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var masterMarketingInfo = new MasterMarketingSubmission
                            {
                                MasterMarketingId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqMarketingSubmission")),
                                ClientId = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("UniqEntity")),
                                MasterMarketing = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("DescriptionOf")),
                                //LinesOfBusiness = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("UniqCdPolicyLineType")),
                                Effective = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EffectiveDate")),
                                Expiration = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate")),
                                LastSubmitted = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("LastSubmittedDate")),
                                Flags = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("Flags")),
                                DelFlag = sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("DelFlag"))

                            };
                            if (masterMarketingCollection.Where(m => m.MasterMarketingId == masterMarketingInfo.MasterMarketingId).Count() == 0)
                                masterMarketingCollection.Add(masterMarketingInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                    if (masterMarketingCollection.Count > 0)
                    {
                        if (addactivityMainPage != null)
                        {
                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = null;
                            addactivityMainPage.addactivityFirstPage.dgAddActivity.ItemsSource = masterMarketingCollection.Where(m => m.DelFlag != -1).ToList();
                        }
                    }

                    //masterMarketingCollection.Add(new MasterMarketingSubmission { MasterMarketing = "Commercial Line Marketing", LinesOfBusiness = "CAUT,CWCK", Effective = "3/18/2013", Expiration = "3/18/2014", LastSubmitted = "3/18/2013" });
                    //masterMarketingCollection.Add(new MasterMarketingSubmission { MasterMarketing = "Professional Liability", LinesOfBusiness = "CMIS", Effective = "4/17/2014", Expiration = "4/17/2015", LastSubmitted = "3/18/2013" });
                    //masterMarketingCollection.Add(new MasterMarketingSubmission { MasterMarketing = "2014 Package", LinesOfBusiness = "CAUT,CGLI,CPRG", Effective = "11/1/2014", Expiration = "11/1/2015", LastSubmitted = "3/18/20137" });
                    //dgAddActivity.ItemsSource = masterMarketingCollection;
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        ///<summary>
        /// getting the bill detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>ActivityBillInfo</returns>
        private ActivityBillInfo GetActivityBillForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string activityBillForActivitiesQuery = string.Format("Select * From HIBOPActivityBill where UniqTranshead ={0} limit 1", selectedActivity.UniqAssociatedItem);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(activityBillForActivitiesQuery);
                var activityBillInfo = new ActivityBillInfo();
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            activityBillInfo.BillNumber = SQLite.SQLiteHandler.CheckNull<Int32>(sqliteDataReader["BillNumber"]);
                            activityBillInfo.AgencyName = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("AgencyName"));
                            activityBillInfo.Balance = SQLite.SQLiteHandler.CheckNull<decimal?>(sqliteDataReader["Balance"]);
                            activityBillInfo.Amount = SQLite.SQLiteHandler.CheckNull<decimal?>(sqliteDataReader["Amount"]);
                            activityBillInfo.BillId = SQLite.SQLiteHandler.CheckNull<Int32>(sqliteDataReader["BillId"]);
                            activityBillInfo.AgencyId = SQLite.SQLiteHandler.CheckNull<Int32>(sqliteDataReader["UniqAgency"]);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    return activityBillInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(ActivityBillInfo);
        }

        ///<summary>
        /// getting the Carrier detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>ActivityCarrierInfo</returns>
        private ActivityCarrierInfo GetActivityCarrierSubmissionForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string activityCarrierForActivitiesQuery = string.Format("Select * From HIBOPCarrierSubmission where UniqCarrierSubmission ={0} limit 1", selectedActivity.UniqAssociatedItem);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(activityCarrierForActivitiesQuery);
                var activityCarrierInfo = new ActivityCarrierInfo();
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            activityCarrierInfo.MarketingSubmission = SQLite.SQLiteHandler.CheckNull<string>(sqliteDataReader["MarkettingSubmission"]);
                            activityCarrierInfo.Carrier = SQLite.SQLiteHandler.CheckNull<string>(sqliteDataReader["Carrier"]);
                            activityCarrierInfo.CarrierSubmission = SQLite.SQLiteHandler.CheckNull<string>(sqliteDataReader["CarrierSubmission"]);
                            activityCarrierInfo.LastSubmittedDate = SQLite.SQLiteHandler.CheckNull<DateTime?>(sqliteDataReader["LastSubmittedDate"]);
                            activityCarrierInfo.RequestedPremium = SQLite.SQLiteHandler.CheckNull<decimal?>(sqliteDataReader["RequestedPremium"]);
                            activityCarrierInfo.SubmissionStatus = SQLite.SQLiteHandler.CheckNull<string>(sqliteDataReader["SubmissionStatus"]);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }

                    }
                    sqliteDataReader.Close();
                    return activityCarrierInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(ActivityCarrierInfo);
        }

        ///<summary>
        /// getting the Transaction detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>ActivityTransactionInfo</returns>
        private ActivityTransactionInfo GetActivityTransactionForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string activityTransactionForActivitiesQuery = string.Format("Select * From HIBOPActivityTransaction where UniqTranshead ={0} limit 1", selectedActivity.UniqAssociatedItem);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(activityTransactionForActivitiesQuery);
                var activityTransactionInfo = new ActivityTransactionInfo();
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            activityTransactionInfo.InvoiceNumber = SQLite.SQLiteHandler.CheckNull<Int32>(sqliteDataReader["InvoiceNumber"]);
                            activityTransactionInfo.Code = SQLite.SQLiteHandler.CheckNull<string>(sqliteDataReader["Code"]);
                            activityTransactionInfo.DescriptionOf = SQLite.SQLiteHandler.CheckNull<string>(sqliteDataReader["DescriptionOf"]);
                            activityTransactionInfo.Amount = SQLite.SQLiteHandler.CheckNull<decimal?>(sqliteDataReader["Amount"]);
                            activityTransactionInfo.Balance = SQLite.SQLiteHandler.CheckNull<decimal?>(sqliteDataReader["Balance"]);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    return activityTransactionInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(ActivityTransactionInfo);
        }

        ///<summary>
        /// getting the Certificate detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>ActivityCertificateInfo</returns>
        private ActivityCertificateInfo GetActivityCertificateForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string activityCertificateForActivitiesQuery = string.Format("Select * From HIBOPActivityCertificate where UniqCertificate ={0} limit 1", selectedActivity.UniqAssociatedItem);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(activityCertificateForActivitiesQuery);
                var activityCertificateInfo = new ActivityCertificateInfo();
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            activityCertificateInfo.InsertedDate = SQLite.SQLiteHandler.CheckNull<DateTime>(sqliteDataReader["InsertedDate"]);
                            activityCertificateInfo.Title = SQLite.SQLiteHandler.CheckNull<string>(sqliteDataReader["Title"]);
                            activityCertificateInfo.UpdatedDate = SQLite.SQLiteHandler.CheckNull<DateTime>(sqliteDataReader["UpdatedDate"]);

                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    return activityCertificateInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(ActivityCertificateInfo);
        }

        ///<summary>
        /// getting the Evidence detail for selected activity which will be shown in multiple email attachment second page.
        /// </summary>
        /// <returns>ActivityEvidenceInfo</returns>
        private ActivityEvidenceInfo GetActivityEvidenceForActivityFromLocalDatabase(PolicyInfo selectedActivity)
        {
            try
            {
                string activityEvidenceForActivitiesQuery = string.Format("Select * From HIBOPActivityEvidence where UniqEvidence ={0} limit 1", selectedActivity.UniqAssociatedItem);
                var sqliteDataReader = SQLite.SQLiteHandler.ExecuteSelectQuery(activityEvidenceForActivitiesQuery);
                var activityEvidenceInfo = new ActivityEvidenceInfo();
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            activityEvidenceInfo.InsertedDate = SQLite.SQLiteHandler.CheckNull<DateTime>(sqliteDataReader["InsertedDate"]);
                            activityEvidenceInfo.Title = SQLite.SQLiteHandler.CheckNull<string>(sqliteDataReader["Title"]);
                            activityEvidenceInfo.UpdatedDate = SQLite.SQLiteHandler.CheckNull<DateTime>(sqliteDataReader["UpdatedDate"]);
                            activityEvidenceInfo.FormEditionDate = SQLite.SQLiteHandler.CheckNull<string>(sqliteDataReader["FormEditionDate"]);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                        }


                    }
                    sqliteDataReader.Close();
                    return activityEvidenceInfo;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return default(ActivityEvidenceInfo);
        }


        #endregion


        private void PrepareFailedAttachmentWindow()
        {
            try
            {
                failedAttachmentWindow = new Window();
                dynamic activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                IntPtr outlookHwnd = new OfficeWin32Window(activeWindow).Handle;
                WindowInteropHelper wih = new WindowInteropHelper(failedAttachmentWindow);
                wih.Owner = outlookHwnd;
                failedAttachmentWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                failedAttachmentWindow.WindowStyle = WindowStyle.None;
                failedAttachmentWindow.ResizeMode = ResizeMode.NoResize;
                failedAttachmentWindow.Background = System.Windows.Media.Brushes.Transparent;
                Globals.ThisAddIn.failedAttachments = new FailedAttachments(failedAttachmentWindow);
                Globals.ThisAddIn.failedAttachments._attachmentControl = this;
                Grid mainGrid = new Grid();
                mainGrid.Children.Add(Globals.ThisAddIn.failedAttachments);
                mainGrid.Background = Brushes.White;
                mainGrid.Effect = new DropShadowEffect
                {
                    Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#0072C6"),
                    Direction = 320,
                    ShadowDepth = 0,
                    Opacity = 1
                };
                failedAttachmentWindow.Content = mainGrid;
                failedAttachmentWindow.Height = 620;
                failedAttachmentWindow.Width = 1005;


                double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                double windowWidth = failedAttachmentWindow.Width;
                double windowHeight = failedAttachmentWindow.Height;
                failedAttachmentWindow.Left = (screenWidth / 2) - (windowWidth / 2);
                failedAttachmentWindow.Top = (screenHeight / 3.1) - (windowHeight / 3.1);
                Globals.ThisAddIn.failedAttachments.FailedNotificationCollection = GetFailedAttachmentsFromLocalDB();
                Globals.ThisAddIn.failedAttachments.LstFailedAttachment.ItemsSource = null;
                Globals.ThisAddIn.failedAttachments.LstFailedAttachment.ItemsSource = Globals.ThisAddIn.failedAttachments.FailedNotificationCollection;
                failedAttachmentWindow.ShowDialog();

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
        }



        private void FailedBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PrepareFailedAttachmentWindow();
        }
    }

    public static class MailClone
    {
        public static OutlookNS.MailItem CopyTo(this OutlookNS.MailItem fromMailItem, OutlookNS.MailItem toMailItem)
        {

            toMailItem.Subject = fromMailItem.Subject;
            toMailItem.Body = fromMailItem.Body;
            toMailItem.To = fromMailItem.To;
            toMailItem.CC = fromMailItem.CC;
            toMailItem.BCC = fromMailItem.BCC;

            return toMailItem;
        }
    }

    public class ClientInfo
    {
        public long ClientId { get; set; }

        public string UniqEntity { get; set; }
        public string EpicCode { get; set; }
        public string ClientName { get; set; }
        public string Street { get; set; }
        public string ClientDescription { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public String Contact { get; set; }
        public string ZipCode { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
    }
    public class PolicyInfo
    {
        public Int32 ClientId { get; set; }
        public long ActivityId { get; set; }
        public string PolicyType { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyCode { get; set; }
        public string PolicyDesc { get; set; }
        public string PolicyDisplayDesc { get; set; }
        public string AddToType { get; set; }
        public bool IsClosed { get; set; }
        public DateTime Effective { get; set; }
        public DateTime Expiration { get; set; }
        public string ShowExpirationDate { get; set; }
        public string ShowEffectiveDate { get; set; }
        public Int32 UniqAssociatedItem { get; set; }
        public string AssociationType { get; set; }
        public Int32 UniqAgency { get; set; }
        public Int32 UniqBranch { get; set; }
        public string Status { get; set; }
        public string OwnerCode { get; set; }
        public string OwnerDescription { get; set; }
        public DateTime InsertedDate { get; set; }
        public string ActivityGuid { get; set; }
        public DateTime ClosedDate { get; set; }
        public Nullable<int> UniqPolicy { get; set; }
        public Nullable<int> UniqLine { get; set; }
        public Nullable<int> UniqClaim { get; set; }
        public Nullable<System.DateTime> LossDate { get; set; }
        public string Policydescription { get; set; }
        public string LineCode { get; set; }
        public string LineDescription { get; set; }
        public string ICO { get; set; }
        public DateTime LineEffectiveDate { get; set; }
        public DateTime LineExpirationDate { get; set; }
    }
    public class PolicyTypeInfo
    {
        public string PolicyTypeCode { get; set; }
        public string PolicyTypeDescription { get; set; }

    }
    public class PolicyYear
    {
        public bool IsSelected { get; set; }
        public string Description { get; set; }
    }
    public class FolderInfo
    {
        public string FolderType { get; set; }
        public long FolderId { get; set; }
        public string FolderName { get; set; }
        public long ParentFolderId { get; set; }
        public string ParentFolderName { get; set; }
    }
    public class MailItemInfo
    {

        public string Identifier { get; set; }
        public OutlookNS.MailItem MailItem { get; set; }
        public string Description { get; set; }
    }
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the PropertyChange event for the property specified
        /// </summary>
        /// <param name="propertyName">Property name to update. Is case-sensitive.</param>
        public virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members
    }
    public class FailedNotificationViewModel : ObservableObject
    {
        private Int32 _failedNotificationCount;
        public Int32 FailedNotificationCount
        {
            get
            {
                return _failedNotificationCount;
            }
            set
            {
                _failedNotificationCount = value;
                RaisePropertyChanged("FailedNotificationCount");
            }
        }
    }
    public class OfficeWin32Window : IWin32Window, IDisposable
    {

        ///<summary>
        /// The <b>FindWindow</b> method finds a window by it's classname and caption.
        ///</summary>
        ///<param name="lpClassName">The classname of the window (use Spy++)</param>
        ///<param name="lpWindowName">The Caption of the window.</param>
        ///<returns>Returns a valid window handle or 0.</returns>
        [DllImport("user32")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region IWin32Window Members

        ///<summary>
        /// This holds the window handle for the found Window.
        ///</summary>
        IntPtr _windowHandle = IntPtr.Zero;

        ///<summary>
        /// The <b>Handle</b> of the Outlook WindowObject.
        ///</summary>
        public IntPtr Handle
        {
            get { return _windowHandle; }
        }
        private string _windowTitle = string.Empty;
        public string WindowTitle
        {
            get
            {
                return _windowTitle;
            }
        }

        #endregion

        ///<summary>
        /// The <b>OfficeWin32Window</b> class could be used to get the parent IWin32Window for Windows.Forms and MessageBoxes.
        ///</summary>
        ///<param name="windowObject">The current WindowObject.</param>
        public OfficeWin32Window(object windowObject)
        {
            string caption = windowObject.GetType().InvokeMember("Caption", System.Reflection.BindingFlags.GetProperty, null, windowObject, null).ToString();

            // try to get the HWND ptr from the windowObject / could be an Inspector window or an explorer window
            _windowHandle = FindWindow("rctrl_renwnd32\0", caption);

            StringBuilder s = new StringBuilder(50);
            int count = 50;
            GetWindowText(Handle, s, count);
            _windowTitle = s.ToString();
        }
    }

}


