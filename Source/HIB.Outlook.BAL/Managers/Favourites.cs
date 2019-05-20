using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.Model;
using System.Collections.Generic;


namespace HIB.Outlook.BAL.Managers
{
    public class Favourites : IFavourites
    {
        IFavouriteRepository _favouriteRepository;
        public Favourites(IFavouriteRepository favouriteRepository)
        {
            _favouriteRepository = favouriteRepository;
        }
        public int SaveFavouriteDetails(List<FavouriteInfo> favouriteInfo)
        {
            return _favouriteRepository.SaveFavouriteDetails(favouriteInfo);
        }
    }
}
