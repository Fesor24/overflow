using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuestionService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer("keycloak", "overflow", opts =>
    {
        opts.RequireHttpsMetadata = false;
        opts.Audience = "overflow";
    });
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<QuestionDbContext>("question-db");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.MapDefaultEndpoints();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var dbContext = services.GetRequiredService<QuestionDbContext>();

    await dbContext.Database.MigrateAsync();
}
catch(Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();

    logger.LogError("An error occurred while applying migrations", ex);
}

app.Run();
