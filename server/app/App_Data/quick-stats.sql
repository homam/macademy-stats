-- quick statistics

DECLARE @from DATETIME = '{from}';
DECLARE @to DATETIME = '{to}';

SELECT
	(SELECT COUNT (*) FROM dbo.Wap_Visits V  WHERE V.Date_Created > @from AND V.Date_Created < @to AND v.Redirection = 3) AS Visits,
	(SELECT COUNT(DISTINCT UserId) FROM dbo.MobiSpy_Events WHERE appId = 7 AND ip <> '80.227.47.62' AND creationTime > @from AND creationTime < @to) AS Users,
	(SELECT COUNT(DISTINCT UserId) FROM dbo.ML_App_User WHERE appId = 7 AND creationTime > @from AND CreationTime < @to) AS RegisteredUsers,
	(SELECT COUNT(DISTINCT UserId) FROM dbo.ML_App_User WHERE appId = 7 AND creationTime > @from AND CreationTime < @to AND DataSource = 'facebok') AS FacebookConnects,
	(SELECT COUNT(DISTINCT userId) FROM dbo.MobiSpy_Events E WHERE E.appId = 7 AND ip <> '80.227.47.62' AND E.eventType = 'IAP-Purchased' AND creationTime > @from AND creationTime < @to) AS Purchases
