-----------------------------------
-- (Parameters)
-- 1- {0} FromDate
-- 2- {1} ToDate
-- 3- {2} StoreId
-- 4- {3} ShiftId
-- 5- {4} BranchId
-- 6- {5} CreateUserId
-----------------------------------

	---------- Get Openning Balance ----------

	SELECT 
	1 OpenningBalance,
	0 Type,
	NULL ReferenceId, 
	NULL Code ,
	1 TypeId , 
	N'Openning Balance' TypeName , 
	CONVERT(datetime , CONVERT(VARCHAR(20),N'1-1-2012',111)) [Date] ,
	StoreId , 
	StoreName ,
	ProductId , 
	ProductName ,
	SUM(Quantity) Quantity ,
	CASE WHEN SUM(Quantity) > 0 THEN 1 ELSE -1 END InOut

	FROM (
		SELECT 
		trn.StoreId , 
		st.[Name] StoreName , 
		trnp.ProductId , 
		pr.[Name] ProductName , 
		trnp.Quantity * trnt.InOut Quantity 

		FROM Org.[Transaction] trn
		INNER JOIN Org.TransactionProduct trnp ON trnp.TransactionId = trn.Id
		INNER JOIN Org.TransactionType trnt ON trnt.Id = trn.TypeId
		INNER JOIN Org.Store st ON st.Id = trn.StoreId
		INNER JOIN Org.Product pr ON pr.Id = trnp.ProductId

		WHERE 		
		CONVERT(datetime , CONVERT(VARCHAR(20),trn.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
		(Convert(bigint, N'{2}') = 0 OR trn.StoreId = Convert(bigint, N'{2}')) AND
		(Convert(bigint, N'{3}') = 0 OR trn.ShiftId = Convert(bigint, N'{3}')) AND
		(Convert(bigint, N'{4}') = 0 OR trn.BranchId = Convert(bigint, N'{4}')) AND
		(Convert(bigint, N'{5}') = 0 OR trn.CreateUserId = N'{5}')
		
	) OpenningBalance 
	
	GROUP BY 
	StoreId , 
	StoreName,
	ProductId,
	ProductName

	UNION ALL

	---------- Get All Transactions ----------

	SELECT 
	0 OpenningBalance,
	1 Type ,
	trn.Id ReferenceId, 
	trn.Code ,
	trnt.TypeId , 
	trnt.[Name] TypeName , 
	trn.[Date] ,
	trn.StoreId , 
	st.[Name] StoreName , 
    trnp.ProductId , 
	pr.[Name] ProductName , 
	trnp.Quantity * trnt.InOut Quantity ,
	1

	FROM Org.[Transaction] trn
	INNER JOIN Org.TransactionProduct trnp ON trnp.TransactionId = trn.Id
	INNER JOIN Org.TransactionType trnt ON trnt.Id = trn.TypeId
	INNER JOIN Org.Store st ON st.Id = trn.StoreId
	INNER JOIN Org.Product pr ON pr.Id = trnp.ProductId

	WHERE 
	CONVERT(datetime , CONVERT(VARCHAR(20),trn.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
	CONVERT(datetime , CONVERT(VARCHAR(20),trn.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	(Convert(bigint, N'{2}') = 0 OR trn.StoreId = Convert(bigint, N'{2}')) AND
	(Convert(bigint, N'{3}') = 0 OR trn.ShiftId = Convert(bigint, N'{3}')) AND
	(Convert(bigint, N'{4}') = 0 OR trn.BranchId = Convert(bigint, N'{4}')) AND
	(Convert(bigint, N'{5}') = 0 OR trn.CreateUserId = N'{5}')