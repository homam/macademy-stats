DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';
DECLARE @appid INT = {appid};

WITH T AS (
	SELECT DISTINCT userId,
	(SELECT TOP 1 V.viewId FROM dbo.MobiSpy_Events V WHERE V.userId = E.userId AND v.eventType = 'IAP-Purchased' ORDER BY V.eventId ASC) AS [View]
	FROM dbo.MobiSpy_Events E
	WHERE E.appId = @appid AND ip <> '80.227.47.62' AND E.eventType = 'IAP-Purchased' AND creationTime > @from AND creationTime < @to
)

SELECT [View], COUNT(*) AS [Count] from T GROUP BY [View]