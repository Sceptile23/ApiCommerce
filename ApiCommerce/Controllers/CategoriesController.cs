using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ApiCommerce.Repository.IRepository;
using AutoMapper;
using ApiCommerce.Repository;
using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;
using ApiCommerce.Mapping.CategoryProfile;
using Microsoft.AspNetCore.Cors;
using ApiCommerce.Constants.Policy;
using Microsoft.AspNetCore.Authorization;

namespace ApiCommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [EnableCors (PolicyName.AllowFrontend)]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // [EnableCors (PolicyName.AllowFrontend)]
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

        [AllowAnonymous]
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

        [HttpPatch ("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateCategory(int id, [FromBody] CreateCategoryDtos updateCategoryDto)
        {
            if (!_categoryRepository.CategoryExists(id))
            {
                ModelState.AddModelError("Customer", "Category not found");
                return StatusCode(404, ModelState);
            }

            if (_categoryRepository.CategoryExists(updateCategoryDto.Name))
            {
                ModelState.AddModelError("Customer", "Same name.");
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(updateCategoryDto);
            category.Id = id;

            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("Customer", "Server error.");
                return StatusCode(500, ModelState);
            }
            
            return CreatedAtRoute("GetCategory", new {id = category.Id}, category);
        }

        [HttpDelete ("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryRepository.GetCategory(id);

            if(category == null)
            {
                return NotFound($"the Category {id} not exists");
            }

            if (!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("Customer", "Server error.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
} 