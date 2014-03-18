using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RedCell.ProScrum.WebUI.Models.Mapping
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            // Primary Key
            this.HasKey(t => t.UsuarioId);

            // Properties
            this.Property(t => t.Codigo)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Nombres)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Apellidos)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CorreoElectronico)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.UriAvatar)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Usuario");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
            this.Property(t => t.Nombres).HasColumnName("Nombres");
            this.Property(t => t.Apellidos).HasColumnName("Apellidos");
            this.Property(t => t.CorreoElectronico).HasColumnName("CorreoElectronico");
            this.Property(t => t.UriAvatar).HasColumnName("UriAvatar");
            this.Property(t => t.EsEliminado).HasColumnName("EsEliminado");
        }
    }
}
