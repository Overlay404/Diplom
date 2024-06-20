using LibraryCloudDB.Http;
using System.Net.Http;

namespace LibraryCloudDB.Query
{
    public class CloudDBQuery
    {
        private HttpClient client;
        private readonly string secretToken;
        private readonly string baseUrl;

        public CloudDBQuery(string secretToken, string url)
        {
            this.secretToken = secretToken;
            baseUrl = url;

            ArgumentNullException.ThrowIfNull(secretToken);
            ArgumentNullException.ThrowIfNull(url);

            client = GetHttpClient();
        }

        public async Task<IEnumerable<CloudDBObject<T>>> GetAsync<T>()
        {
            var url = buildingUrl(secretToken, "get");
            return await HttpClientExtensions.GetObjectCollectionAsync<T>(client, baseUrl + url);
        }

        public async Task<string> AddAsync<T>(T objectRequest)
        {
            var url = buildingUrl(secretToken, "add");
            return await HttpClientExtensions.PostObjectAsync<T>(client, baseUrl + url, objectRequest);
        }

        public async Task<string> UpdateAsync<T>(T oldObject, T newObject)
        {
            var url = buildingUrl(secretToken, "update");
            dynamic objectRequest = new System.Dynamic.ExpandoObject();
            objectRequest.new_value = newObject;
            objectRequest.old_value = oldObject;
            return await HttpClientExtensions.PostObjectAsync<object>(client, baseUrl + url, objectRequest);
        }

        public async Task<string> DeleteAsync(int id)
        {
            var url = buildingUrl(secretToken, "delete", id.ToString());
            return await HttpClientExtensions.GetAsync(client, baseUrl + url);
        }

        private static HttpClient GetHttpClient()
        {
            return new HttpClient();
        }

        private string buildingUrl(string secretToken, string action, string id = "")
        {
            if (id == "")
                return $"{action}/?token={secretToken}";
            else
                return $"{action}/?id={id}&token={secretToken}";
        }

    }
}
