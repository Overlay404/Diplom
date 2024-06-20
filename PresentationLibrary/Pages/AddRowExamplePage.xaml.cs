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
    public partial class AddRowExamplePage : Page
    {
        public string CodeValue
        {
            get { return (string)GetValue(CodeValueProperty); }
            set { SetValue(CodeValueProperty, value); }
        }

        public static readonly DependencyProperty CodeValueProperty =
            DependencyProperty.Register("CodeValue", typeof(string), typeof(AddRowExamplePage));


        public AddRowExamplePage()
        {
            CodeValue = "///<summary>\r\n///Пример запроса для добавления данных в таблицу\r\n/// </summary>\r\nvar resposnse = await cloudDBClient.Child(\"NewTable2\").AddAsync(new NewTable\r\n{\r\n    Name = \"Ivan\",\r\n    Lastname = \"Ivanov\",\r\n    Patronomic = \"Ivanovich\",\r\n    ForId = 1\r\n});";
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
            ///Пример запроса для добавления данных в таблицу
            /// </summary>
            var responce = await App.cloudDBClient.Child("NewTable2").AddAsync(new NewTable
            {
                Name = "Ivan",
                Lastname = "Ivanov",
                Patronomic = "Ivanovich",
                ForId = 1
            });
            return responce;
        }
    }
}
