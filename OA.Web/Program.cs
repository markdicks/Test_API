using Microsoft.EntityFrameworkCore;
using OA.Repo;
using OA.Service;

var builder = WebApplication.CreateBuilder(args);
var connectionLocal = builder.Configuration.GetConnectionString("ConStrLocal");

/// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionLocal ?? throw new InvalidOperationException("Connection string invalid")));
/// Repository (based on DbContext) - Scoped
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
/// Services - Scoped (same as repository)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();


var app = builder.Build();

/// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    /// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
