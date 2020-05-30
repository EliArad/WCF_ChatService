using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatServiceLib
{

    [ServiceContract(SessionMode = SessionMode.Required,
                      CallbackContract = typeof(IDuplexServiceCallback))]
    public interface IChatService
    {
         
        [OperationContract]
        string GetVersion();

        [OperationContract]
        bool Connect(string userName, string freedesc, Guid serverGuid, DateTime time);

        [OperationContract()]
        bool SendMessage(string fromUserName, Guid fromServerGuid, string toUserName, Guid toServerGuid, string message);
          
        [OperationContract()]
        bool Broadcast(string fromUserName, Guid fromServerGuid, string message);

        [OperationContract()]
        bool BroadcastServer(string fromUserName, Guid fromServerGuid, Guid toServerGuid, string message);

        [OperationContract(IsOneWay = true)]
        void Disconnect(string userName, Guid ServerGuid, bool notify);
         
    }

}
