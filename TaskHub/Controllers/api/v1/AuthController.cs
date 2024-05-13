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
    public class AuthController : ControllerBase
    {
        private readonly AuthUseCases _authUseCases;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserServices userServices, TokenServices tokenServices, ILogger<AuthController> logger)
        {
            _logger = logger;
            _authUseCases = new AuthUseCases(userServices, tokenServices, _logger);

        }

        /// <summary>
        ///      get token by password auth ( Auth not required )
        /// </summary>
        /// 
        /// <param name="userAuthReq">user data for auth</param>
        [HttpPost("token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserAuthRes), 200)]
        public async Task<CustumHttpResponse> getToken([FromBody] UserAuthReq userAuthReq)
        {
            return await _authUseCases.getToken(userAuthReq);
        }

        /// <summary>
        ///      get new access and refresh token with refresh 
        /// </summary>
        [HttpGet("refresh")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(UserAuthRes), 200)]
        public async Task<CustumHttpResponse> refresh()
        {
            return await _authUseCases.refreshToken(User);
        }
    }
}
