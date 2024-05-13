using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.Project;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{

    public class DeveloperController : APIsBaseController
    {
        private readonly UserManager<Users> _userManager;
        private readonly IMapper _mapper;

        public DeveloperController(UserManager<Users> userManager, IMapper mapper)
        //SignInManager<Users> signInManager,
        //RoleManager<IdentityRole> roleManager,

        {
            _userManager = userManager;
            _mapper = mapper;
            //_signInManager = signInManager;
            //_roleManager = roleManager;
            //this.userManager = userManager;
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeveloperDto>>> GetProjects(string? name)
        {

            var Developers = (IReadOnlyList<Users>)await _userManager.GetUsersInRoleAsync("Developer");
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (Developers is not null)
                    Developers = Developers.Where(D => D.DisplayName == name).ToList();//get project by name
            }
            else
            {
                if (Developers is not null)
                    Developers = Developers.Where(D => D.ProjectId == null).ToList();//get all developers that not in project
            }
          
            var DeveloperDTO = _mapper.Map<IReadOnlyList<Users>, IReadOnlyList<DeveloperDto>>(Developers);
            return Ok(DeveloperDTO);

        }




    }
}
