using System.Data;
using System.Data.SqlClient;

namespace PersonnelManagementSystem
{
    public class DataAccess
    {
        private string connectionString = ConfigurationHelper.GetConnectionString("LocalDb1");
        // 如果需要用户名密码连接，请改为: "Server=YOUR_SERVER;Database=MyDb;User Id=xxx;Password=xxx;"

        public bool ValidateUser(string username, string password, out string role, out bool isActive)
        {
            role = null;
            isActive = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT Role, PasswordHash, IsActive FROM Users WHERE Username=@u";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string dbHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                            role = reader.GetString(reader.GetOrdinal("Role"));
                            isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));

                            // 简化示例：直接比较明文密码。请改为实际哈希验证。
                            if (dbHash == password && isActive)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool IsInEntryUsers(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM EntryUsers WHERE Username=@u";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool IsInViewUsers(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM ViewUsers WHERE Username=@u";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public void AddUser(string username, string password, string role)
        {
            // 新增用户到Users表
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@u,@p,@r)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password); // 请在实际中使用加密后的password
                    cmd.Parameters.AddWithValue("@r", role);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // 先删除子表记录
                string sqlDelEntry = "DELETE FROM EntryUsers WHERE Username=@u";
                using (SqlCommand cmd = new SqlCommand(sqlDelEntry, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.ExecuteNonQuery();
                }

                string sqlDelView = "DELETE FROM ViewUsers WHERE Username=@u";
                using (SqlCommand cmd = new SqlCommand(sqlDelView, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.ExecuteNonQuery();
                }

                // 再删除主表记录
                string sqlDelUser = "DELETE FROM Users WHERE Username=@u";
                using (SqlCommand cmd = new SqlCommand(sqlDelUser, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddToEntryUsers(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO EntryUsers (Username) VALUES (@u)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddToViewUsers(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO ViewUsers (Username) VALUES (@u)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT UserID, Username, Role, IsActive FROM Users";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // 可按需添加修改密码、禁用用户、查询数据表等功能方法
    }
}