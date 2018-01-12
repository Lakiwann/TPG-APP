USE [APP]
GO

Declare @CategoryId tinyint
-- Insert 'Collateral' category descriptions

SET @CategoryId = (Select ID from [Trd].LU_DiligenceCategory 
WHERE CategoryName = 'Collateral')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Mod Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'LNA without Copy of Note')


INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Note Copy without LNA')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Note Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'CEMA Documents Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Coop Docs Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Endorsement Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Title Policy Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Security Instrument Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Note Copy with Outdated Bailee Letter')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'AOM Copy')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'AOM Unrecorded')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Loan File Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'AOM Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Mod Unexecuted')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Security Instrument Unrecorded')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Recognition Agreement Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Stock Certificate Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Note Amount Discrepancy')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Bailee Restriction - Note')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Note Cancelled')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'AOM Chain Corrections')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'LNA Restriction')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Lease Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'No AOM Signing Contact')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Incomplete File')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Collateral File Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'No END Signing Contact')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'LNA format')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Note with Court')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryId
           ,'Reperforming loan with bailee')
GO

DECLARE @CategoryIdComp tinyint
SET @CategoryIdComp = (Select ID from [Trd].LU_DiligenceCategory 
WHERE CategoryName = 'Compliance')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'Texas Home Equity Loan')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'RESPA')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'Federal High Cost Loan')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'State High Cost Loan')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'Missing TIL')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'Missing HUD')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'Incomplete HUD')


INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'State Issue')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'Incomplete TIL')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'Unable to Test')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'RTC Incomplete')


INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'ROR Missing')


INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'State Non-Compliant')


INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'Texas Cashout Violation')


INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'Finance Charge Underdiscolsure')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'No Tangible benefit')


INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdComp
           ,'HPML - not compliant')

GO

DECLARE @CategoryIdTitle tinyint
SET @CategoryIdTitle = (Select ID from [Trd].LU_DiligenceCategory 
WHERE CategoryName = 'Title')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Delinquent Taxes')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Corporate Borrower')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Borrower Not on Title')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Delinquent Lien')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Lien Position')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Judgments')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Statute of Limitations')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Lien Released')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Junior Mortgage')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Tax Sale')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Unreleased Prior Mortgage')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Municipal Tax Lien')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'State Tax Lien')


INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Federal Tax Lien')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'HOA Lien')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Mortgage Released')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdTitle
           ,'Large Muni Judgements')

GO

DECLARE @CategoryIdData tinyint
SET @CategoryIdData = (Select ID from [Trd].LU_DiligenceCategory 
WHERE CategoryName = 'Data')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdData
           ,'Step Incorrect')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdData
           ,'Step Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdData
           ,'Mod Incorrect')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdData
           ,'Mod Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdData
           ,'ARM Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdData
           ,'ARM Incorrect')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdData
           ,'Undisclosed PRA')

GO

DECLARE @CategoryIdProp tinyint
SET @CategoryIdProp = (Select ID from [Trd].LU_DiligenceCategory 
WHERE CategoryName = 'Property')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdProp
           ,'Type Restriction')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdProp
           ,'Vacant Land')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdProp
           ,'Damage')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdProp
           ,'Uninhabitable')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdProp
           ,'Condemnded')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdProp
           ,'Litigation')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdProp
           ,'REO')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdProp
           ,'Value')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdProp
           ,'Condition')

Go

DECLARE @CategoryIdSrvc tinyint
SET @CategoryIdSrvc = (Select ID from [Trd].LU_DiligenceCategory 
WHERE CategoryName = 'Servicing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Loss Mitigation')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Borrower Unemployed')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Borrower Hardship')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Borrower Deceased')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'CFPB Violation')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Delinquency Status')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Fraud')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Hardest Hit Fund')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Litigation')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Pay History Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Servicing Comments Missing')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Servicing Dispute')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Flood Zone Dispute')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Unsupported Deferred')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSrvc
           ,'Undisclosed PRA')

GO

DECLARE @CategoryIdSeller tinyint
SET @CategoryIdSeller = (Select ID from [Trd].LU_DiligenceCategory 
WHERE CategoryName = 'Seller')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSeller
           ,'Error')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSeller
           ,'Holding Entity Restriction')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSeller
           ,'Loss Mitigation')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSeller
           ,'Resolved')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSeller
           ,'Delinquency Status')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSeller
           ,'Price Adjustment Disagreement')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSeller
           ,'Subsequent Settlement')

INSERT INTO [Trd].[LU_DiligenceDesc]
           ([CategoryID]
           ,[Description])
     VALUES
           (@CategoryIdSeller
           ,'Bid Price not Accepted')

GO

