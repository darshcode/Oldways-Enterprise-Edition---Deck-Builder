using Azure.Storage.Blobs;
using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OldWays.Areas.Identity.Data;
using OldWays.Data;
using Microsoft.Extensions.Azure;
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
        options.SignIn.RequireConfirmedEmail = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredUniqueChars = 0;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();



//Blob Storage
builder.Services.AddAzureClients(clientBuilder =>
{
    var storage = builder.Configuration.GetConnectionString("StorageConnection");

    clientBuilder.AddBlobServiceClient(storage).WithCredential(new DefaultAzureCredential());           
});


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
