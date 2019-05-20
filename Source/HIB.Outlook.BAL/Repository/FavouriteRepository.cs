using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.DAL;
using HIB.Outlook.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace HIB.Outlook.BAL.Repository
{
    public class FavouriteRepository : IFavouriteRepository
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(FavouriteRepository));
        private readonly Int32 commandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
        ///<summary>
        ///Save Favourite Details from local to Sync centralized database
        /// </summary>
        /// <param name="errorlogInfo"></param>
        /// <returns></returns>
        public int SaveFavouriteDetails(List<FavouriteInfo> favouriteInfo)
        {
            int result = -1;
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    SqlParameter HIBOPFavorite = new SqlParameter("HIBOPFavorite", SqlDbType.Structured) { Value = ConvertfavouriteListToDatatableUT(favouriteInfo), TypeName = "dbo.HIBOPFavourite_UT" };
                    context.Database.ExecuteSqlCommand("exec HIBOPSyncFavouriteToCentralized_SP @HIBOPFavorite", HIBOPFavorite);
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
            return result;
        }
        ///<summary>
        // Convert from List to datatable for favourite Details 
        /// </summary>
        /// <param name="favouriteInfo"></param>
        /// <returns></returns>
        private DataTable ConvertfavouriteListToDatatableUT(List<FavouriteInfo> favouriteInfo)
        {
            DataTable favouriteInfoUtTable = new DataTable("HIBOPFavourite_UT");
            try
            {
                favouriteInfoUtTable.Columns.Add("UniqEmployee", typeof(string));
                favouriteInfoUtTable.Columns.Add("FavName", typeof(string));
                favouriteInfoUtTable.Columns.Add("UniqEntity", typeof(int));
                favouriteInfoUtTable.Columns.Add("UniqActivity", typeof(int));
                favouriteInfoUtTable.Columns.Add("PolicyYear", typeof(string));
                favouriteInfoUtTable.Columns.Add("PolicyType", typeof(string));
                favouriteInfoUtTable.Columns.Add("DescriptionType", typeof(string));
                favouriteInfoUtTable.Columns.Add("Description", typeof(string));
                favouriteInfoUtTable.Columns.Add("FolderId", typeof(int));
                favouriteInfoUtTable.Columns.Add("SubFolder1Id", typeof(int));
                favouriteInfoUtTable.Columns.Add("SubFolder2Id", typeof(int));
                favouriteInfoUtTable.Columns.Add("ClientAccessibleDate", typeof(string));
                favouriteInfoUtTable.Columns.Add("InsertedDate", typeof(string));
                favouriteInfoUtTable.Columns.Add("UserLookupCode", typeof(string));
                favouriteInfoUtTable.Columns.Add("IPAddress", typeof(string));


                if (favouriteInfo != null && favouriteInfo.Any())
                {
                    foreach (var l in favouriteInfo)
                    {
                        DataRow row = favouriteInfoUtTable.NewRow();
                        row["UniqEmployee"] = l.UniqEmployee;
                        row["FavName"] = l.FavourtieName;
                        row["UniqEntity"] = l.UniqEntity;
                        row["UniqActivity"] = l.UniqActivity;
                        row["PolicyYear"] = l.PolicyYear;
                        row["PolicyType"] = l.PolicyType;
                        row["DescriptionType"] = l.DescriptionType;
                        row["Description"] = l.Description;
                        row["FolderId"] = l.FolderId;
                        row["SubFolder1Id"] = l.SubFolder1Id;
                        row["SubFolder2Id"] = l.SubFolder2Id;                    
                        row["ClientAccessibleDate"] = l.ClientAccessibleDate;
                        row["InsertedDate"] = l.CreatedDate;
                        row["IPAddress"] = l.IPAddress;
                        favouriteInfoUtTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return favouriteInfoUtTable;
        }


    }
}
