using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Web.Data;
using Web.Models;
using Web.Services;
using Web.Filters;
using Web.Repositories;
using Web.ApplicationContext;
using AutoMapper;
using Web.ViewModels;
using Newtonsoft.Json.Serialization;
using Web.Enums;
using Web.Middlewares;

namespace Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ShipWarsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ShipWarsDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(opt => opt.Filters.Add(typeof(OnlineFilter)))
                .AddJsonOptions(config =>
                {
                    config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            ;
            //services.AddCaching();
            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromMinutes(60);
                opt.CookieName = ".ShipWars";
            });

            // Add application services.
           
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICurrentUser, ShipWarsContext>();
            services.AddTransient<IActiveWar, ShipWarsContext>();
            services.AddScoped<IObserver<ActiveWarVm>, WarObserver>();
            services.AddScoped<WarService>();

            services.AddSingleton(Configuration);
            services.AddSingleton<FieldBuilder>();
            services.AddSingleton<GameConfig>();
            services.AddSingleton<IWarSessionManager, WarSessionManager>();
            services.AddScoped<NotificationsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, OnlinePlayerVm>().ReverseMap();
                cfg.CreateMap<Invitation, MyInvitationVm>()
                   .ProjectUsing(m => new MyInvitationVm
                   {
                       InvitationId = m.Id,
                       Nickname = m.Inviter.Nickname,
                       Wins = m.Invited.Wins,
                       Loses = m.Invited.Loses
                   });
                //.ForMember(vm => vm.InvitationId, conf => conf.MapFrom(i => i.Id))
                //.ForMember(vm => vm.Nickname, conf => conf.MapFrom(i => i.Inviter.Nickname))
                //.ForMember(vm => vm.Wins, conf => conf.MapFrom(i => i.Invited.Wins))
                //.ForMember(vm => vm.Loses, conf => conf.MapFrom(i => i.Invited.Loses))

                cfg.CreateMap<War, ActiveWarVm>()
                    .ForMember(dest => dest.Field, opt=>opt.Ignore())
                    .ForMember(dest=> dest.Player1Nickname, opt=>opt.MapFrom(src => src.Player1.Nickname))
                    .ForMember(dest => dest.Player2Nickname, opt => opt.MapFrom(src => src.Player2.Nickname))
                    .ForMember(dest => dest.IsFinished, opt => opt.MapFrom(src => src.Status == WarStatus.Finished));
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStatusCodePages();

            app.UseStaticFiles();
                   
            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseSession();

            app.UseWebSockets();

            app.UseMiddleware<WebSocketMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
