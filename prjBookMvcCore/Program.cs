using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using prjBookMvcCore.ViewModel;
using System.Diagnostics.Eventing.Reader;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

//=======db連線設定
builder.Services.AddDbContext<BookShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookShopConnection"));
});

//=======AspNetCore.Authentication用戶登入驗證操作機制使用
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserInforService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    //未登入時自動移轉到此網址
    option.LoginPath = new PathString("/Member/Login"); 
});
//=======AspNetCore.Authentication用戶登入驗證操作機制使用
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//=======AspNetCore.Authentication用戶登入驗證操作機制使用
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
//=======AspNetCore.Authentication用戶登入驗證操作機制使用

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");

app.Run();
