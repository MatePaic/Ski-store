using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController(IUnitOfWork unit) : BaseApiController
    {
        [HttpGet] //https://localhost:5001/api/products
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery]ProductSpecParams productSpecParams)
        {
            var specification = new ProductSpecification(productSpecParams);

            return await CreatePagedResult(
                unit.Repository<Product>(), specification, 
                productSpecParams.PageNumber, productSpecParams.PageSize);
        }

        [HttpGet("{id:int}")] // api/products/2
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            unit.Repository<Product>().Add(product);

            if (await unit.Complete())
            {
                return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
            }

            return BadRequest("Problem creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !unit.Repository<Product>().Exists(id))
                return BadRequest("Cannot update this product");

            unit.Repository<Product>().Update(product);

            if (await unit.Complete()) 
            {
                return NoContent();
            }

            return BadRequest("Problem updating the project");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);

            if (product == null) return NotFound();

            unit.Repository<Product>().Remove(product);

            if (await unit.Complete())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting the project");
        }

        [HttpGet("filters")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetFilters()
        {
            var brandSpecification = new BrandListSpecification();
            var typeSpecification = new TypeListSpecification();

            var brands = await unit.Repository<Product>().GetWithSpecificationAsync(brandSpecification);
            var types = await unit.Repository<Product>().GetWithSpecificationAsync(typeSpecification);

            return Ok(new {brands, types});
        }
    }
}
