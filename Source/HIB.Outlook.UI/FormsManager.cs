using System;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using AddinExpress.OL;
using HIB.Outlook.UI;
using System.Runtime.InteropServices;
using HIB.Outlook.Helper.Common;
using Microsoft.Office.Interop.Outlook;

namespace AttachmentBridge
{
    public partial class ThisAddIn
    {

        public ADXOlFormsManager FormsManager = null;
        public ADXOlFormsCollectionItem PushToEpicFormRegionItem;

        /// <summary>
        /// Use this event to initialize regions and connect to the events of the ADXOlFormsManager class
        /// </summary>
        private void FormsManager_OnInitialize()
        {
            #region Events Initialization 

            // TODO: See the Class Reference for the complete list of events of the ADXOlFormsManager class
            //this.FormsManager.ADXBeforeFormInstanceCreate
            //    += new ADXOlFormsManager.BeforeFormInstanceCreate_EventHandler(FormsManager_ADXBeforeFormInstanceCreate);
            //this.FormsManager.ADXBeforeFolderSwitchEx
            //    += new ADXOlFormsManager.BeforeFolderSwitchEx_EventHandler(FormsManager_ADXBeforeFolderSwitchEx);
            //this.FormsManager.ADXFolderSwitch
            //    += new ADXOlFormsManager.FolderSwitch_EventHandler(FormsManager_ADXFolderSwitch);
            //this.FormsManager.ADXFolderSwitchEx
            //    += new ADXOlFormsManager.FolderSwitchEx_EventHandler(FormsManager_ADXFolderSwitchEx);
            //this.FormsManager.ADXNewInspector
            //    += new ADXOlFormsManager.NewInspector_EventHandler(FormsManager_ADXNewInspector);
            //this.FormsManager.OnError
            //    += new ADXOlFormsManager.Error_EventHandler(FormsManager_OnError);
            #endregion

            #region PushToEpicFormRegion

            // TODO: Use the PushToEpicFormRegionItem properties to configure the region's location, appearance and behavior.
            // See the "The UI Mechanics" chapter of the Add-in Express Developer's Guide for more information.

            PushToEpicFormRegionItem = new ADXOlFormsCollectionItem
            {
                Cached = ADXOlCachingStrategy.OneInstanceForAllFolders,
                ExplorerLayout = ADXOlExplorerLayout.BottomReadingPane,
                ExplorerItemTypes = ADXOlExplorerItemTypes.olMailItem,
                InspectorLayout = ADXOlInspectorLayout.BottomSubpane,
                InspectorItemTypes = ADXOlInspectorItemTypes.olMail,
                UseOfficeThemeForBackground = true,
                RestoreFromMinimizedState = true,
                FormClassName = typeof(PushToEpicFormRegion).FullName,
                Splitter = ADXOlSplitterBehavior.None
            };
            this.FormsManager.Items.Add(PushToEpicFormRegionItem);
            #endregion

        }

        //#region ADXBeforeFormInstanceCreate
        ///// <summary>
        ///// Use this event to cancel the creation of a form instance.
        ///// </summary>
        ///// <remarks> To prevent a form instance from being shown, you set ADXOlFom.Visible = false 
        ///// in the ADXBeforeFormShow event of the ADXOlForm class. </remarks>
        //private void FormsManager_ADXBeforeFormInstanceCreate(object sender, AddinExpress.OL.BeforeFormInstanceCreateEventArgs args)
        //{

        //}
        //#endregion

        //#region ADXBeforeFolderSwitchEx
        ///// <summary>
        ///// Occurs before an Outlook explorer goes to a new folder,
        ///// either as a result of user action or through program code.
        ///// </summary>
        //private void FormsManager_ADXBeforeFolderSwitchEx(object sender, AddinExpress.OL.BeforeFolderSwitchExEventArgs args)
        //{

        //}
        //#endregion

        //#region ADXFolderSwitch
        ///// <summary>
        ///// Occurs before an Outlook explorer goes to a new folder,
        ///// either as a result of user action or through program code.
        ///// </summary>
        //private void FormsManager_ADXFolderSwitch(object sender, AddinExpress.OL.FolderSwitchEventArgs args)
        //{

        //}
        //#endregion

        //#region ADXFolderSwitchEx
        ///// <summary>
        ///// Occurs when an Outlook explorer goes to a new folder, 
        ///// either as a result of user action or through program code.
        ///// </summary>
        ///// <remarks>
        ///// Set args.ShowForm = False to prevent any ADXOlForm from display. This also prevents 
        ///// the ADXFolderSwitch events from firing.
        ///// <para>To prevent a given form instance from being shown, you set ADXOlFom.Visible = false
        ///// in the ADXBeforeFormShow event of the corresponding ADXOlForm. </para>
        ///// </remarks>
        //private void FormsManager_ADXFolderSwitchEx(object sender, AddinExpress.OL.FolderSwitchExEventArgs args)
        //{

        //}
        //#endregion



        //#region ADXNewInspector
        ///// <summary>
        ///// Occurs whenever a new inspector window is opened,
        ///// either as a result of user action or through program code.
        ///// </summary>
        ////private void FormsManager_ADXNewInspector(object inspectorObj)
        ////{
        ////    if (newInspector1 == null)
        ////    {

        ////        newInspector1 = inspectorObj as Inspector;
        ////        ((InspectorEvents_10_Event)newInspector1).Activate -=
        ////         new InspectorEvents_10_ActivateEventHandler(
        ////         Inspector1_Activate);
        ////        ((InspectorEvents_10_Event)newInspector1).Activate +=
        ////         new InspectorEvents_10_ActivateEventHandler(
        ////         Inspector1_Activate);

        ////    }
        ////}

        //public ADXOlForm FindCurrentForm(ADXOlFormsCollectionItem item)
        //{
        //    IntPtr hwndActive = OfficeWin32Window.GetActiveWindow();
        //    for (int i = 0; i <= item.FormInstanceCount - 1; i++)
        //    {
        //        ADXOlForm form = item.FormInstances(i);
        //        if (form.Visible && form.Active)
        //        {
        //            // Active form in form region has Visible = true , Active = true
        //            // Inactive form in form region has Visible = true , Active = false
        //            if ((hwndActive == form.Handle) || (hwndActive == form.CurrentOutlookWindowHandle))
        //                return form;
        //        }
        //    }
        //    return null/* TODO Change to default(_) if this is not a reference type */;
        //}
        //#endregion

        //#region OnError
        ///// <summary>
        ///// Occurs when ADXOlFormaManager generates an exception.
        ///// </summary>
        //private void FormsManager_OnError(object sender, AddinExpress.OL.ErrorEventArgs args)
        //{

        //}
        //#endregion

        #region RequestService
        /// <summary>
        /// Required method for DockRight, DockLeft, DockTop and DockBottom layout support.
        /// </summary>
        protected override object RequestService(Guid serviceGuid)
        {
            if (serviceGuid == typeof(Office.ICustomTaskPaneConsumer).GUID)
            {
                return AddinExpress.OL.CTPFactoryGettingTaskPane.Instance;
            }
            return base.RequestService(serviceGuid);
        }
        #endregion
    }
}
