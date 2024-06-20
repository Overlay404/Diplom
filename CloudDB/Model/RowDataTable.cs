using System.Text.Json.Serialization;

namespace CloudDB.Model
{
    public class RowDataTable
    {
        public required Dictionary<string, object> Dict { get; set; }
        [JsonIgnore]
        public bool isEditing;
    }
}
