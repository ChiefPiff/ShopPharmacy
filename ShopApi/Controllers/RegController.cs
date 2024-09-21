using Microsoft.AspNetCore.Mvc;
using ShopApi.Model;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegController : Controller
    {
        private readonly ShopbdContext _context;

        public RegController(ShopbdContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Регистрирует нового продавца.
        /// </summary>
        /// <param name="model">Модель регистрации.</param>
        /// <returns>Статус регистрации.</returns>
        /// <response code="200">Продавец успешно зарегистрирован.</response>
        /// <response code="400">Если введены некорректные данные или пользователь уже существует.</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult RegisterSeller([FromBody] RegisterSellerModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Пароли не совпадают.");
            }

            if (_context.Users.Any(u => u.UserLogin == model.Login))
            {
                return BadRequest("Пользователь с таким логином уже существует.");
            }

            string salt = GenerateSalt();
            string hashedPassword = ComputeSha256Hash(model.Password, salt);

            var seller = new User
            {
                UserLogin = model.Login,
                UserPassword = hashedPassword,
                UserSalt = salt
            };

            _context.Users.Add(seller);
            _context.SaveChanges();

            return Ok("Продавец успешно зарегистрирован.");
        }

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
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

    public class RegisterSellerModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
