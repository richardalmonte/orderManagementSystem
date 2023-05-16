using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Contracts;
using ProductService.API.Contracts.V1.Requests;
using ProductService.API.Contracts.V1.Responses;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.API.Controllers.V1;

[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IProductService _productService;

    public ProductsController(IMapper mapper, IProductService productService)
    {
        _mapper = mapper;
        _productService = productService;
    }

    [HttpGet(ApiRoutes.Products.GetAll)]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            var productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return Ok(productResponses);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet(ApiRoutes.Products.Get)]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product is null)
            {
                return NotFound();
            }

            var productResponse = _mapper.Map<ProductResponse>(product);
            return Ok(productResponse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPost(ApiRoutes.Products.Create)]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRegistrationRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productRequest = _mapper.Map<Product>(request);

            var createdProduct = await _productService.CreateProductAsync(productRequest);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Products.Get.Replace("{productId}", createdProduct.Id.ToString());

            var response = _mapper.Map<ProductResponse>(createdProduct);
            return Created(locationUri, response);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPut(ApiRoutes.Products.Update)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] ProductUpdateRequest request)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product is null)
            {
                return NotFound();
            }

            var updatedProduct = _mapper.Map(request, product);
            await _productService.UpdateProductAsync(updatedProduct);

            var productResponse = _mapper.Map<ProductResponse>(updatedProduct);
            return Ok(productResponse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete(ApiRoutes.Products.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        if (product is null)
        {
            return NotFound();
        }

        await _productService.DeleteProductAsync(productId);
        return NoContent();
    }
}