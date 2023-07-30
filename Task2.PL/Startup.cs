using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2.BLL.Interfaces;
using Task2.BLL.Repositories;
using Task2.DAL.Contexts;
using Task2.DAL.Models;
using Task2.PL.Mapping_Profiles;

namespace Task2.PL
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
			services.AddDbContext<MVCAppDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});
			//services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			//services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
			services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile()));
			services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
            services.AddAutoMapper(M => M.AddProfile(new RoleProfile()));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				//options.Password.RequiredLength = 5;

		    })
					.AddEntityFrameworkStores<MVCAppDbContext>()
					.AddDefaultTokenProviders();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "Account/Login";
					options.AccessDeniedPath = "Home/Error";
				});



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

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Account}/{action=Login}/{id?}");
			});
		}
	}
}
