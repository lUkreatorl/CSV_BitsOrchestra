using CSV_Backend.Database;
using Microsoft.EntityFrameworkCore;

namespace CSV_Backend;

public static class StartUp
{
    public static WebApplication ConfigureAndBuild(WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.AddControllers();
        appBuilder.Services.AddEndpointsApiExplorer();
        appBuilder.Services.AddSwaggerGen();

        appBuilder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        string conString = appBuilder.Configuration.GetConnectionString("DefaultConnection") +
                           "TrustServerCertificate=true";
        appBuilder.Services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(conString));

        return appBuilder.Build();
    }

    public static void MiddlewareSettings(ref WebApplication application)
    {
        
        if (application.Environment.IsDevelopment())
        {
            application.UseSwagger();
            application.UseSwaggerUI();
        }

        application.UseStaticFiles();
        application.UseRouting();
        application.UseCors("AllowAll");
        application.MapControllers();
    }
}