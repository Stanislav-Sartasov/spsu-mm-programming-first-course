using System.ServiceModel;

namespace FilterServiceContract
{
    public interface IFilterServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ProgressCallback(int progress);

        [OperationContract(IsOneWay = true)]
        void ImageCallback(byte[] img);

    }
}
