using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using TermProject.models;
using TermProject.models.WatchlistModels;
using TermProject.services.StockService;
using TermProject.Services.UserService;
using TermProject.services.WatchlistService;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    "Server=db-mysql-fra1-55994-do-user-15111911-0.c.db.ondigitalocean.com;Port=25060;User ID=doadmin;Password=AVNS_Wvj598dywbNenMoAW5k;Database=defaultdb";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        }
    );
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddIdentityCore<MyUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IWatchlistService, WatchlistService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(UserMappingProfile)));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(WatchlistMappingProfile)));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapIdentityApi<MyUser>();
    endpoints.MapControllers();
});
app.Run();