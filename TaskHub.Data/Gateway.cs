using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Data.Repositories;

namespace TaskHub.Data
{
    public class Gateway
    {
        private readonly IServiceProvider _serviceProvider;

        public Gateway(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TaskItemRepository TaskItemRepository()
        {
            return _serviceProvider.GetRequiredService<TaskItemRepository>();
        }

        public UserRepository UserRepository()
        {
            return _serviceProvider.GetRequiredService<UserRepository>();
        }
    }
}
