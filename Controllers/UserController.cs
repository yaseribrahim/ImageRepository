using System;
using ImageRepo.Entities;
using ImageRepo.Helpers;
using Microsoft.AspNetCore.Authorization;
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

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost("Signup")]
        public ActionResult<string> PostUser([FromForm] User newUser)
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
    }
}
