using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace MVC
{

    public class Apiclass
    {

        public static string apiurl = Convert.ToString(ConfigurationManager.AppSettings["apiurl"]);
        public static string accept = "application/json";
        public static string content_type = "application/x-www-form-urlencoded";
        public static string jsoncontentype = "application/json";
        private delegate Task<string> CallAPIDIGT();
        public enum TypeContent
        {
            JSON = 0,
            STRING = 1
        }
        TypeContent Ctype { get; set; }
        HttpMethod httpmthd;
        string controllername;
        string methodname;
        string body;
        public Apiclass(HttpMethod httpmthd, string controllername, string methodname, string body, TypeContent Ctype)
        {
            this.httpmthd = httpmthd;
            this.controllername = controllername;
            this.methodname = methodname;
            this.body = body;
            this.Ctype = Ctype;
        }
        public string apicall()
        {
            CallAPIDIGT apgcall = new CallAPIDIGT(callapi);
            IAsyncResult asyncResult1 = apgcall.BeginInvoke(null, null);
            Task<string> taskresponse = apgcall.EndInvoke(asyncResult1);
            string response = taskresponse.Result;
            return response;
        }
        protected async Task<string> callapi()
        {
            try
            {
                string struri = "";
                if (httpmthd == HttpMethod.Post)
                {
                    struri = apiurl + "/" + controllername + "/" + methodname;
                }
                else if (httpmthd == HttpMethod.Get)
                {
                    struri = apiurl + "/" + controllername + "/" + methodname + "?" + body;
                }
                var Request = new HttpRequestMessage(httpmthd, struri);
                Request.Headers.Add("accept", "application/json");
                Request.Headers.Add("cache-control", "no-store");
                Request.Headers.Add("Authorization", "No Auth");
                if (httpmthd == HttpMethod.Post)
                {
                    if (Ctype == TypeContent.JSON)
                    {
                        Request.Content = new StringContent(body, System.Text.Encoding.UTF8, jsoncontentype);
                    }

                    if (Ctype == TypeContent.STRING)
                    {
                        Request.Content = new StringContent(body, System.Text.Encoding.UTF8, content_type);
                    }

                }
                HttpClient client = new HttpClient();
                var response = await client.SendAsync(Request);
                //response.EnsureSuccessStatusCode();
                string responsebody = "";
                if (response.IsSuccessStatusCode)
                {
                    responsebody = await response.Content.ReadAsStringAsync() + "*" + response.StatusCode.ToString();
                    // responsebody = response.StatusCode.ToString();

                }
                else
                {
                    var ErrMsg = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                    // MessageBox.Show(ErrMsg.Message);
                    responsebody = ErrMsg.Message.ToString() + "*" + response.StatusCode.ToString();
                    //(int)response.StatusCode,
                }
                //string responsebody = await response.Content.ReadAsStringAsync();
                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //string mp = (string)serializer.Deserialize(responsebody,typeof(String));
                return responsebody;
            }
            catch (HttpRequestException ex)
            {

                return ex.ToString();
            }
        }
    }
}