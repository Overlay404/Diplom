namespace CloudDB.Model
{
    public class UpdateRequestSerializeble
    {
        public Dictionary<string, object>? new_value { get; set; }
        public Dictionary<string, object>? old_value { get; set; }
    }
}
