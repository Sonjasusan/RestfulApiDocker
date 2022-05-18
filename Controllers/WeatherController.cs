using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RestfulApi.Controllers
{
    //Tämä osio ei toimi

    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        [Route("{key}")]
        public string GetWeather(string key)
        {
            WebClient client = new WebClient();
            try
            {
                string data = client.DownloadString("https://ilmatieteenlaitos.fi/saa/" + key);
                int index = data.IndexOf("<div class=\"apparent-temperature-value\">");
                if (index > 0)
                {
                    string weather = data.Substring(index + 40, 3);
                    return weather;
                }
            }
            finally
            {
                client.Dispose();
            }
            return "(unknown)";
        }
    }
}
