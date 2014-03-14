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
        public DbSet<EstadoActividad> EstadoActividades { get; set; }
        public DbSet<EstadoProyecto> EstadoProyectos { get; set; }
        public DbSet<EstadoSprint> EstadoSprints { get; set; }
        public DbSet<EstadoUserStory> EstadoUserStories { get; set; }
        public DbSet<IntegranteProyecto> IntegrantesProyecto { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<TipoActividad> TipoActividades { get; set; }
        public DbSet<UserStory> UserStories { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Bloqueo> Bloqueos { get; set; }
        public DbSet<TipoBloqueo> TipoBloqueos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new ActividadMap());
            modelBuilder.Configurations.Add(new CompetenciaMap());
            modelBuilder.Configurations.Add(new ContactoMap());
            modelBuilder.Configurations.Add(new EmpresaMap());
            modelBuilder.Configurations.Add(new EstadoActividadMap());
            modelBuilder.Configurations.Add(new EstadoProyectoMap());
            modelBuilder.Configurations.Add(new EstadoSprintMap());
            modelBuilder.Configurations.Add(new EstadoUserStoryMap());
            modelBuilder.Configurations.Add(new IntegranteProyectoMap());
            modelBuilder.Configurations.Add(new ProyectoMap());
            modelBuilder.Configurations.Add(new SprintMap());
            modelBuilder.Configurations.Add(new TipoActividadMap());
            modelBuilder.Configurations.Add(new UserStoryMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new BloqueoMap());
            modelBuilder.Configurations.Add(new TipoBloqueoMap());
        }
    }
}
