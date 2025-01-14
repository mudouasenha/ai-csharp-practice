using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

namespace AICachara.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    [Route("GetWeather")]
    public async Task<IActionResult> Get(Kernel kernel)
    {
        var temp = Random.Shared.Next(-20, 55);
        var result = new WeatherForecast
        (
            Date: DateOnly.FromDateTime(DateTime.Now),
            TemperatureC: temp,
            Summary: await kernel.InvokePromptAsync<string>($"Please provide a short description of the temp {temp} C")
        );

        return Ok(result);
    }
}