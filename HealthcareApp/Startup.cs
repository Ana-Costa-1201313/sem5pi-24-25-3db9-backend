﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using HealthcareApp.Infraestructure;
using HealthcareApp.Infraestructure.Categories;
using HealthcareApp.Infraestructure.Shared;
using HealthcareApp.Domain.Shared;
using HealthcareApp.Domain.Categories;
using HealthcareApp.Domain.OperationTypes;
using HealthcareApp.Domain.Specializations;
using HealthcareApp.Infraestructure.Specializations;

namespace HealthcareApp
{
    public class Startup
    {
        public string DbPath { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "BD.db");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContext<BDContext>(opt =>
            //    opt.UseSqlServer("Server=vsgate-s1.dei.isep.ipp.pt,10513;User Id=sa;Password=rscxDifxGw==Xa5;encrypt=true;TrustServerCertificate=True;")
            //    .ReplaceService<IValueConverterSelector, StronglyEntityIdValueConverterSelector>());


            services.AddDbContext<BDContext>(opt =>
                opt.UseSqlite($"Data Source={DbPath}")
                .ReplaceService<IValueConverterSelector, StronglyEntityIdValueConverterSelector>());
        
            ConfigureMyServices(services);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureMyServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<CategoryService>();

            services.AddTransient<IOperationTypeRepository, OperationTypeRepository>();
            services.AddTransient<OperationTypeService>();

            services.AddTransient<ISpecializationRepository, SpecializationRepository>();
            services.AddTransient<SpecializationService>();
        }
    }
}