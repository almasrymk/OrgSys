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
--dl.TypeId = Convert(bigint, N'{0}') AND 
( N'{0}' = 0 OR dl.Id =  '{0}')



GROUP BY i.DealerId ,dl.Name
