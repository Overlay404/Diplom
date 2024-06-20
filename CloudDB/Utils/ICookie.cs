namespace CloudDB.Utils
{
    public interface ICookie
    {
        public Task SetValue(string key, string value);
        public Task DeleteValue(string key);
        public Task<string> GetValue(string key, string def = "");
    }

}
