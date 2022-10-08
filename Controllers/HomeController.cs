using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PeopleForPeople.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
#pragma warning disable CS8618



namespace PeopleForPeople.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment WebHostEnvironment;

    private MyContext _context;

    // here we can "inject" our context service into the constructor
    public HomeController(ILogger<HomeController> logger, MyContext context, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _context = context;
        WebHostEnvironment = webHostEnvironment;
    }
    [HttpGet("")]
    public RedirectToActionResult Index()
    {
        return RedirectToAction("Login");
    }
    [HttpGet("Register")]
    public IActionResult Register()
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return View();
        }
        return RedirectToAction("HomePage");
    }
    [HttpPost("Register")]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
      
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already in use!");

                return View();
                // You may consider returning to the View at this point
            }
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            user.Password = Hasher.HashPassword(user, user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("userId", user.UserId);
           
            return RedirectToAction("HomePage");
        }
        return View();
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return View();
        }
        return RedirectToAction("HomePage");
    }
    [HttpPost("Login")]
    public IActionResult Login(LoginUser userSubmission)
    {
        if (ModelState.IsValid)
        {
            // If initial ModelState is valid, query for a user with provided email
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
            // If no user exists with provided email
            if (userInDb == null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("User", "Invalid Email/Password");
                return View("Login");
            }

            // Initialize hasher object
            var hasher = new PasswordHasher<LoginUser>();

            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

            // result can be compared to 0 for failure
            if (result == 0)
            {
                ModelState.AddModelError("Password", "Invalid Password");
                return View("Login");
                // handle failure (this should be similar to how "existing email" is handled)
            }
            HttpContext.Session.SetInt32("userId", userInDb.UserId);

            return RedirectToAction("HomePage");
        }

        return View("Login");
    }

    [HttpGet("Logout")]
    public IActionResult Logout()
    {

        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }



    [HttpGet("HomePage")]
    public IActionResult HomePage()
    {
        if (HttpContext.Session.GetInt32("userId") == null)
            {
                return RedirectToAction("Login");
            }
            int id = (int)HttpContext.Session.GetInt32("userId");

          
        
        // Marr te loguarin me te dhena dhe cases
        ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == id);
        ViewBag.cases = _context.Cases.OrderByDescending(e => e.CreatedAt).Include(e => e.Creator).ToList();


        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [HttpGet("/Case/Add")]
    public IActionResult CaseAdd()
    {
         if (HttpContext.Session.GetInt32("userId") == null)
            {
                return RedirectToAction("Login");
            }
        return View("CaseAdd");
    }

    [HttpPost("/Case/Add")]
        public IActionResult CaseAdd(Case marrNgaView,string Description)
        {
             if (HttpContext.Session.GetInt32("userId") == null)
            {
                return RedirectToAction("Login");
            }
            int id = (int)HttpContext.Session.GetInt32("userId");            
           
        if (Description == null)
        {
            {
            ModelState.AddModelError("Description", "Description must be 20 letters or longer!");
            ViewBag.Cases = _context.Cases.Include(e => e.Creator).ToList();

            
            //Marr te loguarin me te dhena
            ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == id);
            return View("CaseAdd");
            
        }
        }
        if (Description.Length < 20)
        {
            ModelState.AddModelError("Description", "Description must be 20 letters or longer!");
               ViewBag.Cases = _context.Cases.Include(e => e.Creator).ToList();

            
            //Marr te loguarin me te dhena
            ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == id);
            return View("CaseAdd");
            
        }


        string StringFileName = UploadFile(marrNgaView);

        Case newCase = new Case(){ // Krijohet nje objekt CASE
            UserId = id,
            Tittle = marrNgaView.Tittle,
            Description = Description,
            FamilySurname = marrNgaView.FamilySurname,
            City = marrNgaView.City,
            Address = marrNgaView.Address,
            Myimage = StringFileName
        };
        _context.Cases.Add(newCase); // objekti CASE behet gati per tu rujt ne DB (ne kte moment nuk ka nje ID pasi ID eshte automatike (AUTOINCREMENT))
        _context.SaveChanges(); // ne momentin qe ben SaveChanges() nga context-i ky Object ruhet ne DB dhe krijohet ID e ktij objektit

        Chat newChat = new Chat(){
            CaseId = newCase.CaseId, // mqns eshte krijuar ID e objektit atehere ate ID mund ta marrim dhe ta vendosim tek CHAT
            UserId = id
        };
        _context.Chats.Add(newChat);
        _context.SaveChanges();
       
       
        return RedirectToAction("HomePage");
    }
    private string UploadFile(Case marrNgaView)
    {
       string fileName = null;
       if(marrNgaView.Image!=null){
        string Uploaddir = Path.Combine(WebHostEnvironment.WebRootPath,"Images");
        fileName = Guid.NewGuid().ToString() + "-" + marrNgaView.Image.FileName;
        string filePath = Path.Combine(Uploaddir,fileName);
        using (var filestream = new FileStream(filePath,FileMode.Create))
        {
                marrNgaView.Image.CopyTo(filestream);
        }
       }
       return fileName;
    }

    

[HttpGet("/Case/Delete/{id}")]
    public IActionResult Delete(int id)
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Login");
        }
        Case deleteCase = _context.Cases.First(e => e.CaseId == id);
        _context.Cases.Remove(deleteCase);
        _context.SaveChanges();
        List<Message> MessagesOfChat = _context.Messages.Where(a => a.CaseId == id).ToList();
        foreach (var message in MessagesOfChat)
        {
            _context.Messages.Remove(message);
            _context.SaveChanges();
        }
        List<Chat> ChatsOfCase = _context.Chats.Where(a => a.CaseId == id).ToList();
        foreach (var chatofcase in ChatsOfCase)
        {
            _context.Chats.Remove(chatofcase);
            _context.SaveChanges();
        }
        return RedirectToAction("HomePage");
    }


    [HttpGet("HomePage/{id}")]
    public IActionResult TheCase(int id)
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Login");
        }
        int idFromSession = (int)HttpContext.Session.GetInt32("userId");           
        ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == idFromSession);
        ViewBag.theCase = _context.Cases.Include(a => a.Creator).First(a=> a.CaseId == id);
        ViewBag.CaseChatWithUsers = _context.Chats.Where(e => e.CaseId == id).ToList();
        
        
        return View("TheCase");
    }


    [HttpGet("/AddUserToChat/{caseId}")]
    public IActionResult AddUserToChat(int caseId)
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Login");
        }
        int idFromSession = (int)HttpContext.Session.GetInt32("userId");  

        Chat newUserInChat = new Chat(){
            CaseId = caseId,
            UserId = idFromSession
        };

        _context.Chats.Add(newUserInChat);
        _context.SaveChanges();

        return RedirectToAction("TheCase", new { id = caseId });
        // return RedirectToAction("TheCase(int id)");
    }
    [HttpGet("/RemoveUserFromChat/{caseId}")]
    public IActionResult RemoveUserFromChat(int caseId)
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Login");
        }
        int id = (int)HttpContext.Session.GetInt32("userId");  

        Chat deleteUserFromChat = _context.Chats.First(e => e.CaseId == caseId && e.UserId == id);
        _context.Chats.Remove(deleteUserFromChat);
        _context.SaveChanges();

        return RedirectToAction("TheCase", new { id = caseId, });
        // return RedirectToAction("TheCase(int id)");
    }

    [HttpGet("/Chats")]
    public IActionResult Chats()
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Login");
        }
        int idFromSession = (int)HttpContext.Session.GetInt32("userId");            
        ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == idFromSession);
        ViewBag.chatsOfUser = _context.Chats.Where(chat => chat.UserId == idFromSession).Include(a => a.ConnectedWith).ToList();
        return View("GroupChats");
    }


    [HttpGet("Chat/{caseId}")]
    public IActionResult groupChat(int caseId)
    {
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Login");
        }
        int idFromSession = (int)HttpContext.Session.GetInt32("userId");            
        ViewBag.iLoguari = _context.Users.FirstOrDefault(e => e.UserId == idFromSession);
        ViewBag.caseIdToView = caseId;
        ViewBag.Messages = _context.Messages.OrderByDescending(a => a.CreatedAt).Include(b => b.Creator).Where(a => a.CaseId == caseId).ToList();
        return View("GroupChat");
    }

    [HttpPost("/AddMessage/{id}")]
    public IActionResult addMessage(Message messageFromView , int id){
        if (ModelState.IsValid)
        {
            
        if (HttpContext.Session.GetInt32("userId") == null)
        {
            return RedirectToAction("Login");
        }
        int idFromSession = (int)HttpContext.Session.GetInt32("userId"); 

        Message newMessage = new Message(){
            MessageContent = messageFromView.MessageContent,
            UserId = idFromSession,
            CaseId = id
        };
        _context.Messages.Add(newMessage);
        _context.SaveChanges();
        }

        return RedirectToAction("groupChat", new { caseId = id });





    }
}


