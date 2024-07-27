using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class Browser
    {
        public string name { get; set; }
        public object version { get; set; }
        public object version_major { get; set; }
        public string engine { get; set; }
    }

    public class Crawler
    {
        public bool is_crawler { get; set; }
        public object category { get; set; }
        public object last_seen { get; set; }
    }

    public class Device
    {
        public bool is_mobile_device { get; set; }
        public string type { get; set; }
        public string brand { get; set; }
        public string brand_code { get; set; }
        public string brand_url { get; set; }
        public string name { get; set; }
    }

    public class Os
    {
        public string name { get; set; }
        public string code { get; set; }
        public string url { get; set; }
        public string family { get; set; }
        public string family_code { get; set; }
        public string family_vendor { get; set; }
        public string icon { get; set; }
        public string icon_large { get; set; }
    }

    public class DeviceTrackResponse
    {
        public string ua { get; set; }
        public string type { get; set; }
        public string brand { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Os os { get; set; }
        public Device device { get; set; }
        public Browser browser { get; set; }
        public Crawler crawler { get; set; }
    }
    public class UserStackDT
    {
        private string accessKey = "1fb30aa5bc6edf2e0883a0696de629e7";

        public DeviceTrackResponse GetResponse(string useragent)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://api.userstack.com/detect?access_key=" + accessKey + "&ua=" + useragent);
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
                DeviceTrackResponse ss = JsonConvert.DeserializeObject<DeviceTrackResponse>(test);
                return ss;
            }
            catch (Exception ex)
            {
            }
            return new DeviceTrackResponse();
        }
    }
}
