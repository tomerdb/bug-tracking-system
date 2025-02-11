using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Reposotories
{
    /// <summary>
    /// Interface for managing bug categories in the system.
    /// </summary>
    public interface IBugCategoryRepository
    {
        /// <summary>
        /// Gets all bug categories.
        /// </summary>
        /// <returns>A collection of all bug categories.</returns>
        Task<IEnumerable<BugCategory>> GetAll();

        /// <summary>
        /// Gets a specific bug category by ID.
        /// </summary>
        /// <param name="id">The ID of the bug category.</param>
        /// <returns>The bug category with the specified ID, or null if not found.</returns>
        Task<BugCategory?> Get(int id);

        /// <summary>
        /// Adds a new bug category to the system.
        /// </summary>
        /// <param name="category">The bug category to add.</param>
        /// <returns>The ID of the added bug category.</returns>
        Task<int> Add(BugCategory category);

        /// <summary>
        /// Deletes a bug category from the system by ID.
        /// </summary>
        /// <param name="id">The ID of the bug category to delete.</param>
        Task Delete(int id);

        /// <summary>
        /// Updates an existing bug category in the system.
        /// </summary>
        /// <param name="category">The updated bug category.</param>
        /// <param name="oldId">The ID of the bug category to update.</param>
        Task Update(BugCategory category, int oldId);
    }
}