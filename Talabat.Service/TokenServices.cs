using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class TokenServices : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(Users User, UserManager<Users> userManager)
        {

            #region payload => register claims , private claims

            // 1.Private Claims [user-defined]]  As name,email,password ... info from user 

            var AuthClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,User.DisplayName),
                new Claim(ClaimTypes.Email,User.Email),
                new Claim(ClaimTypes.GivenName,User.UserName)

            };
            var UserRoles = await userManager.GetRolesAsync(User);//whos add this roles to this user
            foreach (var Role in UserRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, Role));
            }

    
            #endregion

            #region Key
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            #endregion


            var Token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issure"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:ExpireTimeInDays"])),
                claims:AuthClaims,
                signingCredentials:new SigningCredentials(AuthKey,SecurityAlgorithms.HmacSha256Signature)
                );


            return new JwtSecurityTokenHandler().WriteToken(Token);

        }
    }
}
