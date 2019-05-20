﻿using System;

namespace HIB.Outlook.Model
{
    public class ActivityServiceInfo
    {
        #region Public Members
        public Nullable<int> ServiceHeadId { get; set; }
        public Nullable<int> EntityId { get; set; }
        public Nullable<short> ServiceNumber { get; set; }
        public string ServiceCodeId { get; set; }
        public string Description { get; set; }
        public string ContractNumber { get; set; }
        public Nullable<System.DateTime> InceptionDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UserLookupCode { get; set; }
        public int? Flags { get; set; }
        #endregion
    }
}
