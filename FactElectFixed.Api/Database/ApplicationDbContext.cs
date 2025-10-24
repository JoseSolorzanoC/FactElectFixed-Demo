using FactElectFixed.Api.Features.Configuracion.Entities;
using Microsoft.EntityFrameworkCore;

namespace FactElectFixed.Api.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ConfiguracionEntity> Configuraciones => Set<ConfiguracionEntity>();
}
