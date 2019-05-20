using System;

namespace HIB.Outlook.Model
{
    public class ErrorLogInfo
    {
        #region Public Members
        public int LogId { get; set; }
        public string Source { get; set; }
        public Nullable<int> Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string LoggedBy { get; set; }
        public System.DateTime LogDate { get; set; } 
        #endregion
    }
}
