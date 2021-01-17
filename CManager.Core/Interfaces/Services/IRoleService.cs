using CManager.Core.Entity.AuthenticationModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CManager.Core.Interfaces.Services
{
    public interface IRoleService
    {
        void Add(Role entity);
        Task AddAsync(Role entity);
    }
}
