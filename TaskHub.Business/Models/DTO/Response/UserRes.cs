using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHub.Business.Models.DTO.Response
{
    public class UserDataRes
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
    }


    public class UserAuthRes
    {
        public string Access { get; set; }
        public string Refresh { get; set; }
        public UserDataRes? User { get; set; }
    }

}
