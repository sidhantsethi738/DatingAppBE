using DatingAppBE.Data;
using DatingAppBE.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppBE.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [Route("GetUSers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUSers()
        {
            return await _context.Users.ToListAsync();

        }

        [Authorize]
        [Route("GetUserAsync")]
        [HttpGet]

        public ActionResult<AppUser> GetUserAsync(int id)
        {
            return _context.Users.Find(id);

        }
    }
}
