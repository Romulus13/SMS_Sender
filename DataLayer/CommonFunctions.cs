using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public static class CommonFunctions
    {
        public static string CheckReaderElementExists(SqlDataReader reader, string elemName)
        {

            if (String.IsNullOrEmpty(elemName) || reader == null || reader[elemName] == null || reader[elemName] == DBNull.Value)
                return null;
            return reader[elemName].ToString();
        }


    }
}
