using Autofac;
using FriendOrganizer.UI.Statrup;
using System;
using System.Windows;

namespace FriendOrganizer.UI
{

    public partial class App : Application
    {
        public void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrap = new Bootstraper();
            var container = bootstrap.Bootstrap();
            var mainWindow=container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured.Please inform the admin"+Environment.NewLine+e.Exception.Message,"Unexcepted error");    
            e.Handled = true;
        }
    }
}
