using HIB.Outlook.Model;
using System;
using System.Collections.Generic;

namespace HIB.Outlook.BAL.Repository.Interfaces
{
    public interface IFolderRepository
    {
        List<FolderInfo> SyncFolders(string userId, DateTime? lastSyncDate, string ipAddress);
    }
}
