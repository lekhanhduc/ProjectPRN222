using E_Learning.Entity;
using E_Learning.Repositories;

namespace E_Learning.Servies.Impl
{
    public class RoleService : IRoleService
    {
        private readonly RoleRepository roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }
        public Task<Role> CreateRole(Role request)
        {
            return roleRepository.CreateRole(request);
        }

        public Task<Role?> FindByRoleName(string RoleName)
        {
            return roleRepository.FindByRoleName(RoleName);
        }
    }
}
