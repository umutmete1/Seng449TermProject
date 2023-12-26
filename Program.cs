var builder = WebApplication.CreateBuilder(args);
var connectionString = "Server=db-mysql-fra1-55994-do-user-15111911-0.c.db.ondigitalocean.com;Port=25060;User ID=doadmin;Password=AVNS_Wvj598dywbNenMoAW5k;Database=defaultdb";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();

