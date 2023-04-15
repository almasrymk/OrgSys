	---------- Get Openning Balance ----------

	SELECT 
	1 OpenningBalance,
	0 Type,
	NULL ReferenceId, 
	NULL Code ,
	1 TypeId , 
	N'Openning Balance' TypeName , 
	CONVERT(datetime , CONVERT(VARCHAR(20),N'1-1-2012',111)) [Date] ,
	StockId , 
	StockName ,
	ProductId , 
	ProductName ,
	SUM(Quantity) Quantity ,
	CASE WHEN SUM(Quantity) > 0 THEN 1 ELSE -1 END InOut

	FROM (
		SELECT 
		trn.StockId , 
		st.[Name] StockName , 
		trnp.ProductId , 
		pr.[Name] ProductName , 
		trnp.Quantity * trnt.InOut Quantity 

		FROM Org.[Transaction] trn
		INNER JOIN Org.TransactionProduct trnp ON trnp.TransactionId = trn.Id
		INNER JOIN Org.TransactionType trnt ON trnt.Id = trn.TypeId
		INNER JOIN Org.Stock st ON st.Id = trn.StockId
		INNER JOIN Org.Product pr ON pr.Id = trnp.ProductId

		--WHERE 
		--dr.TypeId = Convert(bigint, N'{0}') AND 
		--CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) < CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
		--(Convert(bigint, N'{3}') = 0 OR dr.Id = Convert(bigint, N'{3}')) AND 
		--(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
		--(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
		--(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')

		
	) OpenningBalance 
	
	GROUP BY 
	StockId , 
	StockName,
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
	trn.StockId , 
	st.[Name] StockName , 
    trnp.ProductId , 
	pr.[Name] ProductName , 
	trnp.Quantity * trnt.InOut Quantity ,
	1

	FROM Org.[Transaction] trn
	INNER JOIN Org.TransactionProduct trnp ON trnp.TransactionId = trn.Id
	INNER JOIN Org.TransactionType trnt ON trnt.Id = trn.TypeId
	INNER JOIN Org.Stock st ON st.Id = trn.StockId
	INNER JOIN Org.Product pr ON pr.Id = trnp.ProductId

	--WHERE 
	--dr.TypeId = Convert(bigint, N'{0}') AND 
	--CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
	--CONVERT(datetime , CONVERT(VARCHAR(20),inv.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{2}',111)) AND 
	--(Convert(bigint, N'{3}') = 0 OR dr.Id = Convert(bigint, N'{3}')) AND 
	--(Convert(bigint, N'{4}') = 0 OR inv.ShiftId = Convert(bigint, N'{4}')) AND
	--(Convert(bigint, N'{5}') = 0 OR inv.BranchId = Convert(bigint, N'{5}')) AND
	--(Convert(bigint, N'{6}') = 0 OR inv.CreateUserId = N'{6}')
