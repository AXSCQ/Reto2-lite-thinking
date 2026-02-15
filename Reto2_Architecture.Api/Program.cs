using Reto2_Architecture.Application;
using Reto2_Architecture.Domain;
using Reto2_Architecture.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<CreateOrderUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/orders", (string productName, CreateOrderUseCase coordinador) =>
{
    coordinador.Execute(productName);
    return Results.Ok($"Orden para {productName} creada y guardada con exito");
});

app.Run();