using Microsoft.AspNetCore.Mvc;
using ShopApi.Model;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ShopbdContext _context;

        public AuthController(ShopbdContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Авторизует пользователя как администратора.
        /// </summary>
        /// <param name="model">Модель авторизации.</param>
        /// <returns>Статус авторизации.</returns>
        /// <response code="200">Успешная авторизация.</response>
        /// <response code="401">Неправильный логин или пароль.</response>
        [HttpPost("admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserLogin == model.Login);

            if (user == null)
            {
                return Unauthorized("Неправильный логин или пароль.");
            }

            string hashedInputPassword = ComputeSha256Hash(model.Password, user.UserSalt);

            if (user.UserPassword == hashedInputPassword)
            {
                return Ok("Успешная авторизация.");
            }
            else
            {
                return Unauthorized("Неправильный логин или пароль.");
            }
        }

        private static string ComputeSha256Hash(string rawData, string salt)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string saltedData = rawData + salt;
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltedData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

    public class LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
