using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHub.Data.Models.Custum
{
    public class Paginate<T>
    {
        public List<T> datas { get; set; }
        public int total { get; set; }
        public int count { get; set; }
        public int page { get; set; }
    }
}
