using System;


namespace HIB.Outlook.Sync
{
    public class ClientInfo
    {

        #region Public Members

        public int EnitityId { get; set; }
        public string LookupCode { get; set; }
        public string Nameof { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public string PrimaryContactName { get; set; }
        public string Status { get; set; }
        public System.DateTime InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        #endregion

    }
}
