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
    
    public partial class HIBOPSyncLog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HIBOPSyncLog()
        {
            this.HIBOPSyncLogUserDetails = new HashSet<HIBOPSyncLogUserDetail>();
        }
    
        public long SLId { get; set; }
        public string Fields { get; set; }
        public Nullable<System.DateTime> SyncDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HIBOPSyncLogUserDetail> HIBOPSyncLogUserDetails { get; set; }
    }
}
