using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OldWays.Areas.Identity.Data;
using OldWays.Data;
using OldWays.Models;
using OldWays.Services;


var builder = WebApplication.CreateBuilder(args);

// SQL Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//SQL identity extension
builder.Services.AddDefaultIdentity<ApplicationUser>
    (
    options => { 
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredUniqueChars = 0;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Weather service
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddTransient<WeatherService>();


// Email sender service
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(
    builder.Configuration.GetSection("SendGrid"));

//weather settings service
builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherSettings"));


//Blob Storage
if (builder.Environment.IsDevelopment())
{
    // Local Azurite
    builder.Services.AddSingleton(_ =>
        new BlobServiceClient("UseDevelopmentStorage=true"));
}
else
{
    // Real Azure Blob Storage
    builder.Services.AddSingleton(x =>
    {
        var config = x.GetRequiredService<IConfiguration>();
        var storage = config.GetConnectionString("StorageConnection");

        return new BlobServiceClient(
            new Uri(storage),
            new DefaultAzureCredential()
        );
    });
}

builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // default HSTS value is 30 days. 
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// Seed roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeeder.SeedRoles(roleManager);
}
app.Run();
