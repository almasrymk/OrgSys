-----------------------------------
-- (Parameters)
-- 1- {0} FromDate
-- 2- {1} ToDate
-- 3- {2} ProductId
-- 4- {3} StockId
-- 5- {4} ClassificationId
-----------------------------------

select SUM(TP.Quantity * TT.InOut ) AS [Balance],
TP.ProductId,
P.[Name] ProductName,
TP.StockId,
S.[Name] StockName,
C.Id ClassificationId,
C.[Name] ClassificationName,
C.[ImgPath] ClassificationImgPath,
P.[ImgPath] ProductImgPath

from [org].[TransactionProduct] TP
inner join [org].[Transaction] T on T.Id = TP.TransactionId
inner join [org].[Product] P on P.Id = TP.ProductId
inner join [org].[Stock] S on S.Id = TP.StockId
inner join [org].[TransactionType] TT on TT.Id = T.TypeId
inner join [org].[Classification] C on C.Id = P.ClassificationId

WHERE 		
CONVERT(datetime , CONVERT(VARCHAR(20),T.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
(Convert(bigint, N'{1}') = 0 OR TP.ProductId= Convert(bigint, N'{1}')) AND
(Convert(bigint, N'{2}') = 0 OR TP.StockId = Convert(bigint, N'{2}')) AND
(Convert(bigint, N'{3}') = 0 OR C.Id = Convert(bigint, N'{3}'))

group by TP.ProductId, P.[Name] , TP.StockId,S.[Name] , C.[Name] , C.Id , C.ImgPath,P.ImgPath