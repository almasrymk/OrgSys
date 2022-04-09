SELECT 
dr.* , 
drg.[Name] DealerGroupName ,  
cnt.[Name] CountryName ,
cty.[Name] CityName , 
dst.[Name] DistrictName 

FROM Org.Dealer dr
LEFT JOIN Org.DealerGroup drg on drg.Id = dr.DealerGroupId
LEFT JOIN Org.Country cnt on cnt.Id = dr.CountryId
LEFT JOIN Org.City cty on cty.Id = dr.CityId
LEFT JOIN Org.District dst on dst.Id = dr.DistrictId

WHERE 
(N'{0}' = '' OR
dr.[Name] LIKE N'%{0}%' OR 
dr.[Address] LIKE N'%{0}%' OR
dr.Email LIKE N'%{0}%' OR
dr.Phone  LIKE N'%{0}%') AND
dr.TypeId = CONVERT(bigint , '{1}')