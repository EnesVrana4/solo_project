using System.ComponentModel.DataAnnotations;
namespace PeopleForPeople.Models;
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations.Schema;


public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    // [RegularExpression(@"^[a-zA-Z]+$",   ErrorMessage = "First Name should be only letters characters!")]
    public string FirstName { get; set; }
    [Required]

    public string LastName { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
      
    
    [DataType(DataType.Password)]
    [Required]
    [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
    public string Password { get; set; }
    // [Required]
    public string City {get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Chat> Chats {get;set;} = new List<Chat>();
    public List<Case> Cases { get; set; } = new List<Case>(); 
    public List<Message> CreatedMessages { get; set; } = new List<Message>(); 

    
    
    // Will not be mapped to your users table!
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Confirm { get; set; }
    public User? CreatedCase { get; set;}

   
}
public class LoginUser
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
