using System;
using System.Collections.Generic;
using System.IO;
using ImageRepo.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ImageRepo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImageController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public ActionResult<string> UploadImage([FromForm] Upload upload)
        {
            if (!unitOfWork.Users.Exists(upload.Username))
            {
                return Unauthorized("User does not exist");
            }
            var allowedExtensions = new List<string>() { ".gif", ".png", ".jpeg", ".jpg" };
            if (!allowedExtensions.Contains(Path.GetExtension(upload.File.FileName)))
            {
                return BadRequest("File type not supported");
            }

            var imageId = Guid.NewGuid().ToString();
            //TODO: abstract writing file to disk
            string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, $"{upload.Username}/");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var fileName = imageId + Path.GetExtension(upload.File.FileName);
            var diskFilePath = Path.Combine(uploadPath, fileName);
            var path = $"/{upload.Username}/" + fileName;
            using var fileStream = new FileStream(diskFilePath, FileMode.Create);
            try
            {
                upload.File.CopyTo(fileStream);
                var image = new Image()
                {
                    Id = imageId,
                    Username = upload.Username,
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
            if (!unitOfWork.Users.Exists(username))
            {
                return Unauthorized("User does not exist");
            }

            return Ok(unitOfWork.Images.GetEntities(i => i.Username.Equals(username)));
        }

        [HttpGet("public")]
        public ActionResult<List<Image>> GetImages()
        {
            return Ok(unitOfWork.Images.GetEntities(i => !i.IsPrivate));
        }
    }
}