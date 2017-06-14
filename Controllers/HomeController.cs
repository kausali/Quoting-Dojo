using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace quoting_dojo.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbConnector _dbConnector;
 
        public HomeController(DbConnector connect)
        {
            _dbConnector = connect;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/error")]
        public IActionResult Process(string name, string quote)
        {
            string userName = Request.Form["name"];
            string userQuote = Request.Form["quote"];
            if (userName.Length < 3 ||userQuote.Length < 3){
                ViewBag.Status = "error";
                TempData["Messages"] = "Username and Quote should be longer than 3 characters!!";
                ViewBag.Message = TempData["Messages"];
                return View ("Error");
            }

            _dbConnector.Execute($"INSERT INTO Quotes(name, quote, created_at, updated_at) VALUES ('{name}', '{quote}', now(), now())");

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("/quotes")]
        public IActionResult Quotes ()
        {
            List<Dictionary<string, object>> AllQuotes = _dbConnector.Query("SELECT * FROM Quotes ORDER BY updated_at DESC");
            ViewBag.Quotes = AllQuotes;
            return View("Quotes");
        }

    }
}
