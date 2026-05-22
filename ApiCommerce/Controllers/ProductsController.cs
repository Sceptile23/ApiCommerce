using System;
using ApiCommerce.Mapping;
using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;
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
        public readonly IMapper _Mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            this._ProductRepository = productRepository;
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
            var products = _ProductRepository.GetProductForCategory(productId);
            if(products == null)
            {
                ModelState.AddModelError("Customer", "The product no exists.");
                return NotFound(ModelState);
            }
            var productsDtos = _Mapper.Map<List<ProductDtos>>(products);
            return Ok(productsDtos);
        }
    }
}

