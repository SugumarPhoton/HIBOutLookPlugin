using HIB.Outlook.Model;
using System.Collections.Generic;


namespace HIB.Outlook.SQLite.Repository.IRepository
{
    public interface IFolderRepository
    {
        ResultInfo SyncDBFolder(List<FolderInfo> folderList);
    }
}
