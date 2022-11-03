-----------------------------------
-- (Parameters)
-- 1- {0} FromDate
-- 2- {1} ToDate
-- 3- {2} ProductId
-- 4- {3} ShiftId
-- 5- {4} BranchId
-- 6- {5} CreateUserId
-----------------------------------

SELECT 

ROW_NUMBER() OVER(ORDER BY   DealerId , [type] , ReferenceId ,  OpenningBalance ASC) AS Id,
*,
SUM(Amount * InOut) OVER(PARTITION BY tb.DealerId ORDER BY tb.DealerId , tb.OpenningBalance  DESC, tb.Date ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS Balance

FROM (

    ---------- Get Openning Balance ----------

	SELECT 
	1 OpenningBalance,
	0 Type,
	NULL ReferenceId, 
	NULL Code ,
	1 TypeId , 
	N'Openning Balance' TypeName , 
	CONVERT(datetime , CONVERT(VARCHAR(20),N'1-1-2012',111)) [Date] ,
	ProductId , 
	ProductName ,
	SUM(Amount) Amount ,
	CASE WHEN SUM(Amount) > 0 THEN 1 ELSE -1 END InOut

	FROM (
		SELECT 
		invp.ProductId , 
		pr.[Name] ProductName , 
		invp.Net * invt.InOut Amount 

		FROM Org.Invoice inv
	    INNER JOIN Org.InvoiceProduct invp ON invp.InvoiceId = inv.Id
		INNER JOIN Org.InvoiceType invt ON invt.Id = inv.TypeId
		INNER JOIN Org.Product pr ON pr.Id = invp.ProductId

		WHERE 		
		CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
		(Convert(bigint, N'{2}') = 0 OR pr.Id = Convert(bigint, N'{2}')) AND 
		(Convert(bigint, N'{3}') = 0 OR inv.ShiftId = Convert(bigint, N'{3}')) AND
		(Convert(bigint, N'{4}') = 0 OR inv.BranchId = Convert(bigint, N'{4}')) AND
		(Convert(bigint, N'{5}') = 0 OR inv.CreateUserId = N'{5}')

	) OpenningBalance 
	
	GROUP BY 
	DealerId , 
	DealerName

	UNION ALL

	---------- Get All Invoices ----------

	SELECT 
	0 OpenningBalance,
	1 Type ,
	inv.Id ReferenceId, 
	Inv.Code ,
	inv.TypeId , 
	invt.[Group] + ' ' + invt.[Name] TypeName , 
	inv.[Date] ,
	invp.ProductId , 
	pr.[Name] ProductName , 
	invp.Net Amount ,
	1
	FROM Org.Invoice inv
	INNER JOIN Org.InvoiceProduct invp ON invp.InvoiceId = inv.Id
	INNER JOIN Org.InvoiceType invt ON invt.Id = inv.TypeId
	INNER JOIN Org.Product pr ON pr.Id = invp.ProductId

	WHERE 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	(Convert(bigint, N'{2}') = 0 OR pr.Id = Convert(bigint, N'{2}')) AND 
	(Convert(bigint, N'{3}') = 0 OR inv.ShiftId = Convert(bigint, N'{3}')) AND
	(Convert(bigint, N'{4}') = 0 OR inv.BranchId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR inv.CreateUserId = N'{5}')

) AS TB 
--ORDER BY DealerId , OpenningBalance , [Date]