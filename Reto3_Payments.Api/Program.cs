using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/payments", (object paymentData, ILogger<Program> logger) =>
{
    logger.LogInformation("Recibiendo solicitud de pago...");
    logger.LogInformation("Procesando pago: {PaymentData}", paymentData);
    
    return Results.Ok(new { message = "Payment successfully processed" });
});

app.Run();
