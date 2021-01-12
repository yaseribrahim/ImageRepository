using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ImageRepo.Entities;
using ImageRepo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ImageRepo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImagesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        [HttpPost("image")]
        public ActionResult<string> UploadImage([FromForm] Upload upload)
        {
            var username = JWTAuth.GetUsername(HttpContext.User);
            if (username is null)
            {
                return BadRequest("Bearer token doesn't carry username");
            }

            if (!unitOfWork.Users.Exists(username))
            {
                return Unauthorized("User does not exist");
            }

            //TODO: abstract writing file to disk
            var allowedExtensions = new List<string>() { ".gif", ".png", ".jpeg", ".jpg" };
            if (!allowedExtensions.Contains(Path.GetExtension(upload.File.FileName)))
            {
                return BadRequest("File type not supported");
            }

            var imageId = Guid.NewGuid().ToString();
            string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, $"{username}/");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var fileName = imageId + Path.GetExtension(upload.File.FileName);
            var diskFilePath = Path.Combine(uploadPath, fileName);
            var path = $"/{username}/" + fileName;
            using var fileStream = new FileStream(diskFilePath, FileMode.Create);
            try
            {
                upload.File.CopyTo(fileStream);
                var image = new Image()
                {
                    Id = imageId,
                    Username = username,
                    Path = path,
                    IsPrivate = upload.IsPrivate
                };
                unitOfWork.Images.Create(image);
                unitOfWork.Save();
                return Ok(path);
            }
            catch (Exception e)
            {
                return UnprocessableEntity(e);
            }
        }

        [HttpGet("{username}")]
        public ActionResult<List<Image>> GetImages(string username)
        {
            string currentUserName = JWTAuth.GetUsername(HttpContext.User);

            if (!unitOfWork.Users.Exists(username))
            {
                return Unauthorized("User does not exist");
            }

            IEnumerable images;
            if (currentUserName == username)
            {
                images = unitOfWork.Images.GetEntities(i => i.Username.Equals(username));
            }
            else
            {
                images = unitOfWork.Images.GetEntities(i => i.Username.Equals(username) && !i.IsPrivate);
            }

            return Ok(images);
        }

        [HttpGet("public")]
        public ActionResult<List<Image>> GetImages()
        {
            return Ok(unitOfWork.Images.GetEntities(i => !i.IsPrivate));
        }
    }
}