using DataAccess.DataInterface;
using DataAccess.Entities;
using Domain.DomainInterface;
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

        public CandidateDomain(ICandidateData candidateData) 
        {
            _candidateData = candidateData;
        }

       public void RegisterCandidate(Candidates model, List<Candidateexperiences> exp)
       {
            _candidateData.RegisterCandidate(model, exp);
       }
    }
}
