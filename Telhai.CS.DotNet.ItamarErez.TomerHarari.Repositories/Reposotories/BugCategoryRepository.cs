using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Reposotories
{
    /// <summary>
    /// Repository for managing bug categories using SQL Server.
    /// </summary>
    public class BugCategoryRepository : IBugCategoryRepository
    {
        private static BugCategoryRepository? _instance;
        private static readonly object _lock = new object();
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="BugCategoryRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing the connection string.</param>
        private BugCategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        /// <summary>
        /// Initializes and returns the singleton instance of the <see cref="BugCategoryRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing the connection string.</param>
        /// <returns>The singleton instance of BugCategoryRepository.</returns>
        public static BugCategoryRepository Initialize(IConfiguration configuration)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new BugCategoryRepository(configuration);
                }
            }
            return _instance;
        }

        /// <summary>
        /// Adds a new bug category to the database.
        /// </summary>
        /// <param name="category">The bug category to add.</param>
        /// <returns>The ID of the added bug category.</returns>
        public async Task<int> Add(BugCategory category)
        {
            string query = @"INSERT INTO BugsCategory (CategoryName, ParentCategoryId) 
                OUTPUT INSERTED.Id
                VALUES (@CategoryName, @ParentCategoryId)";
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(query, category);
        }

        /// <summary>
        /// Deletes a bug category from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the bug category to delete.</param>
        public async Task Delete(int id)
        {
            string query = "DELETE FROM BugsCategory WHERE Id = @Id";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(query, new { Id = id });
        }

        /// <summary>
        /// Gets a bug category from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the bug category to retrieve.</param>
        /// <returns>The bug category with the specified ID, or null if not found.</returns>
        public async Task<BugCategory?> Get(int id)
        {
            string query = "SELECT * FROM BugsCategory WHERE Id = @Id";
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<BugCategory>(query, new { Id = id });
        }

        /// <summary>
        /// Gets all bug categories from the database.
        /// </summary>
        /// <returns>A collection of all bug categories.</returns>
        public async Task<IEnumerable<BugCategory>> GetAll()
        {
            string query = "SELECT * FROM BugsCategory";
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<BugCategory>(query);
        }

        /// <summary>
        /// Updates an existing bug category in the database.
        /// </summary>
        /// <param name="category">The updated bug category.</param>
        /// <param name="oldId">The ID of the bug category to update.</param>
        public async Task Update(BugCategory category, int oldId)
        {
            string query = @"UPDATE BugsCategory   
                     SET CategoryName = @CategoryName,
                         ParentCategoryId = @ParentCategoryId
                     WHERE Id = @oldId";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(query, new
            {
                category.CategoryName,
                category.ParentCategoryId,
                OldId = oldId
            });
        }
    }
}