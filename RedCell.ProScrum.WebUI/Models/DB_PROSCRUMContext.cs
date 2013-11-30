using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using RedCell.ProScrum.WebUI.Models.Mapping;

namespace RedCell.ProScrum.WebUI.Models
{
    public partial class DB_PROSCRUMContext : DbContext
    {
        static DB_PROSCRUMContext()
        {
            Database.SetInitializer<DB_PROSCRUMContext>(null);
        }

        public DB_PROSCRUMContext()
            : base("Name=DB_PROSCRUMContext")
        {
        }

        public DbSet<Actividad> Actividads { get; set; }
        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Contacto> Contactoes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<IntegranteProyecto> IntegranteProyectoes { get; set; }
        public DbSet<Proyecto> Proyectoes { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<TipoActividad> TipoActividads { get; set; }
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
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new TipoActividadMap());
            modelBuilder.Configurations.Add(new UserStoryMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
        }
    }
}
