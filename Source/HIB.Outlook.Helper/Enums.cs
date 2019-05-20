using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Helper
{
    public class Enums
    {

        public enum ServiceMethod
        {
            [Description("SyncClients")]
            Client,
            [Description("SyncActivities")]
            Activity,
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
        }


    }
}
