using DataAccess.DataInterface;
using DataAccess.Entities;
using DTO;
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
        private readonly CandidateContext context;

        public CandidateData(DbContextOptions<CandidateContext> options, CandidateContext _context)
        {
            _options = options;
            context = _context;
        }

        public Candidates? GetCandidateById(int idCandidate)
        {            
            return context.Candidates.Where(c => c.IdCandidate == idCandidate).FirstOrDefault();            
        }

        public List<Candidates> GetAllCandidates()
        {              
            return context.Candidates.ToList();            
        }

        public Candidateexperiences? GetCandidateExperiencesById(int idExp)
        {            
            return context.Candidateexperiences.Where(c => c.IdCandidateExperience == idExp).FirstOrDefault();             
        }

        public List<Candidateexperiences> GetAllCandidateExperiencesByIdCandidate(int idCandidate)
        {            
             return context.Candidateexperiences.Where(c => c.IdCandidate == idCandidate).ToList();           
        }

        public int RegisterCandidate(Candidates model, List<Candidateexperiences> exp) 
        {
            int Response = 0;
                        
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

            return Response;
        } 


        public bool DeleteCandidate(int idCandidate)
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
                    //ELIMINAMOS LAS EXPERIENCIAS
                    context.Candidateexperiences.Remove(item);
                    context.SaveChanges();
                }

                //ELIMINAMOS EL CANDIDATO
                context.Candidates.Remove(candidate);
                context.SaveChanges();
                // RETORNAMOS VERDADERO SI FUE EXITOSO EL BORRADO
                return true;
            }                          
        }

        public int EditCandidate(Candidates model, List<Candidateexperiences> exp)
        {            
            //ACTUALIZAMOS EL CANDIDATO
            context.Candidates.Update(model);
            context.SaveChanges();
                    
            //ACTUALIZAMOS LA EXPERIENCIAS
            EditCandidateExperiences(model.IdCandidate, exp);
          
            return model.IdCandidate;
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
                            context.Candidateexperiences.Remove(_experience);
                            context.SaveChanges(); 
                        }
                    } 
                }

                //GUARDAR TODAS LAS EXPERIENCIAS
                SaveCandidateExperiences(IdCandidate, exp);
            }
        }

        public void SaveCandidateExperiences(int IdCandidate, List<Candidateexperiences> exp)
        {          
            //Agregamos las experiencias
            foreach (var item in exp)
            {
                // GUARDAMOS LAS EXPERIENCIAS
                item.IdCandidateExperience = 0;
                item.IdCandidate = IdCandidate;
                context.Candidateexperiences.Add(item);
                context.SaveChanges();
            }             
        }

        public bool ValidateRegisteredEmail(string _email)
        {                        
            var response = context.Candidates.Where(x=> x.Email == _email).FirstOrDefault();
            if (response != null)
                return true;
            else
                return false;            
        }
    }
}
