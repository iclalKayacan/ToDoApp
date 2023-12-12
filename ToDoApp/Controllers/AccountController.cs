using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ToDoApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User userModel)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userModel.Email && u.Password == userModel.Password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserID", user.Id);
                HttpContext.Session.SetString("Email", user.Email);

                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                ViewBag.Error = "Geçersiz kullanıcı adı veya şifre";
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User userModel)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.Any(u => u.Email == userModel.Email);
                if (!existingUser)
                {
                    _context.Users.Add(userModel);
                    _context.SaveChanges();

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ViewBag.Error = "Bu email ile bir hesap zaten mevcut";
                    return View();
                }
            }

            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
