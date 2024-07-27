using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizMaster.Models;
using QuizMaster.Models.BkashPaymentGateWay;
using QuizMaster.Models.StaticModels;
using QuizMaster.Models.WapPortal;
using RestSharp;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace QuizMaster.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly BasketContext _entity = new BasketContext();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BkashPaymentGateWayContext _context;
        private readonly BasketContext _basketContext;
        private readonly WapPortalContext  _wapPortalContext;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, BkashPaymentGateWayContext context, BasketContext basketContext, WapPortalContext wapPortal)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _context = context;
            _basketContext = basketContext;
            _wapPortalContext = wapPortal;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpPost]
        public async Task<IActionResult> UnSubscription([FromQuery] string msisdn, [FromQuery] string reason = "TestCancelSubscription")
        {
            //var subData = _context.SubscriptionRequestDatas.Where(x => x.SubscriptionReference == msisdn || x.SubscriptionReference == "88" + msisdn).FirstOrDefault();
            var sub = _basketContext.TblQuizStarSubscriptions.Where(x => (x.Msisdn == msisdn || x.Msisdn == "88" + msisdn) && x.DeactivationDate == null && x.RegStatus == 1).OrderByDescending(x => x.TimeStamp).FirstOrDefault();

            if (sub != null)
            {
                //var subReqId = subData.SubscriptionRequestId;
                var subReqId = sub.SubscriptionRequestId;
                //var token = LoginToken();
                var subId = GetSubId( subReqId);

                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                var client = new RestClient("https://bkashpaymentapi.shabox.mobi/api/RecurringPayment/CancelSubscription?subId=" + subId + "&reason=" + reason);

                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                //request.AddHeader("Authorization", "Bearer " + token);
                request.AddHeader("Cookie", ".AspNetCore.Identity.Application=CfDJ8Fj-XSTGdixDrE39DyfMxK_yIzw84dbW_puilKsFSzXPimkkcodL7yEI0c5bXFodVhGeMU8AfGzPgRKBcIj6LFxxBki-AjoQNbIuVi5wNLjJPxjequPp8g8UMpVA0A8f-b1bDNKJOI4EOR8mfWaHWPLiuilGuNVj4cA0xHqlx3ENHTkeeTD76i_Gqc9g9_cJQ3eXcNlqyuAYvO-s8lHjGYE6ZkV4HXOSWcyW2V3i0UNdqx6xZ-S0MdZCkSKzw4LX0sis9swjBgpmzTCs4LP7lNcTQgKyrjGegQsFI7E0P1DI9vKM85kSzc40n5kv8RnFjM53RFAqfDfMkTwedM4eJDE4c1Tn7hMOOtCm7mkLeMBdxG9uw_2BVZ_hCR06cTlll8KHtzn1j4jiD3tIzLJsdh-QfLG58jhTcWcS_jKvXGwDkk1UcCo9A8oIKq0cwGy9S2Kp-SKvFMikq4R3M4XR5tA9X6lHPgMAU4wzI-Lev3_Cq-W-Bnc18pugGtzHeXJ3782IpslKeIKGaVIBU7GwvHW7WMHRhqngl6n3EkOZHtqFvln1xGZ3fKZw-YUtM1fW226rTDP87mm2HclrKYks8crJILBmwNTsLmx8cHwsadT__j8ZpmEs8cgXepA1m1ovRw-1ZKONjQwQy6fUunLqhew6WWGWAbwAgWlU5lMyW9qqPj5Chwws1qO7lSxyaiLQfR7NRh1VuxS4aisYlDv47ewY_xbR37dPR5n3FlO1dmYw");
                IRestResponse response = client.Execute(request);
                var UnsubResult = JsonConvert.DeserializeObject<UnSubscriptionResponseModel>(response.Content);





                using (var db = new BasketContext())
                {
                    var resSub = await db.TblQuizStarSubscriptions.Where(x => (x.Msisdn == msisdn || x.Msisdn == "88" + msisdn) && x.DeactivationDate == null && x.RegStatus == 1).OrderByDescending(x => x.TimeStamp).FirstOrDefaultAsync();

                    if (resSub != null)
                    {
                        //db.TblQuizStarSubscriptions.Remove(resSub);
                        //await db.SaveChangesAsync();


                        //var resSubData = new TblQuizStarSubscription()
                        //{
                        //    Msisdn = msisdn,
                        //    Fbid = msisdn,
                        //    //RegDate= RegDate,
                        //    DeactivationDate = DateTime.Now,
                        //    RegStatus = -1,
                        //    TimeStamp = DateTime.Now,
                        //    RegistrationType = 2,
                        //    //LastChargeDate=LastChargeDate,
                        //};

                        resSub.RegStatus = -1;
                        resSub.DeactivationDate = DateTime.Now;


                        await db.SaveChangesAsync();
                    }

                }


                return Ok(UnsubResult.SubscriptionStatus);
            }
            else
            {
                return Ok("Please first you subscription");

            }


        }


        [HttpGet]
        public async Task<IActionResult> CheckWebHookStatusV2([FromQuery] string subReqId)
        {
            //using (var db = new BkashPaymentGateWayContext())
            //{

            string connectionString = "server=103.134.68.67;database=BkashPaymentGateway;user=sa;pwd=adplayVu@1234;";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string commandString = "SELECT Status FROM [BkashPaymentGateway].[dbo].[WebHookTrigger] with (nolock) where SubscriptionRequestId = " + "'" + subReqId + "'";
                connection.Open();
                SqlCommand command = new SqlCommand(commandString, connection);
                //command.Parameters.AddWithValue("@SubscriptionRequestId", subReqId);
                object result = command.ExecuteScalar();
                connection.Close();

                //var msisdn = await _context.SubscriptionRequestDatasV3s.Where(x => x.SubscriptionRequestId == subReqId).Select(x => x.SubscriptionReference).FirstOrDefaultAsync();

                if (result.ToString().Contains("SUCCEEDED"))
                {
                    return Ok(new { message = "SUCCEEDED" });

                }
                else
                {
                    return Ok(new { message = "FAILED_PAYMENT"});
                }
            }

            catch (Exception ex)
            {
                return Ok(new { message = "FAILED For Unsuccess" });
            }




        }
        [HttpGet]
        public ActionResult InsertFB(string name, string number, string answer)
        {
            string connectionString = "server=103.134.68.67;database=Basket;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string roundCheckQuery = "SELECT COUNT(*) FROM Basket.dbo.tbl_FBquizAnswer WHERE FbId = @FbId AND Round = 1 AND CONVERT(DATE, TimeStamp) = CONVERT(DATE, GETDATE())";

                    using (SqlCommand roundCheckCommand = new SqlCommand(roundCheckQuery, connection))
                    {
                        roundCheckCommand.Parameters.AddWithValue("@FbId", number);

                        int existingRoundCount = (int)roundCheckCommand.ExecuteScalar();

                        if (existingRoundCount > 0)
                        {
                            return Json(new { success = false, message = "আপনি ইতিমধ্যে একবার খেলে ফেলেছেন। কাল আবার চেষ্টা করুন ধন্যবাদ ।" });
                        }
                    }



                    string insertQuery = "INSERT INTO Basket.dbo.tbl_FBquizAnswer (Name,FbId, Answer, TimeStamp, Round) VALUES (@Name,@FbId, @Answer, @TimeStamp, @Round)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@FbId", number);
                        command.Parameters.AddWithValue("@Answer", answer);
                        command.Parameters.AddWithValue("@TimeStamp", DateTime.Now);
                        command.Parameters.AddWithValue("@Round", 1);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Successful insertion
                            return Json(new { success = true, message = "ধন্যবাদ ." });
                        }
                        else
                        {
                            // Insertion failed
                            return Json(new { success = false, message = "ধন্যবাদ" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
                return Json(new { success = false, message = "ধন্যবাদ" });
            }
        }


        [HttpGet]
        public async Task<string> UpdateSubscriptionData(string msisdn, string subReqId,string CKEY, int? registrationMedium = null)
        {
            if (msisdn != null && msisdn != "")
            {
                if (subReqId.ToLower().Contains("kidstar") )
                {
                    using (var db = new BasketContext())
                    {
                        try
                        {
                            var res = await db.TblQuizStarSubscriptionAppInApps.AddAsync(new TblQuizStarSubscriptionAppInApp()
                            {
                                Msisdn = msisdn,
                                Fbid = msisdn,
                                RegDate = DateTime.Now,
                                TimeStamp = DateTime.Now,
                                LastChargeDate = DateTime.Now,
                                RegStatus = 1,
                                RegistrationType = 2,
                                SubscriptionRequestId = subReqId,
                                ReistrationMedium = registrationMedium,
                                CKEY = CKEY
                            });
                            await db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {

                        }
                        
                    }
                }
                else if (subReqId.Contains("KidStar-MultiService1") )
                {
                    using (var db = new BasketContext())
                    {
                        var res = await db.TblQuizStarSubscriptionTournament1s.AddAsync(new TblQuizStarSubscriptionTournament1()
                        {
                            Msisdn = msisdn,
                            Fbid = msisdn,
                            RegDate = DateTime.Now,
                            TimeStamp = DateTime.Now,
                            LastChargeDate = DateTime.Now,
                            RegStatus = 1,
                            RegistrationType = 2,
                            SubscriptionRequestId = subReqId,
                            ReistrationMedium = registrationMedium,
                            CKEY = CKEY
                        });
                        await db.SaveChangesAsync();
                    }
                }
                else if (subReqId.Contains("KidStar-MultiService2"))
                {
                    using (var db = new BasketContext())
                    {
                        var res = await db.TblQuizStarSubscriptionTournament2s.AddAsync(new TblQuizStarSubscriptionTournament2()
                        {
                            Msisdn = msisdn,
                            Fbid = msisdn,
                            RegDate = DateTime.Now,
                            TimeStamp = DateTime.Now,
                            LastChargeDate = DateTime.Now,
                            RegStatus = 1,
                            RegistrationType = 2,
                            SubscriptionRequestId = subReqId,
                            ReistrationMedium = registrationMedium,
                            CKEY = CKEY
                        });
                        await db.SaveChangesAsync();
                    }
                }

            }
            else
            {
                using (var dbContext = new BkashPaymentGateWayContext())
                {
                    //var Msi = await dbContext.SubscriptionRequestDatas.Where(x => x.SubscriptionRequestId == subReqId).OrderByDescending(x => x.TimeStamp).Select(x => x.SubscriptionReference).FirstOrDefaultAsync();
                    var Msi = await dbContext.SubscriptionRequestDatasV3s.Where(x => x.SubscriptionRequestId == subReqId).OrderByDescending(x => x.TimeStamp).Select(x => x.SubscriptionReference).FirstOrDefaultAsync();

                    if (subReqId.Contains("QuizMaster") && !subReqId.Contains("MultiService"))
                    {
                        var res = await _basketContext.TblQuizStarSubscriptions.AddAsync(new TblQuizStarSubscription()
                        {
                            Msisdn = Msi,
                            Fbid = Msi,
                            RegDate = DateTime.Now,
                            TimeStamp = DateTime.Now,
                            LastChargeDate = DateTime.Now,
                            RegStatus = 1,
                            RegistrationType = 2,
                            SubscriptionRequestId = subReqId,
                            ReistrationMedium = registrationMedium
                        });
                    }

                    else if (subReqId.Contains("QuizMaster-MultiService1"))
                    {
                        var res = await _basketContext.TblQuizStarSubscriptionTournament1s.AddAsync(new TblQuizStarSubscriptionTournament1()
                        {
                            Msisdn = Msi,
                            Fbid = Msi,
                            RegDate = DateTime.Now,
                            TimeStamp = DateTime.Now,
                            LastChargeDate = DateTime.Now,
                            RegStatus = 1,
                            RegistrationType = 2,
                            SubscriptionRequestId = subReqId,
                            ReistrationMedium = registrationMedium
                        });
                    }
                    else if (subReqId.Contains("QuizMaster-MultiService1"))
                    {
                        var res = await _basketContext.TblQuizStarSubscriptionTournament2s.AddAsync(new TblQuizStarSubscriptionTournament2()
                        {
                            Msisdn = Msi,
                            Fbid = Msi,
                            RegDate = DateTime.Now,
                            TimeStamp = DateTime.Now,
                            LastChargeDate = DateTime.Now,
                            RegStatus = 1,
                            RegistrationType = 2,
                            SubscriptionRequestId = subReqId,
                            ReistrationMedium = registrationMedium
                        });
                    }

                    await _basketContext.SaveChangesAsync();
                }
            }


            return "Success";

        }


        [HttpGet]
        public async Task<string> UpdateSubscriptionDataNew(string msisdn, string subReqId,string CKEY, int? registrationMedium = null)
        {
            string connectionString = "server=103.134.68.67;database=Basket;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;"; // Stop using too much entity framework this is bullshit too much object orientation is bad for health

            string query = @"INSERT INTO tbl_QuizStarSubscription (Msisdn, Fbid, RegDate, TimeStamp, LastChargeDate, RegStatus, RegistrationType, SubscriptionRequestId, ReistrationMedium)
                 VALUES (@Msisdn, @Fbid, @RegDate, @TimeStamp, @LastChargeDate, @RegStatus, @RegistrationType, @SubscriptionRequestId, @RegistrationMedium)"; // Getting into bangla way for speed

            if (msisdn != null && msisdn != "")
            {
                //using (var db = new BasketContext())
                //{
                //    var res = await db.TblQuizStarSubscriptions.AddAsync(new TblQuizStarSubscription()
                //    {
                //        Msisdn = msisdn,
                //        Fbid = msisdn,
                //        RegDate = DateTime.Now,
                //        TimeStamp = DateTime.Now,
                //        LastChargeDate = DateTime.Now,
                //        RegStatus = 1,
                //        RegistrationType = 2,
                //        SubscriptionRequestId = subReqId,
                //        ReistrationMedium = registrationMedium
                //    });
                //    await db.SaveChangesAsync();
                //}

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create the SqlCommand object with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set the parameter values
                        command.Parameters.AddWithValue("@Msisdn", msisdn);
                        command.Parameters.AddWithValue("@Fbid", msisdn);
                        command.Parameters.AddWithValue("@RegDate", DateTime.Now);
                        command.Parameters.AddWithValue("@TimeStamp", DateTime.Now);
                        command.Parameters.AddWithValue("@LastChargeDate", DateTime.Now);
                        command.Parameters.AddWithValue("@RegStatus", 1);
                        command.Parameters.AddWithValue("@RegistrationType", 2);
                        command.Parameters.AddWithValue("@SubscriptionRequestId", subReqId);
                        command.Parameters.AddWithValue("@RegistrationMedium", registrationMedium);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                    }
                }
            }
            else
            {
                using (var dbContext = new BkashPaymentGateWayContext())
                {
                    //var Msi = await dbContext.SubscriptionRequestDatas.Where(x => x.SubscriptionRequestId == subReqId).OrderByDescending(x => x.TimeStamp).Select(x => x.SubscriptionReference).FirstOrDefaultAsync();
                    var Msi = await dbContext.SubscriptionRequestDatasV3s.Where(x => x.SubscriptionRequestId == subReqId).OrderByDescending(x => x.TimeStamp).Select(x => x.SubscriptionReference).FirstOrDefaultAsync();

                    //var res = await _basketContext.TblQuizStarSubscriptions.AddAsync(new TblQuizStarSubscription()
                    //{
                    //    Msisdn = Msi,
                    //    Fbid = Msi,
                    //    RegDate = DateTime.Now,
                    //    TimeStamp = DateTime.Now,
                    //    LastChargeDate = DateTime.Now,
                    //    RegStatus = 1,
                    //    RegistrationType = 2,
                    //    SubscriptionRequestId = subReqId,
                    //    ReistrationMedium = registrationMedium
                    //});


                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Create the SqlCommand object with the query and connection
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Set the parameter values
                            command.Parameters.AddWithValue("@Msisdn", Msi);
                            command.Parameters.AddWithValue("@Fbid", Msi);
                            command.Parameters.AddWithValue("@RegDate", DateTime.Now);
                            command.Parameters.AddWithValue("@TimeStamp", DateTime.Now);
                            command.Parameters.AddWithValue("@LastChargeDate", DateTime.Now);
                            command.Parameters.AddWithValue("@RegStatus", 1);
                            command.Parameters.AddWithValue("@RegistrationType", 2);
                            command.Parameters.AddWithValue("@SubscriptionRequestId", subReqId);
                            command.Parameters.AddWithValue("@RegistrationMedium", registrationMedium);

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();

                        }
                    }
                    //await _basketContext.SaveChangesAsync();
                }
            }


            return "Success";

        }
       
        ///Hitting Bkash API


        public string SubscriptionQueryAsync(string subscriptionRequestId)
        {


            string msisdn, ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, serviceName, cKey, sourceurl2;
            //GeneralInfo(out msisdn, out ua, out ip, out HS_MANUFAC, out HS_MOD, out HS_DIM, out HS_OS, out serviceName, out cKey, out sourceurl2);

            try
            {
                var accept = "application/json";
                var version = "v1.0";
                var channelId = "Merchant WEB";
                var xApiKey = "KN71D_oUMJdmy6IGYkYHsIblYM0deaWo";
                var contentType = "application/json";
                var Url = "https://gateway.recurring.pay.bka.sh/gateway/api/subscriptions/request-id/";
                var timeStamp = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ");


                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(Url + subscriptionRequestId);


                request.Headers.Add("Accept", accept);
                request.Headers.Add("version", version);
                request.Headers.Add("channelId", channelId);
                request.Headers.Add("timeStamp", timeStamp);
                request.Headers.Add("x-api-key", xApiKey);
                request.Headers.Add("Content-Type", contentType);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                Console.WriteLine(response.StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();

                return responseFromServer ;

            }
            catch (Exception ex)
            {
                return "";
            }

            return "";
        }



        public async Task<IActionResult> GetRecentPaymentStatus(string subscriptionRequestId)
        {
            try
            {
                var accept = _configuration.GetSection("Bkash_Recurring:Subscription:Accept").Value;
                var version = _configuration.GetSection("Bkash_Recurring:Subscription:version").Value;
                var channelId = _configuration.GetSection("Bkash_Recurring:Subscription:channelId").Value;
                var xApiKey = _configuration.GetSection("Bkash_Recurring:Subscription:x-api-key").Value;
                var contentType = _configuration.GetSection("Bkash_Recurring:Subscription:Content-Type").Value;
                string Url = _configuration.GetSection("Bkash_Recurring:Subscription:GetPaymentListBySubId").Value;
                var timeStamp = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ");

                string subscriptionId = JsonConvert.DeserializeObject<GetSubIdModel>(SubscriptionQueryAsync(subscriptionRequestId)).id.ToString();

                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(Url + subscriptionId);

                //-------------Header----------
                request.Headers.Add("Accept", accept);
                request.Headers.Add("version", version);
                request.Headers.Add("channelId", channelId);
                request.Headers.Add("timeStamp", timeStamp);
                request.Headers.Add("x-api-key", xApiKey);
                request.Headers.Add("Content-Type", contentType);


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                Console.WriteLine(response.StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                var converted = JsonConvert.DeserializeObject<List<GetPaymentListBySubIdModel>>(responseFromServer);

                GetPaymentListBySubIdModel RecentStatus = converted.Last();

                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();


                // Display the content.
                //return (RecentStatus.Status );
                return Ok(new { message = RecentStatus.Status }); 
            }

            catch (Exception ex)
            {
                //return "FAILED_PAYMENT";
                return Ok(new { message = "FAILED_PAYMENT" });
            }

        }



        [HttpGet]
        public async Task<IActionResult> FindSubscriptionId(string subscriptionRequestId)
        {
            var subResponse = SubscriptionQueryAsync(subscriptionRequestId);
            var status = JsonConvert.DeserializeObject<GetSubIdModel>(subResponse).status;
            return Ok(new { message = status });
        }

        [HttpGet]
        public async Task<IActionResult> PaymentResult ()
        {
            List<PaymentStatus> status = new List<PaymentStatus>();
            try
            {
                SqlConnection cn = new SqlConnection("server=103.134.68.67;database=Basket;user=sa;pwd=adplayVu@1234;");
                cn.Open();
                string searchQuery = "SELECT SubscriptionRequestId  FROM [Basket].[dbo].[tbl_QuizStarSubscriptionAppInApp] where CAST (TimeStamp as date)= CAST (GETDATE()-1 as date) order by TimeStamp desc";
                SqlCommand cmd = new SqlCommand(searchQuery, cn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    
                    try
                    {
                        string subreqid = reader.GetString(0);
                        string statusM = JsonConvert.DeserializeObject<GetSubIdModel>(SubscriptionQueryAsync(reader.GetString(0))).status.ToString();
                        if (subreqid != null || statusM != null || statusM != "")
                        {
                            PaymentStatus paymentStatus = new PaymentStatus
                            {
                                SubscriptionRequestId = reader.GetString(0),
                                Status = JsonConvert.DeserializeObject<GetSubIdModel>(SubscriptionQueryAsync(reader.GetString(0))).status.ToString()
                            };
                            status.Add(paymentStatus);
                        }
                    }
                    catch (Exception ex)
                    {
                        PaymentStatus paymentStatus = new PaymentStatus
                        {
                            SubscriptionRequestId = reader.GetString(0),
                            Status = null
                        };
                        status.Add(paymentStatus);
                    }
                    
                    Thread.Sleep(3000);
                }
                cn.Close();
                string jsonResult = JsonConvert.SerializeObject(status, Formatting.Indented);

                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                string jsonResult = JsonConvert.SerializeObject(status, Formatting.Indented);
                return Json(jsonResult);
            }
            

        }


        [HttpGet]
        public async Task<IActionResult> FindSubscriptionIdJson(string subscriptionRequestId)
        {
            var subResponse = SubscriptionQueryAsync(subscriptionRequestId);
            var status = JsonConvert.DeserializeObject<GetSubIdModel>(subResponse);
            return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> UnSubscriptionWithTrxId(string trxid)
        {

            //var subReqId = subscriptionReqId;
            ////var token = LoginToken();
            //var subId = GetSubId(subReqId);
            string command = "select subscriptionRequestId from BkashPaymentGateway.dbo.WebHookSubscriptionDatasV3 with (nolock) where trxId ='" + trxid + "'";
            SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=BkashPaymentGateway;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
            con.Open();
            SqlCommand cmd = new SqlCommand(command,con);
            string subReqId = cmd.ExecuteScalar().ToString();
            con.Close();
            var subResponse = SubscriptionQueryAsync(subReqId);
            var subId = JsonConvert.DeserializeObject<GetSubIdModel>(subResponse).id;
            //var subId = "";
            string subCancelString = CancelSubscriptionAsync(subId.ToString(), "Manual Unsubscription");
            return Ok(subCancelString);

        }

        public string CancelSubscriptionAsync(string subscriptionId, string reason)
        {


            string msisdn, ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, serviceName, cKey, sourceurl2;

            var accept = "application/json";
            var version = "v1.0";
            var channelId = "Merchant WEB";
            var xApiKey = "KN71D_oUMJdmy6IGYkYHsIblYM0deaWo";
            var contentType = "application/json";
            var Url = "https://gateway.recurring.pay.bka.sh/gateway/api/subscriptions/";
            var timeStamp = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ");

            var client = new RestClient(Url + subscriptionId + "/?reason=" + reason);
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Accept", accept);
            request.AddHeader("version", version);
            request.AddHeader("channelId", channelId);
            request.AddHeader("timeStamp", timeStamp);
            request.AddHeader("x-api-key", xApiKey);
            request.AddHeader("Content-Type", contentType);

            IRestResponse response = client.Execute(request);

            return response.Content;
        }


        ///Bkash APi
        public string GetSubId(string subRequestId)
        {

            try
            {
                var client = new RestClient("https://bkashpaymentapi.shabox.mobi/api/RecurringPayment/SubscriptionQuery?subId=" + subRequestId);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                //request.AddHeader("Authorization", "Bearer " + token);
                IRestResponse response = client.Execute(request);
                var result = JsonConvert.DeserializeObject<GetSubIdModel>(response.Content);

                return result.id.ToString();
            }
            catch (Exception ex)
            {

                return "";
            }
            

        }

        [HttpPost]
        public async Task<IActionResult> UnSubscriptionFromMultiTournament([FromQuery] string msisdn, string serviceName, [FromQuery] string reason = "TestCancelSubscription")
        {
            //var subData = _context.SubscriptionRequestDatas.Where(x => x.SubscriptionReference == msisdn || x.SubscriptionReference == "88" + msisdn).FirstOrDefault();


            dynamic sub = null;
            sub = _basketContext.TblQuizStarSubscriptionAppInApps.Where(x => (x.Msisdn == msisdn || x.Msisdn == "88" + msisdn) && x.DeactivationDate == null && x.RegStatus == 1).OrderByDescending(x => x.TimeStamp).FirstOrDefault();
           

            if (sub != null)
            {
                //var subReqId = subData.SubscriptionRequestId;
                var subReqId = sub.SubscriptionRequestId;
                //var token = LoginToken();
                //var subId = GetSubId(subReqId);
                var subResponse = SubscriptionQueryAsync(subReqId);
                //var result = JsonConvert.DeserializeObject<GetSubIdModel>(subResponse) ;
                var subId = JsonConvert.DeserializeObject<GetSubIdModel>(subResponse).id ;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                string subCancelString = CancelSubscriptionAsync(subId.ToString() , "Baler service");

              
                using (var db = new BasketContext())
                {
                    var resSub = await db.TblQuizStarSubscriptionAppInApps.Where(x => (x.Msisdn == msisdn || x.Msisdn == "88" + msisdn) && x.DeactivationDate == null && x.RegStatus == 1).OrderByDescending(x => x.TimeStamp).FirstOrDefaultAsync();

                    if (resSub != null)
                    {

                        resSub.RegStatus = -1;
                        resSub.DeactivationDate = DateTime.Now;


                        await db.SaveChangesAsync();
                    }

                }


                return Ok(subCancelString);

                //return Ok("success");
            }
            else
            {
                return Ok("Please first do subscription");
            }


        }
        public string YesterDayScore(string MSISDN)
        {


            if(MSISDN == "null")
            {
                return "0";
            }
            if (MSISDN == null)
            {
                return "0";
            }
            else
            {
                string connectionString = "server=103.134.68.67;database=WapPortal;user=sa;pwd=adplayVu@1234;";
                string searchQuery = "select Score from WapPortal.dbo.tbl_GameSpecipic_LeaderBoard_for_Special_Tournament where MSISDN = @MSISDN and CAST (TimeStamp as date)= CAST (GETDATE()-1 as date) and ServiceName = 'GameStarTournamentGame5'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(searchQuery, connection))
                    {
                        command.Parameters.AddWithValue("@MSISDN", MSISDN);

                        object result = command.ExecuteScalar();
                        connection.Close();
                        return result.ToString();
                    }
                }
            }
        }


        public string DailyScore(string MSISDN )
        {
            

            if (MSISDN == "null")
            {
                return "0";
            }
            if (MSISDN == null)
            {
                return "0";
            }
            else
            {
                string connectionString = "server=103.134.68.67;database=WapPortal;user=sa;pwd=adplayVu@1234;";
                string searchQuery = "select Score from WapPortal.dbo.tbl_GameSpecipic_LeaderBoard_for_Special_Tournament where MSISDN = @MSISDN and CAST (TimeStamp as date)= CAST (GETDATE() as date) and ServiceName = 'GameStarTournamentGame5'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(searchQuery, connection))
                    {
                        command.Parameters.AddWithValue("@MSISDN", MSISDN);
                       
                        object result = command.ExecuteScalar();
                        connection.Close();
                        return result.ToString();
                    }
                }
            }
        }

        public ActionResult BkashPayment()
        {

            ViewBag.Amount = "1.0";
            ViewBag.Intent = "sale";
            ViewBag.Currency = "BDT";
            ViewBag.MerchantInvoiceNumber = DateTime.Now.ToString("yyyyMMddhhmmss");

            return View();
        }


        public ActionResult Success()
        {

            return View();
        }


        public ActionResult ErrorPage()
        {

            return View();
        }

        public ActionResult WordMixupGame()
        {

            return View();
        }

        public ActionResult SorryPage()
        {

            return View();
        }

        public ActionResult OnCloseModal()
        {

            return View();
        }

        public ActionResult PaymentStatusModal()
        {
            ViewBag.UserName = "";
            return View();

        }

        public async Task<IActionResult> PaymentStatus([FromQuery] string fbid, [FromQuery] string serviceName)
        {
            using (var entity = new BasketContext())
            {
                var result = await entity.CheckoutUserDatas.Where(x => (x.Msisdn == fbid || x.Msisdn=="88"+ fbid) && x.ServiceName== serviceName).ToListAsync();

                return Ok(new { result = result });
            }
            // return View();
        }

        public async Task<IActionResult> HappyHourBannerLogic()
        {
            using (var entity = new BasketContext())
            {
                var currentDay = DateTime.Now.Date;

                try
                {
                    var res = await entity.sp_HappyHourBannerSelections.FromSqlRaw("EXEC [sp_HappyHourBannerSelections]").ToListAsync();
                    var result = res.FirstOrDefault();
                    //var src = "https://"+ _httpContextAccessor.HttpContext.Request.Host +"/Assets/HH"+ result.BannerNo+".jpg";
                    var src = "https://"+ _httpContextAccessor.HttpContext.Request.Host + "/Assets/HHFinal.jpg";

                    return Ok(new { result = src, startTime = result.startTime, endTime = result.endTime });
                }
                catch (Exception ex)
                {
                    return Ok(new { exception = ex.Message });

                }
                
            }
            // return View();
        }


        public async Task<IActionResult> SpecialQuizPlayQountChecking(string quizType,string msisdn)
        {

            using (var entity = new BasketContext())
            {

                try
                {
                    var res = await entity.GeneralProcedureReturnTypes.FromSqlRaw("EXEC [sp_BkashQuizMasterSpecialQuizPlayQountChecking] @serviceName, @quizType, @msisdn", new SqlParameter("serviceName", "QuizMaster"), new SqlParameter("quizType", quizType), new SqlParameter("msisdn", msisdn)).FirstOrDefaultAsync();
                    //var result = res.FirstOrDefault();
                    //var src = "https://"+ _httpContextAccessor.HttpContext.Request.Host +"/Assets/HH"+ result.BannerNo+".jpg";

                    return Ok(new { result = res.responseInt, });


                }
                catch (Exception ex)
                {
                    return Ok(new { exception = ex.Message });

                }

            }
            // return View();
        }



        public async Task<IActionResult> HappyHourBannerTimeChecking()
        {
            using (var entity = new BasketContext())
            {
                var currentDay = DateTime.Now.ToShortDateString(); 
                var currentDate = DateTime.Now.ToString("hh:mm tt");

                try
                {
                    var res = await entity.sp_HappyHourBannerSelections.FromSqlRaw("EXEC [sp_HappyHourBannerSelections]").ToListAsync();
                    var result = res.FirstOrDefault();
                    //var src = "https://"+ _httpContextAccessor.HttpContext.Request.Host +"/Assets/HH"+ result.BannerNo+".jpg";

                    DateTime currentTime = DateTime.Parse(currentDay+" "+ currentDate);

                    DateTime FirstDateCompare = DateTime.Parse(currentDay + " "+ result.startTime);
                    DateTime EndDateCompare = DateTime.Parse(currentDay + " "+ result.endTime);

                    if (FirstDateCompare <= currentTime && currentTime <= EndDateCompare)
                    {
                        return Ok(new { result = "true", startTime = result.startTime, endTime = result.endTime });
                    }
                    else
                    {
                        return Ok(new { result = "false", startTime = result.startTime, endTime = result.endTime });
                    }


                    
                }
                catch (Exception ex)
                {
                    return Ok(new { exception = ex.Message });

                }

            }
            // return View(); localhost:5001
        }

        public async Task<IActionResult> BkashQuizMasterQuizTimeChecking(string quiztype)
        {
            //using (var entity = new BasketContext())
            //{
                var currentDay = DateTime.Now.ToShortDateString(); 
                var currentDate = DateTime.Now.ToString("hh:mm tt");

                try
                {
                    //var res = await entity.sp_HappyHourBannerSelections.FromSqlRaw("EXEC [sp_BkashQuizMasterQuizTimeSelections] @QuizType", new SqlParameter("QuizType", quiztype)).ToListAsync();

                    //var result = res.FirstOrDefault();

                    //DateTime currentTime = DateTime.Parse(currentDay + " " + currentDate);

                    //DateTime FirstDateCompare = DateTime.Parse(currentDay + " " + result.startTime);
                    //DateTime EndDateCompare = DateTime.Parse(currentDay + " " + result.endTime);

                    //if (FirstDateCompare <= currentTime && currentTime <= EndDateCompare)
                    //{
                    //    return Ok(new { result = "true", startTime = result.startTime, endTime = result.endTime });
                    //}
                    //else
                    //{
                    //    return Ok(new { result = "false", startTime = result.startTime, endTime = result.endTime });
                    //}

                    //var res = await _wapPortalContext.sp_BkashQuizMasterLiveQuizTimeSelection.FromSqlRaw("EXEC [sp_BkashQuizMasterLiveQuizTimeSelection] @QuizType", new SqlParameter("QuizType", quiztype)).ToListAsync();
                    var res = await _basketContext.sp_HappyHourBannerSelections.FromSqlRaw("EXEC [sp_BkashQuizMasterQuizTimeSelections] @QuizType", new SqlParameter("QuizType", quiztype)).ToListAsync();

                var fianlResult = new List<HappyHourMultipleTimeCheck>();
                //var fianlResult = new HappyHourMultipleTimeCheck();

                //fianlResult.result = "true";
                //fianlResult.SlNo = res[0].SlNo.ToString();
                //fianlResult.startTime = res[0].startTime;

                foreach (var item in res)
                {
                    var response = new HappyHourMultipleTimeCheck();
                    DateTime currentTime = DateTime.Parse(currentDay + " " + currentDate);

                    DateTime FirstDateCompare = DateTime.Parse(currentDay + " " + item.startTime);
                    DateTime EndDateCompare = DateTime.Parse(currentDay + " " + item.endTime);

                    if (FirstDateCompare <= currentTime && currentTime <= EndDateCompare)
                    {
                        response.result = "true";
                        response.startTime = item.startTime;
                        response.endTime = item.endTime;
                        response.SlNo = item.SlNo.ToString();

                        fianlResult.Add(response);
                        //return Ok(new { result = "true", startTime = result.startTime, endTime = result.endTime });
                    }
                    else
                    {
                        response.result = "false";
                        response.startTime = item.startTime;
                        response.endTime = item.endTime;
                        response.SlNo = item.SlNo.ToString();

                        fianlResult.Add(response);
                        //return Ok(new { result = 
                        //return Ok(new { result = "false", startTime = result.startTime, endTime = result.endTime });
                    }
                }

                return Ok(new { response = fianlResult });

                }
                catch (Exception ex)
                {
                    return Ok(new { exception = ex.Message });

                }

            //}
            // return View();
        }




        public ActionResult UnSubscriptionView()
        {
            ViewBag.Panel = "UnSubscription";
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> SaveUserInfo([FromBody] UserInfo user)
        {
            try
            {
                var existingUser = await _basketContext.TblQuizMasterUserInfos.Where(x => (x.Msisdn == user.MSISDN || x.Msisdn == "88" + user.MSISDN)).FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    existingUser.SourceUrl = user.SourceUrl;
                    existingUser.TimeStamp = DateTime.Now;
                    if (user.Name != "" && user.Name != null)
                    {
                        existingUser.Name = user.Name;
                    }
                    if (user.FPToken != "" && user.FPToken != null && user.FPToken != "null" )
                    {
                        existingUser.PushNotificationToken = user.FPToken;
                    }

                    await _basketContext.SaveChangesAsync();
                    return Ok(new { response = "true", newUser = 0 });
                }
                else
                {
                    var TblQuizMasterUserInfo = new TblQuizMasterUserInfo();

                    TblQuizMasterUserInfo.Msisdn = user.MSISDN;
                    TblQuizMasterUserInfo.Name = user.Name;
                    TblQuizMasterUserInfo.SourceUrl = user.SourceUrl;
                    TblQuizMasterUserInfo.TimeStamp = DateTime.Now;
                    if (user.FPToken != "" && user.FPToken != null && user.FPToken != "null")
                    {
                        TblQuizMasterUserInfo.PushNotificationToken = user.FPToken;
                    }

                    await _basketContext.TblQuizMasterUserInfos.AddAsync(TblQuizMasterUserInfo);
                    await _basketContext.SaveChangesAsync();

                    return Ok(new { response = "true", newUser = 1 });
                    //if (user.IsSignup == 1)
                    //{
                    //    await _basketContext.TblQuizMasterUserInfos.AddAsync(new TblQuizMasterUserInfo { Msisdn = user.MSISDN, Name = user.Name, SourceUrl = user.SourceUrl, TimeStamp = DateTime.Now });
                    //    await _basketContext.SaveChangesAsync();
                    //}
                    //else
                    //{
                    //    return Ok(new { response = "false"});
                    //}

                }

                return Ok(true);
            }
            catch (Exception ex)
            {

            }
            return Ok(false);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCheckoutUrl([FromQuery] string msisdn, string ckey = "000000")
        {
            try
            {
                //var getSession = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("SessionUser"));

                var getSession = await _context.SessionData.Where(x => x.Msisdn == msisdn && x.ServiceName == "QuizMaster").FirstOrDefaultAsync();

                    try
                    {
                        var headerReferer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
                        if (headerReferer == "")
                        {
                            headerReferer = "https://localhost:5001/";
                        }
                        if (headerReferer == "https://localhost:5007/Home/BkashPayment" || headerReferer == "https://quizmasterappinapp.shabox.mobi/Home/BkashPayment" || headerReferer == "https://gamestar.shabox.mobi/Home/BkashPayment" || headerReferer.Contains("https://quizmaster.shabox.mobi") || headerReferer.Contains("https://quizmastertest.shabox.mobi") || headerReferer.Contains("https://quizmasterappinappbkash.shabox.mobi") || headerReferer.Contains("https://localhost:5007") || headerReferer.Contains("https://quizmasterappinapp.shabox.mobi") || headerReferer.Contains("https://localhost:5011") || headerReferer.Contains("https://localhost:5018") || headerReferer.Contains("https://localhost:5001"))
                        {

                            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                            string url = _configuration.GetSection("BkashPayment:createCheckoutUrl").Value;
                            var token = LoginToken();
                            //////////////

                            var client = new RestClient(url);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("Authorization", "Bearer " + token);
                            request.AddHeader("Content-Type", "application/json");

                            
                            //Console.WriteLine(response.Content);

                            //////////////////
                            //// Create a request using a URL that can receive a post.
                            //WebRequest request = WebRequest.Create(url);

                            //// Set the Method property of the request to POST.
                            //request.Method = "POST";

                            //request.Headers.Add("Authorization", "Bearer " + token);
                            //request.Headers.Add("Content-Type", "application/json");

                            var startDate = DateTime.Now.ToString("yyyy-MM-dd");
                            var presentTime = DateTime.Now.ToString("hh:mm:ss tt");
                            TimeSpan startTime = new TimeSpan(23, 30, 0); // 11:30 PM
                            TimeSpan endTime = new TimeSpan(0, 0, 0); //12:00 AM
                            TimeSpan currentTime = DateTime.Now.TimeOfDay;
                            //if (currentTime >= startTime || currentTime < endTime)
                            //{
                            //    startDate = DateTime.Now.AddMinutes(40).ToString("yyyy-MM-dd");
                            //// Your logic here for the specified time range
                            //}
                            if(DateTime.Now.Hour==23 && DateTime.Now.Minute >= 30)
                            {
                                startDate= DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                            }
                            var endDate = DateTime.Now.AddDays(700).ToString("yyyy-MM-dd");
                            
                            var subReqId = "";

                            if (ckey == "009999" || ckey == "008888")
                            {
                                subReqId = "QuizMaster-" + Guid.NewGuid().ToString();
                            }
                            else
                            {
                                subReqId = "QuizMaster-Campaign" + Guid.NewGuid().ToString();
                            }

                            var bkashCredentialData = await _context.BkashSecrets.Where(x => x.ServiceName == "QuizMaster").FirstOrDefaultAsync();

                            //--------- For json type data ---------
                            var data = new
                            {
                                amount = bkashCredentialData.Amount.ToString(),
                                //firstPaymentAmount = "null",
                                firstPaymentIncludedInCycle = bkashCredentialData.FirstPaymentIncludedInCycle,
                                serviceId = bkashCredentialData.ServiceId,
                                currency = bkashCredentialData.Currency,
                                startDate = startDate,
                                expiryDate = endDate,
                                frequency = "WEEKLY",
                                subscriptionType = bkashCredentialData.SubscriptionType,
                                //maxCapAmount= "null",
                                maxCapRequired = bkashCredentialData.MaxCapRequired,
                                merchantShortCode = bkashCredentialData.MerchantShortCode,
                                //payer = "01770618575",
                                payerType = bkashCredentialData.PayerType,
                                paymentType = bkashCredentialData.PaymentType,
                                redirectUrl = "https://quizmasterappinapp.shabox.mobi/Home/Success?subReqId=" + subReqId,
                                //redirectUrl = "https://quizmaster.shabox.mobi/Home/Success?subReqId=" + subReqId,   // Need to change Later For Publish
                                subscriptionRequestId = subReqId,
                                subscriptionReference = msisdn,
                                ckey = ckey
                                //extraParams= "hello"

                            };


                            string postData = JsonConvert.SerializeObject(data);

                            //byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                            //// Set the ContentType property of the WebRequest.
                            //request.ContentType = "application/json";

                            //// Set the ContentLength property of the WebRequest.
                            //request.ContentLength = byteArray.Length;

                            //// Get the request stream.
                            //Stream dataStream = request.GetRequestStream();
                            //// Write the data to the request stream.
                            //dataStream.Write(byteArray, 0, byteArray.Length);
                            //// Close the Stream object.
                            //dataStream.Close();

                            //// Get the response.
                            //WebResponse response = request.GetResponse();
                            //// Display the status.
                            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                            request.AddParameter("application/json", postData, ParameterType.RequestBody);

                            IRestResponse response = client.Execute(request);

                            // Get the stream containing content returned by the server.
                            // The using block ensures the stream is automatically closed.


                            var result = JsonConvert.DeserializeObject<SubResponse>(response.Content);

                            return Ok(result.RedirectURL);

                            //using (dataStream = response.GetResponseStream())
                            //{
                            //    // Open the stream using a StreamReader for easy access.
                            //    StreamReader reader = new StreamReader(dataStream);
                            //    // Read the content.
                            //    string responseFromServer = reader.ReadToEnd();

                            //    var result = JsonConvert.DeserializeObject<SubResponse>(responseFromServer);

                            //    return Ok(result.RedirectURL);

                            //    //return Ok(responseFromServer);
                            //}
                        }

                        else
                        {
                            return Unauthorized();
                        }
                    }
                    catch (Exception ex)
                    {
                        return Ok(ex.Message);
                    }
                



            }
            catch (Exception ex)
            {
                //
            }

            return Unauthorized();

        }



        [HttpPost]
        public async Task<IActionResult> CreateCheckoutUrlHardCode([FromQuery] string msisdn, string ckey = "000000")
        {
            try
            {
         

                var getSession = await _context.SessionData.Where(x => x.Msisdn == msisdn && x.ServiceName == "QuizMaster").FirstOrDefaultAsync();

                try
                {
                    var headerReferer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
                    if (headerReferer == "")
                    {
                        headerReferer = "https://localhost:5001/";
                    }
                    if (headerReferer == "https://localhost:5007/Home/BkashPayment" || headerReferer == "https://quizmaster.shabox.mobi/Home/BkashPayment" || headerReferer == "https://gamestar.shabox.mobi/Home/BkashPayment" || headerReferer.Contains("https://quizmaster.shabox.mobi") || headerReferer.Contains("https://quizmastertest.shabox.mobi") || headerReferer.Contains("https://quizmasterappinappbkash.shabox.mobi") || headerReferer.Contains("https://localhost:5007") || headerReferer.Contains("https://quizmasterappinapp.shabox.mobi") || headerReferer.Contains("https://localhost:5011") || headerReferer.Contains("https://localhost:5018") || headerReferer.Contains("https://localhost:5001"))
                    {

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        string url = _configuration.GetSection("BkashPayment:createCheckoutUrl").Value;
                        var token = LoginToken();
                        //////////////

                        var client = new RestClient(url);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Authorization", "Bearer " + token);
                        request.AddHeader("Content-Type", "application/json");


                        //Console.WriteLine(response.Content);

                        //////////////////
                        //// Create a request using a URL that can receive a post.
                        //WebRequest request = WebRequest.Create(url);

                        //// Set the Method property of the request to POST.
                        //request.Method = "POST";

                        //request.Headers.Add("Authorization", "Bearer " + token);
                        //request.Headers.Add("Content-Type", "application/json");

                        var startDate = DateTime.Now.ToString("yyyy-MM-dd");
                        var presentTime = DateTime.Now.ToString("hh:mm:ss tt");
                        TimeSpan startTime = new TimeSpan(23, 30, 0); // 11:30 PM
                        TimeSpan endTime = new TimeSpan(0, 0, 0); //12:00 AM
                        TimeSpan currentTime = DateTime.Now.TimeOfDay;
                        //if (currentTime >= startTime || currentTime < endTime)
                        //{
                        //    startDate = DateTime.Now.AddMinutes(40).ToString("yyyy-MM-dd");
                        //// Your logic here for the specified time range
                        //}
                        if (DateTime.Now.Hour == 23 && DateTime.Now.Minute >= 30)
                        {
                            startDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                        }
                        var endDate = DateTime.Now.AddDays(700).ToString("yyyy-MM-dd"); 

                        var subReqId = "";

                        if (ckey == "009999" || ckey == "008888")
                        {
                            subReqId = "QuizMaster-" + Guid.NewGuid().ToString();
                        }
                        else
                        {
                            subReqId = "QuizMaster-Campaign" + Guid.NewGuid().ToString();
                        }

                        var bkashCredentialData = await _context.BkashSecrets.Where(x => x.ServiceName == "QuizMaster").FirstOrDefaultAsync();

                        //--------- For json type data ---------
                        var data = new
                        {
                            amount = bkashCredentialData.Amount.ToString(),
                            //firstPaymentAmount = "null",
                            firstPaymentIncludedInCycle = bkashCredentialData.FirstPaymentIncludedInCycle,
                            serviceId = bkashCredentialData.ServiceId,
                            currency = bkashCredentialData.Currency,
                            startDate = startDate,
                            expiryDate = endDate,
                            frequency = "WEEKLY",
                            subscriptionType = bkashCredentialData.SubscriptionType,
                            //maxCapAmount= "null",
                            maxCapRequired = bkashCredentialData.MaxCapRequired,
                            merchantShortCode = bkashCredentialData.MerchantShortCode,
                            //payer = "01770618575",
                            payerType = bkashCredentialData.PayerType,
                            paymentType = bkashCredentialData.PaymentType,
                            redirectUrl = "https://quizmasterappinapp.shabox.mobi/Home/Success?subReqId=" + subReqId,
                            //redirectUrl = "https://quizmaster.shabox.mobi/Home/Success?subReqId=" + subReqId,   // Need to change Later For Publish
                            subscriptionRequestId = subReqId,
                            subscriptionReference = msisdn,
                            ckey = ckey
                            //extraParams= "hello"

                        };


                        string postData = JsonConvert.SerializeObject(data);

                        //byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                        //// Set the ContentType property of the WebRequest.
                        //request.ContentType = "application/json";

                        //// Set the ContentLength property of the WebRequest.
                        //request.ContentLength = byteArray.Length;

                        //// Get the request stream.
                        //Stream dataStream = request.GetRequestStream();
                        //// Write the data to the request stream.
                        //dataStream.Write(byteArray, 0, byteArray.Length);
                        //// Close the Stream object.
                        //dataStream.Close();

                        //// Get the response.
                        //WebResponse response = request.GetResponse();
                        //// Display the status.
                        //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                        request.AddParameter("application/json", postData, ParameterType.RequestBody);

                        IRestResponse response = client.Execute(request);

                        // Get the stream containing content returned by the server.
                        // The using block ensures the stream is automatically closed.


                        var result = JsonConvert.DeserializeObject<SubResponse>(response.Content);

                        return Ok(result.RedirectURL);

                        //using (dataStream = response.GetResponseStream())
                        //{
                        //    // Open the stream using a StreamReader for easy access.
                        //    StreamReader reader = new StreamReader(dataStream);
                        //    // Read the content.
                        //    string responseFromServer = reader.ReadToEnd();

                        //    var result = JsonConvert.DeserializeObject<SubResponse>(responseFromServer);

                        //    return Ok(result.RedirectURL);

                        //    //return Ok(responseFromServer);
                        //}
                    }

                    else
                    {
                        return Unauthorized();
                    }
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }




            }
            catch (Exception ex)
            {
                //
            }

            return Unauthorized();

        }




        [HttpPost]
        public async Task<IActionResult> CreateCheckoutUrlMultiTournament([FromQuery] string msisdn, string serviceName , string ckey = "000000")
        {
            try
            {
                if(serviceName==null|| serviceName== "undefined")
                {
                    serviceName = "QuizMaster";
                }

                //var getSession = await _context.SessionData.Where(x => x.Msisdn == msisdn && x.ServiceName == "QuizMaster").FirstOrDefaultAsync();

                try
                {
                    var headerReferer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
                    if (headerReferer == "")
                    {
                        headerReferer = "https://localhost:5001/";
                    }
                    if (headerReferer == "https://localhost:5007/Home/BkashPayment" || headerReferer == "https://quizmaster.shabox.mobi/Home/BkashPayment" || headerReferer == "https://gamestar.shabox.mobi/Home/BkashPayment" || headerReferer.Contains("https://quizmaster.shabox.mobi") || headerReferer.Contains("https://quizmastertest.shabox.mobi") || headerReferer.Contains("https://quizmasterappinappbkash.shabox.mobi") || headerReferer.Contains("https://localhost:5007") || headerReferer.Contains("https://quizmasterappinapp.shabox.mobi") || headerReferer.Contains("https://localhost:5011") || headerReferer.Contains("https://localhost:5018") || headerReferer.Contains("https://localhost:5001"))
                    {

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        string url = _configuration.GetSection("BkashPayment:createCheckoutUrl").Value;
                        var token = LoginToken();
                        //////////////

                        var client = new RestClient(url);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Authorization", "Bearer " + token);
                        request.AddHeader("Content-Type", "application/json");


                        //Console.WriteLine(response.Content);

                        //////////////////
                        //// Create a request using a URL that can receive a post.
                        //WebRequest request = WebRequest.Create(url);

                        //// Set the Method property of the request to POST.
                        //request.Method = "POST";

                        //request.Headers.Add("Authorization", "Bearer " + token);
                        //request.Headers.Add("Content-Type", "application/json");

                        var startDate = DateTime.Now.ToString("yyyy-MM-dd");
                        var presentTime = DateTime.Now.ToString("hh:mm:ss tt");
                        TimeSpan startTime = new TimeSpan(23, 30, 0); // 11:30 PM
                        TimeSpan endTime = new TimeSpan(0, 0, 0); //12:00 AM
                        TimeSpan currentTime = DateTime.Now.TimeOfDay;
                        //if (currentTime >= startTime || currentTime < endTime)
                        //{
                        //    startDate = DateTime.Now.AddMinutes(40).ToString("yyyy-MM-dd");
                        //// Your logic here for the specified time range
                        //}
                        if (DateTime.Now.Hour == 23 && DateTime.Now.Minute >= 30)
                        {
                            startDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                        }
                        var endDate = DateTime.Now.AddDays(700).ToString("yyyy-MM-dd");

                        var subReqId = "";

                        if (ckey == "009999" || ckey == "008888")
                        {
                            subReqId = serviceName + "-" + Guid.NewGuid().ToString();
                        }
                        else
                        {
                            subReqId = serviceName + "-Campaign" + Guid.NewGuid().ToString();
                        }

                        
                        // need to change for other campaign

                        var bkashCredentialData = await _context.BkashSecrets.Where(x => x.ServiceName == serviceName).FirstOrDefaultAsync();

                        //--------- For json type data ---------
                        var data = new
                        {
                            amount = bkashCredentialData.Amount.ToString(),
                            //firstPaymentAmount = "null",
                            firstPaymentIncludedInCycle = bkashCredentialData.FirstPaymentIncludedInCycle,
                            serviceId = bkashCredentialData.ServiceId,
                            currency = bkashCredentialData.Currency,
                            startDate = startDate,
                            expiryDate = endDate,
                            frequency = "WEEKLY",
                            subscriptionType = bkashCredentialData.SubscriptionType,
                            //maxCapAmount= "null",
                            maxCapRequired = bkashCredentialData.MaxCapRequired,
                            merchantShortCode = bkashCredentialData.MerchantShortCode,
                            //payer = "01770618575",
                            payerType = bkashCredentialData.PayerType,
                            paymentType = bkashCredentialData.PaymentType,
                            redirectUrl = "https://quizmasterappinapp.shabox.mobi/Home/Success?subReqId=" + subReqId,
                            //redirectUrl = "https://quizmaster.shabox.mobi/Home/Success?subReqId=" + subReqId,   // Need to change Later For Publish
                            subscriptionRequestId = subReqId,
                            subscriptionReference = msisdn,
                            ckey = ckey
                            //extraParams= "hello"

                        };


                        string postData = JsonConvert.SerializeObject(data);


                        request.AddParameter("application/json", postData, ParameterType.RequestBody);

                        IRestResponse response = client.Execute(request);

                        // Get the stream containing content returned by the server.
                        // The using block ensures the stream is automatically closed.


                        var result = JsonConvert.DeserializeObject<SubResponse>(response.Content);

                        return Ok(result.RedirectURL);

                    }

                    else
                    {
                        return Unauthorized();
                    }
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }




            }
            catch (Exception ex)
            {
                //
            }

            return Unauthorized();

        }




        public bool CheckMSISDNExistsInTrafficLog(string MSISDN)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=WapPortal_CMS;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                string Query = "select COUNT(*) from WapPortal_CMS.dbo.tbl_successful_adnetwork_traffic_log where MSISDN= '" + MSISDN + "' and CAST(TIME_STAMP as date) = CAST (GETDATE() as date)";
                con.Open();
                SqlCommand command = new SqlCommand(Query, con);
                int count = (int)command.ExecuteScalar();
                con.Close();
                if (count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        //---Ratul---2023-12-18---New Multi Tournament Checkout URL
        [HttpPost]
        public async Task<IActionResult> SubscriptionV3Insert ([FromBody] SubscriptionModel data)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                string ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, cKey, sourceurl2;

                var accept = "application/json";
                var version = "v1.0";
                var channelId = "Merchant WEB";
                var xAppKey = "";
                var contentType = "application/json";
                var timeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ssZ");
                bool existence = CheckMSISDNExistsInTrafficLog(data.subscriptionReference);

                if (existence == false )
                {
                    try
                    {
                        var client = new HttpClient();
                        //var request = new HttpRequestMessage(HttpMethod.Get, "https://wap.shabox.mobi/aoc/api/PostBackBkash/postbackbkash?msisdn=" + data.subscriptionReference);
                        var request = new HttpRequestMessage(HttpMethod.Get, "https://postbackaoc.shabox.mobi/home/postbackbkash?msisdn=" + data.subscriptionReference);
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        xAppKey = response.EnsureSuccessStatusCode().ToString();
                    }
                    catch (Exception ex)
                    {
                        xAppKey = "Postback Not Done";
                    }
                }
                


                SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=BkashPaymentGateway;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                con.Open();
                SqlCommand cmd = new SqlCommand("proc_SaveSubscriptionRequestDataV3", con);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Accept", accept == null ? DBNull.Value : accept);
                cmd.Parameters.AddWithValue("@Versions", version == null ? DBNull.Value : version);
                cmd.Parameters.AddWithValue("@ChannelId", channelId == null ? DBNull.Value : channelId);
                cmd.Parameters.AddWithValue("@XAppKey", xAppKey == null ? DBNull.Value : xAppKey);
                cmd.Parameters.AddWithValue("@ContentType", contentType == null ? DBNull.Value : contentType);
                cmd.Parameters.AddWithValue("@Amount", data.amount == null ? DBNull.Value : data.amount);
                cmd.Parameters.AddWithValue("@AmountQueryUrl", data.amount == null ? DBNull.Value : data.amount);
                cmd.Parameters.AddWithValue("@FirstPaymentAmount", data.amount == null ? DBNull.Value : data.amount);
                cmd.Parameters.AddWithValue("@FirstPaymentIncludedInCycle", data.firstPaymentIncludedInCycle == null ? DBNull.Value : data.firstPaymentIncludedInCycle);
                cmd.Parameters.AddWithValue("@ServiceId", data.serviceId == null ? DBNull.Value : data.serviceId);
                cmd.Parameters.AddWithValue("@Currency", data.currency == null ? DBNull.Value : data.currency);
                cmd.Parameters.AddWithValue("@StartDate", data.startDate == null ? DBNull.Value : data.startDate);
                cmd.Parameters.AddWithValue("@ExpiryDate", data.expiryDate == null ? DBNull.Value : data.expiryDate);
                cmd.Parameters.AddWithValue("@Frequency", data.frequency == null ? DBNull.Value : data.frequency);
                cmd.Parameters.AddWithValue("@SubscriptionType", data.subscriptionType == null ? DBNull.Value : data.subscriptionType);
                cmd.Parameters.AddWithValue("@MaxCapAmount", data.maxCapRequired == null ? DBNull.Value : data.maxCapRequired);
                cmd.Parameters.AddWithValue("@MaxCapRequired", data.maxCapRequired == null ? DBNull.Value : data.maxCapRequired);
                cmd.Parameters.AddWithValue("@MerchantShortCode", data.merchantShortCode == null ? DBNull.Value : data.merchantShortCode);
                cmd.Parameters.AddWithValue("@Payer", data.subscriptionReference == null ? DBNull.Value : data.subscriptionReference);
                cmd.Parameters.AddWithValue("@PayerType", data.payerType == null ? DBNull.Value : data.payerType);
                cmd.Parameters.AddWithValue("@PaymentType", data.paymentType == null ? DBNull.Value : data.paymentType);
                cmd.Parameters.AddWithValue("@RedirectUrl", data.redirectUrl == null ? DBNull.Value : data.redirectUrl);
                cmd.Parameters.AddWithValue("@SubscriptionRequestId", data.subscriptionRequestId == null ? DBNull.Value : data.subscriptionRequestId);
                cmd.Parameters.AddWithValue("@SubscriptionReference", data.subscriptionReference == null ? DBNull.Value : data.subscriptionReference);
                cmd.Parameters.AddWithValue("@CKEY", data.ckey == null ? DBNull.Value : data.ckey);
                cmd.Parameters.AddWithValue("@SourceUrl", DBNull.Value);
                cmd.Parameters.AddWithValue("@TimeStamp", DateTime.Now);
                cmd.ExecuteNonQuery();
                con.Close();
                

                return Ok("SUCCESS");
            }
            catch (Exception ex)
            {
                return Ok("FAILED_PAYMENT");
            }


        }


        [HttpGet]
        public string GenerateUniQueSubscriptionRequestId(string serviceName, string ckey, string msisdn)
        {
            if (serviceName == null)
            {
                serviceName = "KidStar";
            }
            string subReqId = "";

            var startDate = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute >= 30)
            {
                startDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            var endDate = DateTime.Now.AddDays(720).ToString("yyyy-MM-ddThh:mm:ss");

            if (ckey == "009988")
            {
                subReqId = serviceName.ToString() + Guid.NewGuid().ToString();
            }
            else
            {
                subReqId = serviceName.ToString() + "-Campaign" + Guid.NewGuid().ToString();
            }

            //string amount = "49";
            //if (serviceName == "QuizMaster")
            //{
            //    amount = "19";
            //}
            //if (serviceName == "QuizMaster-MultiService1")
            //{
            //    amount = "49";
            //}
            //if (serviceName == "QuizMaster-MultiService2")
            //{
            //    amount = "29";
            //}
            


            var data = new
            {
                amount = "15",
                //firstPaymentAmount = "null",
                firstPaymentIncludedInCycle = "True",
                serviceId = "100001",
                currency = "BDT",
                startDate = startDate,
                expiryDate = endDate,
                frequency = "WEEKLY",
                subscriptionType = "BASIC",
                //maxCapAmount= "null",
                maxCapRequired = "False",
                //merchantShortCode = "01313717498",
                merchantShortCode = "01884104842",
                //payer = "01770618575",
                payerType = "CUSTOMER",
                paymentType = "FIXED",
                redirectUrl = "https://kidstar.shabox.mobi/Home/Success?subReqId=" + subReqId,
                subscriptionRequestId = subReqId,
                subscriptionReference = msisdn,
                ckey = ckey
                //extraParams= "hello"

            };

            //string encryptedString = EncryptString(_configuration.GetSection("Bkash_Recurring:Subscription:x-api-key").Value , JsonConvert.SerializeObject(data));

            return JsonConvert.SerializeObject(data);
        }


        // For Campaign 

        [HttpGet]
        public string GenerateUniQueSubscriptionRequestIdForCampaign(string ckey, string msisdn)
        {
            string serviceName = "KidStar";
            string subReqId = "KidStar-Campaign";

            var startDate = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute >= 30)
            {
                startDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            var endDate = DateTime.Now.AddDays(720).ToString("yyyy-MM-ddThh:mm:ss");

            if (ckey == "009988")
            {
                subReqId = serviceName.ToString() + Guid.NewGuid().ToString();
            }
            else
            {
                subReqId = serviceName.ToString() + "-Campaign" + Guid.NewGuid().ToString();
            }

            string amount = "15";

            var data = new
            {
                amount = amount,
              
                firstPaymentIncludedInCycle = "True",
                serviceId = "100001",
                currency = "BDT",
                startDate = startDate,
                expiryDate = endDate,
                frequency = "WEEKLY",
                subscriptionType = "BASIC",
                //maxCapAmount= "null",
                maxCapRequired = "False",
                merchantShortCode = "01884104842",
                //payer = "01770618575",
                payerType = "CUSTOMER",
                paymentType = "FIXED",
                redirectUrl = "https://kidstar.shabox.mobi/Home/Success?subReqId=" + subReqId,
                subscriptionRequestId = subReqId,
                subscriptionReference = msisdn,
                ckey = ckey
                //extraParams= "hello"

            };

            //string encryptedString = EncryptString(_configuration.GetSection("Bkash_Recurring:Subscription:x-api-key").Value , JsonConvert.SerializeObject(data));

            return JsonConvert.SerializeObject(data);
        }

        [HttpPost]
        public async Task<IActionResult> MultiTournamentInBuildCheckoutUrl(string packetData)
        {
            try
            {
                //var getSession = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("SessionUser"));

                //if (!string.IsNullOrEmpty(getSession.Msisdn))
                if (true)
                {
                    try
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };


                        //--------- For json type data ------

                        //New update
                        string ua, ip, HS_MANUFAC, HS_MOD, HS_DIM, HS_OS, cKey, sourceurl2;
                        //GeneralInfo(out msisdn, out ua, out ip, out HS_MANUFAC, out HS_MOD, out HS_DIM, out HS_OS, out serviceName, out cKey, out sourceurl2);


                        var accept = _configuration.GetSection("Bkash_Recurring:Subscription:Accept").Value;
                        var version = _configuration.GetSection("Bkash_Recurring:Subscription:version").Value;
                        var channelId = _configuration.GetSection("Bkash_Recurring:Subscription:channelId").Value;
                        var xAppKey = _configuration.GetSection("Bkash_Recurring:Subscription:x-api-key").Value;
                        var contentType = _configuration.GetSection("Bkash_Recurring:Subscription:Content-Type").Value;
                        var Url = _configuration.GetSection("Bkash_Recurring:Subscription:subscriptionURL").Value;
                        var timeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ssZ");
                        ///

                        WebRequest request = WebRequest.Create(Url);

                        request.Method = "POST";

                        request.Headers.Add("Accept", accept);
                        request.Headers.Add("version", version);
                        request.Headers.Add("channelId", channelId);
                        request.Headers.Add("timeStamp", timeStamp);
                        request.Headers.Add("x-api-key", xAppKey);
                        request.Headers.Add("Content-Type", contentType);

                        //string reQuestData = DecryptString(_configuration.GetSection("Bkash_Recurring:Subscription:x-api-key").Value , packetData);

                        //string postData = JsonConvert.SerializeObject(packetData);

                        byte[] byteArray = Encoding.UTF8.GetBytes(packetData);

                        request.ContentType = "application/json";

                        request.ContentLength = byteArray.Length;

                        Stream dataStream = request.GetRequestStream();
                        // Write the data to the request stream.
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        // Close the Stream object.
                        dataStream.Close();

                        WebResponse response = request.GetResponse();
                        Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                        using (dataStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(dataStream);
                            // Read the content.
                            string responseFromServer = reader.ReadToEnd();

                            var result = JsonConvert.DeserializeObject<SubResponse>(responseFromServer);

                            return Ok(result.RedirectURL);

                            //return Ok(responseFromServer);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Ok(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                //
            }

            return Unauthorized();

        }



        ///



        [HttpPost]
        public async Task<IActionResult> CreateCheckoutUrlMultiTournamentHardCode([FromQuery] string msisdn, string serviceName, string ckey = "000000")
        {
            try
            {
                if (serviceName == null || serviceName == "undefined")
                {
                    serviceName = "QuizMaster";
                }

                //var getSession = await _context.SessionData.Where(x => x.Msisdn == msisdn && x.ServiceName == "QuizMaster").FirstOrDefaultAsync();

                try
                {
                    var headerReferer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
                    if (headerReferer == "")
                    {
                        headerReferer = "https://localhost:5001/";
                    }
                    if (headerReferer == "https://localhost:5007/Home/BkashPayment" || headerReferer == "https://quizmasterappinappbkash.shabox.mobi/Home/BkashPayment" || headerReferer == "https://gamestar.shabox.mobi/Home/BkashPayment" || headerReferer.Contains("https://quizmaster.shabox.mobi") || headerReferer.Contains("https://quizmastertest.shabox.mobi") || headerReferer.Contains("https://quizmasterappinappbkash.shabox.mobi") || headerReferer.Contains("https://localhost:5007") || headerReferer.Contains("https://quizmasterappinapp.shabox.mobi") || headerReferer.Contains("https://localhost:5011") || headerReferer.Contains("https://localhost:5018") || headerReferer.Contains("https://localhost:5001"))
                    {

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        string url = _configuration.GetSection("BkashPayment:createCheckoutUrl").Value;
                        var token = LoginToken();
                        //////////////

                        var client = new RestClient(url);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Authorization", "Bearer " + token);
                        request.AddHeader("Content-Type", "application/json");


                        //Console.WriteLine(response.Content);

                        //////////////////
                        //// Create a request using a URL that can receive a post.
                        //WebRequest request = WebRequest.Create(url);

                        //// Set the Method property of the request to POST.
                        //request.Method = "POST";

                        //request.Headers.Add("Authorization", "Bearer " + token);
                        //request.Headers.Add("Content-Type", "application/json");

                        var startDate = DateTime.Now.ToString("yyyy-MM-dd");
                        var presentTime = DateTime.Now.ToString("hh:mm:ss tt");
                        TimeSpan startTime = new TimeSpan(23, 30, 0); // 11:30 PM
                        TimeSpan endTime = new TimeSpan(0, 0, 0); //12:00 AM
                        TimeSpan currentTime = DateTime.Now.TimeOfDay;
                        //if (currentTime >= startTime || currentTime < endTime)
                        //{
                        //    startDate = DateTime.Now.AddMinutes(40).ToString("yyyy-MM-dd");
                        //// Your logic here for the specified time range
                        //}
                        if (DateTime.Now.Hour == 23 && DateTime.Now.Minute >= 30)
                        {
                            startDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                        }
                        var endDate = DateTime.Now.AddDays(700).ToString("yyyy-MM-dd");

                        var subReqId = "";

                        if (ckey == "009999" || ckey == "008888")
                        {
                            subReqId = serviceName + "-" + Guid.NewGuid().ToString();
                        }
                        else
                        {
                            subReqId = serviceName + "-Campaign" + Guid.NewGuid().ToString();
                        }

                        int amount = 17;
                        if(serviceName == "QuizMaster")
                        {
                             amount = 17;
                        }
                        if (serviceName == "QuizMaster-MultiService1")
                        {
                            amount = 17;
                        }
                        if (serviceName == "QuizMaster-MultiService2")
                        {
                            amount = 17;
                        }


                        // need to change for other campaign

                        //var bkashCredentialData = await _context.BkashSecrets.Where(x => x.ServiceName == serviceName).FirstOrDefaultAsync();

                        //--------- For json type data ---------
                        var data = new
                        {
                            amount = amount,
                            //firstPaymentAmount = "null",
                            firstPaymentIncludedInCycle = "True",
                            serviceId = "100001",
                            currency = "BDT",
                            startDate = startDate,
                            expiryDate = endDate,
                            frequency = "WEEKLY",
                            subscriptionType = "BASIC",
                            //maxCapAmount= "null",
                            maxCapRequired = "False",
                            merchantShortCode = "01884104842",
                            //payer = "01770618575",
                            payerType = "CUSTOMER",
                            paymentType = "FIXED",
                            redirectUrl = "https://quizmasterappinappbkash.shabox.mobi/Home/Success?subReqId=" + subReqId,
                            //redirectUrl = "https://quizmaster.shabox.mobi/Home/Success?subReqId=" + subReqId,   // Need to change Later For Publish
                            subscriptionRequestId = subReqId,
                            subscriptionReference = msisdn,
                            ckey = ckey
                            //extraParams= "hello"

                        };


                        string postData = JsonConvert.SerializeObject(data);


                        request.AddParameter("application/json", postData, ParameterType.RequestBody);

                        IRestResponse response = client.Execute(request);

                        // Get the stream containing content returned by the server.
                        // The using block ensures the stream is automatically closed.


                        var result = JsonConvert.DeserializeObject<SubResponse>(response.Content);

                        return Ok(result.RedirectURL);

                    }

                    else
                    {
                        return Unauthorized();
                    }
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }




            }
            catch (Exception ex)
            {
                //
            }

            return Unauthorized();

        }




        [HttpGet]
        public async Task<IActionResult> CheckSubscriptionWithSubReqId([FromQuery] string msisdn, [FromQuery] string subReqId)
        {
            try
            {
                //var getSession = await _context.SessionData.Where(x => x.Msisdn == msisdn && x.ServiceName == "QuizMaster").FirstOrDefaultAsync();


                //if (!string.IsNullOrEmpty(getSession.Msisdn))
                //{
                    try
                    {

                            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                            string url = "https://bkashpaymentapi.shabox.mobi/api/RecurringPayment/SubscriptionQuery?subId=" + subReqId;
                            
                            var token = LoginToken();

                    ///

                    var client = new RestClient(url);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddHeader("Content-Type", "application/json");

                    IRestResponse response = client.Execute(request);

                    Console.WriteLine(response.Content);


                    var result = JsonConvert.DeserializeObject<SubResponse>(response.Content);

                    return Ok(result);



                    ////

                    //// Create a request using a URL that can receive a post.
                    //WebRequest request = WebRequest.Create(url);

                    //        // Set the Method property of the request to POST.
                    //        request.Method = "GET";

                    //        request.Headers.Add("Authorization", "Bearer " + token);
                    //        request.Headers.Add("Content-Type", "application/json");

                    //        request.ContentType = "application/json";

                    //    // Get the request stream.
                    //        Stream dataStream = null; //request.GetRequestStream();

                    //        // Get the response.
                    //        WebResponse response = request.GetResponse();
                    //        // Display the status.
                    //        Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                    //        // Get the stream containing content returned by the server.
                    //        // The using block ensures the stream is automatically closed.
                    //        using (dataStream = response.GetResponseStream())
                    //        {
                    //            // Open the stream using a StreamReader for easy access.
                    //            StreamReader reader = new StreamReader(dataStream);
                    //            // Read the content.
                    //            string responseFromServer = reader.ReadToEnd();

                    //            var result = JsonConvert.DeserializeObject<SubResponse>(responseFromServer);

                    //            return Ok(result);

                    //            //return Ok(responseFromServer);
                    //        }
                }
                    catch (Exception ex)
                    {
                        return Ok(ex.Message);
                    }
                //}



            }
            catch (Exception ex)
            {
                //
            }

            return Ok(new { status = "FAILED" });

        }



        [HttpPost]
        public async Task<IActionResult> Login(string userName = "", string msisdn = "")
        {
            //User user = new User()
            //{
            //    UserName = userName,
            //    Msisdn = msisdn
            //};
            userName = userName == null ? "" : userName;

            try
            {
                SessionDatum user = new SessionDatum()
                {
                    UserName = userName,
                    Msisdn = msisdn,
                    ServiceName = "QuizMaster",
                    TimeStamp = DateTime.Now
                };



                var result = await _context.SessionData.Where(x => x.Msisdn == msisdn && x.ServiceName == "QuizMaster").FirstOrDefaultAsync();

                if (result != null)
                {
                    _context.SessionData.Remove(result);
                    await _context.SaveChangesAsync();
                }

                await _context.SessionData.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }



            //HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(user));

            //SessionHelper.SetObjectAsJson(HttpContext.Session, "SessionUser", user);

            return Ok(new { result = "Success" });

        }

        

        [HttpGet]
        public ActionResult<string> BreakTimeQuizCounter()
        {
            try
            {
                string connectionString = "Server=103.134.68.67;Database=WapPortal;User ID=sa;Password=adplayVu@1234;";
                string queryStartTime = "SELECT TOP(1) CAST(StartTime AS TIME) FROM Basket.dbo.tbl_BkashHappyhoursV2 WHERE CAST(date AS DATE) = CAST(GETDATE() AS DATE) ORDER BY date DESC";
                string queryStartTimeNextDay = "SELECT TOP(1) CAST(StartTime AS TIME) FROM Basket.dbo.tbl_BkashHappyhoursV2 WHERE CAST(date AS DATE) = CAST(GETDATE() + 1 AS DATE) ORDER BY date DESC";
                string queryEndTime = "SELECT TOP(1) CAST(EndTime AS TIME) FROM Basket.dbo.tbl_BkashHappyhoursV2 WHERE CAST(date AS DATE) = CAST(GETDATE() AS DATE) ORDER BY date DESC";

                DateTime currentTime = DateTime.Now;
                TimeSpan currentTimeOfDay = currentTime.TimeOfDay;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand commandStartTime = new SqlCommand(queryStartTime, connection);
                    SqlCommand commandStartTimeNextDay = new SqlCommand(queryStartTimeNextDay, connection);
                    SqlCommand commandEndTime = new SqlCommand(queryEndTime, connection);

                    var startTime = (TimeSpan)commandStartTime.ExecuteScalar();
                    var endTime = (TimeSpan)commandEndTime.ExecuteScalar();
                    var startTimeNextDay = (TimeSpan)commandStartTimeNextDay.ExecuteScalar();

                
                    if (currentTimeOfDay <= startTime)
                    {
                        var difference = startTime - currentTimeOfDay;
                        return Ok(new { difference = difference.ToString(), counterText = 0 });
                    }
                    else if (currentTimeOfDay >= startTime && currentTimeOfDay <= endTime)
                    {
                        var difference = endTime - currentTimeOfDay;
                        return Ok(new { difference = difference.ToString(), counterText = 1 });
                    }
                    else if (currentTimeOfDay > endTime)
                    {
                        var difference = startTimeNextDay - currentTimeOfDay + TimeSpan.FromHours(24);
                        return Ok(new { difference = difference.ToString(), counterText = 2 });
                    }

                }

                return Ok(new { difference = "0" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetHappyHoursTimes()
        {
            try
            {
                string connectionString = "server=103.134.68.67;database=WapPortal;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";
                string queryTimes = "SELECT TOP(2) CONVERT(VARCHAR, StartTime, 8) AS Start, CONVERT(VARCHAR, EndTime, 8) AS [End] FROM Basket.dbo.tbl_BkashHappyhoursV2 WHERE CAST([date] AS date) >= CAST(GETDATE() AS date) ORDER BY [date]";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand commandTimes = new SqlCommand(queryTimes, connection);
                    SqlDataReader reader = await commandTimes.ExecuteReaderAsync();

                    List<string> startTimes = new List<string>();
                    List<string> endTimes = new List<string>();

                    while (reader.Read())
                    {
                        startTimes.Add(reader["Start"].ToString());
                        endTimes.Add(reader["End"].ToString());
                    }

                    return Ok(new { StartTimes = startTimes, EndTimes = endTimes });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(new { error = "An error occurred while fetching start and end times." });
            }
        }

        public class HappyHoursTime
        {
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
        }




        [HttpGet]
        public async Task<bool> GetHappyHoursForCurrentDate()
        {
            try
            {
                string connectionString = "server=103.134.68.67;database=WapPortal;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";
                string queryStartTime = "SELECT top(1) CAST(StartTime as time) FROM Basket.dbo.tbl_BkashHappyhoursV2 where CAST(date as date) = CAST(GETDATE() as date) order by date desc";



                string queryEndTime = "SELECT top(1) CAST(EndTime as time) FROM Basket.dbo.tbl_BkashHappyhoursV2 where CAST(date as date) = CAST(GETDATE() as date) order by date desc";



                DateTime currentTime = DateTime.Now;
                TimeSpan currentTimeOfDay = currentTime.TimeOfDay;



                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand commandStartTime = new SqlCommand(queryStartTime, connection);
                    SqlCommand commandEndTime = new SqlCommand(queryEndTime, connection);
                    var startTime = (TimeSpan)commandStartTime.ExecuteScalar();
                    var endTime = (TimeSpan)commandEndTime.ExecuteScalar();


                    if (currentTimeOfDay >= startTime && currentTimeOfDay <= endTime)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    //return startTime.ToString();
                }
            }



            catch (Exception ex)



            {
                return false;
                //return null;
            }



        }

        [HttpPost]
        public async Task<IActionResult> Logout(string userName, string msisdn)
        {

            var result = await _context.SessionData.Where(x => x.Msisdn == msisdn && x.ServiceName == "QuizMaster").FirstOrDefaultAsync();
            if (result != null)
            {
                _context.SessionData.Remove(result);
            }

            return Ok(new { result = "Success" });
        }


        [HttpGet]
        public async Task<IActionResult> CheckWebHookStatus([FromQuery] string subReqId)
        {

            using (var db = new BkashPaymentGateWayContext())
            {
                //var result = await db.WebHookSubscriptionDatas.Where(x => x.SubscriptionRequestId == subReqId && x.Type == "PAYMENT").FirstOrDefaultAsync();
                var result = await db.WebHookSubscriptionDatasV3s.Where(x => x.SubscriptionRequestId == subReqId && x.Type == "PAYMENT").FirstOrDefaultAsync();

                if (result != null)
                {
                    if (result.PaymentStatus.StartsWith("SUCCEEDED"))
                    {
                        return Ok(new { message = "SUCCEEDED" });

                    }
                    else
                    {
                        return Ok(new { message = "FAILED" });
                    }
                }
                return Ok(new { message = "FAILED For Unsuccess" });
            }





        }




        public string LoginToken(/*string msisdn, string isLogin*/)
        {

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            // Create a request using a URL that can receive a post.
            string url = _configuration.GetSection("BkashPayment:LoginUrl").Value;

            WebRequest request = WebRequest.Create(url);
            // Set the Method property of the request to POST.
            request.Method = "POST";

            //----------- For Header --------------
            request.Headers.Add("Cookie",
                ".AspNetCore.Identity.Application=CfDJ8Fo1OSefV4RJh4VxLpaNt3r9DpCKtbUR9woSabHwRwF9yBIGrZJkUBD9C2WJOdHovBEbHj-jg5TSKwS-jnI31ZGtASKhPMkD48keGZ-9kPxc2MIU1bzKMAedZpXdnVTugVWfxo5CRr4m_EYo9U4Kma2uHdYSyxDANd8s1zRaf3UDwqXNLNNhNTZCEgjMuOuBWTyEDzrDukW4GxWsnrxsDKqgMxSTkmG5EfwUHb_RX7mIhnwHHGFE2itqjG9mjkUZ9sOUFzmsPUGrV8P6nH869fyOiFk5S0YVT6l8JUEhn3WzrQZWbguU2AQ4pymxo6sujd12Bm6_-XRfGzjpoel2I5jGDzpJ7Ij_jiXYbzLnPCkq9rw51wA2YpyKllc_hXyW4FCpVeWC3FOyjA3O45R7E_Qnpc6BPQmqD9X21P7H95Iq6VFKveS7ABcDXgmO0rv692qvv6BhmtOxKFGfSJvw8eGJVMft3X9FVqnT5HidYJ6cZtbnUNFng9KRaHpYfIaagKSu1jta6G8a1stLWVqlXravDLSvz2_o7EWo16hX-H0MB1iZlVu6VRgmGO_4Dq5GTk098xD8AgYG17zU6S69PhuMC6vVznGQV1Mo3Vo3g-j4tyPdE2Pah23S2IMyjHy5nq3xBjmErErk5gzKi6TcG-lfkAn0zl6--hpXbM5b5_FOSHogDVcf800OvdrVv2ZSXBoq-o-TYU2pOSmMw6m2PIRYCeP4YhWW6o21aBHaWKTx");


            var getEmailPassword = _context.BkashSecrets.FirstOrDefault();

            //--------- For json type data ---------
            var data = new
            {
                email = getEmailPassword.Email,
                password = getEmailPassword.Password
            };

            //--------- For json type data(Example) ---------
            //var data = new
            //{
            //    app_key= "t4masn6b5n3j",
            //    app_secret= "qd4hqk9g96o9rrrp2jftvek578v"
            //};


            string postData = JsonConvert.SerializeObject(data);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                return responseFromServer;
            }


        }

        public IActionResult ReqData()
        {
            var headerReferer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();

            if (headerReferer == "https://localhost:44326/Home/BkashPayment" || headerReferer == "https://quizmasterappinapp.shabox.mobi/Home/BkashPayment" || headerReferer == "https://gamestar.shabox.mobi/Home/BkashPayment")
            {

                var requestData = new RequestData()
                {
                    amount = "17.0",
                    intent = "sale",
                    currency = "BDT"
                };

                return Ok(requestData);
            }
            return BadRequest();
        }



        public async Task<IActionResult> UpdateCheckoutDataTable([FromQuery] string msisdn, [FromQuery] string userName)
        {

            //var result = await _context.SubscriptionRequestDatas.Where(x => x.SubscriptionReference == msisdn)
            //    .Join(_context.WebHookSubscriptionDatas.Where(y => y.Type == "Payment" && y.PaymentStatus == "SUCCEEDED_PAYMENT" && y.SubscriptionRequestId.StartsWith("QuizMaster")),

            //    subReq => subReq.SubscriptionRequestId,
            //    webHook => webHook.SubscriptionRequestId,
            //    (subReq, webHook) => new
            //    {
            //        msisdn = subReq.SubscriptionReference,
            //        trxId = webHook.TrxId,
            //        amount = webHook.Amount,
            //        createTime = webHook.TrxDate,

            //    }).OrderByDescending(x => x.createTime).Take(5).ToListAsync();
            try
            {
                var result = await _context.SubscriptionRequestDatasV3s.Where(x => x.SubscriptionReference == msisdn)
                .Join(_context.WebHookSubscriptionDatasV3s.Where(y => y.Type == "Payment" && y.PaymentStatus == "SUCCEEDED_PAYMENT" && y.SubscriptionRequestId.StartsWith("QuizMaster")),

                subReq => subReq.SubscriptionRequestId,
                webHook => webHook.SubscriptionRequestId,
                (subReq, webHook) => new
                {
                    msisdn = subReq.SubscriptionReference,
                    trxId = webHook.TrxId,
                    amount = webHook.Amount,
                    createTime = webHook.TrxDate,
                    OrderId = webHook.Id

                }).OrderByDescending(x => x.OrderId).Distinct().ToListAsync();

                return Ok(new { result = result.OrderByDescending(x => x.OrderId).Take(5).ToList() });
            }
            catch (Exception ex)
            {
            }
            return Ok();
        }
    

        public ActionResult GenericModal(string message)
        {
            return View();
        }
        /// <summary>
        /// ///
        /// </summary>
        /// <param name="msisdn"></param>
        /// <param name="ckey"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public async Task<IActionResult> UnsufficientCheckOut([FromQuery] string msisdn, int amount , string ckey = "000000")
        {



            try
            {
                //var getSession = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("SessionUser"));




                if (true)
                {
                    try
                    {
                        var headerReferer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();

                        if (true)
                        {

                            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                            string url = _configuration.GetSection("BkashPayment:createCheckoutUrl").Value;
                            //var token = LoginToken();

                            var client = new RestClient(url);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);
                            //request.AddHeader("Authorization", "Bearer " + token);
                            request.AddHeader("Content-Type", "application/json");

                            var startDate = DateTime.Now.ToString("yyyy-MM-dd");
                            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute >= 30)
                            {
                                startDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                            }
                            var endDate = DateTime.Now.AddDays(700).ToString("yyyy-MM-dd");

                            var subReqId = "QuizMaster-Campaign" + Guid.NewGuid().ToString();


                            var bkashCredentialData = await _context.BkashSecrets.Where(x => x.ServiceName == "QuizMaster").FirstOrDefaultAsync();

                            //--------- For json type data ---------
                            var data = new
                            {
                                amount = amount,
                                //firstPaymentAmount = "null",
                                firstPaymentIncludedInCycle = "True",
                                serviceId = "100001",
                                currency = "BDT",
                                startDate = startDate,
                                expiryDate = endDate,
                                frequency = "WEEKLY",
                                subscriptionType = "BASIC",
                                //maxCapAmount= "null",
                                maxCapRequired = "False",
                                merchantShortCode = "01884104842",
                                //payer = "01770618575",
                                payerType = "CUSTOMER",
                                paymentType = "FIXED",
                                redirectUrl = "https://quizmasterappinapp.shabox.mobi/Home/CampaignSuccess?subReqId=" + subReqId,
                                subscriptionRequestId = subReqId,
                                subscriptionReference = msisdn,
                                ckey = ckey
                                //extraParams= "hello"

                            };

                            string postData = JsonConvert.SerializeObject(data);
                            request.AddParameter("application/json", postData, ParameterType.RequestBody);

                            IRestResponse response = client.Execute(request);

                            // Get the stream containing content returned by the server.
                            // The using block ensures the stream is automatically closed.


                            var result = JsonConvert.DeserializeObject<SubResponse>(response.Content);

                            return Ok(result.RedirectURL);

                        }

                        else
                        {
                            return Unauthorized();
                        }
                    }
                    catch (Exception ex)
                    {
                        return Ok(ex.Message);
                    }
                }



            }
            catch (Exception ex)
            {
                //
            }

            return Unauthorized();

        }
        ///






        [HttpPost]
        public async Task<IActionResult> ForCampaign([FromQuery] string msisdn, string ckey = "000000")
        {

        

            try
            {
                //var getSession = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("SessionUser"));

             


                if (true)
                {
                    try
                    {
                        var headerReferer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();

                        if (true)
                        {

                            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                            string url = _configuration.GetSection("BkashPayment:createCheckoutUrl").Value;
                            //var token = LoginToken();

                            var client = new RestClient(url);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);
                            //request.AddHeader("Authorization", "Bearer " + token);
                            request.AddHeader("Content-Type", "application/json");

                            //// Create a request using a URL that can receive a post.
                            //WebRequest request = WebRequest.Create(url);

                            //// Set the Method property of the request to POST.
                            //request.Method = "POST";

                            //request.Headers.Add("Authorization", "Bearer " + token);
                            //request.Headers.Add("Content-Type", "application/json");

                            var startDate = DateTime.Now.ToString("yyyy-MM-dd");
                            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute >= 30)
                            {
                                startDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                            }
                            var endDate = DateTime.Now.AddDays(700).ToString("yyyy-MM-dd");

                            var subReqId = "QuizMaster-Campaign" + Guid.NewGuid().ToString();


                            var bkashCredentialData = await _context.BkashSecrets.Where(x => x.ServiceName == "QuizMaster").FirstOrDefaultAsync();

                            //--------- For json type data ---------
                            var data = new
                            {
                                amount = "17",
                                //firstPaymentAmount = "null",
                                firstPaymentIncludedInCycle = "True",
                                serviceId = "100001",
                                currency = "BDT",
                                startDate = startDate,
                                expiryDate = endDate,
                                frequency = "WEEKLY",
                                subscriptionType = "BASIC",
                                //maxCapAmount= "null",
                                maxCapRequired = "False",
                                merchantShortCode = "01884104842",
                                //payer = "01770618575",
                                payerType = "CUSTOMER",
                                paymentType = "FIXED",
                                redirectUrl = "https://quizmasterappinapp.shabox.mobi/Home/NewSuccess?subReqId=" + subReqId, //Was CampaignSuccess
                                subscriptionRequestId = subReqId,
                                subscriptionReference = msisdn,
                                ckey = ckey
                                //extraParams= "hello"

                            };

                            string postData = JsonConvert.SerializeObject(data);
                            request.AddParameter("application/json", postData, ParameterType.RequestBody);

                            IRestResponse response = client.Execute(request);

                            // Get the stream containing content returned by the server.
                            // The using block ensures the stream is automatically closed.


                            var result = JsonConvert.DeserializeObject<SubResponse>(response.Content);

                            return Ok(result.RedirectURL);


                            //var result = JsonConvert.DeserializeObject<SubResponse>(responseFromServer);

                            //return Ok(result.RedirectURL);

                            

                            //byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                            //// Set the ContentType property of the WebRequest.
                            //request.ContentType = "application/json";

                            //// Set the ContentLength property of the WebRequest.
                            //request.ContentLength = byteArray.Length;

                            //// Get the request stream.
                            //Stream dataStream = request.GetRequestStream();
                            //// Write the data to the request stream.
                            //dataStream.Write(byteArray, 0, byteArray.Length);
                            //// Close the Stream object.
                            //dataStream.Close();

                            //// Get the response.
                            //WebResponse response = request.GetResponse();
                            //// Display the status.
                            ////Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                            //// Get the stream containing content returned by the server.
                            //// The using block ensures the stream is automatically closed.
                            //using (dataStream = response.GetResponseStream())
                            //{
                            //    // Open the stream using a StreamReader for easy access.
                            //    StreamReader reader = new StreamReader(dataStream);
                            //    // Read the content.
                            //    string responseFromServer = reader.ReadToEnd();

                            //    var result = JsonConvert.DeserializeObject<SubResponse>(responseFromServer);

                            //    return Ok(result.RedirectURL);

                            //    //return Ok(responseFromServer);
                            //}
                        }

                        else
                        {
                            return Unauthorized();
                        }
                    }
                    catch (Exception ex)
                    {
                        return Ok(ex.Message);
                    }
                }



            }
            catch (Exception ex)
            {
                //
            }

            return Unauthorized();

        }


        public ActionResult CampaignPayment()
        {
            return View();
        }

        public ActionResult CampaignSuccess()
        {
            return View();
        }

        public ActionResult CampaignImage()
        {
            return View();
        }
        public ActionResult FBQuiz()
        {
            return View();
        }

        public ActionResult CampaignImage1()
        {
            return View();
        }


        public ActionResult CampaignImage2()
        {
            return View();
        }

        public ActionResult CampaignImage3()
        {
            return View();
        }
        public ActionResult CampaignImage4()
        {
            return View();
        }
        public ActionResult FreeQuiz()
        {
            return View();
        }
        public ActionResult NewSuccess()
        {
            return View();
        }

        [HttpGet]
        public async Task<string> UpdateWordPuzzleGameLeaderboard(string msisdn, int? score, string gameName, string timeCount)
        {
            try
            {
                await _basketContext.TblQuizMasterWordPuzzleGames.AddAsync(new TblQuizMasterWordPuzzleGame
                {
                    Msisdn = msisdn,
                    GameName = gameName,
                    Score = score,
                    TimeStamp = DateTime.Now,
                    TimeCount = decimal.Parse(timeCount)
                });
                await _basketContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
            return "Success";
        }

        [HttpGet]
        public async Task<string> UpdateWordPuzzleGameNotPlayedFulltime(string msisdn, int? score, string gameName, string timeCount)
        {
            try
            {
                await _basketContext.TblQuizMasterWordPuzzleGamePlayerNotPlayed.AddAsync(new TblQuizMasterWordPuzzleGamePlayerNotPlayed
                {
                    Msisdn = msisdn,
                    GameName = gameName,
                    Score = score,
                    TimeStamp = DateTime.Now,
                    TimeCount = decimal.Parse(timeCount)
                });
                await _basketContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
            return "Success";
        }


    }
}
