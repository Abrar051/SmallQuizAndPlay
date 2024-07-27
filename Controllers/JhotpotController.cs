using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QuizMaster.Models;
using QuizMaster.Models.WapPortal;
using static System.Net.Mime.MediaTypeNames;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuizMaster.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JhotpotController : ControllerBase
    {
        private readonly BasketContext _entity = new BasketContext();
        private readonly WapPortalContext _wapPortalContext = new WapPortalContext();
        private readonly IHttpContextAccessor _contextHttp;
        string UAPROF_URL = string.Empty;
        private IConfiguration _Configuration;
        public JhotpotController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _contextHttp = httpContextAccessor;
            _Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetJhotpotThemeQuestionsNew(string fbid, string test = null, string quizCats = null)
        {
            if (quizCats == null)
            {
                if (test == null)
                {
                    var today = DateTime.Now.Date;

                    //var count = await _wapPortalContext.TblJhotpotPlayCounts.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date).FirstOrDefaultAsync();
                    var count = await _wapPortalContext.TblJhotpotPlayCounts.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date).Where(i => i.ServiceName == "QuizMaster").FirstOrDefaultAsync();

                    //count.Count = 0; // Change must
                    if (count == null)
                    {
                        await _wapPortalContext.TblJhotpotPlayCounts.AddAsync(new TblJhotpotPlayCount()
                        {
                            FbId = fbid,
                            Count = 1,
                            TimeStamp = DateTime.Now,
                            ServiceName = "QuizMaster"
                        });

                        await _wapPortalContext.SaveChangesAsync();

                        var qcount = await _wapPortalContext.TblQuizStarQuestionTimers.Where(x => x.TypeName.ToLower() == "jhotpot").Select(x => x.QuestionCount).FirstOrDefaultAsync();
                        var Qcount = Convert.ToInt32(qcount);

                        var listOfImageQuestions = await _wapPortalContext.JhotPotThemeQuestions
                           .Where(x =>x.QuestionCategory != "Spelling" &&
                                    x.QuestionCategory != "বিনোদন জগত")
                            .Select(x => new Qlis
                        {
                            Id = x.Id.ToString(),
                            QuestionImage = x.QuestionImage,
                            Question = x.Question,
                            Option1 = x.Option1,
                            Option2 = x.Option2,
                            Option3 = x.Option3,
                            Answer = x.Answer
                        }).Where(x => x.QuestionImage != null).Distinct().OrderBy(x => Guid.NewGuid()).Take(5).ToListAsync();

                        var listOfQuestion = await _wapPortalContext.JhotPotThemeQuestions
                            .Where(x => x.QuestionCategory != "Spelling" &&
                                    x.QuestionCategory != "বিনোদন জগত")
                            .Select(x => new Qlis
                        {
                            Id = x.Id.ToString(),
                       
                            Question = x.Question,
                            Option1 = x.Option1,
                            Option2 = x.Option2,
                            Option3 = x.Option3,
                            Answer = x.Answer
                        }).Distinct().OrderBy(x => Guid.NewGuid()).Take(Qcount - listOfImageQuestions.Count).ToListAsync();

                        var concatedQuestionList = listOfQuestion.Concat(listOfImageQuestions).ToList();
                        var finalQuestionList = concatedQuestionList.OrderBy(x => Guid.NewGuid()).ToList();

                        JhotPotModel zz = new JhotPotModel();
                        List<Qlis> yy = new List<Qlis>();
                        zz.IsPlayed = "0";
                        zz.Qlist = finalQuestionList;
                        return Ok(new { result = zz });
                    }
                    else if (count.Count < 2)
                    {
                        count.Count += 1;
                        //_wapPortalContext.Entry(count).State = EntityState.Modified;
                        await _wapPortalContext.SaveChangesAsync();

                        var qcount = await _wapPortalContext.TblQuizStarQuestionTimers.Where(x => x.TypeName.ToLower() == "jhotpot").Select(x => x.QuestionCount).FirstOrDefaultAsync();
                        var Qcount = Convert.ToInt32(qcount);

                        var listOfImageQuestions = await _wapPortalContext.JhotPotThemeQuestions
                             .Where(x => x.QuestionCategory != "Spelling" &&
                                    x.QuestionCategory != "বিনোদন জগত")
                            .Select(x => new Qlis
                        {
                            Id = x.Id.ToString(),
                            QuestionImage = x.QuestionImage,
                            Question = x.Question,
                            Option1 = x.Option1,
                            Option2 = x.Option2,
                            Option3 = x.Option3,
                            Answer = x.Answer
                        }).Where(x => x.QuestionImage != null).Distinct().OrderBy(x => Guid.NewGuid()).Take(5).ToListAsync();

                        var listOfQuestion = await _wapPortalContext.JhotPotThemeQuestions
                             .Where(x => x.QuestionCategory != "Spelling" &&
                                    x.QuestionCategory != "বিনোদন জগত")
                            .Select(x => new Qlis
                        {
                            Id = x.Id.ToString(),
                            QuestionImage = x.QuestionImage,
                            Question = x.Question,
                            Option1 = x.Option1,
                            Option2 = x.Option2,
                            Option3 = x.Option3,
                            Answer = x.Answer
                        }).Distinct().OrderBy(x => Guid.NewGuid()).Take(Qcount - listOfImageQuestions.Count).ToListAsync();

                        var concatedQuestionList = listOfQuestion.Concat(listOfImageQuestions).ToList();
                        var finalQuestionList = concatedQuestionList.OrderBy(x => Guid.NewGuid()).ToList();

                        JhotPotModel zz = new JhotPotModel();
                        List<Qlis> yy = new List<Qlis>();
                        zz.IsPlayed = "0";
                        zz.Qlist = finalQuestionList;
                        return Ok(new { result = zz });
                    }

                    List<Qlis> ll = new List<Qlis>();
                    JhotPotModel pp = new JhotPotModel();
                    pp.IsPlayed = "1";
                    pp.Qlist = ll;
                    return Ok(new { result = pp });
                }
                else
                {
                    //tbl_JhotpotPlayCount pot = new tbl_JhotpotPlayCount { FbId = fbid, Count = 1, TimeStamp = DateTime.Now };
                    //_context.tbl_JhotpotPlayCount.Add(pot);
                    //await _context.SaveChangesAsync();

                    var qcount = await _wapPortalContext.TblQuizStarQuestionTimers.Where(x => x.TypeName.ToLower() == "jhotpot").Select(x => x.QuestionCount).FirstOrDefaultAsync();
                    var Qcount = Convert.ToInt32(qcount);

                    var listOfImageQuestions = await _wapPortalContext.JhotPotThemeQuestions
                         .Where(x => x.QuestionCategory != "Spelling" &&
                                    x.QuestionCategory != "বিনোদন জগত")
                        .Select(x => new Qlis
                    {
                        Id = x.Id.ToString(),
                        QuestionImage = x.QuestionImage,
                        Question = x.Question,
                        Option1 = x.Option1,
                        Option2 = x.Option2,
                        Option3 = x.Option3,
                        Answer = x.Answer
                    }).Where(x => x.QuestionImage != null).Distinct().OrderBy(x => Guid.NewGuid()).Take(5).ToListAsync();

                    var listOfQuestion = await _wapPortalContext.JhotPotThemeQuestions
                         .Where(x => x.QuestionCategory != "Spelling" &&
                                    x.QuestionCategory != "বিনোদন জগত")
                        .Select(x => new Qlis
                    {
                        Id = x.Id.ToString(),
                        QuestionImage = x.QuestionImage,
                        Question = x.Question,
                        Option1 = x.Option1,
                        Option2 = x.Option2,
                        Option3 = x.Option3,
                        Answer = x.Answer
                    }).Distinct().OrderBy(x => Guid.NewGuid()).Take(Qcount - listOfImageQuestions.Count).ToListAsync();

                    var concatedQuestionList = listOfQuestion.Concat(listOfImageQuestions).ToList();
                    var finalQuestionList = concatedQuestionList.OrderBy(x => Guid.NewGuid()).ToList();

                    JhotPotModel zz = new JhotPotModel();
                    List<Qlis> yy = new List<Qlis>();
                    zz.IsPlayed = "0";
                    zz.Qlist = finalQuestionList;
                    return Ok(new { result = zz });
                }

            }
            return Ok(new { result = true });

        }


        [HttpGet]
        public async Task<IActionResult> GetJhotpotFreeQuestionsNew()
        {
            //var qcount = await _wapPortalContext.TblQuizStarQuestionTimers.Where(x => x.TypeName.ToLower() == "jhotpot").Select(x => x.QuestionCount).FirstOrDefaultAsync();
            var qcount = 15;
            var Qcount = Convert.ToInt32(qcount);
            var listOfImageQuestions = await _wapPortalContext.JhotPotThemeQuestions.Select(x => new Qlis
            {
                Id = x.Id.ToString(),
                QuestionImage = x.QuestionImage,
                Question = x.Question,
                Option1 = x.Option1,
                Option2 = x.Option2,
                Option3 = x.Option3,
                Answer = x.Answer
            }).Where(x => x.QuestionImage != null).Distinct().OrderBy(x => Guid.NewGuid()).Take(5).ToListAsync();

            var listOfQuestion = await _wapPortalContext.JhotPotThemeQuestions.Select(x => new Qlis
            {
                Id = x.Id.ToString(),
                QuestionImage = x.QuestionImage,
                Question = x.Question,
                Option1 = x.Option1,
                Option2 = x.Option2,
                Option3 = x.Option3,
                Answer = x.Answer
            }).Distinct().OrderBy(x => Guid.NewGuid()).Take(Qcount - listOfImageQuestions.Count).ToListAsync();

            var concatedQuestionList = listOfQuestion.Concat(listOfImageQuestions).ToList();
            var finalQuestionList = concatedQuestionList.OrderBy(x => Guid.NewGuid()).ToList();

            JhotPotModel zz = new JhotPotModel();
            List<Qlis> yy = new List<Qlis>();
            zz.IsPlayed = "0";
            zz.Qlist = finalQuestionList;
            return Ok(new { result = zz });

        }

        ////////// 
        ///2023-09-13 <summary>
        /// Cricket related Question Api 
        /// 

        [HttpGet]
        public async Task<IActionResult> GetJhotpotQuestionsSpellingAndCine(string fbid, string serviceName, string test = null, string quizCats = null )
        {
            if (quizCats == null)
            {
                if (test == null)
                {
                    string category = "";
                    if (serviceName == "KidStar-Math")
                    {
                        category = "'MathOLogic1'";
                    }
                    if (serviceName == "KidStar-GuessWord")
                    {
                        category = "'GuessTheWord1'";
                    }

                    var today = DateTime.Now.Date;

                    var count = await _wapPortalContext.TblJhotpotPlayCounts
                        .Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date)
                        .Where(i => i.ServiceName == serviceName)
                        .FirstOrDefaultAsync();

                    if (count == null)
                    {
                        await _wapPortalContext.TblJhotpotPlayCounts.AddAsync(new TblJhotpotPlayCount()
                        {
                            FbId = fbid,
                            Count = 0,   ///Change in here before uploading
                            TimeStamp = DateTime.Now,
                            ServiceName = serviceName
                        });

                        await _wapPortalContext.SaveChangesAsync();

                        var qcount = await _wapPortalContext.TblQuizStarQuestionTimers
                            .Where(x => x.TypeName.ToLower() == "jhotpot")
                            .Select(x => x.QuestionCount)
                            .FirstOrDefaultAsync();

                        var Qcount = Convert.ToInt32(qcount);
                        var targetCategory = category;

                        string connectionString = "server=103.134.68.67;database=WapPortal;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";

                        string Query = "SELECT TOP 15 * FROM WapPortal.dbo.JhotPot_ThemeQuestionForGuessTheWordKids WHERE QuestionCategory = " + targetCategory + "   ORDER BY NEWID()";

                        List<Qlis> listOfQuestion = new List<Qlis>();

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            await connection.OpenAsync();

                            using (SqlCommand command = new SqlCommand(Query, connection))
                            {
                                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        Qlis qlis = new Qlis
                                        {
                                            Id = reader["Id"].ToString(),
                                            QuestionImage = reader["QuestionImage"].ToString(),
                                            Question = reader["Question"].ToString(),
                                            Option1 = reader["Option1"].ToString(),
                                            Option2 = reader["Option2"].ToString(),
                                            Option3 = reader["Option3"].ToString(),
                                            Answer = reader["Answer"].ToString(),
                                            Theme = reader["Theme"].ToString(),
                                            QuestionCategory = serviceName // Set the category here
                                        };

                                        listOfQuestion.Add(qlis);
                                    }
                                }
                            }
                        }

                        var rnd = new Random();
                        var finalQuestionList = listOfQuestion.OrderBy(x => rnd.Next()).ToList();

                        JhotPotModel zz = new JhotPotModel();
                        zz.IsPlayed = "0";
                        zz.Qlist = finalQuestionList;
                        return Ok(new { result = zz });
                    }
                    else if (count.Count < 2)
                    {
                        count.Count = 1;
                        await _wapPortalContext.SaveChangesAsync();

                        var qcount = await _wapPortalContext.TblQuizStarQuestionTimers
                            .Where(x => x.TypeName.ToLower() == "jhotpot")
                            .Select(x => x.QuestionCount)
                            .FirstOrDefaultAsync();

                        var Qcount = Convert.ToInt32(qcount);
                        var targetCategory = category;

                        string connectionString = "server=103.134.68.67;database=WapPortal;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";

                        string Query = "SELECT TOP 15 * FROM WapPortal.dbo.JhotPot_ThemeQuestionForGuessTheWordKids WHERE QuestionCategory = " + targetCategory + " ORDER BY NEWID()";

                        List<Qlis> listOfQuestion = new List<Qlis>();

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            await connection.OpenAsync();

                            using (SqlCommand command = new SqlCommand(Query, connection))
                            {
                                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        Qlis qlis = new Qlis
                                        {
                                            Id = reader["Id"].ToString(),
                                            QuestionImage = reader["QuestionImage"].ToString(),
                                            Question = reader["Question"].ToString(),
                                            Option1 = reader["Option1"].ToString(),
                                            Option2 = reader["Option2"].ToString(),
                                            Option3 = reader["Option3"].ToString(),
                                            Answer = reader["Answer"].ToString(),
                                            Theme = reader["Theme"].ToString(),
                                            QuestionCategory = serviceName // Set the category here
                                        };

                                        listOfQuestion.Add(qlis);
                                    }
                                }
                            }
                        }

                        var rnd = new Random();
                        var finalQuestionList = listOfQuestion.OrderBy(x => rnd.Next()).ToList();

                        JhotPotModel zz = new JhotPotModel();
                        zz.IsPlayed = "0";
                        zz.Qlist = finalQuestionList;
                        return Ok(new { result = zz });
                    }

                    List<Qlis> ll = new List<Qlis>();
                    JhotPotModel pp = new JhotPotModel();
                    pp.IsPlayed = "1";
                    pp.Qlist = ll;
                    return Ok(new { result = pp });
                }
                else
                {
                    string category = "";
                    if (serviceName == "KidStar-Math")
                    {
                        category = "'MathOLogic1'";
                    }
                    if (serviceName == "KidStar-GuessWord")
                    {
                        category = "'GuessTheWord1'";
                    }
                    var qcount = await _wapPortalContext.TblQuizStarQuestionTimers
                        .Where(x => x.TypeName.ToLower() == "jhotpot")
                        .Select(x => x.QuestionCount)
                        .FirstOrDefaultAsync();

                    var Qcount = Convert.ToInt32(qcount);
                    var targetCategory = category;

                    string connectionString = "server=103.134.68.67;database=WapPortal;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";

                    string Query = "SELECT TOP 15 * FROM WapPortal.dbo.JhotPot_ThemeQuestionForGuessTheWordKids WHERE QuestionCategory = " + targetCategory + " ORDER BY NEWID()";

                    List<Qlis> listOfQuestion = new List<Qlis>();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        using (SqlCommand command = new SqlCommand(Query, connection))
                        {
                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    Qlis qlis = new Qlis
                                    {
                                        Id = reader["Id"].ToString(),
                                        QuestionImage = reader["QuestionImage"].ToString(),
                                        Question = reader["Question"].ToString(),
                                        Option1 = reader["Option1"].ToString(),
                                        Option2 = reader["Option2"].ToString(),
                                        Option3 = reader["Option3"].ToString(),
                                        Answer = reader["Answer"].ToString(),
                                        Theme = reader["Theme"].ToString(),
                                        QuestionCategory = serviceName // Set the category here
                                    };

                                    listOfQuestion.Add(qlis);
                                }
                            }
                        }
                    }

                    var rnd = new Random();
                    var finalQuestionList = listOfQuestion.OrderBy(x => rnd.Next()).ToList();

                    JhotPotModel zz = new JhotPotModel();
                    zz.IsPlayed = "0";
                    zz.Qlist = finalQuestionList;
                    return Ok(new { result = zz });
                }
            }
            return Ok(new { result = true });
        }

     


        [HttpGet]
        public async Task<IActionResult> GetHappyHoursForCurrentDateAsync([FromQuery] string currentTime)
        {
            try
            {
                string connectionString = "server=103.134.68.67;database=WapPortal;Trusted_Connection=false;user=sa;pwd=adplayVu@1234;";

                string query = "SELECT StartTime, EndTime FROM Basket.dbo.tbl_BkashHappyhoursV2 WHERE CONVERT(DATE, GETDATE()) = CONVERT(DATE, date);";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var startTimeStr = reader["StartTime"].ToString();
                                var endTimeStr = reader["EndTime"].ToString();

                                var startTime = DateTime.ParseExact(startTimeStr, "HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                                var endTime = DateTime.ParseExact(endTimeStr, "HH:mm:ss.fffffff", CultureInfo.InvariantCulture);

                                // Convert currentTime from ISO string to DateTime
                                var currentTimeDt = DateTime.Parse(currentTime, null, DateTimeStyles.RoundtripKind);

                                // Check if the current time is between start and end times
                                if (currentTimeDt >= startTime && currentTimeDt <= endTime)
                                {
                                    return Ok(1);
                                }
                                else
                                {
                                    return Ok(0);
                                }
                            }
                            else
                            {
                                return NotFound("No happy hours found for the current date.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }






        [HttpGet]
        public async Task<IActionResult> GetJhotpotThemeQuestionsForSpecialQuizes(string fbid, string test = null, string ServiceType = "WorldCupQuiz")
        {
            try
            {
                if (test == null)
                {
                    var today = DateTime.Now.Date;

                    var count = await _wapPortalContext.TblJhotpotPlayCountForSpecialQuizzes.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date && i.ServiceType == ServiceType).FirstOrDefaultAsync();
                    if (count == null)
                    {
                        await _wapPortalContext.TblJhotpotPlayCountForSpecialQuizzes.AddAsync(new TblJhotpotPlayCountForSpecialQuiz()
                        {
                            FbId = fbid,
                            Count = 1,
                            TimeStamp = DateTime.Now,
                            ServiceName = "QuizMaster",
                            ServiceType = ServiceType
                        });

                        await _wapPortalContext.SaveChangesAsync();


                        //var qcount = await _wapPortalContext.TblQuizStarQuestionTimers.Where(x => x.TypeName.ToLower() == "jhotpot").Select(x => x.QuestionCount).FirstOrDefaultAsync();
                        var qcount = ServiceType switch
                        {
                            "FunFriday" => 10,
                            "WorldCupQuiz" => 60,
                            "EkusheyQuiz" => 21,
                            _ => 60
                        };
                        //var qcount = 60; // Added statically for funfriday
                        var Qcount = Convert.ToInt32(qcount);


                        if (ServiceType == "FunFriday")
                        {
                            var listOfImageQuestions = await _wapPortalContext.JhotPotThemeQuestions.Select(x => new Qlis
                            {
                                Id = x.Id.ToString(),
                                QuestionImage = x.QuestionImage,
                                Question = x.Question,
                                Option1 = x.Option1,
                                Option2 = x.Option2,
                                Option3 = x.Option3,
                                Answer = x.Answer
                            }).Where(x => x.QuestionImage != null).Distinct().OrderBy(x => Guid.NewGuid()).Take(10).ToListAsync();
                            var finalQuestionList = listOfImageQuestions.OrderBy(x => Guid.NewGuid()).ToList();

                            JhotPotModel zz = new JhotPotModel();
                            List<Qlis> yy = new List<Qlis>();
                            zz.IsPlayed = "0";
                            zz.Qlist = finalQuestionList;
                            zz.TimeDuration = 30;
                            return Ok(new { result = zz });
                        }
                        else if (ServiceType == "EkusheyQuiz")
                        {

                            var listOfQuestion = await _wapPortalContext.JhotPotThemeQuestions.Select(x => new Qlis
                            {
                                Id = x.Id.ToString(),
                                QuestionImage = x.QuestionImage,
                                Question = x.Question,
                                Option1 = x.Option1,
                                Option2 = x.Option2,
                                Option3 = x.Option3,
                                Answer = x.Answer,
                                QuestionCategory = x.QuestionCategory
                            }).Where(x => x.QuestionCategory == ServiceType).Distinct().OrderBy(x => Guid.NewGuid()).Take(Qcount).ToListAsync();

                            //var concatedQuestionList = listOfQuestion.Concat(listOfImageQuestions).ToList();
                            //var finalQuestionList = concatedQuestionList.OrderBy(x => Guid.NewGuid()).ToList();

                            JhotPotModel zz = new JhotPotModel();
                            List<Qlis> yy = new List<Qlis>();
                            zz.IsPlayed = "0";
                            zz.Qlist = listOfQuestion;
                            zz.TimeDuration = 60;
                            return Ok(new { result = zz });

                        }
                        else
                        {
                            var listOfImageQuestions = await _wapPortalContext.JhotPotThemeQuestions.Select(x => new Qlis
                            {
                                Id = x.Id.ToString(),
                                QuestionImage = x.QuestionImage,
                                Question = x.Question,
                                Option1 = x.Option1,
                                Option2 = x.Option2,
                                Option3 = x.Option3,
                                Answer = x.Answer
                            }).Where(x => x.QuestionImage != null).Distinct().OrderBy(x => Guid.NewGuid()).Take(10).ToListAsync();

                            var listOfQuestion = await _wapPortalContext.JhotPotThemeQuestions.Select(x => new Qlis
                            {
                                Id = x.Id.ToString(),
                                QuestionImage = x.QuestionImage,
                                Question = x.Question,
                                Option1 = x.Option1,
                                Option2 = x.Option2,
                                Option3 = x.Option3,
                                Answer = x.Answer
                            }).Distinct().OrderBy(x => Guid.NewGuid()).Take(Qcount - listOfImageQuestions.Count).ToListAsync();

                            var concatedQuestionList = listOfQuestion.Concat(listOfImageQuestions).ToList();
                            var finalQuestionList = concatedQuestionList.OrderBy(x => Guid.NewGuid()).ToList();

                            //var listOfImageQuestions = await _wapPortalContext.JhotPotThemeQuestions.Select(x => new Qlis
                            //{
                            //    Id = x.Id.ToString(),
                            //    QuestionImage = x.QuestionImage,
                            //    Question = x.Question,
                            //    Option1 = x.Option1,
                            //    Option2 = x.Option2,
                            //    Option3 = x.Option3,
                            //    Answer = x.Answer
                            //}).Where(x => x.QuestionImage != null).Distinct().OrderBy(x => Guid.NewGuid()).Take(qcount).ToListAsync();

                            //var listOfQuestion = await _wapPortalContext.JhotPotThemeQuestions.Select(x => new Qlis
                            //{
                            //    Id = x.Id.ToString(),
                            //    QuestionImage = x.QuestionImage,
                            //    Question = x.Question,
                            //    Option1 = x.Option1,
                            //    Option2 = x.Option2,
                            //    Option3 = x.Option3,
                            //    Answer = x.Answer
                            //}).Distinct().OrderBy(x => Guid.NewGuid()).Take(Qcount - listOfImageQuestions.Count).ToListAsync();

                            //var concatedQuestionList = listOfQuestion.Concat(listOfImageQuestions).ToList();
                            //var finalQuestionList = concatedQuestionList.OrderBy(x => Guid.NewGuid()).ToList();
                            //var finalQuestionList = listOfImageQuestions;

                            JhotPotModel zz = new JhotPotModel();
                            List<Qlis> yy = new List<Qlis>();
                            zz.IsPlayed = "0";
                            zz.Qlist = finalQuestionList;
                            zz.TimeDuration = 180;
                            return Ok(new { result = zz });
                        }


                    }
                    else if (count.Count == 1)
                    {
                        JhotPotModel zz = new JhotPotModel();
                        List<Qlis> yy = new List<Qlis>();
                        zz.IsPlayed = "0";
                        zz.TimeDuration = 0;
                        return Ok(new { result = zz });
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return Ok(new { result = true });

        }


        [HttpGet]
        public async Task<IActionResult> JhotpotAnswerWithTimeBkashForSpecialQuiz(string fbid, string qid, string answer, int timetaken, string ServiceName, string test = null, string ServiceType = "WorldCupQuiz")
        {
            if (test == null || test == "null")
            {
                int isRight = 0;
                try
                {
                    var qcheck = await _wapPortalContext.JhotPotThemeQuestions.Where(x => x.Id.ToString() == qid && x.Answer.ToLower() == answer.ToLower()).FirstOrDefaultAsync();

                    if (qcheck != null)
                    {
                        isRight = 1;
                    }

                    Int64 Intqid = Convert.ToInt64(qid);
                    //var round = _wapPortalContext.TblJhotpotPlayCounts.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date).FirstOrDefault() == null ? 1 : _wapPortalContext.TblJhotpotPlayCounts.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date).FirstOrDefault().Count;
                    //var condition = _wapPortalContext.TblJhotPotAnswers.Where(x => x.FbId == fbid && x.TimeStamp.Value.Date == DateTime.Now.Date && x.JhotpotId == Intqid && x.Round == round).FirstOrDefault();

                    //var roundCount = _wapPortalContext.TblJhotPotAnswers.Where(x => x.FbId == fbid && x.TimeStamp.Value.Date == DateTime.Now.Date && x.Round == round).Count();
                    //if (condition == null)
                    //{
                    //if (roundCount <= 60)
                    //{
                    await _wapPortalContext.TblJhotPotAnswerForSpecialQuizzes.AddAsync(new TblJhotPotAnswerForSpecialQuiz()
                    {
                        Answer = answer,
                        FbId = fbid,
                        IsRight = isRight,
                        JhotpotId = Convert.ToInt64(qid),
                        TimeTaken = timetaken,
                        TimeStamp = DateTime.Now,
                        Round = 1, //_wapPortalContext.TblJhotpotPlayCountForSpecialQuizzes.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date).FirstOrDefault().Count,
                        ServiceName = "QuizMasterBkash",
                        ServiceType = ServiceType
                    });
                    await _wapPortalContext.SaveChangesAsync();
                    //}

                    //}
                }
                catch (Exception ex)
                {
                }
                //var qcheck = await _context.JhotPots.Where(x => x.Id.ToString() == qid && x.Answer.ToLower() == answer.ToLower()).FirstOrDefaultAsync();


                return Ok(new { result = "success", isRight = isRight });
            }
            else
            {
                return Ok(new { result = "Failed" });
            }

        }


        /// <summary>
        /// 2023-11-14
        /// </summary>
        /// <param name="fbid"></param>
        /// <param name="qid"></param>
        /// <param name="answer"></param>
        /// <param name="timetaken"></param>
        /// <param name="ServiceName"></param>
        /// <param name="test"></param>
        /// <param name="ServiceType"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> JhotpotAnswerWithTimeBkash(string fbid, string qid, string answer, int timetaken, string ServiceName, string test = null)
        {
            if (test == null)
            {
                try
                {
                    var qcheck = await _wapPortalContext.JhotPotThemeQuestions.Where(x => x.Id.ToString() == qid && x.Answer.ToLower() == answer.ToLower()).FirstOrDefaultAsync();
                    int isRight = 0;
                    if (qcheck != null)
                    {
                        isRight = 1;
                    }


                    Int64 Intqid = Convert.ToInt64(qid);
                    await _wapPortalContext.TblJhotPotAnswers.AddAsync(new TblJhotPotAnswer()
                    {
                        Answer = answer,
                        FbId = fbid,
                        IsRight = isRight,
                        JhotpotId = Convert.ToInt64(qid),
                        TimeTaken = timetaken,
                        TimeStamp = DateTime.Now,
                        Round = _wapPortalContext.TblJhotpotPlayCounts.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date).Where(i => i.ServiceName == ServiceName).FirstOrDefault().Count,
                        ServiceName = ServiceName
                    });
                    await _wapPortalContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                }



                return Ok(new { result = "success" });
            }
            else
            {
                return Ok(new { result = "Failed" });
            }

        }
        ///




        [HttpGet]
        public async Task<IActionResult> JhotpotAnswerWithTimeBkashForLiveVideoQuiz(string fbid, string qid, string answer, int timetaken, string ServiceName, string test = null, string ServiceType = "WorldCupQuiz")
        {
            if (test == null || test == "null")
            {
                int isRight = 0;
                try
                {
                    var qcheck = await _wapPortalContext.TblLiveQuizVideoQuestionsLists.Where(x => x.Id.ToString() == qid && x.Answer.ToLower() == answer.ToLower()).FirstOrDefaultAsync();

                    if (qcheck != null)
                    {
                        isRight = 1;
                    }

                    Int64 Intqid = Convert.ToInt64(qid);
                    //var round = _wapPortalContext.TblJhotpotPlayCounts.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date).FirstOrDefault() == null ? 1 : _wapPortalContext.TblJhotpotPlayCounts.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date).FirstOrDefault().Count;
                    //var condition = _wapPortalContext.TblJhotPotAnswers.Where(x => x.FbId == fbid && x.TimeStamp.Value.Date == DateTime.Now.Date && x.JhotpotId == Intqid && x.Round == round).FirstOrDefault();

                    //var roundCount = _wapPortalContext.TblJhotPotAnswers.Where(x => x.FbId == fbid && x.TimeStamp.Value.Date == DateTime.Now.Date && x.Round == round).Count();
                    //if (condition == null)
                    //{
                    //if (roundCount <= 60)
                    //{
                    await _wapPortalContext.TblJhotPotAnswerForSpecialQuizzes.AddAsync(new TblJhotPotAnswerForSpecialQuiz()
                    {
                        Answer = answer,
                        FbId = fbid,
                        IsRight = isRight,
                        JhotpotId = Convert.ToInt64(qid),
                        TimeTaken = timetaken,
                        TimeStamp = DateTime.Now,
                        Round = 1, //_wapPortalContext.TblJhotpotPlayCountForSpecialQuizzes.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date).FirstOrDefault().Count,
                        ServiceName = "QuizMasterBkash",
                        ServiceType = ServiceType
                    });
                    await _wapPortalContext.SaveChangesAsync();
                    //}

                    //}
                }
                catch (Exception ex)
                {
                }
                //var qcheck = await _context.JhotPots.Where(x => x.Id.ToString() == qid && x.Answer.ToLower() == answer.ToLower()).FirstOrDefaultAsync();


                return Ok(new { result = "success", isRight = isRight });
            }
            else
            {
                return Ok(new { result = "Failed" });
            }

        }

        


        [HttpPost]
        public async Task<IActionResult> BreaktimequizAnswer([FromBody] AnswerRequestModel request)
        {
            try
            {
                // Check if the user has played for any round on the current date
                bool hasPlayedForCurrentDate = await _entity.BreaktimequizAnswers
                    .AnyAsync(a => a.FbId == request.FbId && a.TimeStamp.Date == DateTime.Now.Date);

                if (hasPlayedForCurrentDate)
                {
                    return Ok(new { result = "AlreadyPlayed" });
                }

                foreach (var answerModel in request.Answers)
                {
                    await _entity.BreaktimequizAnswers.AddAsync(new BreaktimequizAnswer
                    {
                        FbId = request.FbId,
                        Answer = answerModel,
                        Round = request.Round,
                        TimeStamp = DateTime.Now
                    });
                }

                await _entity.SaveChangesAsync();

                return Ok(new { result = "Success" });
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, logging, etc.
                return Ok(new { result = "Failed" });
            }
        }


        [HttpGet]
        public async Task<IActionResult> HasPlayedForRoundToday(string fbId, int round)
        {
            try
            {
                // Check if the user has played for the specified round on the current date
                bool hasPlayedForRoundToday = await _entity.BreaktimequizAnswers
                    .AnyAsync(a => a.FbId == fbId && a.Round == round && a.TimeStamp.Date == DateTime.Now.Date);

                return Ok(new { hasPlayedForRoundToday });
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, logging, etc.
                return Ok(new { error = "Failed to check play status" });
            }
        }




        /// <summary>
        /// /Ratul tbl_JhotpotWCup 9-14-23
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> JhotpotAnswerWithTimeBkashWC(string fbid, string qid, string answer, int timetaken, string ServiceName, string test = null)
        {
            if (test == null)
            {
                try
                {
                    var qcheck = await _wapPortalContext.JhotPotThemeQuestions.Where(x => x.Id.ToString() == qid && x.Answer.ToLower() == answer.ToLower()).FirstOrDefaultAsync();
                    int isRight = 0;
                    if (qcheck != null)
                    {
                        isRight = 1;
                    }

                    Int64 Intqid = Convert.ToInt64(qid);
            
                    await _wapPortalContext.TblJhotPotAnswersWcup.AddAsync(new TblJhotPotAnswerWCup()
                    {
                        Answer = answer,
                        FbId = fbid,
                        IsRight = isRight,
                        JhotpotId = Convert.ToInt64(qid),
                        TimeTaken = timetaken,
                        TimeStamp = DateTime.Now,
                        Round = _wapPortalContext.TblJhotpotPlayCounts.Where(i => i.FbId == fbid && i.TimeStamp.Value.Date == DateTime.Now.Date).Where(i => i.ServiceName == "QuizMasterBkashWCup").FirstOrDefault().Count,
                        ServiceName = "QuizMasterBkashWCup"
                    }); ;
                    await _wapPortalContext.SaveChangesAsync();
                    //}

                    //}
                }
                catch (Exception ex)
                {
                }
                //var qcheck = await _context.JhotPots.Where(x => x.Id.ToString() == qid && x.Answer.ToLower() == answer.ToLower()).FirstOrDefaultAsync();


                return Ok(new { result = "success" });
            }
            else
            {
                return Ok(new { result = "Failed" });
            }

        }





        public class AnswerRequestModel
        {
            public string FbId { get; set; }
            public int Round { get; set; }
            public List<string> Answers { get; set; }
        }


        public class JhotPotModel
        {
            public string IsPlayed { get; set; }
            public int? TimeDuration { get; set; }
            public List<Qlis> Qlist { get; set; }
        }

        public class Qlis
        {
            public string Id { get; set; }
            public string Question { get; set; }
            public string QuestionImage { get; set; }
            public string Option1 { get; set; }
            public string Option2 { get; set; }
            public string Option3 { get; set; }
            public string Answer { get; set; }
            public string Theme { get; set; }
            public string QuestionCategory { get; set; }
        }

    }
}
