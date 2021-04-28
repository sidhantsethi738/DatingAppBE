using DatingAppBE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppBE.Controllers
{
    [Route("api/Buggy")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [Route("Auth")]
        [HttpGet]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }


        [Route("NotFound")]
        [HttpGet]
        public ActionResult<string> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound();
            return Ok(thing);
        }

        [Route("ServerError")]
        [HttpGet]
        public ActionResult<string> GetServerError()
        {

            var thing = _context.Users.Find(-1);
            var thingreturn = thing.ToString();
            return thingreturn;


        }

        [Route("BadRequest")]
        [HttpGet]
        public ActionResult<string> BadRequesT()
        {
            return BadRequest("This is not a good request");
        }


    }
}
