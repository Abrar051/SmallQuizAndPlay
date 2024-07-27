using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Data.QuizMasterLiveQuizLog;
using QuizMaster.Models.BkashPaymentGateWay;
using QuizMaster.Models.WapPortal;
using QuizMaster.Models;
using System.Configuration;
using System.Net;

namespace QuizMaster.Extension
{
    public static class ServiceExtensions
    {
        public static void CorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        public static void ForwardedHeadersOptionsConfig(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });
        }

        public static void FireBaseConfig(this IServiceCollection services, IWebHostEnvironment _env)
        {
            ///// Firebase push Notification
            var googleCredential = _env.ContentRootPath;
            //googleCredential = Path.Combine(googleCredential, "push-notification-test-b16ee-firebase-adminsdk-5m73c-0f9ab95f68.json");
            googleCredential = Path.Combine(googleCredential, "push-notification-test-77c9d-0538217abf76.json");
            var credential = GoogleCredential.FromFile(googleCredential);
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential,
                ServiceAccountId = "firebase-adminsdk-unks6@push-notification-test-77c9d.iam.gserviceaccount.com",
                //ServiceAccountId = "my-client-id@my-project-id.iam.gserviceaccount.com",
            });
            ///// Firebase push Notification End
        }

        public static void ConfigDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BasketContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddDbContext<BkashPaymentGateWayContext>(options => options.UseSqlServer(configuration.GetConnectionString("BkashPaymentGateWay")))
                .AddDbContext<QuizMasterLiveQuizLogContext>(options => options.UseSqlServer(configuration.GetConnectionString("QuizMasterLiveQuizLogDB")))
                .AddDbContext<WapPortalContext>(options => options.UseSqlServer(configuration.GetConnectionString("WapPortal")));
        }

    }
}
