-- how many page visits was made before making a purchase

DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';

WITH T AS (
	SELECT DISTINCT userId,
		(SELECT TOP 1 V.eventId FROM dbo.MobiSpy_Events V WHERE 
			V.userId = E.userId AND v.eventType = 'IAP-Purchased' ORDER BY V.eventId ASC) AS eventId
	FROM dbo.MobiSpy_Events E
	WHERE E.appId = 7 AND ip <> '80.227.47.62' AND E.eventType = 'IAP-Purchased' AND creationTime > @from AND creationTime < @to
),
T2 AS (
	SELECT T.userId, 
		(SELECT COUNT(*) FROM dbo.MobiSpy_Events E WHERE
			E.userId = T.userId AND E.eventId <  T.eventId
			AND E.eventType = 'pageReady') AS [Visits]
	FROM T
)

SELECT Visits, COUNT(*) AS Users FROM T2 GROUP BY Visits
ORDER BY Visits

