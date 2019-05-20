using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using OutlookNS = Microsoft.Office.Interop.Outlook;


namespace HIB.Outlook.Common
{
    public class SelectedEmailInfo : INotifyPropertyChanged
    {
        public string Identifier { get; set; }

        public OutlookNS.MailItem MailItem { get; set; }

        public string From { get; set; }

        public string _imagePath = "../Asset/email_image.png";
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                _imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        public string MailRecievedDateWithTime { get; set; }
        public string RecievedDate { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string PolicyYear { get; set; }

        public string PolicyType { get; set; }

        public string EntryId { get; set; }

        public bool IsApplyToAllNeedToBeVisible { get; set; }

        public string ActivityDesc { get; set; }

        public string _textBoxValue = string.Empty;
        public string TextBoxValue
        {
            get
            {
                return _textBoxValue;
            }
            set
            {
                _textBoxValue = value;
                OnPropertyChanged("TextBoxValue");
            }
        }

        public Brush _borderBrushValue = Brushes.LightGray;
        public Brush BorderBrushValue
        {
            get
            {
                return _borderBrushValue;
            }
            set
            {
                _borderBrushValue = value;
            }
        }
        public string To { get; set; }
        public string Cc { get; set; }
        public string HtmlBody { get; set; }
        public string ErrorMessage { get; set; }
        public string Client { get; set; }
        public string ClientEpicCode { get; set; }
        public string Activity { get; set; }
        public int AttachmentId { get; set; }

        public bool IsChecked { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
