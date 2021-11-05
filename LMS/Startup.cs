using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LMS.Extensions;
using Microsoft.AspNetCore.Authorization;
using LMS.Authorization;
using Microsoft.AspNetCore.Http;

namespace LMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews().AddJsonOptions(o => { 
            o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                o.JsonSerializerOptions.PropertyNamingPolicy = null;   });
            services.AddRazorPages().AddRazorRuntimeCompilation();


            services.AddAuthorization(options =>
            {
                

                options.AddPolicy("EnvioTareaAuthorization",
                    policy =>
                        policy.AddRequirements(
                            new EnvioTareaAuthorization()
                        ));


                options.AddPolicy("CalificacionEnvioTareaAuthorization",
                    policy =>
                        policy.AddRequirements(
                            new CalificacionEnvioTareaAutorization()
                        ));


                options.AddPolicy("CalificarTareaAutorization",
                  policy =>
                      policy.AddRequirements(
                          new CalificarTareaAutorization()
                      ));


                options.AddPolicy("TareaAuthorization",
                 policy =>
                     policy.AddRequirements(
                         new TareaAuthorization()
                     ));

                options.AddPolicy("EstudianteCursoAutorization",
              policy =>
                  policy.AddRequirements(
                      new EstudianteCursoAutorization()
                  ));

                options.AddPolicy("ProfesorCursoAuthorization",
           policy =>
               policy.AddRequirements(
                   new ProfesorCursoAuthorization()
               ));


                options.AddPolicy("QuizAuthorization",
        policy =>
            policy.AddRequirements(
                new QuizAuthorization()
            ));


            });

            services.AddSingleton<IAuthorizationHandler, EnvioTareaAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, CalificacionEnvioTareaAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, CalificarTareaAutorizationHandler>();

            services.AddSingleton<IAuthorizationHandler, TareaAuthorizationHandler>();

            services.AddSingleton<IAuthorizationHandler, EstudianteCursoAutorizationHandler>();

            services.AddSingleton<IAuthorizationHandler, ProfesorCursoAuthorizationHandler>();

            services.AddSingleton<IAuthorizationHandler, QuizAuthorizationHandler>();

            services.AddDistributedMemoryCache();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = Context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMemoryCache();
            services.AddSession(options => {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });


            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();


            AppDomain.CurrentDomain.SetData("ContentRootPath", env.ContentRootPath);
            AppDomain.CurrentDomain.SetData("WebRootPath", env.WebRootPath);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area=Usuarios}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
