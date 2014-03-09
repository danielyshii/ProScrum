using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class EstadoProyectoMap : EntityTypeConfiguration<EstadoProyecto>
    {
        public EstadoProyectoMap()
        {
            // Primary Key
            this.HasKey(t => t.EstadoProyectoId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("EstadoProyecto");
            this.Property(t => t.EstadoProyectoId).HasColumnName("EstadoProyectoId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}