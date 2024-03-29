using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using REST.Data;
using REST.Models.Entities;
using REST.Repositories.Interfaces;
using REST.Utilities.Exceptions;
using UniqueConstraintException = EntityFramework.Exceptions.Common.UniqueConstraintException;

namespace REST.Repositories.Implementations.EFCore;

public class IssueRepository(AppDbContext dbContext) : IIssueRepository<long>
{
    /// <inheritdoc/>
    /// <exception cref="Utilities.Exceptions.AssociationException">Occurs if <see cref="Editor"/> with given id not exist</exception>
    public async Task<Issue> AddAsync(Issue entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            await dbContext.Issues.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        catch (UniqueConstraintException)
        {
            throw new Utilities.Exceptions.UniqueConstraintException($"Title {entity.Title} already exist", 40301);
        }
        catch (ReferenceConstraintException)
        {
            throw new AssociationException($"Editor with Id = {entity.EditorId} not exist", 40311);
        }

        return entity;
    }

    public async Task<bool> ExistAsync(long id)
    {
        return await dbContext.Issues.FirstOrDefaultAsync(issue => issue.Id == id) is not null;
    }

    public async Task<Issue> GetByIdAsync(long id)
    {
        Issue? result = await dbContext.Issues.FirstOrDefaultAsync(issue => issue.Id == id);
        if (result is null)
        {
            throw new ResourceNotFoundException(code: 40401);
        }

        return result;
    }

    public async Task<IEnumerable<Issue>> GetAllAsync()
    {
        return await dbContext.Issues.ToListAsync();
    }

    /// <inheritdoc/>
    /// <exception cref="Utilities.Exceptions.AssociationException">Occurs if <see cref="Editor"/> with given id not exist</exception>
    public async Task<Issue> UpdateAsync(long id, Issue entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            dbContext.Issues.Update(entity);
            if (await dbContext.SaveChangesAsync() == 0)
            {
                throw new ResourceNotFoundException(code: 40402);
            }
        }
        catch (UniqueConstraintException)
        {
            throw new Utilities.Exceptions.UniqueConstraintException($"Title {entity.Title} already exist", 40302);
        }
        catch (ReferenceConstraintException)
        {
            throw new AssociationException($"Editor with Id = {entity.EditorId} not exist", 40312);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ResourceNotFoundException(code: 40402);
        }

        return entity;
    }

    public async Task DeleteAsync(long id)
    {
        Issue? issue = await dbContext.Issues.FirstOrDefaultAsync(i => i.Id == id);
        if (issue is null)
        {
            throw new ResourceNotFoundException(code: 40403);
        }

        try
        {
            dbContext.Issues.Remove(issue);
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