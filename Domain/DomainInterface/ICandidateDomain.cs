using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainInterface
{
    public interface ICandidateDomain
    {
        void RegisterCandidate(Candidates model, List<Candidateexperiences> exp);
    }
}
