using HIB.Outlook.UI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using HIB.Outlook.Model.Activities;
using HIB.Outlook.Sync.Common;
using HIB.Outlook.Helper.Common;

namespace OutlookAddIn1.UserControls
{
    /// <summary>
    /// Interaction logic for AddActivityMainPage.xaml
    /// </summary>
    public partial class AddActivityMainPage : UserControl
    {

        internal Window _addactivityWindow = null;
        internal AttachmentControls _attachmentControl = null;
        string EmployeeLookupCode = string.Empty;

        public AddActivityMainPage()
        {
            EmployeeLookupCode = CommonHelper.GetLookUpCode();
        }
        public AddActivityMainPage(Window addactivityWindow, AttachmentControls attachmentControl)
        {
            InitializeComponent();
            _addactivityWindow = addactivityWindow;
            _attachmentControl = attachmentControl;
            addactivityFirstPage.Tag = this;
            addactivitySecondPage.Tag = this;
        }


        private void btnAddActivityClose_Click(object sender, RoutedEventArgs e)
        {
            addactivitySecondPage.ResetAllDataForAddActivity();
            addactivitySecondPage.ClearErrorsforAddActivity();
            _addactivityWindow.Hide();
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_addactivityWindow != null)
            {
                _addactivityWindow.DragMove();
            }
        }

        private void btnAddNewActivityFinish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addactivitySecondPage.addActivityRequiredFieldAlert.Visibility = Visibility.Collapsed;


                if (addactivitySecondPage.updateComboBox.SelectedItem != null)
                {
                    var currentSelectedItem = addactivitySecondPage.updateComboBox.SelectedItem as HIB.Outlook.Model.ActivityCommonLookUpInfo;
                    if (!string.Equals(currentSelectedItem.CommonLkpCode, "UEC", StringComparison.OrdinalIgnoreCase))
                    {
                        if (addactivitySecondPage.EndDatePicker.SelectedDate == null)
                        {
                            var startTimeValue = new DateTime(addactivitySecondPage.startdateDateTimePicker.SelectedDate.Value.Year, addactivitySecondPage.startdateDateTimePicker.SelectedDate.Value.Month, addactivitySecondPage.startdateDateTimePicker.SelectedDate.Value.Day);
                            startTimeValue = startTimeValue + addactivitySecondPage.startTime.CurrentTime.TimeOfDay;
                            addactivitySecondPage.EndDatePicker.SelectedDate = startTimeValue;
                            addactivitySecondPage.endTime.CurrentTime = startTimeValue;
                        }
                    }
                }
                else
                {
                    if (addactivitySecondPage.startdateDateTimePicker.SelectedDate != null && addactivitySecondPage.EndDatePicker.SelectedDate == null)
                    {
                        var startTimeValue = new DateTime(addactivitySecondPage.startdateDateTimePicker.SelectedDate.Value.Year, addactivitySecondPage.startdateDateTimePicker.SelectedDate.Value.Month, addactivitySecondPage.startdateDateTimePicker.SelectedDate.Value.Day);
                        startTimeValue = startTimeValue + addactivitySecondPage.startTime.CurrentTime.TimeOfDay;
                        addactivitySecondPage.EndDatePicker.SelectedDate = startTimeValue;
                        addactivitySecondPage.endTime.CurrentTime = startTimeValue;
                    }
                }

                if (addactivitySecondPage.ValidateMandatoryFieldsInAddActivity())
                {
                    // AddActivityPopup.IsOpen = false;
                    var addActivity = addactivitySecondPage.SaveNewActivityInfoToLocalDatabase();
                    var ActivitySelectedItem = addactivityFirstPage.AddToActivityTypeComboBox.SelectedItem as string;
                    var query = string.Empty;
                    if (ActivitySelectedItem != null)
                    {
                        //if (ActivitySelectedItem == "Account")
                        //    query = "Select *  From HIBOPAddActivity A  INNER JOIN HIBOPActivityAccount AT on A.AddToTypeId =AT.AccountId  where   A.TaskEventEpicId= 0 and A.IsPushedToEpic = 0 and A.UniqEntity ={0} and A.CurrentlyLoggedLookupCode ='{1}' order by Id desc Limit 1";
                        //else if (ActivitySelectedItem == "Claim")
                        //    query = "Select *  From HIBOPAddActivity A  INNER JOIN HIBOPClaim AT on A.AddToTypeId =AT.UniqClaim  where   A.TaskEventEpicId= 0 and A.IsPushedToEpic = 0 and A.UniqEntity ={0} and A.CurrentlyLoggedLookupCode ='{1}' order by Id desc Limit 1";
                        //else if (ActivitySelectedItem == "Policy")
                        //    query = "Select *  From HIBOPAddActivity A  INNER JOIN HIBOPPolicy AT on A.AddToTypeId =AT.UniqPolicy  where   A.TaskEventEpicId= 0 and A.IsPushedToEpic = 0 and A.UniqEntity ={0} and A.CurrentlyLoggedLookupCode ='{1}' order by Id desc Limit 1";
                        //else if (ActivitySelectedItem == "Line")
                        //    query = "Select *  From HIBOPAddActivity A  INNER JOIN HIBOPActivityLine AT on A.AddToTypeId =AT.UniqLine  where   A.TaskEventEpicId= 0 and A.IsPushedToEpic = 0 and A.UniqEntity ={0} and A.CurrentlyLoggedLookupCode ='{1}' order by Id desc Limit 1";
                        //else if (ActivitySelectedItem == "Opportunities")
                        //    query = "Select *  From HIBOPAddActivity A  INNER JOIN HIBOPActivityOpportunity AT on A.AddToTypeId =AT.UniqOpportunity  where   A.TaskEventEpicId= 0 and A.IsPushedToEpic = 0 and A.UniqEntity ={0} and A.CurrentlyLoggedLookupCode ='{1}' order by Id desc Limit 1";
                        //else if (ActivitySelectedItem == "Services")
                        //    query = "Select *  From HIBOPAddActivity A  INNER JOIN HIBOPActivityServices AT on A.AddToTypeId =AT.UniqServiceHead  where   A.TaskEventEpicId= 0 and A.IsPushedToEpic = 0 and A.UniqEntity ={0} and A.CurrentlyLoggedLookupCode ='{1}' order by Id desc Limit 1";
                        //else if (ActivitySelectedItem == "Master Marketing Submission")
                        //    query = "Select *  From HIBOPAddActivity A  INNER JOIN HIBOPActivityMasterMarketing AT on A.AddToTypeId =AT.UniqMarketingSubmission  where   A.TaskEventEpicId= 0 and A.IsPushedToEpic = 0 and A.UniqEntity ={0} and A.CurrentlyLoggedLookupCode ='{1}' order by Id desc Limit 1";

                        var latestAddedActivity = _attachmentControl.GetNewAddedActivities(addActivity);

                        _attachmentControl.ShowStatus("Activity is being created sucessfully");
                        _attachmentControl.OpenActivitiesInfoCollection.OrderBy(x => x.PolicyDesc);
                        //_attachmentControl.OnPropertyChanged("OpenActivitiesInfoCollection");
                        //_attachmentControl.OnPropertyChanged("ClosedActivitiesInfoCollection");

                        if (latestAddedActivity?.FirstOrDefault()?.Status == "Open")
                        {
                            _attachmentControl.OpenActivityTab.IsSelected = true;
                            _attachmentControl.ActiveActivityList.SelectedItem = latestAddedActivity?.FirstOrDefault();
                            _attachmentControl.ActiveActivityList.ScrollIntoView(_attachmentControl.ActiveActivityList.SelectedItem);
                        }
                        else
                        {
                            _attachmentControl.ClosedActivityTab.IsSelected = true;
                            _attachmentControl.InActiveActivityList.SelectedItem = latestAddedActivity?.FirstOrDefault();
                            _attachmentControl.InActiveActivityList.ScrollIntoView(_attachmentControl.InActiveActivityList.SelectedItem);
                        }

                    }


                    addactivitySecondPage.ResetAllDataForAddActivity();
                    addactivityFirstPage.Visibility = Visibility.Visible;
                    addactivitySecondPage.Visibility = Visibility.Collapsed;
                    _addactivityWindow.Hide();
                }
                else
                {
                    addactivitySecondPage.addActivityRequiredFieldAlert.Visibility = Visibility.Visible;
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

        private void btnAddNewActivityCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_addactivityWindow != null)
                {
                    addactivityFirstPage.Visibility = Visibility.Visible;
                    addactivitySecondPage.Visibility = Visibility.Collapsed;
                    addactivitySecondPage.ResetAllDataForAddActivity();
                    addactivitySecondPage.ClearErrorsforAddActivity();
                    _addactivityWindow.Hide();
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (addactivityMainPageGrid.RowDefinitions.Count >= 2)
                    addactivityMainPageGrid.RowDefinitions[1].Height = new GridLength(0);
                addactivityFirstPage.Visibility = Visibility.Visible;
                addactivitySecondPage.Visibility = Visibility.Collapsed;
                // addactivitySecondPage.ClearErrorsforAddActivity();
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
        internal void RefreshDateControls()
        {
            addactivitySecondPage.reminderTextBox.CurrentTime = DateTime.Now;
            addactivitySecondPage.startTime.CurrentTime = DateTime.Now.Date.AddMinutes(1);
            addactivitySecondPage.endTime.CurrentTime = DateTime.Now;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
