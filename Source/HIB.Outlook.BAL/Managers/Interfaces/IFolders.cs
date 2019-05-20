using HIB.Outlook.Model;
using System;
using System.Collections.Generic;


namespace HIB.Outlook.BAL.Managers.Interfaces
{
    public interface IFolders
    {
        List<FolderInfo> SyncFolders(string userId, DateTime? lastSyncDate,string ipAddress);
    }
}
