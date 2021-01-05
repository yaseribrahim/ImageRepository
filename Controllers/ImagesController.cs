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
    public class ImagesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImagesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public ActionResult<string> UploadImages([FromForm] Image image)
        {
            if (!unitOfWork.Users.Exists(image.Username))
            {
                return Unauthorized("User does not exist");
            }
            var allowedExtensions = new List<string>() { ".gif", ".png", ".jpeg", ".jpg" };
            if (!allowedExtensions.Contains(Path.GetExtension(image.File.FileName)))
            {
                return BadRequest("File type not supported");
            }

            var imageId = Guid.NewGuid().ToString();
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";
            string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, $"{image.Username}/");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var fileName = Guid.NewGuid() + Path.GetExtension(image.File.FileName);
            var filePath = Path.Combine(uploadPath, fileName);
            var fileUrl = baseUrl + $"/{image.Username}/" + fileName;
            using var fileStream = new FileStream(filePath, FileMode.Create);
            try
            {
                image.File.CopyTo(fileStream);
                return Ok(fileUrl);
            }
            catch (Exception e)
            {
                return UnprocessableEntity(e);
            }
        }
    }
}