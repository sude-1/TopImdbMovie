using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace TopImdbMovie
{
    public class ImdbContext
    {
        private SqlConnection sqlConnection;
        public SqlConnection Connection()
        {
            if (sqlConnection == null)
                sqlConnection = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;DataBase=ImbdTop100;Trusted_Connection=true");
            if(sqlConnection.State==System.Data.ConnectionState.Closed)
                sqlConnection.Open();
            return sqlConnection;
        }
    }
}
