using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using SigmalHex.AOP.Interceptors;
using SigmalHex.Domain.FrameContext.ApplicationServices;
using SigmalHex.Domain.KBContext.ApplicationServices;
using SigmalHex.EntityFramework;
using SigmalHex.WebApi.WebDashbord;
using Swashbuckle.Swagger.Model;
using System;
using System.Globalization;
using System.IO;

namespace SigmalHex.WebApi
{
    public class Startup
    {
        private IServiceCollection _services;

        public static log4net.Repository.ILoggerRepository repository { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("Sigmal.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            repository = log4net.LogManager.CreateRepository("NETCoreRepository");
            log4net.Config.XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public IConfigurationRoot Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _services = services;

            services.AddDbContext<SigmalHexContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN")
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "zh-CN", uiCulture: "zh-CN");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;

                // You can change which providers are configured to determine the culture for requests, or even add a custom
                // provider with your own logic. The providers will be asked in order to provide a culture for each request,
                // and the first to provide a non-null result that is in the configured supported cultures list will be used.
                // By default, the following built-in providers are configured:
                // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
                // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
                // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
                options.RequestCultureProviders.Insert(0, new UrlRequestCultureProvider());
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();

            // Add framework services.
            services.AddMvc(/*opts => opts.Conventions.Insert(0, new ApiPrefixConvention())*/);

            //******************* swagger start ***********************
            services.AddSwaggerGen(c =>
            {
                c.SingleApiVersion(new Info
                {
                    Version = "v1",     // 这个属性必须要填，否则会引发一个异常
                    Title = "Feature List",
                    Description = "特征"
                });
            });

            services.ConfigureSwaggerGen(c =>
            {
                // 配置生成的 xml 注释文档路径
                c.IncludeXmlComments(GetXmlCommentsPath());
            });

            //******************* swagger end ***********************


            //******************* cors start ***********************
            var urls = Configuration[SigmalHexConstant.SigmalCoresUrls].Split(',');
            services.AddCors(
                options =>
                options.AddPolicy(SigmalHexConstant.DefaultCorsPolicy,
                builder => builder.WithOrigins(urls).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials())
            );
            //******************* cors end ***********************

            services.AddScoped<IKnowledgeApplicationService, KnowledgeApplicationService>();

            //******************* autofac start ***********************
            // Create the container builder.
            var autofacBuilder = new ContainerBuilder();

            autofacBuilder.Register(c => new NullInterceptor());
            autofacBuilder.RegisterType<TCPCollectorApplicationService>().As<ITCPCollectorApplicationService>().EnableInterfaceInterceptors().InterceptedBy(typeof(NullInterceptor));
            autofacBuilder.Populate(services);
            this.ApplicationContainer = autofacBuilder.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
            //******************* autofac start ***********************
        }

        private string GetXmlCommentsPath()
        {
            var app = PlatformServices.Default.Application;
            return Path.Combine(app.ApplicationBasePath, Path.ChangeExtension(app.ApplicationName, "xml"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,SigmalHexContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var log = log4net.LogManager.GetLogger(repository.Name, typeof(Startup));
            log.Info("test log4net");

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseStatusCodePages();

            if (env.IsDevelopment())
            {
                //print all services
                app.Map("/allservices", builder => builder.Run(async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync($"<h1>所有服务{_services.Count}个</h1><table><thead><tr><th>类型</th><th>生命周期</th><th>Instance</th></tr></thead><tbody>");
                    foreach (var svc in _services)
                    {
                        await context.Response.WriteAsync("<tr>");
                        await context.Response.WriteAsync($"<td>{svc.ServiceType.FullName}</td>");
                        await context.Response.WriteAsync($"<td>{svc.Lifetime}</td>");
                        await context.Response.WriteAsync($"<td>{svc.ImplementationType?.FullName}</td>");
                        await context.Response.WriteAsync("</tr>");
                    }
                    await context.Response.WriteAsync("</tbody></table>");
                }));

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseMvc();

            //******************* swagger start ***********************
            app.UseSwagger();
            app.UseSwaggerUi("swagger/ui/index");
            //******************* swagger end ***********************

            //******************* cors start ***********************
            app.UseCors(SigmalHexConstant.DefaultCorsPolicy);
            //******************* cors end ***********************

            DbInitializer.Initialize(dbContext);
        }
    }
}
