//TODO: consider using namespace and placing UserContext in it aswell
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public string Username { get; set; } = default!;
}