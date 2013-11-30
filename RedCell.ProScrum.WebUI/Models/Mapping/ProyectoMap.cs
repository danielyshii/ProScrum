using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class ProyectoMap : EntityTypeConfiguration<Proyecto>
    {
        public ProyectoMap()
        {
            // Primary Key
            this.HasKey(t => t.ProyectoId);

            // Properties
            this.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Mnemonico)
                .IsRequired()
                .HasMaxLength(6);

            // Table & Column Mappings
            this.ToTable("Proyecto");
            this.Property(t => t.ProyectoId).HasColumnName("ProyectoId");
            this.Property(t => t.JefeProyectoId).HasColumnName("JefeProyectoId");
            this.Property(t => t.ContactoId).HasColumnName("ContactoId");
            this.Property(t => t.Nombre).HasColumnName("Nombre");
            this.Property(t => t.Mnemonico).HasColumnName("Mnemonico");
            this.Property(t => t.InicioEstimado).HasColumnName("InicioEstimado");
            this.Property(t => t.FinEstimado).HasColumnName("FinEstimado");
            this.Property(t => t.HorasEstimadas).HasColumnName("HorasEstimadas");
            this.Property(t => t.EstadoId).HasColumnName("EstadoId");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");

            // Relationships
            this.HasRequired(t => t.Contacto)
                .WithMany(t => t.Proyectoes)
                .HasForeignKey(d => d.ContactoId);
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.Proyectoes)
                .HasForeignKey(d => d.JefeProyectoId);

        }
    }
}
