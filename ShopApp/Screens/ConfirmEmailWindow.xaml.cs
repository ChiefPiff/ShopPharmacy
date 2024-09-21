using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShopApp.Screens
{
    /// <summary>
    /// Логика взаимодействия для ConfirmEmailWindow.xaml
    /// </summary>
    public partial class ConfirmEmailWindow : Window
    {
        private string _correctCode;

        public ConfirmEmailWindow(string confirmationCode)
        {
            InitializeComponent();
            _correctCode = confirmationCode;
        }

        private void CheckCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = CodeInputTextBox.Text;

            if (userInput == _correctCode)
            {
                MessageBox.Show("Код подтверждения верный!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный код подтверждения.");
                this.DialogResult = false;
            }
        }
    }

}
