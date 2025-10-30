var builder = WebApplication.CreateBuilder(args);

// Servicios - EL ORDEN IMPORTA
builder.Services.AddRazorPages();  // ← Esto PRIMERO
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Mapeo de endpoints - EL ORDEN IMPORTA
app.MapRazorPages();  // ← Esto DEBE ir ANTES de MapControllerRoute

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();