using System;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.DAL;
using System.Linq;
using HIB.Outlook.Model;
using System.Collections.Generic;
using log4net;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace HIB.Outlook.BAL.Repository
{
    public class ActivityRepository : IActivityRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ActivityRepository));
        private readonly Int32 commandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
        /// <summary>
        /// Get list of activity detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public ActivityDetail SyncActivities(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {

            var activityDetail = new ActivityDetail();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var activityInfo = new List<ActivityInfo>();
                    ObjectParameter outputRowCount = new ObjectParameter("RowCount", typeof(long));
                    var result = context.HIBOPGetActivityDetails_SP(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber, outputRowCount).ToList();
                    activityInfo.AddRange(result.Select(a => new ActivityInfo { ActivityId = a.UniqActivity, ActivityCodeId = a.UniqActivityCode, ActivityCode = a.ActivityCode, DescriptionOf = a.DescriptionOf, PolicyNumber = a.PolicyNumber, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, Status = a.Status, CdPolicyLineTypeCode = a.CdPolicyLineTypeCode, EntityId = Convert.ToInt32(a.UniqEntity), EffectiveDate = a.EffectiveDate, ExpirationDate = a.ExpirationDate, AgencyId = a.UniqAgency, BranchId = a.UniqBranch, DepartmentId = a.UniqDepartment, ProfitCenterId = a.UniqProfitCenter, AssociatedItemId = a.UniqAssociatedItem, AssociationType = a.AssociationType, CdPolicyLineTypeId = a.UniqCdPolicyLineType, OwnerName = a.OwnerName, ClosedDate = a.ClosedDate, UniqPolicy = a.UniqPolicy, UniqLine = a.UniqLine, UniqClaim = a.UniqClaim, LossDate = a.LossDate, Policydescription = a.Policydescription, LineCode = a.LineCode, LineDescription = a.LineDescription, ICO = a.ICO, LineEffectiveDate = a.LineEffectiveDate, LineExpirationDate = a.LineExpirationDate }));
                    activityDetail.Activity = activityInfo;
                    activityDetail.RowCount = Convert.ToInt64(outputRowCount.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityDetail;
        }

        /// <summary>
        /// Get list of activity employee detail to sync local database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public ActivityEmployeeDetail SyncActivityEmployees(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            var activityEmployeeDetail = new ActivityEmployeeDetail();
            try
            {

                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var clientEmployee = new List<ActivityEmployee>();
                    ObjectParameter outputRowCount = new ObjectParameter("RowCount", typeof(long));
                    var result = context.HIBOPGetActivityEmployee_SP(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber, outputRowCount).ToList();
                    clientEmployee.AddRange(result.Select(a => new ActivityEmployee { UniqEmployee = a.UniqEmployee, EmployeeLookupcode = a.EmployeeLookupcode, UniqEntity = a.UniqEntity, UniqActivity = a.UniqActivity }));
                    activityEmployeeDetail.ActivityEmployees = clientEmployee;
                    activityEmployeeDetail.RowCount = Convert.ToInt64(outputRowCount.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityEmployeeDetail;
        }

        /// <summary>
        /// Get list of activity Claim detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ActivityClaimInfo> SyncActivityClaims(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityClaims = new List<ActivityClaimInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityClaim_SP(userId, lastSyncDate, ipAddress).ToList();
                    activityClaims.AddRange(result.Select(a => new ActivityClaimInfo { ClaimId = a.UniqClaim, EntityId = a.UniqEntity, ClaimCode = a.ClaimCode, ClaimName = a.ClaimName, LossDate = a.LossDate, ReportedDate = a.ReportedDate, AgencyClaimNumber = a.AgencyClaimNumber, CompanyClaimNumber = a.CompanyClaimNumber, ClosedDate = a.ClosedDate, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, Flags = a.Flags }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityClaims;
        }

        /// <summary>
        /// Get list of activity policy detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public ActivityPolicyDetail SyncActivityPolicies(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            var activityPolicydetail = new ActivityPolicyDetail();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var activityPolicies = new List<ActivityPolicyInfo>();
                    ObjectParameter outputRowCount = new ObjectParameter("RowCount", typeof(long));
                    var result = context.HIBOPGetActivityPolicy_SP(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber, outputRowCount).ToList();
                    activityPolicies.AddRange(result.Select(a => new ActivityPolicyInfo { EmployeeLookUpCode = a.EmployeeLookUpCode, EntityId = Convert.ToInt32(a.UniqEntity), PolicyLineTypeCode = a.CdPolicyLineTypeCode, PolicyId = Convert.ToInt32(a.UniqPolicy), PolicyNumber = a.PolicyNumber, DescriptionOf = a.DescriptionOf, EffectiveDate = a.EffectiveDate, ExpirationDate = a.ExpirationDate, PolicyStatus = a.PolicyStatus, Status = a.Status, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, Flags = a.Flags }));
                    activityPolicydetail.ActivityPolicies = activityPolicies;
                    activityPolicydetail.RowCount = Convert.ToInt64(outputRowCount.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityPolicydetail;
        }
        private void ExecuteStoreProcedureUsingSQL(string storedProcedure, string userId, DateTime? lastSyncDate, int? rowsPerPage, int? PageNumber)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter adapter;
                // long rowCount = 0;
                var connectionString = ConfigurationManager.AppSettings["HIBOutlook"];
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand(storedProcedure, conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    var lastSyncDateValue = string.Empty;
                    if (lastSyncDate != null)
                    {
                        lastSyncDateValue = lastSyncDate?.ToString();
                    }
                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@User", userId);
                    cmd.Parameters.AddWithValue("@LastSyncDate", lastSyncDateValue);
                    cmd.Parameters.AddWithValue("@RowsPerPage", rowsPerPage);
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    cmd.Parameters.Add("@RowCount", SqlDbType.BigInt);
                    cmd.Parameters["@RowCount"].Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("@FruitName", SqlDbType.VarChar, 30);

                    adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);

                    //var result = cmd.ExecuteNonQuery();
                    //// execute the command
                    //using (SqlDataReader rdr = cmd.ExecuteReader())
                    //{
                    //    // iterate through results, printing each to console
                    //    while (rdr.Read())
                    //    {
                    //        Console.WriteLine("Product: {0,-35} Total: {1,2}", rdr["ProductName"], rdr["Total"]);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        /// <summary>
        /// Get list of activity service detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        public List<ActivityServiceInfo> SyncActivityServices(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityServices = new List<ActivityServiceInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityServices_SP(userId, lastSyncDate, ipAddress).ToList();
                    activityServices.AddRange(result.Select(a => new ActivityServiceInfo { ServiceHeadId = a.UniqServiceHead, EntityId = a.UniqEntity, ServiceNumber = a.ServiceNumber, ServiceCodeId = a.UniqCdServiceCode, Description = a.Description, ContractNumber = a.ContractNumber, InceptionDate = a.InceptionDate, ExpirationDate = a.ExpirationDate, Status = a.Status, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, Flags = a.Flags }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityServices;
        }

        /// <summary>
        /// Get list of activity line detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        public ActivityLineDetail SyncActivityLines(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            var activityLinedetail = new ActivityLineDetail();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var activityLines = new List<ActivityLineInfo>();
                    ObjectParameter outputRowCount = new ObjectParameter("RowCount", typeof(long));
                    var result = context.HIBOPGetActivityLine_SP(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber, outputRowCount).ToList();
                    activityLines.AddRange(result.Select(a => new ActivityLineInfo { LineId = a.UniqLine, PolicyId = a.UniqPolicy, EntityId = a.UniqEntity, PolicyType = a.PolicyType, PolicyDesc = a.PolicyDesc, LineCode = a.LineCode, LineOfBusiness = a.LineOfBusiness, LineStatus = a.LineStatus, PolicyNumber = a.PolicyNumber, PolicyLineTypeId = a.UniqCdPolicyLineType, LineStatusId = a.UniqCdLineStatus, BillModeCode = a.BillModeCode, ExpirationDate = a.ExpirationDate, EffectiveDate = a.EffectiveDate, IOC = a.IOC, Status = a.Status, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, Flags = a.Flags }));
                    activityLinedetail.ActivityLines = activityLines;
                    activityLinedetail.RowCount = Convert.ToInt64(outputRowCount.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityLinedetail;
        }

        /// <summary>
        /// Get list of activity Opportunity detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ActivityOpportunityInfo> SyncActivityOpportunities(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityOpportunities = new List<ActivityOpportunityInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityOpportunity_SP(userId, lastSyncDate, ipAddress).ToList();
                    activityOpportunities.AddRange(result.Select(a => new ActivityOpportunityInfo { OpportunityId = a.UniqOpportunity, EntityId = a.UniqEntity, OppDesc = a.OppDesc, TargetedDate = a.TargetedDate, OwnerName = a.OwnerName, SalesManager = a.SalesManager, Stage = a.Stage, Status = a.Status, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, Flags = a.Flags }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityOpportunities;
        }

        /// <summary>
        /// Get list of activity Opportunity detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public ActivityAccountDetail SyncActivityAccounts(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            var activityAccountDetail = new ActivityAccountDetail();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var activityAccounts = new List<ActivityAccountInfo>();
                    ObjectParameter outputRowCount = new ObjectParameter("RowCount", typeof(long));
                    var result = context.HIBOPGetActivityAccount_SP(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber, outputRowCount).ToList();
                    activityAccounts.AddRange(result.Select(a => new ActivityAccountInfo { RowNumber = a.AccountId, EntityId = Convert.ToInt32(a.UniqEntity), AgencyId = Convert.ToInt32(a.UniqAgency), AgencyCode = a.AgencyCode, AgencyName = a.AgencyName, BranchId = Convert.ToInt32(a.UniqBranch), BranchCode = a.BranchCode, BranchName = a.BranchName, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate }));
                    activityAccountDetail.ActivityAccounts = activityAccounts;
                    activityAccountDetail.RowCount = Convert.ToInt64(outputRowCount.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityAccountDetail;
        }


        /// <summary>
        /// Get list of activity Marketing detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ActivityMarketingInfo> SyncActivityMarketing(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            List<ActivityMarketingInfo> activityMarketing = new List<ActivityMarketingInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    object[] activityMarketingparameters = { userId, lastSyncDate };
                    var result = context.HIBOPGetActivityMarketing_SP(userId, lastSyncDate, ipAddress).ToList();

                    activityMarketing.AddRange(result.Select(a => new ActivityMarketingInfo
                    {
                        EffectiveDate = a.EffectiveDate,
                        ExpirationDate = a.ExpirationDate,
                        InsertedDate = a.InsertedDate,
                        LastSubmittedDate = a.LastSubmittedDate,
                        LineOfBusiness = a.LineOfBusiness,
                        MarketingSubbmission = a.MarketingSubbmission,
                        Status = a.Status,
                        UniqAgency = a.UniqAgency,
                        UniqBranch = a.UniqBranch,
                        UniqEntity = a.UniqEntity,
                        UniqMarketingSubmission = a.UniqMarketingSubmission,
                        UpdatedDate = a.UpdatedDate,
                        Flags = a.Flags
                    }));

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityMarketing;
        }

        /// <summary>
        /// Get list of activity client contact detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ActivityClientContactInfo> SyncActivityClientContacts(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityClientContacts = new List<ActivityClientContactInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityClientContacts_SP(userId, lastSyncDate, ipAddress).ToList();
                    activityClientContacts.AddRange(result.Select(a => new ActivityClientContactInfo { ClientContactId = a.ClientContactId, ContactNumberId = a.UniqContactNumber, EntityId = a.UniqEntity, ContactNameId = a.UniqContactName, ContactName = a.ContactName, ContactType = a.ContactType, ContactValue = a.ContactValue, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityClientContacts;
        }

        /// <summary>
        /// Get list of activity common look up detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ActivityCommonLookUpInfo> SyncActivityCommonLookUp(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityCommonLookUp = new List<ActivityCommonLookUpInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetCommonLookUp_SP(userId, lastSyncDate, ipAddress).ToList();
                    activityCommonLookUp.AddRange(result.Select(a => new ActivityCommonLookUpInfo { CommonLkpId = a.CommonLkpId, CommonLkpTypeCode = a.CommonLkpTypeCode, CommonLkpCode = a.CommonLkpCode, CommonLkpName = a.CommonLkpName, CommonLkpDescription = a.CommonLkpDescription, SortOrder = a.SortOrder, IsDeleted = a.IsDeleted, CreatedDate = a.CreatedDate, ModifiedDate = a.ModifiedDate }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityCommonLookUp;
        }



        /// <summary>
        /// Get list of activity Owner detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        /// 
        public ActivityOwnerListDetail SyncActivityOwnerList(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            var activityOwnerListdetail = new ActivityOwnerListDetail();
            try
            {

                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var activityOwnerList = new List<ActivityOwnerListInfo>();
                    ObjectParameter outputRowCount = new ObjectParameter("RowCount", typeof(long));
                    var result = context.HIBOPGetActivityOwnerList_SP(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber, outputRowCount).ToList();
                    activityOwnerList.AddRange(result.Select(a => new ActivityOwnerListInfo { Lookupcode = a.Lookupcode, EmployeeName = a.EmployeeName }));
                    activityOwnerListdetail.ActivityOwnerLists = activityOwnerList;
                    activityOwnerListdetail.RowCount = Convert.ToInt64(outputRowCount.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityOwnerListdetail;
        }



        /// <summary>
        /// Get list of activity List detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        public List<ActivityListInfo> SyncActivityList(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityList = new List<ActivityListInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityList_SP(userId, lastSyncDate, ipAddress).ToList();
                    activityList.AddRange(result.Select(a => new ActivityListInfo { ActivityCodeId = a.UniqActivityCode, ActivityCode = a.ActivityCode, ActivityName = a.ActivityName, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, lookupCode = a.lookupCode, EmployeeName = a.EmployeeName, IsClosedStatus = a.ISClosedStatus }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityList;
        }


        /// <summary>
        /// Get list of activity bill detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        public List<ActivityBillInfo> SyncActivityBills(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityBill = new List<ActivityBillInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityBill_SP(userId, lastSyncDate, ipAddress).ToList();
                    activityBill.AddRange(result.Select(a => new ActivityBillInfo { BillId = a.BillId, EntityId = a.UniqEntity, TransheadId = a.UniqTranshead, AgencyName = a.AgencyName, BillNumber = a.BillNumber, AgencyId = a.UniqAgency, Amount = a.Amount, Balance = a.Balance, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityBill;
        }
        /// <summary>
        /// Get list of activity carrier detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ActivityCarrierInfo> SyncActivityCarriers(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityCarrier = new List<ActivityCarrierInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityCarrierSubmission_SP(userId, lastSyncDate, ipAddress).ToList();
                    activityCarrier.AddRange(result.Select(a => new ActivityCarrierInfo { EntityId = a.UniqEntity, CarrierSubmissionId = a.CarrierSubmissionId, UniqCarrierSubmission = a.UniqCarrierSubmission, CarrierSubmission = a.CarrierSubmission, Carrier = a.Carrier, MarketingSubmissionId = a.UniqMarketingSubmission, LastSubmittedDate = a.LastSubmittedDate, RequestedPremium = a.RequestedPremium, SubmissionStatus = a.SubmissionStatus, MarketingSubmission = a.MarkettingSubmission }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityCarrier;
        }


        /// <summary>
        /// Get list of activity Transaction detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ActivityTransactionInfo> SyncActivityTransactions(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityTransaction = new List<ActivityTransactionInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityTransaction_SP(userId, lastSyncDate, ipAddress).ToList();
                    activityTransaction.AddRange(result.Select(a => new ActivityTransactionInfo { TransactionId = a.TransactionId, TransheadId = a.UniqTranshead, EntityId = a.UniqEntity, InvoiceNumber = a.InvoiceNumber, Code = a.Code, DescriptionOf = a.DescriptionOf, Amount = a.Amount, Balance = a.Balance, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityTransaction;
        }

        /// <summary>
        /// Get list of activity Certificate detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ActivityCertificateInfo> SyncActivityCertificate(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityCertificateCollection = new List<ActivityCertificateInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityCertificate_SP(userId, lastSyncDate, ipAddress).ToList();
                    if (result != null && result.Count > 0)
                        activityCertificateCollection.AddRange(result.Select(a => new ActivityCertificateInfo { UniqCertificate = a.UniqCertificate, UniqEntity = a.UniqEntity, Title = a.Title, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityCertificateCollection;
        }


        /// <summary>
        /// Get list of activity Evidence detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ActivityEvidenceInfo> SyncActivityEvidence(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityCertificateCollection = new List<ActivityEvidenceInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityEvidence_SP(userId, lastSyncDate, ipAddress).ToList();
                    if (result != null && result.Count > 0)
                        activityCertificateCollection.AddRange(result.Select(a => new ActivityEvidenceInfo { UniqEvidence = a.UniqEvidence, UniqEntity = a.UniqEntityClient, Title = a.Title, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, FormEditionDate = a.FormEditionDate }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityCertificateCollection;
        }



        /// <summary>
        /// Get list of activity look up detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<ActivityLookUpInfo> SyncActivityLookUps(string userId, DateTime? lastSyncDate)
        {
            var activityLookUp = new List<ActivityLookUpInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetActivityLookupDetails_SP(userId, lastSyncDate).ToList();
                    activityLookUp.AddRange(result.Select(a => new ActivityLookUpInfo { ALDId = a.ALDId, LineId = a.UniqLine, PolicyId = a.UniqPolicy, EntityId = a.UniqEntity, ClaimId = a.UniqClaim, LineCode = a.LineCode, PolicyType = a.PolicyType, Linedescription = a.Linedescription, PolicyNumber = a.PolicyNumber, PolicyDesc = a.PolicyDesc, CdPolicyLineTypeId = a.UniqCdPolicyLineType, CdLineStatusId = a.UniqCdLineStatus, LineExpDate = a.LineExpDate, LineEffDate = a.LineEffDate, PolicyExpDate = a.PolicyExpDate, PolicyEffDate = a.PolicyEffDate, ClaimNumber = a.ClaimNumber, CompanyClaimNumber = a.CompanyClaimNumber, DateLoss = a.DateLoss, ClosedDate = a.ClosedDate, LookupCode = a.LookupCode, AccountName = a.AccountName, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, ClaimAssociationId = a.UniqClaimAssociation, IOC = a.IOC, IOCCode = a.IOCCode, AttachmentDesc = a.AttachmentDesc }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityLookUp;
        }
        /// <summary>
        /// Get list of activity employee agency detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        public List<EmployeeAgencyInfo> SyncActivityEmployeeAgencies(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var activityAgency = new List<EmployeeAgencyInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetEmployeeAgency_SP(userId, lastSyncDate, ipAddress).ToList();
                    activityAgency.AddRange(result.Select(a => new EmployeeAgencyInfo { EntityId = a.UniqEntity, EmployeeName = a.EmployeeName, LookupCode = a.LookupCode, AgencyId = a.UniqAgency, AgencyName = a.AgencyName, BranchId = a.UniqBranch, BranchName = a.BranchName, DepartmentId = a.UniqDepartment, DepartmentCode = a.DepartmentCode, Departmetname = a.DepartmentName, ProfitCenterId = a.UniqProfitCenter, ProfitCenterCode = a.ProfitCenterCode, ProfitCenterName = a.ProfitCenterName, AgencyCode = a.AgencyCode, BranchCode = a.BranchCode }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityAgency;
        }

        /// <summary>
        /// Get list of activity employee detail to sync local database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<EmployeeInfo> SyncActivityEmployee(string userId)
        {
            var activityEmployee = new List<EmployeeInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetEmployee_SP(userId).ToList();
                    activityEmployee.AddRange(result.Select(a => new EmployeeInfo { LookupCode = a.LookupCode, EntityId = a.UniqEntity, EmployeeName = a.EmployeeName, Department = a.Department, JobTitle = a.JobTitle, InactiveDate = a.InactiveDate, RoleFlags = a.RoleFlags, Flags = a.Flags, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, IsAdmin = a.IsAdmin }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return activityEmployee;
        }
    }
}
