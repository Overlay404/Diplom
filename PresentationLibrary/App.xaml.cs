using LibraryCloudDB;
using System.Windows;

namespace PresentationLibrary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static AuthorizationClient authorizationClient = new AuthorizationClient("e1b7e325f5460f023c9e993e36070130", "http://127.0.0.1:8000/api");
        public static CloudDBClient cloudDBClient = new CloudDBClient(authorizationClient);
    }
}
