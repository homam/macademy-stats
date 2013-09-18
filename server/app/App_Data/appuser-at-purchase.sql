-- registered / connected to FB at the time of purchase

DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';

WITH T AS (
	SELECT DISTINCT userId,
		(SELECT TOP 1 V.creationTime FROM dbo.MobiSpy_Events V WHERE 
			V.userId = E.userId AND v.eventType = 'IAP-Purchased' ORDER BY V.eventId ASC) AS [creationTime]
	FROM dbo.MobiSpy_Events E
	WHERE E.appId = 7 AND ip <> '80.227.47.62' AND E.eventType = 'IAP-Purchased' AND creationTime > @from AND creationTime < @to
),
T2 AS (
	SELECT T.userId, 
		(SELECT TOP 1 A.DataSource FROM dbo.ML_App_User A WHERE
			A.userId = T.userId 
			AND A.CreationTime < T.creationTime
			) AS [Source]
	FROM T
)

SELECT [Source], COUNT(*) AS Users FROM T2 GROUP BY [Source]
