using QuizMaster.Models;
using QuizMaster.Models.BkashPaymentGateWay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using QuizMaster.Models.WapPortal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace QuizMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BkashAppNAppController : Controller
    {
        private readonly ILogger<BkashAppNAppController> _logger;
        private readonly IConfiguration _configuration;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BkashPaymentGateWayContext _context;
        private readonly WapPortalContext _wapPortalContext;
        private readonly BasketContext _basketContext;

        public BkashAppNAppController(ILogger<BkashAppNAppController> logger, IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, BkashPaymentGateWayContext context, WapPortalContext wapPortalContext)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _context = context;
            _wapPortalContext = wapPortalContext;
        }


        [HttpPost("auth")]
        public async Task<IActionResult> Auth([FromBody] SignInModel signinModel)
        {
            // For Validating the UserName and Pass

            var IsValid = await _context.BkashSecrets.Where(x => x.ServiceName == "QuizMasterAppInApp" && x.AppNappUserName == signinModel.username && x.AppNappPassWord == signinModel.password).FirstOrDefaultAsync();
            var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name , signinModel.mobile_number),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(4),
                claims: authClaims,
                signingCredentials: new SigningCredentials
                (authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            //
            var FinalUser = new TblQuizMasterUserInfo();
            if (IsValid != null)
            {

                int countUser = countExistence(signinModel.mobile_number);
                if (countUser >= 1)
                {
                    updateToken(signinModel.mobile_number, jwtToken);
                }
                else
                {
                    if (signinModel.mobile_number.StartsWith("88"))
                    {
                        signinModel.mobile_number = signinModel.mobile_number;
                    }
                    else
                    {
                        signinModel.mobile_number = "88" + signinModel.mobile_number;
                    }
                    try
                    {
                        string connectionString = "server=103.134.68.67;database=Basket;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";
                        string Name = "User_" + signinModel.mobile_number;
                        string SourceUrl = _httpContextAccessor.HttpContext.Request.Host + "/" + _httpContextAccessor.HttpContext.Request.QueryString;
                        string insertQuery = "INSERT INTO Tbl_QuizMaster_UserInfo (MSISDN, Name, SourceUrl, CKey, TimeStamp, AppInAppToken, AppInAppTokenTimeStamp, imageName, PushNotificationToken) " +
                             "VALUES (@MSISDN, @Name, @SourceUrl, @CKey, @TimeStamp, @AppInAppToken, @AppInAppTokenTimeStamp, @ImageName, @PushNotificationToken)";
                        FinalUser.Msisdn = signinModel.mobile_number;
                        FinalUser.Name = Name;
                        FinalUser.SourceUrl = SourceUrl;
                        FinalUser.TimeStamp = DateTime.Now;
                        FinalUser.AppInAppToken = jwtToken;
                        FinalUser.AppInAppTokenTimeStamp = DateTime.Now;

                        SqlConnection connection = new SqlConnection(connectionString);
                        connection.Open();
                        SqlCommand command = new SqlCommand(insertQuery, connection);
                        command.Parameters.AddWithValue("@MSISDN", FinalUser.Msisdn);
                        command.Parameters.AddWithValue("@Name", FinalUser.Name);
                        command.Parameters.AddWithValue("@SourceUrl", FinalUser.SourceUrl);
                        command.Parameters.AddWithValue("@CKey", ' ');
                        command.Parameters.AddWithValue("TimeStamp", FinalUser.TimeStamp);
                        command.Parameters.AddWithValue("@AppInAppToken", FinalUser.AppInAppToken);
                        command.Parameters.AddWithValue("@AppInAppTokenTimeStamp", FinalUser.AppInAppTokenTimeStamp);
                        command.Parameters.AddWithValue("@imageName", ' ');
                        command.Parameters.AddWithValue("@PushNotificationToken", ' ');
                        command.ExecuteNonQuery();
                        connection.Close();

                    }
                    catch (Exception ex2)
                    {

                    }
                }

                //_httpContextAccessor.HttpContext.Session.SetString("Token", jwtToken);

                //var result = "";
                //if (string.IsNullOrEmpty(result))
                //{
                //    return Unauthorized();
                //}

                return Ok(new { token = jwtToken });
            }

            return Ok(new { response = "false" });
        }

        [HttpGet("loginToken/{token}")]
        public async Task<IActionResult> LoginToken(string token)
        {
            //string sessionToken = _httpContextAccessor.HttpContext.Session.GetString("Token");

            string connectionString = "server=103.134.68.67;database=Basket;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenParse = tokenHandler.ReadJwtToken(token);
            var mobileNumberClaim = tokenParse.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            string MSISDN = mobileNumberClaim.Value;
            /// Decrypting JWT to get mobile number from claim
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["JWT:ValidAudience"];
            validationParameters.ValidIssuer = _configuration["JWT:ValidIssuer"];
            validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out validatedToken);

            //var mobile = principal.FindFirst("Name").Value;
            var mobile = principal.Identity.Name == null ? "" : principal.Identity.Name;

            /////
            ///Update the existing token

            //var ExistingUser = await _basketContext.TblQuizMasterUserInfos.Where(x => (x.Msisdn == mobile || "88" + x.Msisdn == mobile || "880" + x.Msisdn == mobile || "88" + x.Msisdn == "88" + mobile || "880" + x.Msisdn == "880" + mobile || x.Msisdn == "88" + mobile)).FirstOrDefaultAsync();
            //ExistingUser.AppInAppToken = null;
            //ExistingUser.AppInAppTokenTimeStamp = null;
            //await _wapPortalContext.SaveChangesAsync();

            //if (ExistingUser != null)
            //{
            //    try
            //    {
            //        SessionDatum user = new SessionDatum()
            //        {
            //            UserName = ExistingUser.Name,
            //            Msisdn = ExistingUser.Msisdn,
            //            ServiceName = "QuizMasterAppInApp",
            //            TimeStamp = DateTime.Now
            //        };

            //        var result = await _context.SessionData.Where(x => x.Msisdn == ExistingUser.Msisdn && x.ServiceName == "QuizMasterAppInApp").FirstOrDefaultAsync();

            //        if (result != null)
            //        {
            //            _context.SessionData.Remove(result);
            //            await _context.SaveChangesAsync();
            //        }

            //        await _context.SessionData.AddAsync(user);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (Exception ex)
            //    {

            //    }

            //}

            //var Url = "https://localhost:5011/?" + "ckey=009999&mbl=" + ExistingUser.Msisdn;
            var Url = "https://" + _httpContextAccessor.HttpContext.Request.Host + "?" + "ckey=" + _configuration.GetSection("BkashCkey:Ckey").Value + "&mbl=" + "88" +mobileNumberClaim.Value;//+ ExistingUser.Msisdn;
            return Redirect(Url);

            //return RedirectToAction("Index", "LandingPage");
            //return Ok(new { response = "success" });




            //return Ok(new { response = "Unsuccessful" });
        }


        public int countExistence (string MSISDN)
        {
            string connectionString = "server=103.134.68.67;database=Basket;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";
            string searchQuery = "SELECT COUNT(*) FROM Basket.dbo.Tbl_QuizMaster_UserInfo where MSISDN=@MSISDN";
            if (!MSISDN.StartsWith("88"))
            {
                MSISDN = "88" + MSISDN;
            }
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(searchQuery,connection);
            command.Parameters.AddWithValue("@MSISDN", MSISDN);
            connection.Open();
            int count = (int)command.ExecuteScalar();
            connection.Close();
            return count;
        }

        public void updateToken (string MSISDN , string Token)
        {
            string connectionString = "server=103.134.68.67;database=Basket;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";
            string updateQuery = "UPDATE Basket.dbo.Tbl_QuizMaster_UserInfo set AppInAppToken = @AppInAppToken WHERE MSISDN = @MSISDN ";
            if (!MSISDN.StartsWith("88"))
            {
                MSISDN = "88" + MSISDN;
            }
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(updateQuery, connection);
            command.Parameters.AddWithValue("@MSISDN", MSISDN);
            command.Parameters.AddWithValue("@AppInAppToken", Token);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        

    }
}
