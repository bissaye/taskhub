using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Data.Repositories.Interfaces;

namespace TaskHub.Data
{
    public interface IGateway
    {
        public ITaskItemRepository TaskItemRepository();
        public IUserRepository UserRepository();
    }
}
