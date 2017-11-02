using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wedding.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace wedding.Controllers
{
    public class HomeController : Controller
    {
        private weddingContext _context;
        public HomeController(weddingContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            if(TempData["user_id"] != null)
            {
                System.Console.WriteLine(TempData["user_id"]);
                int uid = Convert.ToInt32(TempData["user_id"]);
                User user = _context.users.SingleOrDefault(User => User.userID == uid);
                @ViewBag.uname = user.first;
                List<Wedding> AllWeddings = _context.weddings.ToList();
                @ViewBag.weddings = AllWeddings;
                TempData["user_id"] = TempData["user_id"];
                return View("Dashboard");
            }
            else{
                @ViewBag.errors = "Unauthorized access attempt.";
                return View("Index");
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(LogReg incoming)
        {
            RegistrationViewModel registrant = new RegistrationViewModel();
            registrant = incoming.reg;
            List<User> check = _context.users.Where(User=>User.email == registrant.email).ToList();
            if(check.Count>0)
            {
                @ViewBag.errors = "Account already exists. Please login.";
                return View("Index", incoming);
            }
            TryValidateModel(registrant);
            if(ModelState.IsValid)
            {
                User newUser = new User();
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.password = Hasher.HashPassword(newUser, registrant.password);
                newUser.first = registrant.first;
                newUser.last = registrant.last;
                newUser.email = registrant.email;
                List<RSVP> rsvps = new List<RSVP>();
                newUser.rsvped = rsvps;
                newUser.created_at = DateTime.Now;
                newUser.updated_at = DateTime.Now;
                _context.users.Add(newUser);
                _context.SaveChanges();
                User confirmed = _context.users.Last();
                TempData["user_id"] = confirmed.userID;
                return View("Dashboard");
            }
            else
            {
                System.Console.WriteLine("We have an invalid registration");
            }
            return View("Index", incoming);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LogReg incoming)
        {
            LoginViewModel user = new LoginViewModel();
            user = incoming.login;
            TryValidateModel(user);
            if(ModelState.IsValid)
            {
                User identified = _context.users.SingleOrDefault(User=>User.email == user.lemail);
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(identified, identified.password, user.pwd))
                {
                    TempData["user_id"] = identified.userID;
                    return RedirectToAction("Dashboard");
                }
                else{
                    System.Console.WriteLine("Danger Will Robinson!");
                }
            }
            @ViewBag.errors = "Unable to authenticate account";
            System.Console.WriteLine("We have an invalid login");
            return View("Index", incoming);
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            TempData.Remove("user_id");
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("planning")]
        public IActionResult Planning()
        {
            if(TempData["user_id"] != null)
            {
                int uid = Convert.ToInt32(TempData["user_id"]);
                User user = _context.users.SingleOrDefault(User => User.userID == uid);
                @ViewBag.uname = user.first;
                List<Wedding> AllWeddings = _context.weddings.ToList();
                TempData["user_id"] = TempData["user_id"];
                return View("Planning");
            }
            else{
                @ViewBag.errors = "Unauthorized access attempt.";
                return View("Index");
            }
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(PlanningViewModel model)
        {
            TempData["user_id"] = TempData["user_id"];
            int uid = Convert.ToInt32(TempData["user_id"]);
            User user = _context.users.SingleOrDefault(User => User.userID == uid);
            @ViewBag.uname = user;
            if(ModelState.IsValid)
            {
                Wedding newWed = new Wedding();
                // public int ID { get; set; } - auto incrimented
                // public string bride { get; set; } = model.bride
                newWed.bride = model.bride;
                // public string groom { get; set; } = model.groom
                newWed.groom = model.groom;
                // public DateTime date { get; set; } = model.date
                newWed.date = model.date;               
                // public string address { get; set; } = model.address
                newWed.address = model.address;
                // public DateTime created_at { get; set; } = DateTime.Now
                newWed.created_at = DateTime.Now;
                // public DateTime updated_at { get; set; } = DateTime.Now
                newWed.updated_at = DateTime.Now;
                // public User planner { get; set; }
                newWed.planner = user;
                // public int userID { get; set; }
                newWed.userID = uid;
                // public List<RSVP> attendees { get; set; }
                List<RSVP> WeddingRSVPs = new List<RSVP>();
                newWed.attendees = WeddingRSVPs;
                _context.weddings.Add(newWed);
                _context.SaveChanges();
                @ViewBag.uname = user.first;
                return RedirectToAction("Dashboard");
            }
            else
            {
                TempData["user_id"] = TempData["user_id"];
                return View("Planning", model);
            }
        }
        [HttpGet]
        [Route("display/{ID}")]
        public IActionResult Display(int ID)
        {
            
            if(TempData["user_id"] != null)
            {
                int uid = Convert.ToInt32(TempData["user_id"]);
                User user = _context.users.SingleOrDefault(User => User.userID == uid);
                @ViewBag.uname = user.first;
                TempData["user_id"] = TempData["user_id"];
                Wedding selected = _context.weddings.SingleOrDefault(Wedding => Wedding.ID == ID);
                ViewBag.wedding = selected;
                return View("Display");
            }
            else{
                @ViewBag.errors = "Unauthorized access attempt.";
                return View("Index");
            }
        }
    }
}
