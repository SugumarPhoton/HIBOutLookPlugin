using System;


namespace HIB.Outlook.Model
{
    public class AttachmentInfo
    {
        /// <summary>
        ///  UploadInfo
        /// </summary>
        /// <param name="UploadId"> UploadId </param>
        /// <param name="ClientId"> ClientId </param>  
        /// <param name="DomainName"> DomainName </param>    
        /// <param name="UserName"> UserName </param>        
        /// <param name="ActivityId"> ActivityId </param>  
        /// <param name="AgencyCode"> AgencyCode </param> 
        /// <param name="AttachedDate"> AttachedDate </param> 
        /// <param name="Description"> Description </param>
        /// <param name="Comments"> Comments </param>
        /// <param name="Subject"> Subject </param>
        /// <param name="ReceivedDate"> ReceivedDate </param>
        /// <param name="EmailAddress"> EmailAddress </param>
        /// <param name="Summary"> Summary </param>
        /// <param name="FolderDetails"> FolderDetails </param>
        /// <param name="FileDetails"> FileDetails </param>
        /// <param name="AttachmentStatus"> AttachmentStatus </param>

        public int AttachmentId { get; set; }
        public long UploadId { get; set; }
        public long ClientId { get; set; }
        public bool IsActiveClient { get; set; }
        public string DomainName { get; set; }
        public string UserName { get; set; }
        public string EmployeeCode { get; set; }
        public long ActivityId { get; set; }
        public bool IsClosedActivity { get; set; }
        public string AgencyCode { get; set; }
        public DateTime AttachedDate { get; set; }
        public string Description { get; set; }
        public string DescriptionFrom { get; set; }
        public string MailBody { get; set; }
        public string Comments { get; set; }
        public string Subject { get; set; }
        public string ReceivedDate { get; set; }
        public string ReceivedDateWithTime { get; set; }
        public string EmailFromAddress { get; set; }
        public string EmailFromDisplayName { get; set; }
        public string EmailToAddress { get; set; }
        public string EmailToDisplayName { get; set; }
        public string EmailCCAddress { get; set; }
        public string EmailCCDisplayName { get; set; }
        public bool IsEmail { get; set; } = true;
        public string Summary { get; set; }
        public FolderInfo FolderDetails { get; set; }
        public FileInfo FileDetails { get; set; }
        public AttachmentStatus Status { get; set; }
        public string PolicyCode { get; set; }
        public string PolicyType { get; set; }
        public string PolicyYear { get; set; }
        public string ClientAccessible { get; set; }
        public bool IsClientAccessible { get; set; }
        public string AttachmentFilePath { get; set; }
        public bool IsPushedToEpic { get; set; }
        public bool IsAttachDelete { get; set; }
        public bool IsDeletedFromZFolder { get; set; }
        public string Identifier { get; set; }
        public string AttachmentMailBody { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ActivityGuid { get; set; }
        public bool IsEpicPushInProgress { get; set; }
        public string AttachmentIdentifier { get; set; }
        public string EntryId { get; set; }
        public string ErrorMessage { get; set; }
        public string DisplayMailBody { get; set; }

    }

    public class AttachmentFileInfo
    {
        public string FileName { get; set; }
        public string FileNameInternal { get; set; }
        public string FileExtension { get; set; }
        public string FileSize { get; set; }
    }
}
