using System;


namespace HIB.Outlook.Model
{
    public class FavouriteInfo
    {

        #region Public Members

        public Int32 FavId { get; set; }

        public string FavourtieName { get; set; }

        public string UniqEmployee { get; set; }

        public long UniqEntity { get; set; }

        public Int32 IsActiveClient { get; set; }
        public long UniqActivity { get; set; }
        public Int32 IsClosedActivity { get; set; }

        public string PolicyYear { get; set; }

        public string PolicyType { get; set; }

        public string DescriptionType { get; set; }

        public string Description { get; set; }

        public long FolderId { get; set; }

        public long SubFolder1Id { get; set; }

        public long SubFolder2Id { get; set; }

        public string ClientAccessibleDate { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string ModifiedDate { get; set; }

        public string ActivityGuid { get; set; }

        public string IPAddress { get; set; }

        public string UserLookupCode { get; set; }

        #endregion

    }
}
