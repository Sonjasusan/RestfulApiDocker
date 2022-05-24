using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.Models;

namespace RestfulApi.Controllers
{
    //Kurssitehtävä

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //private readonly northwindContext db = new northwindContext();
        
        //Dependency injection tapa (tehdä sama kuin yllä)
        private readonly northwindContext db = new northwindContext(); //alustetaan tietokanta "tyhjänä"

        public EmployeesController(northwindContext dbparam)
        {
            db = dbparam;
        }

        //Haku pääavaimella / yksi rivi (GET)
        [HttpGet]
        [Route("{Id}")]
        public ActionResult HaeYksi(int id)
        {
            try
            {
                var tuote = db.Employees.Find(id);
                if (tuote == null)
                {
                    return BadRequest("Työntekijää id:llä " + id + " ei löytynyt");
                }

                return Ok(tuote);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        //Haku kaikki rivit (GET)
        [HttpGet]
        public ActionResult HaeKaik()
        {
            var employees = db.Employees.Find();
            return Ok(employees);
        }

        //Haku jollain muulla, kuin pääavaimella, jolloin voi tulla 0-n riviä (GET)
        [HttpGet]
        [Route("employees/{kaupunki}")] //Haetaan kaupungilla
        public ActionResult HaeKaupungilla(string kaupunki)
        {
            var EmployeesCity = (from c in db.Employees where c.City == kaupunki select c).ToList();
            return Ok(EmployeesCity);
        }

        //Lisäys (POST)
        [HttpPost]
        public ActionResult LisaaTekija([FromBody] Employee emp)
        {
            try
            {
                db.Employees.Add(emp); //Lisätään työntekijä
                db.SaveChanges(); //tallennetaan
                return Ok("Lisättiin työntekijä: " + emp.HireDate); //onnistunut lisäys
            }
            catch (Exception e)
            {

                return BadRequest("Työntekijän lisääminen epäonnistui " + e.Message);
            }
        }

        //Päivitys (PUT)
        [HttpPut]
        [Route("{id}")]
        public ActionResult PaivitysMuokkaus(int id, [FromBody] Employee employee)
        {
            try
            {
                var employees = db.Employees.Find(id); //etsitään työntekijä id:llä
                if (employee != null)
                {
                    employee.FirstName = employee.FirstName;
                    employee.LastName = employee.LastName;
                    employee.Title = employee.Title;
                    employee.TitleOfCourtesy = employee.TitleOfCourtesy;
                    employee.BirthDate = employee.BirthDate;
                    employee.HireDate = employee.HireDate;
                    employee.Address = employee.Address;
                    employee.City = employee.City;
                    employee.Region = employee.Region;
                    employee.PostalCode = employee.PostalCode; 
                    employee.Country = employee.Country;
                    employee.HomePhone = employee.HomePhone;    
                    employee.Extension = employee.Extension;
                    employee.Photo = null;
                    employee.Notes = null;
                    employee.ReportsTo = null;
                    employee.PhotoPath = null;

                    db.SaveChanges(); //tallennetaan
                    return Ok("Muokattu työntekijää: " + id); //onnistunut tallennus
                }
                else
                {
                    return NotFound("Muokattavaa työntekijää tietoineen ei löytynyt.");
                }
            }
            catch (Exception e)
            {

                return BadRequest("Jokin meni pieleen." + e.Message);
            }
        }
        //Poisto (DELETE)
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Poista(int id)
        {
            var employee = db.Employees.Find(id); //Etsitään id:llä
            if (employee == null)
            {
                return NotFound("Työntekijää id:llä: " +id + " ei löytynyt.");

            }
            else
            {
                try
                {
                    db.Employees.Remove(employee); //poistetaan
                    db.SaveChanges(); //tallennetaan poisto
                    return Ok("Poisto onnistui." + "\n" + " Poistettiin työntekijä id:llä: " + id); //ilmoitus onnistuneesta poistosta
                }
                catch (Exception e)
                {

                    return BadRequest("Poisto epäonnistui. " + e.Message);
                }
            }
        }

    }
}
