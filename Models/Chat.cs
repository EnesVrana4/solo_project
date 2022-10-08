using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PeopleForPeople.Models;
#pragma warning disable CS8618


public class Chat {
[Key]
public int ChatId {get; set; }
public int UserId {get; set;}
public int CaseId {get; set;}
public User? User {get; set;}
public Case? ConnectedWith {get; set;}
// List<Message> Messages {get; set;}


}