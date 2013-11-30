using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class TipoActividadMap : EntityTypeConfiguration<TipoActividad>
    {
        public TipoActividadMap()
        {
            // Primary Key
            this.HasKey(t => t.TipoActividadId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TipoActividad");
            this.Property(t => t.TipoActividadId).HasColumnName("TipoActividadId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}
