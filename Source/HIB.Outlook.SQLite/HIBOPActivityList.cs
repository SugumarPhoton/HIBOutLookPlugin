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
    
    public partial class HIBOPActivityList
    {
        public long AId { get; set; }
        public int UniqActivityCode { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string LookupCode { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> IsClosedStatus { get; set; }
    }
}