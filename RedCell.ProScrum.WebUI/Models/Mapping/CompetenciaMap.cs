using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class CompetenciaMap : EntityTypeConfiguration<Competencia>
    {
        public CompetenciaMap()
        {
            // Primary Key
            this.HasKey(t => t.CompetenciaId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Competencia");
            this.Property(t => t.CompetenciaId).HasColumnName("CompetenciaId");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.TipoActividadId).HasColumnName("TipoActividadId");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");

            // Relationships
            this.HasRequired(t => t.TipoActividad)
                .WithMany(t => t.Competencias)
                .HasForeignKey(d => d.TipoActividadId);
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.Competencias)
                .HasForeignKey(d => d.UsuarioId);

        }
    }
}
