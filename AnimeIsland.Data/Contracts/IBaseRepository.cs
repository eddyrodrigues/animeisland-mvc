using AnimeIsland.Data.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace AnimeIsland.Data.Contracts;

public interface IBaseRepository<T> where T : Entity
{
    public T GetById(Guid id);
    public Task<IEnumerable<T>> GetAllAsync();
    T Add(T entity);
    Task Remove(T entity);
    T Update(T entity);
}