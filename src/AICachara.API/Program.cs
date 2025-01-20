using AICachara.API.Plugins.Exercise.ExerciseLibrary;
using AICachara.API.Plugins.MusicConcert;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// More information about kernel building:
// https://learn.microsoft.com/en-us/semantic-kernel/concepts/kernel?pivots=programming-language-csharp#using-dependency-injection
builder.Services.AddKernel()
    .Plugins.AddFromPromptDirectory("Prompts"); // not available in Dependency injection
//builder.Services.AddSingleton<ExerciseLibraryPlugin>();
builder.Services.AddSingleton<KernelPluginCollection>(serviceProvider =>
[
#pragma warning disable SKEXP0050
    KernelPluginFactory.CreateFromType<ConversationSummaryPlugin>(),
#pragma warning restore SKEXP0050
    KernelPluginFactory.CreateFromType<ExerciseLibraryPlugin>(),
    KernelPluginFactory.CreateFromType<MusicConcertPlugin>(),
    KernelPluginFactory.CreateFromType<MusicLibraryPlugin>()
    //KernelPluginFactory.CreateFromObject(serviceProvider.GetRequiredService<ExerciseLibraryPlugin>()),
]);
builder.Services.AddTransient(serviceProvider =>
{
    var pluginCollection = serviceProvider.GetRequiredService<KernelPluginCollection>();
    return new Kernel(serviceProvider, pluginCollection);
});

builder.Services.AddOpenAIChatCompletion(
    builder.Configuration["AI:OpenAI:DeploymentModel"],
    builder.Configuration["AI:OpenAI:ApiKey"],
    builder.Configuration["AI:OpenAI:OrganizationId"]
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
