using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHub.Business.Models.Custum
{
    public class GenericResponse
    {
        public int errorNumber { get; set; }
        public string value { get; set; }
        public dynamic detail { get; set; }
        public int? count { get; set; }
        public int? page { get; set; }
        public int? total { get; set; }
    }
}
