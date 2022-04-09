SELECT * FROM (
SELECT 
inv.Id , 
Inv.Code ,
inv.TypeId , 
invt.[Name] TypeName , 
inv.[Date] ,
inv.DealerId , 
dr.[Name] DealerName , 
inv.Net Amount ,
invt.InOut
FROM Org.Invoice inv
INNER JOIN Org.InvoiceType invt ON invt.Id = inv.TypeId
INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

WHERE 
invt.Id IN (Convert(bigint, '{0}') , Convert(bigint, '{1}')) AND 
inv.Date >= '' AND inv.Date <= '' AND
(Convert(bigint, '{2}') = 0 OR dr.Id = Convert(bigint, '{2}')) AND 
(Convert(bigint, '{2}') = 0 OR inv.ShiftId = Convert(bigint, '{2}')) AND
(Convert(bigint, '{2}') = 0 OR inv.BranchId = Convert(bigint, '{2}')) AND
(Convert(bigint, '{2}') = 0 OR inv.CreateUserId = '')

UNION ALL

SELECT 
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
) AS TB ORDER BY tb.Date desc