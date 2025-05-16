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
        ResponseDto RegisterCandidate(CandidateDto model);

        List<CandidateDto> GetAllCandidates();

        ResponseDto DeleteCandidate(int idCandidate);

        ResponseDto EditCandidate(CandidateEditDto model);

        CandidateEditDto GetCandidateById(int idCandidate);
    }
}
