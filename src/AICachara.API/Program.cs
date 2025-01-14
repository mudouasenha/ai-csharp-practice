using AICachara.API.Plugins;
using Microsoft.SemanticKernel;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// More information about kernel building:
// https://learn.microsoft.com/en-us/semantic-kernel/concepts/kernel?pivots=programming-language-csharp#using-dependency-injection
builder.Services.AddKernel(); // Add plugins here
builder.Services.AddSingleton(() => new InvoicePlugin());
builder.Services.AddSingleton<KernelPluginCollection>((serviceProvider) =>
[
    KernelPluginFactory.CreateFromObject(serviceProvider.GetRequiredService<InvoicePlugin>()),
]);
builder.Services.AddTransient((serviceProvider)=> {
    KernelPluginCollection pluginCollection = serviceProvider.GetRequiredService<KernelPluginCollection>();
    return new Kernel(serviceProvider, pluginCollection);
});

builder.Services.AddOpenAIChatCompletion(
    modelId: builder.Configuration["AI:OpenAI:DeploymentModel"], 
    apiKey: builder.Configuration["AI:OpenAI:ApiKey"],
    orgId: builder.Configuration["AI:OpenAI:OrganizationId"]
    //serviceId: "YOUR_SERVICE_ID", // Optional; for targeting specific services within Semantic Kernel
    );


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