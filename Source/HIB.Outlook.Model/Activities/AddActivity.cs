using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model.Activities
{
    public class AddActivity
    {

        public Int32 Id { get; set; }

        public Int64? ClientId { get; set; }
        public string ClientLookupCode { get; set; }

        public string AddtoType { get; set; }
        public Int64? AddToTypeId { get; set; }

        public string AddActivityCode { get; set; }
        public string AddActivityDescription { get; set; }
        public string AddActivityDisplayDescription { get; set; }

        public Int32? AddActivityTypeClosedStatus { get; set; }
        public Int64? AddActivityId { get; set; }

        public string OwnerCode { get; set; }
        public string OwnerDecription { get; set; }

        public Int64? PriorityId { get; set; }
        public string Priority { get; set; }

        public Int64? UpdateId { get; set; }
        public string Update { get; set; }

        public string ReminderDate { get; set; }
        public string ReminderTime { get; set; }

        public string StartDate { get; set; }
        public string StartTime { get; set; }

        public string EndDate { get; set; }
        public string EndTime { get; set; }

        public string WhoToContactName { get; set; }
        public Int64? WhoToContactId { get; set; }

        public string ContactMode { get; set; }
        public Int64? ContactModeId { get; set; }

        public string ContactDetail { get; set; }
        public Int64? ContactDetailId { get; set; }

        public string AccessLevel { get; set; }
        public Int64? AccessLevelId { get; set; }

        public string Description { get; set; }

        public string CurrentlyLoggedUserCode { get; set; }

        public string AgencyCode { get; set; }

        public Int32 IsPushToEpic { get; set; }

        public string BranchCode { get; set; }
        public Int32 UniqAgency { get; set; }
        public Int32 UniqBranch { get; set; }
        public Int32 TaskEventEpicId { get; set; }
        public string ActivityGuid { get; set; }
        public string Status { get; set; }

    }

    public class ResultInfo
    {
        public bool IsSuccess { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public dynamic Id { get; set; }
        public string Code { get; set; }
    }

    public class ActivityDetails
    {
        public long TaskEventId { get; set; }
        public Nullable<int> TaskEventEPICId { get; set; }
        public string TaskEventType { get; set; }
        public string TaskEventTypeDesc { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerCode { get; set; }
        public int AssignedToId { get; set; }
        public string AssignedToCode { get; set; }
        public string AssignedToName { get; set; }
        public string Location { get; set; }
        public Nullable<int> StatusLkpId { get; set; }
        public string Status { get; set; }
        public string Subject1 { get; set; }
        public DateTime DueDate { get; set; }
        public Nullable<bool> IsAllDayEvent { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public Nullable<int> TranTypeId { get; set; }
        public string TranTypeName { get; set; }
        public Nullable<long> TranId { get; set; }
        public string TransactionName { get; set; }
        public Nullable<int> ShowTimeAsLkpId { get; set; }
        public string ShowTime { get; set; }
        public Nullable<int> PriorityLkpId { get; set; }
        public string Priority { get; set; }
        public Nullable<int> TypeLkpId { get; set; }
        public string Type { get; set; }
        public Nullable<int> IndustryProjectsLkpId { get; set; }
        public string IndustryProject { get; set; }
        public string Note { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> IsReminder { get; set; }
        public Nullable<System.DateTime> ReminderDateTime { get; set; }
        public string ReminderDate { get; set; }
        public string ReminderTime { get; set; }
        public Nullable<int> ReminderBeforeLkpId { get; set; }
        public string ReminderBefore { get; set; }
        public Nullable<System.DateTime> ReminderBeforeTime { get; set; }
        public Nullable<bool> IsDismiss { get; set; }
        public Nullable<System.DateTime> Snooze { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<int> ProAccId { get; set; }
        public string OrganizationPermissionCode { get; set; }
        public Nullable<int> RecordTypeID { get; set; }
        public string RecordTypeName { get; set; }
        public Nullable<int> AgencyEpicId { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public Nullable<int> BranchEpicId { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public Nullable<int> ClientID { get; set; }
        public string ClientLookupCode { get; set; }
        public Nullable<int> DepartmentEpicId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> ProfitCenterEpicId { get; set; }
        public string ProfitCenterName { get; set; }
        public int SalesTeamEpicID { get; set; }
        public string SalesTeamName { get; set; }
        public string Source { get; set; }
        public Nullable<bool> IsMovedToEpic { get; set; }
        public int? AssociatedToId { get; set; }

    }
}
