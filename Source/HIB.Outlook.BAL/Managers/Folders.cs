using System;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository;
using HIB.Outlook.Model;
using System.Collections.Generic;
using HIB.Outlook.BAL.Repository.Interfaces;

namespace HIB.Outlook.BAL.Managers
{
    public class Folders : IFolders
    {

        IFolderRepository _folderRepository;

        public Folders(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }
        ///<summary>
        /// Get list of folder to sync local database
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<FolderInfo> SyncFolders(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            return _folderRepository.SyncFolders(userId, lastSyncDate,ipAddress);

        }
    }
}
