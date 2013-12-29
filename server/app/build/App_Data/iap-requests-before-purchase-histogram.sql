-- IAP Requests before purchase

DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';
DECLARE @appid INT = {appid};

IF OBJECT_ID('tempdb.dbo.#Purchases_RequestsBeforePurchase', 'U') IS NOT NULL
	DROP TABLE  #Purchases_RequestsBeforePurchase

SELECT * into #Purchases_RequestsBeforePurchase FROM dbo.MobiSpy_Events E WITH (NOLOCK)
WHERE  E.appId = @appid AND ip <> '80.227.47.62' AND E.eventType = 'IAP-Purchased' AND E.creationTime > @from AND E.creationTime < @to
;
WITH T2 AS (
	SELECT T.userId, 
		(SELECT COUNT(*) FROM dbo.MobiSpy_Events E WHERE
			E.userId = T.userId 
			AND E.eventId < T.eventId
			AND e.eventType = 'IAP-Requested'
			) AS [Requests]
	FROM #Purchases_RequestsBeforePurchase AS T
)

SELECT [Requests], COUNT(*) AS Users FROM T2 GROUP BY [Requests]
ORDER BY [Requests]
