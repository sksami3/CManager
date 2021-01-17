using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Interfaces.Repositories;
using CManager.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CManager.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public void Add(Role entity)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(Role entity)
        {
            await _roleRepository.AddAsync(entity);
        }
    }
}
