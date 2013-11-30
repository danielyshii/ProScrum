using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using RedCell.ProScrum.WebUI.Models.Mapping;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class ProScrumContext : DbContext
    {
        static ProScrumContext()
        {
            Database.SetInitializer<ProScrumContext>(null);
        }

        public ProScrumContext()
            : base("Name=ProScrumConnection")
        {
        }

        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<IntegranteProyecto> IntegrantesProyecto { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<TipoActividad> TipoActividades { get; set; }
        public DbSet<UserStory> UserStories { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ActividadMap());
            modelBuilder.Configurations.Add(new CompetenciaMap());
            modelBuilder.Configurations.Add(new ContactoMap());
            modelBuilder.Configurations.Add(new EmpresaMap());
            modelBuilder.Configurations.Add(new IntegranteProyectoMap());
            modelBuilder.Configurations.Add(new ProyectoMap());
            modelBuilder.Configurations.Add(new SprintMap());
            modelBuilder.Configurations.Add(new TipoActividadMap());
            modelBuilder.Configurations.Add(new UserStoryMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
        }
    }
}