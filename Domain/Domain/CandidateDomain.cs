using DataAccess;
using DataAccess.DataInterface;
using DataAccess.Entities;
using Domain.DomainInterface;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CandidateDomain : ICandidateDomain
    {
        private readonly ICandidateData _candidateData;
        private readonly ResponseDto _ResponseDto;

        public CandidateDomain(ICandidateData candidateData, ResponseDto responseDto) 
        {
            _candidateData = candidateData;
            _ResponseDto = responseDto;
        }

       public ResponseDto RegisterCandidate(Candidates model, List<Candidateexperiences> exp)
       { 
            //REGISTRAMOS EL CANDIDATO
            var response = _candidateData.RegisterCandidate(model, exp);
            if (response > 0)
            {
                _ResponseDto.Mensaje = "REGISTRADO EXITOSAMENTE.";
                _ResponseDto.Codigo = 200;
            }
            else 
            {
                _ResponseDto.Mensaje = "ERROR AL REGISTRAR CANDIDATO.";
                _ResponseDto.Codigo = 0;
            }

            return _ResponseDto;
       }

        public ResponseDto DeleteCandidate(int idCandidate)
        {           
            var response = _candidateData.DeleteCandidate(idCandidate);
            if (response == true)
            {
                _ResponseDto.Mensaje = "BORRADO EXITOSAMENTE.";
                _ResponseDto.Codigo = 200;
            }
            else 
            {
                _ResponseDto.Mensaje = "ERROR AL BORRAR EL CANDIDATO.";
                _ResponseDto.Codigo = 0;
            } 

            return _ResponseDto;
        }

        public List<CandidateDto> GetAllCandidates()
       {
            List<CandidateDto> ListCandidates = new List<CandidateDto>();    

            //CONSULTAR TODOS LOS CANDIDATOS
            var _candidates = _candidateData.GetAllCandidates();              
            foreach (var itemCandidate in _candidates)
            {

                //CONSULTAMOS LAS EXPERIENCIAS
                List<CandidateexperiencesDto> ListExperiences = new List<CandidateexperiencesDto>();
                var _experiences = _candidateData.GetAllCandidateExperiencesByIdCandidate(itemCandidate.IdCandidate);
                foreach (var itemExp in _experiences)
                {
                    //CREAMOS UN OBJECTO PARA LAS EXPERIENCIAS
                    var NewExperience = new CandidateexperiencesDto
                    {
                        Company = itemExp.Company,
                        Job = itemExp.Job,
                        Description = itemExp.Description,
                        Salary = itemExp.Salary,
                        BeginDate = itemExp.BeginDate,
                        EndDate = itemExp.EndDate,
                        InsertDate = itemExp.InsertDate,
                        ModifyDate = itemExp.ModifyDate
                    };

                    ListExperiences.Add(NewExperience);
                }


                //CREAMOS UN OBJECTO PARA EL CANDIDATO
                var NewCandidate = new CandidateDto
                {
                    Name = itemCandidate.Name,
                    Surname = itemCandidate.Surname,
                    Birthdate = itemCandidate.Birthdate,
                    Email = itemCandidate.Email,
                    InsertDate = itemCandidate.InsertDate,
                    ModifyDate = itemCandidate.ModifyDate,
                    IdCandidate = itemCandidate.IdCandidate,
                    Experiencias = ListExperiences
                };

                //AGREGAMOS LOS CANDIDATOS
                ListCandidates.Add(NewCandidate);
            }
        
            return ListCandidates;
       }
    }
}
