namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models
{
    /// <summary>
    /// Represents a category for bugs in the system.
    /// </summary>
    public class BugCategory
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bug category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the bug category.
        /// </summary>
        public required string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parent category, if any.
        /// </summary>
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the subcategories of this bug category.
        /// </summary>
        public List<BugCategory> SubCategories { get; set; } = new List<BugCategory>();

        /// <summary>
        /// Gets or sets the bugs associated with this category.
        /// </summary>
        public List<Bug> Bugs { get; set; } = new List<Bug>();

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{CategoryName} (ID: {Id})";
        }
    }
}