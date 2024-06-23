using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHub.Business.Services.Interfaces
{
    public interface IPasswordServices
    {
        public bool comparePassword(string password, string convertedPassword);
        public string createPassword(string password);
    }
}
