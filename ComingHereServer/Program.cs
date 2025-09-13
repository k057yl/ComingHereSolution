using ComingHereServer.Data;
using ComingHereServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Сервисы и конфигурации
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddDataLayer();
builder.Services.AddIdentityWithRoles();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsPolicy();
builder.Services.AddSessionAndCaching();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseCors("_myAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Статика и загрузки
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "uploads")
    ),
    RequestPath = "/uploads"
});

app.MapControllers();

// Сидаем роли, админа и категории
using var scope = app.Services.CreateScope();
await DatabaseSeeder.SeedAsync(scope.ServiceProvider);

app.Run();