using SV21T1020052.Shop.Appcodes;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Thêm cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(op =>
    {
        op.LoginPath = "/Account/Login";
        op.AccessDeniedPath = "/Account/Denied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// Thêm middleware Session
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

ApplicationContext.Configure
    (
        context: app.Services.GetRequiredService<IHttpContextAccessor>(),
        enviroment: app.Services.GetRequiredService<IWebHostEnvironment>()
    );

string connectionString = builder.Configuration.GetConnectionString("LiteCommerceDB");
SV21T1020052.BusinessLayers.Configuration.Initialize(connectionString);

app.Run();