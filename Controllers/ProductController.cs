using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.DAO;



namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        //private readonly IProductRepository _productRepository;
        private readonly IProductDao _productDao;

        //public ProductController(IProductRepository productRespository)
        //{
        //    _productRepository = productRespository;
        //}
        public ProductController(IProductDao productDao)
        {
            _productDao = productDao;    //Injection of Interface into the controller
        }

        //[Route("Get-ALL-Products")]
        //[HttpGet]
        //public async Task<ActionResult<List<Product>>> GetProducts()
        //{
        //    var products = await _productDao.GetProducts();
        //    if (products == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(products);
        //}

        //[Route("/")]
        //[Route("")]
        //[Route("index")]
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productDao.GetProducts();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            //Product? pdtFound = await _productRepository.GetProductById(id);
            Product? pdtFound = await _productDao.GetProductById(id);
            if (pdtFound == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(pdtFound);
            }
        }

        ////'name' is the endpoint and 'GetProductByName' is the routing name.
        //[HttpGet("{name:alpha}")]
        //[HttpGet(@"{name:regex(^[[a-zA-Z ]]+$)}")]
        //public async Task<ActionResult<List<Product>>> GetProductsByName(string name)
        //{
        //    List<Product> list = null;
        //    list = await _productRepository.GetProductsByName(name);
        //    if (list != null)
        //    {
        //        return Ok(list);
        //    }
        //    else
        //    {
        //        return NotFound("No Product Found");
        //    }
        //}



        //[HttpGet()]
        //[Route("code/{code}")]
        //public async Task<ActionResult<Product?>> GetProductsByCode(string code)
        //{
        //    Product? pdtFound = await _productRepository.GetProductsByCode(code);
        //    if (pdtFound == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return Ok(pdtFound);
        //    }
        //}

        //[HttpGet("{rating:double}")]
        //public async Task<ActionResult<List<Product?>>> GetProductsByRating(double rating)
        //{
        //    List<Product?> list = null;
        //    list = await _productRepository.GetProductsByRating(rating);
        //    if (list != null)
        //    {
        //        return Ok(list);
        //    }
        //    else
        //    {
        //        return NotFound("No product found");
        //    }

        //}


        //// HttpPut is used to update the value of a variable in the data
        //// we are giving id because we are searching the element based on the id.
        //// to update the value of price to new_price we are using [FromBody] in the price args
        //// just give the value to replace th evalue in from body block in the frontend.
       
        //[HttpPut("{id}")]
        //public async Task<ActionResult<Product>> UpdateProduct(int id, double price)
        //{
        //    Product? product = null;
        //    product = await _productRepository.UpdateProductPrice(id, price);
        //    if (product != null)
        //    {
        //        return NoContent();
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }

        //    //return NoContent();
        //}

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (product != null)
            {
                if (ModelState.IsValid)
                {

                // The bool variable is created to await for the AddProduct function to execute the code and add the product to the product list.
                // AddProduct function is added in the productrepo as well as addproductrepo Interface.
                // _productRepository is the endpoint which point towards the Productrepo.
                    //bool res = await _productRepository.AddProduct(product);
                    int res = await _productDao.InsertProduct(product);

                    if (res > 0)
                    {
                        return CreatedAtRoute(nameof(GetProduct), new { id = product.ProductId }, product);
                    }
                }
                return BadRequest("failed to add product");
            }
            else
            {
                //return BadRequest("Product not found");
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> UpdateProduct(int id, decimal Price)
        {
            Product? product = null;
            int val = await _productDao.UpdateProductPriceById(id, Price);
            if (val > 0)  // Agar product exist karta hai toh changes hone do
            {
                return NoContent();
            }
            else  // else say product NotFound.
            {
                return NotFound("Id not found");
            }

        }

        //[HttpGet("price-range")]
        //public async Task<ActionResult<List<Product>>> GetProductsByPrice([FromHeader] int minPrice, [FromHeader] int maxPrice)
        //{
        //    List<Product> list = await _productRepository.GetProductsByPrice(minPrice, maxPrice);
        //    if (list != null)
        //    {
        //        return Ok(list);
        //    }
        //    else
        //    {
        //        return NotFound("No Product Found");
        //    }
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteProduct(int id)
        //{
        //    bool isDeleted = false;
        //    isDeleted = await _productRepository.RemoveProduct(id);

        //    if (isDeleted)
        //    {
        //        return NoContent();
        //    }
        //    else
        //    {
        //        return NotFound("Product Not Deleted");
        //    }
        //}


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            int deletedCount = 0;
            deletedCount = await _productDao.DeleteProductById(id);

            if (deletedCount > 0)
            {
                return NoContent();

            }
            else
            {
                return NotFound("Product Not deleted");
            }

        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            int count = await _productDao.GetTotalProductsCount();
            return Ok(count);
        }


        [HttpGet("sorted-by-price")]
        public async Task<ActionResult<List<Product>>> GetProductsSortedByPrice()
        {
            List<Product> products = await _productDao.GetProductsSortedByPrice();
            if (products.Count > 0)
            {
                return Ok(products);
            }
            else
            {
                return NotFound("No products found");
            }
        }

        [HttpGet("by-price-range/{minPrice}/{maxPrice}")]
        public async Task<ActionResult<List<Product>>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            List<Product> products = await _productDao.GetProductsByPriceRange(minPrice, maxPrice);
            if (products.Count > 0)
            {
                return Ok(products);
            }
            else
            {
                return NotFound("No products found in this price range");
            }
        }
    }
}