using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class ActividadMap : EntityTypeConfiguration<Actividad>
    {
        public ActividadMap()
        {
            // Primary Key
            this.HasKey(t => t.ActividadId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Actividad");
            this.Property(t => t.ActividadId).HasColumnName("ActividadId");
            this.Property(t => t.UserStoryId).HasColumnName("UserStoryId");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.TipoActividadId).HasColumnName("TipoActividadId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
            this.Property(t => t.Inicio).HasColumnName("Inicio");
            this.Property(t => t.Fin).HasColumnName("Fin");
            this.Property(t => t.EstadoId).HasColumnName("EstadoId");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");

            // Relationships
            this.HasRequired(t => t.TipoActividad)
                .WithMany(t => t.Actividads)
                .HasForeignKey(d => d.TipoActividadId);
            this.HasRequired(t => t.UserStory)
                .WithMany(t => t.Actividads)
                .HasForeignKey(d => d.UserStoryId);
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.Actividads)
                .HasForeignKey(d => d.UsuarioId);

        }
    }
}
