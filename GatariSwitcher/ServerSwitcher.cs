using System.Linq;
using KawataSwitcher.Extensions;
using KawataSwitcher.Helpers;
using System.Threading.Tasks;

namespace KawataSwitcher
{
    class ServerSwitcher
    {
        private readonly string serverAddress;

        public ServerSwitcher(string kawataIpAddress)
        {
            this.serverAddress = kawataIpAddress;
        }

        public void SwitchToKawata()
        {
            var lines = HostsFile.ReadAllLines();
            var result = lines.Where(x => !x.Contains("")).ToList();
            result.AddRange
            (
                serverAddress + " hk4e-sdk-os.hoyoverse.com",
                serverAddress + " webstatic-sea.hoyoverse.com",
                serverAddress + " abtest-api-data-sg.hoyoverse.com",
                serverAddress + " log-upload-os.mihoyo.com",
                serverAddress + " osasiadispatch.yuanshen.com",
                serverAddress + " dispatchosglobal.yuanshen.com",
                serverAddress + " overseauspider.yuanshen.com",
                serverAddress + " dispatch-hk4e-global-os-euro.mihoyo.com",
                serverAddress + " sdk-os-static.hoyoverse.com",
                serverAddress + " bruh.hoyoverse.com"
            );
            HostsFile.WriteAllLines(result);
        }

        public void SwitchToOfficial()
        {
            HostsFile.WriteAllLines(HostsFile.ReadAllLines().Where(x => !x.Contains("ppy.sh")));
        }

        public Task<Server> GetCurrentServerAsync()
        {
            return Task.Run<Server>(() => GetCurrentServer());
        }

        public Server GetCurrentServer()
        {
            bool isKawata = HostsFile.ReadAllLines().Any(x => x.Contains("osu.ppy.sh") && !x.Contains("#"));
            return isKawata ? Server.Kawata : Server.Official;
        }
    }

    public enum Server
    {
        Official,
        Kawata
    }
}
