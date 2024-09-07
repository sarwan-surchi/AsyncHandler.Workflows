using AsyncHandler.EventSourcing;
using AsyncHandler.EventSourcing.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration["AzureSqlDatabase"] ??
    throw new Exception($"not connection string found");
builder.Services.AddAsyncHandler(ah =>
{
    ah.AddEventSourcing(source => source.SelectEventSource(EventSources.AzureSql, conn));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("placeorder", async (IEventSource<OrderAggregate> service) =>
{
    var aggregate = await service.CreateOrRestore();
    aggregate.PlaceOrder(new OrderPlaced());
    await service.Commit(aggregate);
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
