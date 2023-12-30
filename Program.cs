using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TermProject.services.StockService;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    "Server=db-mysql-fra1-55994-do-user-15111911-0.c.db.ondigitalocean.com;Port=25060;User ID=doadmin;Password=AVNS_Wvj598dywbNenMoAW5k;Database=defaultdb";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddIdentityCore<MyUser>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IStockService, StockService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapIdentityApi<MyUser>();
app.MapControllers();
app.Run();