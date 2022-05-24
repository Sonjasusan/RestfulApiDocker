using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.Models;

namespace RestfulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {

        //Dependency injection tapa (tehdä sama kuin yllä)
        private readonly northwindContext db = new northwindContext(); //alustetaan tietokanta "tyhjänä"

        public DocumentationController(northwindContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        [Route("documentation/{avainkoodi}")]
        public ActionResult GetKeycode(string avainkoodi)
        {
            try
            {
                var avain = db.Documentations.Where(c => c.Keycode == avainkoodi); //Haetaan avainkoodilla
                
                if (avainkoodi == null)
                {
                    return NotFound(DateTime.Now + " Documentation missing.");
                }
               
                return Ok(avain); //Jos löytyy tulostetaan avainkoodin tiedot

            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. " +e.Message);

            }
        }

        //30.03.22. verkkotapaaminen
        //Simon esimerkki documentations kotitehtävästä
        [HttpGet]
        [Route("{keycode}")]
        public ActionResult GetAll(string keycode)
        {
            var paths = db.Documentations.Where(d => d.Keycode == keycode);
            return Ok(paths);

        }
    }
}
