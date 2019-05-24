using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphiQl;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using OrderService.Infra.CrossCutting.IoC;
using OrderService.Model;
using OrderService.Model.InputTypes;
using OrderService.Model.ObjectTypes;
using OrderService.Model.Schemas;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace OrderService
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
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            // configure jwt authentication
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
            services.AddMediatR(typeof(Startup));
            NativeInjector.RegisterServices(services, Configuration);
            //graphql
            //ObjectTypes
            services.AddTransient<ProductType>();
            services.AddTransient<UnitType>();
            services.AddTransient<NameType>();
            services.AddTransient<IdentityType>();
            services.AddTransient<UIntGraphType>();
            services.AddTransient<GetAllUnitResponsType>();
            services.AddTransient<AddNewUnitResponseType>();
            //InputTypes
            services.AddTransient<UnitInputType>();
            //schemas
            services.AddScoped<OrderQuery>();
            services.AddScoped<OrderMutation>();
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            var sp = services.BuildServiceProvider();
            services.AddScoped<ISchema>(_ => new GraphQLSchema(type => (GraphType)sp.GetService(type)) { Query = sp.GetService<OrderQuery>() });


            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("X-Pagination"));
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
           
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseGraphiQl();
            app.UseSwagger();
            app.UseStaticFiles();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                
            });
            app.UseCors("CorsPolicy");
        }
        public static void RegisterServices(IServiceCollection services)
        {

        }
    }
}
