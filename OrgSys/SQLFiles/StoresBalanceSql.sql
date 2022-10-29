SELECT 
StoreId ,  
StoreName ,
ProductId,
ProductName ,
SUM(Quantity) Quantity FROM
(
SELECT 
trn.StoreId ,  
st.[Name] StoreName ,
trnp.ProductId ,
pr.[Name] ProductName ,
SUM(trnp.Quantity * trnt.InOut) Quantity 

FROM org.[Transaction] trn 
INNER JOIN org.TransactionProduct trnp ON trnp.TransactionId = trn.Id
INNER JOIN org.TransactionType trnt ON trnt.Id = trn.TypeId
INNER JOIN org.Store st ON st.Id = trn.StoreId
INNER JOIN org.Product pr ON pr.Id = trnp.ProductId

--WHERE
--dl.TypeId = Convert(bigint, N'{0}') AND 
--CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
--(Convert(bigint, N'{2}') = 0 OR dl.Id = Convert(bigint, N'{2}')) AND 
--(Convert(bigint, N'{3}') = 0 OR inv.ShiftId = Convert(bigint, N'{3}')) AND
--(Convert(bigint, N'{4}') = 0 OR inv.BranchId = Convert(bigint, N'{4}')) AND
--(Convert(bigint, N'{5}') = 0 OR inv.CreateUserId = N'{5}')

GROUP BY trn.StoreId , st.[Name] , trnp.ProductId , pr.[Name]

) TB 

GROUP BY StoreId , StoreName , ProductId , ProductName