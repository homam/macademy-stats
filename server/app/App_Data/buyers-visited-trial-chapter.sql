-- buyers that visited the trial chapter

DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';
DECLARE @appid INT = {appid};

WITH T AS (
	SELECT DISTINCT userId,
		(SELECT TOP 1 V.eventId FROM dbo.MobiSpy_Events V WHERE 
			V.userId = E.userId AND v.eventType = 'IAP-Purchased' ORDER BY V.eventId ASC) AS eventId
	FROM dbo.MobiSpy_Events E
	WHERE E.appId = @appid AND ip <> '80.227.47.62' AND E.eventType = 'IAP-Purchased' AND creationTime > @from AND creationTime < @to
),
T2 AS (
	SELECT T.userId,
		(SELECT TOP 1 V.eventId FROM dbo.MobiSpy_Events V WHERE
			v.userId = T.userId AND v.eventId < T.eventId AND v.eventType = 'pageReady' AND v.eventArgs LIKE '%levelId%' ORDER BY eventId ASC) AS VisitedTrialChapter
	FROM T
)

SELECT 
	(SELECT COUNT(userId) FROM T2) AS Users,
	(SELECT COUNT(VisitedTrialChapter)  FROM T2) AS Visits

