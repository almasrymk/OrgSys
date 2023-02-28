-----------------------------------
-- (Parameters)
-- 1- {0} TypeId
-- 2- {1} FromDate
-- 3- {2} DealerId
-- 4- {3} ShiftId
-- 5- {4} BranchId
-- 6- {5} CreateUserId
-----------------------------------

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

SELECT 
DealerId ,  
DealerName ,
SUM(Amount) Amount FROM
(
SELECT 
inv.DealerId ,  
dl.[Name] DealerName ,
SUM(inv.Net * invt.InOut) Amount 

FROM org.Invoice inv 

INNER JOIN org.InvoiceType invt ON invt.Id = inv.TypeId
INNER JOIN org.Dealer dl ON dl.Id = inv.DealerId

WHERE
dl.TypeId = Convert(bigint, N'{0}') AND 
CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
(Convert(bigint, N'{2}') = 0 OR dl.Id = Convert(bigint, N'{2}')) AND 
(Convert(bigint, N'{3}') = 0 OR inv.ShiftId = Convert(bigint, N'{3}')) AND
(Convert(bigint, N'{4}') = 0 OR inv.BranchId = Convert(bigint, N'{4}')) AND
(Convert(bigint, N'{5}') = 0 OR inv.CreateUserId = N'{5}')

GROUP BY inv.DealerId , dl.[Name] 
UNION ALL
SELECT 
fin.DealerId ,  
dl.[Name] DealerName ,
ABS(SUM(fin.Amount * fint.InOut)) * -1 Amount 

FROM org.Financial fin

INNER JOIN org.FinancialType fint ON fint.Id = fin.TypeId
INNER JOIN org.Dealer dl ON dl.Id = fin.DealerId

WHERE
dl.TypeId = Convert(bigint, N'{0}') AND 
CONVERT(datetime , CONVERT(VARCHAR(20),fin.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
(Convert(bigint, N'{2}') = 0 OR dl.Id = Convert(bigint, N'{2}')) AND 
(Convert(bigint, N'{3}') = 0 OR fin.ShiftId = Convert(bigint, N'{3}')) AND
(Convert(bigint, N'{4}') = 0 OR fin.BranchId = Convert(bigint, N'{4}')) AND
(Convert(bigint, N'{5}') = 0 OR fin.CreateUserId = N'{5}')

GROUP BY fin.DealerId , dl.[Name] 
) TB 

GROUP BY DealerId , DealerName