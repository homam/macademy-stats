-- IAP Requests

DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';
DECLARE @appid INT = {appid};

WITH R AS (
	SELECT V.userId, COUNT(*) AS Requests FROM dbo.MobiSpy_Events V WITH (NOLOCK) WHERE
	V.appId = @appid AND V.creationTime >@from AND v.creationTime < @to
	AND V.eventType = 'IAP-Requested'
	GROUP BY V.userId
), 
T AS (

	SELECT DISTINCT E.userId, ISNULL( R.Requests, 0) AS Requests
	FROM dbo.MobiSpy_Events E WITH (NOLOCK)
	LEFT JOIN R ON R.userID = E.userId
	WHERE E.appId = @appid AND ip <> '80.227.47.62' AND creationTime > @from AND creationTime < @to
)

SELECT [Requests], COUNT(*) AS Users FROM T GROUP BY [Requests]
ORDER BY [Requests]