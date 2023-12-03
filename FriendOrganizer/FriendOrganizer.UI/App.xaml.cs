using Autofac;
using FriendOrganizer.UI.Statrup;
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
    }
}
