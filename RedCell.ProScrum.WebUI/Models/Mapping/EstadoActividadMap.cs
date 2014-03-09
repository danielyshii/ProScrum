using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class EstadoActividadMap : EntityTypeConfiguration<EstadoActividad>
    {
        public EstadoActividadMap()
        {
            // Primary Key
            this.HasKey(t => t.EstadoActividadId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("EstadoActividad");
            this.Property(t => t.EstadoActividadId).HasColumnName("EstadoActividadId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}