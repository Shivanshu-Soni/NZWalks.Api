using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NZWalks.Api.Repository
{
    public interface ITokenRepository
    {
        string createJWTToken(IdentityUser user, List<string> roles);
    }
}