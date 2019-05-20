using HIB.Outlook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.BAL.Repository.Interfaces
{
    public interface IFavouriteRepository
    {
        int SaveFavouriteDetails(List<FavouriteInfo> favouriteInfo);
    }
}
