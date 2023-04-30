using Microsoft.EntityFrameworkCore;
using WebSisMed.Models.Entities;
using WebSisMed.Models.EntityConfigurations;

namespace WebSisMed.Models.Contexts
{
    public class SisMedContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public SisMedContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Medico> Medicos => Set<Medico>();
        public DbSet<Paciente> Pacientes => Set<Paciente>();
        public DbSet<InformacoesComplementaresPaciente> InformacoesComplementares => Set<InformacoesComplementaresPaciente>();
        public DbSet<MonitoramentoPaciente> MonitoramentoPaciente => Set<MonitoramentoPaciente>();
        public DbSet<Consulta> Consultas => Set<Consulta>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("WebSisMed"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MedicoConfiguration());
            builder.ApplyConfiguration(new PacienteConfiguration());
            builder.ApplyConfiguration(new InformacoesComplementaresPacienteConfiguration());
            builder.ApplyConfiguration(new MonitoramentoPacienteConfiguration());
            builder.ApplyConfiguration(new ConsultaConfiguration());
        }
    }
}
