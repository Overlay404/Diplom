namespace LibraryCloudDB
{
    public class CloudDBObject<T>
    {
        internal CloudDBObject(string key, T obj)
        {
            Key = key;
            Object = obj;
        }

        public string Key
        {
            get;
        }

        public T Object
        {
            get;
        }
    }
}
