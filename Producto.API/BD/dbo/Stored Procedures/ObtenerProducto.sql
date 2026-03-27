-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE ObtenerProducto
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		P.Id,
		P.Nombre,
		P.Descripcion,
		P.Precio,
		P.Stock,
		P.CodigoBarras,
		SC.Nombre AS SubCategoria,
		C.Nombre AS Categoria
	FROM Producto P
	INNER JOIN SubCategorias SC ON P.IdSubCategoria = SC.Id
	INNER JOIN Categorias C ON SC.IdCategoria = C.Id
	WHERE P.Id = @Id
END