using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Services;
using Telhai.CS.DotNet.ItamarErez.UI.Services;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Interfaces
{
    /// <summary>
    /// Factory interface for creating services related to bugs and bug categories.
    /// </summary>
    public interface IServiceFactory
    {
        /// <summary>
        /// Creates an instance of a service for managing bugs.
        /// </summary>
        /// <returns>An instance of <see cref="IBugService"/>.</returns>
        IBugService CreateBugService();

        /// <summary>
        /// Creates an instance of a service for managing bug categories.
        /// </summary>
        /// <returns>An instance of <see cref="IBugCategoryService"/>.</returns>
        IBugCategoryService CreateCategoryService();
    }
}