using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using PortalWeb.Services;
using PortalWeb.Intermediate;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Globalization;
namespace PortalWeb
{
    public sealed class Constraints
    {
        public int HttpTimeout { get; set; }
        public int CacheLifetime { get; set; }
        public int CookieMaxAge { get; set; }
        public string LoggingPath { get; set; }
        public string[] Queries { get; set; }
        public string[] Headers { get; set; }
        public string[] IPAddresses { get; set; }
        public bool ContainsQuery(string value) => Queries.Contains(value);
        public bool ContainsHeader(string value) => Headers.Contains(value);
        public bool ContainsIP(string value) => IPAddresses.Contains(value);
    }
    public enum RateLevel
    {
        Good,Normal,Bad
    }
    public enum MethodName
    {
        GET,POST,PUT,DELETE,PATCH
    }
    public enum ServiceType
    {
        [Display(Name ="Http Request")]
        Http
    }
    public static class ConfigurationExtensions
    {
        public static ConfigurationBuilder AddDatFile(this ConfigurationBuilder builder, string shortPath)
        {
            builder.Add(new BinaryConfigurationSource(shortPath));
            return builder;
        }
    }
    public sealed class Startup
    {
        public Startup()
        {
            Configuration = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("config.json").Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerWrapper, LoggerWrapper>();
            services.AddSingleton<IActionRecognizer, ActionRecognizer>();
            services.AddSingleton<IHttpInitiator, HttpInitiator>();
            services.AddSingleton<IHttpResponseFormatter, HttpResponseFormatter>();
            services.AddSingleton<IBasedOnMethodHttpInitiator, BasedOnMethodHttpInitiator>();
            services.AddSingleton<IFormFileHandler, FormFileHandler>();
            services.AddScoped<CommentManager>();
            services.AddScoped<IIdentityAuthenticator, IdentityAuthenticator>();
            services.AddSingleton<IAvatarMemento,AvatarMemento>();
            services.AddSingleton<IPasswordCacher,PasswordCacher>();
            services.AddMvc(ent =>
            {
                //ent.ModelBinderProviders.Insert(1, new KeyValueModelBinderProvider());
                ent.EnableEndpointRouting = true;
                ent.CacheProfiles.Add("viewCache", new CacheProfile() { Duration = 300, Location = ResponseCacheLocation.Client });
            }).AddViewLocalization().AddDataAnnotationsLocalization();
            services.AddHttpsRedirection(opt =>
            {
                opt.RedirectStatusCode = 308;
                opt.HttpsPort = 44338;
            });
            services.AddSession(opt =>
            {
                opt.Cookie.Name = ".AspNet.Session";
                opt.Cookie.HttpOnly = false;
                opt.Cookie.IsEssential = true;
                opt.IdleTimeout = TimeSpan.FromMinutes(10);
            });
            services.AddDistributedMemoryCache();
            services.AddHsts(opt =>
            {
                opt.Preload = true;
                opt.MaxAge = TimeSpan.FromHours(360);
                opt.IncludeSubDomains = true;
                opt.ExcludedHosts.Add("www.nuget.org");
            });
            services.AddResponseCompression(opt =>
            {
                opt.EnableForHttps = true;
                opt.Providers.Add<BrotliCompressionProvider>();
                opt.Providers.Add<GzipCompressionProvider>();
                opt.Providers.Add<DeflateCompressionProvider>();
            });
            services.AddDbContext<Repository>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("default")).LogTo((val) =>
                {
                    using (Stream basing = new FileStream("C:\\0\\PortalWeb\\Logs\\eflog.txt", FileMode.Append, FileAccess.Write, FileShare.None))
                    {
                        using (StreamWriter writer = new StreamWriter(basing))
                        {
                            writer.NewLine = "<!---->" + Environment.NewLine;
                            writer.WriteLine(val);
                        }
                    }
                }, LogLevel.Information);
            });
            services.Configure<Constraints>(Configuration);
            services.Configure<MvcViewOptions>(opt => opt.ViewEngines.Add(new HtmlViewEngine()));
            services.AddSingleton<System.Net.WebClient>();
            services.AddLocalization(ent => ent.ResourcesPath = "Resources");
            services.AddIdentity<CustomUser, CustomRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredLength = 5;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<Repository>();
            services.Configure<BrotliCompressionProviderOptions>(ent => ent.Level = CompressionLevel.Optimal);
            services.AddCors(opt => opt.AddPolicy("secure", (builder) => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:80/").WithExposedHeaders("Server-Additional")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,Microsoft.Extensions.Options.IOptions<Constraints> config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US"),
                SupportedCultures = new CultureInfo[]
            {
                new CultureInfo("ru-RU"),
                new CultureInfo("en-US")
            },
                SupportedUICultures = new CultureInfo[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ru-RU")
                }
            });
            var options = new FileServerOptions() { RequestPath = new PathString("/files"), FileProvider = env.ContentRootFileProvider, EnableDirectoryBrowsing = true, EnableDefaultFiles = false };
            options.StaticFileOptions.OnPrepareResponse = (ctx) => ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=2000");
            app.UseFileServer(options);
            app.UseMiddleware<ConfigurationInitMiddleware>(config);
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute("default", "First", "{controller}/{action}/{id?}", new {controller="Home",action="Index" }, new { });
            });
        }
    }
    public readonly struct ConfigurationInitMiddleware
    {
        private readonly RequestDelegate _del;
        public ConfigurationInitMiddleware(RequestDelegate del,Microsoft.Extensions.Options.IOptions<Constraints> configWrap)
        {
            if(configWrap is null || configWrap.Value is null)
            {
                throw new ArgumentNullException();
            }
            _del = del;
            if(Program._singleTone is null)
            {
                Program._singleTone = configWrap.Value;
            }
        }
        public async Task Invoke(HttpContext ctx)
        {
            await _del(ctx);
        }
    }
}
