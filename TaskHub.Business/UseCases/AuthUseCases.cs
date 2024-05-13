using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Business.Models.Errors;
using TaskHub.Business.Services;
using TaskHub.Data.Models.DAO;
using TaskHub.Data.Models.Errors;

namespace TaskHub.Business.UseCases
{
    public class AuthUseCases
    {
        private readonly UserServices _userService;
        private readonly TokenServices _tokenService;
        private readonly ILogger _logger;

        public AuthUseCases( UserServices userServices, TokenServices tokenServices, ILogger logger)
        {
            _userService = userServices;
            _tokenService = tokenServices;
            _logger = logger;
        }

        public async Task<CustumHttpResponse> getToken(UserAuthReq userAuth)
        {
            try
            {
                _logger.LogInformation($"Attempting to authenticate user {userAuth.Email}");

                User user = await _userService.checkAuthUser(userAuth);

                _logger.LogInformation($"User {userAuth.Email} authenticated successfully");

                GenericResponse response = CustomHttpErrorNumber.success;
                response.detail = new UserAuthRes()
                {
                    Access = _tokenService.GenerateAccessToken(_tokenService.GetClaims(user.Id)),
                    Refresh = _tokenService.GenerateRefreshToken(_tokenService.GetClaims(user.Id, true)),
                    User = _userService.userToUserDataRes(user)
                };

                return new CustumHttpResponse(
                content: response,
                statusCode: 200
                );

            }
            catch (Exception ex) when (ex is BadCredentialsErrorException || ex is NotFoundException)
            {
                _logger.LogWarning($"Bad credentials provided for user {userAuth.Email}: {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.badCredentials;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 401
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred during authentication for user {userAuth.Email}: {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.serverError;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 500
                );
            }
        }

        public async Task<CustumHttpResponse> refreshToken(ClaimsPrincipal User)
        {
            try
            {
                Guid userId = _tokenService.GetGuid(User);

                _logger.LogInformation($"Refreshing token for user {userId}");

                if (_tokenService.CheckRefreshToken(User))
                {
                    GenericResponse response = CustomHttpErrorNumber.success;

                    UserDataRes user = await _userService.getUserDataResById(userId);

                    _logger.LogInformation($"Token refreshed successfully for user {userId}");

                    response.detail = new UserAuthRes()
                    {
                        Access = _tokenService.GenerateAccessToken(_tokenService.GetClaims(user.Id)),
                        Refresh = _tokenService.GenerateRefreshToken(_tokenService.GetClaims(user.Id, true)),
                        User = user
                    };

                    return new CustumHttpResponse(
                        content: response,
                        statusCode: 200
                        );
                }
                else
                {
                    GenericResponse response = CustomHttpErrorNumber.badCredentials;

                    response.detail = "bad token";

                    return new CustumHttpResponse(
                        content: response,
                        statusCode: 401
                        );
                }
            }
            catch (Exception ex) when (ex is NotFoundException || ex is BadTokenErrorException)
            {
                _logger.LogWarning($"Refresh token failed : {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.badCredentials;

                response.detail = ex.Message;

                return new CustumHttpResponse(
                    content: response,
                    statusCode: 401
                    );
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error occurred while refreshing token : {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.serverError;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 500
                );
            }
        }
    }
}
