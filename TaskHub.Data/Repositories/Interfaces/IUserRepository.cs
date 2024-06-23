

using TaskHub.Data.Models.DAO;

namespace TaskHub.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> createUser(User user);
        public Task<User> getUserById(Guid Id);
        public Task<User> getUserByMail(string email);
        public Task<User> updateUser(Guid Id, User user);
        public Task<User> updateUserPassword(Guid Id, string password);
        public void deleteUser(Guid Id);
    }
}
