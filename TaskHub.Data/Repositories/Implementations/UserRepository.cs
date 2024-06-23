using Microsoft.EntityFrameworkCore;
using TaskHub.Data.Models.DAO;
using TaskHub.Data.Models.Errors;
using TaskHub.Data.Repositories.Interfaces;

namespace TaskHub.Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User> createUser(User user)
        {
            try
            {
                User _user = user;
                _dataContext.Users.Add(_user);
                await _dataContext.SaveChangesAsync();

                return _user;
            }
            catch (Exception ex)
            {
                throw new SavingErrorException($"error wile saving user : {ex.Message}");
            }
        }

        public async Task<User> getUserById(Guid Id)
        {
            try
            {
                User? user = await _dataContext.Users.FindAsync(Id);
                if (user == null)
                {
                    throw new NotFoundException("User not found");
                }
                return user;
            }
            catch (Exception ex) when (!(ex is NotFoundException))
            {
                throw new ReadErrorException($"error while reading user : {ex.Message}");
            }

        }


        public async Task<User> getUserByMail(string email)
        {
            try
            {
                User? user = await _dataContext.Users.FirstOrDefaultAsync(user => user.Email == email);
                if (user == null)
                {
                    throw new NotFoundException("User not found");
                }
                return user;
            }
            catch (Exception ex) when (!(ex is NotFoundException))
            {
                throw new ReadErrorException($"error while reading user : {ex.Message}");
            }

        }

        public async Task<User> updateUser(Guid Id, User user)
        {

            User? _user = await getUserById(Id);
            if (_user != null)
            {
                try
                {
                    _dataContext.Users.Attach(_user);
                    _user.Email = user.Email;
                    _user.Firstname = user.Firstname;
                    _user.Lastname = user.Lastname;
                    await _dataContext.SaveChangesAsync();

                    return _user;
                }
                catch (Exception ex)
                {
                    throw new UpdateErrorException($"rror while updating user :{ex.Message}");
                }

            }
            else
            {
                throw new NotFoundException($"these user dosen't exists");
            }
        }

        public async Task<User> updateUserPassword(Guid Id, string password)
        {

            User _user = await getUserById(Id);

            try
            {
                _dataContext.Users.Attach(_user);
                _user.Password = password;
                await _dataContext.SaveChangesAsync();
                return _user;
            }
            catch (Exception ex)
            {
                throw new UpdateErrorException($"Error while updating user : {ex.Message}");
            }
        }

        public async void deleteUser(Guid Id)
        {
            User? user = await getUserById(Id);
            if (user == null)
            {
                throw new NotFoundException("these user dosen't exists");
            }
            else
            {
                try
                {
                    _dataContext.Users.Remove(user);
                    await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new DeleteErrorException($"error while deleting user : {ex.Message}");
                }
            }
        }
    }
}
