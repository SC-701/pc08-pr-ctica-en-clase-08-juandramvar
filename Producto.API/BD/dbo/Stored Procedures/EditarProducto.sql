-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE EditarProducto
	@Id UNIQUEIDENTIFIER,
	@IdSubCategoria UNIQUEIDENTIFIER,
	@Nombre VARCHAR(MAX),
	@Descripcion VARCHAR(MAX),
	@Precio DECIMAL(18,2),
	@Stock INT,
	@CodigoBarras VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION
		UPDATE Producto
		SET
			IdSubCategoria = @IdSubCategoria,
			Nombre = @Nombre,
			Descripcion = @Descripcion,
			Precio = @Precio,
			Stock = @Stock,
			CodigoBarras = @CodigoBarras
		WHERE Id = @Id

		SELECT @Id
	COMMIT TRANSACTION
END