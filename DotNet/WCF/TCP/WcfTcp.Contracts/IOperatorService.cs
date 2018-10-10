using System.ServiceModel;

namespace WcfTcp.Contracts
{
    [ServiceContract]
    public interface IOperatorService : IService
    {
        [OperationContract]
        ResponseDto SignIn(string userName, string password);
    }
}

