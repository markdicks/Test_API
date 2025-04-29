using Microsoft.EntityFrameworkCore;
using OA.Repo;
using OA.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var connectionLocal = builder.Configuration.GetConnectionString("ConStrLocal");
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

/// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionLocal ?? throw new InvalidOperationException("Connection string invalid")));
/// Repository (based on DbContext) - Scoped
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
/// Services - Scoped (same as repository)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

/// Extra services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "My Test API",
        Version = "v1",
        Description = "An example API for users and profiles",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Mark Dicks",
            Email = "markdicks03@gmail.com"
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7067", "https://localhost:7191") // Adjust origin
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});



var app = builder.Build();

/// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    /// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("CorsPolicy");
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();
