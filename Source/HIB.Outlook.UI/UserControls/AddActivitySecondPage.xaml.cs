using AttachmentBridge;
using HIB.Outlook.Helper.Common;
using HIB.Outlook.Model.Activities;
using HIB.Outlook.Sync.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using HIB.Outlook.UI;

namespace OutlookAddIn1.UserControls
{
    /// <summary>
    /// Interaction logic for AddActivitySecondPage.xaml
    /// </summary>
    public partial class AddActivitySecondPage : UserControl
    {

        AddActivityMainPage _addActivityMainPage = null;
        string EmployeeLookupCode = string.Empty;
        public AddActivitySecondPage()
        {
            InitializeComponent();
            EmployeeLookupCode = CommonHelper.GetLookUpCode();
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
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }

        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddActivityCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedItem = AddActivityCode.SelectedItem as AddActivityCode;
                if (selectedItem != null)
                {
                    DescriptionTextBox.Text = BuildingUpDescription(selectedItem.Description);
                    descriptionTextBlock.Text = selectedItem.Description;
                }

                //if (_addActivityMainPage != null && _addActivityMainPage._attachmentControl != null && _addActivityMainPage._attachmentControl.ActivityLookupDetailsCollection.Count > 0)
                //{
                //    var activitylookup = _addActivityMainPage._attachmentControl.ActivityLookupDetailsCollection[0];
                //    if (activitylookup != null)
                //        DescriptionTextBox.Text = _addActivityMainPage._attachmentControl.BuildingUpDescriptionFromActivityLookupDetails(DescriptionTextBox.Text, activitylookup);
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

        ///<summary>
        /// changing the add activity Code Description as per business rules with selected Add to Type values
        /// </summary>
        /// <returns>string</returns>
        internal string BuildingUpDescription(string OriginalDescription)
        {
            try
            {
                OriginalDescription = OriginalDescription.Replace("&AcctName&", " " + AddActivity2ClientDescLabel.Content + " ");
                OriginalDescription = OriginalDescription.Replace("&AccName&", " " + AddActivity2ClientDescLabel.Content + " ");
                OriginalDescription = OriginalDescription.Replace("&AttachDesc&", " ");
                if (SelectedActivityValueGrid.Items.Count > 0)
                {
                    if (SelectedActivityValueGrid.Items[0] is HIB.Outlook.Model.Activities.Line)
                    {
                        var selectedLine = SelectedActivityValueGrid.Items[0] as HIB.Outlook.Model.Activities.Line;
                        OriginalDescription = OriginalDescription.Replace("&LineCode&", " " + selectedLine.LineCode + " ");
                        OriginalDescription = OriginalDescription.Replace("&LineEffDate&", " " + Convert.ToDateTime(selectedLine.Effective).ToShortDateString() + " ");
                        OriginalDescription = OriginalDescription.Replace("&LineExpDate&", " " + Convert.ToDateTime(selectedLine.Expiration).ToShortDateString() + " ");
                        OriginalDescription = OriginalDescription.Replace("&LineDesc&", " " + selectedLine.LineOfBusiness + " ");
                        OriginalDescription = OriginalDescription.Replace("&ICOName&", " " + selectedLine.ICO + " ");
                        if (OriginalDescription.Contains("&PolicyType&") || OriginalDescription.Contains("&PolType&") || OriginalDescription.Contains("&Policy&") || OriginalDescription.Contains("&PolicyExpDate&") || OriginalDescription.Contains("&PolExpDate&") || OriginalDescription.Contains("&PolicyDesc&") || OriginalDescription.Contains("&PolDesc&") || OriginalDescription.Contains("&PolicyEffDate&") || OriginalDescription.Contains("&PolEffDate&"))
                        {
                            //using (var context = new HIB.Outlook.SQLite.HIBOutlookEntities())
                            //{
                            //    var linkedPolicy = context.HIBOPPolicies.Where(m => m.UniqPolicy == selectedLine.UniqPolicy).FirstOrDefault();
                            //    if (linkedPolicy != null)
                            //    {
                            //        OriginalDescription = OriginalDescription.Replace("&PolicyType&", " " + linkedPolicy.CdPolicyLineTypeCode + " ");
                            //        OriginalDescription = OriginalDescription.Replace("&PolType&", " " + linkedPolicy.CdPolicyLineTypeCode + " ");
                            //        //OriginalDescription = OriginalDescription.Replace("&Policy&", " " + linkedPolicy.PolicyNumber + " ");
                            //        OriginalDescription = OriginalDescription.Replace("&Policy#&", " " + linkedPolicy.PolicyNumber + " ");
                            //        OriginalDescription = OriginalDescription.Replace("&PolicyExpDate&", " " + linkedPolicy.ExpirationDate?.ToShortDateString() + " ");
                            //        OriginalDescription = OriginalDescription.Replace("&PolicyEffDate&", " " + linkedPolicy.EffectiveDate?.ToShortDateString() + " ");
                            //        OriginalDescription = OriginalDescription.Replace("&PolEffDate&", " " + linkedPolicy.EffectiveDate?.ToShortDateString() + " ");
                            //        OriginalDescription = OriginalDescription.Replace("&PolExpDate&", " " + linkedPolicy.ExpirationDate?.ToShortDateString() + " ");
                            //        OriginalDescription = OriginalDescription.Replace("&PolicyDesc&", " " + linkedPolicy.DescriptionOf + " ");
                            //        OriginalDescription = OriginalDescription.Replace("&PolDesc&", " " + linkedPolicy.DescriptionOf + " ");
                            //    }
                            //}
                            var policyInfo = new Policy();
                            string policyActivitiesQuery = string.Format("Select * From HIBOPPolicy where UniqPolicy={0} limit 1", selectedLine.UniqPolicy);
                            var sqliteDataReader = HIB.Outlook.SQLite.SQLiteHandler.ExecuteSelectQuery(policyActivitiesQuery);

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
                                        policyInfo.Effective = Convert.ToDateTime(sqliteDataReader["EffectiveDate"]).ToShortDateString();// sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("EffectiveDate"));
                                        policyInfo.Expiration = Convert.ToDateTime(sqliteDataReader["ExpirationDate"]).ToShortDateString();// sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("ExpirationDate"));
                                        policyInfo.PolicyNumber = Convert.ToString(sqliteDataReader["PolicyNumber"]);
                                        policyInfo.PolicyDescription = Convert.ToString(sqliteDataReader["DescriptionOf"]);
                                        policyInfo.Flags = Convert.ToInt32(sqliteDataReader["Flags"]);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                    }

                                }
                                sqliteDataReader.Close();
                            }
                            if (policyInfo.PolicyId != 0)
                            {
                                OriginalDescription = OriginalDescription.Replace("&PolicyType&", " " + policyInfo.Type + " ");
                                OriginalDescription = OriginalDescription.Replace("&PolType&", " " + policyInfo.Type + " ");
                                //OriginalDescription = OriginalDescription.Replace("&Policy&", " " + linkedPolicy.PolicyNumber + " ");
                                OriginalDescription = OriginalDescription.Replace("&Policy#&", " " + policyInfo.PolicyNumber + " ");
                                OriginalDescription = OriginalDescription.Replace("&PolicyExpDate&", " " + policyInfo.Expiration + " ");
                                OriginalDescription = OriginalDescription.Replace("&PolExpDate&", " " + policyInfo.Expiration + " ");
                                OriginalDescription = OriginalDescription.Replace("&PolicyEffDate&", " " + policyInfo.Effective + " ");
                                OriginalDescription = OriginalDescription.Replace("&PolEffDate&", " " + policyInfo.Effective + " ");
                                OriginalDescription = OriginalDescription.Replace("&PolicyDesc&", " " + policyInfo.PolicyDescription + " ");
                                OriginalDescription = OriginalDescription.Replace("&PolDesc&", " " + policyInfo.PolicyDescription + " ");
                            }

                        }
                        OriginalDescription = OriginalDescription.Replace("&DateLoss&", " ");

                    }
                    else if (SelectedActivityValueGrid.Items[0] is HIB.Outlook.Model.Activities.Policy)
                    {
                        var selectedPolicy = SelectedActivityValueGrid.Items[0] as HIB.Outlook.Model.Activities.Policy;
                        OriginalDescription = OriginalDescription.Replace("&PolicyType&", " " + selectedPolicy.Type + " ");
                        OriginalDescription = OriginalDescription.Replace("&PolType&", " " + selectedPolicy.Type + " ");
                        //OriginalDescription = OriginalDescription.Replace("&Policy&", " " + selectedPolicy.PolicyNumber + " ");
                        OriginalDescription = OriginalDescription.Replace("&Policy#&", " " + selectedPolicy.PolicyNumber + " ");
                        OriginalDescription = OriginalDescription.Replace("&PolicyExpDate&", " " + Convert.ToDateTime(selectedPolicy.Expiration).ToShortDateString() + " ");
                        OriginalDescription = OriginalDescription.Replace("&PolExpDate&", " " + Convert.ToDateTime(selectedPolicy.Expiration).ToShortDateString() + " ");
                        OriginalDescription = OriginalDescription.Replace("&PolicyDesc&", " " + selectedPolicy.PolicyDescription + " ");
                        OriginalDescription = OriginalDescription.Replace("&PolDesc&", " " + selectedPolicy.PolicyDescription + " ");
                        OriginalDescription = OriginalDescription.Replace("&PolicyEffDate&", " " + Convert.ToDateTime(selectedPolicy.Effective).ToShortDateString() + " ");
                        OriginalDescription = OriginalDescription.Replace("&PolEffDate&", " " + Convert.ToDateTime(selectedPolicy.Effective).ToShortDateString() + " ");
                        if (OriginalDescription.Contains("&LineCode&") || OriginalDescription.Contains("&LineEffDate&") || OriginalDescription.Contains("&LineExpDate&") || OriginalDescription.Contains("&LineDesc&") || OriginalDescription.Contains("&ICOName&"))
                        {
                            var LineInfo = new Line();
                            string linesActivitiesQuery = string.Format("Select * From HIBOPActivityLine where UniqPolicy={0} limit 1", selectedPolicy.PolicyId);
                            var sqliteLineDataReader = HIB.Outlook.SQLite.SQLiteHandler.ExecuteSelectQuery(linesActivitiesQuery);

                            if (sqliteLineDataReader != null)
                            {
                                while (sqliteLineDataReader.Read())
                                {
                                    try
                                    {
                                        LineInfo.LineId = Convert.ToInt64(sqliteLineDataReader["UniqLine"]);
                                        LineInfo.ClientId = Convert.ToInt64(sqliteLineDataReader["UniqEntity"]);
                                        LineInfo.LineCode = Convert.ToString(sqliteLineDataReader["LineCode"]);
                                        LineInfo.UniqPolicy = Convert.ToInt64(sqliteLineDataReader["UniqPolicy"]);
                                        LineInfo.LineOfBusiness = Convert.ToString(sqliteLineDataReader["LineOfBusiness"]);
                                        LineInfo.Status = Convert.ToString(sqliteLineDataReader["LineStatus"]);
                                        LineInfo.PolicyNumber = Convert.ToString(sqliteLineDataReader["PolicyNumber"]);
                                        LineInfo.ICO = Convert.ToString(sqliteLineDataReader["IOC"]);
                                        LineInfo.Billing = Convert.ToString(sqliteLineDataReader["BillModeCode"]);
                                        LineInfo.PolicyDescription = Convert.ToString(sqliteLineDataReader["PolicyDesc"]);
                                        LineInfo.Flags = Convert.ToInt32(sqliteLineDataReader["Flags"]);
                                        LineInfo.Effective = Convert.ToDateTime(sqliteLineDataReader["EffectiveDate"]).ToShortDateString();
                                        LineInfo.Expiration = Convert.ToDateTime(sqliteLineDataReader["ExpirationDate"]).ToShortDateString();

                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                    }

                                }
                                sqliteLineDataReader.Close();
                            }
                            if (LineInfo.LineId != 0)
                            {
                                OriginalDescription = OriginalDescription.Replace("&LineCode&", " " + LineInfo.LineCode + " ");
                                OriginalDescription = OriginalDescription.Replace("&LineEffDate&", " " + LineInfo.Effective + " ");
                                OriginalDescription = OriginalDescription.Replace("&LineExpDate&", " " + LineInfo.Expiration + " ");
                                OriginalDescription = OriginalDescription.Replace("&LineDesc&", " " + LineInfo.LineOfBusiness + " ");
                                OriginalDescription = OriginalDescription.Replace("&ICOName&", " " + LineInfo.ICO + " ");
                            }

                        }
                        OriginalDescription = OriginalDescription.Replace("&DateLoss&", " ");
                    }
                    else if (SelectedActivityValueGrid.Items[0] is HIB.Outlook.Model.Activities.Claim)
                    {
                        var selectedClaim = SelectedActivityValueGrid.Items[0] as HIB.Outlook.Model.Activities.Claim;
                        OriginalDescription = OriginalDescription.Replace("&DateLoss&", " " + selectedClaim.DateOfLoss + " ");
                        //OriginalDescription = OriginalDescription.Replace("&DOL&", " " + selectedClaim.DateOfLoss + " ");
                    }
                    else if (SelectedActivityValueGrid.Items[0] is HIB.Outlook.Model.Activities.Account)
                    {
                        var selectedAccount = SelectedActivityValueGrid.Items[0] as HIB.Outlook.Model.Activities.Account;
                        OriginalDescription = OriginalDescription.Replace("&LineCode&", " ");
                        OriginalDescription = OriginalDescription.Replace("&LineEffDate&", " ");
                        OriginalDescription = OriginalDescription.Replace("&LineExpDate&", " ");
                        OriginalDescription = OriginalDescription.Replace("&LineDesc&", " ");
                        OriginalDescription = OriginalDescription.Replace("&ICOName&", " ");
                        OriginalDescription = OriginalDescription.Replace("&PolicyType&", " ");
                        //OriginalDescription = OriginalDescription.Replace("&Policy&", " " + selectedPolicy.PolicyNumber + " ");
                        OriginalDescription = OriginalDescription.Replace("&Policy#&", " ");
                        OriginalDescription = OriginalDescription.Replace("&PolicyExpDate&", " ");
                        OriginalDescription = OriginalDescription.Replace("&PolExpDate&", " ");
                        OriginalDescription = OriginalDescription.Replace("&PolicyDesc&", " ");
                        OriginalDescription = OriginalDescription.Replace("&PolDesc&", " ");
                        OriginalDescription = OriginalDescription.Replace("&PolicyEffDate&", " ");
                        OriginalDescription = OriginalDescription.Replace("&DateLoss&", " ");
                        OriginalDescription = OriginalDescription.Replace("&PolType&", " ");
                        OriginalDescription = OriginalDescription.Replace("&PolEffDate&", " ");
                        // buildupDescription = OriginalDescription.Replace("&AttachDesc&", " " + selectedAccount. + " ");                  
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
            return OriginalDescription;
        }

        ///<summary>
        /// changing the add activity Code Description as per business rules with lookup values. 
        /// </summary>
        /// <returns>string</returns>
        internal string BuildingUpDescriptionFromActivityLookupDetails(string OriginalDescription, ActivityLookupDetails activityLookupDetail)
        {
            try
            {

                OriginalDescription = OriginalDescription.Replace("&AccName&", " " + AddActivity2ClientDescLabel.Content + " ");


                OriginalDescription = OriginalDescription.Replace("&LineCode&", " " + activityLookupDetail.LineCode + " ");
                OriginalDescription = OriginalDescription.Replace("&LineEffDate&", " " + activityLookupDetail.LineEffDate + " ");
                OriginalDescription = OriginalDescription.Replace("&LineExpDate&", " " + activityLookupDetail.LineExpDate + " ");
                OriginalDescription = OriginalDescription.Replace("&LineDesc&", " " + activityLookupDetail.LineDesc + " ");
                OriginalDescription = OriginalDescription.Replace("&ICOName&", " " + activityLookupDetail.IOC + " ");

                OriginalDescription = OriginalDescription.Replace("&PolicyType&", " " + activityLookupDetail.PolicyType + " ");
                OriginalDescription = OriginalDescription.Replace("&PolType&", " " + activityLookupDetail.PolicyType + " ");
                //OriginalDescription = OriginalDescription.Replace("&Policy&", " " + selectedPolicy.PolicyNumber + " ");
                OriginalDescription = OriginalDescription.Replace("&Policy#&", " " + activityLookupDetail.PolicyNumber + " ");
                OriginalDescription = OriginalDescription.Replace("&PolicyExpDate&", " " + activityLookupDetail.PolicyExpDate + " ");
                OriginalDescription = OriginalDescription.Replace("&PolExpDate&", " " + activityLookupDetail.PolicyExpDate + " ");
                OriginalDescription = OriginalDescription.Replace("&PolicyDesc&", " " + activityLookupDetail.PolicyDesc + " ");
                OriginalDescription = OriginalDescription.Replace("&PolDesc&", " " + activityLookupDetail.PolicyDesc + " ");
                OriginalDescription = OriginalDescription.Replace("&PolicyEffDate&", " " + activityLookupDetail.PolicyEffDate + " ");

                OriginalDescription = OriginalDescription.Replace("&DateLoss&", " " + activityLookupDetail.DateOfLoss + " ");


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return OriginalDescription;
        }

        ///<summary>
        /// validating the time picker control for displaying the number only.
        /// </summary>
        /// <returns>void</returns>
        private void ValidateTimePickerControl(TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = !e.Text.Any(x => Char.IsDigit(x) || ':'.Equals(x) || 'A'.ToString().ToUpper().Equals(x.ToString().ToUpper()) || 'P'.ToString().ToUpper().Equals(x.ToString().ToUpper()) || 'M'.ToString().ToUpper().Equals(x.ToString().ToUpper()));
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
        private void startTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                ValidateTimePickerControl(e);
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

        private void contactNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedItem = contactNameComboBox.SelectedItem as HIB.Outlook.Model.ActivityClientContactInfo;
                if (selectedItem != null)
                {
                    var getallContactModes = Globals.ThisAddIn.ActivityClientContactInfoCollection.Where(m => m.ContactNameId == selectedItem.ContactNameId).ToList();
                    if (getallContactModes.Count > 0)
                    {
                        contactModeComboBox.ItemsSource = getallContactModes;
                        contactModeComboBox.SelectedItem = getallContactModes.Where(x => x.ContactType == selectedItem.ContactType).FirstOrDefault();
                        //  contactValueTextblock.Content = string.Empty;
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

        private void contactModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedItem = contactModeComboBox.SelectedItem as HIB.Outlook.Model.ActivityClientContactInfo;
                if (selectedItem != null)
                {
                    contactValueTextblock.Text = selectedItem.ContactValue;
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

        internal void ResetAllDataForAddActivity()
        {
            try
            {
                AddActivityCode.SelectedItem = null;
                ownerComboBox.SelectedItem = null;
                PriorityComboBox.SelectedItem = null;
                updateComboBox.SelectedItem = null;
                startdateDateTimePicker.SelectedDate = null;
                EndDatePicker.SelectedDate = null;
                reminderDatePicker.SelectedDate = null;
                reminderTextBox.CurrentTime = DateTime.MinValue;
                startTime.CurrentTime = DateTime.MinValue;
                endTime.CurrentTime = DateTime.MinValue;
                contactNameComboBox.SelectedItem = null;
                contactModeComboBox.SelectedItem = null;
                accessLevelComboBox.SelectedItem = null;
                descriptionTextBlock.Text = string.Empty;
                contactValueTextblock.Text = string.Empty;
                DescriptionTextBox.Text = string.Empty;

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

        //private void btnAddNewActivityFinish_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        addActivityRequiredFieldAlert.Visibility = Visibility.Collapsed;
        //        if (ValidateMandatoryFieldsInAddActivity())
        //        {
        //            // AddActivityPopup.IsOpen = false;
        //            SaveNewActivityInfoToLocalDatabase();
        //            ResetAllDataForAddActivity();
        //            _addActivityMainPage._addactivityWindow.Hide();
        //        }
        //        else
        //        {
        //            addActivityRequiredFieldAlert.Visibility = Visibility.Visible;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
        //    }
        //    finally
        //    {
        //        Logger.save();
        //    }
        //}

        private void btnAddNewActivityCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _addActivityMainPage.addactivityFirstPage.Visibility = Visibility.Visible;
                _addActivityMainPage.addactivitySecondPage.Visibility = Visibility.Collapsed;
                ClearErrorsforAddActivity();
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                scrAddActivity.ScrollToHome();
                _addActivityMainPage = this.Tag as AddActivityMainPage;
                addToType.Content = _addActivityMainPage?.addactivityFirstPage.AddToActivityTypeComboBox.Text + ":";

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

        internal void ClearErrorsforAddActivity()
        {
            try
            {
                addActivityRequiredFieldAlert.Visibility = Visibility.Collapsed;
                AddActivityCode.Margin = new Thickness(0);
                brdmainAddActivityCode.Visibility = Visibility.Collapsed;

                brdmainownerComboBox.Visibility = Visibility.Collapsed;
                ownerComboBox.Margin = new Thickness(0, 0, 7, 0);

                brdmainupdateComboBox.Visibility = Visibility.Collapsed;
                updateComboBox.Margin = new Thickness(18, 0, 0, 0);

                startdateDateTimePicker.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B8B8B"));
                EndDatePicker.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B8B8B"));
                reminderDatePicker.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B8B8B"));
                DescriptionTextBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABADB3"));
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
        /// validating all the mandatory fields to be filled while clicking the finish  button.
        /// </summary>
        /// <returns>bool</returns>
        internal bool ValidateMandatoryFieldsInAddActivity()
        {
            var isErrorNotExists = true;
            ClearErrorsforAddActivity();
            addActivityRequiredFieldAlert.Content = "Please fill required fields";
            try
            {
                if (AddActivityCode.SelectedItem == null || string.IsNullOrEmpty(AddActivityCode.Text))
                {
                    AddActivityCode.Margin = new Thickness(1);
                    brdmainAddActivityCode.Visibility = Visibility.Visible;
                    isErrorNotExists = false;
                    AddActivityCode.Focus();
                }
                if (string.IsNullOrEmpty(DescriptionTextBox.Text))
                {
                    DescriptionTextBox.BorderBrush = System.Windows.Media.Brushes.Red;
                    isErrorNotExists = false;
                    DescriptionTextBox.Focus();
                }
                if (ownerComboBox.SelectedItem == null || string.IsNullOrEmpty(ownerComboBox.Text))
                {
                    brdmainownerComboBox.Visibility = Visibility.Visible;
                    ownerComboBox.Margin = new Thickness(1, 1, 9, 2);
                    isErrorNotExists = false;
                    ownerComboBox.Focus();
                }
                HIB.Outlook.Model.ActivityCommonLookUpInfo currentSelectedItem = null;

                if (updateComboBox.SelectedItem != null)
                {
                    //brdmainupdateComboBox.Visibility = Visibility.Visible;
                    //updateComboBox.Margin = new Thickness(19, 1, 2, 1);
                    //isErrorNotExists = false;
                    currentSelectedItem = updateComboBox.SelectedItem as HIB.Outlook.Model.ActivityCommonLookUpInfo;
                    if (string.Equals(currentSelectedItem.CommonLkpCode, "UEC", StringComparison.OrdinalIgnoreCase))
                    {
                        if (EndDatePicker.SelectedDate == null || EndDatePicker.SelectedDate.Value.Date < DateTime.Now.Date)
                        {
                            EndDatePicker.BorderBrush = System.Windows.Media.Brushes.Red;
                            isErrorNotExists = false;
                            EndDatePicker.Focus();
                        }
                    }
                }

                if (currentSelectedItem != null && string.Equals(currentSelectedItem.CommonLkpCode, "UEC", StringComparison.OrdinalIgnoreCase) && startdateDateTimePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
                {
                    var startTimeValue = new DateTime(startdateDateTimePicker.SelectedDate.Value.Year, startdateDateTimePicker.SelectedDate.Value.Month, startdateDateTimePicker.SelectedDate.Value.Day);
                    startTimeValue = startTimeValue + startTime.CurrentTime.TimeOfDay;
                    var endTimeValue = new DateTime(EndDatePicker.SelectedDate.Value.Year, EndDatePicker.SelectedDate.Value.Month, EndDatePicker.SelectedDate.Value.Day);
                    endTimeValue = endTimeValue + endTime.CurrentTime.TimeOfDay;

                    if (EndDatePicker.SelectedDate?.Date < startdateDateTimePicker.SelectedDate?.Date && endTimeValue.TimeOfDay > startTimeValue.TimeOfDay)
                    {
                        EndDatePicker.BorderBrush = System.Windows.Media.Brushes.Red;
                        isErrorNotExists = false;
                        addActivityRequiredFieldAlert.Content = "End Date cannot be less than Start Date";
                        EndDatePicker.Focus();

                    }
                }

                if (startdateDateTimePicker.SelectedDate == null || startdateDateTimePicker.SelectedDate.Value.Date < DateTime.Now.Date)
                {
                    startdateDateTimePicker.BorderBrush = System.Windows.Media.Brushes.Red;
                    isErrorNotExists = false;
                    startdateDateTimePicker.Focus();
                }
                if (reminderDatePicker.SelectedDate != null && startdateDateTimePicker.SelectedDate != null)
                {
                    var reminderTimeValue = new DateTime(reminderDatePicker.SelectedDate.Value.Year, reminderDatePicker.SelectedDate.Value.Month, reminderDatePicker.SelectedDate.Value.Day);
                    reminderTimeValue = reminderTimeValue + reminderTextBox.CurrentTime.TimeOfDay;
                    var startTimeValue = new DateTime(startdateDateTimePicker.SelectedDate.Value.Year, startdateDateTimePicker.SelectedDate.Value.Month, startdateDateTimePicker.SelectedDate.Value.Day);
                    startTimeValue = startTimeValue + startTime.CurrentTime.TimeOfDay;

                    if (reminderDatePicker.SelectedDate.Value >= startdateDateTimePicker.SelectedDate.Value && reminderTimeValue.TimeOfDay > startTimeValue.TimeOfDay)
                    {
                        reminderDatePicker.BorderBrush = System.Windows.Media.Brushes.Red;
                        isErrorNotExists = false;
                        addActivityRequiredFieldAlert.Content = "Reminder Date must be less than or equal to Start Date";
                        reminderDatePicker.Focus();
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
            return isErrorNotExists;
        }


        ///<summary>
        /// Get the Associated Item Id from selected Activity.
        /// </summary>
        /// <returns>Int64</returns>
        private Int64 GetIdFromSelectedActivity(object selectedItem, ref Int64 clientId, ref Int32 uniqAgency, ref Int32 uniqBranch)
        {
            Int64 selectedId = 0;
            try
            {

                if (selectedItem is Account)
                {
                    var account = selectedItem as Account;
                    selectedId = account.AccountId;
                    clientId = account.ClientId;
                    uniqAgency = account.UniqAgency;
                    uniqBranch = account.UniqBranch;
                }
                else if (selectedItem is Policy)
                {
                    var policy = selectedItem as Policy;
                    selectedId = policy.PolicyId;
                    clientId = policy.ClientId;
                }
                else if (selectedItem is Claim)
                {
                    var claim = selectedItem as Claim;
                    selectedId = claim.ClaimId;
                    clientId = claim.ClientId;
                }
                else if (selectedItem is HIB.Outlook.Model.Activities.Line)
                {
                    var line = selectedItem as HIB.Outlook.Model.Activities.Line;
                    selectedId = line.LineId;
                    clientId = line.ClientId;
                }
                else if (selectedItem is Opportunities)
                {
                    var opportunities = selectedItem as Opportunities;
                    selectedId = opportunities.OpportunityId;
                    clientId = opportunities.ClientId;
                }
                else if (selectedItem is Services)
                {
                    var services = selectedItem as Services;
                    selectedId = services.ServiceHeaderId;
                    clientId = services.ClientId;
                }
                else if (selectedItem is MasterMarketingSubmission)
                {
                    var masterMarketingSubmission = selectedItem as MasterMarketingSubmission;
                    selectedId = masterMarketingSubmission.MasterMarketingId;
                    clientId = masterMarketingSubmission.ClientId;
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
            return selectedId;
        }

        ///<summary>
        /// saving the new activity information in local database which will be later pushed to epic through background windows service.
        /// </summary>
        /// <returns>bool</returns>
        internal AddActivity SaveNewActivityInfoToLocalDatabase()
        {
            var addActivity = new AddActivity();
            //var status = false;
            try
            {
                Int64 selectedId = 0;
                Int64 clientId = 0;
                Int32 uniqAgency = 0;
                Int32 uniqBranch = 0;

                if (SelectedActivityValueGrid.Items.Count > 0)
                {
                    var selectedActivityAddTo = SelectedActivityValueGrid.Items[0];
                    selectedId = GetIdFromSelectedActivity(selectedActivityAddTo, ref clientId, ref uniqAgency, ref uniqBranch);
                }

                addActivity.ClientId = clientId;
                addActivity.UniqAgency = uniqAgency;
                addActivity.UniqBranch = uniqBranch;
                addActivity.ClientLookupCode = Convert.ToString(_addActivityMainPage?.addactivityFirstPage.AddActivityClientCodeLabel.Content);
                addActivity.AddtoType = _addActivityMainPage?.addactivityFirstPage.AddToActivityTypeComboBox.Text;
                addActivity.AddToTypeId = selectedId;

                var addActivityCodeSelectedItem = AddActivityCode.SelectedItem as AddActivityCode;

                addActivity.AddActivityCode = addActivityCodeSelectedItem?.Code;
                addActivity.AddActivityDescription = addActivityCodeSelectedItem?.Description;
                addActivity.AddActivityDisplayDescription = DescriptionTextBox.Text;
                addActivity.AddActivityTypeClosedStatus = addActivityCodeSelectedItem?.IsClosedStatus;
                addActivity.AddActivityId = addActivityCodeSelectedItem?.ActivityId;


                var ownerSelectedItem = ownerComboBox.SelectedItem as OwnerCode;

                addActivity.OwnerCode = ownerSelectedItem?.Code;
                addActivity.OwnerDecription = ownerSelectedItem?.Description;


                var priority = PriorityComboBox.SelectedItem as HIB.Outlook.Model.ActivityCommonLookUpInfo;

                addActivity.Priority = priority?.CommonLkpName;
                addActivity.PriorityId = priority?.CommonLkpId;

                var updateSelectedItem = updateComboBox.SelectedItem as HIB.Outlook.Model.ActivityCommonLookUpInfo;

                if (updateSelectedItem?.CommonLkpCode != "DefaultValue")
                {
                    addActivity.Update = updateSelectedItem?.CommonLkpName;
                    addActivity.UpdateId = updateSelectedItem?.CommonLkpId;
                }


                if (reminderDatePicker.SelectedDate.HasValue)
                {
                    var reminderdate = reminderDatePicker.SelectedDate as DateTime?;
                    addActivity.ReminderDate = reminderdate.Value.ToString("yyyy-MM-dd");
                    if (reminderTextBox.CurrentTime != null)
                        addActivity.ReminderTime = reminderTextBox.CurrentTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                }


                var Startdate = startdateDateTimePicker.SelectedDate as DateTime?;
                addActivity.StartDate = Startdate?.ToString("yyyy-MM-dd");
                if (startTime.CurrentTime != null)
                    addActivity.StartTime = startTime.CurrentTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);


                var Enddate = EndDatePicker.SelectedDate as DateTime?;
                if (Enddate == null)
                    Enddate = Startdate;

                addActivity.EndDate = Enddate?.ToString("yyyy-MM-dd");
                if (endTime.CurrentTime != null)
                    addActivity.EndTime = endTime.CurrentTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);


                var whoToContactSelectedItem = contactNameComboBox.SelectedItem as HIB.Outlook.Model.ActivityClientContactInfo;
                addActivity.WhoToContactId = whoToContactSelectedItem?.ContactNameId;

                if (addActivity.WhoToContactId == null)
                {
                    addActivity.WhoToContactId = 0;
                }
                addActivity.WhoToContactName = whoToContactSelectedItem?.ContactName;


                var whotoContactModeSelectedItem = contactModeComboBox.SelectedItem as HIB.Outlook.Model.ActivityClientContactInfo;

                addActivity.ContactMode = whoToContactSelectedItem?.ContactType;
                addActivity.ContactModeId = whoToContactSelectedItem?.ContactNameId;
                if (addActivity.ContactModeId == null)
                {
                    addActivity.ContactModeId = 0;
                }

                if (!string.IsNullOrEmpty(addActivity.ContactMode) && addActivity.ContactMode == "Phone")
                {
                    if (!string.IsNullOrEmpty(whoToContactSelectedItem?.ContactValue) && whoToContactSelectedItem?.ContactValue.Length == 10)
                    {
                        addActivity.ContactDetail = whoToContactSelectedItem?.ContactValue;
                    }
                    else
                    {
                        addActivity.ContactDetail = string.Empty;
                    }
                }
                else
                {
                    addActivity.ContactDetail = whoToContactSelectedItem?.ContactValue;
                }


                addActivity.ContactDetailId = whoToContactSelectedItem?.ContactNameId;

                if (addActivity.ContactDetailId == null)
                {
                    addActivity.ContactDetailId = 0;
                }


                var accessLevelSelectedItem = accessLevelComboBox.SelectedItem as HIB.Outlook.Model.ActivityCommonLookUpInfo;

                addActivity.AccessLevel = accessLevelSelectedItem?.CommonLkpName;
                addActivity.AccessLevelId = accessLevelSelectedItem?.CommonLkpId;


                addActivity.Description = accessLevelDescription.Text;

                addActivity.IsPushToEpic = 0;

                addActivity.CurrentlyLoggedUserCode = ThisAddIn.EmployeeLookupCode;

                addActivity.ActivityGuid = Guid.NewGuid().ToString();

                //var ClosedActivityTypes = Convert.ToString(ConfigurationManager.AppSettings["ClosedActivityTypeCode"]);
                //if (ClosedActivityTypes.Contains(addActivity.AddActivityCode))
                if (addActivity.AddActivityTypeClosedStatus == 1)
                {
                    addActivity.Status = "Closed";
                }
                else
                {
                    addActivity.Status = "Open";
                }

                var addActivityInfoCollection = new List<AddActivity>
                {
                    addActivity
                };
                XMLSerializeHelper.Serialize<AddActivity>(addActivityInfoCollection, XMLFolderType.AddIn);


                //string queryString = $"Insert into HIBOPAddActivity (UniqEntity,ClientLookupCode,AddToType,AddToTypeId,AddActivityCode,AddActivityDescription,AddActivityId,OwnerCode,OwnerDecription,PriorityId,Priority,UpdateId,[Update],ReminderDate,ReminderTime,StartDate,StartTime,EndDate,EndTime,WhoToContactName,WhoToContactId,ContactMode,ContactModeId,ContactDetail,ContactDetailId,AccessLevel,AccessLevelId,Description,IsPushedToEpic,CurrentlyLoggedLookupCode,TaskEventEpicId) Values ({addActivity.ClientId},'{addActivity.ClientLookupCode}','{addActivity.AddtoType}',{addActivity.AddToTypeId},'{addActivity.AddActivityCode}','{addActivity.AddActivityDescription}',{addActivity.AddActivityId},'{addActivity.OwnerCode}','{addActivity.OwnerDecription}','{addActivity.PriorityId}','{addActivity.Priority}','{addActivity.UpdateId}','{addActivity.Update}','{addActivity.ReminderDate}','{addActivity.ReminderTime}','{addActivity.StartDate}','{addActivity.StartTime}','{addActivity.EndDate}','{addActivity.EndTime}','{addActivity.WhoToContactName}','{addActivity.WhoToContactId}','{addActivity.ContactMode}',{addActivity.ContactModeId},'{addActivity.ContactDetail}',{addActivity.ContactDetailId},'{addActivity.AccessLevel}',{addActivity.AccessLevelId},'{addActivity.Description}',{addActivity.IsPushToEpic},'{addActivity.CurrentlyLoggedUserCode}',{addActivity.TaskEventEpicId})";

                //var result = HIB.Outlook.SQLite.SQLiteHandler.ExecuteCreateOrInsertQuery(queryString);
                //status= true;

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
            return addActivity;

        }

        private void ownerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ownerComboBox.SelectedItem as OwnerCode;
            if (selectedItem != null)
            {
                ownerName.Text = selectedItem.Description;
                ownerName.ToolTip = selectedItem.Description;
            }
        }

        private void updateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelectedItem = updateComboBox.SelectedItem as HIB.Outlook.Model.ActivityCommonLookUpInfo;
            if (currentSelectedItem != null)
            {
                if (string.Equals(currentSelectedItem.CommonLkpCode, "UEC", StringComparison.OrdinalIgnoreCase))
                {
                    lblEndDateMandatory.Visibility = Visibility.Visible;
                }
                else
                {
                    lblEndDateMandatory.Visibility = Visibility.Collapsed;
                }
            }

        }

        private void startdateDateTimePicker_GotFocus(object sender, RoutedEventArgs e)
        {
            //var dateTimePicker = sender as DatePicker;
            //startdateDateTimePicker.FocusOnText();
        }

        private void startdateDateTimePicker_Loaded(object sender, RoutedEventArgs e)
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
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }
    }
}
