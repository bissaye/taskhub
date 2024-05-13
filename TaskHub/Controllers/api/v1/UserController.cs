using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Business.Services;
using TaskHub.Business.UseCases;
using TaskHub.Data;

namespace TaskHub.Controllers.api.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserUsesCases _userUseCases;
        private readonly ILogger<UserController> _logger;

        public UserController(UserServices userServices, TokenServices tokenServices, ILogger<UserController> logger)
        {
            _logger = logger;
            _userUseCases = new UserUsesCases(userServices, tokenServices, _logger);
        }

        /// <summary>
        ///      register user ( Auth not required )
        /// </summary>
        /// 
        /// <param name="userRegisterReq">user data for registration</param>
        [HttpPost]
        [AllowAnonymous]
        public async Task<CustumHttpResponse> register([FromBody] UserRegisterReq userRegisterReq)
        {
            return await _userUseCases.register(userRegisterReq);
        }

        /// <summary>
        ///      get user data 
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(UserDataRes), 200)]
        public async Task<CustumHttpResponse> get()
        {
            return await _userUseCases.getData(User);
        }


        /// <summary>
        ///      update user data 
        /// </summary>
        /// <param name="userUpdateReq">new user data to updat with</param>
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(UserDataRes), 200)]
        public async Task<CustumHttpResponse> update([FromBody] UserUpdateReq userUpdateReq)
        {
            return await _userUseCases.updateData(User, userUpdateReq);
        }


        /// <summary>
        ///      delete user account 
        /// </summary>
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public CustumHttpResponse delete()
        {
            return _userUseCases.deleteUser(User);
        }

    }
}
