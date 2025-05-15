using DataAccess.Entities;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainInterface
{
    public interface ICandidateDomain
    {
        ResponseDto RegisterCandidate(Candidates model, List<Candidateexperiences> exp);

        List<CandidateDto> GetAllCandidates();
    }
}
