namespace LibraryCloudDB
{
    public class AuthorizationClient
    {
        public string secretToken;
        public string baseUrl;

        public AuthorizationClient(string secretToken, string baseUrl)
        {
            this.secretToken = secretToken;
            this.baseUrl = baseUrl;
        }
    }
}
