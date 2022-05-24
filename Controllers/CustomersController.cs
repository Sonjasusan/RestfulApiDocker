using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.Models;

namespace RestfulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //private readonly northwindContext db = new northwindContext();
        
        //Dependency injection tapa(tehdä sama kuin yllä)
        private readonly northwindContext db = new northwindContext(); //alustetaan tietokanta "tyhjänä"

        public CustomersController(northwindContext dbparam)
        {
            db = dbparam;
        }

        //GET - haetaan kaikki asiakkaat
        [HttpGet]
        public ActionResult GetAll()
        {
            var customers = db.Customers;
            return Ok(customers);
        }

        // GET - yksi asiakas haetaan Id:n mukaan
        [HttpGet]
        [Route("{Id}")]
        public ActionResult GetOneById(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id); //Haetaan asiakas
                if (asiakas == null)
                {
                    return BadRequest("Asiakasta id:llä " + id + " ei löytynyt");
                }

                return Ok(asiakas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // DELETE - Poisto id:n perusteella
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Remove(string id)
        {
            var customer = db.Customers.Find(id);

            if(customer == null)
            {
                return NotFound("Asiakasta id:llä "+ id + " ei löytynyt.");
            }
            else
            {
                try { 
                db.Customers.Remove(customer); //Poisto
                db.SaveChanges(); //Tallennus

                    return Ok("Poistettiin asiakas " +customer.CompanyName); //Palautetaan kuittaus käyttöliittymään
                }
                catch(Exception e)
                {
                    return BadRequest("Poisto ei onnistunut, asiakkaalla saattaa olla voimassaolevia tilauksia. " + e.Message);
                }

            }

        }
        //Uuden lisäys
        [HttpPost]
        public ActionResult PostCreateNew([FromBody]Customer asiakas)
        {
            try
            {
                db.Customers.Add(asiakas);
                db.SaveChanges();
                return Ok("Lisättiin asiakas" + asiakas.CompanyName);

                //Toisia tapoja palauttaa (formatted string & templated string:
                //return Created("..api/customers", asiakas);
                //return Ok($"Lisättiin asiakas { asiakas.CompanyName}");
                //int x = 0; <-voidaan lisätä muuttuja ennen returnia
                // -> ja käyttää sitä return Ok($"Lisättiin asiakas { asiakas.CompanyName} abc {x}");
            }
            catch (Exception e)
            {
                return BadRequest("Asiakkaan lisääminen ei onnistunut "+ e);
            }
        }

        //Muokkaus
        [HttpPut]
        [Route("{id}")]
        public ActionResult PutEdit(string id, [FromBody] Customer asiakas)
        {
            if (asiakas == null) // Jos asiakasta ei ole
            {
                return BadRequest("Asiakas puuttuu pyynnön bodysta.");
            }

            try
            {
                var customer = db.Customers.Find(id);
                if (customer != null)
                {
                    customer.CompanyName = asiakas.CompanyName;
                    customer.ContactName = asiakas.ContactName;
                    customer.ContactTitle = asiakas.ContactTitle;
                    customer.Country = asiakas.Country;
                    customer.Address = asiakas.Address;
                    customer.City = asiakas.City;
                    customer.PostalCode = asiakas.PostalCode;
                    customer.Phone = asiakas.Phone;
                    customer.Fax = asiakas.Fax;

                    db.SaveChanges();
                    return Ok("Muokattu asiakasta: " +customer.CompanyName);
                }
                else
                {
                    return NotFound("Päivitettävää asiakasta ei löytynyt!");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Jokin meni pieleen asiakasta päivitettäessä. Alla lisätietoa" + e);
            }

        }
        //Haku maan mukaan
        [HttpGet]
        [Route("country/{maa}")]
        public ActionResult GetCountryPeople(string maa)
        {
            var countryCustomers = (from c in db.Customers
                                    where c.Country == maa
                                    select c).ToList();

            //Muita tapoja tehdä sama:
            //var cust = db.Customers.Where(c => c.Country == maa);
            //var cust = db.Customers.Where(c => c.Country.ToLower().Contains(maa.ToLower()));

            return Ok(countryCustomers);
        }

        //2. kurssipäivä 04.04.22

        //Haku maan ja kaupungin mukaan
        [HttpGet]
        [Route("country/{maa}/city/{city}")]
        public ActionResult GetByCityAndCountry(string maa, string city)
        {
            //var countryCustomers = (from c in db.Customers
            //                        where c.Country == maa
            //                        select c).ToList();

            //Muita tapoja tehdä sama:
            //var cust = db.Customers.Where(c => c.Country == maa);
            var cust = db.Customers.Where(c => c.Country.ToLower().Contains(maa.ToLower()) &&
            c.City.ToLower().Contains(city.ToLower()));

            return Ok(cust);
        }

    }
}
