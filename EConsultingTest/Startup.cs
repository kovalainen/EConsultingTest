using EConsultingTest.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EConsultingTest
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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			string conString = Configuration["ConnectionStrings:DefaultConnection"];
			services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(conString));
			services.AddTransient<IRepository<Interval>, Repository<Interval>>();
			services.AddTransient<IRepository<Log>, Repository<Log>>();
			services.AddTransient<Logger>();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(options =>
					{
						options.RequireHttpsMetadata = false;
						options.TokenValidationParameters = new TokenValidationParameters
						{
							ValidateIssuer = true,
							ValidIssuer = AuthOptions.ISSUER,
							ValidateAudience = true,
							ValidAudience = AuthOptions.AUDIENCE,
							ValidateLifetime = true,
							IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
							ValidateIssuerSigningKey = true,
						};
					});

			services.AddMvc();
		}
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}
