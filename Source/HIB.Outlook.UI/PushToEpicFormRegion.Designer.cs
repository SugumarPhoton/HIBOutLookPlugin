using HIB.Outlook.Helper.Common;
using System.Linq;
namespace AttachmentBridge
{
    partial class PushToEpicFormRegion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PushToEpicFormRegion));
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.attachmentControls1 = new HIB.Outlook.UI.AttachmentControls();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(740, 310);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.attachmentControls1;
            // 
            // PushToEpicFormRegion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScrollMinSize = new System.Drawing.Size(740, 0);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(0, 310);
            this.Controls.Add(this.elementHost1);
            this.ADXBeforeFormShow += PushToEpicFormRegion_ADXBeforeFormShow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "PushToEpicFormRegion";
            this.Text = "Push to Epic";
            this.ResumeLayout(false);

        }



        private void PushToEpicFormRegion_ADXBeforeFormShow()
        {
            try
            {
                using (var context = new HIB.Outlook.SQLite.HIBOutlookEntities())
                {
                    var result = context.HIBOPEmployees?.Where(d => d.LookupCode == ThisAddIn.EmployeeLookupCode && d.Status == 1)?.FirstOrDefault();
                    if (result != null)
                    {
                        var formRegions = Globals.ThisAddIn.FormsManager?.Items[0];
                        if (formRegions != null)
                        {
                            if (result.AddinPreference == 0)
                            {
                                if (formRegions.GetCurrentForm() != null)
                                    formRegions.GetCurrentForm().Visible = false;
                                Globals.ThisAddIn.IsChecked = false;
                            }
                            else
                            {
                                if (formRegions.GetCurrentForm() != null)
                                    formRegions.GetCurrentForm().Visible = true;
                                Globals.ThisAddIn.IsChecked = true;
                            }

                            if (Globals.ThisAddIn.attachToEpic != null)
                            {
                                Globals.ThisAddIn.attachToEpic?.ribbon?.InvalidateControl("btnVisible");
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
            }
            finally
            {
                Logger.save();
            }
        }
        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        public HIB.Outlook.UI.AttachmentControls attachmentControls1;
    }
}

