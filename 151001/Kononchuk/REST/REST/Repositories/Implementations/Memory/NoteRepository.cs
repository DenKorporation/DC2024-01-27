using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;

namespace REST.Repositories.Implementations.Memory;

public class NoteRepository : MemoryRepository<long, Note>, INoteRepository<long>
{
    private long _globalId;
    
    public override Task<Note> AddAsync(Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        Entities.Add(id, entity);
        return Task.FromResult(entity);
    }

    public override async Task<Note> UpdateAsync(long id, Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        if (await ExistAsync(id))
        {
            entity.Id = id;
            Entities[id] = entity;

            return entity;
        }
        else
        {
            throw new ResourceNotFoundException(code: 40402);
        }
    }
}