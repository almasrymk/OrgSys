-----------------------------------
-- (Parameters)
-- 1- {0} FromDate
-- 2- {1} ProductId
-- 3- {2} ShiftId
-- 4- {3} BranchId
-- 5- {4} CreateUserId
-----------------------------------

SELECT 
ProductId ,  
ProductName ,
SUM(Amount) Amount FROM
(
	SELECT 
	invp.ProductId ,  
	pr.[Name] ProductName ,
	SUM(invp.Net * invt.InOut) Amount 

	FROM org.Invoice inv 
	INNER JOIN Org.InvoiceProduct invp ON invp.InvoiceId = inv.Id
	INNER JOIN org.InvoiceType invt ON invt.Id = inv.TypeId
	INNER JOIN org.Product pr ON pr.Id = inv.ProductId

	WHERE	
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
	(Convert(bigint, N'{1}') = 0 OR pr.Id = Convert(bigint, N'{1}')) AND 
	(Convert(bigint, N'{2}') = 0 OR inv.ShiftId = Convert(bigint, N'{2}')) AND
	(Convert(bigint, N'{3}') = 0 OR inv.BranchId = Convert(bigint, N'{3}')) AND
	(Convert(bigint, N'{4}') = 0 OR inv.CreateUserId = N'{4}')

	GROUP BY invp.ProductId , pr.[Name] 

) TB 

GROUP BY DealerId , DealerName