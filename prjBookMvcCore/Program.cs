using Microsoft.AspNetCore.Authentication.Cookies;
using prjBookMvcCore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BookShopContext>();

//=======AspNetCore.Authentication�Τ�n�J���Ҿާ@����ϥ�
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserInforService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    //���n�J�ɦ۰ʲ���즹���}
    option.LoginPath = new PathString("/Member/Login"); 
});
//=======AspNetCore.Authentication�Τ�n�J���Ҿާ@����ϥ�
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
//=======AspNetCore.Authentication�Τ�n�J���Ҿާ@����ϥ�
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
//=======AspNetCore.Authentication�Τ�n�J���Ҿާ@����ϥ�

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");

app.Run();
