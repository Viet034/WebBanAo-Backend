namespace WebBanAoo.Ultility.Pagination;

public static class QueryableExtensions
{
    public static PageResult<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var totalCount = query.Count();
        var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PageResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
