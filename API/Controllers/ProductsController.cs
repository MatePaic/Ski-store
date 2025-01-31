using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")] //https://localhost:5001/api/products
    public class ProductsController(IGenericRepository<Product> productRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, 
            string? type, string? sort)
        {
            var specification = new ProductSpecification(brand, type, sort);

            var products = await productRepository.GetAsync(specification);

            return Ok(products);
        }

        [HttpGet("{id:int}")] // api/products/2
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            productRepository.Add(product);

            if (await productRepository.SaveChangesAsync())
            {
                return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
            }

            return BadRequest("Problem creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
                return BadRequest("Cannot update this product");

            productRepository.Update(product);

            if (await productRepository.SaveChangesAsync()) 
            {
                return NoContent();
            }

            return BadRequest("Problem updating the project");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null) return NotFound();

            productRepository.Remove(product);

            if (await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting the project");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var specification = new BrandListSpecification();

            var brands = await productRepository.GetAsync(specification);

            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var specification = new TypeListSpecification();

            var types = await productRepository.GetAsync(specification);

            return Ok(types);
        }

        private bool ProductExists(int id)
        {
            return productRepository.Exists(id);
        }
    }
}
