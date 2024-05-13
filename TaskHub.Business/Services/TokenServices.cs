using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Business.Models.Errors;
using TaskHub.Data;

namespace TaskHub.Business.Services
{
    public class TokenServices
    {
        private SymmetricSecurityKey _SymmetricSecurityKey;
        private string? _audience;
        private string? _issuer;
        private string? _cookieName;

        public TokenServices(IConfiguration configuration)
        {
            var TokTokenAuthentication = configuration.GetSection("TokenAuthentication");
            _audience = TokTokenAuthentication.GetSection("Audience").Value;
            _issuer = TokTokenAuthentication.GetSection("Issuer").Value;
            _cookieName = TokTokenAuthentication.GetSection("CookieName").Value;
            _SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokTokenAuthentication.GetSection("SecretKey").Value));

         }



        public bool CheckRefreshToken(ClaimsPrincipal User)
        {
            string refresh;
            try
            {
                refresh = User.Claims.First(s => s.Type == "refresh").Value.ToString();
                if (refresh == "1")
                {
                    string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (userId == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }

        }
        public Guid GetGuid(ClaimsPrincipal User)
        {
            Claim? claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new BadTokenErrorException("bad token");
            }
            string userId = claim.Value;
            if (userId == null)
            {
                throw new BadTokenErrorException("bad token");
            }
            return Guid.Parse(userId);
        }
        public List<Claim> GetClaims(Guid userId, bool refresh = false)
        {


            List<Claim> claims = new();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            if (refresh)
            {
                claims.Add(new Claim("refresh", "1"));
            }

            return claims;
        }

        public string GenerateAccessToken(List<Claim> claims)
        {

            var now = DateTime.UtcNow;
            var notBefore = now.AddMinutes(-5);
            var expiration = now.Add(new TimeSpan(24, 0, 0));
            var signingCredentials = new SigningCredentials(_SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(_issuer, _audience, claims, notBefore, expiration, signingCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;

        }

        public string GenerateRefreshToken(List<Claim> claims)
        {
            var now = DateTime.UtcNow;
            var notBefore = now.AddMinutes(-5);
            var expiration = now.Add(new TimeSpan(720, 0, 0));
            var signingCredentials = new SigningCredentials(_SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(_issuer, _audience, claims, notBefore, expiration, signingCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
