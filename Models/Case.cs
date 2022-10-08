using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PeopleForPeople.Models;
#pragma warning disable CS8618


public class Case {

[Key]
public int CaseId { get;set; }
[Required]
public string Tittle {get; set;}
[Required]
[MinLength(20, ErrorMessage = "Description must be 20 letters or longer!")]
public string Description { get;set; }
[Required]
public string FamilySurname {get; set;}
[Required]
public string City {get; set;}
[Required]
public string Address {get; set;}
public int UserId { get; set; }
public User? Creator { get; set; }
List<Message> Messages {get; set;}


public string Myimage { get; set; } = string.Empty;

[NotMapped]
public IFormFile Image { get; set; }


public DateTime CreatedAt { get; set; } = DateTime.Now;
public DateTime UpdatedAt { get; set; } = DateTime.Now;

}