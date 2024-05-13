using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.Errors;
using TaskHub.Business.Services;
using TaskHub.Data.Models.Errors;

namespace TaskHub.Business.UseCases
{
    public class UserUsesCases
    {
        private readonly UserServices _userService;
        private readonly TokenServices _tokenService;
        private readonly ILogger _logger;
        public UserUsesCases(UserServices userServices , TokenServices tokenServices, ILogger logger)
        {
            _userService = userServices;
            _tokenService = tokenServices;
            _logger = logger;
        }

        public async Task<CustumHttpResponse> register(UserRegisterReq userDataReq)
        {
            try
            {
                _logger.LogInformation($"attempt to register new user {userDataReq.Email}");
                if(await _userService.checkUserMail(userDataReq.Email))
                {
                    _logger.LogWarning($"Email {userDataReq.Email} already exists");
                    GenericResponse response = CustomHttpErrorNumber.conflict;
                    response.detail = "email already used";
                    return new CustumHttpResponse(
                        content: response,
                        statusCode: 409
                    );
                }
                else
                {
                    _userService.createUser(userDataReq);

                    _logger.LogInformation($"User created successfully with email {userDataReq.Email}");

                    GenericResponse response = CustomHttpErrorNumber.success;

                    response.detail = "user created successfully";

                    return new CustumHttpResponse(
                        content: response,
                        statusCode: 201
                    );
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error occurred during user registration: {ex.Message}");
                GenericResponse response = CustomHttpErrorNumber.serverError;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 500
                );
            }
        }

        public async Task<CustumHttpResponse> getData(ClaimsPrincipal User)
        {
            try
            {
                Guid userId = _tokenService.GetGuid(User);

                _logger.LogInformation($"Fetching user data user data for Id {userId}");

                GenericResponse response = CustomHttpErrorNumber.success;
                response.detail = await _userService.getUserDataResById(userId);
                
                return new CustumHttpResponse(
                    content: response,
                    statusCode: 200
                );

            }
            catch (BadTokenErrorException ex)
            {
                _logger.LogWarning($"Bad token error: {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.badCredentials;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 401
                );
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"User data not found : {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.notfound;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 404
                );
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error occurred while fetching user data : {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.serverError;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 500
                );
            }
        }

        public async Task<CustumHttpResponse> updateData(ClaimsPrincipal User, UserUpdateReq user)
        {
            try
            {
                Guid userId = _tokenService.GetGuid(User);
                _logger.LogInformation($"Updating user data for ID {userId}");

                GenericResponse response = CustomHttpErrorNumber.success;

                response.detail = await _userService.updateUser(userId, user);

                return new CustumHttpResponse(
                    content: response,
                    statusCode: 200
                );
            }
            catch (BadTokenErrorException ex)
            {
                _logger.LogWarning($"Bad token error: {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.badCredentials;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 401
                );
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"User data not found : {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.notfound;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 404
                );
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error occurred while updating user data : {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.serverError;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 500
                );
            }
        }

        public CustumHttpResponse deleteUser(ClaimsPrincipal User)
        {
            try
            {
                Guid userId = _tokenService.GetGuid(User);
                _logger.LogInformation($"deleting user data for ID {userId}");

                _userService.deleteUser(_tokenService.GetGuid(User));

                GenericResponse response = CustomHttpErrorNumber.success;

                response.detail = "user deleted successfully";

                return new CustumHttpResponse(
                    content: response,
                    statusCode: 200
                );
            }
            catch (BadTokenErrorException ex)
            {
                _logger.LogWarning($"Bad token error: {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.badCredentials;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 401
                );
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"User data not found : {ex.Message}");

                GenericResponse response = CustomHttpErrorNumber.notfound;
                response.detail = ex.Message;

                return new CustumHttpResponse(
                content: response,
                statusCode: 404
                );
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error occurred while updating user data : {ex.Message}");

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
