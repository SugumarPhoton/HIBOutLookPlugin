using System;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;
using HIB.Outlook.Model;
using HIB.Outlook.Helper.Common;
using AttachmentBridge;

namespace HIB.Outlook.SQLite
{
    internal class SQLiteHandler
    {

        internal static string connectionString = ConfigurationManager.ConnectionStrings["HIBOutlookSQLite"].ConnectionString;

        internal static SQLiteConnection _connection = null;
        static string sql = "SELECT last_insert_rowid()";
        internal static SQLiteConnection Connection
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

        ///<summary>
        /// select query for getting data from local database
        /// </summary>
        /// <returns>SQLiteDataReader</returns>
        internal static SQLiteDataReader ExecuteSelectQuery(string selectQuery)
        {
            try
            {
                SQLiteCommand fmd = Connection.CreateCommand();
                fmd.CommandText = selectQuery;
                fmd.CommandType = CommandType.Text;
                SQLiteDataReader sQLiteDataReader = fmd.ExecuteReader();
                return sQLiteDataReader;
            }
            catch (Exception ex)
            {
                //Logger.ErrorLog(selectQuery, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                // logError(ex);
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                return default(SQLiteDataReader);
            }
            finally
            {
                Logger.save();
            }
        }

        //private static void logError(Exception ex)
        //{
        //    var strpath = ConfigurationManager.AppSettings["FilePath"].ToString();
        //    FileStream fs = new FileStream(strpath, FileMode.Append, FileAccess.Write, FileShare.Write);
        //    fs.Close();
        //    StreamWriter sw = new StreamWriter(strpath, true, Encoding.ASCII);
        //    sw.Write(ex.Message);
        //    sw.Close();
        //}

        private static void LogError(string ex)
        {
            var strpath = ConfigurationManager.AppSettings["FilePath"].ToString();
            using (FileStream fs = new FileStream(strpath, FileMode.Append, FileAccess.Write, FileShare.Write))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(ex);
                sw.Close();
            }

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
                SQLiteCommand cmd = Connection.CreateCommand();
                cmd.CommandText = createTableQuery;
                cmd.CommandType = CommandType.Text;
                var VALUE = cmd.ExecuteNonQuery(CommandBehavior.KeyInfo);
                cmd.CommandText = sql;
                response.RowId = Convert.ToInt64(cmd.ExecuteScalar());
                response.Status = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                //Logger.ErrorLog(createTableQuery, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                response.Status = false;
            }
            finally
            {
                Logger.save();
            }
            return response;
        }


        public static DataTable ExecuteSelecttQueryWithAdapter(string selectQuery)
        {
            DataTable dt = new DataTable();
            try
            {
                SQLiteDataAdapter sQLiteDataReader = new SQLiteDataAdapter(selectQuery, Connection);
                sQLiteDataReader?.Fill(dt);
                Connection?.Close();

            }
            catch (Exception ex)
            {
                // Logger.ErrorLog(selectQuery, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
                Logger.ErrorLog(ex, Logger.SourceType.AddIn, ThisAddIn.EmployeeLookupCode);
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
