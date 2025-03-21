﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Business.UseCases.Interfaces;


namespace TaskHub.Controllers.api.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserUsesCases _userUseCases;

        public UserController(IUserUsesCases userUseCases)
        {
            _userUseCases = userUseCases;
        }

        /// <summary>
        ///      register user ( Auth not required )
        /// </summary>
        /// 
        /// <param name="userRegisterReq">user data for registration</param>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericResponse<string>), 200)]
        public async Task<CustumHttpResponse<string>> register([FromBody] UserRegisterReq userRegisterReq)
        {
            return await _userUseCases.register(userRegisterReq);
        }

        /// <summary>
        ///      get user data 
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(GenericResponse<UserDataRes>), 200)]
        public async Task<CustumHttpResponse<UserDataRes>> get()
        {
            return await _userUseCases.getData(User);
        }


        /// <summary>
        ///      update user data 
        /// </summary>
        /// <param name="userUpdateReq">new user data to updat with</param>
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(GenericResponse<UserDataRes>), 200)]
        public async Task<CustumHttpResponse<UserDataRes>> update([FromBody] UserUpdateReq userUpdateReq)
        {
            return await _userUseCases.updateData(User, userUpdateReq);
        }


        /// <summary>
        ///      delete user account 
        /// </summary>
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(GenericResponse<string>), 200)]
        public CustumHttpResponse<string> delete()
        {
            return _userUseCases.deleteUser(User);
        }

    }
}
