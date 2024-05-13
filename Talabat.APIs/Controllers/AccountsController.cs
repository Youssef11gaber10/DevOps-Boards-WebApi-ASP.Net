using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{

    public class AccountsController : APIsBaseController
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AccountsController(UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        #region Register
        //in reg need to give him email,displayname,phonenumber,password ===> will return email,displayname,token
        [HttpPost("Register")]
        //public async Task<ActionResult<UserDto>> Register(AppUser User)//you can do this but not perfer cause user knows your obj prop not secure
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {


            if (!_roleManager.Roles.Any(R => R.Name == model.RoleName))//if the Roles =>Table dosn't have this value
            {
                var Role = new IdentityRole()
                {
                    Name = model.RoleName
                };
                await _roleManager.CreateAsync(Role);
            }

            var User = new Users()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
                ProjectId=null
            };


            var Result = await _userManager.CreateAsync(User, model.Password);

            if (!Result.Succeeded)
                return BadRequest(new ApiResponse(404));


            if (!await _userManager.IsInRoleAsync(User, model.RoleName))
            {
                var result = await _userManager.AddToRoleAsync(User, model.RoleName);
                if (result.Succeeded)
                {
                    User.RoleName = model.RoleName;
                    var _Result = await _userManager.UpdateAsync(User);
                    if (!Result.Succeeded)
                        return BadRequest(new ApiResponse(404));
                }
            }


            var ReturnedUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager),
                RoleName = User.RoleName//from Model not from User
            };

            return Ok(ReturnedUser);

        }


        #endregion


        #region Login


        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));


            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
            if (!Result.Succeeded) return BadRequest(404);



            return Ok(new UserDto()
            {
                Message = "success",
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager),
                RoleName = User.RoleName
            });


        }

        #endregion




    }
}
