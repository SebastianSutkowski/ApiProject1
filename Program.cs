using _ApiProject1_;
using _ApiProject1_.Entities;
using _ApiProject1_.Services;
using _ApiProject1_.Models;
using _ApiProject1_.Models.Validators;
using _ApiProject1_.Middleware;
using _ApiProject1_.Authorization;
using NLog;
using NLog.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Text;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality"));
    options.AddPolicy("IsOver18", builder => builder.AddRequirements(new MinimumAge(20)));
});
builder.Services.AddScoped<IAuthorizationHandler, MinAgeHandler>();
builder.Services.AddScoped<IAuthorizationHandler, OperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinTwoRestaurantsUserHandler>();
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddDbContext<RestaurantDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDbConnection")));

builder.Services.AddScoped<RestaurantSeeder>();
builder.Services.AddScoped<IRestaurantService,RestaurantService>();
builder.Services.AddScoped<IDishService,DishService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserContextService,UserContextService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IPasswordHasher<User>,PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>,RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<RestaurantQuery>,RestaurantQueryValidator>();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(System.Reflection.Assembly.GetAssembly(typeof(RestaurantMappingProfile)));
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendClient", build =>

    build.AllowAnyMethod()
    .AllowAnyHeader()
    .WithOrigins(builder.Configuration["AllowedOrigins"])
    );
});
builder.Host.UseNLog();


var app = builder.Build();


// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("FrontendClient");
seeder.Seed();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
});



app.MapControllers();

app.Run();
