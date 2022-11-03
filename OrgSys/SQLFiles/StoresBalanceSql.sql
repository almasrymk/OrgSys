-----------------------------------
-- (Parameters)
-- 1- {0} FromDate
-- 2- {1} StoreId
-- 3- {2} ShiftId
-- 4- {3} BranchId
-- 5- {4} CreateUserId
-----------------------------------

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

WHERE 		
CONVERT(datetime , CONVERT(VARCHAR(20),trn.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
(Convert(bigint, N'{1}') = 0 OR trn.StoreId = Convert(bigint, N'{1}')) AND
(Convert(bigint, N'{2}') = 0 OR trn.ShiftId = Convert(bigint, N'{2}')) AND
(Convert(bigint, N'{3}') = 0 OR trn.BranchId = Convert(bigint, N'{3}')) AND
(Convert(bigint, N'{4}') = 0 OR trn.CreateUserId = N'{4}')

GROUP BY trn.StoreId , st.[Name] , trnp.ProductId , pr.[Name]

) TB 

GROUP BY StoreId , StoreName , ProductId , ProductName