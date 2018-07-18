using Simple_HMS.Interface;
using System;
using System.Security.Principal;

namespace Simple_HMS.Security
{
    public class HMSIdentity : IIdentity
    {
        private string nameValue;
        private bool authenticatedValue;
        private string roleValue;

        public HMSIdentity(IUser user, string password)
        {
            if (IsValidNameAndPassword(user, password))
            {
                nameValue = user.Name;
                authenticatedValue = true;
                roleValue = user.Role;
            }
            else
            {
                nameValue = "";
                authenticatedValue = false;
                roleValue = null;
            }
        }

        private bool IsValidNameAndPassword(IUser user, string password)
        {
            /// The below code is from stackoverflow
            /// https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129
            #region From Stackoverflow

            byte[] hashBytes = Convert.FromBase64String(user.passwordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    throw new UnauthorizedAccessException("Username or password incorrect");
            return true; 
            #endregion
        }

        public string Role
        {
            get
            {
                return this.roleValue ?? "";
            }
        }

        public string Name => nameValue;

        public string AuthenticationType => "Custom Authentication";

        public bool IsAuthenticated => authenticatedValue;
    }
}