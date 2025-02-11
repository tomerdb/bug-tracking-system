using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Services;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;
using Telhai.CS.DotNet.ItamarErez.UI.Services;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Implementations
{
    /// <summary>
    /// Service for managing bugs using JSON file storage.
    /// </summary>
    public class JsonBugService : IBugService
    {
        private static JsonBugService? _instance;
        private static readonly object _lock = new object();
        private readonly string _bugFilePath;
        private readonly string _categoryFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonBugService"/> class.
        /// </summary>
        private JsonBugService()
        {
            _bugFilePath = "bugs.json";
            _categoryFilePath = "categories.json";
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="JsonBugService"/> class.
        /// </summary>
        public static JsonBugService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new JsonBugService();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets all bugs from the JSON file.
        /// </summary>
        /// <returns>A collection of all bugs.</returns>
        public async Task<IEnumerable<Bug>> GetAllBugs()
        {
            if (!File.Exists(_bugFilePath))
                return new List<Bug>();

            try
            {
                var json = await File.ReadAllTextAsync(_bugFilePath);
                return JsonConvert.DeserializeObject<List<Bug>>(json) ?? new List<Bug>();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to read bugs from JSON file", ex);
            }
        }

        /// <summary>
        /// Gets all bug categories from the JSON file.
        /// </summary>
        /// <returns>A collection of all bug categories.</returns>
        public async Task<IEnumerable<BugCategory>> GetAllCategories()
        {
            if (!File.Exists(_categoryFilePath))
                return new List<BugCategory>();

            try
            {
                var json = await File.ReadAllTextAsync(_categoryFilePath);
                return JsonConvert.DeserializeObject<List<BugCategory>>(json) ?? new List<BugCategory>();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to read categories from JSON file", ex);
            }
        }

        /// <summary>
        /// Adds a new bug to the JSON file.
        /// </summary>
        /// <param name="bug">The bug to add.</param>
        /// <returns>The ID of the newly added bug.</returns>
        public async Task<int> AddBug(Bug bug)
        {
            try
            {
                var bugs = (await GetAllBugs()).ToList();
                bug.BugID = bugs.Any() ? bugs.Max(b => b.BugID) + 1 : 1;
                bugs.Add(bug);
                await SaveToFileAsync(bugs);
                return bug.BugID;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add bug to JSON file", ex);
            }
        }

        /// <summary>
        /// Updates an existing bug in the JSON file.
        /// </summary>
        /// <param name="bug">The bug to update.</param>
        public async Task UpdateBug(Bug bug)
        {
            try
            {
                var bugs = (await GetAllBugs()).ToList();
                var existingBug = bugs.FirstOrDefault(b => b.BugID == bug.BugID);
                if (existingBug != null)
                {
                    existingBug.Title = bug.Title;
                    existingBug.Description = bug.Description;
                    existingBug.Status = bug.Status;
                    existingBug.CategoryID = bug.CategoryID;
                    await SaveToFileAsync(bugs);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update bug with ID {bug.BugID}", ex);
            }
        }

        /// <summary>
        /// Deletes a bug from the JSON file.
        /// </summary>
        /// <param name="id">The ID of the bug to delete.</param>
        public async Task DeleteBug(int id)
        {
            try
            {
                var bugs = (await GetAllBugs()).ToList();
                bugs.RemoveAll(b => b.BugID == id);
                await SaveToFileAsync(bugs);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete bug with ID {id}", ex);
            }
        }

        /// <summary>
        /// Saves the bugs collection to the JSON file.
        /// </summary>
        /// <param name="bugs">The collection of bugs to save.</param>
        private async Task SaveToFileAsync(List<Bug> bugs)
        {
            try
            {
                var json = JsonConvert.SerializeObject(bugs, Formatting.Indented);
                await File.WriteAllTextAsync(_bugFilePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save bugs to JSON file", ex);
            }
        }
    }
}