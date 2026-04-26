using Ardalis.Specification;
using Ludo.Database.Repository.Entities;

namespace Ludo.Services.Specifications;

public sealed class AccountSpec : Specification<Account>
{
    public AccountSpec(Guid id) => Query.Where(e => e.Id == id);
    public AccountSpec(string email) => Query.Where(e => e.Email == email);
    public AccountSpec(string name) => Query.Where(e => e.Name == name);
}