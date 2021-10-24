using WebApiAutores.DTOs.PAGINATION;

namespace WebApiAutores.Utilities
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable,PaginationDTO pagDTO)
        {
            return queryable.Skip((pagDTO.pagina - 1) * pagDTO.RecordsPorPagina)
                .Take((pagDTO.RecordsPorPagina));
        }
    }
}
