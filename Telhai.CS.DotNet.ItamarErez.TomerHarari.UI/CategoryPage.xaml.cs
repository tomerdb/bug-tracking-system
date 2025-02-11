using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Services;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;
using Telhai.CS.DotNet.ItamarErez.UI.Services;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.UI
{
    /// <summary>
    /// Interaction logic for CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : Page
    {
        private readonly IBugCategoryService _categoryService;
        private readonly IBugService _bugService;
        private List<int> expandedItems = new List<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryPage"/> class.
        /// </summary>
        /// <param name="categoryService">The service for managing bug categories.</param>
        /// <param name="bugService">The service for managing bugs.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="categoryService"/> or <paramref name="bugService"/> is null.
        /// </exception>
        public CategoryPage(IBugCategoryService categoryService, IBugService bugService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _bugService = bugService ?? throw new ArgumentNullException(nameof(bugService));
            InitializeComponent();

            UpdateCategoryButton.IsEnabled = false;
            AddChildButton.IsEnabled = false;
            GetCategoryButton.IsEnabled = false;

            CategoryTree.SelectedItemChanged += CategoryTree_SelectedItemChanged;
        }

        /// <summary>
        /// Handles the event when the selected item in the category tree changes.
        /// </summary>
        private void CategoryTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var isItemSelected = CategoryTree.SelectedItem != null;
            UpdateCategoryButton.IsEnabled = isItemSelected;
            AddChildButton.IsEnabled = isItemSelected;
            GetCategoryButton.IsEnabled = isItemSelected;
        }

        /// <summary>
        /// Handles the Page Loaded event.
        /// </summary>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadCategoriesAndBugs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Loads the categories and bugs from the services.
        /// </summary>
        private async Task LoadCategoriesAndBugs()
        {
            try
            {
                if (CategoryTree.Items.Count > 0)
                    StoreExpandedItems(CategoryTree);

                var categories = await _categoryService.GetAll();
                var categoryHierarchy = await BuildCategoryHierarchy(categories.ToList());
                CategoryTree.ItemsSource = categoryHierarchy;

                if (expandedItems.Any())
                    RestoreExpandedItems(CategoryTree);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load categories and bugs", ex);
            }
        }

        /// <summary>
        /// Stores the IDs of expanded items in the category tree.
        /// </summary>
        private void StoreExpandedItems(ItemsControl items)
        {
            expandedItems.Clear();
            foreach (BugCategory item in items.Items)
            {
                if (CategoryTree.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem tvi && tvi.IsExpanded)
                {
                    expandedItems.Add(item.Id);
                }
            }
        }

        /// <summary>
        /// Restores the expanded state of items in the category tree.
        /// </summary>
        private void RestoreExpandedItems(ItemsControl items)
        {
            foreach (BugCategory item in items.Items)
            {
                if (CategoryTree.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem tvi && expandedItems.Contains(item.Id))
                {
                    tvi.IsExpanded = true;
                }
            }
        }

        /// <summary>
        /// Builds the category hierarchy from a flat list of categories.
        /// </summary>
        private async Task<List<BugCategory>> BuildCategoryHierarchy(List<BugCategory> categories)
        {
            var bugs = await _bugService.GetAllBugs();
            foreach (var category in categories)
            {
                category.Bugs = bugs.Where(b => b.CategoryID == category.Id).ToList();
            }
            var lookup = categories.ToLookup(c => c.ParentCategoryId);
            foreach (var category in categories)
            {
                category.SubCategories = lookup[category.Id].ToList();
            }
            return lookup[null].ToList();
        }

        /// <summary>
        /// Handles the click event of the Add Category button.
        /// </summary>
        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (_categoryService == null)
            {
                MessageBox.Show("Category service is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                MessageBox.Show("Category name is required", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var category = new BugCategory
            {
                CategoryName = CategoryNameTextBox.Text,
                ParentCategoryId = null
            };

            if (CategoryTree.Items.Count > 0)
                StoreExpandedItems(CategoryTree);

            int id = await _categoryService.Add(category);
            category.Id = id;
            var categories = await _categoryService.GetAll();
            var categoryHierarchy = await BuildCategoryHierarchy(categories.ToList());

            CategoryTree.ItemsSource = categoryHierarchy;
            CategoryNameTextBox.Clear();

            if (expandedItems.Any())
                RestoreExpandedItems(CategoryTree);
        }

        /// <summary>
        /// Handles the click event of the Update Category button.
        /// </summary>
        private async void UpdateCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryTree.SelectedItem is BugCategory selectedCategory)
            {
                if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
                {
                    MessageBox.Show("Category name is required", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (CategoryTree.Items.Count > 0)
                    StoreExpandedItems(CategoryTree);

                selectedCategory.CategoryName = CategoryNameTextBox.Text;
                await _categoryService.Update(selectedCategory);

                var categories = await _categoryService.GetAll();
                var categoryHierarchy = await BuildCategoryHierarchy(categories.ToList());
                CategoryTree.ItemsSource = categoryHierarchy;
                CategoryNameTextBox.Clear();

                if (expandedItems.Any())
                    RestoreExpandedItems(CategoryTree);
            }
        }

        /// <summary>
        /// Handles the click event of the Delete Category button.
        /// </summary>
        private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryTree.SelectedItem is BugCategory selectedCategory)
            {
                if (selectedCategory.SubCategories.Any())
                {
                    MessageBox.Show("Cannot delete a category that has sub-categories.", "Delete Category", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (selectedCategory.Bugs.Any())
                {
                    MessageBox.Show("Cannot delete a category that has bugs.", "Delete Category", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (CategoryTree.Items.Count > 0)
                    StoreExpandedItems(CategoryTree);

                await _categoryService.Delete(selectedCategory.Id);

                var categories = await _categoryService.GetAll();
                var categoryHierarchy = await BuildCategoryHierarchy(categories.ToList());
                CategoryTree.ItemsSource = categoryHierarchy;
                CategoryNameTextBox.Clear();

                if (expandedItems.Any())
                    RestoreExpandedItems(CategoryTree);
            }
        }

        /// <summary>
        /// Handles the click event of the Get Category button.
        /// </summary>
        private void GetCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryTree.SelectedItem != null)
            {
                MessageBox.Show(CategoryTree.SelectedItem.ToString());
            }
        }

        /// <summary>
        /// Handles the click event of the Add Child button.
        /// </summary>
        private async void AddChild_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryTree.SelectedItem is BugCategory selectedCategory)
            {
                if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
                {
                    MessageBox.Show("Category name is required", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (CategoryTree.Items.Count > 0)
                    StoreExpandedItems(CategoryTree);

                var category = new BugCategory
                {
                    CategoryName = CategoryNameTextBox.Text,
                    ParentCategoryId = selectedCategory.Id
                };

                await _categoryService.Add(category);
                var categories = await _categoryService.GetAll();
                var categoryHierarchy = await BuildCategoryHierarchy(categories.ToList());

                CategoryTree.ItemsSource = categoryHierarchy;
                CategoryNameTextBox.Clear();

                if (expandedItems.Any())
                    RestoreExpandedItems(CategoryTree);
            }
            else
            {
                MessageBox.Show("Please select a category to add a child to");
            }
        }
    }
}