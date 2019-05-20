using System;


namespace HIB.Outlook.Model
{
    public class PolicyTypeInfo
    {
        #region Public Members

        public string PolicyLineTypeCode { get; set; }
        public string PolicyLineTypeDesc { get; set; }
        public Nullable<int> PolicyLineTypeId { get; set; }
        public System.DateTime InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        #endregion
    }
}
