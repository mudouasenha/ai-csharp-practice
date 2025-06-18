
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//builder.Services.AddOpenAIChatCompletion(
//    builder.Configuration["AI:OpenAI:DeploymentModel"],
//    builder.Configuration["AI:OpenAI:ApiKey"],
//    builder.Configuration["AI:OpenAI:OrganizationId"]
//    //serviceId: "YOUR_SERVICE_ID", // Optional; for targeting specific services within Semantic Kernel
//);


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
