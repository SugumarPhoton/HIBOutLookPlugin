using System;

namespace HIB.Outlook.Model
{
    
    public class LogInfo
    {
        #region Public Members

        public int LogId { get; set; }
        public string Id { get; set; }
        public int EntityId { get; set; }
        public string EmployeeId { get; set; }
        public string ClientLookupCode { get; set; }
        public string ClientName { get; set; }
        public string PrimaryContactName { get; set; }
        public int ActivityId { get; set; }
        public string ActivityCode { get; set; }
        public string DescriptionOf { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyYear { get; set; }
        public string PolicyType { get; set; }
        public string DescriptionType { get; set; }
        public string Description { get; set; }
        public Nullable<int> FolderId { get; set; }
        public string FolderName { get; set; }
        public Nullable<int> SubFolder1Id { get; set; }
        public string FolderName1 { get; set; }
        public Nullable<int> SubFolder2Id { get; set; }
        public string FolderName2 { get; set; }
        public string ClientAccessibleDate { get; set; }
        public string EmailAction { get; set; }
        public int Version { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ActivityDescription { get; set; }
        public string EmailSubject { get; set; }
        public Int64? Status { get; set; }
        public string Message { get; set; }
        #endregion
    }
}
