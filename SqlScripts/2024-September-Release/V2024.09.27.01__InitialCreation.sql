BEGIN TRANSACTION

SET NOCOUNT ON

BEGIN TRY


	PRINT CONCAT('Creating and initializing recipe manager tables with standard data', GetDate())
	PRINT ''

	PRINT 'Creating Recipe Manager Tables...'
        USE RecipeManager
        GO
        CREATE TABLE Entity
        (
            Id UNIQUEIDENTIFIER NOT NULL,
            Name NVARCHAR(50),
            Age INT,
            CookingSkill INT,
            KitchenTypeId INT,
            PRIMARY KEY (Id)
        );
        CREATE TABLE KitchenType
        (
            Id UNIQUEIDENTIFIER NOT NULL,
            Name NVARCHAR(50),
            PRIMARY KEY (Id)
        );
        CREATE TABLE Items
        (
            Id UNIQUEIDENTIFIER NOT NULL,
            ItemTypeId UNIQUEIDENTIFIER,
            Name NVARCHAR(50),
            Cost decimal,
            Quantity decimal,
            UnitOfMeasurement NVARCHAR(50),
            PRIMARY KEY (Id),
        );
        CREATE TABLE KitchenItems
        (
            AutoId UNIQUEIDENTIFIER,
            KitchenId UNIQUEIDENTIFIER,
            ItemId UNIQUEIDENTIFIER,
            Quantity decimal,
            PRIMARY KEY (AutoId)
        );
        CREATE TABLE Recipes
        (
            Id UNIQUEIDENTIFIER NOT NULL,
            Name NVARCHAR(50),
            Difficulty INT,
            Time decimal,
            Cost decimal,
            PRIMARY KEY (Id)
        )
        CREATE TABLE RecipeItems
        (
            AutoId UNIQUEIDENTIFIER,
            RecipeId UNIQUEIDENTIFIER NOT NULL,
            ItemId UNIQUEIDENTIFIER,
            Quantity decimal,
            PRIMARY KEY (AutoId)
        )
        CREATE TABLE ItemType
        (
            Id UNIQUEIDENTIFIER NOT NULL,
            Name NVARCHAR(50),
            PRIMARY KEY (Id)
        );

        PRINT 'Prepopulating Recipe Manager Tables...'

        INSERT INTO Entity
        VALUES
        (NEWID(), 'Ryan', 25, 6, 1)
        DECLARE @BareKitchen UNIQUEIDENTIFIER,
                @CurrentKitchen UNIQUEIDENTIFIER,
                @DreamKitchen UNIQUEIDENTIFIER;
        SET @BareKitchen = NEWID()
        SET @CurrentKitchen = NEWID()
        SET @DreamKitchen = NEWID()
        INSERT INTO KitchenType
        VALUES
        (@BareKitchen, 'bare'),
        (@CurrentKitchen, 'current'),
        (@DreamKitchen, 'dream');
        DECLARE @Ingredient UNIQUEIDENTIFIER,
                @Equipment UNIQUEIDENTIFIER,
                @Tool UNIQUEIDENTIFIER;
        SET @Ingredient = NEWID()
        SET @Equipment = NEWID()
        SET @Tool = NEWID()
        INSERT INTO ItemType
        VALUES
        (@Ingredient, 'Ingredient'),
        (@Equipment, 'Equipment'),
        (@Tool, 'Tool');
        DECLARE @smallpot UNIQUEIDENTIFIER,
                @smallpan UNIQUEIDENTIFIER,
                @spatula UNIQUEIDENTIFIER,
                @groundbeef UNIQUEIDENTIFIER,
                @bun UNIQUEIDENTIFIER,
                @cheese UNIQUEIDENTIFIER,
                @stove UNIQUEIDENTIFIER,
                @fridge UNIQUEIDENTIFIER;
        SET @smallpot = NEWID()
        SET @smallpan = NEWID()
        SET @spatula = NEWID()
        SET @groundbeef = NEWID()
        SET @bun = NEWID()
        SET @cheese = NEWID()
        SET @stove = NEWID()
        SET @fridge = NEWID()
        INSERT INTO Items
        VALUES
        (@smallpot, @Tool, 'Small Pot', 3.99, 1, 'Count'),
        (@smallpan, @Tool, 'Small Pan', 9.99, 1, 'Count'),
        (@spatula, @Tool, 'Spatula', 4.99, 1, 'Count'),
        (@groundbeef, @Ingredient, 'Ground Beef', 5.99, 2, 'lbs'),
        (@bun, @Ingredient, 'Bun', 3.99, 8, 'Count'),
        (@cheese, @Ingredient, 'Cheese', 2.99, 5, 'Slices'),
        (@stove, @Equipment, 'Stove', 399.99, 1, 'Count'),
        (@fridge, @Equipment, 'Refridgerator', 399.99, 1, 'Count');
        INSERT INTO KitchenItems
        VALUES
        (NEWID(), @BareKitchen, @smallpot, 1),
        (NEWID(), @BareKitchen, @smallpan, 1),
        (NEWID(), @BareKitchen, @spatula, 2),
        (NEWID(), @BareKitchen, @groundbeef, 2),
        (NEWID(), @BareKitchen, @bun, 4),
        (NEWID(), @BareKitchen, @cheese, 4),
        (NEWID(), @BareKitchen, @stove, 1),
        (NEWID(), @BareKitchen, @fridge, 1);
        DECLARE @cheeseburgerRecipe UNIQUEIDENTIFIER
        SET @cheeseburgerRecipe = NEWID()
        INSERT INTO Recipes
        VALUES
        (@cheeseburgerRecipe, 'Cheeseburger', 3, 20, 12);
        INSERT INTO RecipeItems
        VALUES
        (NEWID(), @cheeseburgerRecipe, @smallpan, 1),
        (NEWID(), @cheeseburgerRecipe, @spatula, 1),
        (NEWID(), @cheeseburgerRecipe, @groundbeef, .5),
        (NEWID(), @cheeseburgerRecipe, @bun, 1),
        (NEWID(), @cheeseburgerRecipe, @cheese, 1),
        (NEWID(), @cheeseburgerRecipe, @stove, 1)

PRINT '.... DONE'
	PRINT ''

	COMMIT TRANSACTION

	PRINT CONCAT('Scripts Completed Successfully: ', GetDate())

END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION

	PRINT ''
	PRINT CONCAT('An Error Occurred: ', GetDate()) 
	PRINT CONCAT(ERROR_NUMBER(),' - ', ERROR_MESSAGE())

END CATCH

SET NOCOUNT OFF






