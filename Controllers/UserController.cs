using System;
using ImageRepo.Entities;
using ImageRepo.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImageRepo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public UserController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        [HttpPost("signup")]
        public ActionResult<string> Signup([FromForm] User newUser)
        {
            if (String.IsNullOrEmpty(newUser.Username))
            {
                return BadRequest("Username is null or empty");
            }
            
            if (unitOfWork.Users.Exists(newUser.Username))
            {
                return Conflict("Username already in use");
            }

            unitOfWork.Users.Create(newUser);
            unitOfWork.Save();

            return JWTAuth.GenerateJWT(newUser.Username, configuration);
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromForm] User user)
        {
            if (String.IsNullOrEmpty(user.Username))
            {
                return BadRequest("Username is null or empty");
            }

            if (!unitOfWork.Users.Exists(user.Username))
            {
                return NotFound("Username doesn't exist");
            }

            return JWTAuth.GenerateJWT(user.Username, configuration);
        }
    }
}
