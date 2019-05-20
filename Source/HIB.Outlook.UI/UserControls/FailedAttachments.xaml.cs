using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HIB.Outlook.Common;
using HIB.Outlook.Helper.Common;
using HIB.Outlook.Model;
using HIB.Outlook.Sync.Common;
using HIB.Outlook.UI;

namespace OutlookAddIn1.UserControls
{
    /// <summary>
    /// Interaction logic for FailedAttachments.xaml
    /// </summary>
    public partial class FailedAttachments : UserControl
    {
        internal Window _failedAttachmentWindow = null;
        string EmployeeLookupCode = string.Empty;
        internal List<SelectedEmailInfo> FailedNotificationCollection = new List<SelectedEmailInfo>();
        internal AttachmentControls _attachmentControl = null;
        internal bool isUncheckParentOnly = false;
        internal bool isselectAllClicked = false;
        public FailedAttachments(Window failedAttachment)
        {
            InitializeComponent();
            EmployeeLookupCode = CommonHelper.GetLookUpCode();
            _failedAttachmentWindow = failedAttachment;
        }
        private void FailedAttachments_Loaded(object sender, RoutedEventArgs e)
        {
            // LoadDummiesForSelectedItems();
        }
        private void LstFailedAttachment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var selectedItem = e.AddedItems[0] as SelectedEmailInfo;
                if (selectedItem != null)
                {
                    //selectedItem.ImagePath = "../Asset/email_image_selected.png";
                    rightPartMainGrid.Visibility = Visibility.Visible;
                    errorMessage.Text = selectedItem.ErrorMessage;
                    clientLookupCode.Text = selectedItem.Client;
                    activityCode.Text = selectedItem.Activity;
                    MailPreviewToTextBlock.Text = selectedItem.To;
                    MailPreviewSubjectTextBlock.Text = selectedItem.Subject;
                    MailPreviewFromTextBlock.Text = selectedItem.From;
                    MailPreviewRecievedTimeTextBlock.Text = selectedItem.MailRecievedDateWithTime;
                    MailPreviewCcTextBlock.Text = selectedItem.Cc;
                    var html = $"<html><meta charset='UTF-8'><body>{selectedItem.HtmlBody}</body></html>";

                    string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                    MatchCollection matchesImgSrc = Regex.Matches(html, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    for (int i = 0; i < matchesImgSrc.Count; i++)
                    {
                        html = html.Replace(matchesImgSrc[i].Value, "");
                    }
                    webBrowser.NavigateToString(html);
                }
            }
        }




        private void CustomButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _failedAttachmentWindow.Close();
        }

        private void bdrHeader_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _failedAttachmentWindow.DragMove();
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

        private void RetryAttachment_Click(object sender, RoutedEventArgs e)
        {
            List<AttachmentInfo> selectedFailAttachments = new List<AttachmentInfo>();
            var selectedItems = LstFailedAttachment.SelectedItems;
            if (selectedItems.Count > 0)
            {
                foreach (SelectedEmailInfo selectedItem in selectedItems)
                {
                    AttachmentInfo attachmentInfo = new AttachmentInfo();
                    attachmentInfo.AttachmentId = selectedItem.AttachmentId;
                    selectedFailAttachments.Add(attachmentInfo);
                }
                failureTextBlock.Visibility = Visibility.Visible;
                XMLSerializeHelper.Serialize<AttachmentInfo>(selectedFailAttachments, XMLFolderType.Service, "RetryFailedAttachments");
            }

        }

        private void DeleteAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItems = LstFailedAttachment.SelectedItems;
                if (selectedItems.Count > 0)
                {
                    if (selectedItems.Count == 1)
                    {
                        DeleteAction(selectedItems);
                    }
                    else
                    {
                        ConfirmationUser confirmationUser = new ConfirmationUser();
                        confirmationUser.Owner = _failedAttachmentWindow;
                        confirmationUser.ShowDialog();
                        if (confirmationUser._result == MessageBoxResult.Yes)
                        {
                            DeleteAction(selectedItems);
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

        public void DeleteAction(System.Collections.IList selectedItems)
        {
            List<AttachmentInfo> selectedFailAttachments = new List<AttachmentInfo>();
            foreach (SelectedEmailInfo selectedItem in selectedItems)
            {
                AttachmentInfo attachmentInfo = new AttachmentInfo();
                attachmentInfo.AttachmentId = selectedItem.AttachmentId;
                selectedFailAttachments.Add(attachmentInfo);
                FailedNotificationCollection.Remove(selectedItem);
            }
            LstFailedAttachment.ItemsSource = null;
            LstFailedAttachment.ItemsSource = FailedNotificationCollection;

            if (FailedNotificationCollection.Count > 0)
            {
                if (LstFailedAttachment.Items.Count > 0)
                {
                    LstFailedAttachment.SelectedIndex = 0;
                }
                rightPartMainGrid.Visibility = Visibility.Visible;
            }
            else
            {
                rightPartMainGrid.Visibility = Visibility.Collapsed;
            }

            _attachmentControl.UpdateFailedCountForAttachment(FailedNotificationCollection.Count);
            XMLSerializeHelper.Serialize<AttachmentInfo>(selectedFailAttachments, XMLFolderType.Service, "DeleteFailedAttachments");
        }

        private void SelectAllDescription_Checked(object sender, RoutedEventArgs e)
        {
            isselectAllClicked = true;
            LstFailedAttachment.SelectAll();
            isselectAllClicked = false;
        }

        private void SelectAllDescription_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isUncheckParentOnly)
                LstFailedAttachment.SelectedItems.Clear();
        }

        private void Attachmentdescription_Checked(object sender, RoutedEventArgs e)
        {
            if (!isselectAllClicked)
                CheckOrUncheckSelectAll();
        }

        private void CheckOrUncheckSelectAll()
        {
            if (LstFailedAttachment.SelectedItems.Count == FailedNotificationCollection.Count)
            {
                selectAllDescription.IsChecked = true;
            }
            else
            {
                selectAllDescription.IsChecked = false;
            }
        }

        private void Attachmentdescription_Unchecked(object sender, RoutedEventArgs e)
        {
            isUncheckParentOnly = true;
            CheckOrUncheckSelectAll();
            isUncheckParentOnly = false;
        }

    }
}
