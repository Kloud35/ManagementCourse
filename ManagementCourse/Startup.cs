using ManagementCourse.Common;
using ManagementCourse.Models;
using ManagementCourse.Models.Context;
using ManagementCourse.Reposiory;
using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace ManagementCourse
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
            services.AddControllersWithViews();
            services.AddSession(cfg =>
            {
                cfg.IdleTimeout = TimeSpan.FromHours(1);
                cfg.Cookie.HttpOnly = true;
                cfg.Cookie.IsEssential = true;
            });
            services.AddScoped<LessonRepository>();
            services.AddScoped<CourseRepository>();
            services.AddScoped<CourseCatalogRepository>();
            services.AddScoped<FileCourseRepository>();
            services.AddScoped<CourseLessonHistoryRepository>();
            services.AddScoped<DepartmentRepository>();
            services.AddScoped<RTCContext>();
            services.AddScoped<CourseExamRepository>();
            services.AddScoped<GenericRepository<CourseExamResult>>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseStaticFiles();

            app.UseRouting();
            //Session
            app.UseSession();
            app.UseAuthorization();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(@"C:\Users\admin\Desktop\Test")),
                RequestPath = new PathString("/Test")
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(@"C:\Users\admin\Desktop\Test")),
                RequestPath = new PathString("/Test")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });

        }
    }
}
