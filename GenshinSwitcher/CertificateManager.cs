using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GenshinSwitcher
{
    class CertificateManager
    {
        public Task<bool> GetStatusAsync()
        {
            return Task.Run<bool>(() => GetStatus());
        }

        public void Install()
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);

            X509Certificate2 certificate = new X509Certificate2(Properties.Resources.cert);
            X509Certificate2 certificate2 = new X509Certificate2(Properties.Resources.cert2);

            store.Add(certificate);
            store.Add(certificate2);

            store.Close();
        }

        public void Uninstall()
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadWrite);
                
                foreach (X509Certificate2 c in store.Certificates.Find(X509FindType.FindBySubjectName, "*.hoyoverse.com", true))
                    store.Remove(c);

                foreach (X509Certificate2 c in store.Certificates.Find(X509FindType.FindBySubjectName, "*.yuanshen.com", true))
                    store.Remove(c);
            }
            finally
            {
                store.Close();
            }
        }

        public bool GetStatus()
        {
            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection c = store.Certificates.Find(X509FindType.FindBySubjectName, "*.hoyoverse.com", true);
            bool result = c.Count > 0;

            store.Close();

            return result;
        }
    }
}
