using E_Learning.Entity;
using E_Learning.Models.Response;
using E_Learning.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    [Route("api/v1/roles")]
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
<<<<<<< HEAD
        public async Task<ApiResponse<Role>> CreateRole([FromBody] Role request)
=======
        public async Task<ResponseData<Role>> CreateRole([FromBody] Role request)
>>>>>>> 459715e (project-course 26.02.2025)
        {
            var result = await roleService.CreateRole(request);

            return new ApiResponse<Role>(201, "Created success", result);
        }
    }
}
