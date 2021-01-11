using Microsoft.AspNetCore.Http;

namespace ImageRepo.Entities
{
    public class Upload
    {
        public IFormFile File { get; set; }
        public bool IsPrivate { get; set; } = false;
    }
}