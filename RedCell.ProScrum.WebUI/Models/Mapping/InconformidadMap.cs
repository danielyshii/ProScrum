using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class InconformidadMap : EntityTypeConfiguration<Inconformidad>
    {
        public InconformidadMap()
        {
            // Primary Key
            this.HasKey(t => t.InconformidadId);

            // Table & Column Mappings
            this.ToTable("InconformidadId");
            this.Property(t => t.InconformidadId).HasColumnName("InconformidadId");
            this.Property(t => t.UserStoryId).HasColumnName("UserStoryId");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.Descripcion).HasColumnName("Descripcion");
            this.Property(t => t.FechaRegistro).HasColumnName("FechaRegistro");
            this.Property(t => t.UriAdjunto).HasColumnName("UriAdjunto");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");

            // Relationships
            this.HasRequired(t => t.UserStory)
                .WithMany(t => t.Inconformidades)
                .HasForeignKey(d => d.UserStoryId);
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.Inconformidades)
                .HasForeignKey(d => d.UsuarioId);

        }
    }
}