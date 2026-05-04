-----------------------------------
-- (Parameters)
-- 1- {0} ToDate
-- 2- {1} SafeId
-- 3- {2} UserId
-- 4- {3} ShiftId
-----------------------------------

select SUM(f.AmountByDefaultCurrency * ft.InOut ) AS [Balance],
f.SafeId,
sf.[Name] SafeName,
ft.Id FinancialTypeId,
ft.[Name] FinancialTypeName,
u.[Id] UserIs,
u.[Name] UserName,
S.[Id] ShiftIs,
S.[Name] ShiftName

from [org].[Financial] f
inner join [org].[Safe] sf on sf.Id = f.SafeId
inner join [org].[User] u on u.Id = f.CreateUserId
inner join [org].[Shift] S on S.Id = f.ShiftId
inner join [org].[FinancialType] ft on ft.Id = f.TypeId

WHERE 		
CONVERT(datetime , CONVERT(VARCHAR(20),f.Date,111)) <= CONVERT(datetime , CONVERT(VARCHAR(20),N'{0}',111)) AND 
(Convert(bigint, N'{1}') = 0 OR sf.Id= Convert(bigint, N'{1}')) AND
(Convert(bigint, N'{2}') = 0 OR u.Id = Convert(bigint, N'{2}')) AND
(Convert(bigint, N'{3}') = 0 OR s.Id = Convert(bigint, N'{3}'))

group by f.SafeId, sf.[Name] , ft.Id ,ft.[Name] , u.[Id] , u.[Name] , s.Id , s.[Name]