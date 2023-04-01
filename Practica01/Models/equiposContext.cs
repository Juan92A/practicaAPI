using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Practica01.Models
{
    public class equiposContext : DbContext
    {
        public equiposContext(DbContextOptions<equiposContext> dbContext) : base(dbContext) { 
            
      
        }

        public DbSet<equipos> equipos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<TipoEquipos> tipo_equipo { get; set; }
        public DbSet<estadosEquipos> estados_equipo { get; set; }
        public DbSet<estadosReserva> estados_reserva { get; set; }
        public DbSet<carreras> carreras { get; set; }
        public DbSet<Facultades> facultades { get; set; }
        public DbSet<Reserva> reservas { get; set; }
        public DbSet<Usuario> usuarios { get; set; }



    }
}
