using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentaiton.Models;
using Presentaiton.Repository;

namespace Presentaiton.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
       
        private readonly IAdminToken _adminToken;

        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, 
            RoleManager<IdentityRole> roleManager, IAdminToken adminToken)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _adminToken = adminToken;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var userToCreate = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(userToCreate, model.Password);

            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "Admin")]
        [Authorize]
        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminModel model)
        {
            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
            }


            var userToCreate = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Username
            };

            //Create Admin
            var result = await _userManager.CreateAsync(userToCreate, model.Password);

            if (result.Succeeded)
            {
                var userInDb = await _userManager.FindByNameAsync(userToCreate.UserName);

                //Add Role To Admin

                await _userManager.AddToRoleAsync(userInDb,model.Role);

                //Add Claim to Admin

                var claim = new Claim("JobTitle", model.JobTitle);

                await _userManager.AddClaimAsync(userInDb, claim);
              

                return Ok(result.Succeeded);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var userInD = await _userManager.FindByNameAsync(model.Username);

            if (userInD == null)
            {
                return BadRequest("User doesn't exict!!!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(userInD, model.Password, false);

            if (!result.Succeeded)
            {
                return BadRequest("Password wrong!!!!");
            }

            var roles = await _userManager.GetRolesAsync(userInD);

            IList<Claim> claims = await _userManager.GetClaimsAsync(userInD);

            return Ok(new
            {
                result = result,
                username = userInD.UserName,
                email = userInD.Email,
                token = _adminToken.GenerateAdminToken(userInD,roles,claims)

            }); 
        }
    }
}