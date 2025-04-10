var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on all interfaces - HTTP only on port 6969
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // HTTP endpoint on port 6969
    serverOptions.ListenAnyIP(6969);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Comment out HSTS since we're not using HTTPS
    // app.UseHsts();
}

// Comment out HTTPS redirection since we're not using HTTPS
// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
