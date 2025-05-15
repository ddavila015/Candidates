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

        public void RegisterCandidate(Candidates model, List<Candidateexperiences> exp) 
        {
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
                if (newCandidate.IdCandidate > 0) 
                {               
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
        }        
    }
}
