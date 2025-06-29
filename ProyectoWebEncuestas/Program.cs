/*using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Services.AddRazorPages();
builder.Services.AddMvc();
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//	.AddCookie(options =>
//	{
//		options.LoginPath = "/iniciar-sesion";
//		options.LogoutPath = "/cerrar-sesion";
//		options.Cookie.Name = "ASGcookie";
//		options.ExpireTimeSpan = TimeSpan.FromDays(7);
//	});
//builder.Services.AddAuthorization();
//builder.Services.AddHttpClient();
app.MapGet("/", () => "Hello World!");

app.MapRazorPages();
app.UseStaticFiles();
app.UseDeveloperExceptionPage();
//app.UseAuthentication();
//app.UseAuthorization();
app.MapControllerRoute(
	name: "default",
	 pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapDefaultControllerRoute();
app.Run();
*/

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();