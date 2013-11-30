using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class IntegranteProyectoMap : EntityTypeConfiguration<IntegranteProyecto>
    {
        public IntegranteProyectoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProyectoId, t.UsuarioId });

            // Properties
            this.Property(t => t.ProyectoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UsuarioId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("IntegranteProyecto");
            this.Property(t => t.ProyectoId).HasColumnName("ProyectoId");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.Eliminado).HasColumnName("Eliminado");

            // Relationships
            this.HasRequired(t => t.Proyecto)
                .WithMany(t => t.IntegranteProyectoes)
                .HasForeignKey(d => d.ProyectoId);
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.IntegranteProyectoes)
                .HasForeignKey(d => d.UsuarioId);

        }
    }
}
