using ApiCallLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {       
        config.AddJsonFile("config.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Bind config section to ApiSettings
        services.Configure<ApiSettings>(context.Configuration.GetSection("ApiSettings"));
        services.AddTransient<HttpClient>();
        // Register MyService
        services.AddTransient<UserService>();
    })
    .Build();


var serv = host.Services.GetRequiredService<UserService>();
var result = await serv.GetUserByIdAsync(1);

