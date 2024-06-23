using Microsoft.Extensions.Configuration;
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Business.Models.Errors;
using TaskHub.Business.Services.Interfaces;
using TaskHub.Data;
using TaskHub.Data.Models.DAO;
using TaskHub.Data.Models.Errors;

namespace TaskHub.Business.Services.Implementations
{
    public class UserServices : IUserServices
    {
        private readonly IGateway _gateway;
        private readonly PasswordServices _passwordServices;
        public UserServices(IGateway gateway)
        {
            _gateway = gateway;
            _passwordServices = new PasswordServices();
        }

        public async Task<User> checkAuthUser(UserAuthReq auth)
        {

            User user = await _gateway.UserRepository().getUserByMail(auth.Email);
            if (_passwordServices.comparePassword(auth.Password, user.Password))
            {
                return user;
            }
            else
            {
                throw new BadCredentialsErrorException("bad credentials");
            }
        }

        public async Task<bool> checkUserMail(string email)
        {
            try
            {
                User user = await _gateway.UserRepository().getUserByMail(email);
                return true;
            }
            catch (NotFoundException ex)
            {
                return false;
            }
        }

        public async void createUser(UserRegisterReq userRegisterReq)
        {
            await _gateway.UserRepository().createUser(userRegisterReqToUser(userRegisterReq));
        }

        public async Task<UserDataRes> updateUser(Guid userId, UserUpdateReq userUpdateReq)
        {
            User user = await _gateway.UserRepository().updateUser(userId, userUpdateReqToUser(userUpdateReq));
            return userToUserDataRes(user);
        }

        public async Task<UserDataRes> getUserDataResById(Guid Id)
        {
            User user = await _gateway.UserRepository().getUserById(Id);
            return userToUserDataRes(user);
        }

        public async void deleteUser(Guid Id)
        {
            _gateway.UserRepository().deleteUser(Id);
        }

        public UserDataRes userToUserDataRes(User user)
        {
            return new UserDataRes()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
            };
        }

        public User userRegisterReqToUser(UserRegisterReq userRegisterReq)
        {
            return new User()
            {
                Firstname = userRegisterReq.Firstname,
                Lastname = userRegisterReq.Lastname,
                Email = userRegisterReq.Email,
                Password = _passwordServices.createPassword(userRegisterReq.Password),
            };
        }

        public User userUpdateReqToUser(UserUpdateReq userUpdateReq)
        {
            return new User()
            {
                Firstname = userUpdateReq.Firstname,
                Lastname = userUpdateReq.Lastname,
                Email = userUpdateReq.Email,
            };
        }
    }
}
