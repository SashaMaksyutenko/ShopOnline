using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models.Dtos;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await this.productRepository.GetItems();
                var productCategories = await this.productRepository.GetCategories();
                if (products == null || productCategories == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto(productCategories);
                    return Ok(productDtos);

                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "error retrieving Data from the database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await this.productRepository.GetItem(id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var productСategory = await this.productRepository.GetCategory(product.CategoryId);
                    var productDto = product.ConvertToDto(productСategory);
                    return Ok(productDto);

                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "error retrieving Data from the database");
            }
        }
        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductcategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await productRepository.GetCategories();
                var productCategoryDtos = productCategories.ConvertToDto();
                return Ok(productCategoryDtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "error retrieving Data from the database");
            }
        }
        [HttpGet]
        [Route("{categoryId}/GetItemsBycategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>>GetItemsBycategory (int categoryId)
        {
            try
            {
                var products = await productRepository.GetItemsByCategory(categoryId);
                var productCategories = await productRepository.GetCategories();
                var productDtos = products.ConvertToDto(productCategories);
                return Ok(productDtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "error retrieving Data from the database");

            }
        }
    }
}