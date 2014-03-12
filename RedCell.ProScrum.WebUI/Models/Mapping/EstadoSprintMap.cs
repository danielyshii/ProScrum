using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class EstadoSprintMap : EntityTypeConfiguration<EstadoSprint>
    {
        public EstadoSprintMap()
        {
            // Primary Key
            this.HasKey(t => t.EstadoSprintId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("EstadoSprint");
            this.Property(t => t.EstadoSprintId).HasColumnName("EstadoSprintId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}