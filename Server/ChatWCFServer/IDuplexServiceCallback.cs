using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatServiceLib
{

  [ServiceContract]
  public interface IDuplexServiceCallback {

    [OperationContract(IsOneWay = true)]
      void NotifyCallbackMessage(string fieldGuid, string ipAddress, int portNumber, int code, string Msg , DateTime date);

    [OperationContract(IsOneWay = true)]
    void NotifyDataCallback(string fieldGuid, string ipAddress, int portNumber, int code, byte[] buf, int size, DateTime date);
  }
}
