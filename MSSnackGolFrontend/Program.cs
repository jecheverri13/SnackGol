var builder = WebApplication.CreateBuilder(args);

// Servicios - EL ORDEN IMPORTA
builder.Services.AddRazorPages();  // ← Esto PRIMERO
var mvcBuilder = builder.Services.AddControllersWithViews();
// Enable runtime view recompilation only in Development
if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}
builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".SnackGol.Session";
    options.IdleTimeout = TimeSpan.FromDays(7);
    options.Cookie.HttpOnly = true;
});

// HttpClient para el API Backend
var apiBase = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5046";
builder.Services.AddHttpClient("Api", c =>
{
    c.BaseAddress = new Uri(apiBase);
    c.Timeout = TimeSpan.FromSeconds(8);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection(); // Solo forzamos HTTPS en ambientes no desarrollo
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Mapeo de endpoints - EL ORDEN IMPORTA
app.MapRazorPages();  // ← Esto DEBE ir ANTES de MapControllerRoute

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();