using DataAccess.DataInterface;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                // Agregar el candidato y lo guardamos
                context.Candidates.Add(model); 
                context.SaveChanges();
                 

                //VALIDAMOS QUE SE INSERTO EL CANDIDATO
                if (model.IdCandidate > 0 && exp.Count() > 0)
                {
                    //SETEAMOS EL ID DEL CANDIDATO PARA RETORNAR
                    Response = model.IdCandidate;

                    //Agregamos las experiencias
                    SaveCandidateExperiences(model.IdCandidate, exp);
                }
            }

            return Response;
        } 


        public bool DeleteCandidate(int idCandidate)
        {
            using (var context = new CandidateContext(_options))
            {
                //CONSULTAMOS EL CANDIDATO
                var candidate = GetCandidateById(idCandidate);

                if (candidate == null)
                    return false;
                else{
                    //CONSULTAMOS LAS EXPERIENCIAS
                    var _experiencias = GetAllCandidateExperiencesByIdCandidate(idCandidate);
                    foreach (var item in _experiencias) 
                    {
                        context.Candidateexperiences.Remove(item);
                        context.SaveChangesAsync();
                    }

                    context.Candidates.Remove(candidate);
                    context.SaveChangesAsync();
                    // RETORNAMOS VERDADERO SI FUE EXITOSO EL BORRADO
                    return true;
                }              
            }
        }

        public int EditCandidate(Candidates model, List<Candidateexperiences> exp)
        {
            int Response = 0;
            
            //MAPEAMOS LOS CAMPOS A MODIFICAR DEL CANDIDATO
            using (var context = new CandidateContext(_options))
            { 
                //ACTUALIZAMOS EL CANDIDATO
                context.Candidates.Update(model);
                context.SaveChanges();
                    
                //ACTUALIZAMOS LA EXPERIENCIAS
                EditCandidateExperiences(model.IdCandidate, exp);
                

                //SETEAMOS EL ID DEL CANDIDATO PARA RETORNAR
                Response = model.IdCandidate; 
            }           
 
            return Response;
        }


        public void EditCandidateExperiences(int IdCandidate, List<Candidateexperiences> exp) 
        {
            if (IdCandidate > 0 && exp.Count() > 0)
            {    
                //EDITAMOS LAS EXPERIENCIAS
                foreach (var item in exp)
                {
                    //VALIDAMOS QUE EXISTA LA EXPERIENCIA
                    if (item.IdCandidateExperience > 0)
                    {
                        var _experience = GetCandidateExperiencesById(item.IdCandidateExperience);
                        if (_experience != null)
                        {
                            using (var context = new CandidateContext(_options))
                            {
                                _experience.Company = item.Company;
                                _experience.Job = item.Job;
                                _experience.Description = item.Description;
                                _experience.Salary = item.Salary;
                                _experience.BeginDate = item.BeginDate;
                                _experience.EndDate = item.EndDate;
                                _experience.ModifyDate = DateTime.Now;

                                // ACTUALIZAMOS LA EXPERIENCIA
                                context.Candidateexperiences.Update(_experience);
                                context.SaveChanges();
                            }
                        }
                    }
                    else 
                    {
                        //GUARDAMOS LA NUEVA EXPERIENCIA
                        List<Candidateexperiences> SaveExp = new List<Candidateexperiences>();
                        SaveExp.Add(item);                    
                        SaveCandidateExperiences(item.IdCandidate, SaveExp);
                    }              
                }
            }
        }

        public void SaveCandidateExperiences(int IdCandidate, List<Candidateexperiences> exp)
        {
            using (var context = new CandidateContext(_options))
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
                        IdCandidate = IdCandidate
                    };

                    // GUARDAMOS LAS EXPERIENCIAS
                    context.Candidateexperiences.Add(newExperiences);
                    context.SaveChanges();
                }
            }
        }

        public bool ValidateRegisteredEmail(string _email)
        {
            using (var context = new CandidateContext(_options))
            {
                //CONSULTAMOS TODOS LOS CANDIDATOS
                var response = context.Candidates.Where(x=> x.Email == _email).FirstOrDefault();
                if (response != null)
                    return true;
                else
                    return false;
            }
        }
    }
}
