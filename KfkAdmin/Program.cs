using KfkAdmin.Components;
using KfkAdmin.Infrastructure.Database;
using KfkAdmin.Extensions.Startup;
using KfkAdmin.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddKafkaExtension();
builder.Services.AddDbContext<SqLiteContext>(options =>
    options.UseSqlite("Data Source=./Infrastructure/Database/app.db;Mode=ReadWrite;"));


SQLitePCL.Batteries.Init();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteContext>();
    await dbContext.Database.MigrateAsync();
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();