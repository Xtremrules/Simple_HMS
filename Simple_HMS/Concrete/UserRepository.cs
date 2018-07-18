using Simple_HMS.Entity;
using Simple_HMS.Interface;
using System;
using System.Data.SqlClient;

namespace Simple_HMS.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connString;
        public UserRepository(string connString)
        {
            _connString = connString;
        }

        public IUser GetUser(string username)
        {
            using (var conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = "SELECT TOP 1 * FROM officer WHERE username = @username";
                cmd.Parameters.AddWithValue("@username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return default(User);
                    }

                    User user = new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["username"].ToString(),
                        Role = reader["role"].ToString(),
                        passwordHash = reader["passwordHash"].ToString()
                    };

                    return user;
                }
            }
        }
    }
}