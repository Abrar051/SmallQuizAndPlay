using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using QuizMaster.Data.QuizMasterLiveQuizLog;
using QuizMaster.Models;
using QuizMaster.Models.BkashPaymentGateWay;
using QuizMaster.Models.WapPortal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuizMaster
{
    public class Startup
    {
        //private readonly IWebHostEnvironment _env;
        //public Startup(IConfiguration configuration, IWebHostEnvironment webHost)
        //{
        //    Configuration = configuration;
        //    _env = webHost;
        //}

        //public IConfiguration Configuration { get; }

        //// This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{

        //    services.AddControllersWithViews();

        //    //services.AddSession(so =>
        //    //{
        //    //    so.IdleTimeout = TimeSpan.FromSeconds(2);
        //    //});

        //    //services.AddSession();
        //    services.AddSession(options => {
        //        options.IdleTimeout = TimeSpan.FromHours(12);
        //        options.Cookie.Name = ".yourApp.Session"; // <--- Add line
        //        options.Cookie.IsEssential = true;
        //    });

        //    services.AddDbContext<BasketContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))
        //        .AddDbContext<BkashPaymentGateWayContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BkashPaymentGateWay")))
        //        .AddDbContext<QuizMasterLiveQuizLogContext>(options => options.UseSqlServer(Configuration.GetConnectionString("QuizMasterLiveQuizLogDB")))
        //        .AddDbContext<WapPortalContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WapPortal")));


            

        //    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        //    ///// Firebase push Notification

        //    var googleCredential = _env.ContentRootPath;
        //    //googleCredential = Path.Combine(googleCredential, "push-notification-test-b16ee-firebase-adminsdk-5m73c-0f9ab95f68.json");
        //    googleCredential = Path.Combine(googleCredential, "push-notification-test-77c9d-0538217abf76.json");
        //    var credential = GoogleCredential.FromFile(googleCredential);
        //    FirebaseApp.Create(new AppOptions()
        //    {
        //        Credential = credential,
        //        ServiceAccountId = "firebase-adminsdk-unks6@push-notification-test-77c9d.iam.gserviceaccount.com",
        //        //ServiceAccountId = "my-client-id@my-project-id.iam.gserviceaccount.com",
        //    });

        //    ///// Firebase push Notification End

        //    services.AddDistributedMemoryCache();
            


        //    services.Configure<ForwardedHeadersOptions>(options =>
        //    {
        //        options.ForwardedHeaders =
        //            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        //    });

        //    services.Configure<ForwardedHeadersOptions>(options =>
        //    {
        //        options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
        //    });

        //    services.AddCors(option =>
        //    {
        //        option.AddDefaultPolicy(builder =>
        //        {
        //            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        //        });
        //    });
        //}

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    app.UseSession();
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        //app.UseExceptionHandler("/Home/Error");
        //        app.UseExceptionHandler("/Error");
        //        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //        app.UseHsts();
        //    }

        //    //app.UseDeveloperExceptionPage();
        //    //app.UseDatabaseErrorPage();


        //    app.UseHttpsRedirection();

        //    //app.UseStaticFiles();

        //    app.UseStaticFiles(new StaticFileOptions
        //    {
        //        OnPrepareResponse = ctx =>
        //        {
        //            string path = ctx.File.PhysicalPath;

        //            if (path.EndsWith(".jpeg") || path.EndsWith(".gif") || path.EndsWith(".jpg") || path.EndsWith(".png") || path.EndsWith(".svg"))
        //            {
        //                TimeSpan maxAge = new TimeSpan(7, 0, 0, 0);
        //                ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=" + maxAge.TotalSeconds.ToString("0"));
        //            }

        //        }
        //    });


        //    app.UseRouting();
        //    app.UseCors();
        //    app.UseAuthorization();
            
        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllerRoute(
        //            name: "default",
        //            //pattern: "{controller=Landingpage}/{action=DownTimePage}/{id?}");
        //            pattern: "{controller=LandingPage}/{action=Index}/{id?}");
        //    });
        //}
    }
}
