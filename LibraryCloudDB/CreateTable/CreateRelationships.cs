namespace LibraryCloudDB.CreateTable
{
    public class CreateRelationships : CreateTableObject
    {
        public CreateRelationships(Field field, References references)
        {
            this.field = field;
            this.references = references;
        }

        public Field PrimaryKey(bool autoincrement = true)
        {
            field.Is_primary_key = true;
            field.Autoincrement = autoincrement;
            return field;
        }

        public Field ForeignKey(string fieldName, string tableName)
        {
            field.Is_foregraund_key = true;
            references.Field = fieldName;
            references.Table = tableName;
            field.References = references;
            return field;
        }

        public Field NoKey()
        {
            return field;
        }


    }
}
