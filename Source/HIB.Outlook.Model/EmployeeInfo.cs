using System;


namespace HIB.Outlook.Model
{
    public class EmployeeInfo
    {
        public int EntityId { get; set; }
        public string LookupCode { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public Nullable<System.DateTime> InactiveDate { get; set; }
        public Nullable<int> RoleFlags { get; set; }
        public Nullable<int> Flags { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Int32 Status { get; set; }
        public Int32 SyncStatusNotified { get; set; }
        public Int32 CompletedSyncStatusNotified { get; set; }
        public int IsAdmin { get; set; }
    }
}
