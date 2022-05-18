using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.Models;

namespace RestfulApi.Controllers
{
    //Kurssitehtävä

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly northwindContext db = new northwindContext();

        ////Dependency injection tapa (tehdä sama kuin yllä)
        //private readonly northwindContext db = new northwindContext(); //alustetaan tietokanta "tyhjänä"

        //public ProductsController(northwindContext dbparam)
        //{
        //    db = dbparam; 
        //}



        //Haku pääavaimella / yksi rivi (GET)
        [HttpGet]
        [Route("{Id}")]
        public ActionResult HaeYksi(int id)
        {
            try
            {
                var tuote = db.Products.Find(id);
                if (tuote == null)
                {
                    return BadRequest("Tuotetta id:llä " + id + " ei löytynyt");
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
            var products = db.Products.Find();
            return Ok(products);
        }

        //Haku jollain muulla, kuin pääavaimella, jolloin voi tulla 0-n riviä (GET)
        [HttpGet]
        [Route("products/{tuotenimi}")]
        public ActionResult HaeTuoteNimellä(string tuotenimi) //Haetaan tuotenimellä
        {
            var ProductProdName = (from c in db.Products
                                    where c.ProductName == tuotenimi
                                    select c).ToList();
            return Ok(ProductProdName);
        }
        
        //Lisäys (POST)
        [HttpPost]
        public ActionResult LisaaTuote([FromBody]Product tuote)
        {
            try
            {
                db.Products.Add(tuote); //lisätään  
                db.SaveChanges(); //tallennetaan
                return Ok("Lisättiin tuote: " +tuote.ProductName); //onnistunut lisäys
            }
            catch (Exception e)
            {

                return BadRequest("Tuotteen lisääminen epäonnistui " + e.Message);
            }
        }
        //Päivitys (PUT)
        [HttpPut]
        [Route("{id}")]
        public ActionResult PaivitysMuokkaus(int id, [FromBody] Product product)
        {
            try
            {
                var products = db.Products.Find(id); //Etsitään muokattava/päivitettävä id:llä
                if (product != null)
                {
                    product.ProductName = product.ProductName;
                    product.SupplierId = product.SupplierId;
                    product.CategoryId = product.CategoryId;    
                    product.QuantityPerUnit = product.QuantityPerUnit;  
                    product.UnitPrice = product.UnitPrice;
                    product.UnitsInStock = product.UnitsInStock;    
                    product.UnitsOnOrder = product.UnitsOnOrder;
                    product.ReorderLevel = product.ReorderLevel;    
                    product.Discontinued = product.Discontinued;

                    db.SaveChanges(); //tallennetaan
                    return Ok("Muokattu tuotetta: " + product.ProductName); //onnistunut muokkaus
                }
                else
                {
                    return NotFound("Muokattavaa tuotetta ei löytynyt.");
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
        public ActionResult PoistaTuote(int id)
        {
            var product = db.Products.Find(id); //etsitään id:llä
            if (product == null)
            {
                return NotFound("Tuotetta id:llä: " +id +" ei löytynyt.");
            }
            else
            {
                try
                {
                    db.Products.Remove(product); //Poistetaan
                    db.SaveChanges(); //tallennetaan poisto

                    return Ok("Poisto onnistui." + "\n"+ " Poistettiin tuote id:llä: " + id); //onnistunut poisto
                }
                catch (Exception e)
                {
                    return BadRequest("Poisto epäonnistui. "+ e.Message);
                    
                }
            }
        } //Kurssitehtävä osa päättyy
        
        //30.03.22. verkkotapaaminen esimerkkejä/harjotuksia

        //Haku categoryid:llä 
        [HttpGet]
        [Route("cat/{id}")]
        public ActionResult GetByCatId(int cid)
        {
            var p = db.Products.Where(p => p.CategoryId == cid);
            return Ok(p);
        }

        //Haku kategoria nimellä ja taulujen yhdistäminen
        [HttpGet]
        [Route("cname/{cname}")]
        public ActionResult GetByCatname(string cname)
        {
            var products = (from p in db.Products
                            join c in db.Categories on p.CategoryId equals c.CategoryId
                            where c.CategoryName == cname
                            select p).ToList();
            return Ok(products);
        }

        //Haku minimi ja maximi hinnan mukaan
        [HttpGet]
        [Route("min-price/{min}/max-price/{max}")]
        public ActionResult GetByPrice(int min, int max)
        {
            var p = db.Products.Where(p => p.UnitPrice >= min && p.UnitPrice <= max);
            return Ok(p);
        }
        
        //2. kurssipäivä 04.04.22

        [HttpGet]
        [Route("special/{id}")]
        public ActionResult SpecialData(string productName) //Luotiin modeliin ProductData.cs
        {
            var prod = (from p in db.Products where p.ProductName.ToLower().Contains(productName.ToLower())
                        select p).FirstOrDefault();

            var cat = (from c in db.Categories where c.CategoryId == prod.CategoryId select c).FirstOrDefault();

            var sup = (from s in db.Categories where s.CategoryId == prod.CategoryId select s).FirstOrDefault();


            List<ProductData> productdata = new List<ProductData>();
            {
                new ProductData
                {
                    Id = prod.ProductId,
                    ProductName = prod.ProductName,
                    //Supplier = sup.CompanyName,
                    CategoryName = cat.CategoryName,
                };

                };
                return Ok(productdata);
        }

    }
}
