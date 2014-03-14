using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class UserStoryMap : EntityTypeConfiguration<UserStory>
    {
        public UserStoryMap()
        {
            // Primary Key
            this.HasKey(t => t.UserStoryId);

            // Properties
            this.Property(t => t.Codigo)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("UserStory");
            this.Property(t => t.UserStoryId).HasColumnName("UserStoryId");
            this.Property(t => t.ProyectoId).HasColumnName("ProyectoId");
            this.Property(t => t.SprintId).HasColumnName("SprintId");
            this.Property(t => t.ResponsableId).HasColumnName("ResponsableId");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
            this.Property(t => t.HorasEstimadas).HasColumnName("HorasEstimadas");
            this.Property(t => t.Prioridad).HasColumnName("Prioridad");
            this.Property(t => t.Color).HasColumnName("Color");
            this.Property(t => t.EstadoId).HasColumnName("EstadoId");
            this.Property(t => t.BloqueoId).HasColumnName("BloqueoId");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");

            // Relationships
            this.HasRequired(t => t.Proyecto)
                .WithMany(t => t.UserStories)
                .HasForeignKey(d => d.ProyectoId);
            this.HasOptional(t => t.Sprint)
                .WithMany(t => t.UserStories)
                .HasForeignKey(d => d.SprintId);
            this.HasOptional(t => t.Usuario)
                .WithMany(t => t.UserStories)
                .HasForeignKey(d => d.ResponsableId);

        }
    }
}
