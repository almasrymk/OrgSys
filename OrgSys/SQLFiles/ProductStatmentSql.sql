-----------------------------------
-- (Parameters)
-- 1- {0} FromDate
-- 2- {1} ToDate
-- 3- {2} ProductId
-- 4- {3} StockId
-----------------------------------


select 
TP.Id,
TP.Quantity,
TP.ProductId,
TP.TransactionId AS ReferenceId,
P.[Name] ProductName,
P.Code as ProductCode,
P.ImgPath as ProductImgPath,
P.ClassificationId,
T.[Date],
T.Code as TransactionCode,
T.TypeId,
S.[Name] StockName,
TP.StockId,
TT.[Name] as TypeName,
C.[Name] ClassificationName


from [org].[TransactionProduct] TP
inner join [org].[Transaction] T on T.Id = TP.TransactionId
inner join [org].[Product] P on P.Id = TP.ProductId
inner join [org].[Stock] S on S.Id = TP.StockId
inner join [org].[TransactionType] TT on TT.Id = T.TypeId
inner join [org].[Classification] C on C.Id = P.ClassificationId

where 
CONVERT(datetime , CONVERT(VARCHAR(20),T.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
CONVERT(datetime , CONVERT(VARCHAR(20),T.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
(Convert(bigint, N'{2}') = 0 OR TP.StockId = Convert(bigint, N'{2}')) AND
(Convert(bigint, N'{3}') = 0 OR TP.ProductId = Convert(bigint, N'{3}')) 

--WHERE p.Id = N'{3}'


