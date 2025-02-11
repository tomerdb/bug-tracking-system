using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telhai.CS.DotNet.ItamarErez.UI.Services;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.UI
{
    /// <summary>
    /// Interaction logic for BugsPage.xaml
    /// </summary>
    public partial class BugsPage : Page
    {
        private readonly IBugService _bugService;
        private List<BugCategory> _categories;

        /// <summary>
        /// Initializes a new instance of the <see cref="BugsPage"/> class.
        /// </summary>
        /// <param name="bugService">The service for managing bugs.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="bugService"/> is null.
        /// </exception>
        public BugsPage(IBugService bugService)
        {
            _bugService = bugService ?? throw new ArgumentNullException(nameof(bugService));
            InitializeComponent();
            BtnUpdate.IsEnabled = false;
            BugsListView.SelectionChanged += BugsListView_SelectionChanged;
        }

        /// <summary>
        /// Handles the selection changed event of the BugsListView.
        /// </summary>
        private void BugsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnUpdate.IsEnabled = BugsListView.SelectedItem != null;
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
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
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
                BugCategory.Items.Clear();
                var categories = await _bugService.GetAllCategories();
                _categories = categories.ToList();

                AddCategories(_categories);
                await LoadBugs();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load categories and bugs", ex);
            }
        }

        /// <summary>
        /// Adds categories to the ComboBox.
        /// </summary>
        private void AddCategories(List<BugCategory> categories)
        {
            if (categories == null) return;

            var lookup = categories.ToLookup(c => c.ParentCategoryId);

            foreach (var category in categories)
            {
                BuildCategoryHierarchy(category, lookup);
            }

            var rootCategories = categories.Where(c => c.ParentCategoryId == null).ToList();
            foreach (var rootCategory in rootCategories)
            {
                AddCategoryToComboBox(rootCategory);
            }
        }

        /// <summary>
        /// Builds the category hierarchy.
        /// </summary>
        private void BuildCategoryHierarchy(BugCategory category, ILookup<int?, BugCategory> lookup)
        {
            category.SubCategories = lookup[category.Id].ToList();

            foreach (var subCategory in category.SubCategories)
            {
                BuildCategoryHierarchy(subCategory, lookup);
            }
        }

        /// <summary>
        /// Adds a category to the ComboBox.
        /// </summary>
        private void AddCategoryToComboBox(BugCategory category, int level = 0)
        {
            string prefix = new string('-', level * 2);
            prefix = level > 0 ? prefix + " " : "";

            if (BugCategory.Items.Cast<ComboBoxItem>()
                .Any(item => item.Tag != null && (int)item.Tag == category.Id))
            {
                return;
            }

            var comboBoxItem = new ComboBoxItem
            {
                Content = prefix + category.CategoryName,
                Tag = category.Id
            };
            BugCategory.Items.Add(comboBoxItem);

            foreach (var subCategory in category.SubCategories)
            {
                AddCategoryToComboBox(subCategory, level + 1);
            }
        }

        /// <summary>
        /// Loads the bugs from the service.
        /// </summary>
        private async Task LoadBugs()
        {
            try
            {
                var bugs = await _bugService.GetAllBugs();
                foreach (var bug in bugs)
                {
                    bug.CategoryHierarchy = GetCategoryHierarchy(bug.CategoryID);
                }
                BugsListView.ItemsSource = bugs;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load bugs", ex);
            }
        }

        /// <summary>
        /// Gets the category hierarchy for a given category ID.
        /// </summary>
        private string GetCategoryHierarchy(int categoryId)
        {
            try
            {
                var category = _categories?.FirstOrDefault(c => c.Id == categoryId);
                if (category == null) return string.Empty;

                var hierarchy = new List<string>();
                while (category != null)
                {
                    hierarchy.Insert(0, category.CategoryName);
                    category = _categories.FirstOrDefault(c => c.Id == category.ParentCategoryId);
                }
                return string.Join(" -> ", hierarchy);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get category hierarchy for ID {categoryId}", ex);
            }
        }

        /// <summary>
        /// Handles the click event of the Add button.
        /// </summary>
        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                    string.IsNullOrWhiteSpace(StatusTextBox.Text) ||
                    BugCategory.SelectedIndex == -1)
                {
                    MessageBox.Show("Title, Status and Category are required");
                    return;
                }

                var categoryId = (int)((ComboBoxItem)BugCategory.SelectedItem).Tag;

                var bug = new Bug
                {
                    Title = TitleTextBox.Text.Trim(),
                    Description = DescriptionTextBox.Text?.Trim() ?? "",
                    Status = StatusTextBox.Text.Trim(),
                    CategoryID = categoryId,
                    CategoryHierarchy = GetCategoryHierarchy(categoryId)
                };

                int bugId = await _bugService.AddBug(bug);
                bug.BugID = bugId;
                await LoadBugs();

                TitleTextBox.Clear();
                DescriptionTextBox.Clear();
                StatusTextBox.Clear();
                BugCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding bug: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the click event of the Delete button.
        /// </summary>
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BugsListView.SelectedItem is Bug selectedBug)
                {
                    await _bugService.DeleteBug(selectedBug.BugID);
                    await LoadBugs();

                    TitleTextBox.Clear();
                    DescriptionTextBox.Clear();
                    StatusTextBox.Clear();
                    BugCategory.SelectedIndex = -1;
                    BugsListView.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting bug: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the click event of the Update button.
        /// </summary>
        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BugsListView.SelectedItem is Bug selectedBug)
                {
                    if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                        string.IsNullOrWhiteSpace(StatusTextBox.Text) ||
                        BugCategory.SelectedIndex == -1)
                    {
                        MessageBox.Show("Title, Status and Category are required");
                        return;
                    }

                    selectedBug.Title = TitleTextBox.Text.Trim();
                    selectedBug.Description = DescriptionTextBox.Text?.Trim() ?? "";
                    selectedBug.Status = StatusTextBox.Text.Trim();
                    selectedBug.CategoryID = (int)((ComboBoxItem)BugCategory.SelectedItem).Tag;

                    await _bugService.UpdateBug(selectedBug);
                    await LoadBugs();

                    TitleTextBox.Clear();
                    DescriptionTextBox.Clear();
                    StatusTextBox.Clear();
                    BugCategory.SelectedIndex = -1;
                    BugsListView.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating bug: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the click event of the Back button.
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}