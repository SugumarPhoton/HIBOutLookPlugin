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
    
    public partial class HIBOPActivityEvidence
    {
        public long UniqEvidence { get; set; }
        public long UniqEntity { get; set; }
        public string Title { get; set; }
        public string FormEditionDate { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual HIBOPClient HIBOPClient { get; set; }
    }
}
