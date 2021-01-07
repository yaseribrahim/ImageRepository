using Microsoft.AspNetCore.Http;

namespace ImageRepo.Entities
{
    public class Upload
    {
        public string Username { get; set; }
        public IFormFile File { get; set; }
        public bool IsPrivate { get; set; } = false;
    }
}