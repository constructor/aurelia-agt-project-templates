using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Site.Models;

namespace Site.Controllers
{
    /// <summary>
    /// https://auth0.com/blog/securing-asp-dot-net-core-2-applications-with-jwts/
    /// </summary>
    public class HomeController : Controller
    {
        const string AuthSchemes = "Identity.Application, " + JwtBearerDefaults.AuthenticationScheme;

        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(AuthenticationSchemes = AuthSchemes)] //No redirect to login, only 401 unauthorised
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult PrivacyApi()
        {
            return View("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
