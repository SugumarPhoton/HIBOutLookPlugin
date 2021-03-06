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
    
    public partial class HIBOPEmployee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HIBOPEmployee()
        {
            this.HIBOPClientEmployees = new HashSet<HIBOPClientEmployee>();
        }
    
        public string LookupCode { get; set; }
        public Nullable<int> UniqEntity { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public Nullable<System.DateTime> InactiveDate { get; set; }
        public Nullable<int> RoleFlags { get; set; }
        public Nullable<int> Flags { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> SyncStatusNotified { get; set; }
        public Nullable<long> AddinPreference { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HIBOPClientEmployee> HIBOPClientEmployees { get; set; }
    }
}
