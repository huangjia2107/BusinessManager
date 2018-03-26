using AppLog4Net;
using BusinessManager.DataAccess;
using BusinessManager.Resources.Controls;
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

namespace BusinessManager
{
    /// <summary>
    /// FuncPasswordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FuncPasswordWindow : UserWindow
    {
        AppLog appLog = null; 

        public FuncPasswordWindow(string _CurUserName)
        {
            InitializeComponent();
            appLog = new AppLog("FuncPasswordWindow");
            UserNameText.Text = _CurUserName;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(FuncPasswordText.Password.Trim()))
            {
                MessageBox.Show("抱歉,密码不能为空.", "消息", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string message = null;
            if (!LoginDeal.DealFunc(UserNameText.Text.Trim(), FuncPasswordText.Password.Trim(), appLog, ref message))
            {
                MessageBox.Show(message, "消息", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                DialogResult = true;
                this.Close();
            }
        }
    }
}
