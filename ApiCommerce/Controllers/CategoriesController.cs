using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ApiCommerce.Repository.IRepository;
using AutoMapper;
using ApiCommerce.Repository;
using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;
using ApiCommerce.Mapping.CategoryProfile;

namespace ApiCommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            var categoriesDto = new List<CategoryDtos>();
            foreach(var category in categories)
            {
                categoriesDto.Add(_mapper.Map<CategoryDtos>(category));
            }
            return Ok(categoriesDto);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryRepository.GetCategory(id);
            if (category == null)
            {
                return NotFound($"the category with the {id} does not exists.");
            }
            var categoryDtos = _mapper.Map<CategoryDtos>(category);
            
            return Ok(categoryDtos);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDtos createCategoryDtos)
        {
            if (createCategoryDtos == null)
            {
                return BadRequest(ModelState);
            }

            if(_categoryRepository.CategoryExists(createCategoryDtos.Name) == true)
            {
                ModelState.AddModelError("Customer", "The category exists");
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDtos);

            if (!_categoryRepository.CreateCateogry(category))
            {
                ModelState.AddModelError("Customer", "Server error 500");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new {id = category.Id}, category);
        }

    }
} 