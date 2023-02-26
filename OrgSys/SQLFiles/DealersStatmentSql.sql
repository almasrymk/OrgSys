-----------------------------------
-- (Parameters)
-- 1- {0} TypeId
-- 2- {1} FromDate
-- 3- {2} ToDate
-- 4- {3} DealerId
-- 5- {4} ShiftId
-- 6- {5} BranchId
-- 7- {6} CreateUserId
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
	DealerId , 
	DealerName ,
	DealerImgPath ,
	SUM(Amount) Amount ,
	CASE WHEN SUM(Amount) > 0 THEN 1 ELSE -1 END InOut

	FROM (
		SELECT 
		inv.DealerId , 
		dr.[Name] DealerName , 
		dr.ImgPath DealerImgPath , 
		inv.Net * invt.InOut Amount 

		FROM Org.Invoice inv
		INNER JOIN Org.InvoiceType invt ON invt.Id = inv.TypeId
		INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

		WHERE 
		(inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 1 ELSE 2 END OR inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 3 ELSE 4 END)AND
		--dr.TypeId = Convert(bigint, N'{0}') AND 
		CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
		(dr.Id = Convert(bigint, N'{3}')) AND 
		(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
		(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
		(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')

		UNION ALL

		SELECT 
		inv.DealerId , 
		dr.[Name] DealerName , 
	    dr.ImgPath DealerImgPath , 
		inv.Amount * invt.InOut * -1 Amount

		FROM Org.Financial inv
		INNER JOIN Org.FinancialType invt ON invt.Id = inv.TypeId
		INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

		WHERE 
		(inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 1 ELSE 2 END OR inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 3 ELSE 4 END)AND
		--dr.TypeId = Convert(bigint, N'{0}') AND 
		CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
		(dr.Id = Convert(bigint, N'{3}')) AND 
		(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
		(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
		(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')

	) OpenningBalance 
	
	GROUP BY 
	DealerId , 
	DealerName,
	DealerImgPath

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
    dr.ImgPath DealerImgPath , 
	inv.Net Amount ,
	1
	FROM Org.Invoice inv
	INNER JOIN Org.InvoiceType invt ON invt.Id = inv.TypeId
	INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

	WHERE 
	(inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 1 ELSE 2 END OR inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 3 ELSE 4 END)AND
		--dr.TypeId = Convert(bigint, N'{0}') AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{2}',111)) AND 
	(Convert(bigint, N'{3}') = 0 OR dr.Id = Convert(bigint, N'{3}')) AND 
	(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
	(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')

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
	dr.ImgPath DealerImgPath , 
	inv.Amount Amount ,
	-1 InOut

	FROM Org.Financial inv

	INNER JOIN Org.FinancialType invt ON invt.Id = inv.TypeId
	INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

	WHERE 
	(inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 1 ELSE 2 END OR inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 3 ELSE 4 END)AND
		--dr.TypeId = Convert(bigint, N'{0}') AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{2}',111)) AND 
	(Convert(bigint, N'{3}') = 0 OR dr.Id = Convert(bigint, N'{3}')) AND 
	(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
	(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')

) AS TB 
--ORDER BY DealerId , OpenningBalance , [Date]