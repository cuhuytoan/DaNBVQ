using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CMS_V3.Helper
{
    public static class HttpClientHelper
    {
        public static HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://apitest.hanoma.vn");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
