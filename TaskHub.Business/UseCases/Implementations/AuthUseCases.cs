using Bissaye.JwtAuth.Services;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TaskHub.Business.Models.Claims;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Business.Models.Errors;
using TaskHub.Business.Services.Interfaces;
using TaskHub.Business.UseCases.Interfaces;
using TaskHub.Data.Models.DAO;
using TaskHub.Data.Models.Errors;

namespace TaskHub.Business.UseCases.Implementations
{
    public class AuthUseCases : IAuthUseCases
    {
        private readonly IUserServices _userService;
        private readonly ITokenServices _tokenService;
        private readonly ILogger _logger;

        public AuthUseCases(IUserServices userServices, ITokenServices tokenServices, ILogger logger)
        {
            _userService = userServices;
            _tokenService = tokenServices;
            _logger = logger;
        }

        public async Task<CustumHttpResponse<UserAuthRes>> getToken(UserAuthReq userAuth)
        {
            _logger.LogInformation("Attempting to authenticate {User}", userAuth.Email);

            User user = await _userService.checkAuthUser(userAuth);

            GenericResponse<UserAuthRes> response = CustomHttpErrorNumber<UserAuthRes>.success;
            response.detail = new UserAuthRes()
            {
                Access = _tokenService.GenerateToken(_tokenService.GetClaims<AuthClaims>(new AuthClaims { UserId = user.Id }), false),
                Refresh = _tokenService.GenerateToken(_tokenService.GetClaims<AuthClaims>(new AuthClaims { UserId = user.Id }), true),
                User = _userService.userToUserDataRes(user)
            };

            _logger.LogInformation("{User} authenticated successfully", userAuth.Email);

            return new CustumHttpResponse<UserAuthRes> (
            content: response,
            statusCode: 200
            );
        }

        public async Task<CustumHttpResponse<UserAuthRes>> refreshToken(ClaimsPrincipal User)
        {

            Guid userId = _tokenService.GetClaimsValue<AuthClaims>(User).UserId;

            _logger.LogInformation("Refreshing token for {UserId}", userId);

            if (_tokenService.CheckRefreshToken(User))
            {
                GenericResponse<UserAuthRes> response = CustomHttpErrorNumber<UserAuthRes>.success;

                UserDataRes user = await _userService.getUserDataResById(userId);

                response.detail = new UserAuthRes()
                {
                    Access = _tokenService.GenerateToken(_tokenService.GetClaims<AuthClaims>(new AuthClaims { UserId = user.Id }), false),
                    Refresh = _tokenService.GenerateToken(_tokenService.GetClaims<AuthClaims>(new AuthClaims { UserId = user.Id }), true),
                    User = user
                };

                _logger.LogInformation("Token refreshed successfully for {UserId}", userId);

                return new CustumHttpResponse<UserAuthRes>(
                    content: response,
                    statusCode: 200
                    );
            }
            else
            {
                throw new BadTokenErrorException("bad token");
            }
        }
    }
}
