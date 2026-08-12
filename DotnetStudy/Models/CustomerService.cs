namespace DotnetStudy.Models;

public class CustomerService : ICustomerService
{
    private readonly IReadOnlyList<Customer> _customers = CreateTestCustomers(100);

    public async Task<PagedResult<Customer>> GetCustomersAsync(
        string? keyword = null,
        DateTime? from = null,
        DateTime? to = null,
        int? skip = null,
        int? take = null,
        CancellationToken? cancellationToken = null
    )
    {
        var query = _customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var value = keyword.Trim();

            query = query.Where(x =>
                x.Name.Contains(
                    value,
                    StringComparison.OrdinalIgnoreCase) ||
                x.Email.Contains(
                    value,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (from.HasValue)
        {
            var value = from.Value.Date;

            query = query.Where(x =>
                x.CreatedAt >= value);
        }

        if (to.HasValue)
        {
            var toExclusive = to.Value.Date.AddDays(1);

            query = query.Where(x =>
                x.CreatedAt < toExclusive);
        }

        var totalCount = query.Count();

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        var items = query.ToList();

        // 検証用
        int delay = 2000;
        if (cancellationToken.HasValue)
        {
            await Task.Delay(delay, cancellationToken.Value);
        }
        else
        {
            await Task.Delay(delay);
        }

        return new(items, totalCount);
    }

    // 検証用データ作成
    private static IReadOnlyList<Customer> CreateTestCustomers(
        int count,
        int? seed = null)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        var random = seed is null
            ? Random.Shared
            : new Random(seed.Value);

        string[] lastNames =
        [
            "山田", "佐藤", "鈴木", "高橋", "田中",
        "伊藤", "渡辺", "中村", "小林", "加藤",
        "吉田", "山本", "松本", "井上", "木村"
        ];

        string[] firstNames =
        [
            "〇郎", "〇子", "一〇", "美〇", "〇太",
        "さく〇", "翔〇", "〇菜", "大〇", "〇衣",
        "直〇", "彩〇", "〇也", "〇衣", "〇也"
        ];

        string[] departments =
        [
            "営業部",
        "総務部",
        "開発部",
        "経理部",
        "人事部",
        "企画部"
        ];

        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2026, 8, 11);
        var dateRange = (endDate - startDate).Days;

        var customers = new List<Customer>(count);

        for (var i = 0; i < count; i++)
        {
            var id = 1001 + i;

            var lastName = lastNames[random.Next(lastNames.Length)];
            var firstName = firstNames[random.Next(firstNames.Length)];
            var name = $"{lastName} {firstName}";

            // メールアドレスは重複しないよう ID を含める
            var email = $"customer{id}@example.com";

            var department =
                departments[random.Next(departments.Length)];

            var date = startDate.AddDays(
                random.Next(dateRange + 1));

            customers.Add(
                new Customer(
                    id,
                    name,
                    email,
                    department,
                    date));
        }

        return customers;
    }
}
