using AppLog4Net;
using BusinessManager.DataAccess;
using BusinessManager.Resources.Controls;
using DataAccess.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace BusinessManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserWindow
    {
        AppLog appLog = null; 

        public MainWindow()
        {
            InitializeComponent(); 
            appLog = new AppLog("MainWindow"); 
        } 
    }
}
