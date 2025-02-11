using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Interfaces;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Services;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Implementations;
using Telhai.CS.DotNet.ItamarErez.UI.Services;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Implementations
{
    /// <summary>
    /// Factory for creating JSON-based implementations of bug and category services.
    /// Implements the singleton pattern for efficient resource management.
    /// </summary>
    public class JsonServiceFactory : IServiceFactory
    {
        private static JsonServiceFactory? _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonServiceFactory"/> class.
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private JsonServiceFactory() { }

        /// <summary>
        /// Gets the singleton instance of the <see cref="JsonServiceFactory"/> class.
        /// Thread-safe implementation using double-check locking.
        /// </summary>
        public static JsonServiceFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new JsonServiceFactory();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Creates a new instance of the JSON-based bug service.
        /// </summary>
        /// <returns>An instance of IBugService that uses JSON file storage.</returns>
        public IBugService CreateBugService()
        {
            return JsonBugService.Instance;
        }

        /// <summary>
        /// Creates a new instance of the JSON-based bug category service.
        /// </summary>
        /// <returns>An instance of IBugCategoryService that uses JSON file storage.</returns>
        public IBugCategoryService CreateCategoryService()
        {
            return JsonBugCategoryService.Instance;
        }
    }
}