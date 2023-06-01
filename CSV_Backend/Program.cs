using CSV_Backend;

var builder = WebApplication.CreateBuilder(args);

var app = StartUp.ConfigureAndBuild(builder);

StartUp.MiddlewareSettings(ref app);

app.Run();