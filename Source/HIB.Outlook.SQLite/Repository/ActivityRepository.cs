using HIB.Outlook.Helper.Common;
using HIB.Outlook.Helper.Helper;
using HIB.Outlook.Model;
using HIB.Outlook.SQLite.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;

namespace HIB.Outlook.SQLite.Repository
{
    public class ActivityRepository : IActivityRepository
    {
        /// <summary>
        /// Sync client details to local sqlite DB
        /// </summary>
        /// <param name="activityEmployee"></param>
        /// <returns></returns>  
        public ResultInfo SyncActivityEmployees(List<ActivityEmployee> clientEmployeeList)
        {

            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in clientEmployeeList)
                        {
                            try
                            {
                                if (a != null)
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityEmployee(UniqEmployee,EmployeeLookupCode,UniqEntity,UniqActivity) VALUES ('" + a.UniqEmployee + "','" + a.EmployeeLookupcode + "', '" + a.UniqEntity + "', '" + a.UniqActivity + "')";
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }

            return resultInfo;
        }


        /// <summary>
        /// Save Activity detail to local Database
        /// </summary>
        /// <param name="oListActivity"></param>
        /// <returns></returns>

        public ResultInfo SyncActivity(List<ActivityInfo> activityList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivity(UniqActivity,UniqEntity,ActivityCode,DescriptionOf,UniqCdPolicyLineType,PolicyNumber,InsertedDate,UpdatedDate,Status,ExpirationDate,EffectiveDate,UniqAgency,UniqBranch,UniqDepartment,UniqProfitCenter,UniqAssociatedItem,AssociationType,OwnerDescription,ClosedDate,UniqPolicy,UniqLine,UniqClaim,LossDate,PolicyDescription,LineCode,LineDescription,ICO,LineEffectiveDate,LineExpirationDate) VALUES ('" + a.ActivityId + "','" + a.EntityId + "','" + a.ActivityCode?.Replace("'", "''") + "','" + a.DescriptionOf?.Replace("'", "''") + "','" + a.CdPolicyLineTypeCode?.Replace("'", "''") + "','" + a.PolicyNumber?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "','" + a.Status + "','" + ExtensionClass.SqliteDateTimeFormat(a.ExpirationDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.EffectiveDate) + "','" + a.AgencyId + "','" + a.BranchId + "','" + a.DepartmentId + "','" + a.ProfitCenterId + "','" + a.AssociatedItemId + "','" + a.AssociationType?.Replace("'", "''") + "','" + a.OwnerName?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.ClosedDate) + "','" + a.UniqPolicy + "','" + a.UniqLine + "','" + a.UniqClaim + "','" + ExtensionClass.SqliteDateTimeFormat(a.LossDate) + "','" + a.Policydescription?.Replace("'", "''") + "','" + a.LineCode?.Replace("'", "''") + "','" + a.LineDescription?.Replace("'", "''") + "','" + a.ICO?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.LineEffectiveDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.LineExpirationDate) + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }
                        }
                        resultInfo.IsSuccess = true;
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }

            return resultInfo;
        }


        /// <summary>
        /// Save Activity Claims to local Database
        /// </summary>
        /// <param name="activityClaimInfo"></param>
        /// <returns></returns>


        public ResultInfo SyncActivityClaim(List<ActivityClaimInfo> activityClaimList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                var userLookupCode = activityClaimList.FirstOrDefault()?.UserLookupCode;
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityClaimList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPClaim(EmployeeLookUpCode,UniqClaim,UniqEntity,ClaimCode,ClaimName,LossDate,ReportedDate,ClaimNumber,CompanyClaimNumber,ClosedDate,InsertedDate,UpdatedDate,DelFlag) VALUES ('" + userLookupCode + "','" + a.ClaimId + "','" + a.EntityId + "', '" + a.ClaimCode?.Replace("'", "''") + "', '" + a.ClaimName?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.LossDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.ReportedDate) + "','" + a.AgencyClaimNumber + "','" + a.CompanyClaimNumber?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.ClosedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "','" + a.Flags + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }


        /// <summary>
        /// Save Activity policy to local Database
        /// </summary>
        /// <param name="activityPolicyInfo"></param>
        /// <returns></returns>

        public ResultInfo SyncActivityPolicy(List<ActivityPolicyInfo> activityPolicyList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityPolicyList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    int flagVal = (a.Status == "Active") ? 1 : 0;
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPPolicy(EmployeeLookUpCode,UniqPolicy,UniqEntity,CdPolicyLineTypeCode,PolicyNumber,DescriptionOf,EffectiveDate,ExpirationDate,PolicyStatus,Flags,InsertedDate,UpdatedDate,DelFlag) VALUES ('" + a.EmployeeLookUpCode + "','" + a.PolicyId + "','" + a.EntityId + "', '" + a.PolicyLineTypeCode?.Replace("'", "''") + "', '" + a.PolicyNumber.Replace("'", "''") + "','" + a.DescriptionOf.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.EffectiveDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.ExpirationDate) + "','" + a.PolicyStatus?.Replace("'", "''") + "'," + flagVal + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "','" + a.Flags + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }
                        }
                        transaction.Commit();
                        SqliteHelper.Connection.Close();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }


        /// <summary>
        /// Save activity service to local Database
        /// </summary>
        /// <param name="activityPolicyInfo"></param>
        /// <returns></returns>

        public ResultInfo SyncActivityService(List<ActivityServiceInfo> activityServiceList)
        {

            var resultInfo = new ResultInfo();
            try
            {
                var userLookupCode = activityServiceList.FirstOrDefault()?.UserLookupCode;
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityServiceList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    int flagVal = (a.Status == "Active") ? 1 : 0;
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityServices(EmployeeLookUpCode,UniqServiceHead,UniqEntity,ServiceNumber,UniqCdServiceCode,Description,ContractNumber,InceptionDate,ExpirationDate,Flags,InsertedDate,UpdatedDate,DelFlag) VALUES ('" + userLookupCode + "','" + a.ServiceHeadId + "','" + a.EntityId + "', '" + a.ServiceNumber + "', '" + a.ServiceCodeId?.Replace("'", "''") + "','" + a.Description.Replace("'", "''") + "','" + a.ContractNumber?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.InceptionDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.ExpirationDate) + "'," + flagVal + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "','" + a.Flags + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }
                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }


        /// <summary>
        /// Save activity line to local Database
        /// </summary>
        /// <param name="activityPolicyInfo"></param>
        /// <returns></returns>

        public ResultInfo SyncActivityLine(List<ActivityLineInfo> activityLineList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                var userLookupCode = activityLineList.FirstOrDefault()?.UserLookupCode;
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityLineList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    int flagVal = (a.Status == "Active") ? 1 : 0;
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityLine(EmployeeLookUpCode,UniqLine,UniqPolicy,UniqEntity,PolicyDesc,LineCode,LineOfBusiness,LineStatus,PolicyNumber,UniqCdPolicyLineType,UniqCdLineStatus,BillModeCode,ExpirationDate,EffectiveDate,IOC,Flags,InsertedDate,UpdatedDate,DelFlag) VALUES ('" + userLookupCode + "','" + a.LineId + "','" + a.PolicyId + "', '" + a.EntityId + "', '" + a.PolicyDesc?.Replace("'", "''") + "','" + a.LineCode?.Replace("'", "''") + "','" + a.LineOfBusiness?.Replace("'", "''") + "','" + a.LineStatus?.Replace("'", "''") + "','" + a.PolicyNumber?.Replace("'", "''") + "','" + a.PolicyLineTypeId + "','" + a.LineStatusId + "' ,'" + a.BillModeCode?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.ExpirationDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.EffectiveDate) + "','" + a.IOC + "'," + flagVal + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "','" + a.Flags + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }


        /// <summary>
        /// Save activity Opportunity to local Database
        /// </summary>
        /// <param name="activityPolicyInfo"></param>
        /// <returns></returns>
        public ResultInfo SyncActivityOpportunity(List<ActivityOpportunityInfo> activityOpportunityList)
        {
            var resultInfo = new ResultInfo();
            try
            {
                var userLookupCode = activityOpportunityList.FirstOrDefault()?.UserLookupCode;
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityOpportunityList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    int flagVal = (a.Status == "Active") ? 1 : 0;
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityOpportunity(EmployeeLookUpCode,UniqOpportunity,UniqEntity,OppDesc,TargetedDate,OwnerName,SalesManager,Stage,Flags,InsertedDate,UpdatedDate,DelFlag) VALUES ('" + userLookupCode + "','" + a.OpportunityId + "','" + a.EntityId + "', '" + a.OppDesc.Replace("'", "''") + "', '" + ExtensionClass.SqliteDateTimeFormat(a.TargetedDate) + "','" + a.OwnerName?.Replace("'", "''") + "','" + a.SalesManager?.Replace("'", "''") + "','" + a.Stage?.Replace("'", "''") + "'," + flagVal + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "','" + a.Flags + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }

        /// <summary>
        /// Save activity marketing to local Database
        /// </summary>
        /// <param name="activityMarketing"></param>
        /// <returns></returns>

        public ResultInfo SyncActivityMarketing(List<ActivityMarketingInfo> activityMarketingList)
        {
            var resultInfo = new ResultInfo();
            try
            {
                var userLookupCode = activityMarketingList.FirstOrDefault()?.UserLookupCode;
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityMarketingList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    int flagVal = (a.Status == "Active") ? 1 : 0;
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityMasterMarketing(EmployeeLookUpCode,UniqMarketingSubmission,UniqEntity,UniqAgency,UniqBranch,DescriptionOf,EffectiveDate,ExpirationDate,LastSubmittedDate,Flags,LineOfBusiness,InsertedDate,UpdatedDate,DelFlag) VALUES ('" + userLookupCode + "','" + a.UniqMarketingSubmission + "','" + a.UniqEntity + "', '" + a.UniqAgency + "', '" + a.UniqBranch + "','" + a.MarketingSubbmission?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.EffectiveDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.ExpirationDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.LastSubmittedDate) + "'," + flagVal + ",'" + a.LineOfBusiness?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "','" + a.Flags + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }
                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }


        /// <summary>
        /// Save activity Client Contact to local Database
        /// </summary>
        /// <param name="activityClientContactInfo"></param>
        /// <returns></returns>


        public ResultInfo SyncActivityClientContact(List<ActivityClientContactInfo> activityClientContact)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityClientContact)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityClientContacts(ClientContactId,UniqContactNumber,UniqEntity,UniqContactName,ContactName,ContactType,ContactValue,InsertedDate,UpdatedDate) VALUES (" + a.ClientContactId + "," + a.ContactNumberId + ",'" + a.EntityId + "','" + a.ContactNameId + "', '" + a.ContactName?.Replace("'", "''") + "', '" + a.ContactType?.Replace("'", "''") + "','" + a.ContactValue?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }
                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }



        /// <summary>
        /// Save activity Common LookUp to local Database
        /// </summary>
        /// <param name="activityCommonLookUpInfo"></param>
        /// <returns></returns>
        public ResultInfo SyncActivityCommonLookUp(List<ActivityCommonLookUpInfo> activityCommonLookUpList)
        {
            var resultInfo = new ResultInfo();

            using (var context = new HIBOutlookEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in activityCommonLookUpList)
                        {
                            if (item != null)
                            {
                                try
                                {
                                    var isAdd = false;
                                    var activityCommonLookUpItem = context.HIBOPCommonLookups.FirstOrDefault(a => a.CommonLkpId == item.CommonLkpId);
                                    if (activityCommonLookUpItem == null)
                                    {
                                        isAdd = true;
                                        activityCommonLookUpItem = new HIBOPCommonLookup();
                                    }
                                    activityCommonLookUpItem.CommonLkpId = item.CommonLkpId;
                                    activityCommonLookUpItem.CommonLkpTypeCode = item.CommonLkpTypeCode;
                                    activityCommonLookUpItem.CommonLkpCode = item.CommonLkpCode;
                                    activityCommonLookUpItem.CommonLkpName = item.CommonLkpName;
                                    activityCommonLookUpItem.CommonLkpDescription = item.CommonLkpDescription;
                                    activityCommonLookUpItem.SortOrder = item.SortOrder;
                                    activityCommonLookUpItem.IsDeleted = item.IsDeleted;
                                    activityCommonLookUpItem.CreatedDate = item.CreatedDate;
                                    activityCommonLookUpItem.ModifiedDate = item.ModifiedDate;

                                    if (isAdd)
                                        context.HIBOPCommonLookups.Add(activityCommonLookUpItem);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        context.SaveChanges();
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                        transaction.Rollback();
                        resultInfo.IsSuccess = false;
                        resultInfo.ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        Logger.save();
                    }

                }
            }
            return resultInfo;
        }
        /// <summary>
        /// Save activity Owner List details to local Database
        /// </summary>
        /// <param name="activityOwnerListInfo"></param>
        /// <returns></returns>   
        public ResultInfo SyncActivityOwnerList(List<ActivityOwnerListInfo> activityOwnerList)
        {
            var resultInfo = new ResultInfo();
            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {

                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityOwnerList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    cmd.CommandText = $"SELECT Count(*) FROM HIBOPActivityOwnerList WHERE Lookupcode = '{a.Lookupcode}' and EmployeeName = '{a.EmployeeName?.Replace("'", "''")}' ";
                                    int IdCount = Convert.ToInt32(cmd.ExecuteScalar());

                                    if (IdCount == 0)
                                    {
                                        cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityOwnerList(Lookupcode,EmployeeName) VALUES ('" + a.Lookupcode + "','" + a.EmployeeName?.Replace("'", "''") + "')";
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }


        /// <summary>
        /// Save activity List Info to local Database
        /// </summary>
        /// <param name="activityListInfo"></param>
        /// <returns></returns>
        public ResultInfo SyncActivityList(List<ActivityListInfo> activityList)
        {
            var resultInfo = new ResultInfo();
            using (var context = new HIBOutlookEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in activityList)
                        {
                            if (item != null)
                            {
                                try
                                {
                                    var isAdd = false;
                                    var activityListItem = context.HIBOPActivityLists.FirstOrDefault(a => a.UniqActivityCode == item.ActivityCodeId);
                                    if (activityListItem == null)
                                    {
                                        isAdd = true;
                                        activityListItem = new HIBOPActivityList();
                                    }
                                    activityListItem.UniqActivityCode = item.ActivityCodeId;
                                    activityListItem.ActivityCode = item.ActivityCode;
                                    activityListItem.ActivityName = item.ActivityName;
                                    activityListItem.InsertedDate = item.InsertedDate;
                                    activityListItem.UpdatedDate = item.UpdatedDate;
                                    activityListItem.LookupCode = item.lookupCode;
                                    activityListItem.EmployeeName = item.EmployeeName;
                                    activityListItem.IsClosedStatus = item.IsClosedStatus;

                                    if (isAdd)
                                        context.HIBOPActivityLists.Add(activityListItem);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        context.SaveChanges();
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                        transaction.Rollback();
                        resultInfo.IsSuccess = false;
                        resultInfo.ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        Logger.save();
                    }
                }
            }
            return resultInfo;
        }


        /// <summary>
        /// Save activity account to local Database
        /// </summary>
        /// <param name="activityAccountInfo"></param>
        /// <returns></returns>
        /// 


        public ResultInfo SyncActivityAccount(List<ActivityAccountInfo> activityAccount)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {

                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityAccount)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    //cmd.CommandText = $"SELECT count(*) FROM HIBOPActivityAccount WHERE UniqEntity = {a.EntityId}  and UniqAgency = {a.AgencyId} and  UniqBranch = {a.BranchId } and LookupCode ='{a.UserLookupCode}' ";
                                    //int IdCount = Convert.ToInt32(cmd.ExecuteScalar());

                                    //if (IdCount == 0)
                                    //{
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityAccount(AccountId,UniqEntity,UniqAgency,AgencyCode,AgencyName,UniqBranch,BranchCode,BranchName,InsertedDate,UpdatedDate,LookupCode) VALUES ('" + a.RowNumber + "','" + a.EntityId + "','" + a.AgencyId + "', '" + a.AgencyCode?.Replace("'", "''") + "', '" + a.AgencyName?.Replace("'", "''") + "','" + a.BranchId + "','" + a.BranchCode?.Replace("'", "''") + "','" + a.BranchName?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "','" + activityAccount[0].UserLookupCode?.Trim() + "')";
                                    cmd.ExecuteNonQuery();
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }


        /// <summary>
        /// Save activity bill to local Database
        /// </summary>
        /// <param name="activityBillInfo"></param>
        /// <returns></returns>     
        public ResultInfo SyncActivityBill(List<ActivityBillInfo> activityBillList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {

                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityBillList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityBill(BillId,UniqTranshead,UniqEntity,BillNumber,UniqAgency,AgencyName,Amount,Balance,InsertedDate,UpdatedDate) VALUES ('" + a.BillId + "','" + a.TransheadId + "','" + a.EntityId + "','" + a.BillNumber + "', '" + a.AgencyId + "', '" + a.AgencyName?.Replace("'", "''") + "'," + a.Amount + "," + a.Balance + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }



        /// <summary>
        /// Save activity Transaction to local Database
        /// </summary>
        /// <param name="activityTransactionInfo"></param>
        /// <returns></returns>

        public ResultInfo SyncActivityTransaction(List<ActivityTransactionInfo> activityTransactionList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityTransactionList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityTransaction(TransactionId,UniqTranshead,Code,DescriptionOf,UniqEntity,InvoiceNumber,Amount,Balance,InsertedDate,UpdatedDate) VALUES (" + a.TransactionId + "," + a.TransheadId + ", '" + a.Code?.Replace("'", "''") + "', '" + a.DescriptionOf?.Replace("'", "''") + "'," + a.EntityId + "," + a.InvoiceNumber + "," + a.Amount + "," + a.Balance + ",'" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    cmd.ExecuteNonQuery();

                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }



        /// <summary>
        /// Save activity Certificates to local Database
        /// </summary>
        /// <param name="activityCertificateList"></param>
        /// <returns></returns>    
        public ResultInfo SyncActivityCertificate(List<ActivityCertificateInfo> activityCertificateList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {

                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityCertificateList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityCertificate(UniqCertificate,UniqEntity,Title,InsertedDate,UpdatedDate) VALUES (" + a.UniqCertificate + "," + a.UniqEntity + ",'" + a.Title?.Replace("'", "''") + "', '" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "', '" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }
                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }




        /// <summary>
        /// Save activity Evidence to local Database
        /// </summary>
        /// <param name="activityEvidenceList"></param>
        /// <returns></returns>

        public ResultInfo SyncActivityEvidence(List<ActivityEvidenceInfo> activityEvidenceList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityEvidenceList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityEvidence(UniqEvidence,UniqEntity,Title,FormEditionDate,InsertedDate,UpdatedDate)VALUES (" + a.UniqEvidence + "," + a.UniqEntity + ",'" + a.Title?.Replace("'", "''") + "','" + a.FormEditionDate?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }


        /// <summary>
        /// Save activity Carrier to local Database
        /// </summary>
        /// <param name="activityCarrierInfo"></param>
        /// <returns></returns>
        public ResultInfo SyncActivityCarrierSubmission(List<ActivityCarrierInfo> activityCarrierList)
        {
            var resultInfo = new ResultInfo();
            using (var context = new HIBOutlookEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in activityCarrierList)
                        {
                            if (item != null)
                            {
                                try
                                {
                                    var isAdd = false;
                                    var activityCarrierSubmissionItem = context.HIBOPCarrierSubmissions.FirstOrDefault(a => a.CarrierSubmissionId == item.CarrierSubmissionId);
                                    if (activityCarrierSubmissionItem == null)
                                    {
                                        isAdd = true;
                                        activityCarrierSubmissionItem = new HIBOPCarrierSubmission();
                                    }

                                    activityCarrierSubmissionItem.CarrierSubmissionId = item.CarrierSubmissionId;
                                    activityCarrierSubmissionItem.UniqCarrierSubmission = item.UniqCarrierSubmission;

                                    activityCarrierSubmissionItem.UniqEntity = Convert.ToInt32(item.EntityId);
                                    activityCarrierSubmissionItem.Carrier = item.Carrier;
                                    activityCarrierSubmissionItem.UniqMarketingSubmission = item.MarketingSubmissionId;
                                    activityCarrierSubmissionItem.MarkettingSubmission = item.MarketingSubmission;
                                    activityCarrierSubmissionItem.CarrierSubmission = item.CarrierSubmission;
                                    activityCarrierSubmissionItem.LastSubmittedDate = item.LastSubmittedDate;
                                    activityCarrierSubmissionItem.RequestedPremium = item.RequestedPremium;
                                    activityCarrierSubmissionItem.SubmissionStatus = item.SubmissionStatus;

                                    if (isAdd)
                                        context.HIBOPCarrierSubmissions.Add(activityCarrierSubmissionItem);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }
                        }
                        context.SaveChanges();
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                        transaction.Rollback();
                        resultInfo.IsSuccess = false;
                        resultInfo.ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        Logger.save();
                    }

                }
            }
            return resultInfo;
        }
        /// <summary>
        /// Save activity Look Up to local Database
        /// </summary>
        /// <param name="activityLookUpInfo"></param>
        /// <returns></returns>


        public ResultInfo SyncActivityLookUp(List<ActivityLookUpInfo> activityLookUpList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in activityLookUpList)
                        {
                            if (a != null)
                            {
                                try
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPActivityLookupDetails(ALDId,UniqEntity,UniqLine,UniqPolicy,UniqClaim,LineCode,PolicyType,Linedescription,PolicyNumber,PolicyDesc,UniqCdPolicyLineType,UniqCdLineStatus,LineExpDate,LineEffDate,PolicyExpDate,PolicyEffDate,ClaimNumber,CompanyClaimNumber,DateLoss,ClosedDate,LookupCode,AccountName,InsertedDate,UpdatedDate,UniqClaimAssociation,IOC,IOCCode,AttachmentDesc) VALUES ('" + a.ALDId + "','" + a.EntityId + "', '" + a.LineId + "', '" + a.PolicyId + "','" + a.ClaimId + "','" + a.LineCode?.Replace("'", "''") + "','" + a.PolicyType?.Replace("'", "''") + "','" + a.Linedescription?.Replace("'", "''") + "','" + a.PolicyNumber?.Replace("'", "''") + "','" + a.PolicyDesc?.Replace("'", "''") + "' ,'" + a.CdPolicyLineTypeId + "','" + a.CdLineStatusId + "','" + ExtensionClass.SqliteDateTimeFormat(a.LineExpDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.LineEffDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.PolicyExpDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.PolicyEffDate) + "','" + a.ClaimNumber + "','" + a.CompanyClaimNumber?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.DateLoss) + "','" + ExtensionClass.SqliteDateTimeFormat(a.ClosedDate) + "','" + a.LookupCode?.Replace("'", "''") + "','" + a.AccountName.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(a.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(a.UpdatedDate) + "','" + a.ClaimAssociationId + "','" + a.IOC?.Replace("'", "''") + "','" + a.IOCCode?.Replace("'", "''") + "','" + a.AttachmentDesc?.Replace("'", "''") + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");

                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }
            return resultInfo;
        }



        /// <summary>
        /// Save activity Employee Agency to local Database
        /// </summary>
        /// <param name="employeeAgencyInfo"></param>
        /// <returns></returns>

        public ResultInfo SyncActivityEmployeeAgency(List<EmployeeAgencyInfo> activityEmployeeAgencyList)
        {
            var resultInfo = new ResultInfo();
            using (var context = new HIBOutlookEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in activityEmployeeAgencyList)
                        {
                            if (item != null)
                            {
                                try
                                {
                                    var isAdd = false;
                                    var activityEmployeeAgencyItem = context.HIBOPEmployeeAgencies.FirstOrDefault(a => a.UniqAgency == item.AgencyId && a.UniqBranch == item.BranchId && a.UniqDepartment == item.DepartmentId && a.UniqProfitCenter == item.ProfitCenterId && a.UniqEntity == item.EntityId);
                                    if (activityEmployeeAgencyItem == null)
                                    {
                                        isAdd = true;
                                        activityEmployeeAgencyItem = new HIBOPEmployeeAgency();
                                    }
                                    activityEmployeeAgencyItem.UniqEntity = item.EntityId;
                                    activityEmployeeAgencyItem.EmployeeName = item.EmployeeName;
                                    activityEmployeeAgencyItem.LookupCode = item.LookupCode;
                                    activityEmployeeAgencyItem.UniqAgency = item.AgencyId;
                                    activityEmployeeAgencyItem.AgencyCode = item.AgencyCode;
                                    activityEmployeeAgencyItem.BranchCode = item.BranchCode;
                                    activityEmployeeAgencyItem.AgencyName = item.AgencyName;
                                    activityEmployeeAgencyItem.BranchName = item.BranchName;
                                    activityEmployeeAgencyItem.UniqBranch = item.BranchId;
                                    activityEmployeeAgencyItem.UniqDepartment = item.DepartmentId;
                                    activityEmployeeAgencyItem.DepartmentCode = item.DepartmentCode;
                                    activityEmployeeAgencyItem.Departmetname = item.Departmetname;
                                    activityEmployeeAgencyItem.UniqProfitCenter = item.ProfitCenterId;
                                    activityEmployeeAgencyItem.ProfitCenterCode = item.ProfitCenterCode;
                                    activityEmployeeAgencyItem.ProfitCenterNAme = item.ProfitCenterName;
                                    if (isAdd)
                                        context.HIBOPEmployeeAgencies.Add(activityEmployeeAgencyItem);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }


                        }
                        context.SaveChanges();
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                        transaction.Rollback();
                        resultInfo.IsSuccess = false;
                        resultInfo.ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        Logger.save();
                    }
                }
            }
            return resultInfo;
        }


        /// <summary>
        /// Save activity Employee to local Database
        /// </summary>
        /// <param name="employeeInfo"></param>
        /// <returns></returns>
        public ResultInfo SyncActivityEmployee(List<EmployeeInfo> activityEmployeeList)
        {
            var resultInfo = new ResultInfo();
            using (var context = new HIBOutlookEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in activityEmployeeList)
                        {
                            if (item != null)
                            {
                                try
                                {
                                    var isAdd = false;
                                    var activityEmployeeItem = context.HIBOPEmployees.FirstOrDefault(a => a.LookupCode == item.LookupCode);
                                    if (activityEmployeeItem == null)
                                    {
                                        isAdd = true;
                                        activityEmployeeItem = new HIBOPEmployee();
                                        activityEmployeeItem.SyncStatusNotified = 0;
                                    }
                                    activityEmployeeItem.UniqEntity = item.EntityId;
                                    activityEmployeeItem.LookupCode = item.LookupCode;
                                    activityEmployeeItem.EmployeeName = item.EmployeeName;
                                    activityEmployeeItem.Department = item.Department;
                                    activityEmployeeItem.JobTitle = item.JobTitle;
                                    activityEmployeeItem.InactiveDate = item.InactiveDate;
                                    activityEmployeeItem.RoleFlags = item.RoleFlags;
                                    activityEmployeeItem.Flags = item.Flags;
                                    activityEmployeeItem.InsertedDate = item.InsertedDate;
                                    activityEmployeeItem.UpdatedDate = item.UpdatedDate;

                                    activityEmployeeItem.Status = 1;
                                    if (isAdd)
                                        context.HIBOPEmployees.Add(activityEmployeeItem);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
                            }
                        }
                        context.SaveChanges();
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                        transaction.Rollback();
                        resultInfo.IsSuccess = false;
                        resultInfo.ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        Logger.save();
                    }
                }
            }
            return resultInfo;
        }
        /// <summary>
        /// Save Employee lookup code to local Database
        /// </summary>
        /// <param name="employeeInfo"></param>
        /// <returns></returns>
        public ResultInfo InsertEmployeeLookUpCode(List<string> lookUpList, bool status)
        {
            var resultInfo = new ResultInfo();
            using (var context = new HIBOutlookEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in lookUpList)
                        {

                            try
                            {
                                var isAdd = false;
                                var activityEmployeeItem = context.HIBOPEmployees.FirstOrDefault(a => a.LookupCode == item);
                                if (activityEmployeeItem == null)
                                {
                                    isAdd = true;
                                    activityEmployeeItem = new HIBOPEmployee();
                                }

                                activityEmployeeItem.LookupCode = item;
                                if (status)
                                    activityEmployeeItem.Status = 1;
                                else
                                    activityEmployeeItem.Status = 0;

                                if (isAdd)
                                    context.HIBOPEmployees.Add(activityEmployeeItem);
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, item);
                            }

                        }
                        context.SaveChanges();
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                        transaction.Rollback();
                        resultInfo.IsSuccess = false;
                        resultInfo.ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        Logger.save();
                    }
                }
            }
            return resultInfo;
        }

        /// <summary>
        /// Get User LookUp Code to sync data
        /// </summary>
        /// <param name="clientList"></param>
        /// <returns></returns>

        public List<string> GetUserLookUpCode()
        {
            var userLookUpCode = new List<string>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    var result = context.HIBOPEmployees.Where(d => d.Status == 0).ToList();
                    //  var result = context.HIBOPEmployees.ToList();
                    userLookUpCode.AddRange(result.Select(l => l.LookupCode));
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");

            }
            finally
            {
                Logger.save();
            }
            return userLookUpCode;
        }




    }
}