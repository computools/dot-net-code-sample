namespace *********.**********.BusinessServices.Models
{
    public class WebProxyWrap
    {
        public string Ip { get; }
        public int Port { get; }
        public WebProxyWrap(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }
    }
}
