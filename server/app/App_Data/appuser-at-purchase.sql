-- registered / connected to FB at the time of purchase

DECLARE @from DATETIME = '2013-12-20';
DECLARE @to DATETIME = '2013-12-29';
DECLARE @appid INT = 11;


IF OBJECT_ID('tempdb.dbo.#Purchases', 'U') IS NOT NULL
	DROP TABLE  #Purchases

SELECT * into #Purchases FROM dbo.MobiSpy_Events E WITH (NOLOCK)
WHERE  E.appId = @appid AND ip <> '80.227.47.62' AND E.eventType = 'IAP-Purchased' AND E.creationTime > @from AND E.creationTime < @to
;
WITH T2 AS (
	SELECT T.userId, 
		(SELECT TOP 1 A.DataSource FROM dbo.ML_App_User A WHERE
			A.userId = T.userId 
			AND A.CreationTime < T.creationTime
			) AS [Source]
	FROM #Purchases AS T
)

SELECT [Source], COUNT(*) AS Users FROM T2 GROUP BY [Source]
