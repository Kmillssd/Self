using WcfTcp.Contracts;

namespace WcfTcp.Client.Services
{
    public class OperatorClientService : ClientService<IOperatorService>
    {
        public OperatorClientService() 
            : base("Operators")
        { }

        public ResponseDto SignIn(string userName, string password)
        {
            return this.Proxy.SignIn(userName, password);
        }
    }
}
