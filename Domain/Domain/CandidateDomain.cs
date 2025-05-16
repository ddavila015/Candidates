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
        private readonly List<CandidateDto> _ListCandidateDto;
        


        public CandidateDomain(ICandidateData candidateData, ResponseDto responseDto, List<CandidateDto> listCandidateDto) 
        {
            _candidateData = candidateData;
            _ResponseDto = responseDto;
            _ListCandidateDto = listCandidateDto;
        }

       public ResponseDto RegisterCandidate(CandidateDto model)
       {
            //VALIDAMOS QUE EL E-MAIL ESTE REGISTRADO
            var result = _candidateData.ValidateRegisteredEmail(model.Email);
            if (result)
            {
                _ResponseDto.Mensaje = "EL EMAIL YA SE ENCUENTRA REGISTRADO.";
                _ResponseDto.Codigo = 0;
                return _ResponseDto;
            }


            //SETEAMOS EL OBJECTO CANDIDATO
            var newCandidate = new Candidates
            {
                Name = model.Name,
                Surname = model.Surname,
                Birthdate = model.Birthdate,
                Email = model.Email,
                InsertDate = DateTime.Now,
                ModifyDate = DateTime.Now
            };

            //SETEAMOS LAS EXPERIENCIAS
            List<Candidateexperiences> Listexp = new List<Candidateexperiences>();           
            foreach (var item in model.Experiencias)
            {
                Listexp.Add(new Candidateexperiences {
                    Company = item.Company,
                    Job = item.Job,
                    Description = item.Description,
                    Salary = item.Salary,
                    BeginDate = item.BeginDate,
                    EndDate = item.EndDate,
                    InsertDate = DateTime.Now,
                    ModifyDate = DateTime.Now                    
                }); 
            }

            //REGISTRAMOS EL CANDIDATO
            var response = _candidateData.RegisterCandidate(newCandidate, Listexp);
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
                _ResponseDto.Codigo = 500;
            } 

            return _ResponseDto;
        }

       public List<CandidateDto> GetAllCandidates()
       { 
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
                _ListCandidateDto.Add(NewCandidate);
            }
        
            return _ListCandidateDto;
       }

        public ResponseDto EditCandidate(Candidates model, List<Candidateexperiences> exp)
        {
            //CONSULTAR SI EXISTE EL CANDIDATO
            var candidate = _candidateData.GetCandidateById(model.IdCandidate);
            if (candidate == null)
            {
                _ResponseDto.Mensaje = "NO SE ENCONTRÓ EL CANDIDATO.";
                _ResponseDto.Codigo = 0;
                return _ResponseDto;
            }

            //VALIDAR QUE EL EMAIL ESTE REGISTRADO
            if (model.Email != candidate.Email) 
            {
                var result = _candidateData.ValidateRegisteredEmail(model.Email);
                if (result) 
                {
                    _ResponseDto.Mensaje = "EL E-MAIL YA SE ENCUENTRA REGISTRADO.";
                    _ResponseDto.Codigo = 0;
                    return _ResponseDto;
                }
            }

            //SETEAMOS EL OBJECTO CANDIDATO
            candidate.Name = model.Name;
            candidate.Surname = model.Surname;
            candidate.Birthdate = model.Birthdate;
            candidate.Email = model.Email;
            candidate.ModifyDate = DateTime.Now;

            //REGISTRAMOS EL CANDIDATO
            var response = _candidateData.EditCandidate(candidate, exp);
            if (response > 0)
            {
                _ResponseDto.Mensaje = "ACTUALIZADO EXITOSAMENTE.";
                _ResponseDto.Codigo = 200;
            }
            else
            {
                _ResponseDto.Mensaje = "ERROR AL ACTUALIZAR CANDIDATO.";
                _ResponseDto.Codigo = 0;
            }

            return _ResponseDto;
        }
    }
}
