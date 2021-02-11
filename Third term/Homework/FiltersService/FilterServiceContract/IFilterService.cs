using System.ServiceModel;

namespace FilterServiceContract
{
    [ServiceContract(CallbackContract = typeof(IFilterServiceCallback))]
    public interface IFilterService
    {
        [OperationContract]
        string[] GetFilters();

        [OperationContract(IsOneWay = true)]
        void ApplyFilter(byte[] img, string filter);

        [OperationContract(IsOneWay = true)]
        void StopFiltering();
    }    
}
