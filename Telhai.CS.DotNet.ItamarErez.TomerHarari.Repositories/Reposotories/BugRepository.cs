using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Reposotories
{
    /// <summary>
    /// Repository for managing bugs using SQL Server.
    /// </summary>
    public class BugRepository : IBugRepository
    {
        private static BugRepository? _instance;
        private static readonly object _lock = new object();
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="BugRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing the connection string.</param>
        private BugRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        /// <summary>
        /// Initializes and returns the singleton instance of the <see cref="BugRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing the connection string.</param>
        /// <returns>The singleton instance of BugRepository.</returns>
        public static BugRepository Initialize(IConfiguration configuration)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new BugRepository(configuration);
                }
            }
            return _instance;
        }

        /// <summary>
        /// Adds a new bug to the database.
        /// </summary>
        /// <param name="bugToAdd">The bug to add.</param>
        /// <returns>The ID of the added bug.</returns>
        public async Task<int> Add(Bug bugToAdd)
        {
            string query = @"INSERT INTO Bugs (Title, Description, Status, CategoryID) 
                         OUTPUT INSERTED.BugID
                         VALUES (@Title, @Description, @Status, @CategoryID)";
            using var connection = new SqlConnection(_connectionString);
            var newId = await connection.ExecuteScalarAsync<int>(query, bugToAdd);
            return newId;
        }

        /// <summary>
        /// Deletes a bug from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the bug to delete.</param>
        public async Task Delete(int id)
        {
            string query = "DELETE FROM Bugs WHERE BugID = @Id";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(query, new { Id = id });
        }

        /// <summary>
        /// Gets a bug from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the bug to retrieve.</param>
        /// <returns>The bug with the specified ID, or null if not found.</returns>
        public async Task<Bug?> Get(int id)
        {
            string query = "SELECT * FROM Bugs WHERE BugID = @Id";
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Bug>(query, new { Id = id });
        }

        /// <summary>
        /// Gets all bugs from the database.
        /// </summary>
        /// <returns>A collection of all bugs.</returns>
        public async Task<IEnumerable<Bug>> GetAll()
        {
            string query = "SELECT BugID, Title, Description, Status, CategoryID FROM Bugs";
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Bug>(query);
        }

        /// <summary>
        /// Updates an existing bug in the database.
        /// </summary>
        /// <param name="bug">The updated bug.</param>
        public async Task Update(Bug bug)
        {
            string query = @"UPDATE Bugs 
                        SET Title = @Title, 
                            Description = @Description, 
                            Status = @Status, 
                            CategoryID = @CategoryID 
                        WHERE BugID = @BugID";
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(query, bug);
        }
    }
}