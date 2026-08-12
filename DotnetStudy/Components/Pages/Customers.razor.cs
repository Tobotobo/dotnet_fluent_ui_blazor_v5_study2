using DotnetStudy.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace DotnetStudy.Components.Pages;

public partial class Customers(ICustomerService CustomerService) : ComponentBase
{
    private FluentDataGrid<Customer> _grid = default!;

    private readonly PaginationState _pagination = new()
    {
        ItemsPerPage = 10
    };

    // 入力中の検索条件
    private string? _criteriaKeyword;
    private DateTime? _criteriaFrom;
    private DateTime? _criteriaTo;

    // 実際に検索に使用する条件
    private string? _keyword;
    private DateTime? _from;
    private DateTime? _to;

    private async ValueTask<GridItemsProviderResult<Customer>> LoadCustomersAsync(
        GridItemsProviderRequest<Customer> request)
    {
        // 初回ロード時と同じ「自動 Loading 状態」に戻す
        _grid.SetLoadingState(null);

        var skip = request.StartIndex;
        var take = request.Count ?? _pagination.ItemsPerPage;

        var result = await CustomerService.GetCustomersAsync(
            _keyword, _from, _to,
            skip,
            take,
            request.CancellationToken);

        return GridItemsProviderResult.From(
            items: result.Items.ToList(),
            totalItemCount: result.TotalCount
        );
    }

    private async Task SearchAsync()
    {
        _keyword = _criteriaKeyword;
        _from = _criteriaFrom;
        _to = _criteriaTo;

        // 検索条件変更時は1ページ目へ戻す
        await _pagination.SetCurrentPageIndexAsync(0);
    }

    private async Task ClearAsync()
    {
        _criteriaKeyword = null;
        _criteriaFrom = null;
        _criteriaTo = null;

        _keyword = null;
        _from = null;
        _to = null;

        await _pagination.SetCurrentPageIndexAsync(0);
    }
}
