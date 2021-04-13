using System.ServiceModel;

namespace Filter.Server
{
    public interface IFilterServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ProgressCallback(int progress);

        [OperationContract(IsOneWay = true)]
        void ImageCallback(byte[] img);

    }
}