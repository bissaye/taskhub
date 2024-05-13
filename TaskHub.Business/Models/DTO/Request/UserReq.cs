using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHub.Business.Models.DTO.Request
{
    public class UserRegisterReq
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserUpdateReq
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
    }

    public class UserAuthReq
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
