using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Utility.Helper
{
    public class SqliteHelper
    {
        public static string _sQLiteDBPath = @"C:\Projects\Heffernan\Source\Trunk\Trunk\Source\Sqlite Database\HIBOutlook.db";

        public static SQLiteDataReader ExecuteSelectQuery(string selectQuery)
        {
            try
            {
                SQLiteConnection connect = new SQLiteConnection(string.Format("Data Source={0};Version=3;", _sQLiteDBPath));
                connect.Open();
                SQLiteCommand fmd = connect.CreateCommand();
                fmd.CommandText = selectQuery;
                fmd.CommandType = CommandType.Text;
                SQLiteDataReader sQLiteDataReader = fmd.ExecuteReader();
                return sQLiteDataReader;
            }
            catch (Exception ex)
            {
                return default(SQLiteDataReader);
            }
        }
    }
}
