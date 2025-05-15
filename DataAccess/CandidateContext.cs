using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public  class CandidateContext : DbContext
    {

        public CandidateContext(DbContextOptions<CandidateContext> options) : base(options)
        {
        
        }

        public DbSet<Candidates> Candidates { get; set; }
        public DbSet<Candidateexperiences> Candidateexperiences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //clave alternativa única
            modelBuilder.Entity<Candidates>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
