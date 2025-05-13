using APBD_kolokwium.Repositories;
using APBD_kolokwium.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IBookingsService, BookingsService>();
builder.Services.AddScoped<IBookingsRepository, BookingsRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();