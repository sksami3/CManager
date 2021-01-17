using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Interfaces.Repositories.Base;

namespace CManager.Core.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task AddAsync(Role entity);
    }
}
