using HIB.Outlook.Common;
using HIB.Outlook.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OutlookNS = Microsoft.Office.Interop.Outlook;
using System.Windows.Media;
using HIB.Outlook.Sync.Common;
using AttachmentBridge;
using HIB.Outlook.Helper.Common;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace OutlookAddIn1.UserControls
{
    /// <summary>
    /// Interaction logic for MultipleEmailAttachment.xaml
    /// </summary>
    public partial class MultipleEmailAttachment : UserControl
    {
        AttachToEpicMainPage _attachtoEpicMainPage = null;
        string EmployeeLookupCode = string.Empty;

        public MultipleEmailAttachment()
        {
            InitializeComponent();
            EmployeeLookupCode = CommonHelper.GetLookUpCode();
        }

        private void rbCustom_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (_attachtoEpicMainPage != null && _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection.Count > 0)
                {
                    foreach (var selectedMail in _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection)
                    {
                        selectedMail.TextBoxValue = string.Empty;
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

        private void rbEmailDesc_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_attachtoEpicMainPage != null && _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection.Count > 0)
                {
                    foreach (var selectedMail in _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection)
                    {
                        selectedMail.TextBoxValue = selectedMail.Subject;
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

        private void rbActivityDec_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_attachtoEpicMainPage != null && _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection.Count > 0)
                {
                    foreach (var selectedMail in _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection)
                    {
                        selectedMail.TextBoxValue = selectedMail.ActivityDesc;
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (_attachtoEpicMainPage != null)
            {
                _attachtoEpicMainPage.attachmentControls.Visibility = Visibility.Visible;
                _attachtoEpicMainPage._multipleEmailAttachmentWindow.Height = 620;
                _attachtoEpicMainPage.multipleEmailAttachment.Visibility = Visibility.Collapsed;
            }
        }

        ///<summary>
        /// saving the multiple attachment details to local database which will be used later by windows service to push it in epic.
        /// </summary>
        /// <returns>void</returns>
        public void SaveAllAttachmentInfo(bool isAttachDlete = false)
        {
            try
            {
                if (_attachtoEpicMainPage != null)
                {
                    var clientSelectedItem = _attachtoEpicMainPage.attachmentControls.ActiveTab.IsSelected ? (_attachtoEpicMainPage.attachmentControls.LstActiveClient.SelectedItem as HIB.Outlook.UI.ClientInfo) : (_attachtoEpicMainPage.attachmentControls.LstInActiveClient.SelectedItem as HIB.Outlook.UI.ClientInfo);
                    var activitySelectedItem = _attachtoEpicMainPage.attachmentControls.OpenActivityTab.IsSelected ? (_attachtoEpicMainPage.attachmentControls.ActiveActivityList.SelectedItem as PolicyInfo) : (_attachtoEpicMainPage.attachmentControls.InActiveActivityList.SelectedItem as PolicyInfo);
                    var policyTypeSelectedItem = _attachtoEpicMainPage.attachmentControls.RBPolicyType.IsChecked == true ? (_attachtoEpicMainPage.attachmentControls.policyList?.DgPolicyType.SelectedItem as HIB.Outlook.UI.PolicyTypeInfo) : null;
                    var folderSelectedItem = _attachtoEpicMainPage.attachmentControls.mainFolderComboBox.SelectedItem as HIB.Outlook.UI.FolderInfo;
                    var subFolder1SelectedItem = _attachtoEpicMainPage.attachmentControls.subFolder1ComboBox.SelectedItem as HIB.Outlook.UI.FolderInfo;
                    var subFolder2SelectedItem = _attachtoEpicMainPage.attachmentControls.subFolder2ComboBox.SelectedItem as HIB.Outlook.UI.FolderInfo;
                    var attachmentInfoCollection = new List<HIB.Outlook.Model.AttachmentInfo>();
                    var displayName = _attachtoEpicMainPage.attachmentControls.GetDisplayNameFromActiveDirectory();
                    //_attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection = _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection.OrderByDescending(m => m.MailItem?.ReceivedTime).ToList();
                    foreach (var item in _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection)
                    {
                        try
                        {
                            var attachmentIdentifierProperty = item.MailItem?.UserProperties["AttachmentIdentifier"];
                            var description = item.TextBoxValue;
                            var descriptionFrom = rbCustom.IsChecked == true ? rbCustom.Content.ToString() : rbEmailDesc.IsChecked == true ? rbEmailDesc.Content.ToString() : rbActivityDec.IsChecked == true ? rbActivityDec.Content.ToString() : string.Empty;
                            var MailBody = _attachtoEpicMainPage.attachmentControls.RemoveHiddenlinksInHtmlMailBody(item.MailItem?.Body);
                            var filename = string.Format("{0}\\MailItem_{1}.msg", _attachtoEpicMainPage.attachmentControls._FilePath, DateTime.Now.Ticks);


                            var attachmentInfo = new HIB.Outlook.Model.AttachmentInfo
                            {
                                ClientId = clientSelectedItem.ClientId,
                                ActivityId = activitySelectedItem.ActivityId,
                                ActivityGuid = activitySelectedItem.ActivityGuid,
                                IsClosedActivity = activitySelectedItem.IsClosed,
                                IsActiveClient = clientSelectedItem.IsActive,
                                DescriptionFrom = descriptionFrom,
                                Description = description.Length > Convert.ToInt32(ConfigurationManager.AppSettings["DescriptionLength"]) ? description.Remove(Convert.ToInt32(ConfigurationManager.AppSettings["DescriptionLength"]))?.Replace("'", "") : description.Replace("'", ""),
                                //FileDetails = new HIB.Outlook.Model.FileInfo { FileExtension = ".msg", FileName = HIB.Outlook.SQLite.SQLiteHandler.DoQuotes(item.MailItem.Subject?.Replace("'", "")) },
                                FileDetails = new HIB.Outlook.Model.FileInfo { FileExtension = ".msg", FileName = string.IsNullOrEmpty(item.MailItem.Subject?.Trim()) ? HIB.Outlook.SQLite.SQLiteHandler.DoQuotes(description.Replace("'", "")) : HIB.Outlook.SQLite.SQLiteHandler.DoQuotes(item.MailItem.Subject?.Replace("'", "")) },
                                FolderDetails = new HIB.Outlook.Model.FolderInfo { ParentFolderId = folderSelectedItem?.FolderId, ParentFolderName = folderSelectedItem?.FolderName, FolderId = subFolder1SelectedItem?.FolderId, FolderName = subFolder1SelectedItem?.FolderName, SubFolderId = subFolder2SelectedItem == null ? 0 : subFolder2SelectedItem.FolderId, SubFolderName = subFolder2SelectedItem?.FolderName },
                                PolicyCode = string.IsNullOrEmpty(_attachtoEpicMainPage.attachmentControls.lblPolicyType.Text) || string.Equals("(none)", _attachtoEpicMainPage.attachmentControls.lblPolicyType.Text) ? string.Empty : _attachtoEpicMainPage.attachmentControls.lblPolicyType.Text,
                                PolicyType = policyTypeSelectedItem == null ? "(none)" : policyTypeSelectedItem?.PolicyTypeDescription,
                                PolicyYear = (_attachtoEpicMainPage.attachmentControls.LstPolicyList.SelectedItem as PolicyYear)?.Description,
                                ClientAccessible = _attachtoEpicMainPage.attachmentControls.chkUntilDate.IsChecked == true ? (_attachtoEpicMainPage.attachmentControls.DpAccesibility?.SelectedDate != null ? _attachtoEpicMainPage.attachmentControls.DpAccesibility.SelectedDate.Value.ToString("yyyy/MM/dd") : null) : null,
                                IsClientAccessible = (bool)_attachtoEpicMainPage.attachmentControls.chkUntilDate.IsChecked,
                                EmailFromAddress = _attachtoEpicMainPage.attachmentControls.GetSenderMailId(item.MailItem, true)?.Replace("'", ""),
                                EmailFromDisplayName = _attachtoEpicMainPage.attachmentControls.GetSenderMailId(item.MailItem)?.Replace("'", ""),
                                EmailToAddress = _attachtoEpicMainPage.attachmentControls.GetListOfToMailId(item.MailItem, OutlookNS.OlMailRecipientType.olTo, true)?.Replace("'", ""),//item.MailItem.To,
                                EmailToDisplayName = _attachtoEpicMainPage.attachmentControls.GetListOfToMailId(item.MailItem, OutlookNS.OlMailRecipientType.olTo)?.Replace("'", ""),
                                EmailCCAddress = _attachtoEpicMainPage.attachmentControls.GetListOfToMailId(item.MailItem, OutlookNS.OlMailRecipientType.olCC, true)?.Replace("'", ""),
                                EmailCCDisplayName = _attachtoEpicMainPage.attachmentControls.GetListOfToMailId(item.MailItem, OutlookNS.OlMailRecipientType.olCC)?.Replace("'", ""),
                                Subject = item.MailItem.Subject?.Replace("'", ""),
                                MailBody = MailBody?.Length > Convert.ToInt32(ConfigurationManager.AppSettings["SubjectLeng"]) ? MailBody.Remove(Convert.ToInt32(ConfigurationManager.AppSettings["SubjectLeng"])).Replace("'", "") : MailBody.Replace("'", ""),
                                CreatedDate = Convert.ToDateTime(HIB.Outlook.Common.Common.UniversalDateTimeConversionToSQLite(DateTime.Now)),
                                ModifiedDate = Convert.ToDateTime(HIB.Outlook.Common.Common.UniversalDateTimeConversionToSQLite(DateTime.Now)),
                                IsAttachDelete = isAttachDlete,
                                ReceivedDate = String.Format("{0:MM/dd/yyyy}", item.MailItem.ReceivedTime),
                                ReceivedDateWithTime = item.MailItem.ReceivedTime.ToString("ddd MM/dd/yyyy hh:mm tt"),
                                AttachmentFilePath = filename,
                                Identifier = item.Identifier,
                                EmployeeCode = ThisAddIn.EmployeeLookupCode,
                                AttachmentMailBody = MailBody?.Length > Convert.ToInt32(ConfigurationManager.AppSettings["SubjectLeng"]) ? MailBody.Remove(Convert.ToInt32(ConfigurationManager.AppSettings["SubjectLeng"])).Replace("'", "") : MailBody.Replace("'", ""),
                                DomainName = Environment.UserDomainName,
                                UserName = string.IsNullOrEmpty(displayName) ? Environment.UserName : displayName,
                                AttachmentIdentifier = attachmentIdentifierProperty?.Value?.ToString(),
                                DisplayMailBody = item.MailItem?.HTMLBody.Replace("'", "").Trim()
                            };

                            item.MailItem.SaveAs(filename);

                            attachmentInfoCollection.Add(attachmentInfo);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
                        }

                        //string queryString = $"Insert into AttachmentInfo (ClientId,ActivityId,Description,FileExtension,FileName,ParentFolderId,ParentFolderName,FolderId,FolderName,SubFolderId,SubFolderName,PolicyCode,PolicyType,PolicyYear,ClientAccessible,EmailFromAddress,EmailToAddress,Subject,ReceivedDate,AttachmentFilePath,IsPushedToEpic,IsAttachDelete,Identifier,IsActiveClient,IsActiveActivity,DescFrom,EmployeeCode,CreatedDate,ModifiedDate,AttachmentMailBody,IsDeletedFromZFolder) Values ({attachmentInfo.ClientId},{attachmentInfo.ActivityId},'{attachmentInfo.Description}','{attachmentInfo.FileDetails.FileExtension}','{attachmentInfo.FileDetails.FileName}',{attachmentInfo.FolderDetails.ParentFolderId},'{attachmentInfo.FolderDetails.ParentFolderName}',{attachmentInfo.FolderDetails.FolderId},'{attachmentInfo.FolderDetails.FolderName}','{attachmentInfo.FolderDetails.SubFolderId}','{attachmentInfo.FolderDetails.SubFolderName}','{attachmentInfo.PolicyCode}','{attachmentInfo.PolicyType}','{attachmentInfo.PolicyYear}','{attachmentInfo.ClientAccessible}','{attachmentInfo.EmailFromAddress}','{attachmentInfo.EmailToAddress}','{attachmentInfo.Subject}','{attachmentInfo.ReceivedDate}','{attachmentInfo.AttachmentFilePath}',{0},{Convert.ToInt32(attachmentInfo.IsAttachDelete)},'{attachmentInfo.Identifier}',{Convert.ToInt32(attachmentInfo.IsActiveClient)},{Convert.ToInt32(attachmentInfo.IsClosedActivity)}, '{attachmentInfo.DescriptionFrom}','{ThisAddIn.EmployeeLookupCode}','{Common.DateTimeSQLite(DateTime.Now)}','{Common.DateTimeSQLite(DateTime.Now)}','{attachmentInfo.AttachmentMailBody}',{0})";
                        //var result = HIB.Outlook.SQLite.SQLiteHandler.ExecuteCreateOrInsertQuery(queryString);
                        //string logAuditqueryString = $"Insert into HIBOPOutlookPluginLog (UniqId,AttachmentInfoId,UniqEmployee,UniqEntity,UniqActivity,PolicyYear,PolicyType,DescriptionType,Description,FolderId,SubFolder1Id,SubFolder2Id,ClientAccessibleDate,EmailAction,InsertedDate,UpdatedDate,EmailSubject) Values ('{attachmentInfo.Identifier}',{result.RowId},'{ThisAddIn.EmployeeLookupCode}',{attachmentInfo.ClientId},{attachmentInfo.ActivityId},'{attachmentInfo.PolicyYear}','{attachmentInfo.PolicyType}','{attachmentInfo.DescriptionFrom}','{attachmentInfo.Description}',{attachmentInfo.FolderDetails.ParentFolderId},{attachmentInfo.FolderDetails.FolderId},'{attachmentInfo.FolderDetails.SubFolderId}','{attachmentInfo.ClientAccessible}','{""}','{ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}','{ExtensionClass.SqliteDateTimeFormat(DateTime.Now)}','{attachmentInfo.Subject}')";
                        //var Auditresult = HIB.Outlook.SQLite.SQLiteHandler.ExecuteCreateOrInsertQuery(logAuditqueryString);
                    }
                    XMLSerializeHelper.Serialize<HIB.Outlook.Model.AttachmentInfo>(attachmentInfoCollection, XMLFolderType.AddIn);
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
        private void ShowStatus(string message)
        {
            TextBlock text = new TextBlock();
            text.Foreground = System.Windows.Media.Brushes.Green;
            text.Text = message;
            text.Style = this.TryFindResource("FaderStyle") as Style;
            FailureAlertGrid.Children.Clear();
            FailureAlertGrid.Children.Add(text);
        }
        private bool ValidateDescField()
        {
            try
            {
                if (_attachtoEpicMainPage != null && _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection.Count > 0)
                {
                    var DescTextBoxemptyCollection = _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection.Where(m => string.IsNullOrEmpty(m.TextBoxValue)).ToList();
                    if (DescTextBoxemptyCollection.Count > 0)
                    {
                        var resource = this.TryFindResource("DescBorderBrush") as SolidColorBrush;
                        resource.Color = Colors.Red;
                        return false;
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
            return true;
        }

        private void btnAttachmentOnly_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (_attachtoEpicMainPage != null)
                {
                    var resource = this.TryFindResource("DescBorderBrush") as SolidColorBrush;
                    Color clr = (Color)ColorConverter.ConvertFromString("#FF8D8A8A");
                    resource.Color = clr;
                    requiredFieldAlert.Visibility = Visibility.Collapsed;
                    if (ValidateDescField())
                    {
                        _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection.ForEach(x =>
                        {
                            var attachmentIdentifier = x.MailItem.UserProperties.Add("AttachmentIdentifier", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                            attachmentIdentifier.Value = Guid.NewGuid().ToString();
                            _attachtoEpicMainPage.attachmentControls.AddACategory(x.MailItem);
                            x.MailItem.Save();
                            //var processingcopy = x.MailItem.Copy() as OutlookNS.MailItem;
                            //processingcopy.Move(_attachtoEpicMainPage.attachmentControls.GetCustomFolder(_attachtoEpicMainPage.attachmentControls.ProcessingfolderName));

                        });
                        SaveAllAttachmentInfo();
                        ShowStatus("Attachment will be done shortly");
                        _attachtoEpicMainPage._multipleEmailAttachmentWindow.Close();
                    }
                    else
                    {
                        requiredFieldAlert.Visibility = Visibility.Visible;
                    }

                    //var activeExp = Globals.ThisAddIn.Application.ActiveExplorer();
                    //activeExp?.ClearSelection();
                    _attachtoEpicMainPage.attachmentControls?.StartSearchClients();
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

        private void btnAttachmentAndDeleteOnly_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_attachtoEpicMainPage != null)
                {
                    requiredFieldAlert.Visibility = Visibility.Collapsed;

                    if (ValidateDescField())
                    {
                        //var folder = _attachtoEpicMainPage.attachmentControls.GetCustomFolder(_attachtoEpicMainPage.attachmentControls.ProcessingfolderName);
                        _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection.ForEach(x =>
                        {
                            _attachtoEpicMainPage.attachmentControls.AddACategory(x.MailItem);
                            x.MailItem.Save();
                            x.MailItem.Move(_attachtoEpicMainPage.attachmentControls.OLA.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderDeletedItems));
                            //var deletecopy = x.MailItem.Copy() as OutlookNS.MailItem;
                            //deletecopy.Move(_attachtoEpicMainPage.attachmentControls.OLA.GetNamespace("MAPI").GetDefaultFolder(OutlookNS.OlDefaultFolders.olFolderDeletedItems));
                            // x.MailItem.Move(folder);
                        });

                        SaveAllAttachmentInfo(true);
                        ShowStatus("Attachment will be done shortly");
                        _attachtoEpicMainPage._multipleEmailAttachmentWindow.Close();
                    }
                    else
                    {
                        requiredFieldAlert.Visibility = Visibility.Visible;
                    }
                }
                //var activeExp = Globals.ThisAddIn.Application.ActiveExplorer();
                //activeExp?.ClearSelection();

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
            if (this.Tag != null)
            {
                _attachtoEpicMainPage = this.Tag as AttachToEpicMainPage;
            }
            //LoadDummiesForSelectedItems();
        }
        private void LoadDummiesForSelectedItems()
        {
            try
            {
                List<SelectedEmailInfo> selectedEmailInfo = new List<SelectedEmailInfo>();
                selectedEmailInfo.Add(new SelectedEmailInfo() { From = "Grant Low", Subject = "Verify your Apple ID", MailRecievedDateWithTime = "11/7/2014", Client = "Client 1", Description = "desc 1" });
                selectedEmailInfo.Add(new SelectedEmailInfo() { From = "Glportest", Subject = "Changes in Acronis Access shared folder", MailRecievedDateWithTime = "11/7/2014", Client = "Client 2", Description = "desc 2" });
                selectedEmailInfo.Add(new SelectedEmailInfo() { From = "Your recent download with your Apple ID", Subject = "Verify your Apple ID", MailRecievedDateWithTime = "11/7/2014", Client = "Client 3", Description = "desc 3" });
                SelectedMailList.ItemsSource = selectedEmailInfo;
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

        private void applyToAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var applyToAllButton = sender as Button;
                var contentPresenter = applyToAllButton.TemplatedParent as ContentPresenter;
                var selectedItem = contentPresenter.Content as SelectedEmailInfo;
                if (selectedItem != null)
                {
                    if (_attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection.Count > 0)
                    {
                        foreach (var selectedMail in _attachtoEpicMainPage.attachmentControls.selectedEmailInfoCollection)
                        {
                            selectedMail.TextBoxValue = selectedItem.TextBoxValue;
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

        private void SelectedMailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //_attachtoEpicMainPage.attachmentControls.explorer.ShowPane(Microsoft.Office.Interop.Outlook.OlPane.olPreview, true);
                if (e.AddedItems != null && e.AddedItems.Count > 0)
                {
                    var selectedItem = e.AddedItems[0] as SelectedEmailInfo;
                    if (selectedItem != null)
                    {
                        //selectedItem.ImagePath = "../Asset/email_image_selected.png";
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
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }

        private void txtDesc_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            var txtbx = sender as TextBox;
            if (txtbx != null)
            {
                var item = txtbx.DataContext as SelectedEmailInfo;
                SelectedDescMailList.SelectedItem = item;
            }
        }
    }
}
