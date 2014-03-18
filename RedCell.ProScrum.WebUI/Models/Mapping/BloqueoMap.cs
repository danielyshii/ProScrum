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
            this.Property(t => t.UserStoryId).HasColumnName("UserStoryId");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
            this.Property(t => t.InicioBloqueo).HasColumnName("InicioBloqueo");
            this.Property(t => t.FinBloqueo).HasColumnName("FinBloqueo");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");

            // Relationships
            this.HasRequired(t => t.TipoBloqueo)
                .WithMany(t => t.Bloqueos)
                .HasForeignKey(d => d.TipoBloqueoId);

            // Relationships
            this.HasRequired(t => t.UserStory)
                .WithMany(t => t.Bloqueos)
                .HasForeignKey(d => d.UserStoryId);

            // Relationships
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.Bloqueos)
                .HasForeignKey(d => d.UsuarioId);

        }
    }
}