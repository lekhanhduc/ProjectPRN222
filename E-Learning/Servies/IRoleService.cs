using E_Learning.Entity;

namespace E_Learning.Servies
{
    public interface IRoleService
    {
        Task<Role> CreateRole(Role request);
        Task<Role?> FindByRoleName(string RoleName);
    }
}
