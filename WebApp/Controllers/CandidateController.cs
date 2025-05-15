using System.Diagnostics;
using DataAccess.Entities;
using Domain.DomainInterface;
using DTO;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ILogger<CandidateController> _logger;
        private readonly ICandidateDomain _CandidateDomain;

        public CandidateController(ILogger<CandidateController> logger, ICandidateDomain candidateDomain)
        {
            _logger = logger;
            _CandidateDomain = candidateDomain;
        }

        public IActionResult Index()
        { 
            ViewBag.ListCandidate = _CandidateDomain.GetAllCandidates();
            return View();
        }


        public IActionResult Create()
        {
            var newCandidate = new Candidates { Name = "Juan", Surname = "Pérez", Birthdate = new DateTime(1990, 5, 20), Email = "perez32@email.com", InsertDate = DateTime.Now, ModifyDate = DateTime.Now };

            List<Candidateexperiences> exp = new List<Candidateexperiences>();
            var candidateexperiences = new Candidateexperiences
            {
                Company = "Tech Solutions",
                Job = "Software Engineer",
                Description = "Desarrollo de aplicaciones web",
                Salary = 5000,
                BeginDate = Convert.ToDateTime("2022-01-15T00:00:00"),
                EndDate = Convert.ToDateTime("2023-01-15T00:00:00")
            };

            exp.Add(candidateexperiences);

            var response = _CandidateDomain.RegisterCandidate(newCandidate, exp);

            return View();
        }


        public IActionResult Delete(int idCandidate)
        {
            var response = _CandidateDomain.DeleteCandidate(idCandidate);
            return View();
        }

        public IActionResult Edit()
        {
            var newCandidate = new Candidates { Name = "Juan", Surname = "Pérez", Birthdate = new DateTime(1990, 5, 20), Email = "perez32@email.com", InsertDate = DateTime.Now, ModifyDate = DateTime.Now };

            List<Candidateexperiences> exp = new List<Candidateexperiences>();
            var candidateexperiences = new Candidateexperiences
            {
                Company = "Tech Solutions",
                Job = "Software Engineer",
                Description = "Desarrollo de aplicaciones web",
                Salary = 5000,
                BeginDate = Convert.ToDateTime("2022-01-15T00:00:00"),
                EndDate = Convert.ToDateTime("2023-01-15T00:00:00")
            };

            exp.Add(candidateexperiences);


            var response = _CandidateDomain.EditCandidate(newCandidate, exp);
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
