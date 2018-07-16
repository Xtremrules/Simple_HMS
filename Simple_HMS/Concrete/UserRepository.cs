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

        public IUser LoginUser(string username, string password)
        {
            using (var conn = new SqlConnection(_connString))
            {
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = "SELECT TOP 1 username FROM officer WHERE username = @username";
                cmd.Parameters.AddWithValue("@username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    User user = new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["username"].ToString()
                    };

                    /// The below code is from stackoverflow
                    /// https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129

                    string savedPasswordhash = reader.GetString(reader.GetOrdinal("passwordHash"));
                    byte[] hashBytes = Convert.FromBase64String(savedPasswordhash);
                    byte[] salt = new byte[16];
                    Array.Copy(hashBytes, 0, salt, 0, 16);
                    var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 1000);
                    byte[] hash = pbkdf2.GetBytes(20);
                    for (int i = 0; i < 20; i++)
                        if (hashBytes[i + 16] != hash[i])
                            throw new UnauthorizedAccessException("Username or password incorrect");
                    return user;
                }
            }
        }
    }
}