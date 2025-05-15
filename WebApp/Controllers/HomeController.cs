using System.Diagnostics;
using DataAccess.Entities;
using Domain.DomainInterface;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICandidateDomain _CandidateDomain;

        public HomeController(ILogger<HomeController> logger, ICandidateDomain candidateDomain)
        {
            _logger = logger;
            _CandidateDomain = candidateDomain;
        }

        public IActionResult Index()
        {
           
            var newCandidate = new Candidates { Name = "Juan", Surname = "Pérez", Birthdate = new DateTime(1990, 5, 20), Email = "juan.perez@email.com", InsertDate = DateTime.Now, ModifyDate = DateTime.Now };

            List<Candidateexperiences> exp = new List<Candidateexperiences>();
            var candidateexperiences = new Candidateexperiences {
                Company = "Tech Solutions",
                Job = "Software Engineer",
                Description =  "Desarrollo de aplicaciones web",
                Salary = 5000,
                BeginDate = Convert.ToDateTime("2022-01-15T00:00:00"),
                EndDate = Convert.ToDateTime("2023-01-15T00:00:00")
            };

            exp.Add(candidateexperiences);



            _CandidateDomain.RegisterCandidate(newCandidate, exp);
            return View();
        }

        public IActionResult Privacy()
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
