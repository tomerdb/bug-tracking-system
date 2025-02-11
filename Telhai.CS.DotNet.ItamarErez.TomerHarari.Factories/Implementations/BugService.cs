using System.Net.Http;
using System.Net.Http.Json;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Services;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;
using Telhai.CS.DotNet.ItamarErez.UI.Services;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Implementations
{
    /// <summary>
    /// Service for managing bugs using HTTP API calls.
    /// Implements the singleton pattern for efficient resource management.
    /// </summary>
    public class BugService : IBugService
    {
        private static BugService? _instance;
        private static readonly object _lock = new object();
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="BugService"/> class.
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private BugService()
        {
            _client = new HttpClient();
            _baseUrl = "http://localhost:5000/api";
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="BugService"/> class.
        /// Thread-safe implementation using double-check locking.
        /// </summary>
        public static BugService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new BugService();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Gets all bugs from the API.
        /// </summary>
        /// <returns>A collection of all bugs.</returns>
        public async Task<IEnumerable<Bug>> GetAllBugs()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/Bugs");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IEnumerable<Bug>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve bugs. Error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all bug categories from the API.
        /// </summary>
        /// <returns>A collection of all bug categories.</returns>
        public async Task<IEnumerable<BugCategory>> GetAllCategories()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/BugCategories");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IEnumerable<BugCategory>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve categories. Error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds a new bug through the API.
        /// </summary>
        /// <param name="bug">The bug to add.</param>
        /// <returns>The ID of the added bug.</returns>
        public async Task<int> AddBug(Bug bug)
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"{_baseUrl}/Bugs", bug);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Server returned {response.StatusCode}: {errorContent}");
                }
                var createdBug = await response.Content.ReadFromJsonAsync<Bug>();
                return createdBug.BugID;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to add bug: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing bug through the API.
        /// </summary>
        /// <param name="bug">The bug to update.</param>
        public async Task UpdateBug(Bug bug)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"{_baseUrl}/Bugs/{bug.BugID}", bug);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to update bug: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a bug through the API.
        /// </summary>
        /// <param name="id">The ID of the bug to delete.</param>
        public async Task DeleteBug(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"{_baseUrl}/Bugs/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to delete bug: {ex.Message}", ex);
            }
        }
    }
}