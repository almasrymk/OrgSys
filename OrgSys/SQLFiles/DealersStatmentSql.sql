SELECT 

*,
SUM(Amount) OVER(PARTITION BY tb.DealerId ORDER BY tb.DealerId , tb.Type  , tb.Date desc ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS Balance

FROM (

    ---------- Get Openning Balance ----------

	SELECT 
	0 Type,
	NULL Id , 
	NULL Code ,
	NULL TypeId , 
	N'Openning Balance' TypeName , 
	NULL [Date] ,
	DealerId , 
	DealerName ,
	SUM(Amount) Amount ,
	CASE WHEN SUM(Amount) > 0 THEN 1 ELSE -1 END InOut

	FROM (
		SELECT 
		inv.DealerId , 
		dr.[Name] DealerName , 
		inv.Net * invt.InOut Amount 

		FROM Org.Invoice inv
		INNER JOIN Org.InvoiceType invt ON invt.Id = inv.TypeId
		INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

		UNION ALL

		SELECT 
		inv.DealerId , 
		dr.[Name] DealerName , 
		inv.Amount * invt.InOut Amount

		FROM Org.Financial inv
		INNER JOIN Org.FinancialType invt ON invt.Id = inv.TypeId
		INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId
	) OpenningBalance 

	GROUP BY 
	DealerId , 
	DealerName

	UNION ALL

	---------- Get All Invoices ----------

	SELECT 
	1 Type ,
	inv.Id , 
	Inv.Code ,
	inv.TypeId , 
	invt.[Group] + ' ' + invt.[Name] TypeName , 
	inv.[Date] ,
	inv.DealerId , 
	dr.[Name] DealerName , 
	inv.Net Amount ,
	invt.InOut
	FROM Org.Invoice inv
	INNER JOIN Org.InvoiceType invt ON invt.Id = inv.TypeId
	INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId

	--WHERE 
	--invt.Id IN (Convert(bigint, '{0}') , Convert(bigint, '{1}')) AND 
	--inv.Date >= '' AND inv.Date <= '' AND
	--(Convert(bigint, '{2}') = 0 OR dr.Id = Convert(bigint, '{2}')) AND 
	--(Convert(bigint, '{2}') = 0 OR inv.ShiftId = Convert(bigint, '{2}')) AND
	--(Convert(bigint, '{2}') = 0 OR inv.BranchId = Convert(bigint, '{2}')) AND
	--(Convert(bigint, '{2}') = 0 OR inv.CreateUserId = '')

	UNION ALL

	---------- Get All Financial ----------

	SELECT 
	1 Type ,
	inv.Id , 
	Inv.Code ,
	inv.TypeId , 
	invt.[Name] TypeName , 
	inv.[Date] ,
	inv.DealerId , 
	dr.[Name] DealerName , 
	inv.Amount Amount ,
	invt.InOut

	FROM Org.Financial inv

	INNER JOIN Org.FinancialType invt ON invt.Id = inv.TypeId
	INNER JOIN Org.Dealer dr ON dr.Id = inv.DealerId
) AS TB 

ORDER BY 
tb.DealerId , 
tb.Type  , 
tb.Date desc