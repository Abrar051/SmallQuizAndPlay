using FirebaseAdmin.Messaging;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuizMaster.Data.QuizMasterLiveQuizLog;
using QuizMaster.HelperClass;
using QuizMaster.Models;
using QuizMaster.Models.BkashPaymentGateWay;
using QuizMaster.Models.WapPortal;
using QuizStarPortal.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.Net.Http;
using System.Net.Http.Headers;
using Google.Apis.Logging;
using Microsoft.Build.Framework;
using System.Xml.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;





namespace QuizMaster.Controllers
{
    public class LandingPageController : Controller
    {
        
        private readonly IWebHostEnvironment _env;
        private readonly WapPortalContext  _wapPortalContext;
        private readonly BasketContext _entities = new BasketContext();
        private readonly QuizMasterLiveQuizLogContext _QuizMasterLiveQuizLogContext = new QuizMasterLiveQuizLogContext();
        private readonly IHttpContextAccessor _contextHttp;
       
        string UAPROF_URL = string.Empty;
        private IConfiguration _Configuration;
        public LandingPageController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, QuizMasterLiveQuizLogContext quizMasterLiveQuizLogContext, WapPortalContext wapPortal, IWebHostEnvironment env)
        {
            _contextHttp = httpContextAccessor;
            _Configuration = configuration;
            _QuizMasterLiveQuizLogContext = quizMasterLiveQuizLogContext;
            _wapPortalContext = wapPortal;
       
            _env = env;
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
            msisdn =  _contextHttp.HttpContext.Request.Query["msisdn"];
            if (msisdn != null && msisdn.Contains("Error"))
            {
                var ms = msisdn.Split("Error");
                var res = ms[0].Replace(",","");
                msisdn = res == "" ? null : res;
            }
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
        //[HttpGet]
        //public async Task<IActionResult> SaveCapmTimeLog(string msisdn, string ckey, string cname, string theme, string srcurl)
        //{
        //    try
        //    {


        //        string connectionString = "server=103.134.68.67;database=Basket;user=sa;pwd=adplayVu@1234;";

        //        var queryString = _contextHttp.HttpContext.Request.QueryString.Value.ToString();

        //        var splitedString = queryString.Split("srcurl=");

        //        if (string.IsNullOrWhiteSpace(msisdn))
        //        {
        //            msisdn = "";
        //        }

        //        string ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, serviceName, cKey, sourceurl2;
        //        //GeneralInfo(out msisdn, out ua, out ip, out HS_MANUFAC, out HS_MOD, out HS_DIM, out HS_OS, out serviceName, out cKey, out sourceurl2);


        //        var httpRequest = _contextHttp.HttpContext.Request;
        //        //var rawurl = _contextHttp.HttpContext.Request.Url.ToString();
        //        //var rawurl = _contextHttp.HttpContext.Request.Path.ToString();
        //        var rawurl = _contextHttp.HttpContext.Request.Host.ToString();

        //        // _contextHttp.HttpContext.Session.SetString("rawurl", srcurl);
        //        //_httpContextAccessor.HttpContext.Session.SetString("rawurl", splitedString.Length > 1 ? splitedString[1] : srcurl);

        //        UAPROF_URL = GetUserAgent();
        //        DD a = new Hsprofiling().GetResponse(UAPROF_URL);

        //        await _newContext.TblAllLogQuizBkashTimes.AddAsync(new TblAllLogQuizBkashTime
        //        {

        //            Apn = "QuizMaste",
        //            DeviceIp = GetUserIP(),
        //            HsDim = a.dimension,
        //            HsModel = a.model_name,
        //            HsManufacturer = a.manufac,
        //            Msisdn = msisdn,
        //            //Os = HS_OS,
        //            PortalFullnShort = "QuizMaster",
        //            ServiceRequest = "QuizMaster",
        //            SourceUrl = splitedString.Length > 1 ? splitedString[1] : srcurl,
        //            TimeStamp = DateTime.Now,
        //            UaprofileUrl = GetUserAgent(),
        //            ThemeId = theme,
        //            Ckey = ckey,
        //            CamName = cname

        //        });
        //        await _newContext.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Ok(true);

        //}

        public JsonResult Savemsisdn(string msisdn)
        {
            if (string.IsNullOrWhiteSpace(msisdn))
            {
                return null;
            }

            string ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, serviceName, cKey, sourceurl2;
            //GeneralInfo(out msisdn, out ua, out ip, out HS_MANUFAC, out HS_MOD, out HS_DIM, out HS_OS, out serviceName, out cKey, out sourceurl2);



            var httpRequest = _contextHttp.HttpContext.Request;
            //var rawurl = _contextHttp.HttpContext.Request.Url.ToString();
            //var rawurl = _contextHttp.HttpContext.Request.Path.ToString();
            var rawurl = _contextHttp.HttpContext.Request.Host.ToString();
            //_contextHttp.HttpContext.Session.SetString("rawurl", rawurl);
            UAPROF_URL = GetUserAgent();
            DD a = new Hsprofiling().GetResponse(UAPROF_URL);
            _entities.TblMsisdnQuizBkashes.Add(new TblMsisdnQuizBkash
            {

                Apn = "QuizStarWeb", 
                DeviceIp = GetUserIP(),
                HsDim = a.dimension,
                HsModel = a.model_name,
                HsManufacturer = a.manufac,
                Msisdn = msisdn,
                //MSISDN="8801843981414",
                //Os = HS_OS,
                PortalFullnShort = "QuizStarBkash",
                ServiceRequest = "QUIZBkash",
                //SOURCE_URL = rawurl,
                SourceUrl = HttpContext.Session.GetString(rawurl),
                TimeStamp = DateTime.Now,
                UaprofileUrl = GetUserAgent()

            });
            _entities.SaveChanges();

            try
            {
                var nid = _contextHttp.HttpContext.Session.GetString("nid");
                if (!string.IsNullOrWhiteSpace(nid))
                {
                    var exists = _entities.TblQuizStarSubscriptionAppInApps.Where(x => x.Fbid == nid).FirstOrDefault();
                    if (exists != null)
                    {
                        exists.Fbid = msisdn;
                        _entities.SaveChanges();
                    }
                }
            }
            catch { }


            var data = _entities.TblQuizStarSubscriptionAppInApps.Any(x => x.Fbid == msisdn && x.RegStatus.ToString() == "1");
            if (data)
            {
                return Json(new { result = true });

            }
            else
            {

                return Json(new { result = false });
            }


        }
       

        [HttpGet]

        public async Task<IActionResult> SaveCapmLog(string msisdn, string ckey, string cname, string theme, string srcurl)
        {

            //string script = $"alert('Theme Value: {theme}');";
            //await HttpContext.Response.WriteAsync("<script>" + script + "</script>");
            try
            {
               

                var queryString = _contextHttp.HttpContext.Request.QueryString.Value.ToString();

                var splitedString = queryString.Split("srcurl=");
                if (string.IsNullOrWhiteSpace(msisdn))
                {
                    msisdn = "";
                }

                string ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, serviceName, cKey, sourceurl2;
                //GeneralInfo(out msisdn, out ua, out ip, out HS_MANUFAC, out HS_MOD, out HS_DIM, out HS_OS, out serviceName, out cKey, out sourceurl2);



                var httpRequest = _contextHttp.HttpContext.Request;
                //var rawurl = _contextHttp.HttpContext.Request.Url.ToString();
                //var rawurl = _contextHttp.HttpContext.Request.Path.ToString();
                var rawurl = _contextHttp.HttpContext.Request.Host.ToString();

                // _contextHttp.HttpContext.Session.SetString("rawurl", srcurl);
                _contextHttp.HttpContext.Session.SetString("rawurl", splitedString.Length > 1 ? splitedString[1] : srcurl);

                UAPROF_URL = GetUserAgent();
                DD a = new Hsprofiling().GetResponse(UAPROF_URL);

                await _entities.TblAllLogQuizBkashes.AddAsync(new TblAllLogQuizBkash
                {

                    Apn = "QuizMaster",
                    DeviceIp = GetUserIP(),
                    HsDim = a.dimension,
                    HsModel = a.model_name,
                    HsManufacturer = a.manufac,
                    Msisdn = msisdn,
                    //Os = HS_OS,
                    PortalFullnShort = "QuizMaster",
                    ServiceRequest = "QuizMaster",
                    SourceUrl = splitedString.Length > 1 ? splitedString[1] : srcurl,
                    TimeStamp = DateTime.Now,
                    UaprofileUrl = GetUserAgent(),
                    ThemeId = theme,
                    Ckey = ckey,
                    CamName = cname

                });
                await _entities.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            
            return Ok(true);

        }

        public JsonResult SaveThemeInfo(string msisdn, string theme)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(msisdn))
                {
                    return null;
                }

                string ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, serviceName, cKey, sourceurl2;
                //GeneralInfo(out msisdn, out ua, out ip, out HS_MANUFAC, out HS_MOD, out HS_DIM, out HS_OS, out serviceName, out cKey, out sourceurl2);



                var httpRequest = _contextHttp.HttpContext.Request;
                //var rawurl = _contextHttp.HttpContext.Request.Url.ToString();
                //var rawurl = _contextHttp.HttpContext.Request.Path.ToString();
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
                _entities.TblAllLogQuizBkashes.Add(new TblAllLogQuizBkash
                {

                    Apn = "QuizMaster",
                    DeviceIp = GetUserIP(),
                    HsDim = a.dimension,
                    HsModel = a.model_name,
                    HsManufacturer = a.manufac,
                    Msisdn = msisdn,
                    //MSISDN="8801843981414",
                    //Os = HS_OS,
                    PortalFullnShort = "QuizMaster",
                    ServiceRequest = "QuizMaster",
                    //SOURCE_URL = rawurl,
                    SourceUrl = rawUrl,
                    TimeStamp = DateTime.Now,
                    UaprofileUrl = GetUserAgent(),
                    ThemeId = theme

                }); ;
                _entities.SaveChanges();


                try
                {
                    var nid = _contextHttp.HttpContext.Session.GetString("nid");
                    if (!string.IsNullOrWhiteSpace(nid))
                    {
                        var exists = _entities.TblQuizStarSubscriptions.Where(x => x.Fbid == nid).FirstOrDefault();
                        if (exists != null)
                        {
                            exists.Fbid = msisdn;
                            _entities.SaveChanges();
                        }
                    }
                }
                catch { }


                var data = _entities.TblQuizStarSubscriptionAppInApps.Any(x => x.Fbid == msisdn);
                if (data)
                {
                    return Json(new { result = true });
                }
                else
                {

                    return Json(new { result = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false });
            }

        }

        public void Save(string page)
        {
            try
            {
                string msisdn, ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, serviceName, cKey, sourceurl2;
                //GeneralInfo(out msisdn, out ua, out ip, out HS_MANUFAC, out HS_MOD, out HS_DIM, out HS_OS, out serviceName, out cKey, out sourceurl2);


                string nid = string.Empty;
                string ckey = string.Empty;
                try
                {
                    nid = _contextHttp.HttpContext.Request.Query["nid"].ToString();
                    _contextHttp.HttpContext.Session.SetString(nid, nid);

                }
                catch { nid = ""; }
                if (page.ToLower() == "index")
                {
                    try
                    {
                        ckey = _contextHttp.HttpContext.Request.Query["ckey"].ToString();
                        _contextHttp.HttpContext.Session.SetString(ckey, ckey);
                    }
                    catch
                    {
                        ckey = page;
                    }
                }
                else
                {
                    ckey = page;
                }

                var httpRequest = _contextHttp.HttpContext.Request;
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
                //_contextHttp.HttpContext.Session.SetString("rawurl", rawurl);
                UAPROF_URL = GetUserAgent();
                DD a = new Hsprofiling().GetResponse(UAPROF_URL);
                _entities.TblAccessBdtubeAlls.Add(new TblAccessBdtubeAll
                {
                    Apn = "QuizStarWeb",
                    DeviceIp = GetUserIP(),
                    HsDim = a.dimension,
                    HsModel = a.model_name,
                    HsManufacturer = a.manufac,
                    //Msisdn = GetMSISDN().Length > 18 ? "Wifi" : GetMSISDN(),
                    Msisdn = "",
                    //MSISDN="8801843981414",
                    //Os = HS_OS,
                    PortalFullnShort = "QuizStarWeb",
                    ServiceRequest = page,
                    SourceUrl = rawUrl,
                    TimeStamp = DateTime.Now
                    //,UaprofileUrl = GetUserAgent()

                });
                _entities.SaveChanges();

            }
            catch (Exception ex)
            {
                throw;
            }
            

        }

       

        public ActionResult Index()
        {
            Save("Index");
            return View();
        }

        public ActionResult LandingPageV3()
        {
            Save("Index");
            return View();
        }

        public ActionResult OldIndex()
        {
            Save("OldIndex");
            return View();
        }

        public ActionResult PlayBoard()
        {
            Save("PlayBoard");
            return View();
        }
        public ActionResult JhotpotRound()
        {
            Save("JhotpotRound");
            return View();
        }

        public ActionResult JhotpotRoundWC()
        {
            Save("JhotpotRoundWC");
            return View();
        }

        public ActionResult JhotpotRoundBT()
        {
            Save("JhotpotRoundBT");
            return View();
        }
        public ActionResult JhotpotRoundAudio(string serviceName)
        {
            List<AudioQuestion> audioQuestions = GetRandomAudioQuestionsFromDatabase(serviceName);

            if (audioQuestions != null && audioQuestions.Count > 0)
            {
              
                Save("JhotpotRoundAudio");

        
                return View(audioQuestions);
            }
            else
            {
               
                Console.WriteLine("No audio questions retrieved from the database.");

                return View();
            }
        }



        public ActionResult DeviceCheckingApi()
        {
            //Save("JhotpotRound");
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> KidStarLeaderBoard(string serviceName, string type = null)
        {
            //ViewBag.Panel = "Leaderboard";
            return View(await GetWinnerboardForSpecialTournament(serviceName, type));
        }

        public ActionResult KidStarWinnerBoardPreviousDay (string serviceName)
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> KidStarWinnerBoardPreviousDay(string serviceName, string type = null)
        {
            //ViewBag.Panel = "Leaderboard";
            return View(await GetWinnerboardForSpecialTournamentPreviousDay(serviceName, type));
        }

        public async Task<List<Leaderboard>> GetWinnerboardForSpecialTournament(string serviceName, string type = null)
        {
            try
            {
                var dataList = await _wapPortalContext.GameSpecificLeaderboards.FromSqlRaw("exec [dbo].[sp_MultiTournamentDailyLeaderBoard] @ServiceName", new SqlParameter("@ServiceName", serviceName)).ToListAsync();
                //var dataList = GetDailyLeaderBoard();
                var data = dataList.Where(x => x.MSISDN != null).Select(x => new Leaderboard
                {
                    Name = x.Name,
                    MSISDN = (x.MSISDN ?? ""),
                    Point = x.Point,
                    Rank = x.Rank,
                    TimeCount = string.Format("{0:D2} m :{1:D2} s", TimeSpan.FromSeconds(Int32.Parse(x.TimeCount)).Minutes, TimeSpan.FromSeconds(Int32.Parse(x.TimeCount)).Seconds),
                    //Level = x.Level,
                    Score = x.Score
                }).ToList();
                return data;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<List<Leaderboard>> GetWinnerboardForSpecialTournamentPreviousDay(string serviceName, string type = null)
        {
            try
            {
                var dataList = await _wapPortalContext.GameSpecificLeaderboards.FromSqlRaw("exec [dbo].[sp_MultiTournamentDailyLeaderBoard_PreviousDay] @ServiceName", new SqlParameter("@ServiceName", serviceName)).ToListAsync();
                //var dataList = GetDailyLeaderBoard();
                var data = dataList.Where(x => x.MSISDN != null).Select(x => new Leaderboard
                {
                    Name = x.Name,
                    MSISDN = (x.MSISDN ?? ""),
                    Point = x.Point,
                    Rank = x.Rank,
                    TimeCount = string.Format("{0:D2} m :{1:D2} s", TimeSpan.FromSeconds(Int32.Parse(x.TimeCount)).Minutes, TimeSpan.FromSeconds(Int32.Parse(x.TimeCount)).Seconds),
                    //Level = x.Level,
                    Score = x.Score
                }).ToList();
                return data;
            }
            catch (Exception ex)
            {

            }

            return null;
        }


        public async Task< ActionResult> LeaderBoard()
        {
            //Save("LeaderBoard");

            var result = new WinnerListModel();

            
            //var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Bkash_Result_Jhotpot] ").ToListAsync();
            
            //result.dailyWinnerList = data;
            //result.HappyHourWinnerList = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Bkash_Result_Jhotpot_HappyHours] ").ToListAsync();


            ////////////

            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(_Configuration.GetConnectionString("DefaultConnection")))
            {
                //SqlCommand sqlComm = new SqlCommand("sp_Bkash_Result_DailyWinners", conn);
                SqlCommand sqlComm = new SqlCommand("sp_Bkash_Result_DailyWinnersfor_23_08_2022", conn);
                //sqlComm.Parameters.AddWithValue("@Start", StartTime);
                //sqlComm.Parameters.AddWithValue("@Finish", FinishTime);
                //sqlComm.Parameters.AddWithValue("@TimeRange", TimeRange);

                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                result.dailyWinnerList = ConvertData.ConvertDataTable<sp_Bkash_Result_Jhotpot_Result>(ds.Tables[2]);
                result.HappyHourWinnerList = ConvertData.ConvertDataTable<sp_Bkash_Result_Jhotpot_Result>(ds.Tables[0]);
                result.SecondHappyHourWinnerList = ConvertData.ConvertDataTable<sp_Bkash_Result_Jhotpot_Result>(ds.Tables[1]);

                result.dailyWinnerList.RemoveAll(a => result.HappyHourWinnerList.Exists(w => w.fbid == a.fbid && w.name == a.name));
                result.dailyWinnerList.RemoveAll(a => result.SecondHappyHourWinnerList.Exists(w => w.fbid == a.fbid && w.name == a.name));
                if (result.SecondHappyHourWinnerList != null && result.SecondHappyHourWinnerList.Count != 0)
                {
                    result.HappyHourWinnerList.AddRange(result.SecondHappyHourWinnerList);
                }
                
                result.currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            }

            ///////////////
            return View(result);
        }


        public async Task<ActionResult> LeaderBoard_new(string type, string ServiceName)
        {
            
            Save("LeaderBoard_new");

            sp_Bkash_Result_Jhotpot_Result weeklyList = new sp_Bkash_Result_Jhotpot_Result();

            string Service = ServiceName;


            if (type == "DailyQuiz")
            {
               
                var data = await _entities.sp_Bkash_Result_Jhotpot_Results
                    .FromSqlRaw("EXEC [sp_Bkash_Result_Daily_MultiTournament] @serviceName", new SqlParameter("@serviceName", Service))
                    .ToListAsync();

                return View(data);
            }

            else if (type == "WeeklyWinner")
            {
           
                var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Bkash_LiveQuiz_DailyWinnersforWeeklyWinner] ").ToListAsync();
                return View(data);
            }
    
            else if (type == "DailyWinner")
            {
                string Service1 = ServiceName;

                var data = await _entities.sp_Bkash_Result_Jhotpot_Results
                  .FromSqlRaw("EXEC [sp_Bkash_Result_Daily_MultiTournamentPreviousDay] @serviceName", new SqlParameter("@serviceName", Service1))
                  .ToListAsync();
                
                switch (Service1)
                {
                    case "QuizMaster":
                        return View(data.Take(40));
                    case "QuizMaster-MultiService1":
                        return View(data.Take(70));
                    case "QuizMaster-MultiService2":
                        return View(data.Take(60));
                    case "QuizMaster-MultiService3":
                        return View(data.Take(30));
                    default:
                        return View(data.Take(40));
                }

        
            }
            else if (type == "WorldCup")
            {
         
                var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Bkash_Result_DailyWC] ").ToListAsync();
          
                return View(data.Take(75));
            }
            else if (type == "SuperHourWinner")
            {
                var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Bkash_Result_DailyWinnersforSupperHours] ").ToListAsync();
                return View(data);
            }
            else if (type == "FunFriday")
            {
                var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Bkash_Result_forFunFridayWinner] ").ToListAsync();
                return View(data);
            }
            else if (type == "WorldCupQuizWinner")
            {
                var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Bkash_Result_forWorldCupQuizWinner] ").ToListAsync();
                return View(data);
            }
            else if (type == "WorldCupDailyLeaderBoard")
            {
                //var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Bkash_Result_forWorldCupDailyLeaderBoard] ").ToListAsync();
                var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw(" select * from Func_Bkash_Result_forWorldCupDailyLeaderBoard()").ToListAsync();
                return View(data);
            }
            else if (type == "WorldCupWeeklyLeaderBoard")
            {
                var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Bkash_Result_forWorldCupWeeklyLeaderBoard] ").ToListAsync();
                return View(data);
            }
            else if (type == "WordGameAll")
            {
                var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Word_Result_Daily] ").ToListAsync();
                return View(data);
            }

            else if (type == "WordGame")
            {
                //var data1 = WordStarDailyWinner("sp_Bkash_Result_forGameLeaderBoard_ForSpecificTopNumber");
                var data = await _entities.sp_Bkash_Result_Jhotpot_Results.FromSqlRaw("EXEC [sp_Word_Result_Daily] ").ToListAsync() ;
                //var data = WordStarDailyWinner("sp_Word_Result_Daily");
                return View(data.Take(75) );
            }

            else if (type == "QuizMasterWeekly")
            {
                var unionList = CommonNumber().Take(3);
                var data = MapToJhotpotResult(unionList.ToList());
                return View(data);
            }

            var datalist = new List<sp_Bkash_Result_Jhotpot_Result>();
            return View(datalist);
        }


        ////////
        ///Seperate method for wordstar daily winner using ado.net 
        ///////
        
        public object WordStarDailyWinner (string procedureName)
        {
            List<sp_Bkash_Result_Jhotpot_Result> wordResults = new List<sp_Bkash_Result_Jhotpot_Result>();
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=103.134.68.67;Initial Catalog=Basket;Persist Security Info=True;User ID=sa;Password=adplayVu@1234"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable resultTable = new DataTable();
                            resultTable.MinimumCapacity = 100;
                            adapter.Fill(resultTable);
                            return resultTable;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }






        public List<Breaktimequiz> BreaktimeQuizQuestion()
        {
            //DateTime date = DateTime.Now;
            //string url = "select Question from Basket.dbo.tbl_Breaktimequiz where CAST(Date as date) = CAST(" + date + " as date)";
            //var data = _entities.Breaktimequizzes.FromSqlRaw(url).ToListAsync();
            //return null;
            DateTime date = DateTime.Now.Date; // Get the current date without time
            var questions = _entities.Breaktimequizzes
                .Where(b => b.Date.Date == date)
                .Select(b => new Breaktimequiz
                {
                    Id = b.Id,
                    Date = b.Date,
                    Question = b.Question
                })
                .ToList();



            return questions;
        }

        /// <summary>
        /// New leader board calculation
        /// </summary>
        /// Author - Abrar Shahriar
        /// <returns></returns>

        public List<WeeklyResultModel> WeeklyResult (string procedureName)
        {
            DataTable dataTable = new DataTable();
            List<WeeklyResultModel> quizResults = new List<WeeklyResultModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=103.134.68.67;Initial Catalog=Basket;Persist Security Info=True;User ID=sa;Password=adplayVu@1234"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable resultTable = new DataTable();
                            resultTable.MinimumCapacity = 150;
                            adapter.Fill(resultTable);
                            // Return the resultTable
                            foreach (DataRow row in resultTable.Rows)
                            {
                                WeeklyResultModel result = new WeeklyResultModel
                                {
                                    Name = row[0].ToString(),
                                    FbId = row[1].ToString(),
                                    Round = row[2].ToString(),
                                    Time = row[3].ToString(),
                                    RightAns = row[4].ToString()
                                };

                                quizResults.Add(result);
                            }
                            return quizResults;
                        }
                    }
                }
            }
     
            catch (Exception ex)
            {
                return null;
            }
            

        }

        public List<WeeklyResultModel> CommonNumber ()
        {
            List<WeeklyResultModel> unionList = new List<WeeklyResultModel>();
            var QuizWeeklyList = WeeklyResult("sp_Bkash_Result_OnlyQuizResultWeekly");
            var WordStarWeeklyList = WeeklyResult("sp_Bkash_Result_OnlyWordPuzzleResult");
            foreach (var item in WordStarWeeklyList)
            {
                unionList.Add(item);
            }
            foreach (var item in QuizWeeklyList)
            {
                var existingItem = unionList.Find(item1 => item1.FbId == item.FbId);
                if (existingItem != null) {
                    existingItem.Round = (Int32.Parse(existingItem.Round) + Int32.Parse(item.Round)).ToString();
                    existingItem.Time = (Int32.Parse(existingItem.Time) + Int32.Parse(item.Time)).ToString();
                    existingItem.RightAns = (Int32.Parse(existingItem.RightAns) + Int32.Parse(item.RightAns)).ToString();
                }
                else
                {
                    unionList.Add(item);
                }
            }
            unionList = unionList.OrderByDescending(item => int.Parse(item.RightAns)).ToList();
            return unionList;
        }




        public List<sp_Bkash_Result_Jhotpot_Result> MapToJhotpotResult(List<WeeklyResultModel> unionList)
        {
            List<sp_Bkash_Result_Jhotpot_Result> mappedList = new List<sp_Bkash_Result_Jhotpot_Result>();

            foreach (var item in unionList)
            {
                sp_Bkash_Result_Jhotpot_Result result = new sp_Bkash_Result_Jhotpot_Result();
                result.name = item.Name;
                result.fbid = item.FbId;
                result.round = Int32.Parse(item.Round);
                result.Time = Int32.Parse(item.Time) ;
                result.RightAns = Int32.Parse(item.RightAns);

                mappedList.Add(result);
            }

            return mappedList;
        }



        public async Task<ActionResult> Tab_layout()
        {
            return View();
        }

        public async Task<ActionResult> KidsLeaderBoard()
        {
            return View();
        }

        public async Task<ActionResult> KidsWinnerBoard()
        {
            return View();
        }

        public async Task<ActionResult> Tab_layoutForDSBoard()
        {
            return View();
        }

        public async Task<ActionResult> LeaderBoardChoice()
        {
            return View();
        }

        public async Task<ActionResult> KidsGame()
        {
            return View();
        }

        public async Task<ActionResult> WinnerBoardChoice()
        {
            return View();
        }
        public ActionResult MyScore()
        {
            Save("MyScore");
            return View();
        }

        public ActionResult DownTimePage()
        {
            return View();
        }

        public ActionResult LiveQuizVideo()
        {
            //Save("LiveQuizVideo");

            //var LiveQuizInfo = GettingLiveQuizInformation();

            //if (LiveQuizInfo != null)
            //{

            //}


            return View();
        }

        public ActionResult LiveQuizVideoV2()
        {
            //Save("LiveVideoButtonClick");

            return View();
        }

        public ActionResult LiveQuizVideoV3()
        {
            //Save("LiveQuizVideoV3");

            return View();
        }


        //[HttpGet]
        //public async Task<JsonResult> GettingLiveQuizInformation()
        //{
        //    try
        //    {
        //        var CurentTime = new SqlParameter("@CurentTime", DateTime.Now);

        //        var data = await _entities.LiveVideoQuizInformationModels.FromSqlRaw("exec sp_GetUsers @emailAddress", CurentTime)
        //                    .ToListAsync();

        //        return Json(new { result = data });
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Json(new { result = false });

        //}

        [HttpGet]
        public async Task<IActionResult> GettingLiveQuizInformation()
        {

            try
            {
                var CurentTime = new SqlParameter("@CurentTime", DateTime.Now);

                var data = await _entities.LiveVideoQuizInformationModels.FromSqlRaw("exec GetLiveVideoQuizInformation @CurentTime", CurentTime)
                            .ToListAsync();

                //return Json(new { result = data });
                //if (data.Count > 0)
                //{
                    //for (int i = 0; i < data.Count; i++)
                    //{
                        //if (data[i].VideoName.Contains("https://wap.shabox.mobi/CMS/Assets/video/liveQuiz/") == false)
                        //{
                            //data[i].VideoName = "https://wap.shabox.mobi/CMS/Assets/video/liveQuiz/" + data[i].VideoName;
                        //}
                    //}
                //}
                

                return Ok(new { result = data});
            }
            catch (Exception ex)
            {

            }
            return Ok(new { result = false });

        }

        [HttpGet]
        public async Task<IActionResult> GettingLiveQuizInformationV3()
        {

            try
            {
                var CurentTime = new SqlParameter("@CurentTime", DateTime.Now);

                var data = await _entities.LiveVideoQuizInformationModels.FromSqlRaw("exec GetLiveVideoQuizInformationV3 @CurentTime", CurentTime)
                            .ToListAsync();

                //return Json(new { result = data });
                return Ok(new { result = data });
            }
            catch (Exception ex)
            {

            }
            return Ok(new { result = false });

        }



        public ActionResult Entertainment()
        {
            Save("Entertainment");
            return View();
        }

        public ActionResult Settings()
        {
            Save("Settings");
            return View();
        }

        public ActionResult Terms()
        {
            Save("Terms");
            return View();
        }

        public ActionResult Help()
        {
            Save("Help");
            return View();
        }

        public ActionResult Prizes()
        {
            Save("Prizes");
            return View();
        }

        public ActionResult GameRules()
        {
            //Save("Prizes");
            return View();
        }

        //Check Daily QuizStar Status
        public JsonResult CDQS()
        {
            return Json(new { result = false });
        }

        //Check Jgotpot Status
        public JsonResult CJS()
        {
            return Json(new { result = false });
        }

        public JsonResult JR()
        {
            return Json(new { result = false });
        }

        public JsonResult Status()
        {
            var time = _entities.TblDailyQuizStartTimes.FirstOrDefault();

            var date = time.Date.ToString();
            int Hour = int.Parse(time.Hour);
            int Min = int.Parse(time.Minute);
            var nextPlayDate = DateTime.Parse(date).AddHours(Hour).AddMinutes(Min);
            var gg = nextPlayDate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            return Json(new { result = gg });
        }

        public JsonResult Instrunction(string inst)
        {
            var instr = _entities.TblQpTerms.Where(x => x.Type == inst).Select(x => new { Message = x.Message }).FirstOrDefault();
            return Json(new { result = instr });
        }

        public JsonResult SubmitAnswer(string fbid, string msisdn, int qId, string response, int timeTaken)
        {
            var data = _entities.TblDailyAppResponses.Add(
                new TblDailyAppResponse
                {
                    Fbid = fbid,
                    Msisdn = msisdn,
                    Qid = qId,
                    Timetaken = timeTaken,
                    Response = response,
                    Level = 1,
                    Timestamp = DateTime.Now,
                    Type = "Web"
                });
            _entities.SaveChanges();
            return Json(new { result = "success" });
        }

        public async Task< JsonResult> IsSubscribed(string fbid)
        {

            using (var db=new BasketContext())
            {
                try
                {
                    if (fbid != null && fbid != "" && fbid != "null")
                    {
                        var result = await db.TblQuizStarSubscriptionAppInApps.AnyAsync(x => x.Fbid == fbid && x.RegStatus == 1);
                        return Json(new { result = result });
                    }
                }
                catch (Exception ex)
                {

                }

                return Json(new { result = "" });


            }

        }

        public async Task<JsonResult> IsSubscribedForMultiTournament(string fbid , string serviceName)
        {

            if (serviceName == null || serviceName == "undefined")
            {
                serviceName = "QuizMasterAPPINAPP";
            }

            using (var db = new BasketContext())
            {
                try
                {
                    bool result; 

                    if (fbid != null && fbid != "" && fbid != "null" && serviceName != null)
                    {
                        
                            result = await db.TblQuizStarSubscriptionAppInApps.AnyAsync(x => x.Fbid == fbid && x.RegStatus == 1);

                            return Json(new { result = result });
             
                    }
                }
                catch (Exception ex)
                {

                }

                return Json(new { result = "" });


            }

        }

        public async Task<JsonResult> checkIsSubscribedWithSubReqId (string subReqId)
        {

            using (var db = new BasketContext())
            {
                try
                {
                    if (subReqId != null && subReqId != "" && subReqId != "null")
                    {
                        var result = await db.TblQuizStarSubscriptionAppInApps.AnyAsync(x => x.SubscriptionRequestId == subReqId && x.RegStatus == 1);
                        return Json(new { result = result });
                    }
                }
                catch (Exception ex)
                {

                }

                return Json(new { result = "" });


            }

        }


        public bool checkIsSubscribedWithMsisdn(string msisdn)
        {
            if (msisdn.StartsWith("88") == false)
            {
                msisdn = "88" + msisdn;
            }

            using (var db = new BasketContext())
            {
                try
                {
                    if (msisdn != null && msisdn != "" && msisdn != "null")
                    {
                        var result = db.TblQuizStarSubscriptions.AnyAsync(x => x.Msisdn == msisdn && x.RegStatus == 1);
                        return result.Result;
                    }
                }
                catch (Exception ex)
                {

                }

                return false;


            }

        }




        public async Task<JsonResult> GettingTermRules(string type)
        {


            if (type != null)
            {
                try
                {

                    var result = await _entities.TblQpTerms.Where(x => x.Type == type).FirstOrDefaultAsync();

                    return Json(new { term = result.Message });

                }
                catch (Exception ex)
                {
                    
                }
                
            }
            return Json(new { result = "" });
        }

        public async Task<JsonResult> CheckRecurringPaymentStatus(string fbid)
        {

            using (var db = new BasketContext())
            {
                try
                {
                    if (fbid != null && fbid != "" && fbid != "null")
                    {
                        var SubReqId = await db.TblQuizStarSubscriptionAppInApps.Where(x => x.Fbid == fbid && x.RegStatus == 1).Select(x => x.SubscriptionRequestId).FirstOrDefaultAsync();

                        if (SubReqId != null && SubReqId != "")
                        {
                            using (var _bkashContext = new BkashPaymentGateWayContext())
                            {
                                var PaymentStat = await _bkashContext.WebHookSubscriptionDatasV3s.Where(x => x.SubscriptionRequestId == SubReqId && x.Type == "PAYMENT").OrderByDescending(x => x.TimeStamp).FirstOrDefaultAsync();

                                if (PaymentStat.PaymentStatus == "FAILED_PAYMENT")
                                {
                                    return Json(new { result = "false" });
                                }
                                else
                                {
                                    return Json(new { result = "true" });
                                }

                            }
                        }
                        else
                        {
                            return Json(new { result = "Subscription Request Id not found" });
                        }

                    }
                }
                catch (Exception ex)
                {
                    return Json(new { result = ex.Message });
                }

                return Json(new { result = "" });


            }

        }


        public async Task<JsonResult> CheckRecurringPaymentStatusNew (string fbid)
        {
            using (var db = new BasketContext())
            {
                try
                {
                    if (fbid != null && fbid != "" && fbid != "null")
                    {
                        var SubReqId = await db.TblQuizStarSubscriptions.Where(x => x.Fbid == fbid && x.RegStatus == 1).Select(x => x.SubscriptionRequestId).FirstOrDefaultAsync();

                        /// get subscription request id using fbid 

                        if (SubReqId != null && SubReqId != "")
                        {
                            using (var _bkashContext = new BkashPaymentGateWayContext())
                            {
                                var PaymentStat = await _bkashContext.WebHookSubscriptionDatasV3s.Where(x => x.SubscriptionRequestId == SubReqId && x.Type == "PAYMENT").OrderByDescending(x => x.TimeStamp).FirstOrDefaultAsync();



                                if (PaymentStat.PaymentStatus == "FAILED_PAYMENT")
                                {
                                    return Json(new { result = "false" });
                                }
                                else
                                {
                                    return Json(new { result = "true" });
                                }

                            }
                        }
                        else
                        {
                            return Json(new { result = "Subscription Request Id not found" });
                        }

                    }
                }
                catch (Exception ex)
                {
                    return Json(new { result = ex.Message });
                }

                return Json(new { result = "" });


            }
        }



            public ActionResult GameTutorial()
        {
            //Save("ChallangeQuizCatSelecttion");
            return View();
        }

        public async Task<JsonResult> ChallangeQuizRandomCats()
        {
            var data = await _entities.QuizQuestionCategorySelections.FromSqlRaw("EXEC [sp_Rand5ChallangeQuizCatSelection] ").ToListAsync();
            var res = data.OrderBy(x => Guid.NewGuid()).Take(5).Distinct().ToList();
            return Json(new { result = res });
        }


        [HttpPost]

        public async Task<IActionResult> SendNotificationToAll([FromBody] FCMNotification fCMNotification)
        {
            try
            {
                var topic = "SendAll";
                //var topic = "UpdatedVersionV1";
                var message = new Message()
                {
                    Notification = new Notification()
                    {
                        Title = fCMNotification.Title,
                        Body = fCMNotification.Body,

                    },
                    Webpush = new WebpushConfig()
                    {
                        FcmOptions = new WebpushFcmOptions()
                        {
                            Link = "https://quizmaster.shabox.mobi/LandingPage/Leaderboard"
                        }
                    },
                    //Data = new Dictionary<string, string>()
                    //{
                    //    { "score", "850" },
                    //    { "time", "2:45" },
                    //},
                    Topic = topic,
                    //
                    Android = new AndroidConfig()
                    {
                        TimeToLive = TimeSpan.FromHours(1),
                        Notification = new AndroidNotification()
                        {
                            Icon = "stock_ticker_update",
                            Color = "#f45342",
                        },
                    },
                    Apns = new ApnsConfig()
                    {
                        Aps = new Aps()
                        {
                            Badge = 42,
                        },
                    },
                };

                // Send a message to the devices subscribed to the provided topic.
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                // Response is a message ID string.
                Console.WriteLine("Successfully sent message: " + response);

            }
            catch (Exception ex)
            {

            }
            return Ok(true);
        }

        [HttpGet]
        public async Task<IActionResult> NotificationSendingFromCRM( string ID, string ServiceName, string TIME_STAMP)
        {
            decimal id = Convert.ToDecimal(ID);
            var data = await _entities.TblAllLogPushNotificationMessageSendings.Where(x => x.Id == id && x.ServiceName == ServiceName).FirstOrDefaultAsync();
            
            
            if (data != null)
            {
                try
                {
                    var ProcedureName = await _entities.TblPushNotificationUserBases.Where(x => x.UserBase == data.UserBase).Select(x => x.ProcedureName).FirstOrDefaultAsync();
                    var usersRegistrationtokens = new List<string>();
                    var NewusersRegistrationtokens = await _entities.Sp_PushNotificationForQuizMasterWinners.FromSqlRaw("exec "+ ProcedureName).ToListAsync(); 
                    usersRegistrationtokens = NewusersRegistrationtokens.Select(x => x.PushNotificationToken).ToList();
                    var tokenSendingCount = 0;
                    if ((Convert.ToDouble(usersRegistrationtokens.Count) / 1000) > 0 && (Convert.ToDouble(usersRegistrationtokens.Count) / 1000) <= 1)
                    {
                        tokenSendingCount = 1;
                    }
                    else if ((Convert.ToDouble(usersRegistrationtokens.Count) / 1000) > 1)
                    {
                        double value = usersRegistrationtokens.Count / 1000;
                        tokenSendingCount = (int)Math.Ceiling(value);
                    }

                    for (int i = 1; i <= tokenSendingCount; i++)
                    {
                        var topic = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd");

                        var regToken = new List<string>();
                        if (i == tokenSendingCount)
                        {
                            regToken = new List<string>();
                            regToken.AddRange(usersRegistrationtokens);
                        }
                        else
                        {
                            var selected = usersRegistrationtokens.Take(1000).ToList();
                            selected.ForEach(item => usersRegistrationtokens.Remove(item));
                            regToken.AddRange(selected);
                        }

                        var TblAllLogPushNotificationTopicWiseList = new List<TblAllLogPushNotificationTopicWise>();
                        foreach (var item in regToken)
                        {
                            var TblAllLogPushNotificationTopicWise = new TblAllLogPushNotificationTopicWise();

                            TblAllLogPushNotificationTopicWise.Tokens = item;
                            TblAllLogPushNotificationTopicWise.TimeStamp = DateTime.Now;
                            TblAllLogPushNotificationTopicWise.ServiceName = data.ServiceName;
                            TblAllLogPushNotificationTopicWise.Topic = topic;
                            TblAllLogPushNotificationTopicWise.Status = 1;
                            TblAllLogPushNotificationTopicWiseList.Add(TblAllLogPushNotificationTopicWise);

                            var PreviousTokenData = _entities.TblAllLogPushNotificationTopicWises.Where(x => x.ServiceName == data.ServiceName && x.Status == 1 && x.Tokens == item).FirstOrDefault();

                            if (PreviousTokenData != null)
                            {
                                var PT = new List<string>();
                                PT.Add(item);
                                var UnsubscriptionResponse = await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(PT, PreviousTokenData.Topic);

                                // For delete
                                //_entities.TblAllLogPushNotificationTopicWises.Remove(PreviousTokenData);
                                // For update
                                PreviousTokenData.Status = 0;
                                _entities.SaveChanges();
                            }

                        }
                        //// For saving LogPushNotificationTopicWises
                        _entities.TblAllLogPushNotificationTopicWises.AddRange(TblAllLogPushNotificationTopicWiseList);
                        _entities.SaveChanges();
                        /////

                        var SubscriptionResponse = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(regToken, topic);
                        ////string concatedRegTokens = string.Join(",", regToken);
                        ////

                        _entities.Remove(data);
                        await _entities.SaveChangesAsync();


                        _entities.TblAllLogPushNotificationMessageSendings.Add(new TblAllLogPushNotificationMessageSending()
                        {
                            Tokens = regToken.Count.ToString(),
                            TimeStamp = DateTime.Now,
                            ServiceName = data.ServiceName,
                            Topic = topic,
                            NotificationTitle = data.NotificationTitle,
                            NotificationBody = data.NotificationBody,
                            RedirectUrl = data.RedirectUrl,
                            ReScheduleTime = data.ReScheduleTime,
                            Campaign = data.Campaign,
                            Status = 1,
                            UserBase = data.UserBase,

                        });
                        _entities.SaveChanges();

                        /////  Sending Message
                        ///
                        var redirectedURl = data.RedirectUrl + "/?ckey=2222";

                        var message = new Message()
                        {
                            Notification = new Notification()
                            {
                                Title = data.NotificationTitle,
                                Body = data.NotificationBody,

                            },
                            Webpush = new WebpushConfig()
                            {
                                FcmOptions = new WebpushFcmOptions()
                                {

                                    Link = redirectedURl
                                    //Link = "https://quizmaster.shabox.mobi/LandingPage/Leaderboard?push=1"
                                }
                            },
                            //Data = new Dictionary<string, string>()
                            //{
                            //    { "score", "850" },
                            //    { "time", "2:45" },
                            //},
                            Topic = topic,
                            //
                            Android = new AndroidConfig()
                            {
                                TimeToLive = TimeSpan.FromHours(1),
                                Notification = new AndroidNotification()
                                {
                                    Icon = "stock_ticker_update",
                                    Color = "#f45342",
                                },
                            },
                            Apns = new ApnsConfig()
                            {
                                Aps = new Aps()
                                {
                                    Badge = 42,
                                },
                            },
                        };

                        // Send a message to the devices subscribed to the provided topic.
                        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                        // Response is a message ID string.
                        Console.WriteLine("Successfully sent message: " + response);

                        /////
                    }
                }
                catch (Exception ex)
                {
                }

            }


            return Ok(true);
        }


        [HttpPost]
        public async Task<IActionResult> SendNotificationToSelectedUsers([FromBody] FCMNotificationToSelectedUsers fCMNotificationToSelectedUsers)
        {
            try
            {
                var usersRegistrationtokens = _entities.TblAllLogPushNotificationV2s.Where(x => x.ServiceName == fCMNotificationToSelectedUsers.Service && (x.Token != null && x.Token != "") && (x.TimeStamp.Date >= fCMNotificationToSelectedUsers.FromDate.Value.Date && (x.TimeStamp.Date) <= fCMNotificationToSelectedUsers.ToDate.Value.Date)).Select(x => x.Token).ToList();
                var tokenSendingCount = 0;
                if ((Convert.ToDouble(usersRegistrationtokens.Count) / 1000) > 0 && (Convert.ToDouble(usersRegistrationtokens.Count)/1000) <= 1)
                {
                    tokenSendingCount = 1;
                }
                else if((Convert.ToDouble(usersRegistrationtokens.Count) / 1000) > 1)
                {
                    double value = usersRegistrationtokens.Count / 1000;
                    tokenSendingCount = (int)Math.Ceiling(value);
                }

                for (int i = 1; i <= tokenSendingCount; i++)
                {
                    var topic = Guid.NewGuid().ToString() +"_"+ DateTime.Now.ToString("yyyy-MM-dd");

                    var regToken = new List<string>();
                    if (i == tokenSendingCount)
                    {
                        regToken = new List<string>();
                        regToken.AddRange(usersRegistrationtokens);
                    }
                    else
                    {
                        var selected = usersRegistrationtokens.Take(1000).ToList();
                        selected.ForEach(item => usersRegistrationtokens.Remove(item));
                        regToken.AddRange(selected);
                    }

                    var TblAllLogPushNotificationTopicWiseList = new List<TblAllLogPushNotificationTopicWise>();
                    foreach (var item in regToken)
                    {
                        var TblAllLogPushNotificationTopicWise = new TblAllLogPushNotificationTopicWise();

                        TblAllLogPushNotificationTopicWise.Tokens = item;
                        TblAllLogPushNotificationTopicWise.TimeStamp = DateTime.Now;
                        TblAllLogPushNotificationTopicWise.ServiceName = fCMNotificationToSelectedUsers.Service;
                        TblAllLogPushNotificationTopicWise.Topic = topic;
                        TblAllLogPushNotificationTopicWise.Status = 1;
                        TblAllLogPushNotificationTopicWiseList.Add(TblAllLogPushNotificationTopicWise);

                        var PreviousTokenData = _entities.TblAllLogPushNotificationTopicWises.Where(x => x.ServiceName == fCMNotificationToSelectedUsers.Service && x.Status == 1 && x.Tokens == item).FirstOrDefault();

                        if (PreviousTokenData != null)
                        {
                            var PT = new List<string>();
                            PT.Add(item);
                            //var UnsubscriptionResponse = await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(PT, PreviousTokenData.Topic);

                            //// For delete
                            ////_entities.TblAllLogPushNotificationTopicWises.Remove(PreviousTokenData);
                            //// For update
                            PreviousTokenData.Status = 0;
                            //_entities.SaveChanges();
                        }
                        
                    }
                    //// For saving LogPushNotificationTopicWises
                    //_entities.TblAllLogPushNotificationTopicWises.AddRange(TblAllLogPushNotificationTopicWiseList);
                    //_entities.SaveChanges();
                    /////

                    //var SubscriptionResponse = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(regToken, topic);
                    ////string concatedRegTokens = string.Join(",", regToken);

                    //_entities.TblAllLogPushNotificationMessageSendings.Add( new TblAllLogPushNotificationMessageSending()
                    //{
                    //    Tokens = regToken.Count.ToString(),
                    //    TimeStamp = DateTime.Now,
                    //    ServiceName = fCMNotificationToSelectedUsers.Service,
                    //    Topic = topic,
                    //    NotificationTitle = fCMNotificationToSelectedUsers.Title,
                    //    NotificationBody = fCMNotificationToSelectedUsers.Body
                    //});
                    //_entities.SaveChanges();

                    /////  Sending Message
                    ///

                    var message = new Message()
                    {
                        Notification = new Notification()
                        {
                            Title = fCMNotificationToSelectedUsers.Title,
                            Body = fCMNotificationToSelectedUsers.Body,

                        },
                        Webpush = new WebpushConfig()
                        {
                            FcmOptions = new WebpushFcmOptions()
                            {
                                Link = fCMNotificationToSelectedUsers.RedirectUrl + "/?push=1" 
                                //Link = "https://quizmaster.shabox.mobi/LandingPage/Leaderboard?push=1"
                            }
                        },
                        //Data = new Dictionary<string, string>()
                        //{
                        //    { "score", "850" },
                        //    { "time", "2:45" },
                        //},
                        Topic = topic,
                        //
                        Android = new AndroidConfig()
                        {
                            TimeToLive = TimeSpan.FromHours(1),
                            Notification = new AndroidNotification()
                            {
                                Icon = "stock_ticker_update",
                                Color = "#f45342",
                            },
                        },
                        Apns = new ApnsConfig()
                        {
                            Aps = new Aps()
                            {
                                Badge = 42,
                            },
                        },
                    };

                    //// Send a message to the devices subscribed to the provided topic.
                    //string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    //// Response is a message ID string.
                    //Console.WriteLine("Successfully sent message: " + response);

                    /////
                }


            }
            catch (Exception ex)
            {

            }
            return Ok(true);
        }




        [HttpGet]

        public async Task<IActionResult> SaveToken(string Token)
        {
            try
            {
                var SaveToken = Token;
                var topic = "SendAll";
                //var topic = "UpdatedVersionV1";
                var res = await SubscribeToTopicAsync(topic, Token);

                //var message = new Message()
                //{
                //    Notification = new Notification()
                //    {
                //        Title = "$GOOG up 1.43% on the day",
                //        Body = "$GOOG gained 11.80 points to close at 835.67, up 1.43% on the day.",
                //    },
                //    Data = new Dictionary<string, string>()
                //    {
                //        { "score", "850" },
                //        { "time", "2:45" },
                //    },
                //    Topic = topic,
                //    //
                //    Android = new AndroidConfig()
                //    {
                //        TimeToLive = TimeSpan.FromHours(1),
                //        Notification = new AndroidNotification()
                //        {
                //            Icon = "stock_ticker_update",
                //            Color = "#f45342",
                //        },
                //    },
                //    Apns = new ApnsConfig()
                //    {
                //        Aps = new Aps()
                //        {
                //            Badge = 42,
                //        },
                //    },
                //};

                //// Send a message to the devices subscribed to the provided topic.
                //string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                //// Response is a message ID string.
                //Console.WriteLine("Successfully sent message: " + response);

            }
            catch (Exception ex)
            {

            }
            return Ok(true);
        }

        [HttpGet]
        public async Task<string> SubscribeToTopicAsync(string topic, string token)
        {
            // [START subscribe_to_topic]
            // These registration tokens come from the client FCM SDKs.
            var registrationTokens = new List<string>()
            {
                token
            };

            // Subscribe the devices corresponding to the registration tokens to the
            // topic
            var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(
                registrationTokens, topic);
            // See the TopicManagementResponse reference documentation
            // for the contents of response.
            Console.WriteLine($"{response.SuccessCount} tokens were subscribed successfully");
            // [END subscribe_to_topic]
            return response.SuccessCount.ToString();
        }

        public async Task<JsonResult> SavePushNotificationAllowedLogs(string token, string AllowedType, string SourceUrl)
        {
            var existingLog = await _entities.TblAllLogPushNotificationV2s.Where(x => x.Token == token).FirstOrDefaultAsync();
            if (existingLog == null)
            {
                await _entities.TblAllLogPushNotificationV2s.AddAsync(new TblAllLogPushNotificationV2()
                {
                    Token = token,
                    ServiceName = "QuizMaster",
                    SourceUrl = SourceUrl,
                    TimeStamp = DateTime.Now,
                    Status = AllowedType

                });
                await _entities.SaveChangesAsync();
            }
            
            return Json(new { result = "success" });
        }

        [HttpGet]
        public async Task<IActionResult> GetLocationOfIps()
        {
            var ipList = await _entities.TblAllLogQuizBkashes.Where(x => x.ServiceRequest == "DOBQUIZ").Select(x => x.DeviceIp).Distinct().ToListAsync();
            //var ipList = await _entities.TblAllLogQuizBkashes.Where(x => x.Ckey == "28621116" && x.Apn == "QuizMaster" && x.ThemeId == "access_QuizMaster").Select(x => x.DeviceIp).ToListAsync();

            var ips = new {
                ips = ipList
            };
            try
            {
                var client = new RestClient("https://hunter.adplay-mobile.com/client/country_city_from_ip/");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = JsonConvert.SerializeObject(ips);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                //var res = JsonConvert.DeserializeObject(response.Content);

                return Ok(response.Content);
            }
            catch (Exception ex)
            {

            }
            

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetDeviceDataUsingUserStack()
        {
            var UAPROFList = await _entities.TblAllLogQuizBkashes.Where(x => (x.Ckey == "009999") && x.Apn == "QuizMaster" && x.ThemeId == "Initialize_QuizMaster_Phonedevice" && x.TimeStamp.Date == DateTime.Now.Date && x.HsManufacturer == "Generic" && x.FirstPageSession == null).Distinct().Select(x => x.UaprofileUrl).Take(200).ToListAsync();
            //UAPROF_URL = GetUserAgent();
            foreach (var item in UAPROFList)
            {
                DeviceTrackResponse a = new UserStackDT().GetResponse(item);
                var finalResult = a.brand + "-" + a.name + " OS-" + a.os.name;
                var existingUaProf = await _entities.TblAllLogQuizBkashes.Where(x => x.UaprofileUrl == item).ToListAsync();
                foreach (var i in existingUaProf)
                {
                    i.FirstPageSession = finalResult;
                }
                await _entities.SaveChangesAsync();
            }
            

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> SaveUnsubscriptionSurveyResult(string msisdn, string answer)
        {
            if (msisdn != null && answer != null)
            {
                var ans = Convert.ToInt32(answer);
                await _entities.TblQuizMasterUnsubscriptionNews.AddAsync(new TblQuizMasterUnsubscriptionNew
                { 
                    ServiceName = "QuizMaster",
                    UnsubscriptionNumber = msisdn,
                    Answer = ans,
                    TimeStamp = DateTime.Now
                });
                await _entities.SaveChangesAsync();
            }
            return Ok(true);
        }

        [HttpGet]

        public async Task<IActionResult> SaveLiveQuizLog(string msisdn, string ckey, string cname, string theme, string srcurl)
        {
            try
            {
                var queryString = _contextHttp.HttpContext.Request.QueryString.Value.ToString();

                var splitedString = queryString.Split("srcurl=");
                if (string.IsNullOrWhiteSpace(msisdn))
                {
                    msisdn = "";
                }

                string ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, serviceName, cKey, sourceurl2;
                //GeneralInfo(out msisdn, out ua, out ip, out HS_MANUFAC, out HS_MOD, out HS_DIM, out HS_OS, out serviceName, out cKey, out sourceurl2);



                var httpRequest = _contextHttp.HttpContext.Request;
                //var rawurl = _contextHttp.HttpContext.Request.Url.ToString();
                //var rawurl = _contextHttp.HttpContext.Request.Path.ToString();
                var rawurl = _contextHttp.HttpContext.Request.Host.ToString();

                // _contextHttp.HttpContext.Session.SetString("rawurl", srcurl);
                _contextHttp.HttpContext.Session.SetString("rawurl", splitedString.Length > 1 ? splitedString[1] : srcurl);

                UAPROF_URL = GetUserAgent();
                DD a = new Hsprofiling().GetResponse(UAPROF_URL);

                await _QuizMasterLiveQuizLogContext.TblQuizMasterLiveQuizLiveLogs.AddAsync(new TblQuizMasterLiveQuizLiveLog
                {

                    Msisdn = msisdn,
                    SourceUrl = splitedString.Length > 1 ? splitedString[1] : srcurl,
                    TimeStamp = DateTime.Now,
                    Ckey = ckey,
                    ThemId = theme

                });
                await _QuizMasterLiveQuizLogContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            return Ok(true);

        }

        [HttpGet]

        public async Task<IActionResult> GetLiveQuizVideos()
        {
            try
            {

                var res = await _wapPortalContext.GetLiveQuizVideosDataModel.FromSqlRaw("EXEC [sp_BkashQuizMasterLiveQuizVideoFetching] ").ToListAsync();
                //var res = await _wapPortalContext.GetLiveQuizVideosDataModel.FromSqlRaw("EXEC [sp_BkashQuizMasterLiveQuizVideoFetching_forSiteTest] ").ToListAsync();

                return Ok(new { Response = res });


            }
            catch (Exception ex)
            {

            }

            return Ok(true);

        }





        public class AdcashToken
        {
            public string token { get; set; }
        }


        [HttpGet]
        public async Task<string> AdcashTokenAPI()
        {
            try
            {

                HttpClient client = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.myadcash.com/api/v1/auth");

                request.Headers.Add("Cache-Control", "no-cache");

                request.Content = new StringContent("username=sadiarh@vumobile.biz&password=adcash@Vu");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<AdcashToken>(responseBody);

                //return Ok(new { token = token.token });
                return token.token;


            }
            catch (Exception ex)
            {

            }

            //return Ok(true);
            return "";

        }

        [HttpGet]
        public async Task<IActionResult> AdcashAdvertiserReport()
        {
            try
            {
                var Token = await AdcashTokenAPI();

                if (Token != "")
                {

                    //for filtering 
                    var start_date = "2022-11-28";
                    var end_date = "2022-11-29";
                    var advertiser_id = "";
                    var group_by = "group_by=date,country,campaignid,campaignname,devicetype,packid,packname,platformname,browser,zoneid";
                    var country = "country=BD";
                    //var campaignid = "campaignid=";
                    var device = "device=SmartPhone,Mobile";

                    //var AdcashURl = "https://api.myadcash.com/api/v1/advertiser-report?"+start_date+"&"+end_date+"&"+advertiser_id+"&"+group_by+"&"+country+"&"+campaignid+"&"+device;
                    var AdcashURl = "https://api.myadcash.com/api/v1/advertiser-report?start_date=" + start_date+ "&end_date=" + end_date+ "&advertiser_id=" + advertiser_id+"&"+group_by+"&"+country+"&"+device;



                    HttpClient client = new HttpClient();

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AdcashURl);

                    request.Headers.Add("Accept", "application/json");
                    request.Headers.Add("Authorization", "Bearer " + Token);
                    request.Headers.Add("Cache-Control", "no-cache");

                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return Ok(responseBody);
                }
            }
            catch (Exception ex)
            {

            }
            
            return Ok();
        }



        






        [HttpGet]
        public async Task<IActionResult> GetDeviceSpecificData()
        {
            var UserA = "";
            UAPROF_URL = GetUserAgent();
            DD a = new Hsprofiling().GetResponse(UAPROF_URL);
            
            if (a.manufac.ToLower() == "generic")
            {
                UserA = GetDeviceDataUsingUserStackByUserAgent(UAPROF_URL);
            }

            var res = new
            {
                DeviceIp = GetUserIP(),
                HsDim = a.dimension,
                HsModel = a.model_name,
                HsManufacturer = a.manufac,
                DeviceModel = a.manufac.ToLower() == "generic" && (UserA != null || UserA != "" || UserA != "null") ? UserA : a.manufac + " - " + a.model_name
                
            };

            return Ok(res);
        }

        public string  GetDeviceDataUsingUserStackByUserAgent(string UserAgent)
        {
            
                DeviceTrackResponse a = new UserStackDT().GetResponse(UserAgent);
                var finalResult = a.brand + "-" + a.name + " OS-" + a.os.name;
                


            return finalResult;
        }

        [HttpGet]
        public async Task<Char[]> GetRandomPuzzleWords()
        {

            try
            {
                //var match = new HashSet<string>() { "adieu", "ducat", "squad", "awe", "dim", "eon", "acme",
                //    "ajar", "bevy", "golf", "spad", "smar", "froe", "frig", "optic", "coal", "lap", "nib", 
                //    "edict","exult", "ethos", "braid", "bilk", "blase","blunt","bate","fawn" };
                //var word = await _entities.TblPuzzleWordLibs.Where(x => match.Contains(x.Words)).Select(x => x.Words).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                //var word = await _entities.TblPuzzleWordLibs.Where(x => x.TimeStamp.Date >= DateTime.Parse("2023-03-29")).Select(x => x.Words).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                var word = await _entities.TblPuzzleWordLibs.Select(x => x.Words).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

                var res = word.ToUpper().ToCharArray();

                return res;
            }
            catch (Exception ex)
            {
            }
            return new Char[] { };
        }

        [HttpGet]
        public async Task<IActionResult> GetRandomPuzzleQuestionWordCombo()
        {

            try
            {
                //var match = new HashSet<string>() { "adieu", "ducat", "squad", "awe", "dim", "eon", "acme",
                //    "ajar", "bevy", "golf", "spad", "smar", "froe", "frig", "optic", "coal", "lap", "nib", 
                //    "edict","exult", "ethos", "braid", "bilk", "blase","blunt","bate","fawn" };
                //var word = await _entities.TblPuzzleWordLibs.Where(x => match.Contains(x.Words)).Select(x => x.Words).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                //var word = await _entities.TblPuzzleWordLibs.Where(x => x.TimeStamp.Date >= DateTime.Parse("2023-03-29")).Select(x => x.Words).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                //var word = await _entities.WordMixUpQuestionAnswerCombos.Select(x => x.Words).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

                //var question = await _entities.WordMixUpQuestionAnswerCombos.Select(x => x.Question).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

                var questionCombo = await _entities.WordMixUpQuestionAnswerCombos.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

                //var res = word.ToUpper().ToCharArray();

                var questionObject = new { Question = questionCombo.Question, Words = questionCombo.Words.ToUpper().ToCharArray() };

                return  Json(questionObject) ;
            }
            catch (Exception ex)
            {
                return null;
            }
            //return new Char[] { };
        }


        [HttpGet]
        public async Task<Char[]> GetRandomPuzzleWordsAccordingToScore(int score)
        {

            try
            {
                if (score == 1)
                {
                    var word = await _entities.TblPuzzleWordLibs
                        .Where(x => x.Words.Length == 1)
                        .OrderByDescending(x => x.TimeStamp)
                        .Select(x => x.Words)
                        .FirstOrDefaultAsync();

                    if (word != null)
                    {
                        var res = word.ToUpper().ToCharArray();
                        return res;
                    }
                }

                // Handle the case where no suitable record is found or the score is not 1
                // You can return a default value or handle it according to your requirements.


                if (score <=10 )
                {
                    var word = await _entities.TblPuzzleWordLibs.Select(x => x.Words).Where(x => x.Length == 3).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                    var res = word.ToUpper().ToCharArray();
                    return res;
                }
                else if (score>=10 & score <=20)
                {
                    var word = await _entities.TblPuzzleWordLibs.Select(x => x.Words).Where(x => x.Length == 4).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                    var res = word.ToUpper().ToCharArray();
                    return res;
                }
                else if (score >= 20 & score <= 30)
                {
                    var word = await _entities.TblPuzzleWordLibs.Select(x => x.Words).Where(x => x.Length == 5).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                    var res = word.ToUpper().ToCharArray();
                    return res;
                }
                else
                {
                    var word = await _entities.TblPuzzleWordLibs.Select(x => x.Words).Where(x => x.Length >= 5).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                    var res = word.ToUpper().ToCharArray();
                    return res;
                }
                
                

                
            }
            catch (Exception ex)
            {
            }
            return new Char[] { };
        }


        //[HttpPost]
        //public async Task<bool> SavePuzzleWords([FromBody] IEnumerable<string> str)
        //{

        //    try
        //    {
        //        if (str != null)
        //        {
        //            foreach (var item in str)
        //            {
        //                await _entities.TblPuzzleWordLibs.AddAsync(new TblPuzzleWordLib
        //                {
        //                    Words = item
        //                });

        //            }
        //            await _entities.SaveChangesAsync();
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return true;
        //}


        [HttpGet]
            public void UploadFile(
                string bucketName = "live-quiz-videos",
                string localPath = "D:/zzz/Video 1.mp4",
                string objectName = "Video 1.mp4")
            {
                try
                {
                    var storage = StorageClient.Create();
                    using (FileStream fs = System.IO.File.OpenRead(localPath))
                    {
                        //byte[] b = new byte[1024];
                        //UTF8Encoding temp = new UTF8Encoding(true);

                        //while (fs.Read(b, 0, b.Length) > 0)
                        //{
                        //    // Printing the file contents
                        //    Console.WriteLine(temp.GetString(b));
                        //}
                        storage.UploadObject(bucketName, objectName, null, fs);
                        Console.WriteLine($"Uploaded {objectName}.");
                    }
                }
                catch (Exception ex)
                {

                }
                
            //using var fileStream = File.OpenRead(localPath);
            //    storage.UploadObject(bucketName, objectName, null, fileStream);
            //    Console.WriteLine($"Uploaded {objectName}.");
            }



        public async Task BucketFile(string path, string name)
        {
            string clientId = "39156737544-m3itd391ikiud5r0r28qno7pno9vk1r9.apps.googleusercontent.com";
            string clientSecret = "duAXiU1761mx9NK0sO9NiUx9";
            string appname = "oathinfo";
            var clientSecrets = new ClientSecrets();
            clientSecrets.ClientId = clientId;
            clientSecrets.ClientSecret = clientSecret;
            //there are different scopes, which you can find here https://cloud.google.com/storage/docs/authentication
            var scopes = new[] { @"https://www.googleapis.com/auth/devstorage.full_control" };



            var cts = new CancellationTokenSource();
            // var userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, scopes, "vumobilegcp@gmail.com", cts.Token);
            // await userCredential.RefreshTokenAsync(cts.Token);
            var service = new Google.Apis.Storage.v1.StorageService();





            //            var bucketsQuery = service.Buckets.List("songstarcontent");
            //            bucketsQuery.OauthToken = userCredential.Token.AccessToken;
            //            var buckets = bucketsQuery.Execute();



            //enter bucket name to which you want to upload file
            //var bucketToUpload = buckets.Items.FirstOrDefault().Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "xyz";
            }
            var fileobj = new Google.Apis.Storage.v1.Data.Object()
            {
                Bucket = "songstarvideo",
                Name = name
            };



            FileStream fileStream = null;
            try
            {
                // var dir = Directory.GetCurrentDirectory();
                //var path = Path.Combine(dir, "a.mp4");
                fileStream = new FileStream(path, FileMode.Open);
                var uploadRequest = new Google.Apis.Storage.v1.ObjectsResource.InsertMediaUpload(service, fileobj,
                    "songstarvideo", fileStream, "video/mp4");
                //  uploadRequest.OauthToken = userCredential.Token.AccessToken;



                var rr = await uploadRequest.UploadAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
        }
        [HttpGet]
        public ActionResult CheckRound(string FbId)
        {
            try
            {
                string connectionString = "server=103.134.68.67;database=WapPortal;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string checkRoundQuery = "SELECT ISNULL(MAX(Round), 0) FROM [WapPortal].[dbo].[tbl_JhotPotAnswerAudio] " +
                                              "WHERE [FbId] = @FbId AND CONVERT(DATE, [TimeStamp]) = CONVERT(DATE, GETDATE())";

                    using (SqlCommand checkRoundCommand = new SqlCommand(checkRoundQuery, connection))
                    {
                        checkRoundCommand.Parameters.AddWithValue("@FbId", FbId);

                        int round = (int)checkRoundCommand.ExecuteScalar();

                        return Json(new { round });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { round = 0, error = $"Error checking round: {ex.Message}" });
            }
        }


        [HttpGet]
        public ActionResult SaveAnswer(string answer, string fbId, bool isRight, int timeTaken, int round, string serviceName)
        {
            try
            {
                string connectionString = "server=103.134.68.67;database=WapPortal;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";

                DateTime timeStamp = DateTime.Now;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [WapPortal].[dbo].[tbl_JhotPotAnswerAudio] " +
                                         "([Answer], [FbId], [IsRight], [TimeStamp], [TimeTaken], [Round], [ServiceName]) " +
                                         "VALUES (@Answer, @FbId, @IsRight, @TimeStamp, @TimeTaken, @Round, @ServiceName)";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Answer", answer);
                        insertCommand.Parameters.AddWithValue("@FbId", fbId);
                        insertCommand.Parameters.AddWithValue("@IsRight", isRight);
                        insertCommand.Parameters.AddWithValue("@TimeStamp", timeStamp);
                        insertCommand.Parameters.AddWithValue("@TimeTaken", timeTaken);
                        insertCommand.Parameters.AddWithValue("@Round", round);
                        insertCommand.Parameters.AddWithValue("@ServiceName", serviceName);

                        insertCommand.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                return Json(new { success = true, message = "Answer saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error saving answer: {ex.Message}" });
            }
        }



        [HttpPost]
        public async Task<IActionResult> InsertFreeQuizData(string name, string msisdn ,string ckey)
        {
            try
            {
                string connectionString = "server=103.134.68.67;database=Basket;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";
                UAPROF_URL = GetUserAgent();
                DD a = new Hsprofiling().GetResponse(UAPROF_URL);
                Response.Headers.Add("Access-Control-Allow-Origin", "*");
                //_contextHttp.HttpContext.Session.SetString("rawurl", srcurl);
                var queryString = _contextHttp.HttpContext.Request.QueryString.Value.ToString();
                if (string.IsNullOrWhiteSpace(msisdn))
                {
                    msisdn = "";
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = "";
                }

                string Apn = "QuizMaster";
                string DeviceIp = GetUserIP();
                string HsDim = a.dimension;
                string HsModel = a.model_name;
                string HsManufacturer = a.manufac;
                string ServiceRequest = "QuizMaster";
                string TimeStamp = DateTime.Now.ToString();
                string UaprofileUrl = GetUserAgent();
                string Ckey = ckey;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO tbl_FreeQuizPlayers (Name, MSISDN , Apn, DeviceIp ,HsDim,HsModel,HsManufacturer,ServiceRequest,TimeStamp,UaprofileUrl,Ckey ) VALUES (@Name,@MSISDN,@Apn,@DeviceIp,@HsDim,@HsModel,@HsManufacturer,@ServiceRequest,@TimeStamp,@UaprofileUrl,@Ckey)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@MSISDN", msisdn);
                        command.Parameters.AddWithValue("@Apn", Apn);
                        command.Parameters.AddWithValue("@DeviceIp", DeviceIp);
                        command.Parameters.AddWithValue("@HsDim", HsDim);
                        command.Parameters.AddWithValue("@HsModel", HsModel);
                        command.Parameters.AddWithValue("@HsManufacturer", HsManufacturer);
                        command.Parameters.AddWithValue("@ServiceRequest", ServiceRequest);
                        command.Parameters.AddWithValue("@TimeStamp", TimeStamp);
                        command.Parameters.AddWithValue("@UaprofileUrl", UaprofileUrl);
                        command.Parameters.AddWithValue("@Ckey", Ckey);
                        connection.Open();
                        command.ExecuteNonQuery();
                        return Ok(true);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //public IActionResult GetRandomAudioQuestions()
        //{
        //    List<AudioQuestion> audioQuestions = GetRandomAudioQuestionsFromDatabase();

        //    if (audioQuestions != null)
        //    {

        //        return Json(audioQuestions);
        //    }
        //    else
        //    {

        //        return Content("Error fetching audio questions.");
        //    }
        //}

        private List<AudioQuestion> GetRandomAudioQuestionsFromDatabase(string serviceName)
        {
            string connectionString = "server=103.134.68.67;database=WapPortal;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";

            int numberOfQuestions = 15;

        
            Dictionary<string, int> serviceCategoryMapping = new Dictionary<string, int>
                {
                    {"QuizMaster-MultiService3", 1},
                    {"QuizMaster-MultiService4", 4},
                    {"QuizMaster-MultiService5", 3},
                    {"QuizMaster-MultiService6", 2}
                };

            // Check if the serviceName exists in the mapping
            if (serviceCategoryMapping.TryGetValue(serviceName, out int categoryNumber))
            {
                string sqlQuery = $@"
            SELECT TOP {numberOfQuestions} 
                [ImageFile],
                [AudioFile],
                [Option1],
                [Option2],
                [Option3],
                [Answer],
                [TimeStamp],
                [QuestionCategory]
            FROM [JhotPot_AudioQuestionNew]
            WHERE [QuestionCategory] = {categoryNumber} -- Filter by category
            ORDER BY NEWID()";

                // List to store the result
                List<AudioQuestion> audioQuestions = new List<AudioQuestion>();

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    AudioQuestion question = new AudioQuestion
                                    {
                                        ImageFile = reader["ImageFile"].ToString(),
                                        AudioFile = reader["AudioFile"].ToString(),
                                        Option1 = reader["Option1"].ToString(),
                                        Option2 = reader["Option2"].ToString(),
                                        Option3 = reader["Option3"].ToString(),
                                        Answer = reader["Answer"].ToString(),
                                        TimeStamp = (DateTime)reader["TimeStamp"],
                                        QuestionCategory = reader["QuestionCategory"].ToString()
                                    };

                                    audioQuestions.Add(question);
                                }
                            }
                        }
                    } // The connection will be automatically closed here

                    return audioQuestions;
                }
                catch (Exception ex)
                {
                    // Handle the exception (log it, throw it, etc.)
                    Console.WriteLine($"Error: {ex.Message}");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($"Error: Unknown serviceName '{serviceName}'");
                return null;
            }
        }



        public async Task<object> Postup()
        {
            var uploadAsset = Path.Combine(_env.ContentRootPath, "Assets", "LiveQuizRelated");
            
            string[] files;

            try
            {
                //if (Directory.Exists(uploadAsset))
                //{

                    files = Directory.GetFiles(uploadAsset, @"*.mp4", SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {

                    string rawdate = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var tempNameVideo = rawdate + DateTime.Now.Ticks.ToString() + file;



                    //var filePath = Path.Combine(uploads + "Videos", tempNameVideo);
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await file.CopyToAsync(fileStream);
                    //}



                    await BucketFile(uploadAsset, tempNameVideo);
                }



                //}
            }
            catch (Exception ex)
            {
            }
                

            return Ok(new
            {
                result = "Success"
            }) ;

        }

   
        

    }
}
