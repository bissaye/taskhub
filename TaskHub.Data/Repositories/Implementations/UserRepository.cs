using Microsoft.EntityFrameworkCore;
using TaskHub.Data.Models.DAO;
using TaskHub.Data.Models.Errors;
using TaskHub.Data.Repositories.Interfaces;

namespace TaskHub.Data.Repositories.Implementations
{
    public class UserRepository(DataContext dataContext) : Repository<User>(dataContext) , IUserRepository
    {
        public async Task<User> updateUserPassword(Guid id, string password)
        {

            User user = await GetByIdAsync(id);

            try
            {
                user.Password = password;
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new UpdateErrorException($"Error while updating user : {ex.Message}");
            }
        }
    }
}
