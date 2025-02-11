using System.Collections.Generic;
using System.Threading.Tasks;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Reposotories;
using Microsoft.AspNetCore.Mvc;

namespace Telhai.CS.DotNet.ItamarErez.API2.Controllers
{
    /// <summary>
    /// API controller for managing bug categories.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BugCategoriesController : ControllerBase
    {
        private readonly IBugCategoryRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BugCategoriesController"/> class.
        /// </summary>
        /// <param name="repository">The repository for managing bug categories.</param>
        public BugCategoriesController(IBugCategoryRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets all bug categories.
        /// </summary>
        /// <returns>A collection of bug categories.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BugCategory>>> GetAll()
        {
            var categories = await _repository.GetAll();
            return Ok(categories);
        }

        /// <summary>
        /// Gets a specific bug category by ID.
        /// </summary>
        /// <param name="id">The ID of the bug category.</param>
        /// <returns>The bug category with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BugCategory>> Get(int id)
        {
            var category = await _repository.Get(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        /// <summary>
        /// Creates a new bug category.
        /// </summary>
        /// <param name="category">The bug category to create.</param>
        /// <returns>The created bug category.</returns>
        [HttpPost]
        public async Task<ActionResult<BugCategory>> Create(BugCategory category)
        {
            category.Id = await _repository.Add(category);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        /// <summary>
        /// Updates an existing bug category.
        /// </summary>
        /// <param name="id">The ID of the bug category to update.</param>
        /// <param name="category">The updated bug category.</param>
        /// <returns>No content if the update is successful.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BugCategory category)
        {
            if (id != category.Id) return BadRequest();
            await _repository.Update(category, id);  // Pass the id as oldId
            return NoContent();
        }

        /// <summary>
        /// Deletes a bug category by ID.
        /// </summary>
        /// <param name="id">The ID of the bug category to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Delete(id);
            return NoContent();
        }
    }
}