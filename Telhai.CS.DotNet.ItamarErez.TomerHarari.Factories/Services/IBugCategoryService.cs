using System.Collections.Generic;
using System.Threading.Tasks;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Services
{
    /// <summary>
    /// Interface for managing bug categories.
    /// </summary>
    public interface IBugCategoryService
    {
        /// <summary>
        /// Gets all bug categories.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of bug categories.</returns>
        Task<IEnumerable<BugCategory>> GetAll();

        /// <summary>
        /// Adds a new bug category.
        /// </summary>
        /// <param name="category">The category to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the added category.</returns>
        Task<int> Add(BugCategory category);

        /// <summary>
        /// Updates an existing bug category.
        /// </summary>
        /// <param name="category">The category to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Update(BugCategory category);

        /// <summary>
        /// Deletes a bug category by ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Delete(int id);
    }
}