using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Game : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? ShortDescription { get; set; } = null;
    public string? FullDescription { get; set; } = null;
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public ICollection<Interest> Interests { get; set; } = null!;
    public ICollection<Post> Posts { get; set; } = null!;
    public ICollection<GameAesthetic> GameAesthetics { get; set; } = null!;
    public ICollection<Aesthetic> Aesthetics { get; set; } = null!;
    public ICollection<GameGenre> GameGenres { get; set; } = null!;
    public ICollection<Genre> Genres { get; set; } = null!;
    public ICollection<GameMode> GameModes { get; set; } = null!;
    public ICollection<Mode> Modes { get; set; } = null!;
    public ICollection<GamePerspective> GamePerspectives { get; set; } = null!;
    public ICollection<Perspective> Perspectives { get; set; } = null!;
    public ICollection<GamePlatform> GamePlatforms { get; set; } = null!;
    public ICollection<Platform> Platforms { get; set; } = null!;
    public ICollection<GameTheme> GameThemes { get; set; } = null!;
    public ICollection<Theme> Themes { get; set; } = null!;
}