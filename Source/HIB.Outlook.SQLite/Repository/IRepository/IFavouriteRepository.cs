using HIB.Outlook.Model;
using System;
using System.Collections.Generic;


namespace HIB.Outlook.SQLite.Repository.IRepository
{
    public interface IFavouriteRepository
    {
        List<FavouriteInfo> GetFavouriteDetails(DateTime lastSyncDate);
    }
}
