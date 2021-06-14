using AutoMapper;
using DatingAppBE.Data;
using DatingAppBE.DTOs;
using DatingAppBE.Entities;
using DatingAppBE.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingAppBE.Controllers
{
    [Route("api/UserForms")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly DataContext _context;

        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }


        [Route("Register")]
        [HttpPost]

        public async Task<ActionResult<UserDTO>> Register(Register register)
        {

            if (UserExists(register.Username))
            {
                return BadRequest("UserName already taken");
            }

            var user = _mapper.Map<AppUser>(register);

            var hmac = new HMACSHA512();

            user.Username = register.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password));
            user.PassowrdSalt = hmac.Key;


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }


        private bool UserExists(string Username)
        {
            return _context.Users.Any(x => x.Username == Username.ToLower());
        }


        [Route("Login")]
        [HttpPost]

        public async Task<ActionResult<UserDTO>> Login(Login login)
        {

            var user = await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.Username == login.Username);

            if (user == null)
                return BadRequest("Invalid Username");

            var hmac = new HMACSHA512(user.PassowrdSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return BadRequest("Invalid Password");
            }

            return new UserDTO
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };

        }
    }
}
