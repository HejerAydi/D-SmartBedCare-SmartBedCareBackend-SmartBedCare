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

    }
}
