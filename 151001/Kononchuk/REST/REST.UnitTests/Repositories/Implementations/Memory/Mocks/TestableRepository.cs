using System.Diagnostics.CodeAnalysis;
using REST.Repositories.Implementations.Memory;

namespace REST.UnitTests.Repositories.Implementations.Memory.Mocks;

public class TestableRepository : MemoryRepository<long, string>
{
    private long _globalId;

    public override Task<string> AddAsync(string entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;

        Entities.Add(id, entity);
        return Task.FromResult(entity);
    }

    [ExcludeFromCodeCoverage]
    public override Task<string> UpdateAsync(long id, string entity)
    {
        throw new NotImplementedException();
    }
}