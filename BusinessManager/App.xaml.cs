using BusinessManager.Helps;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace BusinessManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "My_Unique_Application_ERP";
        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();
                application.InitializeComponent();
                application.Run();
                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
            else
                MessageBox.Show("当前程序已经运行.");
        }
        #region ISingleInstanceApp Members
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // handle command line arguments of second instance
            // ...
            return true;
        }
        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
                LoginWindow window = new LoginWindow();
                bool? dialogResult = window.ShowDialog();
                if ((dialogResult.HasValue) && (dialogResult.Value))
                {
                    base.OnStartup(e);
                    Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    MainWindow m_MianWindow = new MainWindow();
                    m_MianWindow.ShowDialog();
                }
                else
                {
                    this.Shutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException);
            }
        }
    }
}
