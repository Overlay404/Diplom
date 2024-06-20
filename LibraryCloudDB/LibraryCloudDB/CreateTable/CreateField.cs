namespace LibraryCloudDB.CreateTable
{
    public class CreateField : CreateTableObject
    {
        public CreateField(Field field, References references)
        {
            this.field = field;
            this.references = references;
        }

        public CreateRelationships TypeField(Type type)
        {
            switch (type)
            {
                case Type.INTEGER:
                    field.Type_field = "INTEGER";
                    break;
                case Type.TEXT:
                    field.Type_field = "TEXT";
                    break;
                case Type.REAL:
                    field.Type_field = "REAL";
                    break;
                case Type.NUMERIC:
                    field.Type_field = "NUMERIC";
                    break;
                case Type.BLOB:
                    field.Type_field = "BLOB";
                    break;
            }


            return new CreateRelationships(field, references);
        }
    }
}
