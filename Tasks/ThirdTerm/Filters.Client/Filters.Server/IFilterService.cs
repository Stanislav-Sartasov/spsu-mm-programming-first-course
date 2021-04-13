using System.ServiceModel;

namespace Filter.Server
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