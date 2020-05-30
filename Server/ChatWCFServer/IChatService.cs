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
        void Registration(string fieldGuid);
         
        [OperationContract]
        string GetVersion();

        
    }

}
