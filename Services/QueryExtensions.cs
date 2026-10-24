using BettingSystem.Data.Models;
using BettingSystem.Domain;

namespace BettingSystem.Services;

public static class QueryExtensions
{
    public static IQueryable<T> Page<T>(this IQueryable<T> query, int page = QueryDefaults.FirstPage, int pageSize = QueryDefaults.DefaultPageSize)
        where T : class
    {
        if (page < QueryDefaults.FirstPage || pageSize < QueryDefaults.MinPageSize || pageSize > QueryDefaults.MaxPageSize || (long)(page - QueryDefaults.FirstPage) * pageSize > int.MaxValue)
            throw new BettingValidationException($"Page must be at least {QueryDefaults.FirstPage}; page size must be {QueryDefaults.MinPageSize}–{QueryDefaults.MaxPageSize}.");

        return query.Skip((page - QueryDefaults.FirstPage) * pageSize).Take(pageSize);
    }

    public static void Deconstruct<T>(this PagedResult<T> result, out List<T> data, out int totalCount)
        where T : class => (data, totalCount) = (result.Data, result.TotalCount);
}
