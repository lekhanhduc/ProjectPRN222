using E_Learning.Data;
using E_Learning.Entity;
using E_Learning.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class RoleRepository
    {

        private readonly ELearningDbContext _context;

        public RoleRepository(ELearningDbContext context)
        {
            this._context = context;
        }

        public async Task<Role> CreateRole(Role role)
        {
            var existingRole = await _context.Roles
                .FirstOrDefaultAsync(x => x.Name == role.Name);

            if (existingRole != null)
            {
                throw new AppException(ErrorCode.ROLE_EXISTED);
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return role;
        }


        public async Task<Role?> FindByRoleName(string RoleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x => x.Name == RoleName);

        }
    }
}
