using HIB.Outlook.Model;
using System.Collections.Generic;


namespace HIB.Outlook.BAL.Managers.Interfaces
{
    public interface IFavourites
    {
        int SaveFavouriteDetails(List<FavouriteInfo> favouriteInfo);
    }
}
