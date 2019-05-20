using HIB.Outlook.Helper.Common;
using HIB.Outlook.Sync.Common;
using HIB.Outlook.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OutlookAddIn1.UserControls
{
    /// <summary>
    /// Interaction logic for PolicyTypeList.xaml
    /// </summary>
    public partial class PolicyTypeList : UserControl, INotifyPropertyChanged
    {
        internal Window _policyTypeWindow = null;
        internal AttachmentControls _attachmentControl = null;
        internal List<PolicyTypeInfo> PolicyTypeInfoCollection = new List<PolicyTypeInfo>();
        private const string _policyListActivitiesQuery = "Select * From HIBOPPolicyLineType";
        string EmployeeLookupCode = string.Empty;
        public PolicyTypeList(Window policyTypeWindow, AttachmentControls attachmentControl)
        {
            InitializeComponent();
            EmployeeLookupCode = CommonHelper.GetLookUpCode();
            this.DataContext = this;
            _policyTypeWindow = policyTypeWindow;
            _attachmentControl = attachmentControl;
        }

        private void BtnPolicyTypePopupClose_Click(object sender, RoutedEventArgs e)
        {
            if (_policyTypeWindow != null)
            {
                _policyTypeWindow.Hide();
            }
        }

        public IEnumerable<PolicyTypeInfo> PolicyTypeFilteredItems
        {
            get
            {
                if (string.IsNullOrEmpty(policyTypeSearchTextBox.Text)) return PolicyTypeInfoCollection;
                else
                    return PolicyTypeInfoCollection.Where(x => x.PolicyTypeDescription.ToUpper().Contains(policyTypeSearchTextBox.Text.ToUpper()) || x.PolicyTypeCode.ToUpper().Contains(policyTypeSearchTextBox.Text.ToUpper()));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void policyTypeSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(policyTypeSearchTextBox.Text) && policyTypeSearchTextBox.Text.Length > 0)
                {
                    clearFiltertext.Visibility = Visibility.Visible;
                }
                else
                {
                    clearFiltertext.Visibility = Visibility.Collapsed;
                }
                OnPropertyChanged("PolicyTypeFilteredItems");

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

        private void ClearFiltertext_Click(object sender, RoutedEventArgs e)
        {
            policyTypeSearchTextBox.Clear();
        }

        private void DgPolicyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedItem = DgPolicyType.SelectedItem as PolicyTypeInfo;
                if (selectedItem != null)
                {
                    btnDonePolicyType.IsEnabled = true;
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


        private void BtnDonePolicyType_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var selectedItem = DgPolicyType.SelectedItem as PolicyTypeInfo;
                if (selectedItem != null)
                {
                    _attachmentControl.lblPolicyType.Text = selectedItem.PolicyTypeCode;
                    _attachmentControl.policyTypeTextBlock.Text = selectedItem.PolicyTypeCode;
                    _attachmentControl.policyTypeTextBlock.Visibility = Visibility.Visible;
                }
                _policyTypeWindow.Hide();
                // DgPolicyType.SelectedItem = null;
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

        private void BtnCancelPolicyType_Click(object sender, RoutedEventArgs e)
        {
            if (_policyTypeWindow != null)
            {
                _policyTypeWindow.Hide();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        ///<summary>
        /// getting all the policy type list from local database.
        /// </summary>
        /// <returns>List<MailItemInfo></returns>
        internal void GetPolicyTypeListFromSQLite()
        {
            try
            {
                PolicyTypeInfoCollection = new List<PolicyTypeInfo>();

                var sqliteDataReader = HIB.Outlook.SQLite.SQLiteHandler.ExecuteSelectQuery(_policyListActivitiesQuery);
                if (sqliteDataReader != null)
                {
                    while (sqliteDataReader.Read())
                    {
                        try
                        {
                            var policyTypeInfo = new PolicyTypeInfo
                            {
                                PolicyTypeCode = Convert.ToString(sqliteDataReader["CdPolicyLineTypeCode"]),
                                PolicyTypeDescription = Convert.ToString(sqliteDataReader["PolicyLineTypeDesc"])
                            };
                            PolicyTypeInfoCollection.Add(policyTypeInfo);

                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
                        }

                    }
                    if (PolicyTypeInfoCollection.Count > 0)
                    {
                        PolicyTypeInfoCollection = PolicyTypeInfoCollection.OrderBy(m => m.PolicyTypeCode).ToList();
                    }
                    OnPropertyChanged("PolicyTypeFilteredItems");
                    sqliteDataReader.Close();
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

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_policyTypeWindow != null)
            {
                _policyTypeWindow.DragMove();
            }
        }
    }
}
