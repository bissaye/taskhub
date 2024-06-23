using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;

namespace TaskHub.Business.UseCases.Interfaces
{
    public interface IAuthUseCases
    {
        public Task<CustumHttpResponse> getToken(UserAuthReq userAuth);
        public Task<CustumHttpResponse> refreshToken(ClaimsPrincipal User);

    }
}
