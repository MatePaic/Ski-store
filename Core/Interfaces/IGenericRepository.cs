using Core.Entities;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAsync(ISpecification<T> specification);
        Task<IReadOnlyList<TResult>> GetAsync<TResult>(ISpecification<T, TResult> specification);
        Task<T?> GetByIdAsync(int id);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        bool Exists(int id);
        Task<bool> SaveChangesAsync();
    }
}
