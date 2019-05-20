using AttachmentBridge;
using CustomControls.Controls;
using OutlookAddIn1.Properties;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Effects;
using OutlookNS = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using HIB.Outlook.Helper.Common;
using System.Linq;
using HIB.Outlook.SQLite;
using System.Threading.Tasks;
using System.Windows.Media;
// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new AttachToEpic();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace OutlookAddIn1.UserControls
{
    [ComVisible(true)]
    public class AttachToEpic : Office.IRibbonExtensibility
    {

        internal Office.IRibbonUI ribbon;
        internal Window multipleEmailAttachment = null;
        internal AttachToEpicMainPage attachToEpicMainPage = null;

        public AttachToEpic()
        {

        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("OutlookAddIn1.UserControls.AttachToEpic.xml");
        }

        public System.Drawing.Image GetIcon(Office.IRibbonControl control)
        {
            return Resources.attach_to_epic;
        }

        public string GetSynchronisationLabel(Office.IRibbonControl control)
        {
            return "Attach to Epic";
        }
        public void OnMyButtonClick(Office.IRibbonControl control)
        {

        }



        public bool Control_Enable(Office.IRibbonControl control)
        {
            try
            {
                if (GetSelectedMailItemCount() > 1 && ThisAddIn.IsValidEmployee)
                {
                    this.ribbon.Invalidate();
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
            return false;

        }


        private int GetSelectedMailItemCount()
        {
            var selectedMailCount = 0;
            try
            {
                var application = Globals.ThisAddIn.Application;
                var explorer = application.ActiveExplorer();
                var items = explorer?.Selection;
                for (int i = 1; i < items.Count + 1; i++)
                {
                    if (items[i] is OutlookNS.MailItem)
                    {
                        selectedMailCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            return selectedMailCount;
        }


        public bool Control_Visible(Office.IRibbonControl control)
        {
            try
            {

                if (ThisAddIn.IsValidEmployee)
                {
                    this.ribbon.InvalidateControlMso("ContextMenuMultipleItems");
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
            return false;
        }

        //public void get_Pressed(Office.IRibbonControl control, bool pressed)
        //{
        //    this.ribbon.Invalidate();
        //    return Globals.ThisAddIn.IsChecked;
        //}
        public bool get_Pressed(Office.IRibbonControl control)
        {
            return Globals.ThisAddIn.IsChecked;
        }
        public void ToggleButtonOnAction(Office.IRibbonControl control, bool pressed)
        {
            try
            {
                if (Globals.ThisAddIn?.FormsManager?.Items?.Count > 0)
                {
                    var formRegions = Globals.ThisAddIn.FormsManager.Items[0];
                    if (formRegions != null)
                    {
                        Task.Run(() =>
                        {
                            string squery = string.Format("Update HIBOPEmployee set AddinPreference = '{0}' where LookupCode = '{1}'", Convert.ToInt32(pressed), ThisAddIn.EmployeeLookupCode);
                            var result = SQLiteHandler.ExecuteCreateOrInsertQuery(squery);
                        });
                        formRegions.GetCurrentForm().Visible = pressed;
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


        public void AttachToEpicForMultipleEmails(Office.IRibbonControl control)
        {
            //var application = Globals.ThisAddIn.Application;
            //var explorer = application.ActiveExplorer();

            PrepareAttachToEpicWindow();
            multipleEmailAttachment.ShowDialog();


            //var count = explorer?.Selection?.Count;
            ////var context = control.Context as sys;
            ////if (context != null)
            ////{
            ////    var count = context.Selection?.Count;
            ////}
            ////Application application = context.Application;
            //MessageBox.Show("You've clicked the synchronize context menu item", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PrepareAttachToEpicWindow()
        {
            try
            {
                multipleEmailAttachment = new Window();
                dynamic activeWindow = Globals.ThisAddIn.Application.ActiveWindow();
                
                using (var officeWin32activeWindow = new HIB.Outlook.UI.OfficeWin32Window(activeWindow))
                {
                    IntPtr outlookHwnd = officeWin32activeWindow.Handle;
                    WindowInteropHelper wih = new WindowInteropHelper(multipleEmailAttachment)
                    {
                        Owner = outlookHwnd
                    };
                }
                
                multipleEmailAttachment.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                multipleEmailAttachment.AllowsTransparency = false;
                multipleEmailAttachment.WindowStyle = WindowStyle.None;
                multipleEmailAttachment.ResizeMode = ResizeMode.NoResize;// "NoResize";
                                                                         //  multipleEmailAttachment.AllowsTransparency = true;
                multipleEmailAttachment.Background = System.Windows.Media.Brushes.Transparent;
                attachToEpicMainPage = new AttachToEpicMainPage(multipleEmailAttachment);
                Grid mainGrid = new Grid();
                mainGrid.Background = Brushes.White;
                mainGrid.Children.Add(attachToEpicMainPage);
                mainGrid.Effect = new DropShadowEffect
                {
                    Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#0072C6"),
                    Direction = 320,
                    ShadowDepth = 0,
                    Opacity = 1
                };
                multipleEmailAttachment.Content = mainGrid;
                multipleEmailAttachment.Height = 620;
                multipleEmailAttachment.Width = 1005;
                

                double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                double windowWidth = multipleEmailAttachment.Width;
                double windowHeight = multipleEmailAttachment.Height;
                multipleEmailAttachment.Left = (screenWidth / 2) - (windowWidth / 2);
                multipleEmailAttachment.Top = (screenHeight / 3.1) - (windowHeight / 3.1);
                

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
        }        

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit http://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }


        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }

}
