using HIB.Outlook.Model.Activities;
using HIB.Outlook.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using HIB.Outlook.Sync.Common;
using HIB.Outlook.Helper.Common;
using AttachmentBridge;

namespace OutlookAddIn1.UserControls
{
    /// <summary>
    /// Interaction logic for AddActivityFirstPage.xaml
    /// </summary>
    public partial class AddActivityFirstPage : UserControl
    {
        private bool _includeHistoryForLine = false;
        private bool _includeClosedForOpportunites = false;
        private bool _includeHistoryForPolicy = false;
        private bool _includeClosedForServices = false;
        private bool _includeHistoryForMasterMarketing = false;
        string EmployeeLookupCode = string.Empty;

        AddActivityMainPage _addActivityMainPage = null;
        public AddActivityFirstPage()
        {
            InitializeComponent();
            EmployeeLookupCode = CommonHelper.GetLookUpCode();
        }

        private void AddToActivityTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (AddToActivityTypeComboBox != null)
                {
                    var selectedItem = AddToActivityTypeComboBox.SelectedItem as string;
                    if (selectedItem != null)
                    {
                        SetRetainedIncludeHistoryValuesToAllAccountTypes(selectedItem);
                        if (!string.IsNullOrEmpty(selectedItem))
                        {
                            if (_addActivityMainPage != null && _addActivityMainPage._attachmentControl != null)
                                _addActivityMainPage._attachmentControl.GetActivity(selectedItem);
                        }
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

        private void includeHistoryorClosed_Unchecked(object sender, RoutedEventArgs e)
        {
            RetainIncludeHistoryForAllAccountTypes(false);
            if (_addActivityMainPage != null)
            {
                _addActivityMainPage._attachmentControl.ExcludeHistoryForOldValues();
            }

        }

        private void includeHistoryorClosed_Checked(object sender, RoutedEventArgs e)
        {
            RetainIncludeHistoryForAllAccountTypes(true);
            if (_addActivityMainPage != null)
            {
                _addActivityMainPage._attachmentControl.IncludeHistoryForOldValues();
            }

        }
        ///<summary>
        /// setting the value already chosen for include history option in add activity
        /// </summary>
        /// <returns>void</returns>
        private void SetRetainedIncludeHistoryValuesToAllAccountTypes(string selectedItem)
        {
            try
            {
                switch (selectedItem)
                {
                    case "Policy":
                        {
                            includeHistoryorClosed.IsChecked = _includeHistoryForPolicy;
                            break;
                        }
                    case "Line":
                        {
                            includeHistoryorClosed.IsChecked = _includeHistoryForLine;
                            break;
                        }
                    case "Opportunities":
                        {
                            includeHistoryorClosed.IsChecked = _includeClosedForOpportunites;
                            break;
                        }
                    case "Services":
                        {
                            includeHistoryorClosed.IsChecked = _includeClosedForServices;
                            break;
                        }
                    case "Master Marketing Submission":
                        {
                            includeHistoryorClosed.IsChecked = _includeHistoryForMasterMarketing;
                            break;
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

        ///<summary>
        /// saving the include history option value to be retained for all Add to Type in add activity
        /// </summary>
        /// <returns>void</returns>
        private void RetainIncludeHistoryForAllAccountTypes(bool IsChecked)
        {
            try
            {
                var addtoActivityComboboxValue = AddToActivityTypeComboBox.SelectedItem as string;
                switch (addtoActivityComboboxValue)
                {
                    case "Policy":
                        {
                            _includeHistoryForPolicy = IsChecked;
                            break;
                        }
                    case "Line":
                        {
                            _includeHistoryForLine = IsChecked;
                            break;
                        }
                    case "Opportunities":
                        {
                            _includeClosedForOpportunites = IsChecked;
                            break;
                        }
                    case "Services":
                        {
                            _includeClosedForServices = IsChecked;
                            break;
                        }
                    case "Master Marketing Submission":
                        {
                            _includeHistoryForMasterMarketing = IsChecked;
                            break;
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

        private void dgAddActivity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnAddActivityContinue.IsEnabled = false;
                var selectedItem = dgAddActivity.SelectedItem;
                if (selectedItem != null)
                {
                    btnAddActivityContinue.IsEnabled = true;
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



        private void btnAddActivityContinue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_addActivityMainPage != null)
                {
                    if (_addActivityMainPage.addactivitySecondPage.startdateDateTimePicker.SelectedDate == null || _addActivityMainPage.addactivitySecondPage.startdateDateTimePicker.SelectedDate == DateTime.MinValue)
                        _addActivityMainPage.addactivitySecondPage.startdateDateTimePicker.SelectedDate = DateTime.Now;
                    _addActivityMainPage.addactivitySecondPage.addToType.Content = _addActivityMainPage.addactivityFirstPage.AddToActivityTypeComboBox.Text + ":";
                    if (_addActivityMainPage.addactivityMainPageGrid.RowDefinitions.Count >= 2)
                        _addActivityMainPage.addactivityMainPageGrid.RowDefinitions[1].Height = new GridLength(40);
                    GetSelectedValueFromMainGrid();
                    _addActivityMainPage.addactivityFirstPage.Visibility = Visibility.Collapsed;
                    _addActivityMainPage.addactivitySecondPage.Visibility = Visibility.Visible;
                    _addActivityMainPage.addactivitySecondPage.scrAddActivity.ScrollToHome();
                    _addActivityMainPage.addactivitySecondPage.AddActivity2ClientCodeLabel.Content = AddActivityClientCodeLabel.Content;
                    _addActivityMainPage.addactivitySecondPage.AddActivity2ClientDescLabel.Content = AddActivityClientDescLabel.Content;
                    ClientInfo selectedClient = null;
                    if (_addActivityMainPage._attachmentControl.Tab.SelectedIndex == 0)
                    {
                        selectedClient = _addActivityMainPage._attachmentControl.LstActiveClient.SelectedItem as ClientInfo;
                    }
                    else if (_addActivityMainPage._attachmentControl.Tab.SelectedIndex == 1)
                    {
                        selectedClient = _addActivityMainPage._attachmentControl.LstInActiveClient.SelectedItem as ClientInfo;
                    }

                    if (Globals.ThisAddIn.ActivityClientContactInfoCollection.Count > 0)
                    {
                        var contactList = DistinctHelper.DistinctBy(Globals.ThisAddIn.ActivityClientContactInfoCollection.Where(m => m.EntityId == Convert.ToInt64(selectedClient.UniqEntity)), m => m.ContactName);
                        _addActivityMainPage.addactivitySecondPage.contactNameComboBox.ItemsSource = contactList;
                        _addActivityMainPage.addactivitySecondPage.contactNameComboBox.SelectedItem = contactList.FirstOrDefault();
                        if (contactList == null || contactList?.Count() == 0)
                        {
                            List<string> tempContactModes = new List<string>();
                            _addActivityMainPage.addactivitySecondPage.contactModeComboBox.ItemsSource = null;
                            _addActivityMainPage.addactivitySecondPage.contactModeComboBox.ItemsSource = tempContactModes;
                            _addActivityMainPage.addactivitySecondPage.contactValueTextblock.Text = string.Empty;

                        }


                    }

                }

                if (_addActivityMainPage != null && _addActivityMainPage.addactivitySecondPage.AddActivityCode.SelectedItem != null)
                {
                    var selectedItem = _addActivityMainPage.addactivitySecondPage.AddActivityCode.SelectedItem as AddActivityCode;
                    if (selectedItem != null)
                    {
                        _addActivityMainPage.addactivitySecondPage.DescriptionTextBox.Text = _addActivityMainPage.addactivitySecondPage.BuildingUpDescription(selectedItem.Description);
                        _addActivityMainPage.addactivitySecondPage.descriptionTextBlock.Text = selectedItem.Description;
                    }

                    //if (_addActivityMainPage._attachmentControl != null && _addActivityMainPage._attachmentControl.ActivityLookupDetailsCollection.Count > 0)
                    //{
                    //    var activitylookup = _addActivityMainPage._attachmentControl.ActivityLookupDetailsCollection[0];
                    //    if (activitylookup != null)
                    //        _addActivityMainPage.addactivitySecondPage.DescriptionTextBox.Text = _addActivityMainPage._attachmentControl.BuildingUpDescriptionFromActivityLookupDetails(_addActivityMainPage.addactivitySecondPage.DescriptionTextBox.Text, activitylookup);
                    //}
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

        private void btnAddActivityCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_addActivityMainPage != null && _addActivityMainPage._addactivityWindow != null)
            {
                _addActivityMainPage._addactivityWindow.Hide();
            }

        }

        private void btnAddActivityClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _addActivityMainPage = this.Tag as AddActivityMainPage;
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

        #region Methods

        private void GetSelectedValueFromMainGrid()
        {
            try
            {
                var selectedItem = dgAddActivity.SelectedItem;
                var addtoActivityComboboxValue = AddToActivityTypeComboBox.SelectedItem as string;
                GetActivityForSelectedActivity(addtoActivityComboboxValue, selectedItem);
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

        ///<summary>
        /// displaying the Add to type detail value (for e.g Policy) in data grid which is selected in add activity first page.
        /// </summary>
        /// <returns>void</returns>
        private void GetActivityForSelectedActivity(string selectedActivity, object activitySelectedItem)
        {
            try
            {
                _addActivityMainPage.addactivitySecondPage.SelectedActivityValueGrid.ItemsSource = null;
                switch (selectedActivity)
                {
                    case "Account":
                        {
                            WriteColumnsToSelectedActivityGridFromClass<Account>();
                            List<Account> tempAccountCollection = new List<Account>();
                            var selectedAccount = activitySelectedItem as Account;
                            if (selectedAccount != null)
                            {
                                tempAccountCollection.Add(selectedAccount);
                            }

                            _addActivityMainPage.addactivitySecondPage.SelectedActivityValueGrid.ItemsSource = tempAccountCollection;

                            //var query = string.Format("SELECT * FROM HIBOPActivityLookupDetails where UniqEntity={0}", selectedAccount.ClientId);
                            //_addActivityMainPage._attachmentControl.GetAllActivityLookupDetailsFromDatabase(query);

                            break;
                        }
                    case "Policy":
                        {
                            WriteColumnsToSelectedActivityGridFromClass<Policy>();
                            List<Policy> tempPolicyCollection = new List<Policy>();
                            var selectedPolicy = activitySelectedItem as Policy;
                            if (selectedPolicy != null)
                            {
                                tempPolicyCollection.Add(selectedPolicy);
                            }
                            _addActivityMainPage.addactivitySecondPage.SelectedActivityValueGrid.ItemsSource = tempPolicyCollection;

                            //This is done for Activity Lookup Details 
                            //var query = string.Format("SELECT * FROM HIBOPActivityLookupDetails where UniqPolicy={0}", selectedPolicy.PolicyId);
                            //_addActivityMainPage._attachmentControl.GetAllActivityLookupDetailsFromDatabase(query);

                            break;
                        }
                    case "Claim":
                        {
                            WriteColumnsToSelectedActivityGridFromClass<Claim>();
                            List<Claim> tempClaimsCollection = new List<Claim>();
                            var selectedClaim = activitySelectedItem as Claim;
                            if (selectedClaim != null)
                            {
                                tempClaimsCollection.Add(selectedClaim);
                            }
                            _addActivityMainPage.addactivitySecondPage.SelectedActivityValueGrid.ItemsSource = tempClaimsCollection;

                            //This is done for Activity Lookup Details 
                            //var query = string.Format("SELECT * FROM HIBOPActivityLookupDetails where UniqClaim={0}", selectedClaim.ClaimId);
                            //_addActivityMainPage._attachmentControl.GetAllActivityLookupDetailsFromDatabase(query);

                            break;
                        }
                    case "Line":
                        {
                            WriteColumnsToSelectedActivityGridFromClass<HIB.Outlook.Model.Activities.Line>();
                            List<HIB.Outlook.Model.Activities.Line> tempLinesOfBusinessCollection = new List<HIB.Outlook.Model.Activities.Line>();
                            HIB.Outlook.Model.Activities.Line selectedLine = activitySelectedItem as HIB.Outlook.Model.Activities.Line;
                            if (selectedLine != null)
                            {
                                tempLinesOfBusinessCollection.Add(selectedLine);
                            }
                            _addActivityMainPage.addactivitySecondPage.SelectedActivityValueGrid.ItemsSource = tempLinesOfBusinessCollection;


                            //This is done for Activity Lookup Details 
                            //var query = string.Format("SELECT * FROM HIBOPActivityLookupDetails where UniqLine={0}", selectedLine.LineId);
                            //_addActivityMainPage._attachmentControl.GetAllActivityLookupDetailsFromDatabase(query);

                            break;
                        }
                    case "Opportunities":
                        {
                            WriteColumnsToSelectedActivityGridFromClass<Opportunities>();
                            List<Opportunities> tempOpportunitiesCollection = new List<Opportunities>();
                            var selectedOpportunity = activitySelectedItem as Opportunities;
                            if (selectedOpportunity != null)
                            {
                                tempOpportunitiesCollection.Add(selectedOpportunity);
                            }
                            _addActivityMainPage.addactivitySecondPage.SelectedActivityValueGrid.ItemsSource = tempOpportunitiesCollection;
                            break;
                        }
                    case "Services":
                        {
                            WriteColumnsToSelectedActivityGridFromClass<Services>();
                            List<Services> tempServicesCollection = new List<Services>();
                            var selectedService = activitySelectedItem as Services;
                            if (selectedService != null)
                            {
                                tempServicesCollection.Add(selectedService);
                            }
                            _addActivityMainPage.addactivitySecondPage.SelectedActivityValueGrid.ItemsSource = tempServicesCollection;
                            break;
                        }
                    case "Master Marketing Submission":
                        {
                            WriteColumnsToSelectedActivityGridFromClass<MasterMarketingSubmission>();
                            List<MasterMarketingSubmission> tempMasterMarketingSubmissionCollection = new List<MasterMarketingSubmission>();
                            var selectedMasterMarketingSubmission = activitySelectedItem as MasterMarketingSubmission;
                            if (selectedMasterMarketingSubmission != null)
                            {
                                tempMasterMarketingSubmissionCollection.Add(selectedMasterMarketingSubmission);
                            }
                            _addActivityMainPage.addactivitySecondPage.SelectedActivityValueGrid.ItemsSource = tempMasterMarketingSubmissionCollection;
                            break;
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

        ///<summary>
        /// writing all the columns from selected Add To Type in Data Grid with generic type for add activity second page
        /// </summary>
        /// <returns>void</returns>
        private void WriteColumnsToSelectedActivityGridFromClass<T>()
        {
            try
            {
                if (_addActivityMainPage != null)
                {
                    _addActivityMainPage.addactivitySecondPage.SelectedActivityValueGrid.Columns.Clear();
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
                                _addActivityMainPage.addactivitySecondPage.SelectedActivityValueGrid.Columns.Add(column);
                            }
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
            finally
            {
                Logger.save();
            }
        }
        #endregion
    }
}
