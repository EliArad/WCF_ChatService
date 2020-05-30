using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatServiceLib
{

    [ServiceContract]
    public interface IDuplexServiceCallback
    {

        [OperationContract(IsOneWay = true)]
        void UserJoin(Client client);

        [OperationContract(IsOneWay = true)]
        void UserLeave(string userName, Guid serverGuid , DateTime time);

        [OperationContract(IsOneWay = true)]
        void RefreshClients(List<Client> clients);

        [OperationContract(IsOneWay = true)]
        void ReceiveBroadcast(Message msg);

        [OperationContract(IsOneWay = true)]
        void Receive(Message msg);

    }
}
