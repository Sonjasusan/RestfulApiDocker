using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

//Ensimmäisen kurssipäivän (4.3.22 klo. 9:15-15) harjoitus
namespace RestfulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {
        [HttpGet]
        public ActionResult getBikes() { 

        List<Bike> bikes = new List<Bike>()
{
        new Bike()
        {
        Id = 1,
        Make = "Yamaha",
        YearModel = 2019
        },
        new Bike()
        {
        Id = 2,
        Make = "Suzuki",
        YearModel = 2019
        },
        new Bike()
        {
        Id = 3,
        Make = "Kawasaki",
        YearModel = 2020
        } 
        };
            return Ok(bikes);
        }

    }
}
