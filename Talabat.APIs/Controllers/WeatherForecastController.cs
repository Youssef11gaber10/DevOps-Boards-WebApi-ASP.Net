using Microsoft.AspNetCore.Mvc;

namespace Talabat.APIs.Controllers
{
    [ApiController]//to identify that will be a controller
  //  [Route("[controller]")]//this mean when you want to get this api need to write the name of the controller "WeatherForecastController"
    [Route("api/[controller]")]

    public class WeatherForecastController : ControllerBase//inherts from ControllerBase not Controller this have diffrent behaviour
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        #region dummy endPoint

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        } 
        #endregion

    }
}
