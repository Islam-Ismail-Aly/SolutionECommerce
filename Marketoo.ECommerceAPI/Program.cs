
namespace Marketoo.ECommerceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Register Auto Mapper
            //builder.Services.AddAutoMapper(typeof(Program));
            //builder.Services.AddAutoMapper(typeof(RoleMappingProfile));
            AutoMapperConfiguration.Configure(builder.Services);

            // Allow CORS => Cross Origin Resource Sharing to consume my API
            builder.Services.AddCors();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("MarketooECommerceAPI", new OpenApiInfo
                {
                    Title = "MarketooECommerce",
                    Version = "v1",
                    Description = "MarketooECommerce Web API Application",
                    Contact = new OpenApiContact
                    {
                        Name = "Islam Ismail",
                        Email = "islam.ismail.ali@icloud.com",
                        Url = new Uri("https://www.linkedin.com/in/islam-ismail-ali/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "My License",
                        Url = new Uri("https://www.linkedin.com/in/islam-ismail-ali/")
                    }
                });

                options.SwaggerDoc("AuthenticationAPIv1", new OpenApiInfo
                {
                    Title = "Authentication",
                    Version = "v1",
                    Description = "Authentication API Endpoints",
                });

                // Add Definition for APIs
                options.SwaggerDoc("ProductAPIv1", new OpenApiInfo
                {
                    Title = "Product",
                    Version = "v1",
                    Description = "Product API Endpoints",
                });

                options.SwaggerDoc("AdminAPIv1", new OpenApiInfo
                {
                    Title = "Admin",
                    Version = "v1",
                    Description = "Admin API Endpoints",
                });

                // For Authorize the API with JWT Bearer Tokens

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT API KEY"
                });

                // For Authorize the End Points such as GET,POST 

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                options.EnableAnnotations();
            });


            // Configure the connection string
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Fluent Validation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

            // Validation for check if ModelState.IsValid or not
            builder.Services.AddScoped<ModelStateValidationAttribute>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Configure password requirements
                options.Password.RequireDigit = false; // Requires a digit (0-9)
                options.Password.RequireLowercase = false; // Requires a lowercase letter (a-z)
                options.Password.RequireUppercase = false; // Requires an uppercase letter (A-Z)
                options.Password.RequireNonAlphanumeric = false; // Does not require a non-alphanumeric character
                options.Password.RequiredLength = 8; // Minimum required password length
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<UserManager<ApplicationUser>>();

            builder.Services.Configure<JwtService>(builder.Configuration.GetSection("JWT"));

            // Add services UnitOfWork
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddScoped(typeof(IRoleRepository), typeof(RoleRepository));
            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            builder.Services.AddScoped(typeof(IDashboardService), typeof(DashboardService));
            builder.Services.AddScoped(typeof(ICommonRepository<>), typeof(CommonRepository<>));

            //Add service Account Authentication Service
            builder.Services.AddScoped(typeof(IAccountAuthenticationService), typeof(AccountAuthenticationService));

            // Add Authentication for JwtBearer Json Web Token
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };

            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    //    options.InjectStylesheet("/swagger-ui/custom.css");
                    options.SwaggerEndpoint("/swagger/AuthenticationAPIv1/swagger.json", "AuthenticationAPI");
                    options.SwaggerEndpoint("/swagger/AdminAPIv1/swagger.json", "AdminAPI");
                    options.SwaggerEndpoint("/swagger/ProductAPIv1/swagger.json", "ProductAPI");

                });
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseHttpsRedirection();

            app.UseCors(c => c.AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowAnyOrigin());

            //app.MapControllers();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Admin Map Controller Route
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "api/admin/{controller=Dashboard}/{action=Main}/{id?}");
            });

            app.Run();
        }
    }
}
