using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class TipoBloqueoMap : EntityTypeConfiguration<TipoBloqueo>
    {
        public TipoBloqueoMap()
        {
            // Primary Key
            this.HasKey(t => t.TipoBloqueoId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TipoBloqueo");
            this.Property(t => t.TipoBloqueoId).HasColumnName("TipoBloqueoId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}