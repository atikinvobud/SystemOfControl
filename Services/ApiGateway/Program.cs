using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// Добавляем конфигурацию из appsettings.json и переменных окружения
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Получаем хосты и порты из переменных окружения
var usersHost = builder.Configuration["USERS_HOST"] ;
var usersPort = builder.Configuration["USERS_PORT"];
var ordersHost = builder.Configuration["ORDERS_HOST"];
var ordersPort = builder.Configuration["ORDERS_PORT"] ;

// Подставляем их в конфигурацию YARP
builder.Configuration["YARP:Clusters:userscluster:Destinations:users1:Address"] =
    $"http://{usersHost}:{usersPort}/";
builder.Configuration["YARP:Clusters:orderscluster:Destinations:order1:Address"] =
    $"http://{ordersHost}:{ordersPort}/";

// Добавляем YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("YARP"));

var app = builder.Build();

// Включаем проксирование
app.MapReverseProxy();

app.Run();
