using System.Security.Cryptography.X509Certificates;

namespace Sessions
{
    public interface ISessionHandler
    {
        string Name { get; }

        string SaveSession(bool closeApp);

        void RestoreSession(string data);
    }
}
