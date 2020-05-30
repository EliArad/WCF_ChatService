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
        bool Say(Message msg);

        [OperationContract(IsOneWay = true)]
        void Echo(Message msg);

        [OperationContract()]
        bool Broadcast(Message msg);

        [OperationContract()]
        bool BroadcastServer(Message msg);

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void Disconnect(Client userName);


    }

}
