BEGIN TRANSACTION

SET NOCOUNT ON

BEGIN TRY


	PRINT CONCAT('Adding seperate tables for unique items and initializing with current default item data', GetDate())
	PRINT ''

	PRINT 'Creating Tools and Equipment Combined Tables and Seperate Kitchen/Recipe Item Tables...'
        
    CREATE TABLE ToolsAndEquipment
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR(50),
        Cost DECIMAL,
        Quantity DECIMAL,
        PRIMARY KEY (Id)
    );
    CREATE TABLE RecipeIngredients
    (
        AutoId UNIQUEIDENTIFIER NOT NULL,
        RecipeId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        Quantity DECIMAL,
        PRIMARY KEY (AutoId)
    );
    CREATE TABLE RecipeToolsAndEquipment
    (
        AutoId UNIQUEIDENTIFIER NOT NULL,
        RecipeId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        Quantity DECIMAL,
        PRIMARY KEY (AutoId)
    );
    CREATE TABLE KitchenIngredients
    (
        AutoId UNIQUEIDENTIFIER NOT NULL,
        KitchenId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        Quantity DECIMAL,
        PRIMARY KEY (AutoId)
    );
    CREATE TABLE KitchenToolsAndEquipment
    (
        AutoId UNIQUEIDENTIFIER NOT NULL,
        KitchenId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        Quantity DECIMAL,
        PRIMARY KEY (AutoId)
    );

    PRINT 'Populating Combined Tools and Equipment and Seperate Recipe/Kitchen Tables Using Previous...'

    INSERT INTO ToolsAndEquipment
    SELECT * 
    FROM Tools
    UNION
    SELECT *
    FROM Equipment;

    INSERT INTO RecipeIngredients
    SELECT ri.AutoId, ri.RecipeId, i.Id, ri.Quantity
    FROM RecipeItems ri
    JOIN Ingredients i ON i.Id = ri.ItemId;

    INSERT INTO RecipeToolsAndEquipment
    SELECT ri.AutoId, ri.RecipeId, i.Id, ri.Quantity
    FROM RecipeItems ri
    JOIN Tools i ON i.Id = ri.ItemId

    INSERT INTO RecipeToolsAndEquipment
    SELECT ri.AutoId, ri.RecipeId, i.Id, ri.Quantity
    FROM RecipeItems ri
    JOIN Equipment i ON i.Id = ri.ItemId

    INSERT INTO KitchenIngredients
    SELECT ki.AutoId, ki.KitchenId, i.Id, ki.Quantity
    FROM KitchenItems ki
    JOIN Ingredients i ON i.Id = ki.ItemId

    INSERT INTO KitchenToolsAndEquipment
    SELECT ki.AutoId, ki.KitchenId, i.Id, ki.Quantity
    FROM KitchenItems ki
    JOIN Tools i ON i.Id = ki.ItemId

    INSERT INTO KitchenToolsAndEquipment
    SELECT ki.AutoId, ki.KitchenId, i.Id, ki.Quantity
    FROM KitchenItems ki
    JOIN Equipment i ON i.Id = ki.ItemId

    PRINT 'Deleting Tables RecipeItems, KitchenItems, Tools, Equipment'

    DROP TABLE IF EXISTS RecipeItems
    DROP TABLE IF EXISTS KitchenItems
    DROP TABLE IF EXISTS Tools 
    DROP TABLE IF EXISTS Equipment

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






