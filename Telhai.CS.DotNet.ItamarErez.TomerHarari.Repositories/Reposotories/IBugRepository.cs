using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Reposotories
{
    /// <summary>
    /// Interface for managing bugs in the system.
    /// </summary>
    public interface IBugRepository
    {
        /// <summary>
        /// Gets all bugs.
        /// </summary>
        /// <returns>A collection of all bugs.</returns>
        Task<IEnumerable<Bug>> GetAll();

        /// <summary>
        /// Gets a specific bug by ID.
        /// </summary>
        /// <param name="id">The ID of the bug.</param>
        /// <returns>The bug with the specified ID, or null if not found.</returns>
        Task<Bug?> Get(int id);

        /// <summary>
        /// Adds a new bug to the system.
        /// </summary>
        /// <param name="bug">The bug to add.</param>
        /// <returns>The ID of the added bug.</returns>
        Task<int> Add(Bug bug);

        /// <summary>
        /// Deletes a bug from the system by ID.
        /// </summary>
        /// <param name="id">The ID of the bug to delete.</param>
        Task Delete(int id);

        /// <summary>
        /// Updates an existing bug in the system.
        /// </summary>
        /// <param name="bug">The updated bug.</param>
        Task Update(Bug bug);
    }
}