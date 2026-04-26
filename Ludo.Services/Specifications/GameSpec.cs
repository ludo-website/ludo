using Ardalis.Specification;
using Ludo.Database.Repository.Entities;

namespace Ludo.Services.Specifications;

public sealed class GameSpec : Specification<Game>
{
    public GameSpec(Guid id) => Query.Where(e => e.Id == id);

    public GameSpec(string title) => Query.Where(e => e.Title == title);
}