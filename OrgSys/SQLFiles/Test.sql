
SELECT 
invp.ProductId ,
MIN(invp.Price) MINPrice,
MAX(invp.Price) MAXPice,
SUM(invp.Total) / SUM(invp.Quantity) AVGPrice
FROM org.InvoiceProduct invp
GROUP BY invp.ProductId
