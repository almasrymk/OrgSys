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
TB.DealerId , 
TB.DealerCode ,
TB.DealerName ,
TB.DealerImgPath , 
SUM(TB.OpenningBalance) OpenningBalance,
SUM(TB.Balance) Balance,
SUM(TB.TotalInvoice) TotalInvoice ,
SUM(TB.TotalReturnInvoice) TotalReturnInvoice,
SUM(TB.TotalInvoice) - SUM(TB.TotalReturnInvoice) TotalNetInvoice,
SUM(TB.TotalCreditInvoice) TotalCreditInvoice,
SUM(TB.TotalPaidInvoice) TotalPaidInvoice
FROM (
SELECT 

DealerId , 
DealerCode ,
DealerName ,
DealerImgPath ,
Amount OpenningBalance ,
0 Balance,
0 TotalInvoice,
0 TotalReturnInvoice,
0 TotalCreditInvoice,
0 TotalPaidInvoice
FROM (
	SELECT 
	inv.DealerId , 
	dr.Code DealerCode ,
	dr.[Name] DealerName , 
	dr.ImgPath DealerImgPath , 
	inv.Net * invt.InOut * inv.Rate Amount 

	FROM Org.Invoice inv
	INNER JOIN Org.InvoiceType invt ON invt.Id = inv.TypeId
	INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

	WHERE 
	(inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 1 ELSE 2 END OR inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 3 ELSE 4 END)AND
	--dr.TypeId = Convert(bigint, N'{0}') AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	(Convert(bigint, N'{2}') = 0 OR dr.Id = Convert(bigint, N'{2}')) AND 
	(Convert(bigint, N'{3}') = 0 OR inv.ShiftId = Convert(bigint, N'{3}')) AND
	(Convert(bigint, N'{4}') = 0 OR inv.BranchId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR inv.CreateUserId = N'{5}')

	UNION ALL

	SELECT 
	inv.DealerId , 
	dr.Code DealerCode ,
	dr.[Name] DealerName , 
	dr.ImgPath DealerImgPath , 
	(inv.Amount * invt.InOut * inv.Rate) * -1 Amount

	FROM Org.Financial inv
	INNER JOIN Org.FinancialType invt ON invt.Id = inv.TypeId
	INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

	WHERE 
	(inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 1 ELSE 2 END OR inv.TypeId =  CASE WHEN Convert(bigint, N'{0}') = 1 THEN 3 ELSE 4 END)AND
	--dr.TypeId = Convert(bigint, N'{0}') AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	(Convert(bigint, N'{2}') = 0 OR dr.Id = Convert(bigint, N'{2}')) AND 
	(Convert(bigint, N'{3}') = 0 OR inv.ShiftId = Convert(bigint, N'{3}')) AND
	(Convert(bigint, N'{4}') = 0 OR inv.BranchId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR inv.CreateUserId = N'{5}')

) OpenningBalance 

UNION ALL

SELECT 
inv.DealerId ,  
dl.Code DealerCode ,
dl.[Name] DealerName ,
dl.ImgPath DealerImgPath,
0 OpenningBalance ,
inv.Net  * invt.InOut * inv.Rate Balance ,
CASE WHEN invt.InOut = 1 THEN inv.Net * inv.Rate ELSE 0 END TotalInvoice ,
CASE WHEN invt.InOut = -1 THEN inv.Net * inv.Rate ELSE 0 END TotalReturnInvoice,
inv.Credit * invt.InOut * inv.Rate TotalCreditInvoice,
inv.Paid * invt.InOut * inv.Rate TotalPaidInvoice

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

UNION ALL

SELECT 
fin.DealerId ,  
dl.Code DealerCode ,
dl.[Name] DealerName ,
dl.ImgPath DealerImgPath,
0 OpenningBalance ,
ABS(fin.Amount * fint.InOut * fin.Rate) * -1 Balance ,
0 TotalInvoice,
0 TotalReturnInvoice,
0 TotalCreditInvoice,
0 TotalPaidInvoice

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

) TB 

GROUP BY TB.DealerId , TB.DealerCode , TB.DealerName , TB.DealerImgPath