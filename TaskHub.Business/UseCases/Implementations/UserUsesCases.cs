using Bissaye.JwtAuth.Services;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TaskHub.Business.Models.Claims;
using TaskHub.Business.Models.Custum;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.Errors;
using TaskHub.Business.Services.Interfaces;
using TaskHub.Business.UseCases.Interfaces;
using TaskHub.Data.Models.Errors;

namespace TaskHub.Business.UseCases.Implementations
{
    public class UserUsesCases : IUserUsesCases
    {
        private readonly IUserServices _userService;
        private readonly ITokenServices _tokenService;
        private readonly ILogger _logger;
        public UserUsesCases(IUserServices userServices, ITokenServices tokenServices, ILogger logger)
        {
            _userService = userServices;
            _tokenService = tokenServices;
            _logger = logger;
        }

        public async Task<CustumHttpResponse> register(UserRegisterReq userDataReq)
        {
            _logger.LogInformation("attempt to register new user {Email}", userDataReq.Email);
            if (await _userService.checkUserMail(userDataReq.Email))
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

                _logger.LogInformation("User created successfully with email {Email}", userDataReq.Email);

                GenericResponse response = CustomHttpErrorNumber.success;

                response.detail = "user created successfully";

                return new CustumHttpResponse(
                    content: response,
                    statusCode: 201
                );
            }
        }

        public async Task<CustumHttpResponse> getData(ClaimsPrincipal User)
        {

            AuthClaims userClaims = _tokenService.GetClaimsValue<AuthClaims>(User);

            _logger.LogInformation("Fetching user data user data for Id {userId}", userClaims.UserId);

            GenericResponse response = CustomHttpErrorNumber.success;
            response.detail = await _userService.getUserDataResById(userClaims.UserId);

            return new CustumHttpResponse(
                content: response,
                statusCode: 200
            );
        }

        public async Task<CustumHttpResponse> updateData(ClaimsPrincipal User, UserUpdateReq user)
        {

            AuthClaims userClaims = _tokenService.GetClaimsValue<AuthClaims>(User);
            _logger.LogInformation("Updating user data for ID {userId}", userClaims.UserId);

            GenericResponse response = CustomHttpErrorNumber.success;

            response.detail = await _userService.updateUser(userClaims.UserId, user);

            return new CustumHttpResponse(
                content: response,
                statusCode: 200
            );

        }

        public CustumHttpResponse deleteUser(ClaimsPrincipal User)
        {
            AuthClaims userClaims = _tokenService.GetClaimsValue<AuthClaims>(User);
            _logger.LogInformation("deleting user data for ID {userId}", userClaims.UserId);

            _userService.deleteUser(userClaims.UserId);

            GenericResponse response = CustomHttpErrorNumber.success;

            response.detail = "user deleted successfully";

            return new CustumHttpResponse(
                content: response,
                statusCode: 200
            );
        }

    }
}
