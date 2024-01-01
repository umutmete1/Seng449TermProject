using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using TermProject.services.StockService;
using TermProject.services.UserService;

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
    options.AddPolicy("AllowDynamicOrigin",
        builder => builder.SetIsOriginAllowed(origin => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IWatchlistService, WatchlistService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope()){
    var services = scope.ServiceProvider;
    try{
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<MyUser>>();

        await DbInitializer.InitializeAsync(roleManager);

        var user = await userManager.FindByEmailAsync("abrenji55@gmail.com");
        if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }

    }
    catch (Exception e){
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occurred while seeding the database.");
    }
}

app.UseHttpsRedirection();
app.MapIdentityApi<MyUser>();
app.MapControllers();
app.Run();