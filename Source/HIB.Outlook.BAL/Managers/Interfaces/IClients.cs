using HIB.Outlook.Model;
using System;
using System.Collections.Generic;


namespace HIB.Outlook.BAL.Managers.Interfaces
{
    public interface IClients
    {
        ClientDetail SyncClients(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber);
        ClientEmployeeDetail SyncClientEmployee(string userId, DateTime? lastSyncDate, string ipAddress, int? rowsPerPage, int? PageNumber);
        IntervalTimeDetail SyncIntervalTime();
        List<DeltaSyncObjectInfo> GetDeltaSyncObjectDetail(string userId, string IPAddress, bool? isClient, bool? isFirstSync);
    }
}
