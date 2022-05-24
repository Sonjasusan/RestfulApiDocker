using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.Models;

namespace RestfulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly northwindContext db = new northwindContext();

        //Dependency injection tapa(tehdä sama kuin yllä)
        private readonly northwindContext db = new northwindContext(); //alustetaan tietokanta "tyhjänä"

        public UsersController(northwindContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var users = db.Users;

            foreach (var user in users)
            {
                user.Password = null; //nullataan salasana tieto
            }

            return Ok(users);
        }

        //Uuden lisääminen
        [HttpPost]
        public ActionResult PostCreateNew([FromBody] User u)
        {
            try
            {
                db.Users.Add(u);
                db.SaveChanges();
                return Ok("Lisättiin käyttäjä: " +u.UserName);
            }
            catch (Exception e)
            {
                return BadRequest("Lisääminen ei onnistunut, tässä tietoa: " + e.Message);

            }
        }
    }
}
