using System;
using System.Net.Http;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using Castle.Core.Internal;
using Newtonsoft.Json;
using System.Net;

namespace ShopApp.Screens
{
    public partial class SellerRegWindow : Window
    {
        private int _adminId;
        private readonly HttpClient _httpClient;

        public SellerRegWindow(int adminId)
        {
            InitializeComponent();
            _adminId = adminId;
            _httpClient = new HttpClient();
        }

        private async void regBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginTextBox.Text) || string.IsNullOrEmpty(passwordConfirmTextBox.Text) || string.IsNullOrEmpty(passwordTextBox.Text) || string.IsNullOrEmpty(EmailTextBox.Text))
            {
                MessageBox.Show("Не все поля заполнены.");
            }
            else
            {
                string login = loginTextBox.Text;
                string password = passwordTextBox.Text;
                string confirmPassword = passwordConfirmTextBox.Text;
                string email = EmailTextBox.Text;

                if (password != confirmPassword)
                {
                    MessageBox.Show("Пароли не совпадают.");
                    return;
                }

                string confirmationCode = GenerateConfirmationCode();

                await SendConfirmationEmail(email, confirmationCode);

                var confirmEmailWindow = new ConfirmEmailWindow(confirmationCode);
                bool? result = confirmEmailWindow.ShowDialog();

                if (result == true)
                {
                    var registrationData = new
                    {
                        Login = login,
                        Password = password,
                        ConfirmPassword = confirmPassword
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(registrationData), Encoding.UTF8, "application/json");

                    var success = await RegisterSellerAsync(content);

                    if (success)
                    {
                        MessageBox.Show("Продавец успешно зарегистрирован.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка регистрации.");
                    }
                }
                else
                {
                    MessageBox.Show("Неверный код подтверждения. Регистрация отменена.");
                }
            }
        }

        private string GenerateConfirmationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }


        public async Task SendConfirmationEmail(string email, string confirmationCode)
        {
            try
            {
                var fromAddress = new MailAddress("ilnur.ibatullin.05@mail.ru", "Shop App");
                var toAddress = new MailAddress(email);
                const string fromPassword = "fefPR194hQq3mTheYX5D";
                const string subject = "Подтверждение регистрации";
                string body = $"Ваш код подтверждения: {confirmationCode}";

                var smtp = new SmtpClient
                {
                    Host = "smtp.mail.ru",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке email: {ex.Message}");
            }
        }


        private async Task<bool> RegisterSellerAsync(StringContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync("http://localhost:5247/api/reg/register", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
                return false;
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
