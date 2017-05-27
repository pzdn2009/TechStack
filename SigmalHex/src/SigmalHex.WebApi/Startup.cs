using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using SigmalHex.Domain.KBContext.ApplicationServices;
using Swashbuckle.Swagger.Model;
using System.IO;

namespace SigmalHex.WebApi
{
    public class Startup
    {
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

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
        }

        private string GetXmlCommentsPath()
        {
            var app = PlatformServices.Default.Application;
            return Path.Combine(app.ApplicationBasePath, Path.ChangeExtension(app.ApplicationName, "xml"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var log = log4net.LogManager.GetLogger(repository.Name, typeof(Startup));
            log.Info("test log4net");

            app.UseMvc();

            //******************* swagger start ***********************
            app.UseSwagger();
            app.UseSwaggerUi("swagger/ui/index");
            //******************* swagger end ***********************

            //******************* cors start ***********************
            app.UseCors(SigmalHexConstant.DefaultCorsPolicy);
            //******************* cors end ***********************
        }
    }
}
