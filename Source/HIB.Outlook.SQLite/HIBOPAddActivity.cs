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
    
    public partial class HIBOPAddActivity
    {
        public long Id { get; set; }
        public long UniqEntity { get; set; }
        public string AddToType { get; set; }
        public Nullable<long> AddToTypeId { get; set; }
        public string AddActivityCode { get; set; }
        public string AddActivityDescription { get; set; }
        public Nullable<long> AddActivityId { get; set; }
        public string OwnerCode { get; set; }
        public string OwnerDecription { get; set; }
        public Nullable<long> PriorityId { get; set; }
        public string Priority { get; set; }
        public Nullable<long> UpdateId { get; set; }
        public string Update { get; set; }
        public string ReminderDate { get; set; }
        public string ReminderTime { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public string WhoToContactName { get; set; }
        public Nullable<long> WhoToContactId { get; set; }
        public string ContactMode { get; set; }
        public Nullable<long> ContactModeId { get; set; }
        public string ContactDetail { get; set; }
        public Nullable<long> ContactDetailId { get; set; }
        public string AccessLevel { get; set; }
        public Nullable<long> AccessLevelId { get; set; }
        public string Description { get; set; }
        public Nullable<long> IsPushedToEpic { get; set; }
        public string ClientLookupCode { get; set; }
        public string CurrentlyLoggedLookupCode { get; set; }
        public Nullable<long> TaskEventEpicId { get; set; }
        public string ActivityGuid { get; set; }
        public string Status { get; set; }
        public string AddActivityDisplayDescription { get; set; }
        public Nullable<long> IsEpicPushInProgress { get; set; }
    }
}