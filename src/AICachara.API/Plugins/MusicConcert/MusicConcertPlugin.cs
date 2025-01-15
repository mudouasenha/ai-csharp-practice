using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace AICachara.API.Prompts.Plugins.MusicConcert;

public class MusicConcertPlugin
{
    // Using a planner combined with this plugin.
    [KernelFunction, Description("Get a list of upcoming concerts")]
    public static string GetTours()
    {
        string dir = Directory.GetCurrentDirectory();
        string content = File.ReadAllText($"{dir}/Plugins/MusicConcert/concertdates.txt");
        return content;
    }
}