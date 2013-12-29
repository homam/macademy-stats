DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';
DECLARE @appid INT = {appid};

IF OBJECT_ID('tempdb.dbo.#Purchases_ViewOfPurchase', 'U') IS NOT NULL
	DROP TABLE #Purchases_ViewOfPurchase

SELECT * into #Purchases_ViewOfPurchase FROM dbo.MobiSpy_Events E WITH (NOLOCK)
WHERE  E.appId = @appid AND ip <> '80.227.47.62' AND E.eventType = 'IAP-Purchased' AND E.creationTime > @from AND E.creationTime < @to
;
SELECT [viewId] AS [View], COUNT(*) AS [Count] from #Purchases_ViewOfPurchase GROUP BY viewId