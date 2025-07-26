using CadastraEmpresas.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastraEmpresas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
    }
}