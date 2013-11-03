using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace server.Controllers
{
    public class HomeController : Controller
    {
        volatile string _ConnectionString;
        public string ConnectionString
        {
            get
            {
                return _ConnectionString ?? (_ConnectionString = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/_connectionString.txt")));
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            filterContext.HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        }

        public ActionResult Index()
        {
            return View();
        }

        JsonResult Exec<T>(string queryFileName, int appId, DateTime? from, DateTime? to)
        {
            var fromValue = from ?? new DateTime(2013, 9, 12, 16, 0, 0);
            var toValue = to ?? DateTime.Now;

            var query = System.IO.File.ReadAllText(Server.MapPath(queryFileName));
            query = query
                .Replace("{appid}", appId.ToString())
                .Replace("{from}", fromValue.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{to}", toValue.ToString("yyyy-MM-dd HH:mm:ss"));

            using (var dx = new MobitransDataContext(ConnectionString))
            {
                var res = dx.ExecuteQuery<T>(query);
                return Json(res.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Dates(DateTime from)
        {
            return Json(new {from = from.ToString("yyyy-MM-dd HH:mm:ss")}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UsageAfterPurchase(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<VisitsUsers>("~/App_Data/usage-after-purchase.sql", appId, from, to);
        }

        public JsonResult IapEventsPerUser(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<IAPEvent>("~/App_Data/iap-events-per-user.sql", appId, from, to);
        }

        public JsonResult IapRequestsHistogram(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<RequestsUsers>("~/App_Data/iap-requests-histogram.sql", appId, from, to);
        }

        public JsonResult IapRequestsBeforePurchaseHistogram(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<RequestsUsers>("~/App_Data/iap-requests-before-purchase-histogram.sql", appId, from, to);
        }

        public JsonResult VisitsBeforePurchaseHistogram(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<VisitsUsers>("~/App_Data/visits-before-purchase-histogram.sql", appId, from, to);
        }

        public JsonResult AppUsersSourcesAtPurchase(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<SourceUsers>("~/App_Data/appuser-at-purchase.sql", appId, from, to);
        }

        public JsonResult AppUsersSources(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<SourceUsers>("~/App_Data/appuser-sources.sql", appId, from, to);
        }

        public JsonResult ViewOfPurchase(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<ViewCount>("~/App_Data/view-of-purchase.sql", appId, from, to);
        }

        public JsonResult TrialChapterVisits(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<VisitsUsers>("~/App_Data/users-visited-trial-chapter.sql", appId, from, to);
        }

        public JsonResult TrialChapterVisitsByBuyers(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<VisitsUsers>("~/App_Data/buyers-visited-trial-chapter.sql", appId, from, to);
        }

        public JsonResult QuickStats(int appId = 11, DateTime? from = null, DateTime? to = null)
        {
            return Exec<QuickStats>("~/App_Data/quick-stats.sql", appId, from, to);
        }
  


    }

    public class QuickStats
    {
        public QuickStats() { }
        public int Visits { get; set; }
        public int Users { get; set; }
        public int RegisteredUsers { get; set; }
        public int FacebookConnects { get; set; }
        public int Purchases { get; set; }

    }

    public class ViewCount
    {
        public ViewCount(){ }
        public string View { get; set; }
        public int Count { get; set; }
    }

    public class VisitsUsers
    {
        public VisitsUsers() { }
        public int Visits { get; set; }
        public int Users { get; set; }
    }

    public class RequestsUsers
    {
        public RequestsUsers() { }
        public int Requests { get; set; }
        public int Users { get; set; }
    }

    public class IAPEvent
    {
        public IAPEvent() { }
        public int Users { get; set; }
        public int Requests { get; set; }
        public int Recevies { get; set; }
        public int BuyTries { get; set; }
        public int Failures { get; set; }
        public int Purchases { get; set; }
    }

    public class SourceUsers
    {
        public SourceUsers() { }
        public string Source { get; set; }
        public int Users { get; set; }
    }
}
