using System;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models
{
    /// <summary>
    /// Represents a bug in the system.
    /// </summary>
    public class Bug
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bug.
        /// </summary>
        public int BugID { get; set; }

        /// <summary>
        /// Gets or sets the title of the bug.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the bug.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the status of the bug.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// Gets or sets the category ID of the bug.
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Gets or sets the category hierarchy of the bug.
        /// </summary>
        public string? CategoryHierarchy { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"BugID: {BugID}, Title: {Title}, Description: {Description}, Status: {Status}";
        }
    }
}