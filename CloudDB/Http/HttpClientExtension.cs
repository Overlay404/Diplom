using CloudDB.Model;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudDB.Http
{
    public class HttpClientExtension
    {

        public static HttpClient GetClient()
        {
            return new HttpClient();
        }

        public static async Task<string> PostRequestAsync<T>(HttpClient client, string url, T obj)
        {
            string responseData;
            HttpStatusCode statusCode;

            JsonContent json = JsonContent.Create(obj);
            var responce = await client.PostAsync(url, json);
            statusCode = responce.StatusCode;
            responseData = await responce.Content.ReadAsStringAsync();

            if(statusCode != HttpStatusCode.OK)
            {
                throw new HttpProtocolException((int) statusCode, responseData, null);
            }

            return responseData;
        }

        public static async Task<string> GetRequestAsync(HttpClient client, string url, Cookie? cookie = null)
        {

            if(cookie != null)
            {
                client = CheckCookie(cookie);
            }

            string responseData;
            HttpStatusCode statusCode;

            try
            {
                var response = await client.GetAsync(url).ConfigureAwait(false);
                statusCode = response.StatusCode;
                responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }

            return responseData;
        }

        public static async Task<T> GetRequestAsyncWithParameterToConvert<T>(HttpClient client, string url, Cookie? cookie = null)
        {

            if (cookie != null)
            {
                client = CheckCookie(cookie);
            }

            string responseData;
            HttpStatusCode statusCode;
            T responseObject;

            try
            {
                var response = await client.GetAsync(url).ConfigureAwait(false);
                statusCode = response.StatusCode;
                responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                responseObject = FromJson<T>(responseData);

                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }

            return responseObject;
        }

        private static HttpClient CheckCookie(Cookie cookie)
        {
            Uri uri = new Uri("http://localhost:5274");

            var cookies = new CookieContainer();
            cookies.Add(uri, cookie);
            var handler = new HttpClientHandler
            {
                CookieContainer = cookies
            };
            HttpClient client = new(handler);
            client.DefaultRequestHeaders.Add("cookie", cookies.GetCookieHeader(uri));
            return client;
        }

        public static string ToJson<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T? FromJson<T>(string json)
        {
            T? data;
            try
            {
                data = JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                throw;
            }
            return data;
        }
    }
}
