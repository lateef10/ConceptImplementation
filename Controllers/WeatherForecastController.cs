using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PracticeConcept.API.Filters;
using PracticeConcept.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeConcept.Controllers
{
    //[ApiVersion("2.0")]
    //[Route("api/{v:apiversion}/weatherforecast")]
    [ApiController]
    [ResponseCache(CacheProfileName = "120SecondsDuration")]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
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

        [HttpGet]
        [ResponseCache(Duration = 60)] //you can also set a break point to see the cache effect
        public ActionResult<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogWarning("Calling Weather Forecast");

            //throw new Exception("Error occured");
            
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        //testing the WeatherForecast Filter Attribute
        [WeatherForecastFilter]
        [HttpGet("{val}", Name = "GetByMax")]
        public ActionResult<IEnumerable<WeatherForecast>> GetWeatherForecastByMax(int val)
        {
            _logger.LogWarning("Calling Weather Forecast By Max");

            var rng = new Random();
            return Enumerable.Range(1, val).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //testing the global Validation Action Filter by passing the wrong model
        [HttpPut("PutWeatherForecast")]
        public ActionResult<IEnumerable<WeatherForecast>> Put([FromBody] details details)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //testing the WeatherForecast Filter Attribute 
        [WeatherForecastFilter]
        [HttpDelete("DeleteWeatherForecast")]
        public ActionResult<IEnumerable<WeatherForecast>> Delete(int val)
        {
            return NoContent();
        }

        [HttpGet("ByPagination")]
        public ActionResult<IEnumerable<WeatherForecast>> GetByPagination([FromQuery] WeatherParameter weatherParameter)
        {
            var rng = new Random();
            return Enumerable.Range(1, 50).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }) // Skip = (3-1)*10 = 20
            .Skip((weatherParameter.pageIndex - 1) * weatherParameter.pageSize)
            .Take(weatherParameter.pageSize)
            .ToArray();
        }

        [HttpGet("UsingPageList")]
        public ActionResult<PageList<WeatherForecast>> GetByPageList([FromQuery] WeatherParameter weatherParameter)
        {
            var rng = new Random();
            var item = Enumerable.Range(1, 50).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).AsQueryable();

            var data = PageList<WeatherForecast>.ToPagedList(item, weatherParameter.pageIndex, weatherParameter.pageSize);

            var metadata = new
            {
                data.TotalCount,
                data.PageSize,
                data.CurrentPage,
                data.HasNext,
                data.HasPrevious
            };

            Response.Headers.Add("X-Pagigantion", JsonConvert.SerializeObject(metadata));
            _logger.LogInformation($"Returned {data.TotalCount} from the database");

            return data;
        }

    }
}
