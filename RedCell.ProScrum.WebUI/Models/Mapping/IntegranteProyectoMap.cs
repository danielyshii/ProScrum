using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class IntegranteProyectoMap : EntityTypeConfiguration<IntegranteProyecto>
    {
        public IntegranteProyectoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProyectoId, t.IntegranteId });

            // Properties
            this.Property(t => t.ProyectoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IntegranteId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("IntegranteProyecto");
            this.Property(t => t.ProyectoId).HasColumnName("ProyectoId");
            this.Property(t => t.IntegranteId).HasColumnName("IntegranteId");
            this.Property(t => t.EsEncargado).HasColumnName("EsEncargado");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");

            // Relationships
            this.HasRequired(t => t.Proyecto)
                .WithMany(t => t.IntegranteProyectoes)
                .HasForeignKey(d => d.ProyectoId);
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.IntegranteProyectoes)
                .HasForeignKey(d => d.IntegranteId);

        }
    }
}
