using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Data.Repositories.Implementations;
using TaskHub.Data.Repositories.Interfaces;

namespace TaskHub.Data
{
    public class Gateway : IGateway
    {
        private readonly IServiceProvider _serviceProvider;

        public Gateway(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITaskItemRepository TaskItemRepository()
        {
            return _serviceProvider.GetRequiredService<ITaskItemRepository>();
        }

        public IUserRepository UserRepository()
        {
            return _serviceProvider.GetRequiredService<IUserRepository>();
        }
    }
}
