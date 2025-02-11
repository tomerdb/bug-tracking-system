using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Reposotories;

namespace Telhai.CS.DotNet.ItamarErez.API2.Controllers
{
    /// <summary>
    /// API controller for managing bugs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BugsController : ControllerBase
    {
        private readonly IBugRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BugsController"/> class.
        /// </summary>
        /// <param name="repository">The repository for managing bugs.</param>
        public BugsController(IBugRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets all bugs.
        /// </summary>
        /// <returns>A collection of bugs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bug>>> GetAll()
        {
            var bugs = await _repository.GetAll();
            return Ok(bugs);
        }

        /// <summary>
        /// Gets a specific bug by ID.
        /// </summary>
        /// <param name="id">The ID of the bug.</param>
        /// <returns>The bug with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Bug>> Get(int id)
        {
            var bug = await _repository.Get(id);
            if (bug == null) return NotFound();
            return Ok(bug);
        }

        /// <summary>
        /// Creates a new bug.
        /// </summary>
        /// <param name="bug">The bug to create.</param>
        /// <returns>The created bug.</returns>
        [HttpPost]
        public async Task<ActionResult<Bug>> Create(Bug bug)
        {
            bug.BugID = await _repository.Add(bug);
            return CreatedAtAction(nameof(Get), new { id = bug.BugID }, bug);
        }

        /// <summary>
        /// Updates an existing bug.
        /// </summary>
        /// <param name="id">The ID of the bug to update.</param>
        /// <param name="bug">The updated bug.</param>
        /// <returns>No content if the update is successful.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Bug bug)
        {
            if (id != bug.BugID) return BadRequest();
            await _repository.Update(bug);
            return NoContent();
        }

        /// <summary>
        /// Deletes a bug by ID.
        /// </summary>
        /// <param name="id">The ID of the bug to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Delete(id);
            return NoContent();
        }
    }
}