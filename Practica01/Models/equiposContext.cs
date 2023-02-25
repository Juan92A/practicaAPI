using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Practica01.Models
{
    public class equiposContext : DbContext
    {
        public equiposContext(DbContextOptions<equiposContext> dbContext) : base(dbContext) { 
            
      
        }

        public DbSet<equipos> equipos { get; set; }



    }
}
