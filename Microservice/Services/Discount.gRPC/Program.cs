using Discount.gRPC.Infrastructure;
using Discount.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.GetSection("Settings").Bind(AppSettings.Settings);
builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
