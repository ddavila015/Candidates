using DataAccess.DataInterface;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class CandidateData : ICandidateData
    {
        private readonly DbContextOptions<CandidateContext> _options;

        public CandidateData(DbContextOptions<CandidateContext> options)
        {
            _options = options;
        }

        public Candidates? GetCandidateById(int idCandidate)
        {
            using (var context = new CandidateContext(_options))
            {
                return context.Candidates
                .Where(c => c.IdCandidate == idCandidate)
                .FirstOrDefault();
            }
        }

        public List<Candidates> GetAllCandidates()
        {  
            using (var context = new CandidateContext(_options))
            {
                //CONSULTAMOS TODOS LOS CANDIDATOS
                return context.Candidates.ToList(); 
            } 
        }

        public Candidateexperiences? GetCandidateExperiencesById(int idExp)
        {
            using (var context = new CandidateContext(_options))
            {
                return context.Candidateexperiences
                .Where(c => c.IdCandidateExperience == idExp)
                .FirstOrDefault();
            }
        }

        public List<Candidateexperiences> GetAllCandidateExperiencesByIdCandidate(int idCandidate)
        {
            using (var context = new CandidateContext(_options))
            {
                return context.Candidateexperiences
                .Where(c => c.IdCandidate == idCandidate)
                .ToList();
            }
        }

        public int RegisterCandidate(Candidates model, List<Candidateexperiences> exp) 
        {
            int Response = 0;

            using (var context = new CandidateContext(_options))
            {
                var newCandidate = new Candidates
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Birthdate = model.Birthdate,
                    Email = model.Email,
                    InsertDate = DateTime.Now,
                    ModifyDate = DateTime.Now
                };

                // Agregar el candidato y lo guardamos
                context.Candidates.Add(newCandidate); 
                context.SaveChanges();



                //VALIDAMOS QUE SE INSERTO EL CANDIDATO
                if (newCandidate.IdCandidate > 0 && exp.Count() > 0)
                {
                    //SETEAMOS EL ID DEL CANDIDATO PARA RETORNAR
                    Response = newCandidate.IdCandidate;

                    //Agregamos las experiencias
                    foreach (var item in exp)
                    { 
                        var newExperiences = new Candidateexperiences
                        {
                            Company = item.Company,
                            Job = item.Job,
                            Description = item.Description,
                            Salary = item.Salary,
                            BeginDate = item.BeginDate,
                            EndDate = item.EndDate,
                            InsertDate = DateTime.Now,
                            ModifyDate = DateTime.Now,
                            IdCandidate = newCandidate.IdCandidate
                        };

                        // GUARDAMOS LAS EXPERIENCIAS
                        context.Candidateexperiences.Add(newExperiences);
                        context.SaveChanges();
                    }                   
                }
            }

            return Response;
        }        
    }
}
