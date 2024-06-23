using System.Text;
using System.Security.Cryptography;
using TaskHub.Business.Services.Interfaces;

namespace TaskHub.Business.Services.Implementations
{
    public class PasswordServices : IPasswordServices
    {
        public PasswordServices()
        {

        }

        public bool comparePassword(string password, string convertedPassword)
        {
            return createPassword(password) == convertedPassword;
        }

        public string createPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
