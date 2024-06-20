using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLibrary
{
    public partial class MainWindow : Window
    {
        public int PageNow
        {
            get { return (int)GetValue(PageNowProperty); }
            set { SetValue(PageNowProperty, value); }
        }

        public static readonly DependencyProperty PageNowProperty =
            DependencyProperty.Register("PageNow", typeof(int), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();

            NavigateToLeftButton.MouseDown += NavigateToLeft;
            NavigateToRightButton.MouseDown += NavigateToRight;
            Navigate();
        }

        public void NavigateToLeft(object sender, RoutedEventArgs e)
        {
            if (PageNow <= 0)
            {
                PageNow = 0;
                return;
            }

            PageNow -= 1;
            try
            {
                Navigate();
            }
            catch
            {
                MessageBox.Show("Не удалось перейти на предыдущую страницу");
                PageNow += 1;
            }
        }

        public void NavigateToRight(object sender, RoutedEventArgs e)
        {
            PageNow += 1;
            try
            {
                Navigate();
            }
            catch
            {
                MessageBox.Show("Не удалось перейти на следующую страницу");
                PageNow -= 1;
            }
        }

        public void Navigate()
        {
            string nameTypePages = "PresentationLibrary.Pages." + ((Pages)PageNow).ToString();
            Type typePages = Type.GetType(nameTypePages);
            if(typePages == null)
            {
                return;
            }
            MyFrame.Navigate(Activator.CreateInstance(typePages));
        }

        enum Pages
        {
            CreateTableExamplePage,
            AddRowExamplePage,
            UpdateRowExamplePage,
            DeleteRowExamplePage,
            GetDataExamplePage
        }
    }
}