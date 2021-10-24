using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApiAutores.Filters;
using WebApiAutores.Services;
using WebApiAutores.Utilities;

[assembly:ApiConventionType(typeof(DefaultApiConventions))]
namespace WebApiAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();//Limpiamos el mapeo al obtener claims
            Configuration = configuration;
        }

        public IConfiguration Configuration {  get; set; } 

        public void ConfigureServices(IServiceCollection services)//servicio
        {
               services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://apirequest.io").AllowAnyMethod().AllowAnyHeader()
                    .WithExposedHeaders(new String[] {"cantidadTotalRegistros"});

                });
            });

          //  services.AddTransient<IServicio,servicioA>();
            //Al instancias IServicio se instancia automaticamente servicioA

          //  services.AddScoped<IServicio,servicioA>();
            //Tiempo de instancia aumentada misma instancia solo que al momento de http cambia

            //services.AddTransient<IServicio,servicioA>();
            ////Siempre misma instancia

            //services.AddTransient<servicioTransient>();

            //services.AddSingleton<servicioSingleton>();

            //services.AddScoped<servicioScoped>();

            //services.AddTransient<FiltroDeAccion>();    

            //services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                opciones =>
            {
                opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                   ClockSkew = TimeSpan.Zero
                };

            });

            // services.AddHostedService<EscribirArchivo>();

            services.AddControllers(opciones =>
            {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));
                opciones.Conventions.Add(new SwaggerVersionamientocs());
            }).AddJsonOptions(x => 
            x.JsonSerializerOptions.ReferenceHandler = 
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles)
            .AddNewtonsoftJson();

            services.AddDbContext<AplicationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() {
                    Title = "WebApiAutores",
                    Version = "v1",
                    Description ="Web Api Practica",
                    Contact = new OpenApiContact
                    {
                        Email="andresolaje@gmail.com",
                        Name="Andrés Olaje"
                    },
                    License= new OpenApiLicense
                    {
                        Name="MIT"
                    }
                    }
                );
                c.SwaggerDoc("v2", new() { Title = "WebApiAutores", Version = "v2" });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new String[]{}
                    }
                });
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(opciones =>
            {
                opciones.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin"));
                opciones.AddPolicy("EsVendedor", politica => politica.RequireClaim("esVendedor"));

            });

            services.AddDataProtection();

            services.AddTransient<GeneradorEnlaces>();
            services.AddTransient<HATEOASAutorFiltroAttribute>();
            services.AddSingleton<IActionContextAccessor,ActionContextAccessor>();

            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:ConnectionString"]);
           

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger) //middlewares
        {
            //  app.UseMiddleware<LoguearRespuesta>();

            //app.Map("/ruta1", app =>
            // {
            //     app.Run(async contexto =>
            //     {
            //         await contexto.Response.WriteAsync("Tuberia ");
            //     });

            // });

           // app.UseLoguearRespuesta(); 

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiAutores v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiAutores v2");

                });
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiAutores v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiAutores v2");

            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
