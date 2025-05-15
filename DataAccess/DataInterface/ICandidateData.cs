using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataInterface
{
    public interface ICandidateData
    {
        Candidates? GetCandidateById(int idCandidate);

        List<Candidates> GetAllCandidates();

        Candidateexperiences? GetCandidateExperiencesById(int idExp);

        List<Candidateexperiences> GetAllCandidateExperiencesByIdCandidate(int idCandidate);

        int RegisterCandidate(Candidates model, List<Candidateexperiences> exp);

        bool DeleteCandidate(int idCandidate);

        int EditCandidate(Candidates model, List<Candidateexperiences> exp);

        void EditCandidateExperiences(int IdCandidate, List<Candidateexperiences> exp);

        void SaveCandidateExperiences(int IdCandidate, List<Candidateexperiences> exp);

        bool ValidateRegisteredEmail(string email);
    }
}
