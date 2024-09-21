using System;
using System.Windows;

namespace ShopApp.Screens
{
    public partial class CaptchaWindow : Window
    {
        private string _generatedCaptcha;

        public CaptchaWindow()
        {
            InitializeComponent();
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] captcha = new char[6];

            for (int i = 0; i < captcha.Length; i++)
            {
                captcha[i] = allowedChars[random.Next(allowedChars.Length)];
            }

            _generatedCaptcha = new string(captcha);
            CaptchaTextBlock.Text = _generatedCaptcha;
        }

        private void CheckCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = CaptchaInputTextBox.Text;

            if (userInput == _generatedCaptcha)
            {
                MessageBox.Show("Капча введена правильно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильная капча. Попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                GenerateCaptcha();
                CaptchaInputTextBox.Clear();
            }
        }
    }
}
