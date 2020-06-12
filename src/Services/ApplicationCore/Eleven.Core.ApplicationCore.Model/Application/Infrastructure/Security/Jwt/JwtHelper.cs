using System;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Eleven.Core.ApplicationCore.Model.Domain.Entities;
using Eleven.Core.ApplicationCore.Model.Application.Extensions;
using Eleven.Core.ApplicationCore.Model.Application.Infrastructure.Security.Encryption;

namespace Eleven.Core.ApplicationCore.Model.Application.Infrastructure.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        private CustomTokenOptions _tokenOptions;
        private readonly DateTime _accessTokenExpiration;
        public JwtHelper(IOptions<CustomTokenOptions> tokenOptions)
        {
            _tokenOptions = tokenOptions.Value;
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

        }
        public AccessToken CreateAccessToken(ApplicationUser user)
        {
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(user, signingCredentials);
            var jwtSecuritTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecuritTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration,
                RefreshToken = CreateRefreshToken()
            };
        }

        private JwtSecurityToken CreateJwtSecurityToken(ApplicationUser user, SigningCredentials signingCredentials)
        {
            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: ApplyClaims(user),
                signingCredentials: signingCredentials
                );

            return jwt;
        }

        private IEnumerable<Claim> ApplyClaims(ApplicationUser user)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddJwtId();

            return claims;
        }

        private string CreateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public void RevokeRefreshToken(ApplicationUser user)
        {
            user.RefreshToken = null;
        }
    }
}
