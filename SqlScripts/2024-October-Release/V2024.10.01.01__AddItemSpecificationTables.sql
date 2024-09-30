BEGIN TRANSACTION

SET NOCOUNT ON

BEGIN TRY


	PRINT CONCAT('Adding seperate tables for unique items and initializing with current default item data', GetDate())
	PRINT ''

	PRINT 'Creating Ingredients, Tools, and Equipment Tables...'
        
    CREATE TABLE Ingredients
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR(50),
        Cost DECIMAL,
        Quantity DECIMAL,
        UnitOfMeasurement NVARCHAR(50),
        PRIMARY KEY (Id)
    );
    CREATE TABLE Tools
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR(50),
        Cost DECIMAL,
        Quantity DECIMAL,
        PRIMARY KEY (Id)
    );
    CREATE TABLE Equipment
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR(50),
        Cost DECIMAL,
        Quantity DECIMAL,
        PRIMARY KEY (Id)
    );

    PRINT 'Populating Ingredients, Tools, and Equipment Tables...'

    INSERT INTO Ingredients
    SELECT i.Id, i.Name, i.Cost, i.Quantity, i.UnitOfMeasurement
    FROM Items i
    JOIN ItemType ON i.ItemTypeId = ItemType.Id
    WHERE ItemType.Name = 'Ingredient';

    INSERT INTO Tools
    SELECT i.Id, i.Name, i.Cost, i.Quantity
    FROM Items i
    JOIN ItemType ON i.ItemTypeId = ItemType.Id
    WHERE ItemType.Name = 'Tool';

    INSERT INTO Equipment
    SELECT i.Id, i.Name, i.Cost, i.Quantity
    FROM Items i
    JOIN ItemType ON i.ItemTypeId = ItemType.Id
    WHERE ItemType.Name = 'Equipment';

    PRINT 'Removing Items table after data is retriveved...'

    DROP TABLE IF EXISTS Items;
    DROP TABLE IF EXISTS ItemType;

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






