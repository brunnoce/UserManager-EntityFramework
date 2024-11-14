using Microsoft.EntityFrameworkCore;
using System;
using WebAppEFCore.Models;

namespace TPFinalCe.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TPFinalCe;Trusted_Connection=True;MultipleActiveResultSets=True");
        }

        public DbSet<Socio> Socios { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Cuota> Cuotas { get; set; }
        public DbSet<Beneficios> Beneficios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
