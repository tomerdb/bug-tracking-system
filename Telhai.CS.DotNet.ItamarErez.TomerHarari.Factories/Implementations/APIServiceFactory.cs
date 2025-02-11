using System.Net.Http;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Interfaces;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Services;
using Telhai.CS.DotNet.ItamarErez.UI.Services;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Implementations
{
    /// <summary>
    /// Factory for creating API-based implementations of bug and category services.
    /// Implements the singleton pattern for efficient resource management.
    /// </summary>
    public class APIServiceFactory : IServiceFactory
    {
        private static APIServiceFactory? _instance;
        private static readonly object _lock = new object();
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="APIServiceFactory"/> class.
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private APIServiceFactory()
        {
            _baseUrl = "http://localhost:5000/api";
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="APIServiceFactory"/> class.
        /// Thread-safe implementation using double-check locking.
        /// </summary>
        public static APIServiceFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new APIServiceFactory();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Creates a new instance of the API-based bug service.
        /// </summary>
        /// <returns>An instance of IBugService that communicates with the API.</returns>
        public IBugService CreateBugService()
        {
            return BugService.Instance;
        }

        /// <summary>
        /// Creates a new instance of the API-based bug category service.
        /// </summary>
        /// <returns>An instance of IBugCategoryService that communicates with the API.</returns>
        public IBugCategoryService CreateCategoryService()
        {
            return BugCategoryService.Instance;
        }
    }
}