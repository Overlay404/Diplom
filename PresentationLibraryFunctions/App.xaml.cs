using LibraryCloudDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PresentationLibraryFunctions
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        static AuthorizationClient authorizationClient = new AuthorizationClient("e1b7e325f5460f023c9e993e36070130", "http://127.0.0.1:8000/api");
        public static CloudDBClient cloudDBClient = new CloudDBClient(authorizationClient);
    }
}
