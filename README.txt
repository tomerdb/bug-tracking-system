# Bug Tracking System Setup Instructions

## Prerequisites

- SQL Server 2019 or later
- SQL Server Management Studio (SSMS) 18.0 or later
- Visual Studio 2022 with .NET 8.0 SDK
- Git (optional, for cloning the repository)

## Setup Process Overview

1. Database Setup using provided SQL scripts
2. Project Configuration in Visual Studio
3. Connection String Configuration
4. Running the Application

## Database Setup

1. Open SQL Server Management Studio (SSMS).
2. Connect to your database server.
3. Create a new database by executing the following SQL command:
   CREATE DATABASE Bugs;
4. Select the newly created database
5. Open and execute the provided create_tables.sql script
   - This script will create required tables
   - Insert initial categories
   - Insert sample bug data

## Project Configuration

1. Clone or download the project repository
2. Open the solution file `Telhai.CS.DotNet.ItamarErez.TomerHarari.FinalProject.sln`.
3. Configure Multiple Startup Projects:
   - Right-click on the Solution in Solution Explorer.
   - Select "Set Startup Projects...".
   - Choose "Multiple startup projects".
   - Set both API and UI projects to "Start".
   - Set the API project to start first, then the UI project.
   - Click Apply and OK.

3. Locate the `appsettings.json` file in the API project.
4. Update the connection string to match your database:
	{
	  "ConnectionStrings": {
		"DefaultConnection": "Data Source=YOUR-SERVER;Initial Catalog=Bugs;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True"
	  }
	}
	   Replace `YOUR-SERVER` with your SQL Server instance name (e.g., "DESKTOP-ABC123" or "localhost").

## Running the Project

1. After setting multiple startup projects, simply press `F5` or click `Start Debugging`.
2. The API project will start first, followed by the UI application.
3. Both projects must be running for the application to work correctly.

## Data Storage Options

The project supports two storage options:

### SQL Server Storage (Default)
In `App.xaml.cs`:
	   Replace `YOUR-SERVER` with your SQL Server instance name (e.g., "DESKTOP-ABC123" or "localhost").

### JSON File Storage
In `App.xaml.cs`:
	IServiceFactory serviceFactory = JsonServiceFactory.Instance;


## Project Structure

- **UI**: WPF user interface project containing:
  - Bug management interface
  - Category management interface
  - Service implementations

- **API**: Web API project containing:
  - RESTful endpoints for bugs and categories
  - Configuration settings

- **Repositories**: Data access layer containing:
  - SQL Server repositories
  - Data models
  - Repository interfaces

- **Factories**: Service factory implementations for:
  - SQL Server storage
  - JSON file storage

## Troubleshooting

### Database Connection Issues

- Verify SQL Server is running.
- Check SQL Server name in connection string.
- Ensure Windows Authentication is enabled.
- Verify database 'Bugs' exists.
- Check all tables are created correctly.

### API Connection Issues

- Verify API project is running.
- Check API URL in service configuration.
- Ensure ports are not blocked by firewall.
- Verify no other application is using the same port.
- Make sure both API and UI projects are set to start.

### Data Issues

- Check create_tables.sql script executed successfully.
- Verify initial data was inserted.
- Check foreign key relationships.
- Ensure `CategoryID` values match existing categories.

## Features

- Hierarchical category management
- Bug tracking with status
- Category-based bug organization
- Persistent data storage
- Easy switching between storage types
- Two storage options (SQL Server and JSON)
- API and UI separation for scalability

For any additional issues or questions, please create an issue in the repository.
