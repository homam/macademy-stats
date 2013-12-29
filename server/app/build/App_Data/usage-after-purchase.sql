-- usage - visits after the purchase

DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';
DECLARE @appid INT = {appid};

IF OBJECT_ID('tempdb.dbo.#Purchases_UsageAfter', 'U') IS NOT NULL
	DROP TABLE  #Purchases_UsageAfter

SELECT * into #Purchases_UsageAfter FROM dbo.MobiSpy_Events E WITH (NOLOCK)
WHERE  E.appId = @appid AND ip <> '80.227.47.62' AND E.eventType = 'IAP-Purchased' AND E.creationTime > @from AND E.creationTime < @to
;


WITH T2 AS (
	SELECT T.userId, 
		(SELECT COUNT(*) FROM dbo.MobiSpy_Events E WHERE
			E.userId = T.userId AND E.eventId >  T.eventId
			AND E.eventType = 'pageReady') AS [Visits]
	FROM #Purchases_UsageAfter AS T
)

SELECT Visits, COUNT(*) AS Users FROM T2 GROUP BY Visits
ORDER BY Visits