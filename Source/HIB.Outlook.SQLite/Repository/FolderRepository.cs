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
    public class FolderRepository : IFolderRepository
    {
        #region Private Fields

        string connectionString = ConfigurationManager.ConnectionStrings["HIBOutlookSQLite"].ConnectionString;
        SQLiteConnection _connection = null;
        SQLiteConnection Connection
        {
            get
            {
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {

                    _connection = new SQLiteConnection(connectionString);
                    _connection.Open();
                }

                return _connection;
            }

        }

        #endregion

        #region Public Members
        /// <summary>
        /// Save Folder detail to local Database
        /// </summary>
        /// <param name="folderInfo"></param>
        /// <returns></returns>      
        public ResultInfo SyncDBFolder(List<FolderInfo> folderList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var F in folderList)
                        {
                            if (F != null)
                            {
                                try
                                {
                                    cmd.CommandText = "INSERT OR REPLACE INTO HIBOPFolderAttachment(FolderId,ParentFolderId,FolderName,FolderType,InsertedDate,UpdatedDate) VALUES ('" + F.FolderId + "', '" + F.ParentFolderId + "','" + F.FolderName?.Replace("'", "''") + "','" + F.FolderType?.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(F.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(F.UpdatedDate) + "')";
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                                }
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
        #endregion
    }
}
