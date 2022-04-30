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
	NULL TypeId , 
	N'Openning Balance' TypeName , 
	CONVERT(datetime , CONVERT(VARCHAR(20),N'1-1-2012',111)) [Date] ,
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
		CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/11/30',111)) AND 
		(Convert(bigint, N'0') = 0 OR dr.Id = Convert(bigint, N'0')) AND 
		(Convert(bigint, N'0') = 0 OR inv.ShiftId = Convert(bigint, N'0')) AND
		(Convert(bigint, N'0') = 0 OR inv.BranchId = Convert(bigint, N'0')) AND
		(Convert(bigint, N'0') = 0 OR inv.CreateUserId = N'0')

		UNION ALL

		SELECT 
		inv.DealerId , 
		dr.[Name] DealerName , 
		inv.Amount * invt.InOut * -1 Amount

		FROM org.Financial inv
		INNER JOIN org.FinancialType invt ON invt.Id = inv.TypeId
		INNER JOIN org.Dealer dr ON dr.Id = inv.DealerId

		WHERE 
		dr.TypeId = Convert(bigint, N'1') AND 
		CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/11/30',111)) AND 
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
	0 OpenningBalance,
	1 Type ,
	inv.Id ReferenceId, 
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
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/11/30',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/12/31',111)) AND 
	(Convert(bigint, N'0') = 0 OR dr.Id = Convert(bigint, N'0')) AND 
	(Convert(bigint, N'0') = 0 OR inv.ShiftId = Convert(bigint, N'0')) AND
	(Convert(bigint, N'0') = 0 OR inv.BranchId = Convert(bigint, N'0')) AND
	(Convert(bigint, N'0') = 0 OR inv.CreateUserId = N'0')

	UNION ALL

	---------- Get All Financial ----------

	SELECT 
	0 OpenningBalance,
	2 Type ,
	inv.Id ReferenceId, 
	Inv.Code ,
	inv.TypeId , 
	invt.[Name] TypeName , 
	inv.[Date] ,
	inv.DealerId , 
	dr.[Name] DealerName , 
	inv.Amount Amount ,
	invt.InOut * -1 InOut

	FROM org.Financial inv

	INNER JOIN org.FinancialType invt ON invt.Id = inv.TypeId
	INNER JOIN org.Dealer dr ON dr.Id = inv.DealerId

	WHERE 
	dr.TypeId in (1,2) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/11/30',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'2022/12/31',111)) AND 
	(Convert(bigint, N'0') = 0 OR dr.Id = Convert(bigint, N'0')) AND 
	(Convert(bigint, N'0') = 0 OR inv.ShiftId = Convert(bigint, N'0')) AND
	(Convert(bigint, N'0') = 0 OR inv.BranchId = Convert(bigint, N'0')) AND
	(Convert(bigint, N'0') = 0 OR inv.CreateUserId = N'0')

) AS TB 
--ORDER BY DealerId , OpenningBalance , [Date]