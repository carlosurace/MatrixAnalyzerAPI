using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MatrixAnalyzer.Models;
using MatrixAnalyzer.Library.Models;
using MatrixAnalyzer.Library.Models.Responses;
using MatrixAnalyzer.Library.Repository.Concretes;
using MatrixAnalyzer.Library.Repository.Interfaces;
using System.IO;
using Microsoft.AspNetCore.SpaServices;

namespace MatrixAnalyzer
{
    public class Startup
    {
        public static JsonSerializerSettings SerializerSettings { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   
            SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                //HostingEnvironment.IsDevelopment() || HostingEnvironment.IsStaging() ? Formatting.Indented : Formatting.None,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                TypeNameHandling = new TypeNameHandling(),
                NullValueHandling = NullValueHandling.Include
                //HostingEnvironment.IsDevelopment() || HostingEnvironment.IsStaging()
                    //? NullValueHandling.Include
                    //: NullValueHandling.Ignore
            };
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader() );
            });
            services.Configure<ConnectionStrings>(Configuration.GetSection("connectionStrings"));
            services.AddSingleton<IDealershipRepository, DealershipRepository>(sp =>
                                                                                 {
                                                                                     var connectionStringOptions = sp.GetService<IOptions<ConnectionStrings>>();

                                                                                     return new DealershipRepository(connectionStringOptions.Value);
                                                                                 });
            services.AddControllers().AddNewtonsoftJson(options =>
                                                        {
                                                            options.SerializerSettings.Formatting = SerializerSettings.Formatting;
                                                            options.SerializerSettings.DateFormatHandling = SerializerSettings.DateFormatHandling;
                                                            options.SerializerSettings.DateTimeZoneHandling = SerializerSettings.DateTimeZoneHandling;
                                                            options.SerializerSettings.ContractResolver = SerializerSettings.ContractResolver;
                                                            options.SerializerSettings.TypeNameHandling = SerializerSettings.TypeNameHandling;
                                                            options.SerializerSettings.NullValueHandling = SerializerSettings.NullValueHandling;
                                                        });
            
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
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

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

        }
    }
}
