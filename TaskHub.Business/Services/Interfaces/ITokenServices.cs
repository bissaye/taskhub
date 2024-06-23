using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TaskHub.Business.Services.Interfaces
{
    public interface ITokenServices
    {
        public bool CheckRefreshToken(ClaimsPrincipal User);
        public Guid GetGuid(ClaimsPrincipal User);
        public List<Claim> GetClaims(Guid userId, bool refresh = false);
        public string GenerateAccessToken(List<Claim> claims);
        public string GenerateRefreshToken(List<Claim> claims);
    }
}
