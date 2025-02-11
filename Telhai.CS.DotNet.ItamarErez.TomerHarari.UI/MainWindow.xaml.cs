using System;
using System.Windows;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Services;
using Telhai.CS.DotNet.ItamarErez.UI.Services;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBugCategoryService _categoryService;
        private readonly IBugService _bugService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="categoryService">The service for managing bug categories.</param>
        /// <param name="bugService">The service for managing bugs.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="categoryService"/> or <paramref name="bugService"/> is null.
        /// </exception>
        public MainWindow(IBugCategoryService categoryService, IBugService bugService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _bugService = bugService ?? throw new ArgumentNullException(nameof(bugService));
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event of the CategoryButton.
        /// Navigates to the CategoryPage.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CategoryPage(_categoryService, _bugService));
        }

        /// <summary>
        /// Handles the click event of the BugsButton.
        /// Navigates to the BugsPage.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void BugsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BugsPage(_bugService));
        }
    }
}