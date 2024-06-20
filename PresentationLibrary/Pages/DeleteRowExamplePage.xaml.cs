using System.Windows;
using System.Windows.Controls;

namespace PresentationLibrary.Pages
{
    public partial class DeleteRowExamplePage : Page
    {
        public string CodeValue
        {
            get { return (string)GetValue(CodeValueProperty); }
            set { SetValue(CodeValueProperty, value); }
        }

        public static readonly DependencyProperty CodeValueProperty =
            DependencyProperty.Register("CodeValue", typeof(string), typeof(DeleteRowExamplePage));


        public DeleteRowExamplePage()
        {
            CodeValue = "///<summary>\r\n///Пример запроса для удаления данных в таблице\r\n/// </summary>\r\nvar responce = await App.cloudDBClient.Child(\"NewTable2\").DeleteAsync(2);\r\nreturn responce;";
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
            ///Пример запроса для удаления данных в таблице
            /// </summary>
            var responce = await App.cloudDBClient.Child("NewTable2").DeleteAsync(2);
            return responce;
        }
    }
}
