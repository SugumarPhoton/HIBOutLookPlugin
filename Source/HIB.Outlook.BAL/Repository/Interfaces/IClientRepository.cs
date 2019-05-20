using HIB.Outlook.Model;
using System;
using System.Collections.Generic;


namespace HIB.Outlook.BAL.Repository.Interfaces
{
    public interface IClientRepository
    {
        ClientDetail SyncClients(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber);
        ClientEmployeeDetail SyncClientEmployee(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber);
        List<DeltaSyncObjectInfo> GetDeltaSyncObjectDetail(string userId, string IPAddress, bool? isClient,bool? isFirstSync);
        IntervalTimeDetail SyncIntervalTime();
    }
}
