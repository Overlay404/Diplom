using PresentationLibraryFunctions.Pages;
using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PresentationLibraryFunctions
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
            string nameTypePages = "PresentationLibraryFunctions.Pages." + ((Pages)PageNow).ToString();
            Type typePages = Type.GetType(nameTypePages);
            MyFrame.Navigate(Activator.CreateInstance(typePages));
        }

        enum Pages
        {
            CreateTableExamplePage,
            AddRowExamplePage
        }
    }
}
