using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentaiton.Repository
{
    public interface IAdminToken
    {
        string GenerateAdminToken(IdentityUser user, IList<string> roles, IList<Claim> claims);
    }
}
