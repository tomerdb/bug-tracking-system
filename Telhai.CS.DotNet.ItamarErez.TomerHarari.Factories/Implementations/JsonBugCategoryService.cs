using Newtonsoft.Json;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Services;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Implementations
{
    /// <summary>
    /// Provides JSON file-based implementation of the bug category service.
    /// Implements the singleton pattern for efficient resource management.
    /// </summary>
    public class JsonBugCategoryService : IBugCategoryService
    {
        private static JsonBugCategoryService? _instance;
        private static readonly object _lock = new object();
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonBugCategoryService"/> class.
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private JsonBugCategoryService()
        {
            _filePath = "categories.json";
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="JsonBugCategoryService"/> class.
        /// Thread-safe implementation using double-check locking.
        /// </summary>
        public static JsonBugCategoryService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new JsonBugCategoryService();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Gets all bug categories from the JSON file.
        /// </summary>
        /// <returns>A collection of all bug categories.</returns>
        public async Task<IEnumerable<BugCategory>> GetAll()
        {
            if (!File.Exists(_filePath))
                return new List<BugCategory>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<List<BugCategory>>(json) ?? new List<BugCategory>();
        }

        /// <summary>
        /// Adds a new bug category to the JSON file.
        /// </summary>
        /// <param name="category">The category to add.</param>
        /// <returns>The ID of the added category.</returns>
        public async Task<int> Add(BugCategory category)
        {
            var categories = (await GetAll()).ToList();
            category.Id = categories.Any() ? categories.Max(c => c.Id) + 1 : 1;
            categories.Add(category);
            await SaveToFileAsync(categories);
            return category.Id;
        }

        /// <summary>
        /// Updates an existing bug category in the JSON file.
        /// </summary>
        /// <param name="category">The category to update.</param>
        public async Task Update(BugCategory category)
        {
            var categories = (await GetAll()).ToList();
            var existing = categories.FirstOrDefault(c => c.Id == category.Id);
            if (existing != null)
            {
                existing.CategoryName = category.CategoryName;
                existing.ParentCategoryId = category.ParentCategoryId;
                await SaveToFileAsync(categories);
            }
        }

        /// <summary>
        /// Deletes a bug category from the JSON file.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        public async Task Delete(int id)
        {
            var categories = (await GetAll()).ToList();
            categories.RemoveAll(c => c.Id == id);
            await SaveToFileAsync(categories);
        }

        /// <summary>
        /// Saves the categories to the JSON file.
        /// </summary>
        /// <param name="categories">The categories to save.</param>
        private async Task SaveToFileAsync(List<BugCategory> categories)
        {
            var json = JsonConvert.SerializeObject(categories, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}