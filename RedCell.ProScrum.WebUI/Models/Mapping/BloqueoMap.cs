using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class BloqueoMap : EntityTypeConfiguration<Bloqueo>
    {
        public BloqueoMap()
        {
            // Primary Key
            this.HasKey(t => t.BloqueoId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Bloqueo");
            this.Property(t => t.BloqueoId).HasColumnName("BloqueoId");
            this.Property(t => t.TipoBloqueoId).HasColumnName("TipoBloqueoId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");

            // Relationships
            this.HasRequired(t => t.TipoBloqueo)
                .WithMany(t => t.Bloqueos)
                .HasForeignKey(d => d.TipoBloqueoId);

        }
    }
}