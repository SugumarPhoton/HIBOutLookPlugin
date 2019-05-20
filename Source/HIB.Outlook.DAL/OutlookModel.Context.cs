﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HIB.Outlook.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class HIBOutlookEntities : DbContext
    {
        public HIBOutlookEntities()
            : base("name=HIBOutlookEntities")
        {            
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual int HIBOPEpicSyncClient_SP()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("HIBOPEpicSyncClient_SP");
        }
    
        public virtual ObjectResult<string> HIBOPEpicSyncPolicyLineType_SP()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("HIBOPEpicSyncPolicyLineType_SP");
        }
    
        public virtual int HIBOPGetLogDetails_SP(string uniqId)
        {
            var uniqIdParameter = uniqId != null ?
                new ObjectParameter("UniqId", uniqId) :
                new ObjectParameter("UniqId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("HIBOPGetLogDetails_SP", uniqIdParameter);
        }
    
        public virtual int HIBOPLogInsert_SP(string uniqId, Nullable<int> uniqEntity, Nullable<int> uniqActivity, Nullable<int> policyYear, string policyTypeCode, Nullable<int> descritionLookUpId, string description, Nullable<int> folderId, Nullable<int> subFolder1Id, Nullable<int> subFolder2Id, Nullable<System.DateTime> clientAccessibleDate, Nullable<int> status, string createdBy)
        {
            var uniqIdParameter = uniqId != null ?
                new ObjectParameter("UniqId", uniqId) :
                new ObjectParameter("UniqId", typeof(string));
    
            var uniqEntityParameter = uniqEntity.HasValue ?
                new ObjectParameter("UniqEntity", uniqEntity) :
                new ObjectParameter("UniqEntity", typeof(int));
    
            var uniqActivityParameter = uniqActivity.HasValue ?
                new ObjectParameter("UniqActivity", uniqActivity) :
                new ObjectParameter("UniqActivity", typeof(int));
    
            var policyYearParameter = policyYear.HasValue ?
                new ObjectParameter("PolicyYear", policyYear) :
                new ObjectParameter("PolicyYear", typeof(int));
    
            var policyTypeCodeParameter = policyTypeCode != null ?
                new ObjectParameter("PolicyTypeCode", policyTypeCode) :
                new ObjectParameter("PolicyTypeCode", typeof(string));
    
            var descritionLookUpIdParameter = descritionLookUpId.HasValue ?
                new ObjectParameter("DescritionLookUpId", descritionLookUpId) :
                new ObjectParameter("DescritionLookUpId", typeof(int));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var folderIdParameter = folderId.HasValue ?
                new ObjectParameter("FolderId", folderId) :
                new ObjectParameter("FolderId", typeof(int));
    
            var subFolder1IdParameter = subFolder1Id.HasValue ?
                new ObjectParameter("SubFolder1Id", subFolder1Id) :
                new ObjectParameter("SubFolder1Id", typeof(int));
    
            var subFolder2IdParameter = subFolder2Id.HasValue ?
                new ObjectParameter("SubFolder2Id", subFolder2Id) :
                new ObjectParameter("SubFolder2Id", typeof(int));
    
            var clientAccessibleDateParameter = clientAccessibleDate.HasValue ?
                new ObjectParameter("ClientAccessibleDate", clientAccessibleDate) :
                new ObjectParameter("ClientAccessibleDate", typeof(System.DateTime));
    
            var statusParameter = status.HasValue ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(int));
    
            var createdByParameter = createdBy != null ?
                new ObjectParameter("CreatedBy", createdBy) :
                new ObjectParameter("CreatedBy", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("HIBOPLogInsert_SP", uniqIdParameter, uniqEntityParameter, uniqActivityParameter, policyYearParameter, policyTypeCodeParameter, descritionLookUpIdParameter, descriptionParameter, folderIdParameter, subFolder1IdParameter, subFolder2IdParameter, clientAccessibleDateParameter, statusParameter, createdByParameter);
        }
    
        public virtual ObjectResult<HIBOPSynzClientToLocal_SP_Result> HIBOPSynzClientToLocal_SP(Nullable<int> user, Nullable<System.DateTime> lastSyncDate)
        {
            var userParameter = user.HasValue ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(int));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPSynzClientToLocal_SP_Result>("HIBOPSynzClientToLocal_SP", userParameter, lastSyncDateParameter);
        }
    
        public virtual ObjectResult<HIBOPGetPolicyLineType_SP_Result> HIBOPGetPolicyLineType_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetPolicyLineType_SP_Result>("HIBOPGetPolicyLineType_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPSyncErrorLogToLocal_SP_Result> HIBOPSyncErrorLogToLocal_SP(string user, Nullable<System.DateTime> lastSyncDate)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPSyncErrorLogToLocal_SP_Result>("HIBOPSyncErrorLogToLocal_SP", userParameter, lastSyncDateParameter);
        }
    
        public virtual int HIBOPSyncFavouriteToCentralized_SP(string iPAddress)
        {
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("HIBOPSyncFavouriteToCentralized_SP", iPAddressParameter);
        }
    
        public virtual int HIBOPSyncFavouriteToLocal_SP(string user, Nullable<System.DateTime> lastSyncDate)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("HIBOPSyncFavouriteToLocal_SP", userParameter, lastSyncDateParameter);
        }
    
        public virtual ObjectResult<HIBOPSyncAuditLogToLocal_SP_Result> HIBOPSyncAuditLogToLocal_SP(string user, Nullable<System.DateTime> lastSyncDate)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPSyncAuditLogToLocal_SP_Result>("HIBOPSyncAuditLogToLocal_SP", userParameter, lastSyncDateParameter);
        }
    
        public virtual int HIBOPSyncAuditLogToCentralized_SP()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("HIBOPSyncAuditLogToCentralized_SP");
        }
    
        public virtual int HIBOPSyncErrorLogToCenterlized_SP()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("HIBOPSyncErrorLogToCenterlized_SP");
        }
    
        public virtual ObjectResult<HIBOPGetActivityOwnerList_SP_Result> HIBOPGetActivityOwnerList_SP(string user, Nullable<System.DateTime> lastSynDate, string iPAddress, Nullable<int> rowsPerPage, Nullable<int> pageNumber, ObjectParameter rowCount)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSynDateParameter = lastSynDate.HasValue ?
                new ObjectParameter("LastSynDate", lastSynDate) :
                new ObjectParameter("LastSynDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            var rowsPerPageParameter = rowsPerPage.HasValue ?
                new ObjectParameter("RowsPerPage", rowsPerPage) :
                new ObjectParameter("RowsPerPage", typeof(int));
    
            var pageNumberParameter = pageNumber.HasValue ?
                new ObjectParameter("PageNumber", pageNumber) :
                new ObjectParameter("PageNumber", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityOwnerList_SP_Result>("HIBOPGetActivityOwnerList_SP", userParameter, lastSynDateParameter, iPAddressParameter, rowsPerPageParameter, pageNumberParameter, rowCount);
        }
    
        public virtual ObjectResult<HIBOPGetFolders_SP_Result> HIBOPGetFolders_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetFolders_SP_Result>("HIBOPGetFolders_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityClientContacts_SP_Result> HIBOPGetActivityClientContacts_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityClientContacts_SP_Result>("HIBOPGetActivityClientContacts_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityLookupDetails_SP_Result> HIBOPGetActivityLookupDetails_SP(string user, Nullable<System.DateTime> lastSyncDate)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityLookupDetails_SP_Result>("HIBOPGetActivityLookupDetails_SP", userParameter, lastSyncDateParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityTransaction_SP_Result> HIBOPGetActivityTransaction_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityTransaction_SP_Result>("HIBOPGetActivityTransaction_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityBill_SP_Result> HIBOPGetActivityBill_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityBill_SP_Result>("HIBOPGetActivityBill_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityCarrierSubmission_SP_Result> HIBOPGetActivityCarrierSubmission_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityCarrierSubmission_SP_Result>("HIBOPGetActivityCarrierSubmission_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityCertificate_SP_Result> HIBOPGetActivityCertificate_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityCertificate_SP_Result>("HIBOPGetActivityCertificate_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityEvidence_SP_Result> HIBOPGetActivityEvidence_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityEvidence_SP_Result>("HIBOPGetActivityEvidence_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetCommonLookUp_SP_Result> HIBOPGetCommonLookUp_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetCommonLookUp_SP_Result>("HIBOPGetCommonLookUp_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetEmployeeAgency_SP_Result> HIBOPGetEmployeeAgency_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetEmployeeAgency_SP_Result>("HIBOPGetEmployeeAgency_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetClientEmployee_SP_Result> HIBOPGetClientEmployee_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress, Nullable<int> rowsPerPage, Nullable<int> pageNumber, ObjectParameter rowCount)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            var rowsPerPageParameter = rowsPerPage.HasValue ?
                new ObjectParameter("RowsPerPage", rowsPerPage) :
                new ObjectParameter("RowsPerPage", typeof(int));
    
            var pageNumberParameter = pageNumber.HasValue ?
                new ObjectParameter("PageNumber", pageNumber) :
                new ObjectParameter("PageNumber", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetClientEmployee_SP_Result>("HIBOPGetClientEmployee_SP", userParameter, lastSyncDateParameter, iPAddressParameter, rowsPerPageParameter, pageNumberParameter, rowCount);
        }
    
        public virtual ObjectResult<HIBOPGetEmployee_SP_Result> HIBOPGetEmployee_SP(string user)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetEmployee_SP_Result>("HIBOPGetEmployee_SP", userParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityList_SP_Result> HIBOPGetActivityList_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityList_SP_Result>("HIBOPGetActivityList_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetClientDetails_SP_Result> HIBOPGetClientDetails_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress, Nullable<int> rowsPerPage, Nullable<int> pageNumber, ObjectParameter rowCount)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            var rowsPerPageParameter = rowsPerPage.HasValue ?
                new ObjectParameter("RowsPerPage", rowsPerPage) :
                new ObjectParameter("RowsPerPage", typeof(int));
    
            var pageNumberParameter = pageNumber.HasValue ?
                new ObjectParameter("PageNumber", pageNumber) :
                new ObjectParameter("PageNumber", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetClientDetails_SP_Result>("HIBOPGetClientDetails_SP", userParameter, lastSyncDateParameter, iPAddressParameter, rowsPerPageParameter, pageNumberParameter, rowCount);
        }
    
        public virtual ObjectResult<HIBOPGetActivityDetails_SP_Result> HIBOPGetActivityDetails_SP(string employeeLookupCode, Nullable<System.DateTime> lastSyncDate, string iPAddress, Nullable<int> rowsPerPage, Nullable<int> pageNumber, ObjectParameter rowCount)
        {
            var employeeLookupCodeParameter = employeeLookupCode != null ?
                new ObjectParameter("EmployeeLookupCode", employeeLookupCode) :
                new ObjectParameter("EmployeeLookupCode", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            var rowsPerPageParameter = rowsPerPage.HasValue ?
                new ObjectParameter("RowsPerPage", rowsPerPage) :
                new ObjectParameter("RowsPerPage", typeof(int));
    
            var pageNumberParameter = pageNumber.HasValue ?
                new ObjectParameter("PageNumber", pageNumber) :
                new ObjectParameter("PageNumber", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityDetails_SP_Result>("HIBOPGetActivityDetails_SP", employeeLookupCodeParameter, lastSyncDateParameter, iPAddressParameter, rowsPerPageParameter, pageNumberParameter, rowCount);
        }
    
        public virtual ObjectResult<HIBOPGetActivityAccount_SP_Result> HIBOPGetActivityAccount_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress, Nullable<int> rowsPerPage, Nullable<int> pageNumber, ObjectParameter rowCount)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            var rowsPerPageParameter = rowsPerPage.HasValue ?
                new ObjectParameter("RowsPerPage", rowsPerPage) :
                new ObjectParameter("RowsPerPage", typeof(int));
    
            var pageNumberParameter = pageNumber.HasValue ?
                new ObjectParameter("PageNumber", pageNumber) :
                new ObjectParameter("PageNumber", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityAccount_SP_Result>("HIBOPGetActivityAccount_SP", userParameter, lastSyncDateParameter, iPAddressParameter, rowsPerPageParameter, pageNumberParameter, rowCount);
        }
    
        public virtual ObjectResult<HIBOPGetActivityEmployee_SP_Result> HIBOPGetActivityEmployee_SP(string employeeLookupCode, Nullable<System.DateTime> lastSyncDate, string iPAddress, Nullable<int> rowsPerPage, Nullable<int> pageNumber, ObjectParameter rowCount)
        {
            var employeeLookupCodeParameter = employeeLookupCode != null ?
                new ObjectParameter("EmployeeLookupCode", employeeLookupCode) :
                new ObjectParameter("EmployeeLookupCode", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            var rowsPerPageParameter = rowsPerPage.HasValue ?
                new ObjectParameter("RowsPerPage", rowsPerPage) :
                new ObjectParameter("RowsPerPage", typeof(int));
    
            var pageNumberParameter = pageNumber.HasValue ?
                new ObjectParameter("PageNumber", pageNumber) :
                new ObjectParameter("PageNumber", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityEmployee_SP_Result>("HIBOPGetActivityEmployee_SP", employeeLookupCodeParameter, lastSyncDateParameter, iPAddressParameter, rowsPerPageParameter, pageNumberParameter, rowCount);
        }
    
        public virtual ObjectResult<HIBOPGetActivityPolicy_SP_Result> HIBOPGetActivityPolicy_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress, Nullable<int> rowsPerPage, Nullable<int> pageNumber, ObjectParameter rowCount)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            var rowsPerPageParameter = rowsPerPage.HasValue ?
                new ObjectParameter("RowsPerPage", rowsPerPage) :
                new ObjectParameter("RowsPerPage", typeof(int));
    
            var pageNumberParameter = pageNumber.HasValue ?
                new ObjectParameter("PageNumber", pageNumber) :
                new ObjectParameter("PageNumber", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityPolicy_SP_Result>("HIBOPGetActivityPolicy_SP", userParameter, lastSyncDateParameter, iPAddressParameter, rowsPerPageParameter, pageNumberParameter, rowCount);
        }
    
        public virtual ObjectResult<HIBOPGetActivityClaim_SP_Result> HIBOPGetActivityClaim_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityClaim_SP_Result>("HIBOPGetActivityClaim_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityMarketing_SP_Result> HIBOPGetActivityMarketing_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityMarketing_SP_Result>("HIBOPGetActivityMarketing_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityServices_SP_Result> HIBOPGetActivityServices_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityServices_SP_Result>("HIBOPGetActivityServices_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityOpportunity_SP_Result> HIBOPGetActivityOpportunity_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityOpportunity_SP_Result>("HIBOPGetActivityOpportunity_SP", userParameter, lastSyncDateParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<HIBOPGetActivityLine_SP_Result> HIBOPGetActivityLine_SP(string user, Nullable<System.DateTime> lastSyncDate, string iPAddress, Nullable<int> rowsPerPage, Nullable<int> pageNumber, ObjectParameter rowCount)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var lastSyncDateParameter = lastSyncDate.HasValue ?
                new ObjectParameter("LastSyncDate", lastSyncDate) :
                new ObjectParameter("LastSyncDate", typeof(System.DateTime));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            var rowsPerPageParameter = rowsPerPage.HasValue ?
                new ObjectParameter("RowsPerPage", rowsPerPage) :
                new ObjectParameter("RowsPerPage", typeof(int));
    
            var pageNumberParameter = pageNumber.HasValue ?
                new ObjectParameter("PageNumber", pageNumber) :
                new ObjectParameter("PageNumber", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetActivityLine_SP_Result>("HIBOPGetActivityLine_SP", userParameter, lastSyncDateParameter, iPAddressParameter, rowsPerPageParameter, pageNumberParameter, rowCount);
        }
    
        public virtual ObjectResult<HIBOPSyncIntervalMinutes_Result> HIBOPSyncIntervalMinutes(ObjectParameter intervalMinutes, ObjectParameter clientActIntervalMinutes)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPSyncIntervalMinutes_Result>("HIBOPSyncIntervalMinutes", intervalMinutes, clientActIntervalMinutes);
        }
    
        public virtual ObjectResult<HIBOPGetDeltaSync_SP_Result> HIBOPGetDeltaSync_SP(string user, string iPAddress, Nullable<bool> is_Client, Nullable<bool> is_FirstSync)
        {
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            var is_ClientParameter = is_Client.HasValue ?
                new ObjectParameter("Is_Client", is_Client) :
                new ObjectParameter("Is_Client", typeof(bool));
    
            var is_FirstSyncParameter = is_FirstSync.HasValue ?
                new ObjectParameter("Is_FirstSync", is_FirstSync) :
                new ObjectParameter("Is_FirstSync", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HIBOPGetDeltaSync_SP_Result>("HIBOPGetDeltaSync_SP", userParameter, iPAddressParameter, is_ClientParameter, is_FirstSyncParameter);
        }
    }
}
