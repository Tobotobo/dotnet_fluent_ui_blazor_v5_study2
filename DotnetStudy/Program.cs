using DotnetStudy.Components;
using DotnetStudy.Models;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();
builder.Services.AddFluentUIComponents();



// AddTransient : サービスが要求（注入）されるたびに、毎回新しいインスタンスを生成 
// AddScoped    : 1つの HTTP リクエスト（スコープ）内で同一のインスタンスを共有
// AddSingleton : アプリケーション起動時に1度だけ生成され、アプリ終了まで同じインスタンスを全体で共有

builder.Services.AddSingleton<ICustomerService, CustomerService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>();

app.Run();
