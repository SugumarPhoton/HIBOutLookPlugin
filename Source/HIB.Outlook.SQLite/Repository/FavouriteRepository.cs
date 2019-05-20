using HIB.Outlook.Helper.Common;
using HIB.Outlook.Model;
using HIB.Outlook.SQLite.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HIB.Outlook.SQLite.Repository
{
    public class CommonProperties
    {
        public static string LocalIPAddress()
        {
            //Thread.Sleep(10000000);
            string ipAddress = "";
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = addr.ToString();
                }
            }
            
            return ipAddress;
        }
    }
    public class FavouriteRepository : IFavouriteRepository
    {
        #region Private Fields

        #endregion
        #region Public Members
        /// <summary>
        ///  Get Favourate Details from Local Database to Centralized
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<FavouriteInfo> GetFavouriteDetails(DateTime lastSyncDate)
        {
            var ipAddress = CommonProperties.LocalIPAddress();
            var favouriteList = new List<FavouriteInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    var lastSyncDatetime = lastSyncDate;
                    var result = context.HIBOPFavourites.Where(d => d.CreatedDate >= lastSyncDatetime).ToList();
                    favouriteList.AddRange(result.Select(f => new FavouriteInfo { FavourtieName = f.FavouriteName, UniqEmployee = f.UniqEmployee, UniqEntity = f.UniqEntity, UniqActivity = f.UniqActivity ?? default(long), PolicyYear = f.PolicyYear, PolicyType = f.PolicyType, DescriptionType = f.DescriptionType, Description = f.Description, FolderId = f.FolderId ?? default(long), SubFolder1Id = f.SubFolder1Id ?? default(long), SubFolder2Id = f.SubFolder2Id ?? default(long), ClientAccessibleDate = Convert.ToString(f.ClientAccessibleDate), CreatedDate = Convert.ToString(f.CreatedDate), IPAddress = ipAddress }));
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
            return favouriteList;
        }
        #endregion
    }
}
