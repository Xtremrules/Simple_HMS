using Simple_HMS.Interface;
using System.Security.Principal;

namespace Simple_HMS.Security
{
    public class HMSPrincipal : IPrincipal
    {
        private HMSIdentity identityValue;

        public HMSPrincipal(IUser user, string password)
        {
            this.identityValue = new HMSIdentity(user, password);
        }

        public IIdentity Identity
        {
            get { return this.identityValue; }
        }

        public bool IsInRole(string role)
        {
            return (role == this.identityValue.Role);
        }
    }
}