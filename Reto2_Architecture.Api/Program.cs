using Reto2_Architecture.Application;
using Reto2_Architecture.Domain;
using Reto2_Architecture.Infrastructure;
using System.Net.Http;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddHttpClient(); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.UseHttpsRedirection();


app.MapPost("/orders", async (string productName, CreateOrderUseCase coordinador, IHttpClientFactory httpClientFactory) =>
{
    coordinador.Execute(productName);
    
    var client = httpClientFactory.CreateClient();
    try 
    {
        var response = await client.PostAsJsonAsync("http://payments-api:8080/payments", new { Product = productName });
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("[202 OK].");
        }
    } 
    catch(Exception ex) 
    {
         Console.WriteLine($"[Error] {ex.Message}");
    }

    return Results.Ok($"orden para {productName} creada y guardada con exito");
});

app.Run();