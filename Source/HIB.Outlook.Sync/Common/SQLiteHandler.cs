using System;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;
using HIB.Outlook.Model;
using HIB.Outlook.Helper.Common;

namespace HIB.Outlook.Sync.Common
{
    internal class SQLiteHandler
    {

        static string connectionString = ConfigurationManager.ConnectionStrings["HIBOutlookSQLite"].ConnectionString;

        static SQLiteConnection _connection = null;
        static string sql = "SELECT last_insert_rowid()";
        internal static SQLiteConnection Connection
        {
            get
            {
                if (_connection == null || _connection.State == ConnectionState.Closed && _connection.State != ConnectionState.Open)
                {
                    _connection = new SQLiteConnection(connectionString);
                    _connection.Open();
                }
                return _connection;
            }

        }

        ///<summary>
        /// select query for getting data from local database
        /// </summary>
        /// <returns>SQLiteDataReader</returns>
        internal static SQLiteDataReader ExecuteSelectQuery(string selectQuery)
        {
            var sQLiteDataReader = default(SQLiteDataReader);
            try
            {
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    fmd.CommandText = selectQuery;
                    fmd.CommandType = CommandType.Text;
                    sQLiteDataReader = fmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                //Logger.ErrorLog(selectQuery, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
            return sQLiteDataReader;
        }

        private static void logError(Exception ex)
        {
            var strpath = ConfigurationManager.AppSettings["FilePath"].ToString();
            FileStream fs = new FileStream(strpath, FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(strpath, true, Encoding.ASCII);
            sw.Write(ex.Message);
            sw.Close();
        }


        public static string DoQuotes(string sql)
        {
            if (sql == null)
                return "";
            else
                return sql.Replace("'", "''");
        }

        ///<summary>
        /// insert and update query which will be insert/update data in local database
        /// </summary>
        /// <returns>CustomeResponse</returns>
        internal static CustomeResponse ExecuteCreateOrInsertQuery(string createTableQuery)
        {
            var response = new CustomeResponse();
            try
            {
                using (SQLiteCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = createTableQuery;
                    cmd.CommandType = CommandType.Text;
                    var VALUE = cmd.ExecuteNonQuery(CommandBehavior.KeyInfo);
                    cmd.CommandText = sql;
                    response.RowId = Convert.ToInt64(cmd.ExecuteScalar());
                    response.Status = true;
                }

            }
            catch (Exception ex)
            {

                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                // FileLogger(ex.Message + createTableQuery);
                //Logger.ErrorLog(createTableQuery, Logger.SourceType.WindowsService, "");
                response.Status = false;
            }
            finally
            {
                Logger.save();
            }
            return response;
        }
        //public static void FileLogger(string text)
        //{
        //    try
        //    {
        //        StreamWriter sw = new StreamWriter(@"C:\HG IT Services\Test.txt");
        //        sw.WriteLine(text);
        //        sw.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
        //    }
        //}


        public static DataTable ExecuteSelecttQueryWithAdapter(string selectQuery)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteDataAdapter sQLiteDataReader = new SQLiteDataAdapter(selectQuery, Connection))
                {
                    sQLiteDataReader.Fill(dt);
                    Connection.Close();
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                //Logger.ErrorLog(selectQuery, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
            return dt;
        }


        ///<summary>
        /// Check the object is null or not with generic type
        /// </summary>
        /// <returns>T</returns>
        public static T CheckNull<T>(object obj)
        {
            return (obj == DBNull.Value ? default(T) : (T)obj);
        }
    }
}
