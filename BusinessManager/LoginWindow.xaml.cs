using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AppLog4Net;
using BusinessManager.Helps;
using BusinessManager.DataAccess;
using System.Threading;
using BusinessManager.Resources.Controls;

namespace BusinessManager
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : UserWindow
    {
        AppLog appLog = null;

        class LoginStateObject
        {
            public string UserName;
            public string Password;
            public DataManager dataManager;
        }

        public LoginWindow()
        {
            InitializeComponent();
            appLog = new AppLog("LoginWindows");
            UserNameText.Focus();
        } 

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginBtn.IsEnabled = false;
            LoadingPanel.Visibility = Visibility.Visible;
            LoginStateObject stateObject = new LoginStateObject
            {
                UserName = UserNameText.Text.Trim(),
                Password = PasswordText.Password.Trim(),
                dataManager = DataManager.dataManager
            };
            ThreadPool.QueueUserWorkItem(LoadingProcedure, stateObject);

        }

        private void LoadingProcedure(object state)
        {
            LoginStateObject stateObject = state as LoginStateObject;
            try
            {
                this.Dispatcher.BeginInvoke((Action)(() =>
                  {
                      LoadingText.Text = "用户正在登录...";
                  }));
                string message = null;
                if (!LoginDeal.DealLogin(stateObject.UserName, stateObject.Password, appLog, ref message))
                {
                    this.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        LoginBtn.IsEnabled = true;
                        LoadingPanel.Visibility = Visibility.Collapsed;
                        MessageBox.Show(message, "消息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }));
                }
                else
                {
                    this.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        LoadingText.Text = "正在加载数据...";
                    }));

                    stateObject.dataManager.BeginLoadData();
                    stateObject.dataManager.CurUserName = stateObject.UserName;

                    this.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            LoadingPanel.Visibility = Visibility.Collapsed;
                            this.DialogResult = true;
                            this.Close();
                        }));
                }
            }
            catch (Exception ex)
            {
                appLog.InfoFormat("Load Data Error:{0}", ex.Message + ex.StackTrace);
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    LoginBtn.IsEnabled = true;
                    LoadingPanel.Visibility = Visibility.Collapsed;
                    MessageBox.Show("抱歉，操作发生异常,请重试.");
                }));
            }
        }

        private void UserManagerBtn_Click(object sender, RoutedEventArgs e)
        {
            CardWindow cardWin = new CardWindow();
            cardWin.ShowDialog();
        }
    }
}
