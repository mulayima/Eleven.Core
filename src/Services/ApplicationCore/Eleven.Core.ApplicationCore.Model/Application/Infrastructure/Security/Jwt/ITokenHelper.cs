using Eleven.Core.ApplicationCore.Model.Domain.Entities;

namespace Eleven.Core.ApplicationCore.Model.Application.Infrastructure.Security.Jwt
{
    public interface ITokenHelper
    {
        AccessToken CreateAccessToken(ApplicationUser user);

        void RevokeRefreshToken(ApplicationUser user);
    }
}
