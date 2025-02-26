using E_Learning.Dto.Request;
using E_Learning.Entity;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly IRoleService roleService;

        public RolesController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpPost("create-role")]
        [AllowAnonymous]
        public async Task<ResponseData<Role>> CreateRole([FromBody] RoleCreationRequest request)
        {
            var result = await roleService.CreateRole(request);

            return new ResponseData<Role>(201, "Created success", result);
        }
    }
}
