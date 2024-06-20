using CloudDBLibrary.CreateTable;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLibraryFunctions.Pages
{
    public partial class CreateTableExamplePage : Page
    {
        public string CodeValue
        {
            get { return (string)GetValue(CodeValueProperty); }
            set { SetValue(CodeValueProperty, value); }
        }

        public static readonly DependencyProperty CodeValueProperty =
            DependencyProperty.Register("CodeValue", typeof(string), typeof(CreateTableExamplePage));


        public CreateTableExamplePage()
        {
            CodeValue = "List<CreateTableObject.Field> tables =\r\n[\r\n    CreateTableObject.Create(\"id\").TypeField(CreateTableObject.Type.INTEGER).PrimaryKey(),\r\n    CreateTableObject.Create(\"Name\").TypeField(CreateTableObject.Type.TEXT).NoKey(),\r\n    CreateTableObject.Create(\"Lastname\").TypeField(CreateTableObject.Type.TEXT).NoKey(),\r\n    CreateTableObject.Create(\"Patronomic\").TypeField(CreateTableObject.Type.TEXT).NoKey(),\r\n    CreateTableObject.Create(\"ForId\").TypeField(CreateTableObject.Type.INTEGER).ForeignKey(\"id\",\"for\")\r\n];\r\n\r\n///<summary>\r\n///Пример запроса для создания таблицы\r\n/// </summary>\r\nvar responce3 = await cloudDBClient.CreateTable(\"NewTable2\", tables);";
            InitializeComponent();

            ExecuteCommandBtn.MouseDown += async (s, e) =>
            {
                ResponceServerText.Text = await ExecuteRequest();
            };

            CopyCodeBtn.MouseDown += (s, e) =>
            {
                Clipboard.SetText(CodeValue);
                CopyCodeBtn.Visibility = Visibility.Collapsed;
                CopyCodeBtnText.Visibility = Visibility.Visible;
            };
        }

        public async Task<string> ExecuteRequest()
        {
            List<CreateTableObject.Field> tables = new List<CreateTableObject.Field>()
            {
                CreateTableObject.Create("id").TypeField(CreateTableObject.Type.INTEGER).PrimaryKey(),
                CreateTableObject.Create("Name").TypeField(CreateTableObject.Type.TEXT).NoKey(),
                CreateTableObject.Create("Lastname").TypeField(CreateTableObject.Type.TEXT).NoKey(),
                CreateTableObject.Create("Patronomic").TypeField(CreateTableObject.Type.TEXT).NoKey(),
                CreateTableObject.Create("ForId").TypeField(CreateTableObject.Type.INTEGER).ForeignKey("id","for")
            };

            var responce = await App.cloudDBClient.CreateTable("NewTable2", tables);
            return responce;
        }
    }
}
