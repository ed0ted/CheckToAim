using CheckToAim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CheckToAim.Controllers
{
    public class LoginController : Controller
    {
        CheckListDBContext context = new CheckListDBContext();
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {

            
            if (context.Users.Any(u => u.Username == user.Username))
            {
                 ModelState.AddModelError(nameof(user.Username), "Username is already exist!");
                return View();
            }
            if(context.Users.Any(u=>u.Email == user.Email))
            {
                ModelState.AddModelError(nameof(user.Email), "This email is already used");
                return View();
            }
            else
            {
                context.Users.Add(user);
                context.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = "Account created successfully!";
                return View();
            }
            
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {
           var user = context.Users.Where(a=>a.Email == login.Email&&a.Password==login.Password).FirstOrDefault();
           if(user!=null)
            {
                var Ticket = new FormsAuthenticationTicket(login.Email, true, 3000);
                string Encrypt = FormsAuthentication.Encrypt(Ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, Encrypt);
                cookie.Expires=DateTime.Now.AddDays(1);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                if(user.RoleId==2)
                {
                    return RedirectToAction("UserProfile", "Home");
                }
                else
                {
                    return RedirectToAction("AdminArea", "Home");
                }

            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Home", "Home");
        }
    } 
}