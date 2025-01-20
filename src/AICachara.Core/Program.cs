using Microsoft.SemanticKernel;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var builder = Kernel.CreateBuilder();

builder.Services.AddAzureOpenAIChatCompletion(
    config["DEPLOYMENT_MODEL"],
    config["AZURE_OPEN_AI_ENDPOINT"],
    config["AZURE_OPEN_AI_KEY"]
);

var kernel = builder.Build();

var result = await kernel.InvokePromptAsync("Give me a list of exercises that i can do in the gym");

//app.MapGet("/", () => "Hello World!");

//app.Run();
