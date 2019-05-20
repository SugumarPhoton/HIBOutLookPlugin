using HIB.Outlook.Helper.Common;
using HIB.Outlook.Sync.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OutlookAddIn1.UserControls
{
    /// <summary>
    /// Interaction logic for AttachToEpicMainPage.xaml
    /// </summary>
    public partial class AttachToEpicMainPage : UserControl
    {
        internal Window _multipleEmailAttachmentWindow = null;

        string EmployeeLookupCode = string.Empty;

        public AttachToEpicMainPage(Window multipleEmailAttachmentWindow)
        {
            InitializeComponent();
            EmployeeLookupCode = CommonHelper.GetLookUpCode();
            _multipleEmailAttachmentWindow = multipleEmailAttachmentWindow;
            attachmentControls.Tag = this;
            attachmentControls.attachToEpicMainPage = this;
            multipleEmailAttachment.Tag = this;
        }

        private void btnAddActivityClose_Click(object sender, RoutedEventArgs e)
        {
            if (_multipleEmailAttachmentWindow != null)
            {
                _multipleEmailAttachmentWindow.Hide();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (attachmentControls != null)
                { 
                    attachmentControls.Height = 615;
                    attachmentControls.leftMainGrid.Height = 518;
                    attachmentControls.leftMainGrid.Margin = new Thickness(0, 15, 0, 0);
                    attachmentControls.RightPaneGrid.Margin = new Thickness(0, 10, 0, 0);
                    attachmentControls.multipleattachmentGrid.Visibility = Visibility.Visible;
                    attachmentControls.sendAndAttachGrid.Visibility = Visibility.Collapsed;
                    attachmentControls.attachanddeleteonly.Visibility = Visibility.Collapsed;
                    attachmentControls.TbtnAttachmentAssist.Visibility = Visibility.Collapsed;
                    attachmentControls.descriptionBorder.Visibility = Visibility.Collapsed;
                    attachmentControls.btnMin.Visibility = Visibility.Collapsed;
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
            try
            {
                _multipleEmailAttachmentWindow.DragMove();
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
