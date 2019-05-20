using System;


namespace HIB.Outlook.Sync
{
    public class ActivityOpportunityInfo
    {
        public int OpportunityId { get; set; }
        public Nullable<int> EntityId { get; set; }
        public string OppDesc { get; set; }
        public Nullable<System.DateTime> TargetedDate { get; set; }
        public string OwnerName { get; set; }
        public string SalesManager { get; set; }
        public string Stage { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
