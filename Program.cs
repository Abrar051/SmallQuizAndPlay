using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuizMaster.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//namespace QuizMaster
//{
//    public class Program
//    {

//        #region Old Code Based on core 5
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();


//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    //webBuilder.UseStartup<Startup>();
//                    webBuilder.UseStartup<Startup>().UseUrls("https://0.0.0.0:5007");
//                    ////webBuilder.UseStartup<Startup>().UseUrls("https://0.0.0.0:5006"); //Test Port
//                    //webBuilder.UseStartup<Startup>().UseUrls("https://0.0.0.0:5018"); //Test Port
//                });

//        #endregion



//    }
//}

#region New Code Based on core 6

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.Name = ".yourApp.Session"; // <--- Add line
    options.Cookie.IsEssential = true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.ConfigDatabaseContext(builder.Configuration);
builder.Services.CorsConfiguration();
builder.Services.ForwardedHeadersOptionsConfig();
builder.Services.FireBaseConfig(builder.Environment);
//builder.Services.AddRazorPages();

var app = builder.Build();

app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
///
// Add CORS policy here
app.UseCors(options => options.WithOrigins("https://wap.shabox.mobi", "https://gamestar.shabox.mobi")
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowAnyOrigin());

app.UseCors(); // Apply CORS for posting data 
//app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        string path = ctx.File.PhysicalPath;

        if (path.EndsWith(".jpeg") || path.EndsWith(".gif") || path.EndsWith(".jpg") || path.EndsWith(".png") || path.EndsWith(".svg"))
        {
            TimeSpan maxAge = new TimeSpan(7, 0, 0, 0);
            ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=" + maxAge.TotalSeconds.ToString("0"));
        }

    }
});

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        //pattern: "{controller=Landingpage}/{action=DownTimePage}/{id?}");
        pattern: "{controller=KidsProfileSetup}/{action=kidsHome}/{id?}");
});

//app.Run("https://*:5007");
//app.Run("https://*:5018"); //Test Port
app.Run();
#endregion

