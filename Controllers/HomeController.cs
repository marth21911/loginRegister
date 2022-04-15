using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using loginRegisterDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace loginRegisterDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost("users/create")]
        public IActionResult CreateUser(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(s=>s.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                _context.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Success");
            } else {

                return View("Index");
            }
        }
        [HttpPost("users/login")]
        public IActionResult LoginUser(LogUser loginUser)
        {
            if(ModelState.IsValid)
            {
                User userInDb = _context.Users.FirstOrDefault(d =>d.Email == loginUser.LogEmail);
                if(userInDb ==null)
                {
                    Console.WriteLine("Model Error");
                    ModelState.AddModelError("LogEmail", "Invalid email/password");
                    return View ("Login");
                }
                PasswordHasher<LogUser> hasher =new PasswordHasher<LogUser>();
                var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LogPassword);
                if(result ==0)
                {
                    Console.WriteLine ("PW/Email error");
                    ModelState.AddModelError("LogPassword", "Invalid email/password");
                    return View ("Login"); 
                }
                Console.WriteLine("This should have worked. Check route");
                return RedirectToAction("Success");
            }else {
                Console.WriteLine("modelstate invalid");
            return View ("Login");
            }

        }
        [HttpGet("Login")]
            public IActionResult Login()
            {
                return View();
            }
        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}