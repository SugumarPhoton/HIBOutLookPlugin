using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.Model;
using System;
using System.Collections.Generic;

namespace HIB.Outlook.BAL.Managers
{
    public class Clients : IClients
    {
        IClientRepository _clientRepository;
        public Clients(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// Get List of user client detail to sync local Database
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public ClientDetail SyncClients(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            return _clientRepository.SyncClients(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber);
        }

        public IntervalTimeDetail SyncIntervalTime()
        {
            return _clientRepository.SyncIntervalTime();
        }

        public List<DeltaSyncObjectInfo> GetDeltaSyncObjectDetail(string userId, string IPAddress, bool? isClient, bool? isFirstSync)
        {
            return _clientRepository.GetDeltaSyncObjectDetail(userId, IPAddress, isClient, isFirstSync);
        }
        public ClientEmployeeDetail SyncClientEmployee(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber)
        {
            return _clientRepository.SyncClientEmployee(userId, lastSyncDate, ipAddress, rowsPerPage, PageNumber);
        }
    }
}
