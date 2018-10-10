using System.ServiceModel;

namespace WcfTcp.Contracts
{
    [ServiceContract]
    public interface IOperatorService : IService
    {
        [OperationContract]
        string SignIn(string userName, string password);
    }
}

