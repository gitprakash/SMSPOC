using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DataServiceLibrary
{
    public class ExternalMessageServiceAPI
    {
        public async Task<string> SendMessage(string url)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                DateTime dtsenttime;
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await httpclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
                //  bool isTime= DateTime.TryParse(strresult, out dtsenttime);
            }
        }
    }
}
