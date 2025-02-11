using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Services;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Implementations
{
    /// <summary>
    /// Service for managing bug categories using HTTP API calls.
    /// </summary>
    public class BugCategoryService : IBugCategoryService
    {
        private static BugCategoryService? _instance;
        private static readonly object _lock = new object();
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="BugCategoryService"/> class.
        /// </summary>
        private BugCategoryService()
        {
            _client = new HttpClient();
            _baseUrl = "http://localhost:5000/api";
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="BugCategoryService"/> class.
        /// </summary>
        public static BugCategoryService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new BugCategoryService();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets all bug categories from the API.
        /// </summary>
        /// <returns>A collection of all bug categories.</returns>
        public async Task<IEnumerable<BugCategory>> GetAll()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/bugcategories");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IEnumerable<BugCategory>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve categories: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds a new bug category through the API.
        /// </summary>
        /// <param name="category">The category to add.</param>
        /// <returns>The ID of the added category.</returns>
        public async Task<int> Add(BugCategory category)
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"{_baseUrl}/bugcategories", category);
                response.EnsureSuccessStatusCode();
                var createdCategory = await response.Content.ReadFromJsonAsync<BugCategory>();
                return createdCategory.Id;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to add category: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing bug category through the API.
        /// </summary>
        /// <param name="category">The category to update.</param>
        public async Task Update(BugCategory category)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"{_baseUrl}/bugcategories/{category.Id}", category);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to update category: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a bug category through the API.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        public async Task Delete(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"{_baseUrl}/bugcategories/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to delete category: {ex.Message}", ex);
            }
        }
    }
}