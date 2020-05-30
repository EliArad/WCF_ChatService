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
        void UserJoin(string userName , Guid serverGuid, bool newUser);

        [OperationContract(IsOneWay = true)]
        void UserLeave(string userName, Guid serverGuid , DateTime time);

        [OperationContract(IsOneWay = true)]
        void ClientsList(List<Tuple<string, Guid>> clients);

        [OperationContract(IsOneWay = true)]
        void ReceiveBroadcast(string toUserName, Guid toServerGuid, string fromUserName, Guid fromServerGuid, string message, bool broadcast, DateTime date);

        [OperationContract(IsOneWay = true)]
        void NotifyMessage(string fromUserName, Guid fromServerName, string toUserName, Guid toServerName, string message, DateTime date);

        [OperationContract(IsOneWay = true)]
        void NotifyMessageSent(DateTime date, bool sent);

    }
}
