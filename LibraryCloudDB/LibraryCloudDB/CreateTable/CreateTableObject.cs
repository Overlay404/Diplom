namespace LibraryCloudDB.CreateTable
{
    public class CreateTableObject
    {
        protected Field field;
        protected References references;

        public class Field
        {
            public string Name { get; set; }
            public string Type_field { get; set; }
            public bool Is_primary_key { get; set; }
            public bool Autoincrement { get; set; }
            public bool Is_foregraund_key { get; set; }
            public References? References { get; set; }
        }

        public class References
        {
            public string Table { get; set; }
            public string Field { get; set; }
        }

        public static CreateField Create(string Name)
        {
            return new CreateField(new Field() { Name = Name }, new References());
        }

        public enum Type
        {
            INTEGER,
            TEXT,
            NUMERIC,
            REAL,
            BLOB
        }
    }
}
