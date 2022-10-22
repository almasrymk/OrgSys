SELECT 
Date , 
SUM(CASE WHEN InOut = 1 THEN  Amount ELSE 0 END) TotalInvoice ,
SUM(CASE WHEN InOut = -1 THEN  Amount ELSE 0 END) TotalReturnInvoice   ,
SUM(Amount * InOut) NetAmount 

FROM (
SELECT 
inv.Date , 
inv.Net * inv.Rate Amount ,
invt.InOut

FROM org.Invoice inv
INNER JOIN org.InvoiceType invt ON invt.Id = inv.TypeId
) TB 

GROUP BY Date