using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TubeMonitor.Api.Models;

namespace TubeMonitor.Api.Data.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable("Videos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.YoutubeVideoId).IsRequired().HasMaxLength(11);
        builder.Property(v => v.Titulo).IsRequired().HasMaxLength(200);
        builder.Property(v => v.Visualizacoes).IsRequired();
        builder.Property(v => v.Curtidas).IsRequired();
        builder.Property(v => v.Comentarios).IsRequired();
        builder.Property(v => v.PublicadoEm).IsRequired();
        builder.Property(v => v.CadastradoEm).IsRequired();

        builder.HasIndex(v => v.YoutubeVideoId).IsUnique();

        var dataSeed = new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new Video { Id = 1, CanalId = 1, YoutubeVideoId = "cF4sPq9LmZa", Titulo = "C# em 15 minutos: do zero à primeira API", Visualizacoes = 48_300, Curtidas = 3_900, Comentarios = 412, PublicadoEm = new DateTime(2026, 8, 20, 18, 0, 0, DateTimeKind.Utc), CadastradoEm = dataSeed },
            new Video { Id = 2, CanalId = 1, YoutubeVideoId = "Ht7kWq2NbXe", Titulo = "Entity Framework Core explicado com exemplos", Visualizacoes = 21_750, Curtidas = 1_820, Comentarios = 156, PublicadoEm = new DateTime(2026, 8, 28, 18, 0, 0, DateTimeKind.Utc), CadastradoEm = dataSeed },
            new Video { Id = 3, CanalId = 2, YoutubeVideoId = "Rt9yUi3OpLs", Titulo = "Lasanha de frigideira em 10 minutos", Visualizacoes = 312_000, Curtidas = 18_400, Comentarios = 1_230, PublicadoEm = new DateTime(2026, 8, 15, 12, 0, 0, DateTimeKind.Utc), CadastradoEm = dataSeed },
            new Video { Id = 4, CanalId = 2, YoutubeVideoId = "Qw1eRt5YuIo", Titulo = "3 cafés da manhã rápidos e baratos", Visualizacoes = 97_500, Curtidas = 6_100, Comentarios = 380, PublicadoEm = new DateTime(2026, 8, 30, 12, 0, 0, DateTimeKind.Utc), CadastradoEm = dataSeed },
            new Video { Id = 5, CanalId = 3, YoutubeVideoId = "Zx8cVb4NmAs", Titulo = "Treino de 20 minutos sem equipamento", Visualizacoes = 154_200, Curtidas = 9_870, Comentarios = 640, PublicadoEm = new DateTime(2026, 8, 25, 7, 0, 0, DateTimeKind.Utc), CadastradoEm = dataSeed });
    }
}
