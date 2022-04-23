using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Contracts;
using System;
using System.Linq;
using Northwind.Entities.DataTransferObject;
using AutoMapper;
using System.Collections.Generic;
using Northwind.Entities.Models;
using System.Threading.Tasks;
using Northwind.Entities.RequestFeatures;

namespace NorthwindWebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CategoryController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _repository.Category.GetAllCategoryAsync(trackChanges: false);

                //replace by categoryDto

                /*var categoryDto = categories.Select(c => new CategoryDto
                {
                    Id = c.CategoryId,
                    categoryName = c.CategoryName,
                    description = c.Description
                }).ToList();*/

                var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                return Ok(categoryDto);
            }
            catch (Exception ex)
            {

                _logger.LogError($"{nameof(GetCategories)} message : {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}", Name = "CategoryById")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _repository.Category.GetCategoryAsync(id, trackChanges : false);
            if (category == null)
            {
                _logger.LogInfo($"Category with id : {id} doesn't exist");
                return NotFound();
            }
            else
            {
                var categoryDto = _mapper.Map<CategoryDto>(category);
                return Ok(categoryDto);
            }
        }

        [HttpPost] 
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                _logger.LogError("Category object is null");
                return BadRequest("Category object is null");
            }

            var categoryEntity = _mapper.Map<Category>(categoryDto);
            _repository.Category.CreateCategoryAsync(categoryEntity);
            await _repository.SaveAsync();

            var categoryResult = _mapper.Map<CategoryDto>(categoryEntity);
            return CreatedAtRoute("CategoryById", new {id = categoryResult.categoryId}, categoryResult);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _repository.Category.GetCategoryAsync(id, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Category with Id : {id} not found");
                return NotFound();
            }

            _repository.Category.DeleteCategoryAsync(category);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                _logger.LogError($"Categry must not be null");
                return BadRequest($"Categry must not be null");
            }

            var categoryEntity = await _repository.Category.GetCategoryAsync(id, trackChanges: true);
            if (categoryEntity == null)
            {
                _logger.LogInfo($"Category with Id : {id} not found");
                return NotFound();
            }

            _mapper.Map(categoryDto, categoryEntity);
            _repository.Category.UpdateCategoryAsync(categoryEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery] CategoryParameters categoryParameters)
        {
            var categoryPage = await _repository.Category.GetPaginationCategoryAsync(categoryParameters, trackChanges: false);
            
            var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categoryPage);
            return Ok(categoryDto);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetSearchCategory([FromQuery] CategoryParameters categoryParameters)
        {
            var categorySearch = await _repository.Category.GetSearchCategory(categoryParameters, trackChanges: false);

            var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categorySearch);
            return Ok(categoryDto);
        }
    }
}
