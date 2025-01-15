using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.SemanticKernel;

namespace AICachara.API.Prompts.Plugins.Exercise;

public class ExerciseLibraryPlugin
{
    [KernelFunction, Description("Get a list of the exercises to the user")]
    public static string GetExerciseLibrary()
    {
        string dir = Directory.GetCurrentDirectory();
        string content = File.ReadAllText($"{dir}/Plugins/Exercise/ExerciseLibrary/exerciselibrary.txt");
        return content;
    }

    [KernelFunction, Description("Get a list of exercises recently logged by the user")]
    public static string GetRecentExercises()
    {
        string dir = Directory.GetCurrentDirectory();
        string content = File.ReadAllText($"{dir}/Plugins/Exercise/ExerciseLibrary/recentexercises.txt");
        return content;
    }
    
    [KernelFunction, Description("Add a exercise to the recently logged list")]
    public static string AddToRecentExercises(
        [Description("The name of the exercise")] string exercise,
        [Description("The number of reps that was performed")] string reps,
        [Description("The number of sets that was performed")] string sets)
    {
        string filePath = $"{Directory.GetCurrentDirectory()}/Plugins/Exercise/ExerciseLibrary/recentexercises.txt";
        string jsonContent = File.ReadAllText(filePath);
        var recentlyLogged = (JsonArray) JsonNode.Parse(jsonContent);

        var newExercise = new JsonObject
        {
            ["exercise"] = exercise,
            ["reps"] = reps,
            ["sets"] = sets
        };

        recentlyLogged.Insert(0, newExercise);
        File.WriteAllText(filePath, JsonSerializer.Serialize(recentlyLogged, new JsonSerializerOptions { WriteIndented = true}));

        return $"Added '{exercise}' to recently played";
    }
}