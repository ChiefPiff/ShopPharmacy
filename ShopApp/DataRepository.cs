using System.Linq;

namespace ShopApp
{
    public class DataRepository
    {
        public static bool AdminEnter(string login, string password)
        {
            var adminLoginCheck = App.Context.Users.FirstOrDefault(x => x.UserLogin == login);
            if (adminLoginCheck != null)
            {
                return password == adminLoginCheck.UserPassword;
            }
            return false;
        }

        public static bool SellerEnter(string login, string password)
        {
            var sellerLoginCheck = App.Context.Users.FirstOrDefault(x => x.UserLogin == login);
            if (sellerLoginCheck != null)
            {
                return password == sellerLoginCheck.UserPassword;
            }
            return false;
        }
    }
}
