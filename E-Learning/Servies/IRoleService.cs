using E_Learning.Dto.Request;
using E_Learning.Entity;

namespace E_Learning.Servies
{
    public interface IRoleService
    {
        Task<Role> CreateRole(RoleCreationRequest request);
        Task<Role?> FindByRoleName(string RoleName);
    }
}
