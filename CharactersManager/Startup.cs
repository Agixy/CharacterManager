using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace CharactersManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  // typeof(Startup) LUB services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddDistributedMemoryCache();
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(1); });
            services.AddMvc();
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
         
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                endpoints.MapControllerRoute(
                    name: "filterCharacters",
                    pattern: "{controller}/{action}/{breedId}/{other}");
                endpoints.MapControllerRoute(
                  name: "addRelationship",
                  pattern: "{controller}/{action}/{characterId}/{relationshipCharacterId}/{relationship}");
                endpoints.MapControllerRoute(
                 name: "deleteCharacters",
                 pattern: "{controller}/{action}/{characterId}");
            });
        }
    }
}
