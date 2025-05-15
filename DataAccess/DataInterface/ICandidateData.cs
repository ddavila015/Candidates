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
        void RegisterCandidate(Candidates model, List<Candidateexperiences> exp);
    }
}
