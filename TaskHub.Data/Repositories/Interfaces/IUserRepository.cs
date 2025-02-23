

using TaskHub.Data.Models.DAO;

namespace TaskHub.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> updateUserPassword(Guid Id, string password);
    }
}
