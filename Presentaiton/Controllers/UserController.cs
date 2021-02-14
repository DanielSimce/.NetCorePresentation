using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentaiton.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Only Admins should be able to get all admins
        //For testing roles
        [Authorize(Roles="Admin")]
        [Authorize]
        [HttpGet("Admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return Ok(admins.Select(x => new { Name = x.UserName }));

        }

        [Authorize(Roles = "Manager")]
        [Authorize]
        [HttpGet("Managers")]
        public async Task<IActionResult> GetAllManagers()
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            return Ok(managers.Select(x => new { Name = x.UserName }));
             
        

           
        }

        [Authorize]
        [HttpGet("Users")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userManager.Users.Select(x => new { Name = x.UserName }));
        }

        [HttpGet("manageDevelopers")]
        [Authorize(Policy = "ManageDevelopers")]
        public IActionResult ManagerDevelopers()
        {
            return Ok(new
            {
                role = "This user Role is Manager",
                claim = "User using this Api claims to be Developer"
            });
                          
              
        }

        [HttpGet("adminDevelopers")]
        [Authorize(Policy = "AdminDevelopers")]
        public IActionResult AdminDevelopers()
        {
            return Ok(new
            {
                role = "This user Role is Admin",
                claim = "User using this Api claims to be Developer"
            });


        }

        [HttpGet("roles")]
        public ActionResult<IdentityUser> Roles()
        {
            return Ok(_roleManager.Roles.Select(x => new  {x.Name}));
        }



    }
}