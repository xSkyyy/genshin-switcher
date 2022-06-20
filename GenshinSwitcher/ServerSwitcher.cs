using System.Collections.Generic;
using System.Linq;
using GenshinSwitcher.Extensions;
using GenshinSwitcher.Helpers;
using System.Threading.Tasks;

namespace GenshinSwitcher
{
    class ServerSwitcher
    {
        private readonly string serverAddress;

        public ServerSwitcher(string realistikIpAddress)
        {
            this.serverAddress = realistikIpAddress;
        }

        public void SwitchToRealistik()
        {
            List<string> lines = HostsFile.ReadAllLines();
            List<string> result = lines.Where(x => !x.Contains("")).ToList();
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
            HostsFile.WriteAllLines(HostsFile.ReadAllLines().Where(x => !x.Contains("hoyoverse.com") || !x.Contains("yuanshen.com") || !x.Contains("mihoyo.com")));
        }

        public Task<Server> GetCurrentServerAsync()
        {
            return Task.Run<Server>(() => GetCurrentServer());
        }

        public Server GetCurrentServer()
        {
            bool isRealistik = HostsFile.ReadAllLines().Any(x => x.Contains("hoyoverse.com") || x.Contains("yuanshen.com") || x.Contains("mihoyo.com") && !x.Contains("#"));
            return isRealistik ? Server.Realistik : Server.Official;
        }
    }

    public enum Server
    {
        Official,
        Realistik
    }
}
