using DatingAppBE.Data;
using DatingAppBE.Entities;
using DatingAppBE.Interfaces;
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

        private readonly IUserRepository _repository;
        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        [Route("GetUSers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUSers()
        {
            return Ok(await _repository.GetUsersAsync());

        }

     
        [Route("GetByUsername")]
        [HttpGet]

        public async Task<ActionResult<AppUser>> GetUserAsync(string username)
        {
            return await _repository.GetUserByUserNameAsync(username);

        }
    }
}
