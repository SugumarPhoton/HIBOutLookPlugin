using System;
using System.ComponentModel;

namespace HIB.Outlook.Model
{
    public class ActivityTransactionInfo
    {
        #region Public Members
        [DisplayName("No display")]
        public int TransactionId { get; set; }
        [DisplayName("No display")]
        public Nullable<int> TransheadId { get; set; }
        [DisplayName("No display")]
        public Nullable<int> EntityId { get; set; }

        [DisplayName("Invoice #")]
        public Nullable<int> InvoiceNumber { get; set; }
        
        public string Code { get; set; }

        [DisplayName("Description")]
        public string DescriptionOf { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Balance { get; set; }
        [DisplayName("No display")]
        public Nullable<System.DateTime> InsertedDate { get; set; }
        [DisplayName("No display")]
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        [DisplayName("No display")]
        public string UserLookupCode { get; set; }
        #endregion
    }
}
