using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {//

        }
        public virtual DbSet<Test> test { get; set; }
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<LitMedical> LitsMedicaux { get; set; }
        public virtual DbSet<Rubrique> Rubriques { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationLit> LocationLits { get; set; }
        public virtual DbSet<LocationRubrique> LocationRubriques { get; set; }
        public virtual DbSet<Paiement> Paiements { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Historique> Historiques { get; set; }

    }
}
