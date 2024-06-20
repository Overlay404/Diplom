using System.Runtime.CompilerServices;

namespace CloudDB.Http
{
    public class BuildUrl
    {
        public static string Url
        {
            get
            {
                return "http://127.0.0.1:8000/";
            }
        }

        public static string GetUrl(string requestUrl)
        {
            if(requestUrl.IndexOf('\"') != -1)
            {
                requestUrl = string.Concat(requestUrl.AsEnumerable().Where(ch => !ch.Equals('\"')));
            }

            return Url + requestUrl;
        }
    }
}
