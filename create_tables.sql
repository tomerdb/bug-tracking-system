-- Create BugsCategory table
CREATE TABLE [dbo].[BugsCategory] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [CategoryName]     NVARCHAR (100) NOT NULL,
    [ParentCategoryId] INT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

-- Create Bugs table
CREATE TABLE [dbo].[Bugs] (
    [BugID]       INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Status]      NVARCHAR (50)  NOT NULL,
    [CategoryID]  INT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([BugID] ASC),
    FOREIGN KEY (CategoryID) REFERENCES [dbo].[BugsCategory](ID)
);

-- Insert initial data into BugsCategory table
INSERT INTO [dbo].[BugsCategory] (CategoryName, ParentCategoryId) VALUES ('UI', NULL);
INSERT INTO [dbo].[BugsCategory] (CategoryName, ParentCategoryId) VALUES ('Backend', NULL);
INSERT INTO [dbo].[BugsCategory] (CategoryName, ParentCategoryId) VALUES ('Database', NULL);

-- Insert initial data into Bugs table
INSERT INTO [dbo].[Bugs] (Title, Description, Status, CategoryID) VALUES ('Button not working', 'The submit button does not work on the login page.', 'Open', 1);
INSERT INTO [dbo].[Bugs] (Title, Description, Status, CategoryID) VALUES ('API error', 'The API returns a 500 error when fetching user data.', 'Open', 2);
INSERT INTO [dbo].[Bugs] (Title, Description, Status, CategoryID) VALUES ('Database connection issue', 'The application cannot connect to the database.', 'Open', 3);