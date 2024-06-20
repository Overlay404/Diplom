using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace LibraryCloudDB.Http
{
    internal static class HttpClientExtensions
    {

        public static async Task<IReadOnlyCollection<CloudDBObject<T>>> GetObjectCollectionAsync<T>(this HttpClient client, string requestUri)
        {
            var responseData = string.Empty;
            var statusCode = HttpStatusCode.OK;

            try
            {
                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                statusCode = response.StatusCode;
                responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var list = JsonConvert.DeserializeObject<List<T>>(responseData);
                if (list == null)
                {
                    return Array.Empty<CloudDBObject<T>>();
                }

                return list.Select((item, index) => new CloudDBObject<T>(index.ToString(), item)).ToList();
            }
            catch
            {
                throw;
            }
        }

        public static async Task<string> GetAsync(this HttpClient client, string requestUri)
        {
            try
            {
                var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                HttpStatusCode statusCode = response.StatusCode;
                string? responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                response.EnsureSuccessStatusCode(); 

                return responseData;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<string> PostObjectAsync<T>(this HttpClient client, string requestUri, T objectRequest)
        {
            try
            {
                var message = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = JsonContent.Create(objectRequest)
                };

                var result = await client.SendAsync(message).ConfigureAwait(false);
                var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                return responseData;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<string> PostAsync(this HttpClient client, string requestUri)
        {
            try
            {
                var message = new HttpRequestMessage(HttpMethod.Post, requestUri);
                var result = await client.SendAsync(message).ConfigureAwait(false);
                var responseData = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                return responseData;
            }
            catch
            {
                throw;
            }
        }
    }
}
