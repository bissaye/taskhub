using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Data.Models.Errors;

namespace TaskHub.Business.Models.Errors
{
    public class BadCredentialsErrorException : Exception
    {
        public BadCredentialsErrorException() : base() { }
        public BadCredentialsErrorException(string msg) : base(msg) { }
    }

    public class BadTokenErrorException : Exception
    {
        public BadTokenErrorException() : base() { }
        public BadTokenErrorException(string msg) : base(msg) { }
    }
    
    public class UnauthorizedErrorException : Exception
    {
        public UnauthorizedErrorException() : base() { }
        public UnauthorizedErrorException(string msg) : base(msg) { }
    }             
}
