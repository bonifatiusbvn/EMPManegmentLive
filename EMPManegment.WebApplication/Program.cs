
using DinkToPdf;
using DinkToPdf.Contracts;
using EMPManagment.Web.Helper;
using EMPManegment.EntityModels.Common;
using EMPManegment.Web.Helper;
using EMPManegment.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddScoped<WebAPI, WebAPI>();
builder.Services.AddScoped<UserSession>();
builder.Services.AddScoped<EMPManegment.Web.Helper.Common>();
builder.Services.AddScoped<APIServices, APIServices>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.LoginPath = "/Authentication/Login";
            options.LogoutPath = "/Authentication/Login";
            options.Cookie.HttpOnly = true;
            //options.Cookie.Name = "localhost:7204";
            options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;
        });
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "UserName";
    options.Cookie.Expiration = TimeSpan.FromMinutes(1);
});
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(50);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;

});

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
UserSession.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();

