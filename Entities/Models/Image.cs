using Microsoft.AspNetCore.Http;

namespace ImageRepo.Entities
{
    public class Image
    {
        public string Username { get; set; }
        public IFormFile File { get; set; }
    }
}