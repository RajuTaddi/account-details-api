using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using account.details.api.middlewares;
using account.details.core.interfaces;
using account.details.core.services;
using account.details.infrastructure;
using account.details.infrastructure.interfaces;
using account.details.infrastructure.models;
using account.details.infrastructure.repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace account.details.api
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
            services.AddDbContext<GlobalDbContext>(opt => opt.UseInMemoryDatabase("Accounts"));
            services.AddTransient<IRepository<Account>, AccountRepository>();
            services.AddTransient<IRepository<Transaction>, TransactionRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITransactionService, TransactionService>();

            services.AddMvc().AddJsonOptions(options => 
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Accounts Api",
                    Version = "v1"
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalOrigin", builder =>
                {
                    // Can be Refactored to read from configuration file
                    builder.WithOrigins("http://localhost:3004");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseMiddleware<ExceptionMiddleware>();

            var context = app.ApplicationServices.GetService<GlobalDbContext>();
            TestData.LoadTestData(context);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Accounts Api v1");
            });

            app.UseCors("AllowLocalOrigin");

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
