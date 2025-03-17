
using System.Security.Claims;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;

namespace TaskHub.Business.UseCases.Interfaces
{
    public interface IUserUsesCases
    {
        public Task<CustumHttpResponse<string>> register(UserRegisterReq userDataReq);
        public Task<CustumHttpResponse<UserDataRes>> getData(ClaimsPrincipal User);
        public Task<CustumHttpResponse<UserDataRes>> updateData(ClaimsPrincipal User, UserUpdateReq user);
        public CustumHttpResponse<string> deleteUser(ClaimsPrincipal User);

    }
}
