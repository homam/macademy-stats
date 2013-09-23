using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace server.Controllers
{
    public class UpdateMobiSpyEventsController : Controller
    {
        volatile static string _ConnectionString;
        public static string ConnectionString
        {
            get 
            {
                return _ConnectionString ?? (_ConnectionString = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/_connectionString.txt")));
            }
        }

        static readonly Dictionary<string, string> Cache = new Dictionary<string, string>();

        static MobitransDataContext dx = new MobitransDataContext(ConnectionString);

        public ActionResult UpdateCountry(int from)
        {
            //using(var )
            {
                var record = dx.MobiSpy_Events.FirstOrDefault(e => 
                    7 == e.appId &&
                    e.ip != null && e.country == null && e.eventId > from);

                if (record != null)
                {
                    var country = Query(record.ip);

                    var query = string.Format("UPDATE MobiSpy_Events SET country='{0}' WHERE ip='{1}'", country, record.ip);
                    dx.CommandTimeout = 480;
                    var updatedRecords = dx.ExecuteCommand(query);


                    return new ContentResult() { Content = string.Format("Upadted={0}\nEventId={1}", updatedRecords, record.eventId), ContentType  = "text/plain"};
                }
            }

            return null;
        }

        public string Query(string ip)
        {
            string value;
            if (Cache.TryGetValue(ip, out value))
                return value;

            var client = new WebClient();
            var json = client.DownloadString("http://172.30.0.164:8081/MobiWebService.svc/json/GetCountryISOCode?ip=" + ip + "&key=RIG8UhzkrSKmy7G1P55QdivGTvrbgJ+13thnE8mjzbo=");
            try {
                dynamic res = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>(json);
                var country = res["GetCountryISOCodeResult"] as string;
                if (!string.IsNullOrEmpty(country))
                {
                    if (1 == country.Length)
                        country += "-";
                    value = country.ToLower();
                    Cache[ip] = value;
                    return value;
                }
                return null;

            } catch(Exception ex)
            {
                // eat it!
                return null;
            }
        }
    }
}
