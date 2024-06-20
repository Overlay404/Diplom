using LibraryCloudDB.CreateTable;
using LibraryCloudDB.Http;
using LibraryCloudDB.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCloudDB
{
    public class CloudDBClient
    {
        private readonly string baseUrl;
        private readonly string secretToken;

        public CloudDBClient(AuthorizationClient authorizationClient)
        {
            baseUrl = authorizationClient.baseUrl;
            secretToken = authorizationClient.secretToken;

            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }
        }

        public CloudDBQuery Child(string nameTable)
        {
            var tableName = nameTable + '/';

            return new CloudDBQuery(secretToken, baseUrl + tableName);
        }

        public async Task<string> CreateTable(string nameTable, IEnumerable<CreateTableObject.Field> tableObjects)
        {
            return await HttpClientExtensions.PostObjectAsync(new HttpClient(), baseUrl + "create_table?table_name=" + nameTable + "&token=" + secretToken, tableObjects);
        }
    }
}
