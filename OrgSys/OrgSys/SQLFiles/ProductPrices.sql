-----------------------------------
-- (Parameters)
-- 1- {0} TypeId
-- 2- {1} FromDate
-- 3- {2} DealerId
-- 4- {3} ShiftId
-- 5- {4} BranchId
-- 6- {5} ProductId
-- 7- {6} CreateUserId
-----------------------------------

SELECT 

invp.ProductId ,
pro.Name ProductName,
pro.Nickname ProductNickName,
pro.Code ProductCode,
pro.Barcode ProductBarcode,
pro.Nickname ProductNickName,
MIN(invp.Price) MINPrice,
MAX(invp.Price) MAXPice,
SUM(invp.Total) / SUM(invp.Quantity) AVGPrice

FROM Org.InvoiceProduct invp
INNER JOIN Org.Invoice inv ON inv.Id = invp.InvoiceId
INNER JOIN Org.Product pro ON pro.Id = invp.ProductId

WHERE
dr.TypeId = Convert(bigint, N'{0}') AND 
CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
(Convert(bigint, N'{3}') = 0 OR inv.DealerId = Convert(bigint, N'{3}')) AND 
(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
(Convert(bigint, N'{6}') = 0 OR invp.ProductId = Convert(bigint, N'{6}')) AND
(Convert(bigint, N'{7}') = 0 OR inv.CreateUserId = N'{7}')

GROUP BY 

	invp.ProductId,
	pro.Name ,
	pro.Nickname,
	pro.Code,
	pro.Barcode,
	pro.Nickname