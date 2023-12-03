using FriendOrganizer.UI.ViewModel;
using System;
using System.Runtime.Remoting.Contexts;
using System.Windows;

namespace FriendOrganizer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private  MainViewModel mainViewModel;

        public MainWindow(MainViewModel mainViewModel)
        {
           
            this.mainViewModel = mainViewModel;
            DataContext = this.mainViewModel;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
          await  mainViewModel.Load();
        }
    }
}
