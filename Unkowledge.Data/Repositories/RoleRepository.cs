using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Interfaces.Repositories;
using CManager.Web.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CManager.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly CManagerDbContext context;
        private DbSet<Role> entities;

        public RoleRepository(CManagerDbContext context)
        {
            this.context = context;
            entities = context.Set<Role>();
        }

        public async Task AddAsync(Role entity)
        {
            entity.Id = Guid.NewGuid();
            entity.NormalizedName = entity.Name.ToUpper();
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }
    }
}
