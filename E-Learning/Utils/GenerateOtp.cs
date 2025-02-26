using System.Text;

namespace E_Learning.Utils
{
    public class GenerateOtp
    {
        private GenerateOtp() { }

        public static string Generate()
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= 5; i++)
            {
                sb.Append(random.Next(0, 10));
            }
            return sb.ToString();
        }
    }
}
