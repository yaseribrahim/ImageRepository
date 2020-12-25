//TODO: consider using namespace and placing UserContext in it aswell
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public string UserName { get; set; } = default!;
    public string Password {get; set;} = default!;
}