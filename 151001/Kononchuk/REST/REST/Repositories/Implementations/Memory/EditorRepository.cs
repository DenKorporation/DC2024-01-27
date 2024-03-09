using REST.Models.Entities;
using REST.Repositories.Interfaces;

namespace REST.Repositories.Implementations.Memory;

public class EditorRepository : MemoryRepository<long, Editor>, IEditorRepository<long>
{
    private long _globalId;
    
    public override Editor? Add(Editor entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        long id = ++_globalId;
        entity.Id = id;

        if (Entities.TryAdd(id, entity))
            return entity;

        return null;
    }

    public override Editor? Update(long id, Editor entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        Editor? editor = Entities.FirstOrDefault(pair => pair.Key == id).Value;
        if (editor is not null)
        {
            editor = entity;
            editor.Id = id;
            
            return editor;
        }

        return null;
    }
}