BEGIN TRANSACTION

SET NOCOUNT ON

BEGIN TRY


	PRINT CONCAT('Adding seperate tables for unique items and initializing with current default item data', GetDate())
	PRINT ''

	PRINT 'Removing Quantity From Ingredients Table...'
        
    ALTER TABLE Ingredients DROP COLUMN Quantity

    PRINT 'Add Units Of Measurement To KitchenIngredients Table...'

    ALTER TABLE RecipeIngredients ADD UnitOfMeasurement NVARCHAR(50)

    PRINT 'Renaming Columns For Specification and Convention...'

    EXEC sp_rename 'KitchenIngredients.ItemId', 'IngredientId', 'COLUMN'

    EXEC sp_rename 'KitchenToolsAndEquipment.ItemId', 'ToolsAndEquipmentId', 'COLUMN'

    EXEC sp_rename 'RecipeIngredients.ItemId', 'IngredientId', 'COLUMN'

    EXEC sp_rename 'RecipeToolsAndEquipment.ItemId', 'ToolsAndEquipmentId', 'COLUMN'

    PRINT 'Populating the Unit Of Measurement Column in KitchenIngredients Table...'

    UPDATE ri
    SET ri.UnitOfMeasurement = i.UnitOfMeasurement
    FROM RecipeIngredients ri
    JOIN Ingredients i ON ri.IngredientId = i.Id

    PRINT 'Updateing Foreign Key Relationships Between All Tables...'

    ALTER TABLE KitchenIngredients
    WITH CHECK ADD CONSTRAINT FK_KitchenIngredients_KitchenType
    FOREIGN KEY(KitchenId) REFERENCES KitchenType (Id)

    ALTER TABLE KitchenIngredients CHECK CONSTRAINT FK_KitchenIngredients_KitchenType

    ALTER TABLE KitchenToolsAndEquipment
    WITH CHECK ADD CONSTRAINT FK_KitchenToolsAndEquipment_KitchenType
    FOREIGN KEY(KitchenId) REFERENCES KitchenType (Id)

    ALTER TABLE KitchenToolsAndEquipment CHECK CONSTRAINT FK_KitchenToolsAndEquipment_KitchenType

    ALTER TABLE RecipeIngredients
    WITH CHECK ADD CONSTRAINT FK_RecipeIngredients_Ingredients
    FOREIGN KEY(IngredientId) REFERENCES Ingredients (Id)

    ALTER TABLE RecipeIngredients CHECK CONSTRAINT FK_RecipeIngredients_Ingredients

    ALTER TABLE RecipeIngredients
    WITH CHECK ADD CONSTRAINT FK_RecipeIngredients_Recipes
    FOREIGN KEY(RecipeId) REFERENCES Recipes (Id)

    ALTER TABLE RecipeIngredients CHECK CONSTRAINT FK_RecipeIngredients_Recipes

    ALTER TABLE RecipeToolsAndEquipment
    WITH CHECK ADD CONSTRAINT FK_RecipeToolsAndEquipment_Recipes
    FOREIGN KEY(RecipeId) REFERENCES Recipes (Id)

    ALTER TABLE RecipeToolsAndEquipment CHECK CONSTRAINT FK_RecipeToolsAndEquipment_Recipes

    ALTER TABLE RecipeToolsAndEquipment
    WITH CHECK ADD CONSTRAINT FK_RecipeToolsAndEquipment_ToolsAndEquipment
    FOREIGN KEY(ToolsAndEquipmentID) REFERENCES ToolsAndEquipment (Id)

    ALTER TABLE RecipeToolsAndEquipment CHECK CONSTRAINT FK_RecipeToolsAndEquipment_ToolsAndEquipment

    ALTER TABLE KitchenIngredients
    WITH CHECK ADD CONSTRAINT FK_KitchenIngredients_Ingredients
    FOREIGN KEY(IngredientId) REFERENCES Ingredients (Id)

    ALTER TABLE KitchenIngredients CHECK CONSTRAINT FK_KitchenIngredients_Ingredients

    ALTER TABLE KitchenToolsAndEquipment
    WITH CHECK ADD CONSTRAINT FK_KitchenToolsAndEquipment_ToolsAndEquipment
    FOREIGN KEY(ToolsAndEquipmentId) REFERENCES ToolsAndEquipment (Id)

    ALTER TABLE KitchenToolsAndEquipment CHECK CONSTRAINT FK_KitchenToolsAndEquipment_ToolsAndEquipment


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






