-----------------------------------
-- (Parameters)
-- 1- {0} FromDate
-- 2- {1} ToDate
-- 3- {2} DealerId
-- 4- {3} SafeId
-----------------------------------


select 
f.Id , 
f.SafeId ,
f.DealerId,
s.[Name] SafeName , 
f.TypeId,
ft.[Name] TypeName ,
f.Date , 
f.Code , 
f.CodeNumber as ReferenceId,
d.[Name] DealerName ,
f.Amount , 
c.[Name] as CurrencyName,
f.CurrencyId

from [org].[Financial] f
inner join [org].[Safe] s on f.SafeId = s.Id
inner join [org].[FinancialType] ft on f.TypeId = ft.Id
inner join [org].[Dealer] d on f.DealerId = d.Id
inner join [org].[Currency] c on f.CurrencyId = c.Id

where 
CONVERT(datetime , CONVERT(VARCHAR(20),f.Date,111)) >= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
CONVERT(datetime , CONVERT(VARCHAR(20),f.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{1}',111)) AND 
(Convert(bigint, N'{2}') = 0 OR f.DealerId = Convert(bigint, N'{2}')) AND
(Convert(bigint, N'{3}') = 0 OR f.SafeId = Convert(bigint, N'{3}')) 



