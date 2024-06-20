using CloudDBLibrary.CreateTable;

namespace CloudDB.Model
{
    public class TableStructure
    {
        public string? name { get; set; }
        public string? type_field { get; set; }
        public bool is_primary_key { get; set; }
        public bool is_foregraund_key { get; set; }
        public References references { get; set; } = new References();
     

        public enum Type
        {
            INTEGER,
            TEXT,
            NUMERIC,
            REAL,
            BLOB
        }
    }

    public class References
    {
        public string table { get; set; } = string.Empty;
        public string field { get; set; } = string.Empty;
    }
}
