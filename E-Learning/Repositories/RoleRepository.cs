using E_Learning.Data;
using E_Learning.Dto.Request;
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

        public async Task<Role> CreateRole(RoleCreationRequest request)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(x => x.Name == request.RoleName);

            if (role != null)
            {
                throw new AppException(ErrorCode.ROLE_EXISTED);
            }

            role = new Role();
            role.Name = request.RoleName;
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role?> FindByRoleName(string RoleName)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(x => x.Name == RoleName);

            if (role == null)
            {
                throw new AppException(ErrorCode.ROLE_NOT_EXISTED);
            }
            return role;
        }
    }
}
