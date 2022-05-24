using Microsoft.EntityFrameworkCore;
using RestfulApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Sallii kaikki yhteydet (esim. html)
builder.Services.AddCors(options =>
{
    options.AddPolicy("salliKaikki",
    builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});
// ------Connection string luetaan app settings.json tiedostosta--------------

builder.Services.AddDbContext<northwindContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("azure")
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

////Sallitaan kaik
app.UseCors("salliKaikki");
app.MapControllers();

app.Run();