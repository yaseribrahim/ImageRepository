using System.Text.Json.Serialization;

namespace ImageRepo.Entities
{
    public class Image
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Path { get; set; }
        public bool IsPrivate { get; set; }
    }
}