using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class SprintMap : EntityTypeConfiguration<Sprint>
    {
        public SprintMap()
        {
            // Primary Key
            this.HasKey(t => t.SprintId);

            // Properties
            this.Property(t => t.Objetivo)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Sprint");
            this.Property(t => t.SprintId).HasColumnName("SprintId");
            this.Property(t => t.ProyectoId).HasColumnName("ProyectoId");
            this.Property(t => t.Objetivo).HasColumnName("Objetivo");
            this.Property(t => t.Nombre).HasColumnName("Nombre");
            this.Property(t => t.Inicio).HasColumnName("Inicio");
            this.Property(t => t.Fin).HasColumnName("Fin");
            this.Property(t => t.EstadoId).HasColumnName("EstadoId");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");

            // Relationships
            this.HasRequired(t => t.Proyecto)
                .WithMany(t => t.Sprints)
                .HasForeignKey(d => d.ProyectoId);

        }
    }
}
