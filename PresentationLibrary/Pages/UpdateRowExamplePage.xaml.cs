using LibraryCloudDB;
using LibraryCloudDB.CreateTable;
using PresentationLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLibrary.Pages
{
    public partial class UpdateRowExamplePage : Page
    {
        public string CodeValue
        {
            get { return (string)GetValue(CodeValueProperty); }
            set { SetValue(CodeValueProperty, value); }
        }

        public static readonly DependencyProperty CodeValueProperty =
            DependencyProperty.Register("CodeValue", typeof(string), typeof(UpdateRowExamplePage));


        public UpdateRowExamplePage()
        {
            CodeValue = "var responce = await App.cloudDBClient.Child(\"NewTable2\").UpdateAsync<NewTable>(new NewTable\r\n{\r\n    id = 1,\r\n    Name = \"Ivan\",\r\n    Lastname = \"Ivanov\",\r\n    Patronomic = \"Ivanovich\",\r\n    ForId = 1\r\n}, new NewTable\r\n{\r\n    Name = \"Misha\",\r\n    Lastname = \"Popov\",\r\n    Patronomic = \"Zenkovich\",\r\n    ForId = 2\r\n});\r\nreturn responce;";
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
            ///Пример запроса для обнавления данных в таблице
            /// </summary>
            var responce = await App.cloudDBClient.Child("NewTable2").UpdateAsync<NewTable>(new NewTable
            {
                id = 1,
                Name = "Ivan",
                Lastname = "Ivanov",
                Patronomic = "Ivanovich",
                ForId = 1
            }, new NewTable
            {
                Name = "Misha",
                Lastname = "Popov",
                Patronomic = "Zenkovich",
                ForId = 2
            });
            return responce;
        }
    }
}
