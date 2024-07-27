using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace QuizStarPortal.Models
{
    public class DD
    {
        public string wurflDeviceId { get; set; }
        public string wurflDeviceBrandName { get; set; }
        public string model_name { get; set; }
        public string manufac { get; set; }
        public string dimension { get; set; }
        public string os { get; set; }
        public string uaxml { get; set; }
    }

    public class Hsprofiling
    {
        public DD GetResponse(string useragent)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://wap.shabox.mobi/hsprofiling/api/t/r?useragent=" + useragent);
                request.Method = "GET";
                String test = String.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    test = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
                DD ss = JsonConvert.DeserializeObject<DD>(test);
                return ss;
            }
            catch (Exception ex)
            {


            }
            return new DD();
        }
        public DD PostResponse(string useragent)
        {
            var client = new RestClient("https://wap.shabox.mobi/hsprofiling/api/p/r");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\"useragent\":\"" + useragent + "\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            DD ss = JsonConvert.DeserializeObject<DD>(response.Content);
            return ss;
        }


    }
}
