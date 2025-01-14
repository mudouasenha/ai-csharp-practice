using AICachara.API.Plugins;
using Microsoft.SemanticKernel;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddKernel()
    .Plugins.AddFromType<InvoicePlugin>(); // Add plugins here

builder.Services.AddOpenAIChatCompletion(builder.Configuration["AI:OpenAI:DeploymentModel"], builder.Configuration["AI:OpenAI:ApiKey"]);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.UseHttpsRedirection();
app.MapControllers();
app.Run();