select 
i.DealerId ,  
dl.Name DealerName,

 
SUM(case when it.Id = 1 then i.net  else 0 end) InAmount,
SUM(case when it.Id = 3 then i.net  else 0 end) OutAmount ,
SUM( case when it.Id = 1 then i.net * it.InOut  else 0 end ) Net
from [org].[Invoice] i 
inner join [org].[InvoiceType] it on it.Id = i.TypeId
INNER JOIN [org].[Dealer] dl ON dl.Id = i.DealerId
where
	CONVERT(datetime , CONVERT(VARCHAR(20),i.Date,111))< CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) and
--dl.TypeId = Convert(bigint, N'{0}') AND 
( N'{1}' = 0 OR dl.Id =  '{1}')



GROUP BY i.DealerId ,dl.Name
