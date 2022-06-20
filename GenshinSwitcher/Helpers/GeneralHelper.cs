using System.Net;
using System.Threading.Tasks;

namespace GenshinSwitcher.Helpers
{
    static class GeneralHelper
    {
        public async static Task<string> GetRealistikAddressAsync()
        {
            string result = string.Empty;
            try
            {
                IPAddress[] addresses = await Dns.GetHostAddressesAsync("ussr.pl");

                result = addresses[0].ToString();
            }
            catch { }

            return result.Trim();
        }
    }
}