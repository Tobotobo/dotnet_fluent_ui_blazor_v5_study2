namespace DotnetStudy.Models;

public interface ICustomerService
{
    // TODO: 検索条件を引数で列挙するのではなく、リクエストクラスやコンディションクラスにまとめる
    Task<PagedResult<Customer>> GetCustomersAsync(
        string? keyword = null,
        DateTime? from = null,
        DateTime? to = null,
        int? skip = null,
        int? take = null,
        CancellationToken? cancellationToken = null
    );
}
