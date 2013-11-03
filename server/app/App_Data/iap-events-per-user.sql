-- IAP events breakdown per user

DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';
DECLARE @appid INT = {appid};


WITH T AS (
	SELECT E.userId, E.eventType FROM dbo.MobiSpy_Events E 
	WHERE E.appId = @appid AND E.ip <> '80.227.47.62' AND
	E.creationTime > @from AND E.creationTime < @to
),

T2 AS (
	SELECT userId,
		SUM(CASE WHEN T.eventType = 'pageReady' THEN 1 ELSE NULL END) AS Users,
		SUM(CASE WHEN T.eventType = 'IAP-Requested' THEN 1 ELSE NULL END) AS Requests,
		SUM(CASE WHEN T.eventType = 'IAP-Received' THEN 1 ELSE NULL END) AS Recevies,
		SUM(CASE WHEN T.eventType = 'IAP-BuyTry' THEN 1 ELSE NULL END) AS BuyTries, 	 
		SUM(CASE WHEN T.eventType = 'IAP-Failed' THEN 1 ELSE NULL END) AS Failures,
		SUM(CASE WHEN T.eventType = 'IAP-Purchased' THEN 1 ELSE NULL END) AS Purchases
	FROM T 
	GROUP BY userId

)

SELECT
	COUNT(Users) AS Users,
	COUNT(Requests) AS Requests,
	COUNT(Recevies) AS Recevies,
	COUNT(BuyTries) AS BuyTries,
	COUNT(Failures) AS Failures,
	COUNT(Purchases) AS Purchases
FROM T2