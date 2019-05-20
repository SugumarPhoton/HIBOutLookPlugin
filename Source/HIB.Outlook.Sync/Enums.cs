using System.ComponentModel;


namespace HIB.Outlook.Sync
{
    public class Enums
    {

        public enum ServiceMethod
        {
            [Description("SyncClients")]
            Client,
            [Description("SyncActivities")]
            Activity,
            [Description("SyncActivityEmployees")]
            ActivityEmployees,
            [Description("SyncFolders")]
            Folder,
            [Description("GetPolicyLineTypes")]
            PolicyLineType,
            [Description("SaveAuditLogDetails")]
            AuditLog,
            [Description("SaveErrorLogDetails")]
            ErrorLog,
            [Description("SyncActivityClaims")]
            ActivityClaim,
            [Description("SyncActivityPolicies")]
            ActivityPolicy,
            [Description("SyncActivityServices")]
            ActivityService,
            [Description("SyncActivityLines")]
            ActivityLine,
            [Description("SyncActivityOpportunities")]
            ActivityOpportunity,
            [Description("SyncActivityAccounts")]
            ActivityAccount,
            [Description("SyncActivityMarketing")]
            ActivityMarketing,
            [Description("SyncActivityClientContacts")]
            ActivityClientContact,
            [Description("SyncActivityCommonLookUp")]
            ActivityCommonLookUp,
            [Description("SyncActivityOwnerList")]
            ActivityOwnerList,
            [Description("SaveFavouriteDetails")]
            Favourite,
            [Description("SyncActivityList")]
            ActivityList,
            [Description("SyncActivityBills")]
            ActivityBill,
            [Description("SyncActivityCarriers")]
            ActivityCarrierSubmission,
            [Description("SyncActivityTransactions")]
            ActivityTransaction,
            [Description("SyncActivityCertificate")]
            ActivityCertificate,
            [Description("SyncActivityEvidence")]
            ActivityEvidence,
            [Description("SyncActivityLookUps")]
            ActivityLookUp,
            [Description("SyncActivityEmployeeAgencies")]
            ActivityEmployeeAgency,
            [Description("SyncActivityEmployee")]
            ActivityEmployee,
            [Description("SyncClientEmployee")]
            ClientEmployee,
            [Description("activity")]
            Createactivity,
            [Description("attachments/save")]
            CreateAttachment,
            [Description("attachments/upload")]
            UploadAttachment,
            [Description("SyncIntervalTime")]
            SyncIntervalTime,
            [Description("GetDeltaSyncObjectDetail")]
            SyncObjects
        }

        public enum StoredProcedures
        {
            HIBOPGetActivityAccount_SP,
            HIBOPGetActivityBill_SP,
            HIBOPGetActivityCarrierSubmission_SP,
            HIBOPGetActivityCertificate_SP,
            HIBOPGetActivityClaim_SP,
            HIBOPGetActivityClientContacts_SP,
            HIBOPGetActivityDetails_SP,
            HIBOPGetActivityEmployee_SP,
            HIBOPGetActivityEvidence_SP,
            HIBOPGetActivityLine_SP,
            HIBOPGetActivityList_SP,
            HIBOPGetActivityMarketing_SP,
            HIBOPGetActivityOpportunity_SP,
            HIBOPGetActivityOwnerList_SP,
            HIBOPGetActivityPolicy_SP,
            HIBOPGetActivityServices_SP,
            HIBOPGetActivityTransaction_SP,
            HIBOPGetClientDetails_SP,
            HIBOPGetClientEmployee_SP,
            HIBOPGetCommonLookUp_SP,
            HIBOPGetEmployeeAgency_SP,
            HIBOPGetFolders_SP,
            HIBOPGetPolicyLineType_SP
        }


    }
}
