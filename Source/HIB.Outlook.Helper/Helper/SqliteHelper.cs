using HIB.Outlook.Helper.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Helper.Helper
{
    public class SqliteHelper
    {

        internal static string connectionString = ConfigurationManager.ConnectionStrings["HIBOutlookSQLite"].ConnectionString;
        internal static SQLiteConnection _connection = null;
        public static SQLiteConnection Connection
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

        public static DataTable ExecuteSelecttQueryWithAdapter(string selectQuery)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SQLiteDataAdapter sQLiteDataReader = new SQLiteDataAdapter(selectQuery, Connection))
                {
                    sQLiteDataReader.Fill(dt);
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
            return dt;
        }

        public static SQLiteDataReader ExecuteSelectQuery(string selectQuery)
        {
            try
            {
                SQLiteDataReader sQLiteDataReader = null;
                using (SQLiteCommand fmd = Connection.CreateCommand())
                {
                    fmd.CommandText = selectQuery;
                    fmd.CommandType = CommandType.Text;
                    sQLiteDataReader = fmd.ExecuteReader();
                    //Connection.Close();
                }
                return sQLiteDataReader;

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                return default(SQLiteDataReader);
            }
            finally
            {
                Logger.save();
            }
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

        public static bool ExecuteCreateOrInsertQuery(string createTableQuery)
        {
            try
            {
                var result = false;

                using (SQLiteCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = createTableQuery;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    result = true;
                    Connection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                //  FileLogger(ex.Message + createTableQuery);
                return false;
            }
            finally
            {
                Logger.save();
            }
        }
    }
}
