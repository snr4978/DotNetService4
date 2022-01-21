using System.ServiceModel;

namespace Kean.Presentation.Rest.Soaps.Contracts
{
    [ServiceContract]
    public interface MyServiceContract
    {
        [OperationContract]
        void DoWork(MyRequestContract request);
    }
}
