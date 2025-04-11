using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on all interfaces - HTTP only on port 6969
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // HTTP endpoint on port 6969
    serverOptions.ListenAnyIP(6969);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure compression and caching only once
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
    options.MimeTypes = Microsoft.AspNetCore.ResponseCompression.ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { 
            "text/css", 
            "application/javascript", 
            "text/javascript", 
            "text/html", 
            "application/json", 
            "text/plain", 
            "image/svg+xml" 
        });
});

// Configure Brotli and Gzip compression levels
builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

// Add response caching (only once)
builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Поскольку мы используем только HTTP, отключаем HTTPS редирект и HSTS
    // app.UseHttpsRedirection();
    // app.UseHsts();
}
else
{
    // In development mode, show detailed error pages
    app.UseDeveloperExceptionPage();
}

// Включаем сжатие ответов для всех окружений
app.UseResponseCompression();

// Use middleware in the correct order
app.UseRouting();

// Включаем кэширование
app.UseResponseCaching();

// Улучшаем кэширование статических файлов
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Кэшируем статические файлы на 7 дней
        const int durationInSeconds = 60 * 60 * 24 * 7;
        ctx.Context.Response.Headers[HeaderNames.CacheControl] = 
            "public,max-age=" + durationInSeconds;
    }
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
