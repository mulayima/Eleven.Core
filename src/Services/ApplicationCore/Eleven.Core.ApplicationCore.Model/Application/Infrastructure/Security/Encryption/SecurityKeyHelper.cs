using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Eleven.Core.ApplicationCore.Model.Application.Infrastructure.Security.Encryption
{
    public static class SecurityKeyHelper
    {
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
