 SELECT TOP 1000 [TradeID]
      ,[TradeType]
      ,[TradeName]
      ,[EstSettlementDate]
      ,[ManagerName]
      ,[ManagerInitials]
  FROM [ODS_Palisades].[dbo].[TPG_TradePoolLevel]
  order by TradeId 
  
 SELECT TOP 1000 [TradeID]
      ,[TradeStage]
      ,[TradeStageDate]
  FROM [ODS_Palisades].[dbo].[TPG_TradeStageLevel]
  where tradeid = 9

  --Not needed
  SELECT TOP 1000 [SatgeId]
      ,[StageName]
      ,[Status]
  FROM [ODS_Palisades].[dbo].[TPG_TradeStageDef]


  SELECT TOP 1000 *
  FROM [ODS_Palisades].[dbo].[vTPG_TradeAssetLevelChangeNew]
  --where ServicerAssetID=7600031632
  where TradeID=10 order by ServicerAssetID, Date--and ServicerAssetID=10121101

  select * from TPG_TradeStageLevel where TradeID=10

 --select top 1000 
	--   ln.[TradeID]
 --     ,ln.[ServicerAssetID]
 --     ,ln.[BidPricePct]
 --     ,ln.[Type]
 --     ,ln.[Category]
 --     ,ln.[Description]
 --     ,stg.TradeStage
	--  ,ln.[Date]		
 --     ,ln.[TimeString]
 --     ,ln.[Seq]from [ODS_Palisades].[dbo].[TPG_TradeAssetLevelChangeNew] ln
 --left join [ODS_Palisades].[dbo].[TPG_TradeStageLevel] stg on stg.TradeID = ln.TradeID
 --where ln.TradeId=10  and ServicerAssetID=10121101
 

 select * from [ODS_Palisades].[dbo].[TPG_TradeAssetLevelChangeNew] where TradeId=10 and ServicerAssetID in (10121101, 10051399)
 order by ServicerAssetID

 select * from TPG_TradeStageLevel where TradeID=10


Select 'The view that is used for the display'
Create View [dbo].[vTPG_TradeAssetLevelChangeNew]
AS

SELECT ln.[TradeID]
      ,ln.[ServicerAssetID]
      ,ln.[BidPricePct]
      ,ln.[Type]
      ,ln.[Category]
      ,ln.[Description]
      ,stg.[TradeStageDate]		[Date]
      ,ln.[TimeString]
      ,ln.[Seq]
  FROM [ODS_Palisades].[dbo].[TPG_TradeAssetLevelChangeNew] ln
  Left Join [ODS_Palisades].[dbo].[TPG_TradeStageLevel] stg on stg.TradeID = ln.TradeID AND stg.TradeStage = 1   -- Pool Identified
  WHERE ln.[Type] IS NULL AND ln.[Category] IS NULL AND ln.[Description] IS NULL
  AND ln.[BidPricePct] IS NULL

UNION

SELECT ln.[TradeID]
      ,ln.[ServicerAssetID]
      ,ln.[BidPricePct]
      ,ln.[Type]
      ,ln.[Category]
      ,ln.[Description]
      ,stg.[TradeStageDate]		[Date]
      ,ln.[TimeString]
      ,ln.[Seq]
  FROM [ODS_Palisades].[dbo].[TPG_TradeAssetLevelChangeNew] ln
  Left Join [ODS_Palisades].[dbo].[TPG_TradeStageLevel] stg on stg.TradeID = ln.TradeID AND stg.TradeStage = 3   -- Bids Made
  WHERE ln.[Type] IS NULL AND ln.[Category] IS NULL AND ln.[Description] IS NULL
  AND ln.[BidPricePct] IS NOT NULL

UNION 

SELECT [TradeID]
      ,[ServicerAssetID]
      ,[BidPricePct]
      ,[Type]
      ,[Category]
      ,[Description]
      ,[Date]
      ,[TimeString]
      ,[Seq]
  FROM [ODS_Palisades].[dbo].[TPG_TradeAssetLevelChangeNew]     -- All Changees 
  WHERE TYPE IS NOT NULL
 

GO