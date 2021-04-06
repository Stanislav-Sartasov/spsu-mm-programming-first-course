using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace FilterServiceLibrary
{
    [ServiceContract]
    public interface IContract
    {
        [OperationContract]
        byte[] ApplyFilter(string filter, byte[] image);

        [OperationContract]
        string[] GetFilters();
        [OperationContract]
        int GetStatus();
    }
}
