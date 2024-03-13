using System.Diagnostics.CodeAnalysis;
using REST.Repositories.Implementations.Memory;

namespace REST.Tests.Repositories.Implementations.Memory.Utiles;

public class MemoryRepositoryImplementation : MemoryRepository<long, string>
{
    private long _globalId;

    public override string Add(string entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;

        Entities.Add(id, entity);
        return entity;
    }

    [ExcludeFromCodeCoverage]
    public override string Update(long id, string entity)
    {
        throw new NotImplementedException();
    }
}