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

        public List<San> GetDanhSachSan()
        {
            List<San> danhSachSan = new List<San>();
            string sql = "SELECT * FROM San";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSachSan.Add(new San
                        {
                            SanID = Convert.ToInt32(reader["SanID"]),
                            TenSan = reader["TenSan"].ToString(),
                            DiaChi = reader["DiaChi"].ToString(),
                            LoaiSan = reader["LoaiSan"].ToString(),
                            GiaSan = Convert.ToDecimal(reader["GiaSan"]),
                            MoTa = reader["MoTa"].ToString(),
                            TrangThai = reader["TrangThai"].ToString(),
                            HinhAnh = reader["HinhAnh"].ToString()
                        });
                    }
                }
            }
            return danhSachSan;
        }
        public San GetSanById(int sanId)
        {
            San san = null;
            string sql = "SELECT * FROM San WHERE SanID = @SanID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@SanID", sanId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        san = new San
                        {
                            SanID = Convert.ToInt32(reader["SanID"]),
                            TenSan = reader["TenSan"].ToString(),
                            DiaChi = reader["DiaChi"].ToString(),
                            LoaiSan = reader["LoaiSan"].ToString(),
                            GiaSan = Convert.ToDecimal(reader["GiaSan"]),
                            MoTa = reader["MoTa"].ToString(),
                            TrangThai = reader["TrangThai"].ToString(),
                            HinhAnh = reader["HinhAnh"].ToString()
                        };
                    }
                }
            }
            return san;
        }
        public bool ThemSan(San san)
        {
            string sql = "INSERT INTO San (TenSan, DiaChi, LoaiSan, GiaSan, MoTa, TrangThai, HinhAnh) " +
                         "VALUES (@TenSan, @DiaChi, @LoaiSan, @GiaSan, @MoTa, @TrangThai, @HinhAnh)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@TenSan", san.TenSan);
                command.Parameters.AddWithValue("@DiaChi", san.DiaChi);
                command.Parameters.AddWithValue("@LoaiSan", san.LoaiSan);
                command.Parameters.AddWithValue("@GiaSan", san.GiaSan);
                command.Parameters.AddWithValue("@MoTa", (object)san.MoTa ?? DBNull.Value);
                command.Parameters.AddWithValue("@TrangThai", san.TrangThai);
                command.Parameters.AddWithValue("@HinhAnh", (object)san.HinhAnh ?? DBNull.Value);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public bool CapNhatSan(San san)
        {
            string sql = "UPDATE San SET TenSan = @TenSan, DiaChi = @DiaChi, LoaiSan = @LoaiSan, " +
                         "GiaSan = @GiaSan, MoTa = @MoTa, TrangThai = @TrangThai, HinhAnh = @HinhAnh " +
                         "WHERE SanID = @SanID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@SanID", san.SanID);
                command.Parameters.AddWithValue("@TenSan", san.TenSan);
                command.Parameters.AddWithValue("@DiaChi", san.DiaChi);
                command.Parameters.AddWithValue("@LoaiSan", san.LoaiSan);
                command.Parameters.AddWithValue("@GiaSan", san.GiaSan);
                command.Parameters.AddWithValue("@MoTa", (object)san.MoTa ?? DBNull.Value);
                command.Parameters.AddWithValue("@TrangThai", san.TrangThai);
                command.Parameters.AddWithValue("@HinhAnh", (object)san.HinhAnh ?? DBNull.Value);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public bool XoaSan(int sanId)
        {
            string sql = "DELETE FROM San WHERE SanID = @SanID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@SanID", sanId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        // Lấy danh sách các sân còn trống trong khoảng thời gian mong muốn
        public List<San> GetSanTrong(DateTime ngay, TimeSpan gioBatDau, TimeSpan gioKetThuc)
        {
            List<San> danhSachSanTrong = new List<San>();

            string sql = @"SELECT * FROM San WHERE SanID NOT IN (
                                SELECT SanID FROM LichSan 
                                WHERE Ngay = @Ngay 
                                AND ((@GioBatDau < GioKetThuc AND @GioKetThuc > GioBatDau) 
                                     OR (GioBatDau <= @GioBatDau AND GioKetThuc > @GioBatDau) 
                                     OR (GioBatDau < @GioKetThuc AND GioKetThuc >= @GioKetThuc)))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Ngay", ngay);
                command.Parameters.AddWithValue("@GioBatDau", gioBatDau);
                command.Parameters.AddWithValue("@GioKetThuc", gioKetThuc);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSachSanTrong.Add(new San
                        {
                            SanID = Convert.ToInt32(reader["SanID"]),
                            TenSan = reader["TenSan"].ToString(),
                            DiaChi = reader["DiaChi"].ToString(),
                            LoaiSan = reader["LoaiSan"].ToString(),
                            GiaSan = Convert.ToDecimal(reader["GiaSan"]),
                            MoTa = reader["MoTa"].ToString(),
                            TrangThai = reader["TrangThai"].ToString(),
                            HinhAnh = reader["HinhAnh"].ToString()
                        });
                    }
                }
            }
            return danhSachSanTrong;
        }

        // Đặt sân nếu sân còn trống
        public bool DatSan(DatSan datSan)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO DatSan (KhachHangID, SanID, ThoiGianBatDau, ThoiGianKetThuc, TongTien, TrangThai)
                               VALUES (@KhachHangID, @SanID, @ThoiGianBatDau, @ThoiGianKetThuc, @TongTien, 'Đang chờ')";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@KhachHangID", datSan.KhachHangID);
                command.Parameters.AddWithValue("@SanID", datSan.SanID);
                command.Parameters.AddWithValue("@ThoiGianBatDau", datSan.ThoiGianBatDau);
                command.Parameters.AddWithValue("@ThoiGianKetThuc", datSan.ThoiGianKetThuc);
                command.Parameters.AddWithValue("@TongTien", datSan.TongTien);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool HuyDatSan(int datSanID)
        {
            string sql = "UPDATE DatSan SET TrangThai = 'Đã hủy' WHERE DatSanID = @DatSanID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DatSanID", datSanID);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public DatSan GetDatSanById(int datSanID)
        {
            DatSan datSan = null;
            string sql = "SELECT * FROM DatSan INNER JOIN San ON DatSan.SanID = San.SanID WHERE DatSanID = @DatSanID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DatSanID", datSanID);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        datSan = new DatSan
                        {
                            DatSanID = Convert.ToInt32(reader["DatSanID"]),
                            KhachHangID = Convert.ToInt32(reader["KhachHangID"]),
                            SanID = Convert.ToInt32(reader["SanID"]),
                            ThoiGianBatDau = Convert.ToDateTime(reader["ThoiGianBatDau"]),
                            ThoiGianKetThuc = Convert.ToDateTime(reader["ThoiGianKetThuc"]),
                            TongTien = Convert.ToDecimal(reader["TongTien"]),
                            TrangThai = reader["TrangThai"].ToString(),
                            San = new San
                            {
                                SanID = Convert.ToInt32(reader["SanID"]),
                                TenSan = reader["TenSan"].ToString(),
                                DiaChi = reader["DiaChi"].ToString()
                            }
                        };
                    }
                }
            }
            return datSan;
        }
        public List<DatSan> GetLichSuDatSan(int khachHangID)
        {
            List<DatSan> danhSachDatSan = new List<DatSan>();
            string sql = @"SELECT d.*, s.TenSan 
                   FROM DatSan d
                   INNER JOIN San s ON d.SanID = s.SanID
                   WHERE d.KhachHangID = @KhachHangID
                   ORDER BY d.ThoiGianBatDau DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@KhachHangID", khachHangID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    danhSachDatSan.Add(new DatSan
                    {
                        DatSanID = Convert.ToInt32(reader["DatSanID"]),
                        KhachHangID = Convert.ToInt32(reader["KhachHangID"]),
                        SanID = Convert.ToInt32(reader["SanID"]),
                        ThoiGianBatDau = Convert.ToDateTime(reader["ThoiGianBatDau"]),
                        ThoiGianKetThuc = Convert.ToDateTime(reader["ThoiGianKetThuc"]),
                        TongTien = Convert.ToDecimal(reader["TongTien"]),
                        TrangThai = reader["TrangThai"].ToString(),
                        San = new San
                        {
                            TenSan = reader["TenSan"].ToString()
                        }   
                    });
                }
            }
            return danhSachDatSan;
        }
    }
}