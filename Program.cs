using HealthManagementSystem.Components;
using HealthManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//register db context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowAll", options =>
    {
        options.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

//inject HTTP client
builder.Services.AddHttpClient();

//add the controllers
builder.Services.AddControllers();

//enforce all API endpoints are in lowercase
builder.Services.AddRouting(options => options.LowercaseUrls = true);

//services for swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add scope
builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri("http://localhost:5293")});

var app = builder.Build();

//enable swagger in development

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

//use CORS
app.UseCors("allowAll");

// app.MapStaticAssets();
app.UseStaticFiles();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//map controllers
app.MapControllers();

app.Run();
