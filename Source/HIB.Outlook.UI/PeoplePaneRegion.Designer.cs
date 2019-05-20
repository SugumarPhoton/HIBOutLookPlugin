using System;
using AttachmentBridge;
using Microsoft.Office.Interop.Outlook;
using OutlookAddIn1;
using OutlookNS = Microsoft.Office.Tools.Outlook;
using System.Linq;
namespace HIB.Outlook.UI
{
    [System.ComponentModel.ToolboxItemAttribute(false)]
    partial class PeoplePaneRegion : Microsoft.Office.Tools.Outlook.FormRegionBase
    {
        public PeoplePaneRegion(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            : base(Globals.Factory, formRegion)
        {
            this.InitializeComponent();


        }






        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.attachmentControls1 = new HIB.Outlook.UI.AttachmentControls();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Margin = new System.Windows.Forms.Padding(2);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(800, 310);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.attachmentControls1;
            //   this.elementHost1.AutoSize = true;
            //  this.OutlookFormRegion.Reflow();
            // 
            // PeoplePaneRegion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(800, 0);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.elementHost1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PeoplePaneRegion";
            this.Size = new System.Drawing.Size(0, 310);
            this.FormRegionShowing += PeoplePaneRegion_FormRegionShowing;
            this.ResumeLayout(false);
        }

        private void PeoplePaneRegion_FormRegionShowing(object sender, EventArgs e)
        {
            var currentRegion = sender as Microsoft.Office.Tools.Outlook.FormRegionControl;

            using (var context = new HIB.Outlook.SQLite.HIBOutlookEntities())
            {
                var result = context.HIBOPEmployees.Where(d => d.LookupCode == ThisAddIn.EmployeeLookupCode && d.Status == 1)?.FirstOrDefault();
                if (result != null)
                {
                    if (result.AddinPreference == 0)
                    {
                        currentRegion.OutlookFormRegion.Visible = false;
                        Globals.ThisAddIn.IsChecked = false;
                    }
                    else
                    {
                        currentRegion.OutlookFormRegion.Visible = true;
                        Globals.ThisAddIn.IsChecked = true;
                    }
                    if (Globals.ThisAddIn.attachToEpic != null)
                    {
                        Globals.ThisAddIn.attachToEpic.ribbon.InvalidateControl("btnVisible");
                    }
                }
            }
        }


        #endregion

        #region Form Region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private static void InitializeManifest(Microsoft.Office.Tools.Outlook.FormRegionManifest manifest, Microsoft.Office.Tools.Outlook.Factory factory)
        {
            manifest.FormRegionName = "Attach to EPIC";
            manifest.FormRegionType = Microsoft.Office.Tools.Outlook.FormRegionType.Adjoining;
            manifest.Hidden = true;

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private AttachmentControls attachmentControls1;


        public partial class PeoplePaneRegionFactory : Microsoft.Office.Tools.Outlook.IFormRegionFactory
        {
            public event Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler FormRegionInitializing;

            private Microsoft.Office.Tools.Outlook.FormRegionManifest _Manifest;

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public PeoplePaneRegionFactory()
            {
                this._Manifest = Globals.Factory.CreateFormRegionManifest();
                PeoplePaneRegion.InitializeManifest(this._Manifest, Globals.Factory);
                this.FormRegionInitializing += new Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler(this.PeoplePaneRegionFactory_FormRegionInitializing);

            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public Microsoft.Office.Tools.Outlook.FormRegionManifest Manifest
            {
                get
                {
                    return this._Manifest;
                }
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            Microsoft.Office.Tools.Outlook.IFormRegion Microsoft.Office.Tools.Outlook.IFormRegionFactory.CreateFormRegion(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            {

                PeoplePaneRegion form = new PeoplePaneRegion(formRegion);
                form.Factory = this;
                return form;
            }


            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            byte[] Microsoft.Office.Tools.Outlook.IFormRegionFactory.GetFormRegionStorage(object outlookItem, Microsoft.Office.Interop.Outlook.OlFormRegionMode formRegionMode, Microsoft.Office.Interop.Outlook.OlFormRegionSize formRegionSize)
            {
                throw new System.NotSupportedException();
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            bool Microsoft.Office.Tools.Outlook.IFormRegionFactory.IsDisplayedForItem(object outlookItem, Microsoft.Office.Interop.Outlook.OlFormRegionMode formRegionMode, Microsoft.Office.Interop.Outlook.OlFormRegionSize formRegionSize)
            {
                if (this.FormRegionInitializing != null)
                {
                    Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs cancelArgs = Globals.Factory.CreateFormRegionInitializingEventArgs(outlookItem, formRegionMode, formRegionSize, false);
                    this.FormRegionInitializing(this, cancelArgs);
                    return !cancelArgs.Cancel;
                }
                else
                {
                    return true;
                }
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            Microsoft.Office.Tools.Outlook.FormRegionKindConstants Microsoft.Office.Tools.Outlook.IFormRegionFactory.Kind
            {
                get
                {
                    return Microsoft.Office.Tools.Outlook.FormRegionKindConstants.WindowsForms;
                }
            }
        }
    }

    public partial class WindowFormRegionCollection
    {
        //internal PeoplePaneRegion PeoplePaneRegion
        //{
        //    get
        //    {
        //        foreach (var item in this)
        //        {
        //            if (item.GetType() == typeof(PeoplePaneRegion))
        //                return (PeoplePaneRegion)item;
        //        }
        //        return null;
        //    }
        //}
    }
}
