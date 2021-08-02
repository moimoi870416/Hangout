using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hangout.Controllers;
using Hangout.Centers;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Hangout
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
            //用法1
            //services.AddCors();
            //用法2
            // CorsPolicy 是自訂的 Policy 名稱
            services.AddCors(options =>
            {
                    //註冊CORS的Policy原則
                    //設定允許的跨域來源 允許任何的Request Header 
                    options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:5000", "http://hallowed-nectar-316105.appspot.com", "http://35.201.214.140")
                    //policy.WithOrigins("http://localhost:5000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                    //policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hangout", Version = "v1" });
            });

            services.AddDbContext<Models.db.HangoutContext>(option => option.UseSqlServer(Configuration.GetConnectionString(
             "HangoutDbConnectionString")));
            services.AddScoped<EventCenter>();
            services.AddScoped<MessageCenter>();
            services.AddScoped<CommentCenter>();
            services.AddScoped<MemberCenter>();
            services.AddScoped<ParticipantCenter>();
            services.AddScoped<TestCenter>();
            services.AddScoped<FavoriteCenter>();
            services.AddScoped<FollowCenter>();
            services.AddScoped<RelationshipCenter>();
            services.AddScoped<InviteCenter>();
            services.AddScoped<NoticeCenter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //用法1
            //app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            //用法2
            //全域套用
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();//for the wwwroot folder
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"photo")),
                RequestPath = new PathString("/photo")
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hangout v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
