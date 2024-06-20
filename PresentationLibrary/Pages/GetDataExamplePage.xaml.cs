using LibraryCloudDB;
using PresentationLibrary.Model;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLibrary.Pages
{
    public partial class GetDataExamplePage : Page
    {
        public string CodeValue
        {
            get { return (string)GetValue(CodeValueProperty); }
            set { SetValue(CodeValueProperty, value); }
        }

        public static readonly DependencyProperty CodeValueProperty =
            DependencyProperty.Register("CodeValue", typeof(string), typeof(GetDataExamplePage));


        public GetDataExamplePage()
        {
            CodeValue = "///<summary>\r\n///Пример запроса для получения данных из таблицы\r\n/// </summary>\r\nvar responce = string.Empty;\r\nIEnumerable<CloudDBObject<NewTable>> cloudDBObject = await App.cloudDBClient.Child(\"For\").GetAsync<NewTable>();\r\nforeach (var item in cloudDBObject)\r\n{\r\n    responce += \"id \" + item.Object.id + \"\\n\";\r\n    responce += \"Name \" + item.Object.Name + \"\\n\";\r\n    responce += \"Lastname \" + item.Object.Lastname + \"\\n\";\r\n    responce += \"Patronomic \" + item.Object.Patronomic + \"\\n\";\r\n    responce += \"ForId \" + item.Object.ForId + \"\\n\";\r\n    responce += \"---------------------------------------------------\";\r\n}\r\nreturn responce;";
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
            ///<summary>
            ///Пример запроса для получения данных из таблицы
            /// </summary>
            var responce = string.Empty;
            IEnumerable<CloudDBObject<NewTable>> cloudDBObject = await App.cloudDBClient.Child("NewTable2").GetAsync<NewTable>();
            foreach (var item in cloudDBObject)
            {
                responce += "id: " + item.Object.id + "\n";
                responce += "Name: " + item.Object.Name + "\n";
                responce += "Lastname: " + item.Object.Lastname + "\n";
                responce += "Patronomic: " + item.Object.Patronomic + "\n";
                responce += "ForId: " + item.Object.ForId + "\n";
                responce += "---------------------------------------------------\n";
            }
            return responce;
        }
    }
}
