using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using WebSanCauLong.Models.ViewModels;

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
        // Đăng ký khách hàng mới
        public bool DangKy(KhachHang khachHang)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO KhachHang (HoTen, Email, SoDienThoai, MatKhau, DiaChi, NgaySinh, GioiTinh) VALUES (@HoTen, @Email, @SoDienThoai, @MatKhau, @DiaChi, @NgaySinh, @GioiTinh)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@HoTen", khachHang.HoTen);
                command.Parameters.AddWithValue("@Email", khachHang.Email);
                command.Parameters.AddWithValue("@SoDienThoai", khachHang.SoDienThoai);
                command.Parameters.AddWithValue("@MatKhau", khachHang.MatKhau);
                command.Parameters.AddWithValue("@DiaChi", (object)khachHang.DiaChi ?? DBNull.Value);
                command.Parameters.AddWithValue("@NgaySinh", (object)khachHang.NgaySinh ?? DBNull.Value);
                command.Parameters.AddWithValue("@GioiTinh", (object)khachHang.GioiTinh ?? DBNull.Value);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        // Đăng nhập khách hàng
        public KhachHang DangNhap(string email, string matKhau)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM KhachHang WHERE Email = @Email AND MatKhau = @MatKhau";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@MatKhau", matKhau);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new KhachHang
                        {
                            KhachHangID = Convert.ToInt32(reader["KhachHangID"]),
                            HoTen = reader["HoTen"].ToString(),
                            Email = reader["Email"].ToString(),
                            SoDienThoai = reader["SoDienThoai"].ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}