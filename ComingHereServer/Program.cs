using ComingHereServer.Data;
using ComingHereServer.Services;

var builder = WebApplication.CreateBuilder(args);

// ������� � ������������
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

// ������� � ��������
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "uploads")
    ),
    RequestPath = "/uploads"
});

app.MapControllers();

// ������ ����, ������ � ���������
using var scope = app.Services.CreateScope();
await DatabaseSeeder.SeedAsync(scope.ServiceProvider);

app.Run();