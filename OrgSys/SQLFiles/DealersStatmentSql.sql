SELECT 

*,
SUM(Amount) OVER(PARTITION BY tb.DealerId ORDER BY tb.DealerId , tb.Type  , tb.Date desc ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS Balance

FROM (

    ---------- Get Openning Balance ----------

	SELECT 
	0 Type,
	NULL Id , 
	NULL Code ,
	NULL TypeId , 
	N'Openning Balance' TypeName , 
	NULL [Date] ,
	DealerId , 
	DealerName ,
	SUM(Amount) Amount ,
	CASE WHEN SUM(Amount) > 0 THEN 1 ELSE -1 END InOut

	FROM (
		SELECT 
		inv.DealerId , 
		dr.[Name] DealerName , 
		inv.Net * invt.InOut Amount 

		FROM Org.Invoice inv
		INNER JOIN Org.InvoiceType invt ON invt.Id = inv.TypeId
		INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

		WHERE 
		dr.TypeId = Convert(bigint, N'{0}') AND 
		CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
		(Convert(bigint, N'{3}') = 0 OR dr.Id = Convert(bigint, N'{3}')) AND 
		(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
		(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
		(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')

		UNION ALL

		SELECT 
		inv.DealerId , 
		dr.[Name] DealerName , 
		inv.Amount * invt.InOut Amount

		FROM Org.Financial inv
		INNER JOIN Org.FinancialType invt ON invt.Id = inv.TypeId
		INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

		WHERE 
		dr.TypeId = Convert(bigint, N'{0}') AND 
		CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
		(Convert(bigint, N'{3}') = 0 OR dr.Id = Convert(bigint, N'{3}')) AND 
		(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
		(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
		(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')

	) OpenningBalance 
	
	GROUP BY 
	DealerId , 
	DealerName

	UNION ALL

	---------- Get All Invoices ----------

	SELECT 
	1 Type ,
	inv.Id , 
	Inv.Code ,
	inv.TypeId , 
	invt.[Group] + ' ' + invt.[Name] TypeName , 
	inv.[Date] ,
	inv.DealerId , 
	dr.[Name] DealerName , 
	inv.Net Amount ,
	invt.InOut
	FROM Org.Invoice inv
	INNER JOIN Org.InvoiceType invt ON invt.Id = inv.TypeId
	INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

	WHERE 
	dr.TypeId = Convert(bigint, N'{0}') AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{2}',111)) AND 
	(Convert(bigint, N'{3}') = 0 OR dr.Id = Convert(bigint, N'{3}')) AND 
	(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
	(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')

	UNION ALL

	---------- Get All Financial ----------

	SELECT 
	1 Type ,
	inv.Id , 
	Inv.Code ,
	inv.TypeId , 
	invt.[Name] TypeName , 
	inv.[Date] ,
	inv.DealerId , 
	dr.[Name] DealerName , 
	inv.Amount Amount ,
	invt.InOut

	FROM Org.Financial inv

	INNER JOIN Org.FinancialType invt ON invt.Id = inv.TypeId
	INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

	WHERE 
	dr.TypeId = Convert(bigint, N'{0}') AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{2}',111)) AND 
	(Convert(bigint, N'{3}') = 0 OR dr.Id = Convert(bigint, N'{3}')) AND 
	(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
	(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')

) AS TB 

ORDER BY 
tb.DealerId , 
tb.Type  , 
tb.Date desc