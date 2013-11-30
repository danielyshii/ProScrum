using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class ContactoMap : EntityTypeConfiguration<Contacto>
    {
        public ContactoMap()
        {
            // Primary Key
            this.HasKey(t => t.ContactoId);

            // Properties
            this.Property(t => t.Nombres)
                .IsRequired()
                .HasMaxLength(70);

            this.Property(t => t.Apellidos)
                .IsRequired()
                .HasMaxLength(70);

            this.Property(t => t.CorreoElectronico)
                .HasMaxLength(30);

            this.Property(t => t.NumeroTelefonico)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Contacto");
            this.Property(t => t.ContactoId).HasColumnName("ContactoId");
            this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
            this.Property(t => t.Nombres).HasColumnName("Nombres");
            this.Property(t => t.Apellidos).HasColumnName("Apellidos");
            this.Property(t => t.CorreoElectronico).HasColumnName("CorreoElectronico");
            this.Property(t => t.NumeroTelefonico).HasColumnName("NumeroTelefonico");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");

            // Relationships
            this.HasRequired(t => t.Empresa)
                .WithMany(t => t.Contactoes)
                .HasForeignKey(d => d.EmpresaId);

        }
    }
}
