using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CustomControls
{

    public class CustomDatePicker : DatePicker
    {

        public string Watermark { get; set; }

        protected override void OnSelectedDateChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectedDateChanged(e);
            //SetWatermark();
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            SetWatermark();
        }

        private void SetWatermark()
        {
            FieldInfo fiTextBox = typeof(DatePicker).GetField("_textBox", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiTextBox != null)
            {
                DatePickerTextBox dateTextBox = (DatePickerTextBox)fiTextBox.GetValue(this);
                if (dateTextBox != null)
                {
                    dateTextBox.Margin = new System.Windows.Thickness(0, 2, 0, 0);
                    if (string.IsNullOrWhiteSpace(this.Watermark))
                    {
                        this.Watermark = "MM/DD/YYYY";
                    }

                    var partWatermark = dateTextBox.Template.FindName("PART_Watermark", dateTextBox) as ContentControl;
                    if (partWatermark != null)
                    {
                        partWatermark.Foreground = new SolidColorBrush(Colors.Gray);
                        partWatermark.Content = this.Watermark;
                    }
                }
            }
        }

    }

}
