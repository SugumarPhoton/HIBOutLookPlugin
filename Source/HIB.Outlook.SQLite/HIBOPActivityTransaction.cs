//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HIB.Outlook.SQLite
{
    using System;
    using System.Collections.Generic;
    
    public partial class HIBOPActivityTransaction
    {
        public int TransactionId { get; set; }
        public Nullable<int> UniqTranshead { get; set; }
        public string Code { get; set; }
        public string DescriptionOf { get; set; }
        public long UniqEntity { get; set; }
        public Nullable<int> InvoiceNumber { get; set; }
        public Nullable<int> ItemNumber { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual HIBOPClient HIBOPClient { get; set; }
    }
}
