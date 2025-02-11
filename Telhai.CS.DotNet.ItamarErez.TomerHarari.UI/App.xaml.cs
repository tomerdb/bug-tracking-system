using System.Windows;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Implementations;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Factories.Interfaces;

namespace Telhai.CS.DotNet.ItamarErez.TomerHarari.UI
{
    /// <summary>
    /// Represents the main application class for the WPF application.
    /// Handles application startup and initializes services.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Overrides the OnStartup method to configure services and launch the main window.
        /// </summary>
        /// <param name="e">Startup event arguments.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                // =========================================================
                // SERVICE FACTORY SELECTION
                // =========================================================
                // OPTION 1: Use API-based services (default)
                IServiceFactory serviceFactory = APIServiceFactory.Instance;

                // OPTION 2: Uncomment the following line to switch to JSON-based services
                // NOTE: Comment out the previous line when using this option
                //IServiceFactory serviceFactory = JsonServiceFactory.Instance;

                // =========================================================
                // ADDITIONAL SERVICE FACTORY OPTIONS
                // =========================================================
                // To add more service factories (e.g., SQL, Mock, etc.):
                // 1. Create a new class implementing IServiceFactory
                // 2. Implement the Singleton pattern similar to existing factories
                // 3. Add a new option here to switch between service sources

                // Create services using the selected factory
                var bugService = serviceFactory.CreateBugService();
                var categoryService = serviceFactory.CreateCategoryService();

                // Create and show the main window
                var mainWindow = new MainWindow(categoryService, bugService);
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                // Handle any startup errors
                MessageBox.Show(
                    $"Startup Error: {ex.Message}\n\nStack Trace: {ex.StackTrace}",
                    "Application Startup Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                Shutdown();
            }
        }
    }
}