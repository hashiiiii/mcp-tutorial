using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;

// stdio を利用する場合はこの CreateEmptyApplicationBuilder を利用する必要がある。
// Transport（メッセージ通信）に streamableHTTP を利用する場合は Host.CreateDefaultBuilder を使ってもいいらしい
// これを使うと settings は default の値で初期化されるみたい
// ref. https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostinghostbuilderextensions.configuredefaults?view=net-9.0-pp#microsoft-extensions-hosting-hostinghostbuilderextensions-configuredefaults(microsoft-extensions-hosting-ihostbuilder-system-string())
// settings の docs
// ref. https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostapplicationbuildersettings?view=net-9.0-pp&viewFallbackFrom=net-8.0
// ただ Transport（メッセージ通信）に stdio を利用する場合は Host.CreateDefaultBuilder を利用すると stdio にメッセージを書き込んでしまって JSON-RPC が破損してサーバーが壊れるっぽい。
var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly()
    .WithPromptsFromAssembly();

builder.Services.AddSingleton(_ =>
{
    var client = new HttpClient { BaseAddress = new Uri("https://api.weather.gov") };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("weather-tool", "1.0"));
    return client;
});

var app = builder.Build();

// run MCP server
await app.RunAsync();