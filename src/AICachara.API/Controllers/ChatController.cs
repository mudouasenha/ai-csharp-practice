using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Planning.Handlebars;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

namespace AICachara.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly ILogger<ChatController> _logger;

    public ChatController(ILogger<ChatController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("GetWeather")]
    public async Task<IActionResult> Get(Kernel kernel)
    {
        var temp = Random.Shared.Next(-20, 55);
        var result = new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now),
            temp,
            await kernel.InvokePromptAsync<string>($"Please provide a short description of the temp {temp} C")
        );

        return Ok(result);
    }

    [HttpGet]
    [Route("chat")]
    public async Task<IActionResult> Chat(Kernel kernel, [FromQuery] string prompt)
    {
        var response = await kernel.InvokePromptAsync<string>(prompt);

        return Ok(response);
    }

    [HttpGet]
    [Route("exercises-chathistory")]
    public async Task<IActionResult> ChatHistory(Kernel kernel, [FromQuery] string prompt)
    {
        var prompts = kernel.ImportPluginFromPromptDirectory("Prompts/Plugins/Exercise");
        ChatHistory history = [];
        string input;
        if (string.IsNullOrEmpty(prompt))
        {
            input = @"I want to build my leg strength. I love barbell exercises and compound exercises.";
        }
        else
        {
            input = prompt;
        }

        var response = await kernel.InvokeAsync<string>(prompts["SuggestExercises"],
            new KernelArguments { { "input", input } });

        history.AddUserMessage(input);
        history.AddAssistantMessage(response);

        return Ok(response);
    }

    [HttpGet]
    [Route("exercises-library")]
    public async Task<IActionResult> ExercisesLibrary(Kernel kernel)
    {
        var prompt = @"This is a list of exercises available to the user:
                {{ExerciseLibraryPlugin.GetExerciseLibrary}}

                This is a list of exercises that the user has recently performed:
                {{ExerciseLibraryPlugin.GetRecentExercises}}

                Based on their recent activity, suggest an exercise from the list to do next";

        var response = await kernel.InvokePromptAsync<string>(prompt);
        return Ok(response);
    }

    [HttpGet]
    [Description("""
                 Creates and a lists handlebars plans for music concerts and songs suggestion.
                 These can be used to either execute the kernel based on each plan, or to be saved
                 for later in a .txt file, and be used to create a function from Prompt.
                 """)]
    [Route("planners/music-concerts-and-songs-planners")]
    public async Task<IActionResult> MusicConcerts(Kernel kernel)
    {
        //throw new NotImplementedException();
#pragma warning disable SKEXP0060
        var planner = new HandlebarsPlanner(new HandlebarsPlannerOptions { AllowLoops = true });

        var location = "Redmond WA USA";
        var goal =
            @$"Based on the user's recently played music, suggest a concert for the user living in {location}";

        var concertSuggestionPlan = await planner.CreatePlanAsync(kernel, goal);

        var songSuggesterFunction = kernel.CreateFunctionFromPrompt(
            @"Based on the user's recently played music:
                {{recentlyPlayedSongs}}
                recommend a song to the user from the music library:
                {{$musicLibrary}}",
            functionName: "SuggestSong",
            description: "Suggest a song to the user"
        );

        kernel.Plugins.AddFromFunctions("SuggestSongPlugin", [songSuggesterFunction]);

        var songSuggestPlan = await planner.CreatePlanAsync(kernel, @"Suggest a song from the music library to the
            user based on their recently played songs");

        //var response = await concertSuggestionPlan.InvokeAsync(kernel);
        List<HandlebarsPlan> plans = [concertSuggestionPlan, songSuggestPlan];
        return Ok(plans);
#pragma warning restore SKEXP0060
    }

    [HttpGet]
    [Description("""
                 Gets a previously created plan for music concerts suggestion and executes it as a kernel function
                 considering the parameters needed. Before execution, it creates a function from Prompt considering the
                 .txt plan file.
                 """)]
    [Route("planners/music-concerts-and-songs-planners-execution")]
    public async Task<IActionResult> MusicConcertsExecution(Kernel kernel)
    {
        throw new NotImplementedException("Incomplete method, missing the musicLibrary Data, and musicLibraryPlugin");
#pragma warning disable SKEXP0060
        var songSuggesterFunction = kernel.CreateFunctionFromPrompt(
            @"Based on the user's recently played music:
                {{recentlyPlayedSongs}}
                recommend a song to the user from the music library:
                {{$musicLibrary}}",
            functionName: "SuggestSong",
            description: "Suggest a song to the user"
        );

        kernel.Plugins.AddFromFunctions("SuggestSongPlugin", [songSuggesterFunction]);

        var dir = Directory.GetCurrentDirectory();
        var template = System.IO.File.ReadAllText($"{dir}/../Plugins/MusicConcert/handlebarsTemplate.txt");

        var handleBarsPromptFunction = kernel.CreateFunctionFromPrompt(
            new PromptTemplateConfig { Template = template, TemplateFormat = "handlebars" },
            new HandlebarsPromptTemplateFactory()
        );

        var location = "Redmond WA USA";
        var templateResult = await kernel.InvokeAsync(handleBarsPromptFunction,
            new KernelArguments { { "location", location }, { "suggestConcert", false } });

        return Ok(templateResult);
#pragma warning restore SKEXP0060
    }
}
