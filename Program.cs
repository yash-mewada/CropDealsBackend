using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CropDeals.Models;
using CropDeals.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CropDeals.Repositories.Interfaces;
using CropDeals.Repositories;
using CropDeals.Mappings;
using Microsoft.OpenApi.Any;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Repository Services
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ICropRepository, CropRepository>();
builder.Services.AddScoped<ICropListingRepository, CropListingRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

// 3. Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 4. JWT Settings
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
    throw new Exception("JWT Key must be at least 256 bits (32 characters) long.");

// 5. Identity + Roles
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 6. JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// 7. Authorization
builder.Services.AddAuthorization();

// 8. Controllers
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

// 9. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CropDeals API", Version = "v1" });

    // Enable enums as strings for CropTypeEnum
    options.MapType<CropTypeEnum>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(CropTypeEnum))
                    .Select(n => new OpenApiString(n))
                    .Cast<IOpenApiAny>()
                    .ToList()
    });

    options.MapType<CropAvailability>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(CropAvailability))
                    .Select(n => new OpenApiString(n))
                    .Cast<IOpenApiAny>()
                    .ToList()
    });

    // Bearer JWT support
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter only your JWT token. 'Bearer' will be added automatically."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Build app
var app = builder.Build();

// 10. Use CORS policy
app.UseCors("AllowFrontend");

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CropDeals API V1");
    c.RoutePrefix = "";
});

// Seeding logic
async Task SeedRolesAndAdminAsync(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var config = services.GetRequiredService<IConfiguration>();

    string[] roles = new[] { "Admin", "Farmer", "Dealer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = role });
        }
    }

    var adminEmail = config["AdminUser:Email"];
    var adminPassword = config["AdminUser:Password"];

    if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
    {
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var user = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Admin",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}

// Call seeding
using (var scope = app.Services.CreateScope())
{
    await SeedRolesAndAdminAsync(scope.ServiceProvider);
}

// Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
