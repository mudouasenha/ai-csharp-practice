using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.SemanticKernel;

namespace AICachara.API.Prompts.Plugins.MusicConcert;

public class MusicLibraryPlugin
{
    // Using a planner combined with this plugin.
    [KernelFunction, Description("Get a list of music recently recently played by the user")]
    public static string GetRecentlyPlayed()
    {
        string dir = Directory.GetCurrentDirectory();
        string content = File.ReadAllText($"{dir}/Plugins/MusicConcert/recentlyplayed.txt");
        return content;
    }
    
    // Using a planner combined with this plugin.
    [KernelFunction, Description("Get a list of all music available to the user")]
    public static string GetMusicLibrary()
    {
        string dir = Directory.GetCurrentDirectory();
        string content = File.ReadAllText($"{dir}/Plugins/MusicConcert/musiclibrary.txt");
        return content;
    }
    
    [KernelFunction, Description("Add a song to the recently played list")]
    public static string AddToRecentExercises(
        [Description("The name of the artist")] string artist,
        [Description("The title of the song")] string song,
        [Description("The song genre")] string genre)
    {
        string filePath = $"{Directory.GetCurrentDirectory()}/Plugins/MusicConcert/recentlyplayed.txt";
        string jsonContent = File.ReadAllText(filePath);
        var recentlyLogged = (JsonArray) JsonNode.Parse(jsonContent);

        var newExercise = new JsonObject
        {
            ["artist"] = artist,
            ["song"] = song,
            ["genre"] = genre
        };

        recentlyLogged.Insert(0, newExercise);
        File.WriteAllText(filePath, JsonSerializer.Serialize(recentlyLogged, new JsonSerializerOptions { WriteIndented = true}));

        return $"Added '{song}' to recently played";
    }
}