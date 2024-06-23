
using System.Security.Claims;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;

namespace TaskHub.Business.UseCases.Interfaces
{
    public interface IUserUsesCases
    {
        public Task<CustumHttpResponse> register(UserRegisterReq userDataReq);
        public Task<CustumHttpResponse> getData(ClaimsPrincipal User);
        public Task<CustumHttpResponse> updateData(ClaimsPrincipal User, UserUpdateReq user);
        public CustumHttpResponse deleteUser(ClaimsPrincipal User);

    }
}
