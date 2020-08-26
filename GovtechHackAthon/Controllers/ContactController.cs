using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovtechHackAthon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using GovtechHackAthon.Helpers;
namespace GovtechHackAthon.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {
        private IConfiguration configuration;

        public ContactController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [Route("")]
        [Route("index")]      
        public IActionResult Index()
        {
            return View("Index", new Contact());
        }
    }
}