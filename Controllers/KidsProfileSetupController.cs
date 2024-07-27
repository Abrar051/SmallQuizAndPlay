using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using QuizMaster.Models;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace QuizMaster.Controllers
{
    public class KidsProfileSetupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewOrEdit()
        {
            return View();
        }

        public IActionResult Profile ()
        {
            return View();
        }

        public IActionResult EditProfile()
        {
            return View();
        }

        public IActionResult KidsHome()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Trivia()
        {
            return View();
        }

        public IActionResult Poem ()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostUserDetails([FromBody] StudentProfile profile)
        {
            bool existence = GetUserprofileExistence(profile.msisdn);
            if (existence == false) // Insert
            {
                try
                {
                    SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=KidStar;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                    string insertQuery = "Insert into [KidsProfile] values ('" + profile.fullName + "','" + profile.parentName + "','" + profile.city + "','" + profile.msisdn + "' , '" + profile.birthDate + "','" + profile.imageLocation + "')";
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.CommandTimeout = 600;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return Ok("Success");
                }
                catch (Exception ex)
                {
                    return Ok("Bad Request");
                }
            }
            else
            {
                try {
                    SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=KidStar;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                    string updateQuery = "update KidsProfile set FullName = '"+ profile.fullName +"' , ParentName = '"+ profile.parentName +"' , City = '"+profile.city+"'  , Age = '"+profile.birthDate+"' , ImageLocation = '"+profile.imageLocation+"' where MSISDN = '"+profile.msisdn +"'";
                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.CommandTimeout = 600;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return Ok("Success");
                }
                catch (Exception ex)
                {
                    return Ok("Bad Request");
                }
            }
            
        }

        [HttpGet]
        public async Task<JsonResult> GetProfileExistence(string MSISDN)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=KidStar;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                string query = "Select COUNT(*) from KidsProfile where MSISDN = '" + MSISDN + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 600;
                con.Open();
                var reader = cmd.ExecuteScalar();
                con.Close();
                if (reader.ToString() == "1")
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


        public bool GetUserprofileExistence (string MSISDN)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=KidStar;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                string query = "Select COUNT(*) from KidsProfile where MSISDN = '" + MSISDN +"'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 600;
                con.Open();
                var reader = cmd.ExecuteScalar();
                con.Close();
                if (reader.ToString()=="1")
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
                return false;
            }
            
        }


        [HttpGet]
        public async Task<bool> GetProfileExistenceBool (string MSISDN)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=KidStar;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                string query = "Select COUNT(*) from KidsProfile where MSISDN = '" + MSISDN + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 600;
                con.Open();
                var reader = cmd.ExecuteScalar();
                con.Close();
                if (reader.ToString() == "1")
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
                return false;
            }
        }


        [HttpGet]
        public async Task<int> GetUserAge (string MSISDN)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=KidStar;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                string insertQuery = "select [Age] from [KidStar].[dbo].[KidsProfile] WHERE MSISDN =" + MSISDN ;
                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.CommandTimeout = 600;
                con.Open();
                var reader = cmd.ExecuteScalar();
                con.Close();
                return int.Parse(reader.ToString()) ;
            }
            catch (Exception ex)
            {
                return 9;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProfile (string MSISDN)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=KidStar;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                con.Open();
                string commandQuery = "SELECT [FullName], [ParentName], [City], [MSISDN], [Age] , [ImageLocation] FROM [KidStar].[dbo].[KidsProfile] " + "WHERE MSISDN = '" + MSISDN +"'" + " FOR JSON PATH";
                SqlCommand cmd = new SqlCommand(commandQuery, con);
                var reader = cmd.ExecuteScalar();
                con.Close();
                string jsonString = reader.ToString();
                return Content(jsonString, "application/json");
            }
            catch (Exception ex)
            {
                return Content("", "application/json"); ;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTranslation()
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=KidStar;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                con.Open();
                string commandQuery = "WITH RankedData AS (SELECT *,ROW_NUMBER() OVER (ORDER BY NEWID()) AS SequentialId FROM [KidStar].[dbo].[SentenceLearn] ) SELECT top(10) SequentialId AS Id, English, Bangla FROM RankedData FOR JSON PATH";
                SqlCommand cmd = new SqlCommand(commandQuery, con);
                var reader = cmd.ExecuteScalar();
                con.Close();
                string jsonString = reader.ToString();
                return Content(jsonString, "application/json");
            }
            catch (Exception ex)
            {
                return Content("", "application/json"); ;
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetPoem()
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=KidStar;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                con.Open();
                string commandQuery = "select Poem from KidStar.dbo.KidsPoem order by NEWID() asc";
                SqlCommand cmd = new SqlCommand(commandQuery, con);
                var reader = cmd.ExecuteScalar();
                con.Close();
                string jsonString = reader.ToString();
                return Ok(jsonString);
            }
            catch (Exception ex)
            {
                return Ok("ERROR");
            }
        }


        public IActionResult LearnTranslation ()
        {
            return View();
        }

        public async Task <IActionResult> GetTranslationPair ()
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Server=103.134.68.67;Database=KidStar;User ID=sa;Password=adplayVu@1234;MultipleActiveResultSets=true");
                con.Open();
                string searchQuery = "SELECT TOP(1) English , Bangla  FROM KidStar.dbo.SentenceLearn  ORDER BY NEWID() FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;";
                SqlCommand cmd = new SqlCommand(searchQuery , con);
                var reader = cmd.ExecuteScalar();
                con.Close();
                string jsonString = reader.ToString();
                return Content(jsonString, "application/json");
            }
            catch (Exception ex)
            {
                return Ok("");
            }
            return Ok("");
        }



    }
}
