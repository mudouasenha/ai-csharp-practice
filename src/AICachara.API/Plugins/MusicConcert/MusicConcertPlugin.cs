using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace AICachara.API.Plugins.MusicConcert;

public class MusicConcertPlugin
{
    // Using a planner combined with this plugin.
    [KernelFunction]
    [Description("Get a list of upcoming concerts")]
    public static string GetTours()
    {
        var dir = Directory.GetCurrentDirectory();
        var content = File.ReadAllText($"{dir}/Plugins/MusicConcert/concertdates.txt");
        return content;
    }
}
