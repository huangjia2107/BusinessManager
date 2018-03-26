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
    public partial class CardWindow : UserWindow
    {
        AppLog appLog = null; 

        public CardWindow()
        {
            InitializeComponent();
            appLog = new AppLog("CardWindow");
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(UserNameText.Text.Trim()) || string.IsNullOrEmpty(PrePasswordText.Password.Trim()) 
                || string.IsNullOrEmpty(NewPasswordText.Password.Trim())
                || string.IsNullOrEmpty(ConfirmPasswordText.Password.Trim()))
            {
                MessageBox.Show("所有项均为必填项,请填写完整.","消息", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(NewPasswordText.Password.Trim()!=ConfirmPasswordText.Password.Trim())
            {
                MessageBox.Show("新密码与确认密码不一致.","消息", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string message = null;
            if(loginRadioBtn.IsChecked==true)
            { 
                if (!LoginDeal.DealChangeLoginPassword(UserNameText.Text.Trim(), PrePasswordText.Password.Trim(),NewPasswordText.Password.Trim(), appLog, ref message))
                {
                    MessageBox.Show(message, "消息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                if (!LoginDeal.DealChangeFuncPassword(UserNameText.Text.Trim(), PrePasswordText.Password.Trim(),NewPasswordText.Password.Trim(), appLog, ref message))
                {
                    MessageBox.Show(message, "消息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            PrePasswordText.Password = "";
            NewPasswordText.Password = "";
            ConfirmPasswordText.Password = "";
            MessageBox.Show("恭喜您,修改密码成功.", "消息", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {

        } 
    }
}
