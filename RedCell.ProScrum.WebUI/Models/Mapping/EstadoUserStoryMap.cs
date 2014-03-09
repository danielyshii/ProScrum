using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class EstadoUserStoryMap : EntityTypeConfiguration<EstadoUserStory>
    {
        public EstadoUserStoryMap()
        {
            // Primary Key
            this.HasKey(t => t.EstadoUserStoryId);

            // Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("EstadoUserStory");
            this.Property(t => t.EstadoUserStoryId).HasColumnName("EstadoUserStoryId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
        }
    }
}