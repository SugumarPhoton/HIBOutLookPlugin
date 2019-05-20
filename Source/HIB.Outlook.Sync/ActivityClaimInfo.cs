using System;


namespace HIB.Outlook.Sync
{
    public class ActivityClaimInfo
    {
        #region Public Members
        public int EntityId { get; set; }
        public int ClaimId { get; set; }
        public string ClaimCode { get; set; }
        public string ClaimName { get; set; }
        public Nullable<System.DateTime> LossDate { get; set; }
        public Nullable<System.DateTime> ReportedDate { get; set; }
        public Nullable<int> AgencyClaimNumber { get; set; }
        public string CompanyClaimNumber { get; set; }
        public Nullable<System.DateTime> ClosedDate { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        #endregion
    }
}
