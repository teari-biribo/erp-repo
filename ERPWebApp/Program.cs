using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adding db context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"))
    .LogTo(Console.WriteLine, LogLevel.Information) // Log all Information-level events to the console
    .EnableSensitiveDataLogging(); // Include sensitive data (like key values) in logs
});

// Register repositories and services
builder.Services.AddScoped<IOrgUnitRepository, OrgUnitRepository>();
builder.Services.AddScoped<IOrgUnitService, OrgUnitService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
// builder.Services.AddScoped(typeof(ICRUDRepository<>), typeof(CRUDRepository<>));
// builder.Services.AddScoped<IOrgUnitService, OrgUnitService>();
// builder.Services.AddScoped<IRoleService, RoleService>();

// Automapper config
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
