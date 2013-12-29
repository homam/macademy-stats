-- buyers that visited the trial chapter

DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';
DECLARE @appid INT = {appid};

IF OBJECT_ID('tempdb.dbo.#Purchases_BuyersVisitedTrial', 'U') IS NOT NULL
	DROP TABLE  #Purchases_BuyersVisitedTrial

SELECT * into #Purchases_BuyersVisitedTrial FROM dbo.MobiSpy_Events E WITH (NOLOCK)
WHERE  E.appId = @appid AND ip <> '80.227.47.62' AND E.eventType = 'IAP-Purchased' AND E.creationTime > @from AND E.creationTime < @to
;
WITH T2 AS (
	SELECT T.userId,
		(SELECT TOP 1 V.eventId FROM dbo.MobiSpy_Events V WHERE
			v.userId = T.userId AND v.eventId < T.eventId AND v.eventType = 'pageReady' AND v.eventArgs LIKE '%levelId%' ORDER BY eventId ASC) AS VisitedTrialChapter
	FROM #Purchases_BuyersVisitedTrial AS T
)

SELECT 
	(SELECT COUNT(userId) FROM T2) AS Users,
	(SELECT COUNT(VisitedTrialChapter)  FROM T2) AS Visits


