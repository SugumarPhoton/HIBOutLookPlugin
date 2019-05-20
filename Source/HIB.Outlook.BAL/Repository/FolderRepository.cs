using System;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.DAL;
using System.Linq;
using HIB.Outlook.Model;
using System.Collections.Generic;
using log4net;
using System.Configuration;

namespace HIB.Outlook.BAL.Repository
{
    public class FolderRepository : IFolderRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FolderRepository));
        private readonly Int32 commandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
        ///<summary>
        /// Get list of folder to sync local database
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<FolderInfo> SyncFolders(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var folders = new List<FolderInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetFolders_SP(userId, lastSyncDate, ipAddress).ToList();
                    folders.AddRange(result.Select(f => new FolderInfo { FolderId = f.FolderId, ParentFolderId = f.ParentFolderId, FolderName = f.FolderName, FolderType = f.FolderType, InsertedDate = f.InsertedDate, UpdatedDate = f.UpdatedDate }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return folders;
        }
    }
}

