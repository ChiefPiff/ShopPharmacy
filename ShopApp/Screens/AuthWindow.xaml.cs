using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace ShopApp.Screens
{
    public partial class AuthWindow : Window
    {
        private readonly HttpClient _httpClient;

        public AuthWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async void authBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginTextBox.Text) || string.IsNullOrEmpty(passTextBox.Text))
            {
                MessageBox.Show("Поля не заполнены!");
                return;
            }

            var loginData = new
            {
                Login = loginTextBox.Text,
                Password = passTextBox.Text
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            var isAdminAuth = await TryAuthenticateAsync("admin", content);

            if (isAdminAuth)
            {
                Random random = new Random();
                if (random.Next(0, 2) == 0)
                {
                    MessageBox.Show("Пожалуйста пройдите капчу");
                    CaptchaWindow captchaWindow = new CaptchaWindow();
                    if (captchaWindow.ShowDialog() == true)
                    {
                        MessageBox.Show($"Добро пожаловать, Админ");
                        this.Hide();
                        new AdminWindow(1).Show();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Капча не пройдена.");
                    }
                }
                else
                {
                    MessageBox.Show($"Добро пожаловать, Админ");
                    this.Hide();
                    new AdminWindow(1).Show();
                    return;
                }

            }

            var isSellerAuth = await TryAuthenticateAsync("seller", content);

            if (isSellerAuth)
            {
                MessageBox.Show($"Добро пожаловать, Продавец");
                this.Hide();
                // new SellerWindow().Show();
                return;
            }

            MessageBox.Show("Неправильный логин или пароль");
        }

        private async Task<bool> TryAuthenticateAsync(string role, StringContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync($"http://localhost:5247/api/auth/{role}", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}");
            }

            return false;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Программа \"Pharmacy\" версия 1.4\nРазработано Ибатуллиным И.И.\nВсе права защищены © 2024", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
