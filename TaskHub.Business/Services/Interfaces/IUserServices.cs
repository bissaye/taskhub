
using TaskHub.Business.Models.DTO.Request;
using TaskHub.Business.Models.DTO.Response;
using TaskHub.Data.Models.DAO;

namespace TaskHub.Business.Services.Interfaces
{
    public interface IUserServices
    {
        public Task<User> checkAuthUser(UserAuthReq auth);
        public Task<bool> checkUserMail(string email);
        public void createUser(UserRegisterReq userRegisterReq);
        public Task<UserDataRes> updateUser(Guid userId, UserUpdateReq userUpdateReq);
        public Task<UserDataRes> getUserDataResById(Guid Id);
        public void deleteUser(Guid Id);
        public UserDataRes userToUserDataRes(User user);
        public User userRegisterReqToUser(UserRegisterReq userRegisterReq);
        public User userUpdateReqToUser(UserUpdateReq userUpdateReq);
    }
}
