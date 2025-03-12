using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;

namespace WebSanCauLong.Models
{
    public class DataModel
    {
        private String connectionString = "workstation id=WebSanCauLong.mssql.somee.com;packet size=4096;user id=baotrinh8724_SQLLogin_1;pwd=lei4q27j6c;data source=WebSanCauLong.mssql.somee.com;persist security info=False;initial catalog=WebSanCauLong;TrustServerCertificate=True";
        public ArrayList get (String sql)
        {
            ArrayList datalist = new ArrayList ();
            SqlConnection connection = new SqlConnection (connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open ();
            using (SqlDataReader r = command.ExecuteReader())
            {
                while (r.Read()) 
                {
                    ArrayList row = new ArrayList ();
                    for (int i = 0; i < r.FieldCount; i++) 
                    {
                        row.Add(r.GetValue(i).ToString());
                    }
                    datalist.Add(row);
                }
            }
            connection.Close();
            return datalist; 
        }
    }
}