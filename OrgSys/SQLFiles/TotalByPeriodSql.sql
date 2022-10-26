if(N'{0}' = '1')
BEGIN
SELECT 
CAST(YEAR AS NVARCHAR(50)) Date, 
SUM(CASE WHEN InOut = 1 THEN  Amount ELSE 0 END) TotalInvoice ,
SUM(CASE WHEN InOut = -1 THEN  Amount ELSE 0 END) TotalReturnInvoice   ,
SUM(Amount * InOut) NetAmount 

FROM (
SELECT 
 
YEAR(inv.Date ) YEAR, 
inv.Net * inv.Rate Amount ,
invt.InOut

FROM org.Invoice inv
INNER JOIN org.InvoiceType invt ON invt.Id = inv.TypeId

WHERE 	
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	(Convert(bigint, N'{2}') = 0 OR dr.Id = Convert(bigint, N'{2}')) AND 
	(Convert(bigint, N'{3}') = 0 OR inv.ShiftId = Convert(bigint, N'{3}')) AND
	(Convert(bigint, N'{4}') = 0 OR inv.BranchId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR inv.CreateUserId = N'{5}')

) TB 

GROUP BY YEAR
END
ELSE if(N'{0}' = '2')
BEGIN
SELECT 

CAST(MONTH AS NVARCHAR(50)) + '/' +
CAST(YEAR AS NVARCHAR(50)) Date, 
SUM(CASE WHEN InOut = 1 THEN  Amount ELSE 0 END) TotalInvoice ,
SUM(CASE WHEN InOut = -1 THEN  Amount ELSE 0 END) TotalReturnInvoice   ,
SUM(Amount * InOut) NetAmount 

FROM (
SELECT 

MONTH(inv.Date ) MONTH, 
YEAR(inv.Date ) YEAR, 
inv.Net * inv.Rate Amount ,
invt.InOut

FROM org.Invoice inv
INNER JOIN org.InvoiceType invt ON invt.Id = inv.TypeId

WHERE 	
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	(Convert(bigint, N'{2}') = 0 OR dr.Id = Convert(bigint, N'{2}')) AND 
	(Convert(bigint, N'{3}') = 0 OR inv.ShiftId = Convert(bigint, N'{3}')) AND
	(Convert(bigint, N'{4}') = 0 OR inv.BranchId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR inv.CreateUserId = N'{5}')

) TB 

GROUP BY MONTH , YEAR
END
ELSE if(N'{0}' = '3')
BEGIN
SELECT 

CAST(DAY AS NVARCHAR(50)) + '/' +
CAST(MONTH AS NVARCHAR(50)) + '/' +
CAST(YEAR AS NVARCHAR(50)) Date, 
SUM(CASE WHEN InOut = 1 THEN  Amount ELSE 0 END) TotalInvoice ,
SUM(CASE WHEN InOut = -1 THEN  Amount ELSE 0 END) TotalReturnInvoice   ,
SUM(Amount * InOut) NetAmount 

FROM (
SELECT 
DAY(inv.Date ) DAY, 
MONTH(inv.Date ) MONTH, 
YEAR(inv.Date ) YEAR, 
inv.Net * inv.Rate Amount ,
invt.InOut

FROM org.Invoice inv
INNER JOIN org.InvoiceType invt ON invt.Id = inv.TypeId

WHERE 	
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{2}',111)) AND 
	(Convert(bigint, N'{3}') = 0 OR dr.Id = Convert(bigint, N'{3}')) AND 
	(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
	(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')

) TB 

GROUP BY DAY , MONTH , YEAR
END