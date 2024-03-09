using REST.Models.Entities;
using REST.Repositories.Interfaces;

namespace REST.Repositories.Implementations.Memory;

public class NoteRepository : MemoryRepository<long, Note>, INoteRepository<long>
{
    private long _globalId;
    
    public override Note? Add(Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        if (Entities.TryAdd(id, entity))
            return entity;

        return null;
    }

    public override Note? Update(long id, Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        Note? note = Entities.FirstOrDefault(pair => pair.Key == id).Value;
        if (note is not null)
        {
            note = entity;
            note.Id = id;
            
            return note;
        }

        return null;
    }
}