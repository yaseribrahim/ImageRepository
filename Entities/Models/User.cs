using System.ComponentModel.DataAnnotations;
namespace ImageRepo.Entities
{
    public class User
    {
        [Key]
        public string Username { get; set; } = default!;
    }
}