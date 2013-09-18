-- IAP Requests

DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';

WITH T AS (
	SELECT DISTINCT userId,
		(SELECT COUNT(*) FROM dbo.MobiSpy_Events V WHERE
			V.userId = E.userId 
			AND V.eventType = 'IAP-Requested'
			) AS [Requests]
	FROM dbo.MobiSpy_Events E
	WHERE E.appId = 7 AND ip <> '80.227.47.62' AND creationTime > @from AND creationTime < @to
)

SELECT [Requests], COUNT(*) AS Users FROM T GROUP BY [Requests]
ORDER BY [Requests]