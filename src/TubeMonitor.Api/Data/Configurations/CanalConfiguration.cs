using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TubeMonitor.Api.Models;

namespace TubeMonitor.Api.Data.Configurations;

public class CanalConfiguration : IEntityTypeConfiguration<Canal>
{
    public void Configure(EntityTypeBuilder<Canal> builder)
    {
        builder.ToTable("Canais");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Handle).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Nicho).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Inscritos).IsRequired();
        builder.Property(c => c.CadastradoEm).IsRequired();

        builder.HasIndex(c => c.Handle).IsUnique();

        builder.HasMany(c => c.Videos)
            .WithOne(v => v.Canal)
            .HasForeignKey(v => v.CanalId)
            .OnDelete(DeleteBehavior.Cascade);

        // Dados iniciais para facilitar os testes
        var dataSeed = new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new Canal { Id = 1, Nome = "Código Fácil", Handle = "@codigofacil", Nicho = "Tecnologia", Inscritos = 185_000, CadastradoEm = dataSeed },
            new Canal { Id = 2, Nome = "Cozinha em 10 Minutos", Handle = "@cozinha10min", Nicho = "Culinária", Inscritos = 920_000, CadastradoEm = dataSeed },
            new Canal { Id = 3, Nome = "Treino em Casa BR", Handle = "@treinoemcasabr", Nicho = "Fitness", Inscritos = 430_000, CadastradoEm = dataSeed });
    }
}
