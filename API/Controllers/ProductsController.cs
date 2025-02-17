using API.DTOs;
using API.ImageService;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController(
        IUnitOfWork unit, 
        IMapper mapper,
        IImageService imageService
    ) : BaseApiController
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] CreateProductDto productDto)
        {
            var product = mapper.Map<Product>(productDto);

            if (productDto.File != null)
            {
                var imageResult = await imageService.AddImageAsync(productDto.File);

                if (imageResult.Error != null)
                {
                    return BadRequest(imageResult.Error.Message);
                }

                product.PictureUrl = imageResult.SecureUrl.AbsoluteUri;
                product.PublicId = imageResult.PublicId;
            }

            unit.Repository<Product>().Add(product);

            if (await unit.Complete())
            {
                return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
            }

            return BadRequest("Problem creating product");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromForm] UpdateProductDto updateProductDto)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(updateProductDto.Id);
            if (product == null) return NotFound();

            mapper.Map(updateProductDto, product);

            if (updateProductDto.File != null)
            {
                var imageResult = await imageService.AddImageAsync(updateProductDto.File);

                if (imageResult.Error != null)
                    return BadRequest(imageResult.Error.Message);

                if (!string.IsNullOrEmpty(product.PublicId))
                    await imageService.DeleteImageAsync(product.PublicId);

                product.PictureUrl = imageResult.SecureUrl.AbsoluteUri;
                product.PublicId = imageResult.PublicId;
            }

            if (await unit.Complete())
            {
                return NoContent();
            }

            return BadRequest("Problem updating the project");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);

            if (product == null) return NotFound();

            if (!string.IsNullOrEmpty(product.PublicId))
                await imageService.DeleteImageAsync(product.PublicId);

            unit.Repository<Product>().Remove(product);

            if (await unit.Complete())
            {
                return Ok();
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
