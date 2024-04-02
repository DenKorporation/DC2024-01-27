using Microsoft.EntityFrameworkCore;
using REST.Publisher.Data;
using REST.Publisher.Models.Entities;
using REST.Publisher.Repositories.Interfaces;
using REST.Publisher.Utilities.Exceptions;
using UniqueConstraintException = EntityFramework.Exceptions.Common.UniqueConstraintException;

namespace REST.Publisher.Repositories.Implementations.EFCore;

public class TagRepository(AppDbContext dbContext) : ITagRepository<long>
{
    public async Task<Tag> AddAsync(Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            await dbContext.Tags.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        catch (UniqueConstraintException)
        {
            throw new Utilities.Exceptions.UniqueConstraintException($"Name {entity.Name} already exist", 40301);
        }

        return entity;
    }

    public async Task<bool> ExistAsync(long id)
    {
        return await dbContext.Tags.FirstOrDefaultAsync(tag => tag.Id == id) is not null;
    }

    public async Task<Tag> GetByIdAsync(long id)
    {
        Tag? result = await dbContext.Tags.FirstOrDefaultAsync(tag => tag.Id == id);
        if (result is null)
        {
            throw new ResourceNotFoundException(code: 40401);
        }

        return result;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await dbContext.Tags.ToListAsync();
    }

    public async Task<Tag> UpdateAsync(long id, Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        try
        {
            entity.Id = id;
            dbContext.Tags.Update(entity);
            if (await dbContext.SaveChangesAsync() == 0)
            {
                throw new ResourceNotFoundException(code: 40402);
            }
        }
        catch (UniqueConstraintException)
        {
            throw new Utilities.Exceptions.UniqueConstraintException($"Name {entity.Name} already exist", 40302);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ResourceNotFoundException(code: 40402);
        }

        return entity;
    }

    public async Task DeleteAsync(long id)
    {
        Tag? tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
        if (tag is null)
        {
            throw new ResourceNotFoundException(code: 40403);
        }

        try
        {
            dbContext.Tags.Remove(tag);
            if (await dbContext.SaveChangesAsync() == 0)
            {
                throw new ResourceNotFoundException(code: 40403);
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ResourceNotFoundException(code: 40403);
        }
    }
}