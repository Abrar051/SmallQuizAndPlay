using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuizMaster.Data.QuizMasterLiveQuizLog;
using QuizMaster.Models.WapPortal;
using QuizMaster.Models;
using QuizStarPortal.Models;

namespace QuizMaster.Controllers
{
    public class FreeQuizController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly WapPortalContext _wapPortalContext;
        private readonly BasketContext _entities = new BasketContext();
        private readonly QuizMasterLiveQuizLogContext _QuizMasterLiveQuizLogContext = new QuizMasterLiveQuizLogContext();
        private readonly IHttpContextAccessor _contextHttp;
        string UAPROF_URL = string.Empty;
        private IConfiguration _Configuration;
        // GET: FreeQuizController
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult FreeQuiz ()
        {
            return View();
        }


        // GET: FreeQuizController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FreeQuizController/Create
        public ActionResult Create()
        {
            return View();
        }

        //Inserting data 
        




    // POST: FreeQuizController/Create
    [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FreeQuizController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FreeQuizController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FreeQuizController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> InsertFreeQuizData(string name, string msisdn, string ckey)
        {
            string connectionString = "server=103.134.68.67;database=Basket;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";
            UAPROF_URL = GetUserAgent();
            DD a = new Hsprofiling().GetResponse(UAPROF_URL);
            //_contextHttp.HttpContext.Session.SetString("rawurl", srcurl);
            var queryString = _contextHttp.HttpContext.Request.QueryString.Value.ToString();
            if (string.IsNullOrWhiteSpace(msisdn))
            {
                msisdn = "";
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
                }
            }
            return RedirectToAction("freequiz");
        }

        // POST: FreeQuizController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
        public string GetUserIP()
        {
            // var remoteIpAddress = _contextHttp.HttpContext.Connection.RemoteIpAddress;
            string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                remoteIpAddress = Request.Headers["X-Forwarded-For"];
            return remoteIpAddress.ToString();
        }
    }
}
