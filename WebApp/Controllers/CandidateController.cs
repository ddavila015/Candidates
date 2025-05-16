using System.Diagnostics;
using System.Globalization;
using Azure;
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
         
        public IActionResult Index(string Mensaje, int Codigo)
        {
            ViewBag.Mensaje = string.IsNullOrWhiteSpace(Mensaje) ? null : Mensaje;
            ViewBag.Codigo = Codigo;
            ViewBag.ListCandidate = _CandidateDomain.GetAllCandidates();
            return View();
        }
          
        public IActionResult Create()
        {            
            CandidateDto model = new CandidateDto();
            AddBlankExperience(model);     
            ViewBag.Mensaje = null;
            ViewBag.Codigo = null;
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CandidateDto model)
        {           
            var response = _CandidateDomain.RegisterCandidate(model);
            ViewBag.Mensaje = response.Mensaje;
            ViewBag.Codigo = response.Codigo; 

            return View(model);
        }


        public IActionResult Delete(int id)
        {
            var response = _CandidateDomain.DeleteCandidate(id);            
            return RedirectToAction("Index", new { Mensaje = response.Mensaje, Codigo = response.Codigo });            
        }
         
        public IActionResult Edit(int id)
        {
            //CONSULTAMOS EL CANDIDATO
            CandidateEditDto _Candidate = _CandidateDomain.GetCandidateById(id);

            //VALIDAMOS SI TIENE EXPERIENCIAS
            if(_Candidate.Experiencias.Count() <= 0)
                EditCandidateAddBlankExperience(_Candidate);

            ViewBag.Mensaje = null;
            ViewBag.Codigo = null;
            return View(_Candidate);
        }

        [HttpPost]
        public IActionResult Edit(CandidateEditDto model)
        { 
            var response = _CandidateDomain.EditCandidate(model);
            ViewBag.Mensaje = response.Mensaje;
            ViewBag.Codigo = response.Codigo;
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void AddBlankExperience(CandidateDto model) 
        {
            List<CandidateexperiencesDto> exp = new List<CandidateexperiencesDto>();
            exp.Add(new CandidateexperiencesDto { });            
            model.Experiencias = exp;
        }

        public void EditCandidateAddBlankExperience(CandidateEditDto model)
        {
            List<CandidateExperiencesEditDto> exp = new List<CandidateExperiencesEditDto>();
            exp.Add(new CandidateExperiencesEditDto { });
            model.Experiencias = exp;
        }
    }
}
