using HIB.Outlook.Helper.Common;
using HIB.Outlook.Helper.Helper;
using HIB.Outlook.Model;
using HIB.Outlook.SQLite.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;


namespace HIB.Outlook.SQLite.Repository
{
    public class ClientRepository : IClientRepository
    {

        #region Public Members
        /// <summary>
        /// Sync client details to local sqlite DB
        /// </summary>
        /// <param name="clientList"></param>
        /// <returns></returns>   
        public ResultInfo SyncClient(List<ClientInfo> clientList)
        {
            var resultInfo = new ResultInfo();
            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var C in clientList)
                        {
                            try
                            {
                                cmd.CommandText = "INSERT OR REPLACE INTO HIBOPClient(UniqEntity,LookupCode,NameOf,Address,PrimaryContactName,Status,City,StateCode,StateName,PostalCode,Country,CountryCode,InsertedDate,UpdatedDate,AgencyCode,AgencyName) VALUES ('" + C.EnitityId + "', '" + C.LookupCode?.Replace("'", "''") + "','" + C.Nameof?.Replace("'", "''") + "','" + C.Address?.Replace("'", "''") + "','" + C.PrimaryContactName?.Replace("'", "''") + "','" + C.Status?.Replace("'", "''") + "','" + C.City?.Replace("'", "''") + "','" + C.StateCode?.Replace("'", "''") + "','" + C.StateName?.Replace("'", "''") + "','" + C.PostalCode?.Replace("'", "''") + "','" + C.Country?.Replace("'", "''") + "','" + C.CountryCode?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(C.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(C.UpdatedDate) + "','" + C.AgencyCode?.Replace("'", "''") + "','" + C.AgencyName?.Replace("'", "''") + "')";
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                            }
                        }
                        resultInfo.IsSuccess = true;
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }

            return resultInfo;
        }


        /// <summary>
        /// Sync client details to local sqlite DB
        /// </summary>
        /// <param name="clientEmployee"></param>
        /// <returns></returns>  
        public ResultInfo SyncClientEmployee(List<ClientEmployee> clientEmployeeList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var a in clientEmployeeList)
                        {
                            try
                            {
                                if (a != null)
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPClientEmployee(UserId,UniqEntity,EmployeeLookupcode) VALUES ('" + a.UserId + "','" + a.ClientId + "', '" + a.EmployeeLookupcode + "')";
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                            }

                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }

            return resultInfo;
        }

        #endregion
    }
}
