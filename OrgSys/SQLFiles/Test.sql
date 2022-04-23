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

		FROM org.Invoice inv
		INNER JOIN org.InvoiceType invt ON invt.Id = inv.TypeId
		INNER JOIN org.Dealer dr ON dr.Id = inv.DealerId

		WHERE 
		dr.TypeId = Convert(bigint, N'1') AND 
		CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/01/01',111)) AND 
		(Convert(bigint, N'0') = 0 OR dr.Id = Convert(bigint, N'0')) AND 
		(Convert(bigint, N'0') = 0 OR inv.ShiftId = Convert(bigint, N'0')) AND
		(Convert(bigint, N'0') = 0 OR inv.BranchId = Convert(bigint, N'0')) AND
		(Convert(bigint, N'0') = 0 OR inv.CreateUserId = N'0')

		UNION ALL

		SELECT 
		inv.DealerId , 
		dr.[Name] DealerName , 
		inv.Amount * invt.InOut Amount

		FROM org.Financial inv
		INNER JOIN org.FinancialType invt ON invt.Id = inv.TypeId
		INNER JOIN org.Dealer dr ON dr.Id = inv.DealerId

		WHERE 
		dr.TypeId = Convert(bigint, N'1') AND 
		CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/01/01',111)) AND 
		(Convert(bigint, N'0') = 0 OR dr.Id = Convert(bigint, N'0')) AND 
		(Convert(bigint, N'0') = 0 OR inv.ShiftId = Convert(bigint, N'0')) AND
		(Convert(bigint, N'0') = 0 OR inv.BranchId = Convert(bigint, N'0')) AND
		(Convert(bigint, N'0') = 0 OR inv.CreateUserId = N'0')

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
	FROM org.Invoice inv
	INNER JOIN org.InvoiceType invt ON invt.Id = inv.TypeId
	INNER JOIN org.Dealer dr ON dr.Id = inv.DealerId

	WHERE 
	dr.TypeId = Convert(bigint, N'1') AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/01/01',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/12/31',111)) AND 
	(Convert(bigint, N'0') = 0 OR dr.Id = Convert(bigint, N'0')) AND 
	(Convert(bigint, N'0') = 0 OR inv.ShiftId = Convert(bigint, N'0')) AND
	(Convert(bigint, N'0') = 0 OR inv.BranchId = Convert(bigint, N'0')) AND
	(Convert(bigint, N'0') = 0 OR inv.CreateUserId = N'0')

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

	FROM org.Financial inv

	INNER JOIN org.FinancialType invt ON invt.Id = inv.TypeId
	INNER JOIN org.Dealer dr ON dr.Id = inv.DealerId

	WHERE 
	dr.TypeId = Convert(bigint, N'1') AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/01/01',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/12/31',111)) AND 
	(Convert(bigint, N'0') = 0 OR dr.Id = Convert(bigint, N'0')) AND 
	(Convert(bigint, N'0') = 0 OR inv.ShiftId = Convert(bigint, N'0')) AND
	(Convert(bigint, N'0') = 0 OR inv.BranchId = Convert(bigint, N'0')) AND
	(Convert(bigint, N'0') = 0 OR inv.CreateUserId = N'0')

) AS TB 

ORDER BY 
tb.DealerId , 
tb.Type  , 
tb.Date desc