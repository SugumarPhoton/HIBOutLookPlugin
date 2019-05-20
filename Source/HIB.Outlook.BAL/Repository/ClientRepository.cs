using System;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.DAL;
using System.Linq;
using HIB.Outlook.Model;
using System.Collections.Generic;
using log4net;
using System.Data.Entity.Core.Objects;
using System.Configuration;

namespace HIB.Outlook.BAL.Repository
{
    public class ClientRepository : IClientRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ClientRepository));
        private readonly Int32 commandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
        /// <summary>
        /// Get List of user client detail to sync local Database
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public ClientDetail SyncClients(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            var clientDetail = new ClientDetail();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var clients = new List<ClientInfo>();
                    ObjectParameter outputRowCount = new ObjectParameter("RowCount", typeof(long));
                    var result = context.HIBOPGetClientDetails_SP(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber, outputRowCount).ToList();
                    clients.AddRange(result.Select(a => new ClientInfo { EnitityId = a.UniqEntity, LookupCode = a.LookupCode, Nameof = a.NameOf, Address = a.Address, City = a.City, StateName = a.StateName, PostalCode = a.PostalCode, Country = a.Country, InsertedDate = a.InsertedDate, UpdatedDate = a.UpdatedDate, PrimaryContactName = a.PrimaryContactName, Status = a.Status, AgencyCode = a.AgencyCode, AgencyName = a.AgencyName, CountryCode = a.CountryCode, StateCode = a.StateCode }));
                    clientDetail.Clients = clients;
                    clientDetail.RowCount = Convert.ToInt64(outputRowCount.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return clientDetail;
        }

        /// <summary>
        /// Get Sync Interval Count From Database
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public IntervalTimeDetail SyncIntervalTime()
        {
            IntervalTimeDetail intervalTimeDetail = new IntervalTimeDetail();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    ObjectParameter outputRowCount = new ObjectParameter("IntervalMinutes", typeof(long));
                    ObjectParameter clientActIntervalMinutes = new ObjectParameter("ClientActIntervalMinutes", typeof(long));
                    var result = context.HIBOPSyncIntervalMinutes(outputRowCount, clientActIntervalMinutes).ToList();
                    intervalTimeDetail.IntervalMinutes = Convert.ToInt64(outputRowCount.Value);
                    intervalTimeDetail.ClientActivityIntervalMinutes = Convert.ToInt64(clientActIntervalMinutes.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return intervalTimeDetail;
        }


        /// <summary>
        /// Get Sync Interval Count From Database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="IPAddress"></param>
        /// <param name="isClient"></param>
        /// <returns></returns>
        public List<DeltaSyncObjectInfo> GetDeltaSyncObjectDetail(string userId, string IPAddress, bool? isClient, bool? isFirstSync)
        {
            List<DeltaSyncObjectInfo> deltaSyncObjectInfos = new List<DeltaSyncObjectInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetDeltaSync_SP(userId, IPAddress, isClient, isFirstSync).ToList();
                    deltaSyncObjectInfos.AddRange(result.Select(m => new DeltaSyncObjectInfo() { UserLookupCode = m.UserLookupCode, IPAddress = m.IPAddress, IsDeltaFlag = m.IsDeltaFlag, LastSyncDate = m.LastSyncDate, SpName = m.SpName }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return deltaSyncObjectInfos;
        }


        /// <summary>
        /// Get list of client employee detail to sync local database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public ClientEmployeeDetail SyncClientEmployee(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            var clientEmployeeDetail = new ClientEmployeeDetail();
            try
            {

                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var clientEmployee = new List<ClientEmployee>();
                    ObjectParameter outputRowCount = new ObjectParameter("RowCount", typeof(long));
                    var result = context.HIBOPGetClientEmployee_SP(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber, outputRowCount).ToList();
                    clientEmployee.AddRange(result.Select(a => new ClientEmployee { EmployeeLookupcode = a.EmployeeLookupcode, ClientId = a.ClientId, UserId = a.UserId }));
                    clientEmployeeDetail.ClientEmployees = clientEmployee;
                    clientEmployeeDetail.RowCount = Convert.ToInt64(outputRowCount.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return clientEmployeeDetail;
        }
    }
}
