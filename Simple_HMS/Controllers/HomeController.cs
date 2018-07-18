using Simple_HMS.Interface;
using Simple_HMS.Security;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Simple_HMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _repository;

        public HomeController(IUserRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public ActionResult login(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["error"] = "Enter username or password";
            }

            try
            {
                var user = _repository.GetUser(username);
                if (user == null)
                {
                    TempData["error"] ="User not found";
                    return RedirectToAction("index");
                }

                var principal = new HMSPrincipal(user, password);
                if (!principal.Identity.IsAuthenticated && !principal.IsInRole("officer"))
                {
                    TempData["error"] = "Login failed";
                    return RedirectToAction("index");
                }
                else
                {
                    ///This is a simple authentication. Dont used in production
                    FormsAuthentication.SetAuthCookie(username, false);

                    return RedirectToAction("index","desk");
                }
                
            }
            catch (UnauthorizedAccessException ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("index");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }
    }
}