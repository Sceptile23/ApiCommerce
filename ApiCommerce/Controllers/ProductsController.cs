using System;
using ApiCommerce.Mapping;
using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;
using ApiCommerce.Repository;
using ApiCommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiCommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //instanciamos dos inyecciones de dependencias claves
        public readonly IProductRepository _ProductRepository;
        public readonly ICategoryRepository _CategoryRepository;
        public readonly IMapper _Mapper;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._ProductRepository = productRepository;
            this._CategoryRepository = categoryRepository;
            this._Mapper = mapper;

        }

        //CREACIÓN DE CADA ENDPOINT
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetProducts()
        {
            var products = _ProductRepository.GetProducts();
            var productsDtos = _Mapper.Map<List<ProductDtos>>(products);

            return Ok(productsDtos);
        }

        [HttpGet("{productId:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetProduct(int productId)
        {
            var product = _ProductRepository.GetProduct(productId);
            if(product == null)
            {
                ModelState.AddModelError("Customer", "The product no exists.");
                return NotFound(ModelState);
            }
            var productDto = _Mapper.Map<ProductDtos>(product);
            return Ok(productDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateProduct([FromBody] CreateProductDtos createProduct)
        {
            if(createProduct == null)
            {
                return BadRequest(ModelState);
            }

            if (_ProductRepository.ProductExists(createProduct.Name))
            {
                ModelState.AddModelError("Customer", "Product exists");
                return BadRequest(ModelState);
            }

            if (!_CategoryRepository.CategoryExists(createProduct.CategoryId))
            {
                ModelState.AddModelError("Customer", $"Category ID {createProduct.CategoryId} no exists");
                return BadRequest(ModelState);
            }

            var product = _Mapper.Map<Product>(createProduct);

            if (!_ProductRepository.CreateProduct(product)){
                ModelState.AddModelError("Customer", "Problem of server.");
                return StatusCode(500, ModelState);
            }

            var _createProduct = _ProductRepository.GetProduct(product.ProductId);
            var productDto = _Mapper.Map<CreateProductDtos>(_createProduct);
            return CreatedAtRoute("GetProduct", new {productId = product.ProductId}, productDto);
        }

        [HttpGet ("Category/{categoryID:int}", Name = "GetProductForCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetProductForCategory(int categoryID)
        {
            //VALIDAMOS QUE LA CATEGORÍA EXISTA EN LA BASE DE DATOS
            if (!_CategoryRepository.CategoryExists(categoryID))
            {
                return NotFound($"The category id {categoryID} not exists.");
            }

            //BUSCAMOS EL PRODUCTO SEGÚN LA CATEGORIA
            var products = _ProductRepository.GetProductForCategory(categoryID);
            var productDtos = _Mapper.Map<List<ProductDtos>>(products);

            //Devolvemos el producto
            return Ok(productDtos);
        }

        [HttpGet ("searchByNameProducr/{name}", Name = "SearchProduct")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SearchProduct(string name)
        {
            var products = _ProductRepository.SearchProduct(name);
            if (products.Count == 0)
            {
                return NotFound($"Product {name} not found.");
            }

            var productsDto = _Mapper.Map<List<ProductDtos>>(products);
            return Ok(productsDto);
        }

        [HttpPatch("BuyProduct/{name}/{amount:int}", Name = "BuyProduct")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BuyProduct (string name, int amount)
        {
            if(string.IsNullOrWhiteSpace(name) || amount <=0)
            {
                return BadRequest($"Invalidad name or amount");
            }

            if (!_ProductRepository.ProductExists(name))
            {
                return NotFound($"Product {name} not found.");
            }
            if(!_ProductRepository.BuyProduct(name, amount))
            {
                ModelState.AddModelError("Customer", "Error of transaction");
                return StatusCode(500, ModelState);
            }
            return Accepted($"he has bought {amount} {name}.");
        }

        [HttpPut ("updateProduct/{productId:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProduct (int productId, [FromBody] UpdateProductDtos updateProductDto)
        {
            //VALIDAMOS QUE LA ENTRADA DE DATOS NO SEA NULLA
            if(updateProductDto == null)
            {
                return BadRequest("Invalid data.");
            }

            //VALIDAMOS QUE EL PRODUCTO EXISTA ANTES DE ACTUALIZAR 
            var productDB = _ProductRepository.GetProduct(productId);
            if (productDB == null)
            {
                return NotFound($"Product {productId} not found.");
            }
            if (!_CategoryRepository.CategoryExists(updateProductDto.CategoryId))
            {
                return NotFound($"Category ID {updateProductDto.CategoryId} not found.");
            }

            //ACTUALIZAMOS LOS DATOS
            _Mapper.Map(updateProductDto, productDB);

            //SELECIONAMOS EL ID QUE VAMOS A ACTUALIZAR
            productDB.ProductId = productId;

            if (!_ProductRepository.UpdateProduct(productDB)){
                ModelState.AddModelError("Customer", "");
                return StatusCode(500, ModelState);
            }
            
            return CreatedAtRoute("GetProduct", new {productId = productDB.ProductId}, productDB);
        }

        //CONTROLADOR PARA ELIMINAR PRODUCTO
        [HttpDelete ("deleteProduct/{id:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteProduct(int id)
        {
            //BUSCAMOS EL PRODUCTO POR EL ID 
            var product = _ProductRepository.GetProduct(id);

            if(product == null)
            {
                return NotFound($"Product no found.");
            }

            //ELIMINAMOS EL PRODUCTO
            if (!_ProductRepository.DeleteProduct(product))
            {
                ModelState.AddModelError("Customer", "Error on delete product");
                return StatusCode(500, ModelState);
            }

            return Ok("Product deleted.");
        }
    }
}