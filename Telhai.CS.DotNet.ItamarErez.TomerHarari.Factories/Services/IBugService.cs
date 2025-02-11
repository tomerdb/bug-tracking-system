using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;

namespace Telhai.CS.DotNet.ItamarErez.UI.Services
{
    /// <summary>
    /// Interface for managing bugs and bug categories.
    /// </summary>
    public interface IBugService
    {
        /// <summary>
        /// Gets all bugs.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of bugs.</returns>
        Task<IEnumerable<Bug>> GetAllBugs();

        /// <summary>
        /// Adds a new bug.
        /// </summary>
        /// <param name="bug">The bug to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the added bug.</returns>
        Task<int> AddBug(Bug bug);

        /// <summary>
        /// Deletes a bug by ID.
        /// </summary>
        /// <param name="id">The ID of the bug to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteBug(int id);

        /// <summary>
        /// Updates an existing bug.
        /// </summary>
        /// <param name="bug">The bug to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateBug(Bug bug);

        /// <summary>
        /// Gets all bug categories.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of bug categories.</returns>
        Task<IEnumerable<BugCategory>> GetAllCategories();
    }
}