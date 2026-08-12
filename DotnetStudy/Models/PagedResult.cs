namespace DotnetStudy.Models;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount
);