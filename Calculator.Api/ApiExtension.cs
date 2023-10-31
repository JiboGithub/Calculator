using Calculator.Api.Validators;
using Calculator.Application.Contract;
using Calculator.Application.Dtos.Request;
using Calculator.Application.Filter;
using Calculator.Application.Service;
using Calculator.Domain.Constants;
using Calculator.Domain.Models.Configuration;
using Calculator.Domain.Models.Response;
using Calculator.Infrastructure.Data;
using Calculator.Infrastructure.Extensions;
using Calculator.Infrastructure.Repository.Contracts;
using Calculator.Infrastructure.Repository.Implementation;
using Calculator.Infrastructure.Validation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;

namespace Calculator.Api
{
    public static class ApiExtension
    {
        public static void AddValidationExceptionHandler(this IServiceCollection services)
        {
            services.AddScoped<IValidationExceptionHandler, ValidationExceptionHandler>();
        }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration Configuration)
        {
            var config = Configuration.GetSection("DbConfig");
            DbConfig readconfig = config.Get<DbConfig>()!;

            services.Configure<DbConfig>(config);
            services.AddHttpContextAccessor();
            services.AddHttpClient();

            var migrator = new DatabaseMigrator(readconfig.SqlConnection!);
            migrator.Migrate();
            var dbConfig = new DbConfig();
            Configuration.Bind("DbConfig", dbConfig); // Ensure you have a "DbConfig" section in your appsettings.json

            // Register DbConfig instance to be used wherever it is injected
            services.AddSingleton(dbConfig);

            // Register the ISqlQuery service and its implementation
            services.AddScoped<ISqlQuery, SqlQuery>();
            services.AddScoped(typeof(IValidationService<>), typeof(ValidationService<>));

            services.AddScoped<ValidationFilter>();

            services.AddValidationService<CalculationRequest, CalculationValidator>();

            services.AddValidationServiceFactory();
            services.AddValidationExceptionHandler();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICalculatorService, CalculatorService>();

            services.AddEndpointsApiExplorer();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOriginsPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddResponseCaching();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Calculator API",
                    Version = "v1",
                    Description = "OMS Gera Calculator API Services.",
                    Contact = new OpenApiContact
                    {
                        Name = "Gera."
                    },
                });
            });

            services.AddAuthorization();
            services.AddControllers();

            services.AddRouting(opt => opt.LowercaseUrls = true);
            services.AddHealthChecks();
        }

        public static void ConfigureAppBuilder(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseExceptionHandler(
                appError =>
                {
                    appError.Run(async context =>
                    {
                        context.Response.StatusCode = ApiStatusConstants.InternalServerError;
                        context.Response.ContentType = "application/json";

                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            var exception = contextFeature.Error;
                            // Handle other exceptions
                            await context.Response.WriteAsync(
                                OperationResponse.ExceptionResponse("Unable to process your request at this time.").ToString());
                        }
                    }
                    );
                });

            app.UseCors("AllowAllOriginsPolicy");
            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseResponseCaching();

            app.UseAuthorization();
        }
    }
}
