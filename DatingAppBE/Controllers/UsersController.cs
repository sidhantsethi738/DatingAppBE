using AutoMapper;
using DatingAppBE.Data;
using DatingAppBE.DTOs;
using DatingAppBE.Entities;
using DatingAppBE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingAppBE.Controllers
{
    [Route("api/Users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {

        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository repository , IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
      
        [Route("GetUSers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUSers()
        {

            var users = await _repository.GetMembersAsync();
            return Ok(users);
        }

        [Route("GetByUsername")]
        [HttpGet]

        public async Task<ActionResult<MemberDto>> GetUserAsync(string username)
        {
            return  await _repository.GetMemberAsync(username);
        }

        [Route("UpdateProfile")]
        [HttpPut]
        public async Task<ActionResult> UpdateProfile(MemberUpdateDto updateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _repository.GetUserByUserNameAsync(username);

            _mapper.Map(updateDto, user);
            _repository.Update(user);

            if (await _repository.SaveAllSync()) return NoContent();
            else
                return BadRequest("Failed to update User");
        }  

    }
}
