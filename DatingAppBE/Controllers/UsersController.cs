using AutoMapper;
using DatingAppBE.DTOs;
using DatingAppBE.Entities;
using DatingAppBE.Extensions;
using DatingAppBE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepository repository, IMapper mapper, IPhotoService photoservice)
        {
            _repository = repository;
            _mapper = mapper;
            _photoService = photoservice;
        }

        [Route("GetUSers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUSers()
        {

            var users = await _repository.GetMembersAsync();
            return Ok(users);
        }

        [Route("GetByUsername", Name = "GetUser")]
        [HttpGet]

        public async Task<ActionResult<MemberDto>> GetUserAsync(string username)
        {
            return await _repository.GetMemberAsync(username);
        }

        [Route("UpdateProfile")]
        [HttpPut]
        public async Task<ActionResult> UpdateProfile(MemberUpdateDto updateDto)
        {
            var username = User.GetUsername();
            var user = await _repository.GetUserByUserNameAsync(username);

            _mapper.Map(updateDto, user);
            _repository.Update(user);

            if (await _repository.SaveAllSync()) return NoContent();
            else
                return BadRequest("Failed to update User");
        }

        [Route("AddPhoto")]
        [HttpPost]

        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {

            var user = await _repository.GetUserByUserNameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _repository.SaveAllSync())
            {
                return CreatedAtRoute("GetUser", new { username = user.Username }, _mapper.Map<PhotoDto>(photo));
            }


            return BadRequest("Problem adding photo");
        }

        [Route("SetMainPhoto/{photoId}")]
        [HttpPut]

        public async Task<ActionResult> SetMainPhoto(int PhotoId)
        {
            var user = await _repository.GetUserByUserNameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.id == PhotoId);

            if (photo.IsMain) return BadRequest("this is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _repository.SaveAllSync()) return NoContent();

            return BadRequest("Failed to set main photo");

        }


        [Route("PhotoDelete/{photoId}")]
        [HttpDelete]
        public async Task<ActionResult> PhotoDelete(int PhotoId)
        {
            var user = await _repository.GetUserByUserNameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.id == PhotoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                    return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);
            if (await _repository.SaveAllSync()) return Ok();

            return BadRequest("Failed to delete this message");
        }
    }
}
