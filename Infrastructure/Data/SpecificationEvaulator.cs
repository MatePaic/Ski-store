using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class SpecificationEvaulator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
        {
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria); // x => x.Brand == brand
            }

            if (specification.OrderBy != null) 
            { 
                query = query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsDistinct)
            {
                query = query.Distinct();
            }

            return query;
        }

        public static IQueryable<TResult> GetQuery<TSpec, TResult>(
            IQueryable<T> query, ISpecification<T, TResult> specification)
        {
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria); // x => x.Brand == brand
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            var selectedQuery = query as IQueryable<TResult>;

            if (specification.Select != null)
            {
                selectedQuery = query.Select(specification.Select);
            }

            if (specification.IsDistinct)
            {
                selectedQuery = selectedQuery?.Distinct();
            }

            return selectedQuery ?? query.Cast<TResult>();
        }
    }
}
