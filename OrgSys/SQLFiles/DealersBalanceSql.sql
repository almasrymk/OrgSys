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
CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
(Convert(bigint, N'{1}') = 0 OR dl.Id = Convert(bigint, N'{1}')) AND 
(Convert(bigint, N'{2}') = 0 OR inv.ShiftId = Convert(bigint, N'{2}')) AND
(Convert(bigint, N'{3}') = 0 OR inv.BranchId = Convert(bigint, N'{3}')) AND
(Convert(bigint, N'{4}') = 0 OR inv.CreateUserId = N'{4}')

GROUP BY inv.DealerId , dl.[Name] 
UNION ALL
SELECT 
fin.DealerId ,  
dl.[Name] DealerName ,
SUM(fin.Amount * fint.InOut) Amount 

FROM org.Financial fin

INNER JOIN org.FinancialType fint ON fint.Id = fin.TypeId
INNER JOIN org.Dealer dl ON dl.Id = fin.DealerId

WHERE
CONVERT(datetime , CONVERT(VARCHAR(20),fin.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
(Convert(bigint, N'{1}') = 0 OR dl.Id = Convert(bigint, N'{1}')) AND 
(Convert(bigint, N'{2}') = 0 OR fin.ShiftId = Convert(bigint, N'{2}')) AND
(Convert(bigint, N'{3}') = 0 OR fin.BranchId = Convert(bigint, N'{3}')) AND
(Convert(bigint, N'{4}') = 0 OR fin.CreateUserId = N'{4}')

GROUP BY fin.DealerId , dl.[Name] 
) TB 

GROUP BY DealerId , DealerName