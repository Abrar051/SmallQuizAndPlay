using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizMaster.Models;
using QuizStarPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Controllers
{
    public class LoginController : Controller
    {

        private readonly BasketContext _entities = new BasketContext();
        private readonly IHttpContextAccessor _contextHttp;
        string UAPROF_URL = string.Empty;
        public LoginController(IHttpContextAccessor httpContextAccessor)
        {
            _contextHttp = httpContextAccessor;
        }

        public string GetUserIP()
        {
            // var remoteIpAddress = _contextHttp.HttpContext.Connection.RemoteIpAddress;
            string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                remoteIpAddress = Request.Headers["X-Forwarded-For"];
            return remoteIpAddress.ToString();
        }
        public string GetUserAgent()
        {
            string agent = _contextHttp.HttpContext.Request.Headers["User-Agent"];
            if (!string.IsNullOrEmpty(Request.Headers["X-OperaMini-Phone-UA"]))
            {
                agent = Request.Headers["X-OperaMini-Phone-UA"];
            }
            return agent;
        }
        public string GetMSISDN() // Find out the MSISDN Number of GrameenPhone Mobile
        {
            var httpRequest = _contextHttp.HttpContext.Request;

            string sMsisdnNo = string.Empty;
            foreach (var item in httpRequest.Headers)
            {
                sMsisdnNo += item.Key + "_" + item.Value + "_______";
            }

            return sMsisdnNo;
        }
        private void GeneralInfo(out string msisdn, out string ua, out string ip, out string HS_MANUFAC, out string HS_MOD, out string HS_DIM, out string HS_OS, out string serviceName, out string cKey, out string sourceurl2)
        {
            var httpRequest = _contextHttp.HttpContext.Request;
            //msisdn = GetMSISDN();
            msisdn = _contextHttp.HttpContext.Request.Query["msisdn"];
            ViewData["msisdn"] = msisdn;
            // msisdn = _contextHttp.HttpContext.Request.Query["msisdn"];
            ua = GetUserAgent();

            ip = GetUserIP();
            HS_MANUFAC = string.Empty;
            HS_MOD = string.Empty;
            HS_DIM = string.Empty;
            HS_OS = string.Empty;
            if (ua == "Mozilla/5.0 (Linux; Android 10; SM-J400F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Mobile Safari/537.36")
            {
                HS_MANUFAC = "Samsung";
                HS_MOD = "SM-J400F";
                HS_DIM = "";
                HS_OS = "Android";
            }
            else
            {
                DD a = new Hsprofiling().PostResponse(ua);
                HS_MANUFAC = a.manufac;
                HS_MOD = a.model_name;
                HS_DIM = a.dimension;
                HS_OS = a.os;
            }

            serviceName = string.Empty;
            cKey = string.Empty;
            var sourceurl = _contextHttp.HttpContext.Request.Host;
            var sourceurl1 = _contextHttp.HttpContext.Request.Path;
            sourceurl2 = _contextHttp.HttpContext.Request.Host.ToString() + sourceurl1.ToString() + _contextHttp.HttpContext.Request.QueryString;
        }

        public void Save(string page)
        {

            string msisdn, ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, serviceName, cKey, sourceurl2;
            GeneralInfo(out msisdn, out ua, out ip, out HS_MANUFAC, out HS_MOD, out HS_DIM, out HS_OS, out serviceName, out cKey, out sourceurl2);

            //var httpRequest = _contextHttp.HttpContext.Request;
            var raw = _contextHttp.HttpContext.Request.Headers["Referer"].ToString();

            RawUrlModel rawu = new RawUrlModel()
            {
                RawUrl = raw
            };

            HttpContext.Session.SetString("rawurl", JsonConvert.SerializeObject(rawu));
            var rawUrl = "";

            try
            {
                rawUrl = JsonConvert.DeserializeObject<RawUrlModel>(HttpContext.Session.GetString("rawurl")).RawUrl;

            }
            catch (Exception ex)
            {
                //
            }
            UAPROF_URL = GetUserAgent();
            DD a = new Hsprofiling().GetResponse(UAPROF_URL);
            _entities.TblAccessBdtubeAlls.Add(new TblAccessBdtubeAll
            {
                Apn = "QuizStarWeb",
                DeviceIp = GetUserIP(),
                HsDim = a.dimension,
                HsModel = a.model_name,
                HsManufacturer = a.manufac,
                Msisdn = GetMSISDN().Length > 18 ? "Wifi" : GetMSISDN(),
                //MSISDN="8801843981414",
                Os = HS_OS,
                PortalFullnShort = "QuizStarWeb",
                ServiceRequest = page,
                SourceUrl = rawUrl,
                TimeStamp = DateTime.Now,
                UaprofileUrl = GetUserAgent()

            });
            _entities.SaveChanges();

        }

        public ActionResult Index()
        {
            Save("Login");
            return View();
        }

        [HttpGet]
        public ActionResult Index(string invoiceID, string result)
        {
            Save("Login");
            return View();
        }

    }
}
