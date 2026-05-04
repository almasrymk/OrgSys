-----------------------------------
-- (Parameters)
-- 1- {0} ClassificationId
-----------------------------------

SELECT 
p.Id,
p.Id ItemId,
p.[Name] ItemName, 
p.Barcode BarCode , 
p.Code ItemCode , 
p.Cost PurchasePrice,
p.Price SalesPrice,
p.ClassificationId,
p.ImgPath,
cl.[Name] ClassificationName

FROM Org.Product p
INNER JOIN Org.[Classification] cl on cl.Id = p.ClassificationId

WHERE 

'{0}' = 0 OR  p.ClassificationId = '{0}'

